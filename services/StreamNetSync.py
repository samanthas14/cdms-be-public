# Syncs a local database table with the StreamNet database using the StreamNet REST Web API.
#
# Local data is replicated to the StreamNet server.  This means that any records that exist locally but not remotely,
# or have been modified locally since the last sync, will be uploaded to StreamNet.  Any records that exist remotely
# but not locally are deleted from the StreamNet server.
#
# If the sync process is successful, the program will exit silently.
#
# If there is an error during the sync process, an exception will be raised and the program will terminate.  Data
# already synced by that point will stay synced (i.e. there is no rollback mechanism), but I think this is OK because
# there is no way to put the server into a "broken" state as the result of a partial sync.
#
# This code has not been tested in scenarios outside those required to sync CCT data with StreamNet.

# Requires Python 3

import datetime
import os.path
import pickle
import http.client
import json
import pypyodbc   # pip install pypyodbc
import sys


# For connecting to StreamNet:

########################################
# CCT
# api_key = "A5AF6133-6E0D-4BEF-AC56-3E54606B9990"  # Your api key, provided by StreamNet
# agency = "CCT"                                    # Your agency, canonical name provided by StreamNet

# This is the table that we're interested in replicating upstream -- should be one defined in table_ids below

# For connecting to your local database
# database_name = "Habitat"
# db_connect_string = 'DRIVER={SQL Server};Server=(local);Database=' + database_name + ';Trusted_Connection=True;'
# remapped_columns = { }
# id_column = 'id'
# last_modified_date_column = 'LastModifiedOn'

# tables_to_sync = {'NOSA' : 'CoordinatedAssessments'}     # Dataset : local database table name

########################################

# CTUIR
api_key = "AAA89E60-47FC-43E9-ABCE-67FBD5D8162E"    # Your api key, provided by StreamNet
agency = "CTUIR"                                    # Your agency, canonical name provided by StreamNet

# The tables we're interested in replicating upstream -- should be one defined in table_ids below
tables_to_sync = dict(NOSA  = 'streamnet_nosa_detail_vw',
                      SAR   = 'streamnet_sar_detail_vw',
                      RperS = 'streamnet_rpers_detail_vw')  # Dataset : local database table name


database_name = "CDMS_PROD"
db_connect_string = 'DRIVER={SQL Server};Server=(local);Database=' + database_name + ';Trusted_Connection=True;'

remapped_columns = {'id':'shadowId'}
id_column = 'shadowId'
last_modified_date_column = 'EffDt'
########################################


# Stuff you probably won't want to change
base_url = "api.streamnet.org"
base_url_params = "/api/v1/ca"
last_sync_date_file = "lastsyncdate.pickle"

table_ids = dict(RperS = "3613A357-258C-4615-927F-EB368C61E13E", NOSA        = "4EF09E86-2AA8-4C98-A983-A272C2C2C7E3",
                 SAR   = "AF05E51B-6123-44A4-AEC5-FB11C006713E", Populations = "F37C744F-E1FC-46D7-A8DB-0AB6337A4024")


# These headers should be included with every request to StreamNet
streamnet_standard_headers = { "XApiKey":api_key, "Accept":"application/json" }


# Establish a database connection that we'll use for the life of this script
dbconn = pypyodbc.connect(db_connect_string)


def static_var(varname, value):
    """
    Create decorator for nicer use of method-static variables.
    """
    def decorate(func):
        setattr(func, varname, value)
        return func
    return decorate


def status_ok(response):
    """
    Return true if the response looks valid.  StreamNet sends a 204 if a delete was successful.
    """
    return response.status == 200 or response.status == 204


def time_since_1970(date):
    """
    Return the number of seconds elapsed since 1-Jan-1970.  StreamNet doesn't like fractional seconds.
    """
    return int((date - datetime.datetime(1970, 1, 1)).total_seconds())


def add_agency(agency=agency):
    """
    Add an agency parameter; if none is specified, will default to the global value.
    If None is passed, no agency parameter will be added.
    """
    if agency is None:
        return ""

    return "&agency=" + agency


def add_current_time():
    return "?time=" + str(time_since_1970(datetime.datetime.now()))


def add_table_id(table):
    table_id = table_ids[table]

    if table_id is None:
        raise ValueError("Unknown table: " + table)

    return "&table_id=" + table_id


def add_updated_since(date):
    """
    Adds updated_since.  If None is passed, omit the parameter.
    """
    if date is None or time_since_1970(date) == 0.0:
        return ""

    return "&updated_since=" + str(time_since_1970(date))


