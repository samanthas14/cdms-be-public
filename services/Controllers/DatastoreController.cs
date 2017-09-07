using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using services.Models;
using services.Models.Data;
using services.Resources;
using services.ExtensionMethods;

//kb 5/1/2014
// this is a partial, so basically just a nice logical way of adding more to our DataActionController...

namespace services.Controllers
{
    [System.Web.Http.Authorize]
    public partial class DataActionController : ApiController
    {
        private Boolean debugMode = true;

        private string removeFormattingChars(string aString)
        {
            aString = aString.Replace(System.Environment.NewLine, "");
            aString = aString.Replace("\t", "");
            aString = aString.Replace("  ", " ");

            return aString;
        }

        [HttpGet]
        public IEnumerable<TimeZoneInfo> GetTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones();           
        }

        [HttpGet]
        public IEnumerable<LocationType> GetLocationTypes()
        {
            var db = ServicesContext.Current;
            return db.LocationType.AsEnumerable();
        }

        [HttpGet]
        public IEnumerable<Instrument> GetAllInstruments()
        {
            var db = ServicesContext.Current;
            return db.Instruments.OrderBy(o => o.Name).ThenBy(o => o.SerialNumber).AsEnumerable();
        }

        [HttpGet]
        public IEnumerable<WaterBody> GetWaterBodies()
        {
            var db = ServicesContext.Current;
            return db.WaterBodies.OrderBy(o => o.Name).AsEnumerable();
        }

        [HttpGet]
        public IEnumerable<Source> GetSources()
        {
            var db = ServicesContext.Current;
            return db.Sources.AsEnumerable();
        }

        [HttpGet]
        public IEnumerable<Instrument> GetInstruments()
        {
            var db = ServicesContext.Current;
            //return db.Instruments.AsEnumerable();
            //return db.Instruments.OrderBy(o => o.Name).ThenBy(o => o.SerialNumber).AsEnumerable();

            List<Instrument> i = (from item in db.Instruments
                                       orderby item.Name
                                       select item).ToList();
            return i.AsEnumerable();
        }

        [HttpGet]
        public IEnumerable<Fisherman> GetFishermen()
        {
            var db = ServicesContext.Current;
            logger.Info("Inside DatastoreController, getting fishermen...");

            List<Fisherman> f = (from item in db.Fishermen
                                 orderby item.LastName, item.FirstName, item.Aka
                                 select item).ToList();
            //logger.Debug(db.Fishermen);
            //return db.Fishermen.OrderBy(o => o.FullName).AsEnumerable();
            //return db.Fishermen.AsEnumerable();
            return f.AsEnumerable();
        }


        [HttpGet]
        public IEnumerable<InstrumentType> GetInstrumentTypes()
        {
            var db = ServicesContext.Current;
            return db.InstrumentType.AsEnumerable();
        }



        [HttpGet]
        public IEnumerable<Datastore> GetAllDatastores()
        {
            var db = ServicesContext.Current;
            return db.Datastores.AsEnumerable();
        }


        [HttpGet]
        public IEnumerable<Field> GetAllFields(int Id)
        {
            var db = ServicesContext.Current;
            logger.Info("Getting all fields...where FieldCategoryId = " + Id);
            return db.Fields.Where(o => o.FieldCategoryId == Id).OrderBy(o => o.Name).AsEnumerable();
        }


        [HttpGet]
        public Datastore GetDatastore(int Id)
        {
            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();

            var datastore = db.Datastores.Find(Id);
            if (datastore == null)
                throw new System.Exception("Configuration error: Datastore not recognized.");

            return datastore;
        }

        

        [HttpGet]
        public IEnumerable<Location> GetAllPossibleDatastoreLocations(int Id)
        {
            logger.Debug("Inside GetAllPossibleDatastoreLocations...");
            logger.Debug("Id = " + Id);

            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();

            Dataset ds = db.Datasets.Find(Id);
            logger.Debug("ds.Datastore = " + ds.DatastoreId);

            var datastore = db.Datastores.Find(ds.DatastoreId);
            logger.Debug("datastore.Name = " + datastore.Name);

            if (datastore == null)
                throw new System.Exception("Configuration error: Datastore not recognized");

            return datastore.Locations;

        }


