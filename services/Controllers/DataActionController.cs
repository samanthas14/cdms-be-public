using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using CsvHelper;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using services.Models;
using services.Models.Data;
using services.Resources;
using services.ExtensionMethods;

namespace services.Controllers
{
    [Authorize]
    public partial class DataActionController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public DataTable GetHeadersDataForDataset(int id)
        {
            var db = ServicesContext.Current;
            Dataset dataset = db.Datasets.Find(id);
            if (dataset == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            string query = "SELECT h.* FROM " + dataset.Datastore.TablePrefix + "_Header_VW h JOIN Activities a on a.Id = h.ActivityId WHERE a.DatasetId = " + dataset.Id;
            logger.Debug("query = " + query);

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;
        }


        [HttpPost]
        public IEnumerable<MetadataValue> GetMetadataFor(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;

            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            int EntityTypeId = json.EntityTypeId.ToObject<int>();

            if (project == null || me == null)
                throw new Exception("Configuration error. Please try again.");

            return MetadataHelper.getMetadata(project.Id, EntityTypeId).AsEnumerable();

        }

        // Note:  This is a POST, instead of a GET, because we are pulling lots of data.
        [HttpPost]
        public IEnumerable<Subproject_Hab> ProjectSubprojects(JObject jsonData)
        {
            logger.Debug("Inside ProjectSubprojects...");
            //logger.Debug("Fetching Subprojects for Project " + Id);
            var db = ServicesContext.Current;

            dynamic json = jsonData;
            logger.Debug("json = " + json);

            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            logger.Debug("project.Id = " + project.Id);

            var s = (from item in db.Subproject_Hab()
                     //where item.Id > 1
                     where item.Id > 1 && item.ProjectId == project.Id
                     orderby item.EffDt descending
                     select item).ToList();

            foreach (var sp in s)
            {
                logger.Debug("sp = " + sp.ProjectName);

                // First, convert the results to a list, so that we can sort them easily.
                sp.HabitatItems = sp.HabitatItems.ToList();

                //foreach (var hi in sp.HabitatItems)
                //{
                //    logger.Debug("hi = " + hi.ItemName);
                //}

                // Next, do the sort.
                sp.HabitatItems = sp.HabitatItems.OrderByDescending(x => x.EffDt).ToList();
            }

            return s.AsEnumerable();
        }

        /*[HttpPost]
        public IEnumerable<services.Models.File> SubprojectFiles(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;

            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            logger.Debug("ProjectId = " + project.Id);
            if (project == null || me == null)
                throw new Exception("Configuration error. Please try again.");

            Subproject_Hab subproject = db.Subproject_Hab.Find(json.SubprojectId.ToObject<int>());
            logger.Debug("SubprojectId = " + subproject.Id);

            var result = (from item in db.Files
                     where item.ProjectId == project.Id
                     where item.Subproject_CrppId == subproject.Id
                     select item).ToList();

            return result.AsEnumerable();
        }*/

        [HttpGet]
        public IEnumerable<Dataset> GetMyDatasets()
        {
            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();
            var mydatasets = "";
            try
            {
                mydatasets = me.UserPreferences.Where(o => o.Name == UserPreference.DATASETS).FirstOrDefault().Value;
            }
            catch (Exception e)
            {
                logger.Debug("Couldn't get your datasets -- probably don't have any favorites.");
                logger.Debug(e);
            }

            var datasets = db.Datasets.SqlQuery("SELECT * FROM Datasets WHERE Id in (" + mydatasets +") ORDER BY Name");

            return datasets;
        }

        [HttpGet]
        public IEnumerable<Project> GetMyProjects()
        {
            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();
            var my_projects = "";
            try
            {
                my_projects = me.UserPreferences.Where(o => o.Name == UserPreference.PROJECTS).FirstOrDefault().Value;
            }
            catch (Exception e)
            {
                logger.Debug("Couldn't get your projects -- probably don't have any favorites.");
                logger.Debug(e);
            }

            var myprojects = db.Projects.SqlQuery("SELECT * FROM Projects WHERE Id in (" + my_projects + ") ORDER BY Name");

            return myprojects;
        }

