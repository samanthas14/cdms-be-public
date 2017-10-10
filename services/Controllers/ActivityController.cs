using Newtonsoft.Json.Linq;
using services.Models;
using services.Models.Data;
using services.Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace services.Controllers
{
    public class ActivityController : CDMSController
    {
        [HttpPost]
        public HttpResponseMessage SetQaStatus(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            User me = AuthorizationManager.getCurrentUser();
            Activity activity = db.Activities.Find(json.ActivityId.ToObject<int>());

            if (activity == null || me == null)
                throw new Exception("SetQAStatus: Configuration error. Please try again.");

            logger.Debug("Userid = " + me.Id + " Activity = " + activity.Id);

            //TODO: verify that the user is authorized to modify this activity?  -- yes, that would be nice. (or add Authorized annotation) TODO!


            var aq = new ActivityQA();
            aq.ActivityId = activity.Id;
            aq.EffDt = DateTime.Now;
            aq.Comments = json.Comments;
            aq.UserId = me.Id; //current user.
            aq.QAStatusId = json.QAStatusId;

            db.ActivityQAs.Add(aq);

            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);


        }

        [HttpPost]
        public HttpResponseMessage DeleteDatasetActivities(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            Dataset dataset = db.Datasets.Find(json.DatasetId.ToObject<int>());

            Project project = db.Projects.Find(dataset.ProjectId);

            if (project == null)
                throw new Exception("DeleteDatasetActivities: Configuration error.");

            User me = AuthorizationManager.getCurrentUser();
            if (!project.isOwnerOrEditor(me))
                throw new Exception("DeleteDatasetActivities: Configuration error.");

            var Activities = new List<string>();

            foreach (var item in json.Activities)
            {
                //make sure we can cast as an int otherwise it will throw an exception.
                int test_int = item.Id.ToObject<int>();
                Activities.Add("" + test_int);
            }

            var ActivityIds = string.Join(",", Activities);
            var DataTable = dataset.Datastore.TablePrefix;

            //open a raw database connection...
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "DELETE FROM " + DataTable + "_Detail WHERE ActivityId in (" + ActivityIds + ")";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    logger.Debug(query);
                    cmd.ExecuteNonQuery();
                }

                query = "DELETE FROM " + DataTable + "_Header WHERE ActivityId in (" + ActivityIds + ")";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    logger.Debug(query);
                    cmd.ExecuteNonQuery();
                }

                query = "DELETE FROM ActivityQAs WHERE ActivityId in (" + ActivityIds + ")";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    logger.Debug(query);
                    cmd.ExecuteNonQuery();
                }

                query = "DELETE FROM Activities WHERE DatasetId = " + dataset.Id + " AND Id in (" + ActivityIds + ")";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    logger.Debug(query);
                    cmd.ExecuteNonQuery();
                }
            }


            return new HttpResponseMessage(HttpStatusCode.OK);

        }




        /**
         * Updates activities for a dataset.
         * json with: DatasetId, ProjectId, activities
         * 
         */
        [HttpPost]
        public HttpResponseMessage UpdateDatasetActivities(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;
            //logger.Debug("json = " + json);

            User me = AuthorizationManager.getCurrentUser();

            Dataset dataset = db.Datasets.Find(json.DatasetId.ToObject<int>());
            if (dataset == null)
                throw new Exception("Configuration Error.");

            Project project = db.Projects.Find(dataset.ProjectId);
            if (project == null)
                throw new Exception("Configuration Error");

            if (!project.isOwnerOrEditor(me))
                throw new Exception("Authorization error.");

            //setup our generic data stuff
            var data_header_name = dataset.Datastore.TablePrefix + "_Header";
            var data_detail_name = dataset.Datastore.TablePrefix + "_Detail";
            var dbset_header = db.GetDbSet(data_header_name);
            var dbset_detail = db.GetDbSet(data_detail_name);
            var dbset_header_type = db.GetTypeFor(data_header_name);
            var dbset_detail_type = db.GetTypeFor(data_detail_name);

            //get a list of the fields that are GRID types (relations)
            var grid_fields = dataset.Fields.Where(o => o.ControlType == "grid");

            foreach (var item in json.activities)
            {

                if (item is JProperty)
                {
                    var prop = item as JProperty;
                    dynamic activity_json = prop.Value;

                    logger.Debug("Updating activity id: " + json.ActivityId);

                    Activity activity = db.Activities.Find(json.ActivityId.ToObject<int>());
                    if (activity == null)
                    {
                        throw new Exception("Invalid Activity.");
                    }

                    activity.LocationId = activity_json.LocationId;
                    try
                    {
                        activity.ActivityDate = activity_json.ActivityDate;
                    }
                    catch (Exception e)
                    {
                        logger.Debug("Ooops had an error converting date: " + activity_json.ActivityDate);
                        logger.Debug(e.ToString());
                        throw (e);
                    }

                    //activity.DatasetId = json.DatasetId;
                    activity.UserId = me.Id;
                    activity.SourceId = 1;                                          // TODO get from data
                    activity.ActivityTypeId = 1;
                    activity.InstrumentId = activity_json.InstrumentId;
                    activity.AccuracyCheckId = activity_json.AccuracyCheckId;
                    activity.PostAccuracyCheckId = activity_json.PostAccuracyCheckId;
                    activity.Timezone = activity_json.Timezone;
                    //activity.LaboratoryId = activity_json.LaboratoryId;

                    //activity.CreateDate = DateTime.Now;

                    db.Entry(activity).State = EntityState.Modified;
                    db.SaveChanges();

                    //updated the activity

                    logger.Debug("Updated an activity: ");
                    logger.Debug(" LocationID = " + activity_json.LocationId);
                    logger.Debug(" ActivityDate = " + activity_json.ActivityDate);
                    logger.Debug("  ID = " + activity.Id);

                    //now check our activity status -- update it if we've changed.
                    //if(activity.ActivityQAStatus.ActivityId != )

                    dynamic activityqastatus = activity_json.ActivityQAStatus;

                    //logger.Debug(activity_json.ActivityQAStatus);

                    //logger.Debug(activityqastatus.QAStatusId.ToObject<int>());

                    ActivityQA newQA = new ActivityQA();
                    newQA.ActivityId = activity.Id;
                    newQA.QAStatusId = activityqastatus.QAStatusID.ToObject<int>();
                    newQA.Comments = activityqastatus.Comments;
                    newQA.EffDt = DateTime.Now;
                    newQA.UserId = activity.UserId;
                    newQA.QAStatusName = activityqastatus.Name;
                    newQA.QAStatusDescription = activityqastatus.Description;

                    db.ActivityQAs.Add(newQA);
                    db.SaveChanges();


                    //get our last header and then check against incoming header field values to see if anything has changed.
                    var last_header_list = dbset_header.SqlQuery("SELECT * FROM " + data_header_name + " WHERE ActivityId = " + activity.Id + " ORDER BY EffDt DESC");
                    //.SqlQuery("SELECT * FROM " + data_header_name + " WHERE ActivityId = " + activity.Id + " ORDER BY EffDt DESC").AsQueryable().f; 
                    //db.AdultWeir_Header.Where(o => o.ActivityId == activity.Id).OrderByDescending(d => d.EffDt).FirstOrDefault();

                    var last_header = LookupFieldHelper.getFirstItem(last_header_list);

                    logger.Debug("Ok -- here we are with our lastheader:");
                    logger.Debug(last_header);


                    if (last_header == null)
                        throw new Exception("Somehow there is no previous header even though we are trying to update.");

                    bool header_updated = false;

                    //spin through and check the header fields for changes...
                    foreach (JProperty header_field in activity_json.Header)
                    {
                        logger.Debug("Checking last header value of field : '" + header_field.Name + "' with incoming value + '" + header_field.Value + "'");

                        var objval = last_header.GetType().GetProperty(header_field.Name).GetValue(last_header, null);
                        logger.Debug("Got the value from the header field.");

                        if (objval != null)
                        {
                            logger.Debug("objval not null. header_field.Name: " + header_field.Name + ", value: " + objval + ", with incoming value: " + header_field.Value);

                            if (objval.ToString() != header_field.Value.ToString())
                            {
                                logger.Debug("a different value! we'll save a header then...");
                                header_updated = true;
                                break;
                            }
                        }
                        else
                        {
                            if (header_field.Value.ToString() != "")
                            {
                                logger.Debug("Dunno why, but objval was null." + header_field + " we are going to save a new one.");
                                header_updated = true;
                                break;
                            }
                            else
                                logger.Debug("objval is empty ''");
                        }
                    }

                    if (header_updated)
                    {
                        logger.Debug("Saving a new header then");
                        var header = activity_json.Header.ToObject(dbset_header_type);

                        //now do the saving! -- this works the exact same way for update as it does for new
                        header.ActivityId = activity.Id;
                        header.ByUserId = activity.UserId;
                        header.EffDt = DateTime.Now;
                        dbset_header.Add(header);
                        db.SaveChanges();
                    }

                    //there are three possible cases of changes:
                    //  1) updated row (has ID and is in "updatedRowIds" list)
                    //  2) new row (has no ID)
                    //  3) deleted row (is not in the list, ID is in "deletedRowIds" list)

                    //we ALWAYS make such indication by INSERTING a new row with a matching rowid + activityid + new current effective date.
                    //  exception is NEW row which gets and incremented rowid

                    //first, lets lookup our last row id so we have a place to start if we add rows.                    
                    int rowid = 1;

                    var last_row_list = dbset_detail.SqlQuery("SELECT * FROM " + data_detail_name + " WHERE ActivityId = " + activity.Id + " AND RowStatusId = " + DataDetail.ROWSTATUS_ACTIVE + " ORDER BY RowId DESC");
                    //db.AdultWeir_Detail.Where(o => o.ActivityId == activity.Id).Where(o => o.RowStatusId == DataDetail.ROWSTATUS_ACTIVE).OrderByDescending(d => d.RowId).FirstOrDefault();

                    var last_row = LookupFieldHelper.getFirstItem(last_row_list);
                    if (last_row != null)
                    {
                        rowid = last_row.RowId + 1;
                    }
                    else
                        logger.Debug("Hmm there were no previous details rows for activity : " + activity.Id + " so we are starting at 1.");

                    //now lets iterate our incoming rows and see what we've got.
                    var details = new List<DataDetail>();
                    Dictionary<string, JArray> grids = new Dictionary<string, JArray>();

                    List<int> updated_rows = new List<int>();
                    foreach (var updated_row in json.updatedRowIds)
                    {
                        logger.Debug("Found an updated row: " + updated_row);
                        updated_rows.Add(updated_row.ToObject<int>());
                    }

                    List<int> deleted_rows = new List<int>();
                    foreach (var deleted_row in json.deletedRowIds)
                    {
                        logger.Debug("Found a deleted row: " + deleted_row);
                        deleted_rows.Add(deleted_row.ToObject<int>());
                        if (updated_rows.Contains(deleted_row.ToObject<int>()))
                            updated_rows.Remove(deleted_row.ToObject<int>());
                    }


                    foreach (var detailitem in activity_json.Details)
                    {
                        var adw = detailitem.ToObject(dbset_detail_type);

                        //does this field have a relation/grid field?  If so then save those, too.
                        if (grid_fields != null)
                        {
                            foreach (var grid_field in grid_fields)
                            {
                                logger.Debug("Found a grid field: " + grid_field.DbColumnName);
                                grids.Add(grid_field.DbColumnName, detailitem[grid_field.DbColumnName]);
                            }
                        }

                        //logger.Debug("spinning through incoming details: " + adw.Id);

                        if (adw.Id == 0)
                        {
                            //new record
                            adw.RowId = rowid; rowid++;
                            details.Add(adw);
                        }
                        else
                        {
                            //deleted or updated?
                            if (updated_rows.Contains(adw.Id))
                            {
                                //updated
                                adw.Id = 0;
                                details.Add(adw);
                            }
                            else if (deleted_rows.Contains(adw.Id))
                            {
                                //deleted
                                adw.Id = 0;
                                adw.RowStatusId = DataDetail.ROWSTATUS_DELETED;
                                details.Add(adw);
                            }
                            //otherwise nothing.
                        }

                    }

                    //logger.Debug(JsonConvert.SerializeObject(grids, Formatting.Indented));
                    //logger.Debug(JsonConvert.SerializeObject(details, Formatting.Indented));

                    foreach (var detail in details)
                    {
                        detail.ActivityId = activity.Id;
                        detail.ByUserId = activity.UserId;
                        detail.EffDt = DateTime.Now;

                        dbset_detail.Add(detail);

                        //any relation grids?
                        if (grids.Count > 0)
                        {
                            logger.Debug("We have grids in our data to update...");
                            foreach (KeyValuePair<string, JArray> grid_item in grids)
                            {
                                int grid_rowid = 1; //new grid field

                                var grid_type = dataset.Datastore.TablePrefix + "_" + grid_item.Key;
                                logger.Debug(" Hey we have a relation of type: " + grid_type);

                                //get objecttype of this type
                                var dbset_grid_type = db.GetTypeFor(grid_type);
                                var dbset_relation = db.GetDbSet(grid_type);

                                //logger.Debug("updating items in : " + grid_item.Key );

                                //get the count of relationrows:
                                grid_rowid = grid_item.Value.Count() + 1;

                                foreach (dynamic relation_row in grid_item.Value)
                                {
                                    //logger.Debug("Relationrow: " + relation_row);
                                    var relationObj = relation_row.ToObject(dbset_grid_type);

                                    //is it a new row?
                                    if (relationObj.Id == 0)
                                    {
                                        relationObj.EffDt = DateTime.Now;
                                        relationObj.ParentRowId = detail.RowId;
                                        relationObj.RowId = grid_rowid; grid_rowid++;
                                        relationObj.RowStatusId = DataDetail.ROWSTATUS_ACTIVE;
                                        relationObj.ByUserId = activity.UserId;
                                        relationObj.ActivityId = activity.Id;
                                        relationObj.QAStatusId = dataset.DefaultRowQAStatusId; //TODO?
                                        dbset_relation.Add(relationObj);
                                    }
                                    else //or are we updating...  (what about DELETE?)
                                    {
                                        relationObj.EffDt = DateTime.Now;
                                        relationObj.Id = 0;
                                        dbset_relation.Add(relationObj);
                                        logger.Debug("woot updated a grid row!");
                                    }

                                    //logger.Debug(JsonConvert.SerializeObject( relationObj, Formatting.Indented));

                                }
                            }
                        }

                    }

                    db.SaveChanges();

                    //If there is a ReadingDateTime field in use, set the activity description to be the range of reading dates for this activity.
                    if (dataset.Datastore.TablePrefix == "WaterTemp") // others with readingdatetime?
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
                        {
                            con.Open();
                            var query = "update Activities set Description = (select concat(convert(varchar,min(ReadingDateTime),111), ' - ', convert(varchar,max(ReadingDateTime),111)) from " + dataset.Datastore.TablePrefix + "_Detail_VW where ActivityId = " + activity.Id + ") where Id = " + activity.Id;

                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                logger.Debug(query);
                                cmd.ExecuteNonQuery();
                            }

                        }
                    }
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        

        public HttpResponseMessage SaveDatasetActivities(JObject jsonData)
        {
            return SaveDatasetActivitiesEFF(jsonData);
        }

        [HttpPost]
        private HttpResponseMessage SaveDatasetActivitiesEFF(JObject jsonData)
        {
            logger.Debug("Saving dataset activities: ");
            var db = ServicesContext.RestartCurrent;
            User me = AuthorizationManager.getCurrentUser();

            dynamic json = jsonData;
            logger.Debug("json = " + json);

            //COPY PASTE -- TODO -- reduce code smell!
            Dataset dataset = db.Datasets.Find(json.DatasetId.ToObject<int>());
            if (dataset == null)
                throw new Exception("Configuration Error:  Could not find the DatasetID in the database.");

            Project project = db.Projects.Find(dataset.ProjectId);
            if (project == null)
                throw new Exception("Configuration Error:  Could not find the ProjectId in the database.");

            if (!project.isOwnerOrEditor(me))
                throw new Exception("Authorization Error:  The user attempting to make changes is not an Owner or Editor.");

            //setup our generic data stuff
            var data_header_name = dataset.Datastore.TablePrefix + "_Header";
            var data_detail_name = dataset.Datastore.TablePrefix + "_Detail";
            var dbset_header = db.GetDbSet(data_header_name);
            var dbset_detail = db.GetDbSet(data_detail_name);
            var dbset_header_type = db.GetTypeFor(data_header_name);
            var dbset_detail_type = db.GetTypeFor(data_detail_name);

            DateTime now;

            //var duplicateActivities = new List<Activity>();

            //get a list of the fields that are GRID types (relations)
            var grid_fields = dataset.Fields.Where(o => o.ControlType == "grid");

            var new_records = new List<Activity>();

            db.Configuration.AutoDetectChangesEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;

            foreach (var item in json.activities)
            {
                int newActivityId = 0;

                if (!(item is JProperty))
                {
                    throw new Exception("There is a problem with your request. Format error.");
                }

                var prop = item as JProperty;
                dynamic activity_json = prop.Value;

                Activity activity = new Activity();
                activity.LocationId = activity_json.LocationId;

                try
                {
                    activity.ActivityDate = activity_json.ActivityDate;
                }
                catch (Exception e)
                {
                    logger.Debug("Ooops had an error converting activity date: " + activity_json.ActivityDate);
                    logger.Debug(e.ToString());

                    throw e;
                }

                try
                {
                    activity.DatasetId = json.DatasetId;
                    activity.UserId = me.Id;
                    activity.SourceId = 1;                                                                  // TODO 
                    activity.ActivityTypeId = 1;
                    activity.CreateDate = DateTime.Now;
                    activity.InstrumentId = activity_json.InstrumentId;
                    //activity.LaboratoryId = activity_json.LaboratoryId;
                    activity.AccuracyCheckId = activity_json.AccuracyCheckId;
                    activity.PostAccuracyCheckId = activity_json.PostAccuracyCheckId;
                    activity.Timezone = activity_json.Timezone;

                    /*
                    string strActivity = "activity.DatasetId = " + activity.DatasetId + "\n" +
                        "activity.UserId = " + activity.UserId + "\n" +
                        "activity.SourceId = " + activity.SourceId + "\n" +
                        "activity.ActivityTypeId = " + activity.ActivityTypeId + "\n" +
                        "activity.CreateDate = " + activity.CreateDate + "\n" +
                        "activity.InstrumentId = " + activity.InstrumentId + "\n" +
                        //"activity.LaboratoryId = " + activity.LaboratoryId + "\n" +
                        "activity.AccuracyCheckId = " + activity.AccuracyCheckId + "\n" +
                        "activity.PostAccuracyCheckId = " + activity.PostAccuracyCheckId + "\n" +
                        "activity.Timezone = " + activity.Timezone;

                    logger.Debug("activity = " + activity);
                    logger.Debug(strActivity);
                    */
                    logger.Debug("and we have finished parameters (DataActionController).");
                    /*
                    //check for duplicates.  If it is a duplicate, add it to our list and bail out.
                    if (activity.isDuplicate())
                    {
                        duplicateActivities.Add(activity);
                    }
                    */

                    db.Activities.Add(activity);
                    now = DateTime.Now;
                    //logger.Debug("Added activity..." + now);
                    db.SaveChanges();
                    now = DateTime.Now;
                    //logger.Debug("Saved activity..." + now);

                    dynamic activityqastatus = activity_json.ActivityQAStatus;

                    newActivityId = activity.Id;

                    ActivityQA newQA = new ActivityQA();
                    newQA.ActivityId = activity.Id;
                    newQA.QAStatusId = activityqastatus.QAStatusID.ToObject<int>();
                    newQA.Comments = activityqastatus.Comments;
                    newQA.EffDt = DateTime.Now;
                    newQA.UserId = activity.UserId;
                    newQA.QAStatusName = activityqastatus.Name;
                    newQA.QAStatusDescription = activityqastatus.Description;

                    db.ActivityQAs.Add(newQA);
                    //now = DateTime.Now;
                    //logger.Debug("Added ActivityQAs..." + now);
                    db.SaveChanges();
                    now = DateTime.Now;
                    //logger.Debug("Saved ActivityQAs..." + now);

                    logger.Debug("Created a new activity: ");
                    logger.Debug(" LocationID = " + activity_json.LocationId);
                    logger.Debug(" ActivityDate = " + activity_json.ActivityDate);
                    logger.Debug("  ID = " + activity.Id);

                    var header = activity_json.Header.ToObject(dbset_header_type);
                    //foreach (var hItem in activity_json.Header)
                    //{
                    //    logger.Debug("hItem = " + hItem);
                    //}
                    var details = new List<DataDetail>();
                    Dictionary<string, JArray> grids = new Dictionary<string, JArray>();

                    foreach (var detailitem in activity_json.Details)
                    {
                        //copy this json object into a EFF object // this is probably slow.
                        details.Add(detailitem.ToObject(dbset_detail_type));

                        //does this field have a relation/grid field?  If so then save those, too.
                        if (grid_fields != null)
                        {
                            foreach (var grid_field in grid_fields)
                            {
                                logger.Debug("Found a grid field: " + grid_field.DbColumnName);
                                grids.Add(grid_field.DbColumnName, detailitem[grid_field.DbColumnName]);
                            }
                        }

                    }

                    //now do the saving!
                    header.ActivityId = activity.Id;
                    logger.Debug("header.ActivityId = " + header.ActivityId);

                    header.ByUserId = activity.UserId;
                    logger.Debug("header.ByUserId = " + header.ByUserId);

                    header.EffDt = DateTime.Now;
                    logger.Debug("header.EffDt = " + header.EffDt);

                    //logger.Debug("header.WorkElementName = " + header.WorkElementName);
                    //logger.Debug("header.Measure = " + header.Measure);
                    //logger.Debug("header.PlannedValue = " + header.PlannedValue);
                    //logger.Debug("header.ActualValue = " + header.ActualValue);
                    //logger.Debug("header.Comments = " + header.Comments);

                    dbset_header.Add(header);
                    db.SaveChanges();
                    now = DateTime.Now;
                    logger.Debug("Added header..." + now);

                    //details
                    int rowid = 1;
                    foreach (var detail in details)
                    {
                        //logger.Debug("Got a detail...");
                        detail.RowId = rowid;
                        detail.RowStatusId = DataDetail.ROWSTATUS_ACTIVE;
                        detail.ActivityId = activity.Id;
                        detail.ByUserId = activity.UserId;
                        detail.EffDt = DateTime.Now;

                        dbset_detail.Add(detail);

                        //logger.Debug("added a detail");

                        //relation grids?
                        if (grids.Count > 0)
                        {
                            logger.Debug("We have grids in our data to save...");
                            foreach (KeyValuePair<string, JArray> grid_item in grids)
                            {
                                int grid_rowid = 1; //new grid field

                                var grid_type = dataset.Datastore.TablePrefix + "_" + grid_item.Key;
                                logger.Debug(" Hey we have a relation of type: " + grid_type);

                                //get objecttype of this type
                                var dbset_grid_type = db.GetTypeFor(grid_type);
                                var dbset_relation = db.GetDbSet(grid_type);

                                //logger.Debug("saving items in : " + grid_item.Key );

                                foreach (dynamic relation_row in grid_item.Value)
                                {
                                    //logger.Debug("Relationrow: " + relation_row);
                                    var relationObj = relation_row.ToObject(dbset_grid_type);

                                    relationObj.EffDt = DateTime.Now;
                                    relationObj.ParentRowId = rowid;
                                    relationObj.RowId = grid_rowid;
                                    relationObj.RowStatusId = DataDetail.ROWSTATUS_ACTIVE;
                                    relationObj.ByUserId = activity.UserId;
                                    relationObj.ActivityId = activity.Id;
                                    relationObj.QAStatusId = dataset.DefaultRowQAStatusId; //TODO?

                                    //logger.Debug("woot saving a grid row!");
                                    //logger.Debug(JsonConvert.SerializeObject( relationObj, Formatting.Indented));
                                    dbset_relation.Add(relationObj);
                                    grid_rowid++;
                                }
                            }
                        }

                        rowid++;
                    }
                    if (details.Count > 0)
                    {
                        now = DateTime.Now;
                        logger.Debug("Added details..." + now);
                    }

                    db.SaveChanges(); //save all details for this activity, then iterate to the next activity.
                    now = DateTime.Now;
                    logger.Debug("Saved records..." + now);
                }
                catch (Exception e)
                {
                    logger.Debug("An error occurred saving the activity or details: " + newActivityId, e.Message);

                    db = ServicesContext.RestartCurrent;
                    db.Configuration.AutoDetectChangesEnabled = true;
                    db.Configuration.ValidateOnSaveEnabled = true;

                    //ok, lets try to delete the activity that went bad.
                    db.Activities.Remove(db.Activities.Find(newActivityId));
                    db.SaveChanges();

                    logger.Debug("ok so we auto-deleted the activity we created: " + newActivityId);

                    throw e; //rethrow so that it'll come back as an error in the client.
                }
                finally
                {
                    db.Configuration.AutoDetectChangesEnabled = true;
                    db.Configuration.ValidateOnSaveEnabled = true;
                }

                //If there is a ReadingDateTime field in use, set the activity description to be the range of reading dates for this activity.
                if (newActivityId != 0 && (dataset.Datastore.TablePrefix == "WaterTemp")) // others with readingdatetime?
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
                    {
                        con.Open();
                        var query = "update Activities set Description = (select concat(convert(varchar,min(ReadingDateTime),111), ' - ', convert(varchar,max(ReadingDateTime),111)) from " + dataset.Datastore.TablePrefix + "_Detail_VW where ActivityId = " + newActivityId + ") where Id = " + newActivityId;

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            logger.Debug(query);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else if (newActivityId != 0 && (dataset.Datastore.TablePrefix == "WaterQuality")) // others with readingdatetime?
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
                    {
                        con.Open();
                        var query = "update Activities set Description = (select concat(convert(varchar,min(SampleDate),111), ' - ', convert(varchar,max(SampleDate),111)) from " + dataset.Datastore.TablePrefix + "_Detail_VW where ActivityId = " + newActivityId + ") where Id = " + newActivityId;

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            logger.Debug(query);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

            } //foreach activity

            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        //so we'll build one that generates sql directly since the EFF way has mediocre performance.
        [HttpPost]
        private HttpResponseMessage SaveDatasetActivitiesSQL(JObject jsonData)
        {
            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();

            dynamic json = jsonData;

            Dataset dataset = db.Datasets.Find(json.DatasetId.ToObject<int>());

            if (dataset == null)
                throw new Exception("Configuration Error.");

            Project project = db.Projects.Find(dataset.ProjectId);
            if (!project.isOwnerOrEditor(me))
                throw new Exception("Authorization error.");

            var data_header_name = dataset.Datastore.TablePrefix + "_Header";
            var data_detail_name = dataset.Datastore.TablePrefix + "_Detail";

            //these will get loaded once and then stay the same every time.
            var query_header = "INSERT INTO " + data_header_name + " (";
            var query_detail = "INSERT INTO " + data_detail_name + " (";
            var headerFields = new List<string>();
            var detailFields = new List<string>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                foreach (var item in json.activities)
                {
                    int newActivityId = 0;

                    //each activity in its own scope...

                    var trans = con.BeginTransaction();
                    if (item is JProperty)
                    {

                        var prop = item as JProperty;
                        dynamic activity_json = prop.Value;

                        Activity activity = new Activity();
                        activity.LocationId = activity_json.LocationId;

                        try
                        {
                            activity.ActivityDate = activity_json.ActivityDate;
                        }
                        catch (Exception e)
                        {
                            //TODO -- this is a very bad idea if the date is wrong...
                            logger.Debug("Ooops had an error converting date: " + activity_json.ActivityDate);
                            logger.Debug(e.ToString());

                            throw e;

                        }

                        activity.DatasetId = json.DatasetId;
                        activity.UserId = me.Id;
                        activity.SourceId = 1;                                                                  // TODO 
                        activity.ActivityTypeId = 1;
                        activity.CreateDate = DateTime.Now;
                        activity.InstrumentId = activity_json.InstrumentId;
                        activity.AccuracyCheckId = activity_json.AccuracyCheckId;
                        activity.PostAccuracyCheckId = activity_json.PostAccuracyCheckId;
                        activity.Timezone = (activity_json.Timezone != null) ? activity_json.Timezone.Replace("'", "''") : "";

                        var activity_query = "INSERT INTO Activities (LocationId, ActivityDate, DatasetId, UserId, SourceId, ActivityTypeId, CreateDate, Timezone) VALUES (";
                        activity_query +=
                            activity.LocationId + ",'" +
                            activity.ActivityDate + "'," +
                            activity.DatasetId + "," +
                            activity.UserId + "," +
                            activity.SourceId + "," +
                            activity.ActivityTypeId + "," +
                            "'" + activity.CreateDate + "'," +
                            //activity.InstrumentId + "," +
                            //activity.AccuracyCheckId + "," +
                            //activity.PostAccuracyCheckId + "," +
                            "'" + activity.Timezone + "');";
                        activity_query += "SELECT SCOPE_IDENTITY();";

                        logger.Debug(activity_query);

                        using (SqlCommand cmd = new SqlCommand(activity_query, con, trans))
                        {
                            var result = cmd.ExecuteScalar();
                            //logger.Debug(result + result.GetType().ToString()); = System.Decimal?!
                            newActivityId = Convert.ToInt32(result.ToString());
                        }

                        //db.Activities.Add(activity);
                        //db.SaveChanges();

                        dynamic activityqastatus = activity_json.ActivityQAStatus;

                        activity.Id = newActivityId;
                        logger.Debug("Hey!  we have a new activity id the ol' fashioned way: " + activity.Id);

                        var newQA_query = "INSERT INTO ActivityQAs (ActivityId, QAStatusId, Comments, EffDt, UserId) VALUES (";

                        ActivityQA newQA = new ActivityQA();
                        newQA.ActivityId = newActivityId;
                        newQA.QAStatusId = activityqastatus.QAStatusID.ToObject<int>();
                        newQA.Comments = activityqastatus.Comments.Replace("'", "''");
                        newQA.EffDt = DateTime.Now;
                        newQA.UserId = activity.UserId;

                        newQA_query += newQA.ActivityId + "," +
                            newQA.QAStatusId + "," +
                            "'" + newQA.Comments + "','" +
                            newQA.EffDt + "'," +
                            newQA.UserId + ");";

                        logger.Debug(newQA_query);

                        using (SqlCommand cmd = new SqlCommand(newQA_query, con, trans))
                        {
                            if (cmd.ExecuteNonQuery() == 0)
                            {
                                logger.Debug("Failed to execute query: " + newQA_query);
                                throw new Exception("Failed to execute qa query.  See log.");
                            }
                        }

                        //db.ActivityQAs.Add(newQA);
                        //db.SaveChanges();

                        //get these ready for a new set of values
                        var query_header_values = " VALUES (";
                        var query_detail_values = " VALUES (";
                        var headerValues = new List<string>();
                        var detailValues = new List<string>();

                        //have our headers been populated yet?  we only have to do it once.
                        if (headerFields.Count == 0)
                        {
                            //first the ones we always have
                            headerFields.Add("ActivityId");
                            headerFields.Add("ByUserId");
                            headerFields.Add("EffDt");

                            //now spin through and add any incoming ones from our JSON.
                            var the_header = activity_json.Header as JObject;
                            IList<string> propertyNames = the_header.Properties().Select(p => p.Name).ToList();
                            foreach (var prop_field in propertyNames)
                            {
                                headerFields.Add(prop_field);
                            }
                        }

                        headerValues.Add(activity.Id.ToString());
                        headerValues.Add(activity.UserId.ToString());
                        headerValues.Add("'" + DateTime.Now.ToString() + "'");

                        //now populate header values 
                        foreach (var prop_field in headerFields)
                        {
                            if (prop_field != "ActivityId" && prop_field != "ByUserId" && prop_field != "EffDt") //these are already done.
                            {
                                var control_type = dataset.Fields.Where(o => o.Field.DbColumnName == prop_field).Single().ControlType;
                                var objVal = activity_json.Header.GetValue(prop_field);
                                if (objVal == null)
                                    headerValues.Add("null");
                                else
                                {
                                    headerValues.Add(QueryHelper.getStringValueByControlType(control_type, objVal.ToString()));
                                }
                            }
                        }

                        var the_query = query_header + string.Join(",", headerFields) + ") " + query_header_values + string.Join(",", headerValues) + ")";
                        logger.Debug(the_query);
                        using (SqlCommand cmd = new SqlCommand(the_query, con, trans))
                        {
                            if (cmd.ExecuteNonQuery() == 0)
                            {
                                logger.Debug("Failed to execute query: " + the_query);
                                throw new Exception("Failed to execute header query.  See log.");
                            }
                        }

                        //---------------- now for the details...
                        int rowid = 1;
                        foreach (JObject detail in activity_json.Details)
                        {
                            //have our detail fields been populated yet?  we only have to do it once.
                            if (detailFields.Count == 0)
                            {
                                //first the ones we always have
                                detailFields.Add("ActivityId");
                                detailFields.Add("ByUserId");
                                detailFields.Add("EffDt");
                                detailFields.Add("RowStatusId");
                                detailFields.Add("RowId");
                                detailFields.Add("QAStatusId");

                                //now spin through and add any incoming ones from our JSON.
                                IList<string> propertyNames = detail.Properties().Select(p => p.Name).ToList();
                                foreach (var prop_field in propertyNames)
                                {
                                    DatasetField the_field = dataset.Fields.Where(o => o.Field.DbColumnName == prop_field && o.FieldRoleId == 2).SingleOrDefault();
                                    if (the_field != null)
                                        detailFields.Add(prop_field);
                                }
                            }

                            detailValues.Add(activity.Id.ToString());
                            detailValues.Add(activity.UserId.ToString());
                            detailValues.Add("'" + DateTime.Now.ToString() + "'");
                            detailValues.Add(DataDetail.ROWSTATUS_ACTIVE.ToString());
                            detailValues.Add(rowid.ToString());
                            detailValues.Add(detail.GetValue("QAStatusId").ToString());

                            //now populate detail values 
                            foreach (var prop_field in detailFields)
                            {
                                if (prop_field != "QAStatusId" && prop_field != "ActivityId" && prop_field != "ByUserId" && prop_field != "EffDt" && prop_field != "RowId" && prop_field != "RowStatusId") //these are already done.
                                {
                                    var control_type = dataset.Fields.Where(o => o.Field.DbColumnName == prop_field).SingleOrDefault().ControlType;
                                    var objVal = detail.GetValue(prop_field);
                                    if (objVal == null)
                                        detailValues.Add("null");
                                    else
                                    {
                                        detailValues.Add(QueryHelper.getStringValueByControlType(control_type, objVal.ToString()));

                                    }
                                }
                            }
                            rowid++;
                            var the_detail_query = query_detail + string.Join(",", detailFields) + ") " + query_detail_values + string.Join(",", detailValues) + ")";
                            //logger.Debug(the_detail_query);
                            using (SqlCommand cmd = new SqlCommand(the_detail_query, con, trans))
                            {
                                if (cmd.ExecuteNonQuery() == 0)
                                {
                                    logger.Debug("Problem executing: " + the_detail_query);
                                    throw new Exception("Failed to execute detail query!");
                                }
                            }
                            detailValues = new List<string>();
                        }//foreach detail

                        //If there is a ReadingDateTime field in use, set the activity description to be the range of reading dates for this activity.
                        if (newActivityId != 0 && dataset.Datastore.TablePrefix == "WaterTemp") // others with readingdatetime?
                        {
                            var query = "update Activities set Description = (select concat(convert(varchar,min(ReadingDateTime),111), ' - ', convert(varchar,max(ReadingDateTime),111)) from " + dataset.Datastore.TablePrefix + "_Detail_VW where ActivityId = " + newActivityId + ") where Id = " + newActivityId;

                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                logger.Debug(query);
                                cmd.ExecuteNonQuery();
                            }
                        }


                    }//if is a jproperty

                    trans.Commit();

                }//foreach activity
            }//connection

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