        [HttpGet]
        public IEnumerable<Field> GetAllDatastoreFields(int Id)
        {
            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();

            var datastore = db.Datastores.Find(Id);
            if (datastore == null)
                throw new System.Exception("Configuration error: Datastore not recognized");

            return datastore.Fields;

        }

        [HttpGet]
        public IEnumerable<Project> GetDatastoreProjects(int Id)
        {
            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();

            var datastore = db.Datastores.Find(Id);
            if (datastore == null)
                throw new System.Exception("Configuration error: Datastore not recognized");

            return datastore.Projects;

        }

        [HttpGet]
        public IEnumerable<Fisherman> GetProjectFishermen(int Id)
        {
            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();

            var project = db.Projects.Find(Id);
            if (project == null)
                throw new System.Exception("Configuration error: Project not recognized");

            return project.Fishermen;
        }

        [HttpGet]
        public IEnumerable<Dataset> GetDatastoreDatasets(int Id)
        {
            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();

            var datastore = db.Datastores.Find(Id);
            if (datastore == null)
                throw new System.Exception("Configuration error: Datastore not recognized");

            return datastore.Datasets;

        }

        [HttpPost]
        public HttpResponseMessage SaveDatasetField(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            User me = AuthorizationManager.getCurrentUser();

            DatasetField df = db.DatasetFields.Find(json.Id.ToObject<int>());

            if (df == null || me == null)
                throw new System.Exception("Configuration error. Please try again.");

            df.Label = json.Label;
            df.Validation = json.Validation;
            df.Rule = json.Rule;
            df.FieldRoleId = json.FieldRoleId.ToObject<int>();
            try
            {
                df.OrderIndex = json.OrderIndex.ToObject<int>();
            }
            catch(System.Exception){
                logger.Debug("didn't have an orderindex.");
            }
            df.ControlType = json.ControlType;
            df.SourceId = json.SourceId.ToObject<int>();
            
            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);


        }

        [HttpPost]
        public HttpResponseMessage UpdateFile(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            if (project == null)
                throw new System.Exception("Configuration error.  Please try again.");

            if (!project.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error:  The user attempting the change is neither an Owner nor an Editor.");

            services.Models.File in_file = json.File.ToObject<services.Models.File>();

            services.Models.File existing_file = project.Files.Where(o => o.Id == in_file.Id).SingleOrDefault();

            if (existing_file == null)
                throw new System.Exception("File not found.");

            existing_file.Title = in_file.Title;
            existing_file.Description = in_file.Description;
            db.Entry(existing_file).State = EntityState.Modified;
            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage DeleteFile(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            if (project == null)
                throw new System.Exception("Configuration error.  Please try again.");

            logger.Debug("The project = " + project);

            if (!project.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");
            else
                logger.Debug("User is authorized.");

            services.Models.File in_file = json.File.ToObject<services.Models.File>();
            logger.Debug("Obtained file from input data...");
            logger.Debug("in_file = " + in_file);

            logger.Debug("Checking list for file name...");
            services.Models.File existing_file = project.Files.Where(o => o.Id == in_file.Id).SingleOrDefault();
            logger.Debug("existing_file.Name = " + existing_file.Name);

            if (existing_file == null)
                throw new System.Exception("File not found.");
            else
                logger.Debug("The file exists.");

            //string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads");
            string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\";
            string theFullPath = root + project.Id + "\\" + existing_file.Name;
            string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);
            logger.Debug("Deleting files from location: " + root);
            logger.Debug(" and the root url = " + rootUrl);
            logger.Debug("theFullPath = " + theFullPath);

            var provider = new MultipartFormDataStreamProvider(root);
            logger.Debug("provider.FileData = " + provider.FileData);

            try
            {
                System.IO.File.Delete(theFullPath);
                //result = ActionController.deleteProjectFile(theFullPath);
                //logger.Debug("Result of delete action:  " + result);
            }
            catch (System.Exception e)
            {
                logger.Debug("Delete action encountered an error...\n" + e.Message);
            }

            var fileToDelete = (from f in db.Files
                                where f.ProjectId == project.Id && f.Name == existing_file.Name
                                select f).FirstOrDefault();
            logger.Debug("Removing " + fileToDelete.Name + " from project " + project.Id + " in the database.");

            db.Files.Remove(fileToDelete);
            logger.Debug("Saving the action");
            db.SaveChanges();

            logger.Debug("Done.");

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage DeleteDatasetFile(JObject jsonData)
        {
            logger.Debug("Inside DeleteDatasetFile...");

            var db = ServicesContext.Current;
            dynamic json = jsonData;
            logger.Debug("json = " + json);

            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            if (project == null)
                throw new System.Exception("Configuration (project) error.  Please try again.");

            logger.Debug("ProjectId = " + project.Id);

            Dataset dataset = db.Datasets.Find(json.DatasetId.ToObject<int>());
            if (dataset == null)
                throw new System.Exception("Configuration (dataset) error.  Please try again.");

            logger.Debug("DatasetId = " + dataset.Id);

            if (!project.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");
            else
                logger.Debug("User is authorized.");

            services.Models.File existing_file = json.File.ToObject<services.Models.File>();
            logger.Debug("Obtained file from input data...");
            logger.Debug("existing_file.Name = " + existing_file.Name);

            //string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads");
            string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\";
            string theFullPath = root + project.Id + "\\D\\" + dataset.Id + "\\" + existing_file.Name;
            //string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);
            logger.Debug("Deleting files from: " + theFullPath);
            //logger.Debug(" and the root url = " + rootUrl);
            //logger.Debug("theFullPath = " + theFullPath);

            var provider = new MultipartFormDataStreamProvider(theFullPath);
            logger.Debug("provider.FileData = " + provider.FileData);

            FileInfo fi = new FileInfo(theFullPath);
            bool exists = fi.Exists;
            if (exists)
            {
                logger.Debug("File exists.  Deleting...");
                System.IO.File.Delete(theFullPath);
            }
            else
            {
                logger.Debug("File does not exist.");
            }

            //var fileToDelete = (from f in db.Files
            //                    where f.ProjectId == project.Id && f.Name == existing_file.Name
            //                    select f).FirstOrDefault();

            int numFiles = (from f in db.Files
                            where f.ProjectId == project.Id && f.DatasetId == dataset.Id && f.Name == existing_file.Name
                            select f).Count();

            if (numFiles > 0)
            {
                var fileToDelete = (from f in db.Files
                                    where f.ProjectId == project.Id && f.DatasetId == dataset.Id && f.Name == existing_file.Name
                                    select f).FirstOrDefault();
                logger.Debug("Removing " + fileToDelete.Name + " from project " + project.Id + ", dataset " + dataset.Id + " in the database.");
                db.Files.Remove(fileToDelete);
                logger.Debug("Saving the action");
                db.SaveChanges();
            }
            else
            {
                logger.Debug("No record in tbl Files for Pid:  " + project.Id + ", dataset " + dataset.Id + ", fileName = " + existing_file.Name);
            }
            
            //logger.Debug("Removing " + fileToDelete.Name + " from project " + project.Id + " in the database.");

            //db.Files.Remove(fileToDelete);
            logger.Debug("Saving the action");
            db.SaveChanges();

            logger.Debug("Done.");

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
       

        [HttpPost]
        public HttpResponseMessage SaveProjectLocation(JObject jsonData)
        {
            logger.Debug("Inside SaveProjectLocation...");
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            logger.Debug("json = " + json);
            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            if (project == null)
                throw new System.Exception("Configuration error.  Please try again.");

            if (!project.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error:  The user attempting the change is neither an Owner nor an Editor.");

            Location location = json.Location.ToObject<Location>();
            location.UserId = me.Id;
            location.ProjectId = project.Id;
            string strLocation = "Id = " + location.Id + "\n" +
                "Projection = " + location.Projection + "\n" +
                "UTMZone = " + location.UTMZone + "\n" +
                "Label = " + location.Label + "\n" +
                "Description = " + location.Description + "\n" +
                "GPSEasting = " + location.GPSEasting + "\n" +
                "GPSNorthing = " + location.GPSNorthing + "\n" +
                "ProjectId = " + location.ProjectId + "\n" +
                "SdeObjectId = " + location.SdeObjectId + "\n" +
                "StudyDesign = " + location.StudyDesign + "\n" +
                "ProjectId = " + location.ProjectId;
            logger.Debug(strLocation);

            //IF the incoming location has an ID then we update, otherwise we create a new project location
            if (location.Id == 0)
            {
                location.CreateDateTime = DateTime.Now;
                project.Locations.Add(location);
                db.SaveChanges();
                logger.Debug("success adding NEW project location!");
            }
            else
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                logger.Debug("success updating EXISTING project location!");
            }

            string result = JsonConvert.SerializeObject(location);

            //TODO: actual error/success message handling
            //string result = "{\"message\": \"Success\"}";

            HttpResponseMessage resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            resp.Content = new System.Net.Http.StringContent(result, System.Text.Encoding.UTF8, "text/plain");  //to stop IE from being stupid.

            return resp;

            //return new HttpResponseMessage(HttpStatusCode.OK);
            
        }

        [HttpPost]
        public HttpResponseMessage DeleteLocation(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();

            Location loc = db.Location.Find(json.LocationId.ToObject<int>());

            if (loc == null)
                throw new System.Exception("Configuration error.");

            if (db.Activities.Where(o => o.LocationId == loc.Id).Count() == 0)
            {
                db.Location.Remove(loc);
                db.SaveChanges();
                logger.Debug("Deleted location "+loc.Id+" because there was no activity.");
            }
            else
            {
                logger.Debug("Tried to delete location " + loc.Id + " when activities exist.");
                throw new System.Exception("Location Delete failed because activities exist!");
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage SaveInstrumentAccuracyCheck(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();

            Instrument instrument = db.Instruments.Find(json.InstrumentId.ToObject<int>());

            if (instrument == null)
                throw new System.Exception("Configuration error.  Please try again.");

            InstrumentAccuracyCheck ac = json.AccuracyCheck.ToObject<InstrumentAccuracyCheck>();

            ac.UserId = me.Id;

            if (ac.Id == 0)
            {
                instrument.AccuracyChecks.Add(ac);
                db.SaveChanges();
            }
            else
            {
                db.Entry(ac).State = EntityState.Modified;
                db.SaveChanges();
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage SaveProjectFisherman(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());

            if (!project.isOwnerOrEditor(me))
            {
                logger.Debug("User is not authorized to make this update.");
                throw new System.Exception("Authorization error.");
            }

            Fisherman fisherman = db.Fishermen.Find(json.Fisherman.Id.ToObject<int>());

            if (project == null || fisherman == null)
                throw new System.Exception("Configuration error.  Please try again.");

            project.Fishermen.Add(fisherman);
            db.SaveChanges();
            logger.Debug("success adding NEW project fisherman!");


            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage SaveProjectInstrument(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());

            if (!project.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error:  The user attempting the change is neither an Owner nor an Editor.");

            Instrument instrument = db.Instruments.Find(json.Instrument.Id.ToObject<int>());

            if (project == null || instrument == null)
                throw new System.Exception("Configuration error.  Please try again.");

            project.Instruments.Add(instrument);
            db.SaveChanges();
            logger.Debug("success adding NEW proejct instrument!");
            

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage RemoveProjectInstrument(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();
            Project p = db.Projects.Find(json.ProjectId.ToObject<int>());

            if (!p.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error:  The user attempting the change is neither an Owner nor an Editor.");

            Instrument instrument = db.Instruments.Find(json.InstrumentId.ToObject<int>());
            if (p == null || instrument == null)
                throw new System.Exception("Configuration error.  Please try again.");

            p.Instruments.Remove(instrument);
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        [HttpPost]
        public HttpResponseMessage RemoveProjectFisherman(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();
            Project p = db.Projects.Find(json.ProjectId.ToObject<int>());

            if (!p.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");

            Fisherman fisherman = db.Fishermen.Find(json.FishermanId.ToObject<int>());
            if (p == null || fisherman == null)
                throw new System.Exception("Configuration error.  Please try again.");

            p.Fishermen.Remove(fisherman);
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        

       

        [HttpPost]
        public HttpResponseMessage SaveInstrument(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();
            Project p = db.Projects.Find(json.ProjectId.ToObject<int>());
            if (p == null)
                throw new System.Exception("Configuration error.  Please try again.");

            if (!p.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error:  The user attempting the change is neither an Owner nor an Editor.");

            Instrument instrument = json.Instrument.ToObject<Instrument>();
            instrument.OwningDepartmentId = json.Instrument.OwningDepartmentId.ToObject<int>();

            logger.Debug("The id == " + instrument.OwningDepartmentId);

            //if there is an instrument id already set, then we'll just update the instrument and call it good.
            //  otherwise we'll create the new instrument and a relationship to the project.

            if (instrument.Id == 0)
            {
                instrument.UserId = me.Id;
                p.Instruments.Add(instrument);
                logger.Debug("created new instrument");
            }
            else
            {
                db.Entry(instrument).State = EntityState.Modified;
                logger.Debug("updated existing instrument");
            }
            
            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage SaveFisherman(JObject jsonData)
        {
            if (debugMode) logger.Debug("Inside SaveFisherman...");
            var db = ServicesContext.Current;
            if (debugMode) logger.Debug("db = " + db);

            dynamic json = jsonData;
            if (debugMode) logger.Debug("json = " + json);

            User me = AuthorizationManager.getCurrentUser();
            if (debugMode) logger.Debug("me = " + me);

            int pId = json.ProjectId.ToObject<int>(); // Getting stuck on this line.
            if (debugMode) logger.Debug("pId = " + pId);

            Project p = db.Projects.Find(pId);
            if (debugMode) logger.Debug("p = " + p);
            if (p == null)
                throw new System.Exception("Configuration error.  Please try again.");

            if (!p.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");

            Fisherman f = json.Fisherman.ToObject<Fisherman>();

            if (debugMode) logger.Debug("fisherman.FirstName = " + f.FirstName);

            DateTime? theDateInactive = null;
            if (debugMode) logger.Debug("theDateInactive = " + theDateInactive);

            f.DateInactive = theDateInactive;
            if (debugMode) logger.Debug("f.DateInactive = " + f.DateInactive);

            if (debugMode) logger.Debug(
                "f.FirstName = " + f.FirstName + "\n" +
                "f.Aka = " + f.Aka + "\n" +
                "f.LastName = " + f.LastName + "\n" +
                "f.FullName = " + f.LastName + "\n" +
                "f.PhoneNumber = " + f.PhoneNumber + "\n" +
                "f.Comments = " + f.FishermanComments + "\n" +
                "f.StatusId = " + f.StatusId + "\n" +
                "f.DateAdded = " + f.DateAdded + "\n" +
                "f.DateInactive = " + f.DateInactive + "\n" +
                "f.OkToCallId = " + f.OkToCallId + "\n"
                );

            if (f.Id == 0)
            {
                p.Fishermen.Add(f);
                logger.Debug("created new fisherman");
            }
            else
            {
                db.Entry(f).State = EntityState.Modified;
                logger.Debug("updated existing fisherman");
            }

            db.SaveChanges();
            if (debugMode) logger.Debug("Just saved the DB changes.");

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage AddMasterFieldToDataset(JObject jsonData)
        {
            logger.Debug("Inside AddMasterFieldToDataset...");
            var db = ServicesContext.Current;
            dynamic json = jsonData;

            int DatasetId = json.DatasetId.ToObject<int>();
            logger.Debug("DatasetId = " + DatasetId);
            var dataset = db.Datasets.Find(DatasetId);

            int FieldId = json.FieldId.ToObject<int>();
            logger.Debug("FieldId = " + FieldId);
            var field = db.Fields.Find(FieldId);

            DatasetField df = new DatasetField();

            df.DatasetId = dataset.Id;
            df.FieldId = field.Id;
            df.FieldRoleId = FieldRole.DETAIL;
            df.CreateDateTime = DateTime.Now;
            df.Label = field.Name;
            df.DbColumnName = field.DbColumnName;
            df.SourceId = 1;
            df.ControlType = field.ControlType;

            db.DatasetFields.Add(df);
            db.SaveChanges();
            logger.Debug("Added/saved the field: " + field.DbColumnName);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage DeleteDatasetField(JObject jsonData)
        {
            logger.Debug("Inside DatastoreController.cs, DeleteDatasetField...");
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            //logger.Debug("json = " + json);

            int DatasetId = json.DatasetId.ToObject<int>();
            logger.Debug("DatasetId = " + DatasetId);
            var dataset = db.Datasets.Find(DatasetId);
            if (dataset == null)
                throw new System.Exception("Dataset could not be found: " + DatasetId);

            int FieldId = json.FieldId.ToObject<int>();
            logger.Debug("FieldId = " + FieldId);

            //var field = db.DatasetFields.Find(FieldId); // Original line
            List<DatasetField> datasetFieldRecords = (from item in db.DatasetFields
                                        where item.DatasetId == DatasetId && item.FieldId == FieldId
                                        select item).ToList();

            if (datasetFieldRecords.Count == 0)
                throw new System.Exception("Field could not be retrieved for dataset: " + DatasetId);
            else if (datasetFieldRecords.Count > 1)
                logger.Debug("Found " + datasetFieldRecords.Count + " records (fields).");
            else
                logger.Debug("Found field.");

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                //var query = "DELETE FROM DatasetFields where DatasetId = " + dataset.Id + " and FieldId = " + field.Id; // Original line
                var query = "";
                foreach (var item in datasetFieldRecords)
                {
                    query = "DELETE FROM DatasetFields where Id = " + item.Id;
                    logger.Debug("SQL command = " + query);
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        //logger.Debug(query);
                        //cmd.ExecuteNonQuery(); // Original line
                        try
                        {
                            cmd.ExecuteNonQuery();
                            logger.Debug("Delete action done...");
                        }
                        catch (System.Exception e)
                        {
                            logger.Debug("Delete action failed:  " + e.Message);
                        }
                    }
                }

            }            

            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        [HttpPost]
        public HttpResponseMessage SaveMasterField(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            User me = AuthorizationManager.getCurrentUser();

            Field df = db.Fields.Find(json.Id.ToObject<int>());

            if (df == null || me == null)
                throw new System.Exception("Configuration error. Please try again.");

            
            
            df.Name = json.Name;
            df.Validation = json.Validation;
            df.Rule = json.Rule;
            df.Units = json.Units;
            df.TechnicalName = json.TechnicalName;
            df.DbColumnName = json.DbColumnName;
            df.DataType = json.DataType;
            df.ControlType = json.ControlType;
            df.PossibleValues = json.PossibleValues;
            df.Description = json.Description;

            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);


        }

        private string padNumber(string strNumber)
        {
            if (strNumber.Length < 10)
                strNumber = "0" + strNumber;

            return strNumber;
        }

       
    }
}