        [HttpPost]
        public HttpResponseMessage SetQaStatus(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            User me = AuthorizationManager.getCurrentUser();
            Activity activity = db.Activities.Find(json.ActivityId.ToObject<int>());

            if (activity == null || me == null)
                throw new Exception("Configuration error. Please try again.");

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
        public HttpResponseMessage SetProjectEditors(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            if (project == null)
                throw new Exception("Configuration error.");

            User me = AuthorizationManager.getCurrentUser(); 
            if (me == null)
                throw new Exception("Configuration error.");

            //verify that the sender is the project owner. 
            if (!project.isOwnerOrEditor(me))
                throw new Exception("Authorization error.");

            //First -- remove all editors from this project.
            project.Editors.RemoveAll(o => o.Id > 0);
            db.SaveChanges();

            foreach (var item in json.Editors)
            {
                User user = db.User.Find(item.Id.ToObject<int>());
                if (user == null)
                    logger.Debug("Wow -- user not found!: " + item.Id);
                else
                {
                    logger.Debug("Adding: " + item.Id);
                    project.Editors.Add(user);
                }
            }

            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        [HttpPost]
        public HttpResponseMessage SetDatasetMetadata(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;

            Dataset dataset = db.Datasets.Find(json.DatasetId.ToObject<int>());
            if (dataset == null)
                throw new Exception("Configuration error.");

            Project project = db.Projects.Find(dataset.ProjectId);
            
            User me = AuthorizationManager.getCurrentUser();
            if (!project.isOwnerOrEditor(me))
                throw new Exception("Configuration error.");

            //Now save metadata
            List<MetadataValue> metadata = new List<MetadataValue>();

            foreach (var jmv in json.Metadata)
            {
                var mv = jmv.ToObject<MetadataValue>();
                mv.UserId = me.Id;
                metadata.Add(mv);
                //logger.Debug("Found new metadata: " + mv.MetadataPropertyId + " + + " + mv.Values);
            }

            //fire setMetdata which will handle persisting the metadata
            dataset.Metadata = metadata;

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
                throw new Exception("Configuration error.  Please try again.");

            User me = AuthorizationManager.getCurrentUser();
            if (!project.isOwnerOrEditor(me))
                throw new Exception("Configuration error.");

            var Activities = new List<string>();

            foreach (var item in json.Activities)
            {
                Activities.Add(""+item.Id);
            }

            var ActivityIds = string.Join(",", Activities);
            var DataTable = dataset.Datastore.TablePrefix; 

            //open a raw database connection...
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "DELETE FROM " + DataTable + "_Detail WHERE ActivityId in ("+  ActivityIds + ")";
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
        
        [HttpPost]
        public DataTable GetRelationData(JObject jsonData)
        {
            //int FieldId, int ActivityId, int ParentRowId)
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            int FieldId = json.FieldId.ToObject<int>();

            Field f = db.Fields.Find(FieldId);

            if (f == null || f.ControlType != "grid")
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            DataTable dt = new DataTable();

            if (json["ActivityId"] is JToken && json["ParentRowId"] is JToken)
            {

                int DatasetId = Convert.ToInt32(f.DataSource);
                Dataset dataset = db.Datasets.Find(DatasetId);

                if (dataset == null)
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));

                int ActivityId = json.ActivityId.ToObject<int>();
                int ParentRowId = json.ParentRowId.ToObject<int>();

                //dbcolumname for "grid" type fields 
                string query = "SELECT h.* FROM "+dataset.Datastore.TablePrefix +"_" + f.DbColumnName + "_VW h WHERE h.ActivityId = " + ActivityId + " AND h.ParentRowId = " + ParentRowId;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }
        
        //QUERY
        //QueryDatasetActivities -- can call with a datasetId or a datastoreId
        [HttpPost]
        public DataTable QueryDatasetActivities(JObject jsonData)
        {
            var db = ServicesContext.Current;
            DataTable datatable = null;

            dynamic json = jsonData;
            
            //let's see if we're dealing with a dataset 
            if (json["DatasetId"] is JToken)
            {
                //grab a reference to this dataset so we can parse incoming fields
                Dataset dataset = db.Datasets.Find(json.DatasetId.ToObject<int>());
                if (dataset == null)
                    throw new Exception("Configuration error. Please try again.");

                logger.Debug("Alright!  we are working with dataset: " + dataset.Id);

                //datatable = getQueryResults(dataset, json);
                datatable = getQueryResults(dataset, json, "Query");

            }


            //let's see if we're dealing with a datastore.
            if (json["DatastoreId"] is JToken)
            {
                //grab a reference to this dataset so we can parse incoming fields
                Datastore datastore = db.Datastores.Find(json.DatastoreId.ToObject<int>());
                if (datastore == null)
                    throw new Exception("Configuration error. Please try again.");

                logger.Debug("Alright!  we are working with datastore: " + datastore.Id);

                datatable = getQueryResults(datastore, json, "Query");

            }

            
            return datatable;
            
        }

        //Writes csv export file out to a file and returns the url.
        [HttpPost]
        public ExportResult DownloadDatasetActivities(JObject jsonData)
        {
            logger.Debug("Inside DownloadDatasetActivities...");
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            User me = AuthorizationManager.getCurrentUser();

            //grab a reference to this dataset so we can parse incoming fields
            Dataset dataset = db.Datasets.Find(json.DatasetId.ToObject<int>());
            if (dataset == null || me == null)
                throw new Exception("Configuration error. Please try again.");

            logger.Debug("Alright!  we are working with dataset: " + dataset.Id);

            //DataTable dt = getQueryResults(dataset, json);
            DataTable dt = getQueryResults(dataset, json, "Export");

            logger.Debug("Download data -- we have a result back.");

            string Filename = json.Filename;
            Filename = Filename.Replace("\"", string.Empty);
            Filename = Filename.Replace("\\", string.Empty);
            Filename = Filename.Replace("/", string.Empty);
            
            logger.Debug("Incoming filename specified: " + Filename);

            string root = System.Web.HttpContext.Current.Server.MapPath("~/exports");
            string the_file = root + @"\" + dataset.Id + @"_" + me.Id + @"\" + Filename;

            logger.Debug("saving file to location: " + the_file);

            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(the_file)); //will create if necessary.

            string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);
            //rootUrl += "/services/exports/" + dataset.Id + "_" + me.Id + "/" + Filename;
            rootUrl += "/" + System.Configuration.ConfigurationManager.AppSettings["ExecutingEnvironment"] + "exports/" + dataset.Id + "_" + me.Id + "/" + Filename;
            logger.Debug("rootUrl again = " + rootUrl);

            using (TextWriter writer = System.IO.File.CreateText(the_file)) //will overwrite = good
            {
                using (var csv = new CsvWriter(writer))
                {
                    IEnumerable<string> columnNames = dataset.getExportLabelsList();//dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);

                    string strHeader = "Waypoints File";
                    int intHeaderLength = strHeader.Length;
                    int intFieldHeaderLength = 0;
                    logger.Debug("dataset.Datastore.TablePrefix = " + dataset.Datastore.TablePrefix);

                    //columns
                    foreach (var header in columnNames)
                    {
                        //logger.Debug("header = " + header);
                        intFieldHeaderLength = header.Length;
                        if (dataset.Datastore.TablePrefix == "SpawningGroundSurvey")
                        {
                            if ((intFieldHeaderLength >= intHeaderLength) && (header.IndexOf(strHeader) > -1))
                            {
                                // For Spawning Ground Survey, the "Waypoints File" header is unnecessary,
                                // because the data is not saved.  The header is only used in the front end.
                                // Consequently, it causes the data items to be skewed; everything from Channel to the right,
                                // gets shifted to the left one, because there is no Waypoint File entry.
                                // Therefore, we will skip adding this header.
                                logger.Debug("Skipping Waypoints File Header...");
                            }
                            else
                            {
                                csv.WriteField(header);
                            }
                        }
                        else
                        {
                            csv.WriteField(header);
                        }
                    }
                    csv.NextRecord();

                    //fields
                    foreach (DataRow row in dt.Rows)
                    {
                        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                        foreach (var field in fields)
                        {
                            //logger.Debug("field before replace = " + field);
                            //replace out the multiselect array stuff.
                            var f = field.Replace("[]", string.Empty).Replace("[\"", string.Empty).Replace("\"]", string.Empty).Replace("\",\"", ",");
                            //logger.Debug("field after replace = " + f);
                            csv.WriteField(f);
                        }
                        csv.NextRecord();
                    }
                }
            }

            //TODO-- error handling?

            ExportResult result = new ExportResult();
            result.success = true;
            result.file = rootUrl;
            result.errors = null;
            
            return result;

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

                    var last_header = this.getFirstItem(last_header_list);

                    logger.Debug("Ok -- here we are with our lastheader:");
                    logger.Debug(last_header);


                    if (last_header == null)
                        throw new Exception("Somehow there is no previous header even though we are trying to update.");

                    bool header_updated = false;

                    //spin through and check the header fields for changes...
                    foreach (JProperty header_field in activity_json.Header)
                    {
                        logger.Debug("Checking last header value of field : '" + header_field.Name + "' with incoming value + '" + header_field.Value+"'");

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

                    var last_row_list = dbset_detail.SqlQuery("SELECT * FROM " + data_detail_name + " WHERE ActivityId = "+ activity.Id + " AND RowStatusId = " + DataDetail.ROWSTATUS_ACTIVE + " ORDER BY RowId DESC");
                        //db.AdultWeir_Detail.Where(o => o.ActivityId == activity.Id).Where(o => o.RowStatusId == DataDetail.ROWSTATUS_ACTIVE).OrderByDescending(d => d.RowId).FirstOrDefault();
                    
                    var last_row = this.getFirstItem(last_row_list);
                    if (last_row != null)
                    {
                        rowid = last_row.RowId+1;
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
                    foreach(var deleted_row in json.deletedRowIds)
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
                                grid_rowid = grid_item.Value.Count()+1;

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

        private dynamic getFirstItem(dynamic list)
        {
            dynamic first = null;
            foreach (var item in list)
            {
                first = item;
                break;
            }
            return first;
        }

        public HttpResponseMessage SaveDatasetActivities(JObject jsonData)
        {
            return SaveDatasetActivitiesEFF(jsonData);
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
                            activity.Timezone = (activity_json.Timezone != null) ? activity_json.Timezone.Replace("'","''") : "";
                            
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

                            using (SqlCommand cmd = new SqlCommand(activity_query, con,trans))
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
                            newQA.Comments = activityqastatus.Comments.Replace("'","''");
                            newQA.EffDt = DateTime.Now;
                            newQA.UserId = activity.UserId;

                            newQA_query += newQA.ActivityId + "," +
                                newQA.QAStatusId + "," +
                                "'" + newQA.Comments + "','" +
                                newQA.EffDt  + "'," +
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
                                        headerValues.Add(getStringValueByControlType(control_type, objVal.ToString()));
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
                                            detailValues.Add(getStringValueByControlType(control_type, objVal.ToString()));

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

        private string getStringValueByControlType(string control_type, string in_val)
        {
            string retval = null;

            switch (control_type)
            {
                case "text":
                case "textarea":
                case "multiselect":
                case "select":
                case "date":
                case "datetime":
                    retval = "'" + in_val.Replace("'", "''") + "'";
                    break;
                default:
                    retval = in_val;
                    break;
            }

            return retval;
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

                    //logger.Debug("activity = " + activity);
                    //logger.Debug(strActivity);
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
                    logger.Debug("An error occurred saving the activity or details: "+newActivityId, e.Message);

                    db = ServicesContext.RestartCurrent;
                    db.Configuration.AutoDetectChangesEnabled = true;
                    db.Configuration.ValidateOnSaveEnabled = true;
                                
                    //ok, lets try to delete the activity that went bad.
                    db.Activities.Remove(db.Activities.Find(newActivityId));
                    db.SaveChanges();

                    logger.Debug("ok so we auto-deleted the activity we created: " + newActivityId);

                    throw e; //rethrow so that it'll come back as an error in the client.
                }
                finally{
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
                else if(newActivityId != 0 && (dataset.Datastore.TablePrefix == "WaterQuality")) // others with readingdatetime?
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


         
       /**
       * Handle uploaded files
       * IEnumerable<File>
       */
        [HttpPost]
        public Task<HttpResponseMessage> UploadImportFile()
        {
            logger.Debug("Inside DataActionController, UploadImportFile...");
            logger.Debug("starting to process incoming files.");

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads");
            string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);

            logger.Debug("saving files to location: " + root);
            logger.Debug(" and the root url = " + rootUrl);

            var provider = new MultipartFormDataStreamProvider(root);

            User me = AuthorizationManager.getCurrentUser();

            var db = ServicesContext.Current;

            var task = Request.Content.ReadAsMultipartAsync(provider).ContinueWith(o =>
                {
                    logger.Debug("Inside task section...");

                    if (o.IsFaulted || o.IsCanceled)
                    {
                        logger.Debug("Error: " + o.Exception.Message);
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, o.Exception));
                    }

                    //Look up our project
                    logger.Debug("provider.FormData = " + provider.FormData);
                    Int32 ProjectId = Convert.ToInt32(provider.FormData.Get("ProjectId"));
                    logger.Debug("And we think the projectid === " + ProjectId);

                    Project project = db.Projects.Find(ProjectId);
                    logger.Debug("Project = " + project);
                    if (!project.isOwnerOrEditor(me))
                        throw new Exception("Authorization error:  The user trying to import is neither an Owner nor an Editor.");
                    else
                        logger.Debug("User authorized = " + me);

                    var newFileName = "";

                    foreach (MultipartFileData file in provider.FileData)
                    {

                        logger.Debug("Filename = " + file.LocalFileName);
                        logger.Debug("Orig = " + file.Headers.ContentDisposition.FileName);
                        logger.Debug("Name? = " + file.Headers.ContentDisposition.Name);

                        var fileIndex = ActionController.getFileIndex(file.Headers.ContentDisposition.Name); //"uploadedfile0" -> 0
                        var filename = file.Headers.ContentDisposition.FileName;
                        filename = filename.Replace("\"", string.Empty);

                        if (!String.IsNullOrEmpty(filename))
                        {
                            try
                            {
                                newFileName = ActionController.relocateProjectFile(
                                                file.LocalFileName,
                                                ProjectId,
                                                filename,
                                                true);

                                // For importing, we do not want to add the file to the Files table.
                                /*
                                File newFile = new File();
                                newFile.Title = provider.FormData.Get("Title_" + fileIndex); //"Title_1, etc.
                                newFile.Description = provider.FormData.Get("Description_" + fileIndex); //"Description_1, etc.
                                newFile.Name = info.Name;//.Headers.ContentDisposition.FileName;
                                newFile.Link = rootUrl + "/services/uploads/" + ProjectId + "/" + info.Name; //file.LocalFileName;
                                newFile.Size = (info.Length / 1024).ToString(); //file.Headers.ContentLength.ToString();
                                newFile.FileTypeId = FileType.getFileTypeFromFilename(info);
                                newFile.UserId = me.Id;
                                logger.Debug(" Adding file " + newFile.Name + " at " + newFile.Link);

                                files.Add(newFile);
                                 */ 
                            }
                            catch (Exception e)
                            {
                                logger.Debug("Error: " + e.ToString());
                            }
                        }
                         
                    }

                    logger.Debug("Done saving files.");

                    var data = new ImportDataResult();
                    var info = new FileInfo(newFileName);

                    // Process the file and return all the data!

                    /* Note:  According to Colette, if someone tries to upload a file with an odd extension (.lkg, .fld, MCR, BC1, etc.),
                     * while the extension may vary, it will almost always be a ScrewTrap-PITAGIS related file.
                     * Therefore, we are allowing a wide range of variation in the extensions.
                    */
                    //var regex = new Regex(@"\.(m|r|ur|mc)\d+$");
                    //var regexNums = new Regex(@"\.(m|r|ur|mc|bc)\d+$");
                    //var regexChars = new Regex(@"\.(m|r|ur|mc|bc)\D+$");
                    var regexNums = new Regex(@"\.(m|r|ur|mc|bc|nb)\d+$");
                    var regexChars = new Regex(@"\.(m|r|ur|mc|bc|nb)\D+$");
                    var extension = info.Extension.ToLower();
                    logger.Debug("extension = " + extension);

                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        logger.Debug("Looks like an excel file!");
                        var reader = new ExcelReader(newFileName);
                        //ExcelReader doesn't support starting on a certain line for column names...  we always assume col 1
                        data.columns = reader.getColumns();
                        data.rows = reader.getData().First().Table;
                        reader.close();
                    }
                    else if (extension == ".csv")
                    {
                        logger.Debug("Looks like a csv file!");
                        var StartOnLine = Convert.ToInt32(provider.FormData.Get("StartOnLine")); //only applicable to T/CSV
                        var reader = new CSVReader(newFileName);
                        data = reader.getImportDataResult(StartOnLine); // we do it all in one.
                    }
                    else if (extension == ".tsv")
                    {
                        logger.Debug("Looks like a tsv file!");
                        var StartOnLine = Convert.ToInt32(provider.FormData.Get("StartOnLine")); //only applicable to T/CSV
                        var reader = new TSVReader(newFileName);
                        data = reader.getImportDataResult(StartOnLine); // we do it all in one.
                    }
                    //else if (extension == ".lkg" || extension == ".fld" || regex.Match(extension).Success)
                    else if (extension == ".lkg" || extension == ".fld" || regexNums.Match(extension).Success || regexChars.Match(extension).Success)
                    {
                        logger.Debug("Looks like a PITAGIS file!");
                        var reader = new PitagisReader(newFileName);
                        data = reader.getImportDataResult(); // we do it all in one.
                    }
                    else
                    {
                        logger.Debug("Looks like an unknown file!");
                        throw new Exception("File type not compatible.  We can do Excel (xls/xslx), CSV (csv), TSV (tsv), and PITAGIS (.lkg/.fld/.m01/.r01/.ur1/.mc1).");
                    }

                    var result = JsonConvert.SerializeObject(data);

                    //TODO: actual error/success message handling
                    //string result = "{\"message\": \"Success\"}";

                    var resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent(result, System.Text.Encoding.UTF8, "text/plain");  //to stop IE from being stupid.

                    return resp;

                });

            return task;

        }

        //UploadProjectFile - add a file to this project.
        /**
         * Handle uploaded files
         * IEnumerable<File>
         */
        public Task<HttpResponseMessage> UploadProjectFile()
        {
            logger.Debug("starting to process incoming project files.");

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            // Notes about root:
            // When a project file gets uploaded, it initially gets saved to the root location.
            // Then the file gets moved/renamed to its ultimate destination.
            // If the save fails, a file with a GUID-looking name remains in the root location.
            //string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads");
            //string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads");
            //string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"];
            string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\";
            logger.Debug("root = " + root);
            string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);
            logger.Debug("rootUrl = " + rootUrl);


            // Make sure our folder exists
            DirectoryInfo dirInfo = new DirectoryInfo(root);
            if (!dirInfo.Exists)
            {
                logger.Debug("Director DOES NOT exist; creating it...");
                dirInfo.Create();
                logger.Debug("...Created...");
            }
            else
            {
                logger.Debug("P dir already exists...");
                // Let's do some clean-up first.
                // If the file-move/copy fails for any reason, the Body-part file gets left behind.

                string[] filepaths = Directory.GetFiles(root);
                //logger.Debug("filepaths = " + filepaths);

                // Get yesterday's date.
                DateTime dtYesterday = DateTime.Now.AddDays(-1);

                foreach (string filePath in filepaths)
                {
                    try
                    {
                        // It is possible for two or more people to be uploading a file at the same time.
                        // So they could both have a bodypart showing.
                        // If the file's LastWriteTime <= yesterday's date, no one is working on it anymore,
                        // so we can delete it.  Basically, we let the file hang around for a day, 
                        // so we DO NOT delete a large file that someone is in the process of uploading.
                        FileInfo aFile = new FileInfo(filePath);
                        if (aFile.LastWriteTime <= dtYesterday)
                        {
                            logger.Debug("Cleaning out (deleting) file: " + filePath);
                            System.IO.File.Delete(filePath);
                        }
                    }
                    catch (IOException ioException)
                    {
                        logger.Debug("Had a problem cleaning out project files...");
                        logger.Debug("Exception Message:  " + ioException.Message);
                        logger.Debug("Inner Exception Message:  " + ioException.InnerException.Message);
                    }
                }
            }

            logger.Debug("saving files to location: " + root);
            logger.Debug(" and the root url = " + rootUrl);

            var provider = new MultipartFormDataStreamProvider(root);
            logger.Debug("Got provider...");

            User me = AuthorizationManager.getCurrentUser();
            logger.Debug("me = " + me.Username);

            var db = ServicesContext.Current;
            logger.Debug("Got db context");

            var task = Request.Content.ReadAsMultipartAsync(provider).ContinueWith(o =>
            {
                logger.Debug("Inside task...");
                if (o.IsFaulted || o.IsCanceled)
                {
                    logger.Debug("Error: " + o.Exception.Message);
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, o.Exception));
                }

                //Look up our project
                Int32 ProjectId = Convert.ToInt32(provider.FormData.Get("ProjectId"));
                logger.Debug("And we think the projectid === " + ProjectId);

                Project project = db.Projects.Find(ProjectId);

                if (project == null)
                    throw new Exception("Project ID not found: " + ProjectId);

                if (!project.isOwnerOrEditor(me))
                    throw new Exception("Authorization error.");

                //Now iterate through the files that just came in
                List<services.Models.File> files = new List<services.Models.File>();

                foreach (MultipartFileData file in provider.FileData)
                {

                    logger.Debug("Filename = " + file.LocalFileName);
                    logger.Debug("Orig = " + file.Headers.ContentDisposition.FileName);
                    logger.Debug("Name? = " + file.Headers.ContentDisposition.Name);

                    //var fileIndex = getFileIndex(file.Headers.ContentDisposition.Name); //"uploadedfile0" -> 0
                    var fileIndex = "0";
                    logger.Debug("Fileindex = " + fileIndex);
                    var filename = file.Headers.ContentDisposition.FileName;
                    filename = filename.Replace("\"", string.Empty);

                    if (!String.IsNullOrEmpty(filename))
                    {
                        try
                        {
                            var newFileName = ActionController.relocateProjectFile(
                                            file.LocalFileName,
                                            ProjectId,
                                            filename,
                                            false);

                            var info = new System.IO.FileInfo(newFileName);

                            services.Models.File newFile = new services.Models.File();
                            newFile.Title = provider.FormData.Get("Title"); //"Title_1, etc.
                            logger.Debug("Title = " + newFile.Title);

                            newFile.Description = provider.FormData.Get("Description"); //"Description_1, etc.
                            logger.Debug("Desc = " + newFile.Description);

                            newFile.Name = info.Name;//.Headers.ContentDisposition.FileName;

                            //newFile.Link = rootUrl + "/services/uploads/" + ProjectId + "/" + info.Name; //file.LocalFileName;
                            //newFile.Link = rootUrl + "/" + System.Configuration.ConfigurationManager.AppSettings["ExecutingEnvironment"] + "uploads/" + ProjectId + "/" + info.Name;
                            newFile.Link = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\" + ProjectId + "\\" + info.Name;

                            newFile.Size = (info.Length / 1024).ToString(); //file.Headers.ContentLength.ToString();
                            newFile.FileTypeId = FileType.getFileTypeFromFilename(info);
                            newFile.UserId = me.Id;
                            logger.Debug(" Adding file " + newFile.Name + " at " + newFile.Link);

                            files.Add(newFile);
                        }
                        catch (Exception e)
                        {
                            logger.Debug("Error: " + e.ToString());
                        }
                    }
                }

                List<services.Models.File> thefiles = new List<services.Models.File>();

                //Add files to database for this project.
                if (files.Count() > 0)
                {
                    logger.Debug("woot -- we have file objects to save");
                    foreach (var file in files)
                    {
                        project.Files.Add(file);
                        thefiles.Add(file);
                    }
                    db.Entry(project).State = EntityState.Modified;
                    db.SaveChanges();
                        
                }

                logger.Debug("Done saving project files.");
                var result = JsonConvert.SerializeObject(thefiles);
                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(result, System.Text.Encoding.UTF8, "text/plain");  //to stop IE from being stupid.

                return resp;
            });