# noinspection PyBroadException
def get_last_sync_date():
    """
    Return the date we last synced.
    """
    if os.path.isfile(last_sync_date_file):
        try:
            return pickle.load(open(last_sync_date_file, "rb"))
        except:
            pass        # Fall through and return the default

    # No pickle file?  Return a thematic default.
    return datetime.datetime(1970, 1, 1)


def save_last_sync_date():
    """
    Save the date we last synced.  Uses Python's pickle object, because it has such a cool name!
    """
    pickle.dump(datetime.datetime.now(), open(last_sync_date_file, "wb"))


def retrieve_all_records(table):
    """
    Get all our records on StreamNet, regardless of modification date.
    """
    return retrieve_records_modified_since(table, None)


def retrieve_records_modified_since(table, date):
    """
    Get a list of all records on StreamNet that have been modified since the specified date.
    Pass None for all dates.
    """
    url_params = base_url_params
    url_params += add_current_time()
    url_params += add_updated_since(date)
    url_params += add_table_id(table)
    url_params += add_agency()

    conn = http.client.HTTPSConnection(base_url)
    conn.request("GET", url_params, None, streamnet_standard_headers)

    response = conn.getresponse()

    if status_ok(response):
        return json.loads(response.read().decode('utf-8'))['records']

    # Error handling...
    raise RuntimeError("Unexpected error (" + str(response.code) + ") requesting remote records.")


def retrieve_record_by_id(id):
    """
    Retrieve a record from StreamNet, if any, that has the specified id
    """
    url_params = base_url_params + "/" + id
    url_params += add_current_time()

    conn = http.client.HTTPSConnection(base_url)
    conn.request("GET", url_params, None, streamnet_standard_headers)
    response = conn.getresponse()

    if status_ok(response):
        return response.read()

    # Error handling...
    raise RuntimeError("Unexpected error (" + str(response.code) + ") requesting remote record '" + id + "'.")


def delete_remote_records(ids):
    """
    Delete remote records with the specified ids.
    """
    for id in ids:
        url_params = base_url_params + "/" + id
        url_params += add_current_time()

        conn = http.client.HTTPSConnection(base_url)
        conn.request("DELETE", url_params, None, streamnet_standard_headers)
        response = conn.getresponse()

        if status_ok(response):
            return

        # Error handling...
        raise RuntimeError("Unexpected error (" + str(response.code) + ") deleting remote record '" + id + "'.")


def retrieve_table_list():
    """
    Get a list of all tables that StreamNet knows about.  Not currently used, needs some love.
    """
    url_params = base_url_params + "/tables"

    conn = http.client.HTTPSConnection(base_url)

    url_params += add_current_time()

    conn.request("GET", url_params, None, streamnet_standard_headers)
    response = conn.getresponse()

    if not status_ok(response):
        return None

    json = response.read()

    print(json)


def get_all_local_ids(db_table):
    """
    Return a list of all ids in our local database.
    """
    return get_local_ids_modified_since(db_table, None)


def get_local_ids_modified_since(db_table, date):
    """
    Return a list of IDs of local records that need to be pushed to the server.
    """
    cur = dbconn.cursor()
    sql = "SELECT " + id_column + " FROM " + db_table
    if date is not None:
        sql += " WHERE cast(" + last_modified_date_column + " as datetime2) >= cast('" + str(date) + "' as datetime2)"
    cur.execute(sql)

    return [str(x[0]) for x in cur.fetchall()]      # De-tuplize results


def upload_records_to_server(ids, source_table, target_table, exists_on_server):
    """
    Upload a list of records to the server; ids are the list of records to upload, and exists_on_server is a bool
    that indicates whether we expect the record to already exist on the server.
    """
    for id in ids:
        url_params = base_url_params

        if exists_on_server:
            method = "PUT"          # Update existing record
            url_params += "/" + id  # Need to provide id here for post request
        else:
            method = "POST"         # Upload new record

        headers = streamnet_standard_headers
        headers["Content-type"] = "application/json"

        post_params = {}
        post_params['table_id'] = table_ids[target_table]               # To which table we are uploading?
        post_params['record_values'] = get_record_from_database(source_table, id)     # The data to upload

        conn = http.client.HTTPSConnection(base_url)
        body = json.dumps(post_params)

        conn.request(method, url_params, body, headers)
        response = conn.getresponse()

        if status_ok(response):
            return


        # Error handling...

        response_body = response.read().decode('utf-8')

        if response.status == 422:                          # Status 422 indicates failed validation
            error = json.loads(response_body)['error']      # Response should be json

            err = (error['msg'] + " --> " +
                   json.dumps(error['params'], sort_keys=True, indent=4, ) +
                   json.dumps(error['record_values'], sort_keys=True, indent=4, separators=(',', ' --> ')) )

            raise RuntimeError("Record with database id " + id + " failed validation while uploading to server: " + err + "\n" +
                              "(" + json.dumps(post_params['record_values']) + ")")

        elif response.status == 401:      # Bad API key?
            raise RuntimeError("Server responded with 'Unauthorized' error (code 401) -- is your API key correct?")
        elif response.status == 503:
            raise RuntimeError("Got error 503 - perhaps the StreamNet API is down?  Try again later.")
        else:
            raise RuntimeError("Unexpected error (" + str(response.status) + ") while uploading data for record '" +
                               id + "': " + response.reason + " --> " + response_body)


def get_record_from_database(db_table, id):
    """
    Given a record id, generate an dictionary of values we can send to the server.

    The data is just going to be a straight column-by-column dump of the local database schema; therefore,
    for things to work properly, the local table should exactly match the StreamNet version.  This is the most likely
    scenario, and seems like a logical requirement.  Plus it saves a ton of typing!
    """
    data = {}

    col_names = get_db_column_names(db_table)

    db_row = get_data_for_id(db_table, col_names, id)

    for i in range(0, len(col_names)):
        col = col_names[i]
        val = db_row[i]

        # Make sure we stringify any ints coming from the database, but be careful not to pass None through str()!  That makes StreamNet very angry!
        if(val != None):
            data[col] = str(val)
        else:
            data[col] = None

    return data


def get_data_for_id(table, col_names, id):
    """
    Retrieve a row from the database for the specified id, and return the data.  Will raise an exception
    if there is not exactly one row for that id.
    """

    cols = []

    # Remap column names to account for any entries in remapped_columns
    for index, col_name in enumerate(col_names):
        for key in remapped_columns:
            if col_name.lower() == key.lower():
                cols.append(remapped_columns[key] + ' as ' + col_name)
            else:
                cols.append(col_name)

    sql = "SELECT " + ",".join(cols) + " FROM " + table + " WHERE " + id_column + " = '" + id + "'"


    cur = dbconn.cursor()
    cur.execute(sql)

    data = cur.fetchall()

    if len(data) != 1:
        raise ValueError("Expected one record for ID '" + id + "' in local database, got " + str(len(data)))

    return data[0]


@static_var("columns", {})
def get_db_column_names(table):
    """
    Get a list of column names for the specified table.  This method can be called repeatedly because it caches
    the results.
    """
    if table in get_db_column_names.columns:
        return get_db_column_names.columns[table]

    sql = "SELECT column_name FROM information_schema.columns WHERE table_name = '" + table + "'"
    cur = dbconn.cursor()
    cur.execute(sql)

    get_db_column_names.columns[table] = [x[0] for x in cur.fetchall()]    # De-tuplize results

    return get_db_column_names.columns[table]


def extract_ids(records):
    """
    Given a list of records from a remote server, return a list of ids contained therein.
    """
    ids = []
    for record in records:
        ids.append(record['id'])

    return ids


def main():
    dry_run = False

    date = get_last_sync_date()

    for dataset in tables_to_sync:

        print("Processing " + dataset)

        local_table_name = tables_to_sync[dataset]

        remote_ids = extract_ids(retrieve_all_records(dataset))
        local_ids = get_all_local_ids(local_table_name)
        updated_ids = get_local_ids_modified_since(local_table_name, date)

        records_only_local = [id for id in local_ids   if id not in remote_ids]          # Post these
        records_to_update  = [id for id in updated_ids if id not in records_only_local]  # Put these
        records_to_delete  = [id for id in remote_ids  if id not in local_ids]           # Delete these


        if not dry_run:
            # These methods will all raise exceptions if something goes wrong
            upload_records_to_server(records_only_local, local_table_name, dataset, False)
            upload_records_to_server(records_to_update,  local_table_name, dataset, True)
            delete_remote_records(records_to_delete)

            # And now, if everything went well...
            save_last_sync_date()

        else:
            print("remote_ids: ",  ','.join(remote_ids))
            print("local_ids: ",   ','.join(local_ids))
            print("updated_ids: ", ','.join(updated_ids), "(since " + str(date) + ")")
            print("Send: ",        ','.join(records_only_local))
            print("Update: ",      ','.join(records_to_update))
            print("Delete: ",      ','.join(records_to_delete))

    


try:
    main()      # Just Do It!!
except Exception as e:
    print("Error syncing to StreamNet:")
    print(str(e))
else:
    print("Syncing to StreamNet successful!")