            return task;
        }

        public Task<HttpResponseMessage> UploadDatasetFile()
        {
            logger.Debug("Inside UploadDatasetFile...");
            logger.Debug("starting to process incoming dataset files.");

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            //string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads");
            string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\";
            logger.Debug("root = " + root);
            string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);
            logger.Debug("rootUrl = " + rootUrl);

            // Make sure our folder exists
            DirectoryInfo dirInfo = new DirectoryInfo(@root);
            if (!dirInfo.Exists)
            {
                logger.Debug("Dir does not exist; will create it...");
                try
                {
                    System.IO.Directory.CreateDirectory(root);
                    logger.Debug("Created the dir...");
                }
                catch (IOException ioe)
                {
                    logger.Debug("Exception:  " + ioe.Message + ", " + ioe.InnerException);
                }
            }
            else
            {
                logger.Debug("P dir already exists...");

                // Let's do some clean-up first.
                // If the file-move/copy fails for any reason, the Body-part file gets left behind.

                string[] filepaths = Directory.GetFiles(root);
                //logger.Debug("filepaths = " + filepaths);

                // Get yesterday's date.
                DateTime dtYesterday = DateTime.Now.AddDays(-1);

                foreach (string filePath in filepaths)
                {
                    try
                    {
                        // It is possible for two or more people to be uploading a file at the same time.
                        // So they could both have a bodypart showing.
                        // If the file's LastWriteTime <= yesterday's date, no one is working on it anymore,
                        // so we can delete it.  Basically, we let the file hang around for a day, 
                        // so we DO NOT delete a large file that someone is in the process of uploading.
                        FileInfo aFile = new FileInfo(filePath);
                        if (aFile.LastWriteTime <= dtYesterday)
                        {
                            logger.Debug("Cleaning out (deleting) file: " + filePath);
                            System.IO.File.Delete(filePath);
                        }
                    }
                    catch (IOException ioException)
                    {
                        logger.Debug("Had a problem cleaning out dataset files...");
                        logger.Debug("Exception Message:  " + ioException.Message);
                        logger.Debug("Inner Exception Message:  " + ioException.InnerException.Message);
                    }
                }
            }

            logger.Debug("root location: " + root);
            logger.Debug(" root url = " + rootUrl);

            var provider = new MultipartFormDataStreamProvider(root);
            logger.Debug("provider = " + provider.ToString());

            User me = AuthorizationManager.getCurrentUser();

            var db = ServicesContext.Current;

            // provider below gets set to the root path, a few lines up above.
            var task = Request.Content.ReadAsMultipartAsync(provider).ContinueWith(o =>
            {
                logger.Debug("Inside task part...");

                if (o.IsFaulted || o.IsCanceled)
                {
                    logger.Debug("Error: " + o.Exception.Message);
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, o.Exception));
                }

                //foreach (var item in provider.FormData)
                //{
                //    logger.Debug("item = " + item);
                //}

                //Look up our project
                Int32 ProjectId = Convert.ToInt32(provider.FormData.Get("ProjectId"));
                logger.Debug("And we think the ProjectId === " + ProjectId);

                Int32 DatasetId = Convert.ToInt32(provider.FormData.Get("DatasetId"));
                logger.Debug("And we think the DatasetId === " + DatasetId);

                Project project = db.Projects.Find(ProjectId);

                Dataset dataset = db.Datasets.Find(DatasetId);

                if (project == null)
                    throw new Exception("Project ID not found: " + ProjectId);

                if (!project.isOwnerOrEditor(me))
                    throw new Exception("Authorization error.");

                if (dataset == null)
                    throw new Exception("Dataset ID not found: " + DatasetId);

                //If the project/dataset folder does not exist, create it.
                string datasetPath = root + project.Id + "\\D\\" + dataset.Id;
                //DirectoryInfo datasetDirInfo = new DirectoryInfo(@root);
                DirectoryInfo datasetDirInfo = new DirectoryInfo(datasetPath);
                if (!datasetDirInfo.Exists)
                {
                    logger.Debug("Dir does not exist; will create it...");
                    try
                    {
                        System.IO.Directory.CreateDirectory(root);
                        logger.Debug("Created the dir...");
                    }
                    catch (IOException ioe)
                    {
                        logger.Debug("Exception:  " + ioe.Message + ", " + ioe.InnerException);
                    }
                }



                //Now iterate through the files that just came in
                List<services.Models.File> files = new List<services.Models.File>();

                foreach (MultipartFileData file in provider.FileData)
                {

                    logger.Debug("Filename = " + file.LocalFileName);
                    logger.Debug("Orig = " + file.Headers.ContentDisposition.FileName);
                    logger.Debug("Name? = " + file.Headers.ContentDisposition.Name);

                    //var fileIndex = getFileIndex(file.Headers.ContentDisposition.Name); //"uploadedfile0" -> 0
                    var fileIndex = "0";
                    logger.Debug("Fileindex = " + fileIndex);
                    var filename = file.Headers.ContentDisposition.FileName;
                    filename = filename.Replace("\"", string.Empty);
                    logger.Debug("filename = " + filename);

                    if (!String.IsNullOrEmpty(filename))
                    {
                        try
                        {
                            //var newFileName = ActionController.relocateDatasetFile(
                            //                file.LocalFileName,
                            //                DatasetId,
                            //                filename,
                            //                false);

                            var newFileName = ActionController.relocateDatasetFile(
                                            file.LocalFileName,
                                            ProjectId,
                                            DatasetId,
                                            filename,
                                            false);

                            var info = new System.IO.FileInfo(newFileName);

                            services.Models.File newFile = new services.Models.File();
                            newFile.Title = provider.FormData.Get("Title"); //"Title_1, etc.
                            logger.Debug("Title = " + newFile.Title);

                            newFile.Description = provider.FormData.Get("Description"); //"Description_1, etc.
                            logger.Debug("Desc = " + newFile.Description);

                            newFile.Name = info.Name;//.Headers.ContentDisposition.FileName;

                            //newFile.Link = rootUrl + "/" + System.Configuration.ConfigurationManager.AppSettings["ExecutingEnvironment"] + "uploads/" + ProjectId + "/" + info.Name;
                            //newFile.Link = rootUrl + "/" + System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\" + ProjectId + "\\D\\" + DatasetId + "\\" + info.Name;
                            newFile.Link = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\" + ProjectId + "\\D\\" + DatasetId + "\\" + info.Name;

                            newFile.Size = (info.Length / 1024).ToString(); //file.Headers.ContentLength.ToString();
                            newFile.FileTypeId = FileType.getFileTypeFromFilename(info);
                            newFile.UserId = me.Id;
                            newFile.ProjectId = ProjectId;
                            newFile.DatasetId = DatasetId;
                            newFile.Subproject_CrppId = null; // No subprojectId for dataset files.
                            logger.Debug(" Adding file " + newFile.Name + " at " + newFile.Link);

                            files.Add(newFile);
                        }
                        catch (Exception e)
                        {
                            logger.Debug("Error: " + e.ToString());
                        }
                    }
                }

                // We will return thefiles to the calling program.
                List<services.Models.File> thefiles = new List<services.Models.File>();

                //Add files to database for this project.
                if (files.Count() > 0)
                {
                    logger.Debug("woot -- we have file objects to save");
                    foreach (var file in files)
                    {
                        project.Files.Add(file);
                        thefiles.Add(file);
                    }
                    db.Entry(project).State = EntityState.Modified;
                    db.SaveChanges();

                }

                logger.Debug("Done saving dataset files.");
                var result = JsonConvert.SerializeObject(thefiles);
                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(result, System.Text.Encoding.UTF8, "text/plain");  //to stop IE from being stupid.

                return resp;
            });
            return task;
        }

        

        public Task<HttpResponseMessage> UploadHabitatFile()
        //public IEnumerable<services.Models.File> UploadHabitatFile()
        {
            logger.Debug("Inside UploadHabitatFile...");
            logger.Debug("starting to process incoming Habitat files.");

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            //string root = System.Configuration.ConfigurationManager.AppSettings["PathToHabitatProjectDocuments"];
            string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\";

            logger.Debug("root = " + root);
            string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);
            logger.Debug("rootUrl = " + rootUrl);


            // Make sure our folder exists
            DirectoryInfo dirInfo = new DirectoryInfo(@root);
            if (!dirInfo.Exists)
            {
                logger.Debug("Dir does not exist; will create it...");
                //dirInfo.Create();
                try
                {
                    System.IO.Directory.CreateDirectory(root);
                    logger.Debug("Created the dir...");
                }
                catch (IOException ioe)
                {
                    logger.Debug("Exception:  " + ioe.Message + ", " + ioe.InnerException);
                }
            }
            else
                logger.Debug("Dir already exists...");

            logger.Debug("saving files to location: " + root);
            logger.Debug(" and the root url = " + rootUrl);

            var provider = new MultipartFormDataStreamProvider(root);
            logger.Debug("provider = " + provider.ToString());

            User me = AuthorizationManager.getCurrentUser();

            var db = ServicesContext.Current;

            // provider below gets set to the root path, a few lines up above.
            var task = Request.Content.ReadAsMultipartAsync(provider).ContinueWith(o =>
            {
                if (o.IsFaulted || o.IsCanceled)
                {
                    logger.Debug("Error: " + o.Exception.Message);
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, o.Exception));
                }

                //Look up our project
                Int32 ProjectId = Convert.ToInt32(provider.FormData.Get("ProjectId"));
                logger.Debug("And we think the ProjectId = " + ProjectId);

                Int32 SubprojectId = Convert.ToInt32(provider.FormData.Get("SubprojectId"));
                logger.Debug("And we think the Subprojectid = " + SubprojectId);

                string SubprojectType = provider.FormData.Get("SubprojectType");
                logger.Debug("And we think the SubprojectType = " + SubprojectType);

                int SubprojectFeatureImage = 0;
                SubprojectFeatureImage = Convert.ToInt32(provider.FormData.Get("FeatureImage"));
                logger.Debug("And we think FeatureImage = " + SubprojectFeatureImage);

                //string strDatastoreTablePrefix = provider.FormData.Get("DatastoreTablePrefix");
                //logger.Debug("And we think the DatastoreTablePrefix = " + strDatastoreTablePrefix);

                Project project = db.Projects.Find(ProjectId);
                logger.Debug("project.Id = " + project.Id);

                Subproject_Hab subproject = db.Subproject_Hab().Find(SubprojectId);
                logger.Debug("subproject.Id = " + subproject.Id);

                if (project == null)
                    throw new Exception("Project ID not found: " + ProjectId);

                if (!project.isOwnerOrEditor(me))
                    throw new Exception("Authorization error.");

                if (subproject == null)
                    throw new Exception("Subproject ID not found: " + SubprojectId);

                //if (strDatastoreTablePrefix == null)
                //    throw new Exception("DatastoreTablePrefix not found: " + strDatastoreTablePrefix);

                //Now iterate through the files that just came in
                List<services.Models.File> files = new List<services.Models.File>();

                foreach (MultipartFileData file in provider.FileData)
                {

                    logger.Debug("Filename = " + file.LocalFileName);
                    logger.Debug("Orig = " + file.Headers.ContentDisposition.FileName);
                    logger.Debug("Name? = " + file.Headers.ContentDisposition.Name);

                    //var fileIndex = getFileIndex(file.Headers.ContentDisposition.Name); //"uploadedfile0" -> 0
                    var fileIndex = "0";
                    logger.Debug("Fileindex = " + fileIndex);
                    var filename = file.Headers.ContentDisposition.FileName;
                    filename = filename.Replace("\"", string.Empty);
                    logger.Debug("filename = " + filename);

                    if (!String.IsNullOrEmpty(filename))
                    {
                        try
                        {
                            //var newFileName = ActionController.relocateSubprojectFile(
                            //                file.LocalFileName,
                            //                SubprojectId,
                            //                filename,
                            //                false);

                            var newFileName = ActionController.relocateSubprojectFile(
                                            file.LocalFileName,
                                            ProjectId,
                                            SubprojectId,
                                            filename,
                                            false);

                            var info = new System.IO.FileInfo(newFileName);

                            services.Models.File newFile = new services.Models.File();
                            newFile.Title = provider.FormData.Get("Title"); //"Title_1, etc.
                            logger.Debug("Title = " + newFile.Title);

                            newFile.Description = provider.FormData.Get("Description"); //"Description_1, etc.
                            logger.Debug("Desc = " + newFile.Description);

                            newFile.Name = info.Name;//.Headers.ContentDisposition.FileName;
                            logger.Debug("newFile.Name = " + newFile.Name);

                            //newFile.Link = rootUrl + "/services/uploads/subprojects/" + SubprojectId + "/" + info.Name; //file.LocalFileName;
                            //newFile.Link = rootUrl + "/" + System.Configuration.ConfigurationManager.AppSettings["ExecutingEnvironment"] + "uploads/subprojects/" + SubprojectId + "/" + info.Name;
                            //newFile.Link = System.Configuration.ConfigurationManager.AppSettings["PathToHabitatProjectDocuments"] + "\\" + SubprojectId + "\\" + info.Name;
                            newFile.Link = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\" + ProjectId + "\\S\\" + SubprojectId + "\\" + info.Name;
                            logger.Debug("newFile.Link = " + newFile.Link);

                            newFile.Size = (info.Length / 1024).ToString(); //file.Headers.ContentLength.ToString();
                            logger.Debug("newFile.Size = " + newFile.Size);

                            newFile.FileTypeId = FileType.getFileTypeFromFilename(info);
                            logger.Debug("newFile.FileTypeId = " + newFile.FileTypeId);

                            newFile.UserId = me.Id;
                            logger.Debug("newFile.UserId = " + newFile.UserId);

                            newFile.ProjectId = project.Id;
                            logger.Debug("newFile.ProjectId = " + newFile.ProjectId);

                            // I used this for CRPP, but changed to the SubprojectFiles table later, with the advent of Habitat.
                            // So, CRPP will also need to be changed over to the SubprojectFiles also.
                            // For now, the HabSubproject SubprojectId cannot be stored here, because it causes a problem with the foreign key.
                            /*
                            newFile.Subproject_CrppId = SubprojectId;
                            logger.Debug("newFile.Subproject_CrppId = " + newFile.Subproject_CrppId);
                            
                            newFile.SubprojectType = SubprojectType;
                            logger.Debug("newFile.SubprojectType = " + newFile.SubprojectType);
                            */
                            newFile.UploadDate = DateTime.Now;
                            logger.Debug("newFile.UploadDate = " + newFile.UploadDate);

                            logger.Debug(" Adding file " + newFile.Name + " at " + newFile.Link);

                            files.Add(newFile);
                            logger.Debug("Added file " + newFile.Name);
                        }
                        catch (Exception e)
                        {
                            logger.Debug("Error: " + e.ToString());
                        }
                    }
                }

                //List<services.Models.File> thefiles = new List<services.Models.File>();
                //List<int> fileIds = new List<int>();

                logger.Debug("files.Count() = " + files.Count());
                //Add files to database for this project.
                if (files.Count() > 0)
                {
                    logger.Debug("woot -- we have file objects to save");

                    if (SubprojectFeatureImage > 0)
                    {
                        // First, make a list of the SubprojectFiles for this subproject and set the FeatureImage to 0.
                        // Note:  If we are creating a new subproject, there won't be any records.
                        logger.Debug("ProjectId: " + project.Id + ", SubprojectId: " + subproject.Id);
                        //List<services.Models.SubprojectFiles> spFileList = (from item in db.SubprojectFiles
                        //                                                    where item.ProjectId == project.Id
                        //                                                    where item.SubprojectId == subproject.Id
                        //                                                    orderby item.Id
                        //                                                    select item).ToList();

                        List<services.Models.File> spFileList = (from item in db.Files
                                                                            where item.ProjectId == project.Id
                                                                            where item.Subproject_CrppId == subproject.Id
                                                                            orderby item.Id
                                                                            select item).ToList();

                        logger.Debug("spFileList size = " + spFileList.Count);
                        foreach (var spFile in spFileList)
                        {
                            //logger.Debug("Updating FeatureImage for " + spFile.FileName);
                            logger.Debug("Updating FeatureImage for " + spFile.Name);
                            spFile.FeatureImage = 0;
                            db.Entry(spFile).State = EntityState.Modified;
                        }

                        try
                        {
                            db.SaveChanges();
                        }
                        catch (System.Exception e)
                        {
                            logger.Debug("Error = " + e.InnerException);

                        }
                    }

                    // Next save the file in the Files table.
                    foreach (var file in files)
                    {
                        logger.Debug("file.name = " + file.Name);
                        //if (SubprojectFeatureImage > 0)
                            file.FeatureImage = SubprojectFeatureImage;

                        file.Subproject_CrppId = subproject.Id;
                        
                        project.Files.Add(file);
                        logger.Debug("Added file to project.Files...");
                        logger.Debug("file.id = " + file.Id);
                        //fileIds.Add(file.Id);

                        //thefiles.Add(file);
                        //logger.Debug("Added file to thefiles");
                    }
                    db.Entry(project).State = EntityState.Modified;
                    logger.Debug("Set State...");
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (System.Exception e)
                    {
                        logger.Debug("Error = " + e.InnerException);

                    }
                    logger.Debug("Saved changes to db...");
                }


                // Now get the ID of the file we just saved.
                /*foreach (var file in files)
                {
                    List<services.Models.File> fileList = (from item in db.Files
                                                           where item.Name == file.Name
                                                           where item.ProjectId == project.Id
                                                           where item.FileTypeId == file.FileTypeId
                                                           orderby item.Id
                                                           select item).ToList();

                    // Add the record to the subbproject files
                    // There should be only one.
                    string strAFile = "";
                    foreach (var fileRecord in fileList)
                    {
                        logger.Debug("Saving fileId " + fileRecord.Id + ", fileName = " + fileRecord.Name);
                        SubprojectFiles aFile = new SubprojectFiles();
                        aFile.ProjectId = project.Id;
                        aFile.SubprojectId = subproject.Id;
                        aFile.FileId = fileRecord.Id;
                        aFile.FileName = fileRecord.Name;
                        aFile.FeatureImage = SubprojectFeatureImage;

                        strAFile += " aFile.ProjectId = " + aFile.ProjectId + "\n";
                        strAFile += " aFile.SubprojectId = " + aFile.SubprojectId + "\n";
                        strAFile += " aFile.FileId = " + aFile.FileId + "\n";
                        strAFile += " aFile.FileName = " + aFile.FileName + "\n";
                        strAFile += " aFile.FeatureImage = " + aFile.FeatureImage + "\n";
                        logger.Debug("strAFile = " + strAFile);

                        db.SubprojectFiles.Add(aFile);
                    }
                    db.SaveChanges();
                }
                */

                logger.Debug("Done saving files.");
                //var result = JsonConvert.SerializeObject(thefiles);
                var result = JsonConvert.SerializeObject(files);
                //var result2 = JsonConvert.SerializeObject(fileIds);
                logger.Debug("result = " + result);

                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(result, System.Text.Encoding.UTF8, "text/plain");  //to stop IE from being stupid.

                return resp;
            });

            return task;
        }

        [HttpPost]
        public HttpResponseMessage UploadHabitatLink(JObject jsonData)
        {
            logger.Debug("Inside UploadHabitatLink...");
            logger.Debug("starting to process incoming Habitat links.");

            var db = ServicesContext.RestartCurrent;
            User me = AuthorizationManager.getCurrentUser();

            dynamic json = jsonData;

            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            if (project == null)
                throw new Exception("Configuration Error:  Could not find the ProjectId in the database.");

            if (!project.isOwnerOrEditor(me))
                throw new Exception("Authorization Error:  The user attempting to make changes is not an Owner or Editor.");

            Subproject_Hab subproject = db.Subproject_Hab().Find(json.SubprojectId.ToObject<int>());
            if (subproject == null)
                throw new Exception("Configuration Error:  Could not find the SubprojectId in the database.");

            string SubprojectType = json.SubprojectType.ToObject<string>();
            logger.Debug("And we think the SubprojectType = " + SubprojectType);

            //foreach (var item in linkList)

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        HttpResponseMessage error(string message)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = new StringContent(message, System.Text.Encoding.UTF8, "text/plain");
            return response;
        }


        // Users can upload waypoints; the data will be extracted, converted to json, then sent back to the browser in the response.
        // The uploaded files will NOT be saved.
        [HttpPost]
        public Task<HttpResponseMessage> HandleWaypoints()
        {
            var provider = new MultipartMemoryStreamProvider();

            var task = Request.Content.ReadAsMultipartAsync(provider).ContinueWith(o =>
                {
                    if (!Request.Content.IsMimeMultipartContent())
                        return error("Uploaded filed does not look like a waypoints file");

                    var data = new Dictionary<string, Dictionary<string, string>>();

                    foreach (var contents in provider.Contents)
                    {
                        var csvData = contents.ReadAsStreamAsync();
                        csvData.Wait();
                        var res = csvData.Result;

                        var parser = new TextFieldParser(res);

                        var readingHeaderRow = true;
                        var id = -1;
                        var lat = -1;
                        var lng = -1;
                        var x = -1;
                        var y = -1;

                        parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                        parser.SetDelimiters(",");
                        while (!parser.EndOfData)
                        {
                            var fields = parser.ReadFields();

                            // Figure out which fields we want
                            if (readingHeaderRow)
                            {
                                var ctr = 0;
                                foreach (var f in fields)
                                {
                                    if (f == "WP_")
                                        id = ctr;
                                    else if (f == "Longitude")
                                        lng = ctr;
                                    else if (f == "Latitude")
                                        lat = ctr;
                                    else if (f == "x_proj")
                                        x = ctr;
                                    else if (f == "y_proj")
                                        y = ctr;
                                    ctr++;
                                }

                                if (id == -1)
                                    return error("Could not find waypoint id column");
                                if (lat == -1)
                                    return error("Could not find lat column");
                                if (lng == -1)
                                    return error("Could not find long column");
                                if (x == -1)
                                    return error("Could not find projected x column");
                                if (y == -1)
                                    return error("Could not find projected y column");

                                readingHeaderRow = false;
                            }
                            else    // Parse a data row
                            {
                                try
                                {
                                    var dict = new Dictionary<string, string>();
                                    dict["lat"] = fields[lat];
                                    dict["long"] = fields[lng];
                                    dict["x"] = fields[x];
                                    dict["y"] = fields[y];
                                    // Ids look like 1.00000.... sanitize it to 1
                                    data[int.Parse(fields[id], NumberStyles.Any).ToString()] = dict;    // Will clobber data if ids are reused
                                }
                                catch
                                {
                                    return error("Could not parse waypoint file");
                                }
                            }
                        }
                        parser.Close();
                    }

                    var resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "text/plain"); 
                    return resp;
                });

            return task;
        }


        [HttpPost]
        public HttpResponseMessage SaveProject(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;
            
            var in_project = json.Project.ToObject<Project>();
            
            if (in_project == null)
            {
                logger.Debug("Error:  in_project = null");
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            User me = AuthorizationManager.getCurrentUser();
            logger.Debug("me.username = " + me.Username);

            List<MetadataValue> metadata = new List<MetadataValue>();
            logger.Debug("Created metadata list...");

            foreach (var jmv in json.Project.Metadata)
            {
                var mv = jmv.ToObject<MetadataValue>();
                mv.UserId = me.Id;
                metadata.Add(mv);
                logger.Debug("Found new metadata: " + mv.MetadataPropertyId + " + + " + mv.Values);
            }

            logger.Debug("in_project.Id = " + in_project.Id);
            if (in_project.Id == 0) //is it a NEW project or editing?
            {
                in_project.OrganizationId = Organization.DEFAULT_ORGANIZATION_ID;
                in_project.OwnerId = me.Id;
                in_project.CreateDateTime = DateTime.Now;
                in_project.ProjectTypeId = ProjectType.DEFAULT_PROJECT_TYPE;

                db.Projects.Add(in_project);
                db.SaveChanges();
                in_project.Metadata = metadata;
                db.SaveChanges(); //not sure if this is required.
                logger.Debug("Created new project: " + in_project.Id);
            }
            else
            {
                //find the existing project
                Project project = db.Projects.Find(in_project.Id);
                if (project == null)
                {
                    logger.Debug("project = null");
                    throw new Exception("Configuration error.");
                }

                //ok if they are editing the project, they can only edit projects they own or are editors
                if (!project.isOwnerOrEditor(me))
                {
                    logger.Debug("me is not an owner or editor.");
                    throw new Exception("Authorization error.");
                }

                //map our properties.
                project.Description = in_project.Description;
                project.EndDate = in_project.EndDate;
                project.StartDate = in_project.StartDate;
                project.Name = in_project.Name;

                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                logger.Debug("Saved property changes to project: " + project.Id);

                project.Metadata = metadata;
                db.SaveChanges();

            }

            HttpResponseMessage resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

            return resp;
        }


        /*
         * Executes a query from the given criteria in our json object on the given dataset 
         * datafieldsource can be a dataset or a datastore
         * returns: DataTable of results
         */

        //private DataTable getQueryResults(dynamic datafieldsource, dynamic json)
        private DataTable getQueryResults(dynamic datafieldsource, dynamic json, string productTarget) // productTarget is set by the calling method.
        {
            logger.Debug("Inside getQueryResults...");
            logger.Debug("productTarget = " + productTarget);

            var fields = datafieldsource.Fields;
            
            var conditions = new List<string>();

            //logger.Debug(json.Fields);
            //logger.Debug(json.Fields.ToString());
            logger.Debug("json = " + json);

            //fields in the criteria
            foreach (var item in json.Fields)
            {
                logger.Debug(item);
                logger.Debug("Colname!: " + item.DbColumnName);

                //spin through each of our dataset/datastore fields and find a match, adding it to our criteria...
                foreach (var field in fields)
                {
                    //logger.Debug(" -- alright now I think we're looking up field with id: " + item.Id);

                    if (field.Id != item.Id.ToObject<int>())
                        continue;

                    //logger.Debug("Looked up field: " + field.DbColumnName);

                    //if (field == null)
                    //    throw new Exception("Field not configured properly: " + item.Value);

                    string ControlType = field.ControlType.ToString(); //hmm, can't use directly in a switch.

                    //now add field criteria to our list...
                    switch (ControlType)
                    {
                        case "number":
                        case "currency":
                        case "time":
                        case "easting":
                        case "northing":
                            logger.Debug("A number, currency, time, northing, or easting");
                            conditions.Add(field.DbColumnName + item.Value); //>100
                            break;

                        case "text":
                        case "textarea":
                            logger.Debug("A txt");
                            var conditional = " = ";
                            if (item.Value.ToString().Contains("%"))
                                conditional = " LIKE ";
                            var replaced_value = item.Value.ToString().Replace("'", "''");
                            conditions.Add(field.DbColumnName + conditional + "'" + replaced_value + "'");
                            break;

                        case "multiselect":
                            logger.Debug("A MULTIselect:" + item.Value);
                            if (item.Value == null)
                                break;

                            dynamic mselect_val = item.Value; //array

                            //iterate and construct strings.
                            List<string> ms_condition = new List<string>();
                            foreach (var ms_item in mselect_val)
                            {
                                var replaced_ms_item = ms_item.ToString().Replace("'", "''");
                                ms_condition.Add(field.DbColumnName + " LIKE '%\"" + replaced_ms_item + "\"%'");
                            }

                            conditions.Add("(" + string.Join(" OR ", ms_condition) + ")");

                            break;
                        case "select":
                            logger.Debug("A select:" + item.Value);
                            if (item.Value == null)
                                break;

                            dynamic select_val = item.Value; //array

                            conditions.Add(field.DbColumnName + " in('" + string.Join("','", select_val) + "')");
                            break;
                        case "date":
                        case "datetime":
                            logger.Debug("A date!: ");
                            if (item.Value.ParamFieldDateType == "between") //otherwise, do nothing with this criteria
                            {
                                conditions.Add(field.DbColumnName + " between '" + item.Value.BetweenFromFieldDate + "' and '" + item.Value.BetweenToFieldDate + "'");
                            }


                            break;
                    }
                }

            }

            //DATE criteria
            if (json.DateSearchType == "singleYear")
            {
                if (json.TablePrefix == "ScrewTrap")
                    conditions.Add("MigrationYear = " + json.MigrationYear);
                else if (json.TablePrefix == "AdultWeir")
                    conditions.Add("RunYear = " + json.RunYear);
                else if (json.TablePrefix == "Metrics")
                    conditions.Add("YearReported = " + json.ReportYear);
                else if (json.TablePrefix == "StreamNet_NOSA")
                    conditions.Add("SpawningYear = " + json.SpawningYear);
                else if (json.TablePrefix == "StreamNet_RperS")
                    conditions.Add("BroodYear = '" + json.BroodYear + "'");
                else if (json.TablePrefix == "StreamNet_SAR")
                    conditions.Add("OutmigrationYear = " + json.OutmigrationYear);
            }
            else if (json.DateSearchType == "between")
            {
                if (json.TablePrefix == "WaterQuality")
                    conditions.Add("SampleDate BETWEEN CONVERT(Date, '" + json.FromDate + "') AND DATEADD(DAY,1,CONVERT(Date, '" + json.ToDate + "'))");
                else if (json.TablePrefix == "WaterTemp")
                    conditions.Add("ReadingDateTime BETWEEN CONVERT(Date, '" + json.FromDate + "') AND DATEADD(DAY,1,CONVERT(Date, '" + json.ToDate + "'))");
                else if (json.TablePrefix == "FishScales")
                    conditions.Add("ScaleCollectionDate BETWEEN CONVERT(Date, '" + json.FromDate + "') AND DATEADD(DAY,1,CONVERT(Date, '" + json.ToDate + "'))");
                //else if (json.TablePrefix == "StreamNet")
                //    conditions.Add("SpawningYear BETWEEN CONVERT(Date, '" + json.FromDate + "') AND DATEADD(DAY,1,CONVERT(Date, '" + json.ToDate + "'))");
                else
                    conditions.Add("ActivityDate BETWEEN CONVERT(Date, '" + json.FromDate + "') AND DATEADD(DAY,1,CONVERT(Date, '" + json.ToDate + "'))");
            }

            //LOCATION criteria
            if (json.Locations != "[\"all\"]")
            {
                logger.Debug("Locations = " + json.Locations);
                var locations = new List<string>();
                var locations_in = JArray.Parse(json.Locations.ToObject<string>());
                foreach (var item in locations_in)
                {
                    locations.Add(item.ToObject<string>());
                }
                conditions.Add("LocationId IN (" + string.Join(",", locations.ToArray()) + ")");
            }

            //QASTATUS
            if (json.QAStatusId != "all")
            {
                conditions.Add("ActivityQAStatusId=" + json.QAStatusId);
            }

            //ROWQASTATUS
            if (json.RowQAStatusId != null && json.RowQAStatusId != "[\"all\"]")
            {
                logger.Debug(json.RowQAStatusId);
                var rowqas = new List<string>();
                var rowqas_in = JArray.Parse(json.RowQAStatusId.ToObject<string>());
                foreach (var item in rowqas_in)
                {
                    rowqas.Add(item.ToObject<string>());
                }
                conditions.Add("QAStatusId IN (" + string.Join(",", rowqas.ToArray()) + ")");
            }


            /*

            var all_details = from d in db.AdultWeir_Detail
                              join a in db.Activities on d.ActivityId equals a.Id
                              where d.RowStatusId == DataDetail.ROWSTATUS_ACTIVE
                              join h2 in
                                (
                                    from hh in db.AdultWeir_Detail
                                    where hh.EffDt <= DateTime.Now
                                    group hh by new { hh.ActivityId, hh.RowId } into cig
                                    select new { ActivityId = cig.Key.ActivityId, RowId = cig.Key.RowId, EffDt = cig.Max(ed => ed.EffDt) }
                                ) on new { d.ActivityId, d.RowId, d.EffDt } equals new { h2.ActivityId, h2.RowId, h2.EffDt }
                            select d;

            var criteria_string = string.Join(" AND ", conditions.ToArray());
            logger.Debug(criteria_string);

            all_details = all_details.Where(criteria_string);

            return all_details;
             * */

            var datatable_prefix = "UNKNOWN";

            if(datafieldsource is Dataset)
                datatable_prefix = datafieldsource.Datastore.TablePrefix;
            else
	            datatable_prefix = datafieldsource.TablePrefix;

            //string query = "SET QUOTED_IDENTIFIER OFF; SELECT " + datafieldsource.getExportSelectString() + " from " + datatable_prefix + "_VW WHERE 1=1";
            string query = "";
            if ((datatable_prefix == "WaterTemp") ||
                (datatable_prefix == "WaterQuality"))
            {
                //query = "SET QUOTED_IDENTIFIER OFF; SELECT " + datafieldsource.getExportSelectString() + " from " + datatable_prefix + "_VW WITH (index(ix_ActivityId_EffDt)) WHERE 1=1";
                query = "SET QUOTED_IDENTIFIER OFF; SELECT " + datafieldsource.getExportSelectString(productTarget) + " from " + datatable_prefix + "_VW WITH (index(ix_ActivityId_EffDt)) WHERE 1=1";
            }
            else
            {
                //query = "SET QUOTED_IDENTIFIER OFF; SELECT " + datafieldsource.getExportSelectString() + " from " + datatable_prefix + "_VW WHERE 1=1";
                query = "SET QUOTED_IDENTIFIER OFF; SELECT " + datafieldsource.getExportSelectString(productTarget) + " from " + datatable_prefix + "_VW WHERE 1=1";
            }

            if(datafieldsource is Dataset)
                query += " AND DatasetId = " + datafieldsource.Id ;

            var criteria_string = string.Join(" AND ", conditions.ToArray());

            if (criteria_string != "")
                query += " AND " + criteria_string;

            logger.Debug("final query = " + query);
            //query = "SET QUOTED_IDENTIFIER OFF; SELECT * from AdultWeir_VW WHERE DatasetId = 5 AND Species=\"CHS\"";


            //open a raw database connection...
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                // Original block
                /*using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }*/

                // New block, to enable setting the command timeout.
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 120; // 2 minutes in seconds.
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.SelectCommand.CommandTimeout = 120;
                    da.Fill(dt);
                }
                catch (SqlException e)
                {
                    logger.Debug("Query sql command timed out..." + e.Message);
                    logger.Debug(e.InnerException);
                }
            }

            return dt;
         }
    }

}
