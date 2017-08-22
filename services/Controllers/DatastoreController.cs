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
using System.Net.Mail;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
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
        public IEnumerable<Subproject_Crpp> GetSubprojects()
        {
            var db = ServicesContext.Current;
            logger.Info("Inside DatastoreController, getting subprojects...");

            /*  These are the results we want...
             *  Subproject records sorted in descending order by EffDt (most recent update on top).
             *  Then the Subprojects associated CorrespondenceEvents also sorted descending (most recent update on top).
             *  
             *  In order to achieve the goal, referring to the following article...
             *  http://stackoverflow.com/questions/30869122/how-to-order-by-list-inside-list-using-linq-in-c-sharp
             *  we had to sort each list separately.
             *  Therefore, we sort the Subprojects via our select statement.
             *  Then we walk the list, and sort each Subprojects associated CorrespondenceEvents.
             */

            // ***** This works.
            List<Subproject_Crpp> s = (from item in db.Subproject_Crpp()
                                  where item.Id > 1
                                  orderby item.EffDt descending
                                  select item).ToList();
            // *****
            logger.Info("Got s");

            foreach (var ce in s)
            {
                //logger.Debug("ce = " + ce.ProjectName);
                ce.CorrespondenceEvents = ce.CorrespondenceEvents.OrderByDescending(x => x.EffDt).ToList();
            }


            return s.AsEnumerable();

            /******************************************/
            // This part works, manually testing the function.
            /*Subproject sTest = new Subproject();
            sTest.Id = 1;
            sTest.ProjectName = "PN1";
            sTest.Agency = "A1";
            sTest.ProjectProponent = "PP1";
            sTest.TrackingNumber = "TN1";
            sTest.ActionNeeded = "AN1";
            sTest.YearDate = "20160304_103015";
            sTest.Closed = "No";
            sTest.EffDt = DateTime.Now;
            sTest.ByUserId = 1081;

            List<Subproject> sp = new List<Subproject>();
            sp.Add(sTest);

            return sp.AsEnumerable();
            */
            /******************************************/
        }

        [HttpGet]
        public IEnumerable<Subproject_Hab> GetHabSubprojects()
        //public IEnumerable<Subproject_Hab> GetHabSubprojects(int Id)
        {
            logger.Info("Inside DatastoreController, GetHabSubprojects...");
            //logger.Debug("ProjectId = " + Id);
            var db = ServicesContext.Current;

            /*  These are the results we want...
             *  Subproject records sorted in descending order by EffDt (most recent update on top).
             *  Then the Subprojects associated CorrespondenceEvents also sorted descending (most recent update on top).
             *  
             *  In order to achieve the goal, referring to the following article...
             *  http://stackoverflow.com/questions/30869122/how-to-order-by-list-inside-list-using-linq-in-c-sharp
             *  we had to sort each list separately.
             *  Therefore, we sort the Subprojects via our select statement.
             *  Then we walk the list, and sort each Subprojects associated CorrespondenceEvents.
             */

            // ***** This works.
            List<Subproject_Hab> s = (from item in db.Subproject_Hab()
                                      where item.Id > 1
                                      //where item.Id > 1 && item.ProjectId == Id
                                      orderby item.EffDt descending
                                      select item).ToList();
            // *****
            logger.Info("Got s");

            
            foreach (var hi in s)
            {
                logger.Debug("hi = " + hi.ProjectName);
                //hi.HabitatItems = hi.HabitatItems.ToList();
                hi.HabitatItems = hi.HabitatItems.OrderByDescending(x => x.EffDt).ToList();
            }
            


            return s.AsEnumerable();

            /******************************************/
            // This part works, manually testing the function.
            /*Subproject_Hab sTest = new Subproject_Hab();
            sTest.Id = 1;
            sTest.ProjectName = "PN1";
            sTest.ProjectDescription = "PD1";
            sTest.ProjectStartDate = DateTime.Now;
            sTest.ProjectEndDate = DateTime.Now.AddDays(14);
            sTest.FirstFoods = "FF1";
            sTest.RiverVisionTouchstone = "RVT1";
            sTest.HabitatObjectives = "HO1";
            sTest.NoaaEcologicalConcernsSubcategories = "NECS1";
            sTest.NoaaEcologicalConcerns = "NEC1";
            sTest.LimitingFactors = "LF1";
            sTest.Funding = "F1";
            sTest.Staff = "S1";
            sTest.Collaborators = "C1";
            sTest.Comments = "Comments1";
            sTest.EffDt = DateTime.Now;
            sTest.ByUserId = 1081;

            List<Subproject_Hab> sp = new List<Subproject_Hab>();
            sp.Add(sTest);

            return sp.AsEnumerable();
            */
            /******************************************/
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

        [HttpGet]
        public IEnumerable<Subproject_Hab> GetHabSubproject(int Id)
        {
            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();

            List<Subproject_Hab> s = (from item in db.Subproject_Hab()
                                      where item.Id == Id
                                      select item).ToList();

            return s.AsEnumerable();

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
        public HttpResponseMessage DeleteCorresEventFile(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            logger.Debug("json = " + json);

            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            if (project == null)
                throw new System.Exception("Configuration error.  Please try again.");

            logger.Debug("The project = " + project);

            if (project == null)
                throw new System.Exception("Configuration error.  Please try again.");

            if (!project.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");
            else
                logger.Debug("User is authorized.");

            int subprojectId = json.SubprojectId.ToObject<int>();
            logger.Debug("subprojectId = " + subprojectId);
            int corresEventId = json.CeId.ToObject<int>();
            logger.Debug("corresEventId = " + corresEventId);

            services.Models.File existing_file = json.File.ToObject<services.Models.File>();
            logger.Debug("Obtained file from input data...");
            logger.Debug("existing_file.Name = " + existing_file.Name);

            /*if (existing_file == null)
                throw new System.Exception("File not found.");
            else
                logger.Debug("The file exists.");
             */

            //string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads");
            //string root = System.Configuration.ConfigurationManager.AppSettings["PathToCrppProjectDocuments"] + ("\\uploads\\subprojects");
            //string root = System.Configuration.ConfigurationManager.AppSettings["PathToCrppProjectDocuments"];
            string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\";
            string theFullPath = root + project.Id  + "\\S\\" + subprojectId + "\\" + existing_file.Name;
            //string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);
            //logger.Debug("Deleting files from location: " + root + "\\" + subprojectId);
            //logger.Debug(" and the root url = " + rootUrl);
            //logger.Debug("theFullPath = " + theFullPath);

            //var provider = new MultipartFormDataStreamProvider(root);
            var provider = new MultipartFormDataStreamProvider(theFullPath);
            //logger.Debug("provider = " + provider);

            logger.Debug("About to delete the file:  " + theFullPath);

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

            //result = ActionController.deleteProjectFile(theFullPath);
            //logger.Debug("Result of delete action:  " + result);

            int numFiles = (from f in db.Files
                            where f.ProjectId == project.Id && f.Subproject_CrppId == subprojectId && f.Name == existing_file.Name
                            select f).Count();

            if (numFiles > 0)
            {
                var fileToDelete = (from f in db.Files
                                    where f.ProjectId == project.Id && f.Subproject_CrppId == subprojectId && f.Name == existing_file.Name
                                    select f).FirstOrDefault();
                logger.Debug("Removing " + fileToDelete.Name + " from subproject " + subprojectId + " in the database.");
                db.Files.Remove(fileToDelete);
                logger.Debug("Saving the action");
                db.SaveChanges();
            }
            else
            {
                logger.Debug("No record in tbl Files for Pid:  " + project.Id + ", SubpId = " + subprojectId + ", fileName = " + existing_file.Name);
            }
            logger.Debug("Done.");

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage DeleteHabitatItemFile(JObject jsonData)
        {
            logger.Debug("Inside DeleteHabitatItemFile...");

            var db = ServicesContext.Current;
            dynamic json = jsonData;
            logger.Debug("json = " + json);

            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            if (project == null)
                throw new System.Exception("Configuration error.  Please try again.");

            logger.Debug("The project = " + project);

            if (project == null)
                throw new System.Exception("Configuration error.  Please try again.");

            if (!project.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");
            else
                logger.Debug("User is authorized.");

            int subprojectId = json.SubprojectId.ToObject<int>();
            logger.Debug("subprojectId = " + subprojectId);
            int habItemId = json.HiId.ToObject<int>();
            logger.Debug("habItemId = " + habItemId);

            services.Models.File existing_file = json.File.ToObject<services.Models.File>();
            logger.Debug("Obtained file from input data...");
            logger.Debug("existing_file.Name = " + existing_file.Name);

            /*if (existing_file == null)
                throw new System.Exception("File not found.");
            else
                logger.Debug("The file exists.");
             */

            //string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads");
            //string root = System.Configuration.ConfigurationManager.AppSettings["PathToHabitatProjectDocuments"] + ("\\uploads\\subprojects");
            string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\";
            string theFullPath = root + project.Id + "\\S\\" + subprojectId + "\\" + existing_file.Name;
            //string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);
            //logger.Debug("Deleting files from location: " + root + "\\" + subprojectId);
            //logger.Debug(" and the root url = " + rootUrl);
            //logger.Debug("theFullPath = " + theFullPath);

            //var provider = new MultipartFormDataStreamProvider(root);
            var provider = new MultipartFormDataStreamProvider(theFullPath);
            //logger.Debug("provider = " + provider);

            logger.Debug("About to delete the file:  " + theFullPath);

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

            //result = ActionController.deleteProjectFile(theFullPath);
            //logger.Debug("Result of delete action:  " + result);

            // Using a list, but there should only be one.
            //List<int> FileId = (from sf in db.SubprojectFiles
            //                    where sf.ProjectId == project.Id && sf.SubprojectId == subprojectId && sf.FileName == existing_file.Name
            //                    select sf.Id).ToList();

            //foreach (var aFileId in FileId)
            //{
            //    SubprojectFiles spFile = db.SubprojectFiles.Find(aFileId);
            //    db.SubprojectFiles.Remove(spFile);
            //    db.SaveChanges();
            //    logger.Debug("Removed file with ID = " + aFileId + " from table SubprojectFiles");
            //}

            int numFiles = (from sf in db.Files
                            where sf.ProjectId == project.Id && sf.Subproject_CrppId == subprojectId && sf.Name == existing_file.Name
                            select sf).Count();

            if (numFiles > 0)
            {
                var fileToDelete = (from f in db.Files
                                    where f.ProjectId == project.Id && f.Subproject_CrppId == subprojectId && f.Name == existing_file.Name
                                    select f).FirstOrDefault();

                logger.Debug("Removing " + fileToDelete.Name + " from subproject " + subprojectId + " in the database.");
                db.Files.Remove(fileToDelete);
                logger.Debug("Saving the action");
                db.SaveChanges();
            }
            else
            {
                logger.Debug("No record in tbl Files for Pid:  " + project.Id + ", SubpId = " + subprojectId + ", fileName = " + existing_file.Name);
            }
            logger.Debug("Done.");

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage DeleteHabSubprojectFile(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            logger.Debug("json = " + json);

            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            if (project == null)
                throw new System.Exception("Configuration error.  Please try again.");

            logger.Debug("The project = " + project);

            if (project == null)
                throw new System.Exception("Configuration error.  Please try again.");

            if (!project.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");
            else
                logger.Debug("User is authorized.");

            int subprojectId = json.SubprojectId.ToObject<int>();
            logger.Debug("subprojectId = " + subprojectId);
            //int habItemId = json.HiId.ToObject<int>();
            //logger.Debug("habItemId = " + habItemId);

            services.Models.File existing_file = json.File.ToObject<services.Models.File>();
            logger.Debug("Obtained file from input data...");
            logger.Debug("existing_file.Name = " + existing_file.Name);

            /*if (existing_file == null)
                throw new System.Exception("File not found.");
            else
                logger.Debug("The file exists.");
             */

            //string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads");
            //string root = System.Configuration.ConfigurationManager.AppSettings["PathToHabitatProjectDocuments"] + ("\\uploads\\subprojects");
            //string root = System.Configuration.ConfigurationManager.AppSettings["PathToHabitatProjectDocuments"];
            string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\";
            //string theFullPath = root + "\\" + subprojectId + "\\" + existing_file.Name;
            string theFullPath = root + project.Id + "\\S\\" + subprojectId + "\\" + existing_file.Name;
            //string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);
            //logger.Debug("Deleting files from location: " + root + "\\" + subprojectId);
            //logger.Debug(" and the root url = " + rootUrl);
            //logger.Debug("theFullPath = " + theFullPath);

            //var provider = new MultipartFormDataStreamProvider(root);
            var provider = new MultipartFormDataStreamProvider(theFullPath);
            //logger.Debug("provider = " + provider);

            logger.Debug("About to delete the file:  " + theFullPath);

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

            //result = ActionController.deleteProjectFile(theFullPath);
            //logger.Debug("Result of delete action:  " + result);

            // Using a list, but there should only be one.
            //List<int> FileId = (from sf in db.SubprojectFiles
            //                    where sf.ProjectId == project.Id && sf.SubprojectId == subprojectId && sf.FileName == existing_file.Name
            //                    select sf.Id).ToList();
            List<int> FileId = (from sf in db.Files
                                where sf.ProjectId == project.Id && sf.Subproject_CrppId == subprojectId && sf.Name == existing_file.Name
                                select sf.Id).ToList();

            //foreach (var aFileId in FileId)
            //{
            //    //SubprojectFiles spFile = db.SubprojectFiles.Find(aFileId);
            //    services.Models.File spFile = db.Files.Find(aFileId);
            //    db.Files.Remove(spFile);
            //    db.SaveChanges();
            //    logger.Debug("Removed file with ID = " + aFileId + " from table Files");
            //}

            int numFiles = (from sf in db.Files
                            where sf.ProjectId == project.Id && sf.Name == existing_file.Name
                            select sf).Count();

            if (numFiles > 0)
            {
                var fileToDelete = (from f in db.Files
                                    where f.ProjectId == project.Id && f.Name == existing_file.Name
                                    select f).FirstOrDefault();

                logger.Debug("Removing " + fileToDelete.Name + " from subproject " + subprojectId + " in the database.");
                db.Files.Remove(fileToDelete);
                logger.Debug("Saving the action");
                db.SaveChanges();
            }
            else
            {
                logger.Debug("No record in tbl Files for Pid:  " + project.Id + ", SubpId = " + subprojectId + ", fileName = " + existing_file.Name);
            }
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
        public HttpResponseMessage RemoveSubproject(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();
            Project p = db.Projects.Find(json.ProjectId.ToObject<int>());

            if (!p.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");

            Subproject_Crpp crppSubproject = db.Subproject_Crpp().Find(json.SubprojectId.ToObject<int>());
            if (p == null || crppSubproject == null)
                throw new System.Exception("Configuration error.  Please try again.");

            //string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads/subprojects");
            //string root = System.Configuration.ConfigurationManager.AppSettings["PathToCrppProjectDocuments"] + ("\\uploads\\subprojects");
            string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\";
            logger.Debug("root = " + root);

            string strSubprojectsPath = root + p.Id + "\\S\\" + crppSubproject.Id;
            if (debugMode) logger.Debug("The path for the subproject is:  " + strSubprojectsPath);

            if (Directory.Exists(strSubprojectsPath))
            {
                System.IO.Directory.Delete(strSubprojectsPath, true);
                if (debugMode) logger.Debug("Just deleted documents folder and contents for this subproject:  " + crppSubproject.Id);
            }
            else
            {
                logger.Debug("Could not find folder: " + strSubprojectsPath);
            }

            db.Subproject_Crpp().Remove(crppSubproject);
            if (debugMode) logger.Debug("Just removed this subproject from table CrppSubprojects:  " + crppSubproject.Id);

            //db.CrppSubprojects.State = EntityState.Modified;
            db.SaveChanges();
            if (debugMode) logger.Debug("Changes saved...");

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        [HttpPost]
        public HttpResponseMessage RemoveHabSubproject(JObject jsonData)
        {
            logger.Debug("Inside RemoveHabSubproject...");

            var db = ServicesContext.Current;
            dynamic json = jsonData;
            logger.Debug("json = " + json);

            User me = AuthorizationManager.getCurrentUser();
            logger.Debug("Got me...");
            Project p = db.Projects.Find(json.ProjectId.ToObject<int>());
            logger.Debug("p.Name = " + p.Name);

            if (!p.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");
            logger.Debug("Checked OwnerOrEditor");

            int intSubprojectId = json.SubprojectId.ToObject<int>();
            logger.Debug("SubprojectId to delete:  " + intSubprojectId);

            Subproject_Hab habSubproject = db.Subproject_Hab().Find(intSubprojectId);
            if (p == null || habSubproject == null)
                throw new System.Exception("Configuration error.  Please try again.");
            logger.Debug("habSubproject.ProjectName = " + habSubproject.ProjectName);

            int intLocationId = json.LocationId.ToObject<int>();
            Location location = db.Location.Find(intLocationId);
            if (location == null)
                logger.Debug("Could not find the location in the Locations table.");
            else
            {
                try
                {
                    p.Locations.Remove(location);
                    logger.Debug("Just removed this location:  " + intLocationId + " from LocationProjects.");
                }
                catch (System.Exception)
                {
                    logger.Debug("Could not remove the project location.");
                }

                try
                {
                    db.Location.Remove(location);
                    logger.Debug("Just removed this location:  " + location + " from Locations");
                }
                catch (System.Exception)
                {
                    logger.Debug("Could not remove the location.");
                }
            }

            //*** Remove the associated records from the Funding table.
            List<Funding> fundersList = (from funder in db.Funding
                                         where funder.ProjectId == p.Id && funder.SubprojectId == habSubproject.Id
                                         select funder).ToList();

            foreach (var funder in fundersList)
            {
                db.Funding.Remove(funder);
                logger.Debug("Removed funder: " + funder.Name);
            }
            //***

            //*** Remove the associated records from the Collaborators table.
            List<Collaborator> CollaboratorsList = (from collaborator in db.Collaborators
                                                    where collaborator.ProjectId == p.Id && collaborator.SubprojectId == habSubproject.Id
                                                    select collaborator).ToList();

            foreach (var collaborator in CollaboratorsList)
            {
                db.Collaborators.Remove(collaborator);
                logger.Debug("Removed collaborator: " + collaborator.Name);
            }
            //***

            //List<SubprojectFiles> subprojectFilesList;
            List<services.Models.File> subprojectFilesList;
            //*** Check the SubprojectFiles table; it will give us the ID for record in the Files table.
            //subprojectFilesList = (from file in db.SubprojectFiles
            //                       where file.ProjectId == p.Id && file.SubprojectId == habSubproject.Id && file.FeatureImage == 1
            //                       select file).ToList();

            subprojectFilesList = (from file in db.Files
                                   where file.ProjectId == p.Id && file.Subproject_CrppId == habSubproject.Id && file.FeatureImage == 1
                                   select file).ToList();
            //***

            List<services.Models.File> FileList;
            //*** Remove the record from the Files table.
            foreach (var spFile in subprojectFilesList)
            {
                FileList = (from file in db.Files
                            where file.Id == spFile.Id
                            select file).ToList();

                foreach (var file in FileList)
                {
                    db.Files.Remove(file);
                    logger.Debug("Removed file: " + file.Name);
                }
            }
            db.SaveChanges();
            //***

            //*** Now remove any associated records (FeatureImage and any others) from the SubprojectFiles table.
            //subprojectFilesList = (from file in db.SubprojectFiles
            subprojectFilesList = (from file in db.Files
                                   where file.ProjectId == p.Id && file.Subproject_CrppId == habSubproject.Id
                                   select file).ToList();

            foreach (var spFile in subprojectFilesList)
            {
                //db.SubprojectFiles.Remove(spFile);
                db.Files.Remove(spFile);
                //logger.Debug("Removed spFile: " + spFile.FileName);
                logger.Debug("Removed spFile: " + spFile.Name);
            }
            //***


            //string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads/subprojects");
            //string root = System.Web.HttpContext.Current.Server.MapPath("~/");
            string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\";
            logger.Debug("root = " + root);

            string strSubprojectsPath = root + p.Id + "\\S\\" + habSubproject.Id;
            //string strSubprojectsPath = root + habSubproject.Id;
            if (debugMode) logger.Debug("The path for the subproject is:  " + strSubprojectsPath);

            if (Directory.Exists(strSubprojectsPath))
            {
                System.IO.Directory.Delete(strSubprojectsPath, true);
                if (debugMode) logger.Debug("Just deleted documents folder and contents for this subproject:  " + habSubproject.Id);
            }
            else
            {
                logger.Debug("Could not find folder: " + strSubprojectsPath);
            }

            db.Subproject_Hab().Remove(habSubproject);
            if (debugMode) logger.Debug("Just removed this subproject from table Subproject_Hab:  " + habSubproject.Id);

            db.SaveChanges();
            if (debugMode) logger.Debug("Changes saved...");

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        [HttpPost]
        public HttpResponseMessage RemoveCorrespondenceEvent(JObject jsonData)
        {
            logger.Debug("Inside RemoveCorrespondenceEvent...");
            bool blnEventFilesPresent = true;

            var db = ServicesContext.Current;
            logger.Debug("Set database...");

            dynamic json = jsonData;
            logger.Debug("json = " + json);
            User me = AuthorizationManager.getCurrentUser();
            Project p = db.Projects.Find(json.ProjectId.ToObject<int>());
            logger.Debug("ProjectId = " + p.Id);

            if (!p.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");

            Subproject_Crpp subproject = db.Subproject_Crpp().Find(json.SubprojectId.ToObject<int>());
            if (p == null || subproject == null)
                throw new System.Exception("Configuration error.  Please try again.");

            logger.Debug("crppSubprojectId = " + subproject.Id);

            CorrespondenceEvents correspondenceEvent = db.CorrespondenceEvents().Find(json.CorrespondenceEventId.ToObject<int>());
            if (p == null || correspondenceEvent == null)
                throw new System.Exception("Configuration error.  Please try again.");

            logger.Debug("correspondenceEvent = " + correspondenceEvent.Id);

            string strDatastoreTablePrefix = json.DatastoreTablePrefix.ToObject<string>();

            if (correspondenceEvent.EventFiles != null)
            {
                logger.Debug("EventFiles = " + correspondenceEvent.EventFiles);
                // If there were no attached files, the contents will not be null, but they won't have any files either.
                // The contents will be [];
                if (correspondenceEvent.EventFiles.Length < 3)
                    blnEventFilesPresent = false;

                if (blnEventFilesPresent)
                {
                    // We have a json object (EventFiles) within a json object.  So let's take EventFiles apart, to get the file name.
                    List<string> strFileList = correspondenceEvent.EventFiles.Split(',').ToList();
                    string strFile = "";
                    int intColonLocation = -1;
                    int intLastBracketLocation = -1;
                    int intLastDblQuoteLocation = -1;
                    int intFileNameLength = 0;
                    //string thePath = System.Configuration.ConfigurationManager.AppSettings["PathToServices"] + "uploads\\subprojects\\" + subproject.Id + "\\";
                    string thePath = "";
                    thePath = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\" + p.Id + "\\S\\" + subproject.Id + "\\";

                    if (Directory.Exists(thePath))
                    {
                        string strFilePath = "";
                        foreach (var item in strFileList)
                        {
                            //logger.Debug("item = " + item);
                            intColonLocation = item.IndexOf(":");
                            //logger.Debug("intColonLocation = " + intColonLocation);

                            intLastBracketLocation = item.IndexOf("]");
                            if (intLastBracketLocation == -1)
                                intLastDblQuoteLocation = item.Length - 1;
                            else
                                intLastDblQuoteLocation = item.Length - 2;

                            //logger.Debug("intLastDblQuoteLocation = " + intLastDblQuoteLocation);

                            intFileNameLength = (intLastDblQuoteLocation - 1) - (intColonLocation + 2);
                            //logger.Debug("intFileNameLength = " + intFileNameLength);

                            strFile = item.Substring(intColonLocation + 2, intFileNameLength);
                            //logger.Debug("strFile = " + strFile);

                            strFilePath = thePath + strFile;
                            logger.Debug("The files location = " + strFilePath);

                            System.IO.File.Delete(strFilePath);
                            logger.Debug("Deleted the file...");
                            strFilePath = "";

                            // Now let's remove the file from the Files table in the database.
                            List<Models.File> listOfFiles = (from aFile in db.Files
                                                             where aFile.Subproject_CrppId == subproject.Id && aFile.Name == strFile
                                                             select aFile).ToList();
                            foreach (var fileRecord in listOfFiles)
                            {
                                db.Files.Remove(db.Files.Single(ff => ff.Id == fileRecord.Id));
                            }
                        }
                    }
                    else
                    {
                        logger.Debug("Could not find folder: " + thePath);
                    }
                }
            }

            /*string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads/subprojects");
            logger.Debug("root = " + root);

            string strSubprojectsPath = root + "\\" + crppCorrespondenceEvent.Id;
            if (debugMode) logger.Debug("The path for the subproject is:  " + strSubprojectsPath);

            System.IO.Directory.Delete(strSubprojectsPath, true);
            if (debugMode) logger.Debug("Just deleted documents folder and contents for this subproject:  " + crppCorrespondenceEvent.Id);
            */

            db.CorrespondenceEvents().Remove(correspondenceEvent);
            if (debugMode) logger.Debug("Just removed this event from table CorrespondenceEvents:  " + correspondenceEvent.Id);

            //db.CrppSubprojects.State = EntityState.Modified;
            db.SaveChanges();
            if (debugMode) logger.Debug("Changes saved...");

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage RemoveHabitatItem(JObject jsonData)
        {
            logger.Debug("Inside RemoveHabitatItem...");
            bool blnEventFilesPresent = true;

            var db = ServicesContext.Current;
            logger.Debug("Set database...");

            dynamic json = jsonData;
            logger.Debug("json = " + json);
            User me = AuthorizationManager.getCurrentUser();
            Project p = db.Projects.Find(json.ProjectId.ToObject<int>());
            logger.Debug("ProjectId = " + p.Id);

            if (!p.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");

            Subproject_Hab subproject = db.Subproject_Hab().Find(json.SubprojectId.ToObject<int>());
            if (p == null || subproject == null)
                throw new System.Exception("Configuration error.  Please try again.");

            logger.Debug("habSubprojectId = " + subproject.Id);
            
            int intHabitatItemId = json.HabitatItemId.ToObject<int>();
            logger.Debug("intHabitatItemId = " + intHabitatItemId);

            if (db.HabitatItem().Any(u => u.Id == intHabitatItemId))
            {
                logger.Debug("The Habitat Item exists...");
                //HabitatItem habitatItem = db.HabitatItems.Find(json.HabitatItemId.ToObject<int>());
                HabitatItem habitatItem = db.HabitatItem().Find(intHabitatItemId);
                if (p == null || habitatItem == null)
                    throw new System.Exception("Configuration error.  Please try again.");

                logger.Debug("habitatItem = " + habitatItem.ItemName);

                //string strDatastoreTablePrefix = json.DatastoreTablePrefix.ToObject<string>();

                if (habitatItem.ItemFiles != null)
                {
                    logger.Debug("ItemFiles = " + habitatItem.ItemFiles);
                    // If there were no attached files, the contents will not be null, but they won't have any files either.
                    // The contents will be [];
                    if (habitatItem.ItemFiles.Length < 3)
                        blnEventFilesPresent = false;

                    if (blnEventFilesPresent)
                    {
                        // We have a json object (EventFiles) within a json object.  So let's take EventFiles apart, to get the file name.
                        List<string> strFileList = habitatItem.ItemFiles.Split(',').ToList();
                        string strFile = "";
                        int intColonLocation = -1;
                        int intLastBracketLocation = -1;
                        int intLastDblQuoteLocation = -1;
                        int intFileNameLength = 0;
                        //string thePath = System.Configuration.ConfigurationManager.AppSettings["PathToServices"] + "uploads\\subprojects\\" + subproject.Id + "\\";
                        string thePath = "";

                        //thePath = System.Configuration.ConfigurationManager.AppSettings["PathToHabitatProjectDocuments"] + "\\" + subproject.Id + "\\";
                        thePath = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\" + p.Id + "\\S\\" + subproject.Id + "\\";

                        if (Directory.Exists(thePath))
                        {
                            string strFilePath = "";
                            foreach (var item in strFileList)
                            {
                                //logger.Debug("item = " + item);
                                intColonLocation = item.IndexOf(":");
                                //logger.Debug("intColonLocation = " + intColonLocation);

                                intLastBracketLocation = item.IndexOf("]");
                                if (intLastBracketLocation == -1)
                                    intLastDblQuoteLocation = item.Length - 1;
                                else
                                    intLastDblQuoteLocation = item.Length - 2;

                                //logger.Debug("intLastDblQuoteLocation = " + intLastDblQuoteLocation);

                                intFileNameLength = (intLastDblQuoteLocation - 1) - (intColonLocation + 2);
                                //logger.Debug("intFileNameLength = " + intFileNameLength);

                                strFile = item.Substring(intColonLocation + 2, intFileNameLength);
                                //logger.Debug("strFile = " + strFile);

                                strFilePath = thePath + strFile;
                                logger.Debug("The files location = " + strFilePath);

                                System.IO.File.Delete(strFilePath);
                                logger.Debug("Deleted the file...");
                                strFilePath = "";

                                // First, get the ID number of the file in the Files table.
                                List<Models.File> listOfFiles = (from aFile in db.Files
                                                                 where aFile.ProjectId == p.Id && aFile.Subproject_CrppId == subproject.Id && aFile.Name == strFile
                                                                 select aFile).ToList();

                                // Remove the file from SubprojectFiles table.
                                //foreach (var fileRecord in listOfFiles)
                                //{
                                // Locate the related record(s) in the SubprojectFiles table.
                                //    List<SubprojectFiles> sfList = (from aFile in db.SubprojectFiles
                                //                                    where aFile.ProjectId == p.Id && aFile.SubprojectId == subproject.Id && aFile.FileId == fileRecord.Id && aFile.FileName == fileRecord.Name
                                //                                    select aFile).ToList();

                                // Remove the file from the SubprojectFiles table.
                                //foreach (var singleFile in sfList)
                                //{
                                //    db.SubprojectFiles.Remove(singleFile);
                                //    logger.Debug("Removed file " + singleFile.FileName + " from table SubprojectFiles.");
                                //}
                                //db.SaveChanges();
                                //logger.Debug("Saved changes to table SubprojectFiles.");
                                //}

                                // Now let's remove the file from the Files table in the database.
                                foreach (var fileRecord in listOfFiles)
                                {
                                    db.Files.Remove(db.Files.Single(ff => ff.Id == fileRecord.Id));
                                }

                            }
                        }
                        else
                        {
                            logger.Debug("Could not find folder: " + thePath);
                        }
                    }
                }


                /*string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads/subprojects");
                logger.Debug("root = " + root);

                string strSubprojectsPath = root + "\\" + crppCorrespondenceEvent.Id;
                if (debugMode) logger.Debug("The path for the subproject is:  " + strSubprojectsPath);

                System.IO.Directory.Delete(strSubprojectsPath, true);
                if (debugMode) logger.Debug("Just deleted documents folder and contents for this subproject:  " + crppCorrespondenceEvent.Id);
                */

                db.HabitatItem().Remove(habitatItem);
                if (debugMode) logger.Debug("Just removed this event from table HabitatItems:  " + habitatItem.Id);

                //db.CrppSubprojects.State = EntityState.Modified;
                db.SaveChanges();
                if (debugMode) logger.Debug("Changes saved...");
            }
            else
                logger.Debug("The Habitat Item does not exist...");

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
        public HttpResponseMessage SaveSubproject(JObject jsonData)
        //public int SaveSubproject(JObject jsonData)
        {
            if (debugMode) logger.Debug("Inside SaveSubproject...");
            var db = ServicesContext.Current;
            if (debugMode) logger.Debug("db = " + db);

            dynamic json = jsonData;
            if (debugMode) logger.Debug("json = " + json);

            User me = AuthorizationManager.getCurrentUser();
            if (debugMode) logger.Debug("me = " + me);

            int pId = json.ProjectId.ToObject<int>();
            if (debugMode) logger.Debug("pId = " + pId);

            Project p = db.Projects.Find(pId);
            if (debugMode) logger.Debug("p = " + p);
            if (p == null)
                throw new System.Exception("Configuration error.  Please try again.");

            if (debugMode) logger.Debug("p.isOwnerOrEditor(me) = " + p.isOwnerOrEditor(me));
            if (!p.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");

            Subproject_Crpp s = json.Subproject.ToObject<Subproject_Crpp>();

            if (debugMode) logger.Debug("Found Subproject in incoming data...");

            if (s.OtherAgency == "undefined")
                s.OtherAgency = null;

            if (s.OtherProjectProponent == "undefined")
                s.OtherProjectProponent = null;

            if (s.OtherCounty == "undefined")
                s.OtherCounty = null;

            if (debugMode) logger.Debug(
                "s.ProjectName = " + s.ProjectName + "\n" +
                "s.Agency = " + s.Agency + "\n" +
                "s.OtherAgency = " + s.OtherAgency + "\n" +
                "s.ProjectProponent = " + s.ProjectProponent + "\n" +
                "s.OtherProjectProponent = " + s.OtherProjectProponent + "\n" +
                "s.TrackingNumber = " + s.TrackingNumber + "\n" +
                //"s.ActionNeeded = " + s.ActionNeeded + "\n" +
                "s.Closed = " + s.Closed + "\n" +
                "s.ProjectLead = " + s.ProjectLead + "\n" +
                "s.EffDt = " + s.EffDt + "\n" +
                "s.ByUserId = " + s.ByUserId  + "\n" +
                "s.County = " + s.County + "\n" +
                "s.OtherCounty = " + s.OtherCounty + "\n" +
                "s.ProjectDescription = " + s.ProjectDescription + "\n" +
                "s.UIR = " + s.UIR + "\n" +
                "s.OffResTribalFee = " + s.OffResTribalFee + "\n" +
                "s.Comments = " + s.Comments + "\n"
                );

            s.EffDt = DateTime.Now;
            logger.Debug("Set s.EffDt = " + s.EffDt);

            s.ByUserId = me.Id;
            logger.Debug("s.ByUserId = " + s.ByUserId);

            /* Note:  Up above, we created a subproject, in which put all the incoming data.
             * However, since that item is not "tied" to an item in the database, it can only 
             * be used to add a new record.
             * If we wish to edit an existing record, we must locate that record, and poke the stuff
             * we just pulled in into that existing record.
             * Otherwise, if we try to edit a record, even though we have the ID of an existing record,
             * the system will give a weird error like this:
             * "Attaching an entity of type 'services.Models.User' failed because another entity of the same type already has the same primary key value"
             * So, create a new variable for the existing record in the database, go and fined it, then put the incoming data into it.
             * Then when we save.
             */
            logger.Debug("s.Id = " + s.Id);
            if (s.Id == 0)
            {
                logger.Debug("About to add new subproject...");
                db.Subproject_Crpp().Add(s);
                logger.Debug("created new subproject");
            }
            else
            {
                logger.Debug("About to update subproject...");
                try
                {
                    Subproject_Crpp s2 = db.Subproject_Crpp().Find(s.Id);

                    s2.ProjectName = s.ProjectName;
                    s2.Agency = s.Agency;
                    s2.OtherAgency = s.OtherAgency;
                    s2.ProjectProponent = s.ProjectProponent;
                    s2.OtherProjectProponent = s.OtherProjectProponent;
                    s2.TrackingNumber = s.TrackingNumber;
                    s2.Closed = s.Closed;
                    s2.ProjectLead = s.ProjectLead;
                    s2.County = s.County;
                    s2.OtherAgency = s.OtherAgency;
                    s2.OtherCounty = s.OtherCounty;
                    s2.ProjectDescription = s.ProjectDescription;
                    s2.UIR = s.UIR;
                    s2.OffResTribalFee = s.OffResTribalFee;
                    s2.Comments = s.Comments;
                    s2.EffDt = s.EffDt;
                    s2.ByUserId = s.ByUserId;

                    //db.Entry(s).State = EntityState.Modified;
                    db.Entry(s2).State = EntityState.Modified;
                }
                catch (System.Exception e)
                {
                    logger.Debug("Exception:  " + e.Message + ", " + e.InnerException);
                }
                //db.Entry(s).Property("UpdateTime").IsModified = true;
                logger.Debug("updated existing subproject");
            }

            db.SaveChanges();
            if (debugMode) logger.Debug("Just saved the DB changes.");

            //string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads/subprojects");
            //string root = System.Configuration.ConfigurationManager.AppSettings["PathToCrppProjectDocuments"] + ("\\uploads\\subprojects");
            //string root = System.Configuration.ConfigurationManager.AppSettings["PathToCrppProjectDocuments"];
            string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\" + p.Id + "\\S\\";
            logger.Debug("root = " + root);

            //string strSubprojectsPath = root + "\\" + s.Id;
            string strSubprojectsPath = root + s.Id;
            if (debugMode) logger.Debug("The path for the new subproject will be:  " + strSubprojectsPath);

            System.IO.Directory.CreateDirectory(strSubprojectsPath);
            if (debugMode) logger.Debug("Just created folder for the new subproject:  " + s.Id);

            //return new HttpResponseMessage(HttpStatusCode.OK);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, s);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage SaveHabSubproject(JObject jsonData)
        //public int SaveSubproject(JObject jsonData)
        {
            if (debugMode) logger.Debug("Inside SaveHabSubproject...");
            var db = ServicesContext.Current;
            if (debugMode) logger.Debug("db = " + db);

            dynamic json = jsonData;
            if (debugMode) logger.Debug("json = " + json);

            //string strJson = "[" + json + "]";

            User me = AuthorizationManager.getCurrentUser();
            if (debugMode) logger.Debug("me = " + me);

            int pId = json.ProjectId.ToObject<int>();
            if (debugMode) logger.Debug("pId = " + pId);

            Project p = db.Projects.Find(pId);
            if (debugMode) logger.Debug("p = " + p);
            if (p == null)
                throw new System.Exception("Configuration error.  Please try again.");

            if (debugMode) logger.Debug("p.isOwnerOrEditor(me) = " + p.isOwnerOrEditor(me));
            if (!p.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");

            if (debugMode) logger.Debug("About to check incoming data for Subproject...");
            Subproject_Hab s = new Subproject_Hab();
            if (debugMode) logger.Debug("Found Subproject in incoming data...");


            // Spin through the fields passed in; as we find the fields, we will capture the data.
            foreach (var item in json.Subproject)
            {
                logger.Debug("Inside foreach loop");
                //int newSubprojectId = 0;

                if (!(item is JProperty))
                {
                    throw new System.Exception("There is a problem with your request. Format error.");
                }

                var prop = item as JProperty;
                logger.Debug("Property name = " + prop.Name);

                dynamic subproject_json = prop.Value;
                //logger.Debug("Property value = " + subproject_json);

                if (prop.Name == "Id")
                    s.Id = subproject_json;
                else if (prop.Name == "ProjectId")
                {
                    logger.Debug("Found ProjectId...");
                    s.ProjectId = subproject_json;
                    logger.Debug("ProjectId = " + s.ProjectId);
                }
                else if (prop.Name == "ProjectName")
                    s.ProjectName = subproject_json;
                else if (prop.Name == "ProjectSummary")
                    s.ProjectSummary = subproject_json;
                else if (prop.Name == "ProjectDescription")
                    s.ProjectDescription = subproject_json;
                else if (prop.Name == "LocationId")
                    s.LocationId = subproject_json;
                else if (prop.Name == "ProjectStartDate")
                    s.ProjectStartDate = subproject_json;
                else if (prop.Name == "ProjectEndDate")
                    s.ProjectEndDate = subproject_json;
                else if (prop.Name == "FirstFoods")
                {
                    //logger.Debug("Found FirstFoods...");
                    s.FirstFoods = subproject_json.ToString();
                    s.FirstFoods = removeFormattingChars(s.FirstFoods);
                    //logger.Debug("FirstFoods = " + s.FirstFoods);
                }
                else if (prop.Name == "RiverVisionTouchstone")
                {
                    //logger.Debug("Found RiverVisionTouchstone...");
                    s.RiverVisionTouchstone = subproject_json.ToString();
                    s.RiverVisionTouchstone = removeFormattingChars(s.RiverVisionTouchstone);
                    //logger.Debug("RiverVisionTouchstone = " + s.RiverVisionTouchstone);
                }
                else if (prop.Name == "HabitatObjectives")
                {
                    //logger.Debug("Found HabitatObjectives...");
                    s.HabitatObjectives = subproject_json.ToString();
                    s.HabitatObjectives = removeFormattingChars(s.HabitatObjectives);
                    //logger.Debug("HabitatObjectives = " + s.HabitatObjectives);
                }
                else if (prop.Name == "NoaaEcologicalConcerns")
                {
                    //logger.Debug("Found NoaaEcologicalConcerns...");
                    s.NoaaEcologicalConcerns = subproject_json.ToString();
                    s.NoaaEcologicalConcerns = removeFormattingChars(s.NoaaEcologicalConcerns);
                    //logger.Debug("NoaaEcologicalConcerns = " + s.NoaaEcologicalConcerns);
                }
                else if (prop.Name == "NoaaEcologicalConcernsSubcategories")
                {
                    //logger.Debug("Found NoaaEcologicalConcernsSubcategories...");
                    s.NoaaEcologicalConcernsSubcategories = subproject_json.ToString();
                    s.NoaaEcologicalConcernsSubcategories = removeFormattingChars(s.NoaaEcologicalConcernsSubcategories);
                    //logger.Debug("NoaaEcologicalConcernsSubcategories = " + s.NoaaEcologicalConcernsSubcategories);
                }
                else if (prop.Name == "LimitingFactors")
                {
                    //logger.Debug("Found LimitingFactors...");
                    s.LimitingFactors = subproject_json.ToString();
                    s.LimitingFactors = removeFormattingChars(s.LimitingFactors);
                    //logger.Debug("LimitingFactors = " + s.LimitingFactors);
                }
                else if (prop.Name == "Staff")
                    s.Staff = subproject_json;
                //else if (prop.Name == "Collaborators")
                //{
                //s.Collaborators = subproject_json;
                //    s.Collaborators = subproject_json.ToString();
                //}
                else if (prop.Name == "Comments")
                    s.Comments = subproject_json;
                //else if (prop.Name == "OtherFundingAgency")
                //    s.OtherFundingAgency = subproject_json.ToString();
                //else if (prop.Name == "OtherCollaborators")
                //    s.OtherCollaborators = subproject_json.ToString();
            }

            s.ProjectId = pId;
            logger.Debug("Set s.ProjectId = " + s.ProjectId);

            s.EffDt = DateTime.Now;
            logger.Debug("Set s.EffDt = " + s.EffDt);

            s.ByUserId = me.Id;
            logger.Debug("s.ByUserId = " + s.ByUserId);

            if (debugMode) logger.Debug(
                "s.Id = " + s.Id + "\n" +
                "s.ProjectId = " + s.ProjectId + "\n" +
                "s.ProjectName = " + s.ProjectName + "\n" +
                "s.ProjectDescription = " + s.ProjectDescription + "\n" +
                "s.LocationId = " + s.LocationId + "\n" +
                "s.ProjectStartDate = " + s.ProjectStartDate + "\n" +
                "s.ProjectEndDate = " + s.ProjectEndDate + "\n" +
                "s.FirstFoods = " + s.FirstFoods + "\n" +
                "s.RiverVisionTouchstone = " + s.RiverVisionTouchstone + "\n" +
                "s.HabitatObjectives = " + s.HabitatObjectives + "\n" +
                "s.NoaaEcologicalConcernsSubcategories = " + s.NoaaEcologicalConcernsSubcategories + "\n" +
                "s.NoaaEcologicalConcerns = " + s.NoaaEcologicalConcerns + "\n" +
                "s.LimitingFactors = " + s.LimitingFactors + "\n" +
                    //"s.Funding = " + s.Funding + "\n" +
                "s.Staff = " + s.Staff + "\n" +
                    //"s.Collaborators = " + s.Collaborators + "\n" +
                    //"s.OtherCollaborator = " + s.OtherCollaborators + "\n" +
                "s.EffDt = " + s.EffDt + "\n" +
                "s.ByUserId = " + s.ByUserId + "\n" +
                "s.Comments = " + s.Comments + "\n"
                );

            if (s.Id == 0)
            {
                logger.Debug("About to add new habSubproject...");
                db.Subproject_Hab().Add(s);
                logger.Debug("created new habSubproject");
            }
            else
            {
                logger.Debug("About to update habSubproject...");
                db.Entry(s).State = EntityState.Modified;
                //db.Entry(s).Property("UpdateTime").IsModified = true;
                logger.Debug("updated existing habSubproject");
            }

            db.SaveChanges();
            if (debugMode) logger.Debug("Just saved the DB changes.");

            int newId = s.Id;
            logger.Debug("newId = " + s.Id);

            // Funding Start*********************************
            var funding = new List<Funding>();

            // Let's check for a funding object.
            // This works.  Now we need to add the incoming record to the Funding table.
            try
            {
                string strFunding = (string)jsonData["Subproject"]["Funding"].ToString();// .Value<string>();
                logger.Debug("strFunding = " + strFunding);
                if ((!String.IsNullOrEmpty(strFunding)) || (strFunding.Length < 3)) // < 3 means "[]"
                {
                    JArray aryFunding = (JArray)jsonData["Subproject"]["Funding"];
                    foreach (JToken f in aryFunding)
                    {
                        logger.Debug("Inside aryFunding loop.");
                        //logger.Debug("Name = " + f["Name"]);
                        //logger.Debug("Amount = " + f["Amount"]);

                        Funding funder = new Funding();
                        funder.Id = Convert.ToInt32(f["Id"].ToString());
                        funder.Name = f["Name"].ToString();
                        funder.Amount = Convert.ToDouble(f["Amount"].ToString());
                        funder.SubprojectId = newId;
                        funder.ProjectId = s.ProjectId;
                        //funder.OtherName = f["OtherFunder"].ToString();

                        logger.Debug("funder.Id = " + funder.Id);
                        logger.Debug("funder.Name = " + funder.Name);
                        logger.Debug("funder.Amount = " + funder.Amount);
                        logger.Debug("funder.SubprojectId = " + funder.SubprojectId);
                        logger.Debug("funder.ProjectId = " + funder.ProjectId);
                        //logger.Debug("funder.OtherName = " + funder.OtherName);

                        funding.Add(funder);
                    }

                    logger.Debug("Cleaning out funder data for this subproject...");
                    // If the funder list changes, we cannot use the current list to determine who used to be on the list.
                    // Therefore, just delete all the funders for this subproject first, then we will add the ones actually assocated to the subproject.
                    List<services.Models.Funding> funderList = (from item in db.Funding
                                                                where item.ProjectId == p.Id
                                                                where item.SubprojectId == s.Id
                                                                orderby item.Id
                                                                select item).ToList();

                    foreach (var item in funderList)
                    {
                        db.Funding.Remove(item);
                        logger.Debug("Removed funder: " + item.Name);
                    }



                    logger.Debug("");
                    logger.Debug("funding.length = " + funding.Count());
                    foreach (var item in funding)
                    {
                        logger.Debug("item.Id = " + item.Id);
                        logger.Debug("item.Name = " + item.Name);
                        logger.Debug("item.Amount = " + item.Amount);
                        //logger.Debug("item.OtherName = " + item.OtherName);
                        //if (item.Id == 0)
                        //{
                        db.Funding.Add(item);
                        logger.Debug("Added funder Name: " + item.Name);
                        //}
                        //else
                        //{
                        //    db.Entry(item).State = EntityState.Modified;
                        //    logger.Debug("Updated funder Id: " + item.Id + ", Name: " + item.Name);
                        //}
                    }
                    db.SaveChanges();
                }

                //habFunding.Add(fundingItem.ToObject(dbset_funding_type));
                //var fundingRec = new Funding();

            }
            catch (System.Exception e)
            {
                logger.Debug(e.Message + ", " + e.InnerException);
                logger.Debug("Could not locate Funding (optional field).  Skipping...");
            }
            // Funding End**************************************

            // Collaborators Start*********************************
            var collaborators = new List<Collaborator>();

            // Let's check for a collaborator object.
            // This works.  Now we need to add the incoming record to the Funding table.
            try
            {
                string strCollaborator = (string)jsonData["Subproject"]["Collaborators"].ToString();// .Value<string>();
                logger.Debug("strCollaborator = " + strCollaborator);
                if ((!String.IsNullOrEmpty(strCollaborator)) || (strCollaborator.Length < 3)) // < 3 means "[]"
                {
                    JArray aryCollaborator = (JArray)jsonData["Subproject"]["Collaborators"];
                    foreach (JToken c in aryCollaborator)
                    {
                        logger.Debug("Inside aryCollaborator loop.");
                        //logger.Debug("Name = " + c["Name"]);

                        Collaborator collaborator = new Collaborator();
                        collaborator.Id = Convert.ToInt32(c["Id"].ToString());
                        collaborator.Name = c["Name"].ToString();
                        collaborator.SubprojectId = newId;
                        collaborator.ProjectId = s.ProjectId;

                        logger.Debug("collaborator.Id = " + collaborator.Id);
                        logger.Debug("collaborator.Name = " + collaborator.Name);
                        logger.Debug("collaborator.SubprojectId = " + collaborator.SubprojectId);
                        logger.Debug("collaborator.ProjectId = " + collaborator.ProjectId);

                        collaborators.Add(collaborator);
                    }

                    logger.Debug("Cleaning out collaborator data for this subproject...");
                    // If the collaborator list changes, we cannot use the current list to determine who used to be on the list.
                    // Therefore, just delete all the collaborators for this subproject first, then we will add the ones actually assocated to the subproject.
                    List<services.Models.Collaborator> collaboratorList = (from item in db.Collaborators
                                                                           where item.ProjectId == p.Id
                                                                           where item.SubprojectId == s.Id
                                                                           orderby item.Id
                                                                           select item).ToList();

                    foreach (var item in collaboratorList)
                    {
                        db.Collaborators.Remove(item);
                        logger.Debug("Removed collaborator: " + item.Name);
                    }



                    logger.Debug("");
                    logger.Debug("collaborators.length = " + collaborators.Count());
                    foreach (var item in collaborators)
                    {
                        logger.Debug("item.Id = " + item.Id);
                        logger.Debug("item.Name = " + item.Name);
                        //logger.Debug("item.OtherName = " + item.OtherName);
                        //if (item.Id == 0)
                        //{
                        db.Collaborators.Add(item);
                        logger.Debug("Added collaborator Name: " + item.Name);
                        //}
                        //else
                        //{
                        //    db.Entry(item).State = EntityState.Modified;
                        //    logger.Debug("Updated funder Id: " + item.Id + ", Name: " + item.Name);
                        //}
                    }
                    db.SaveChanges();
                }
            }
            catch (System.Exception e)
            {
                logger.Debug(e.Message + ", " + e.InnerException);
                logger.Debug("Could not locate collaborator (optional field).  Skipping...");
            }
            // Collaborators end**************************************


            //string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads/subprojects");
            //string root = System.Configuration.ConfigurationManager.AppSettings["PathToHabitatProjectDocuments"] + ("uploads\\subprojects");
            //string root = System.Configuration.ConfigurationManager.AppSettings["PathToHabitatProjectDocuments"];
            string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"];
            logger.Debug("root = " + root);

            //string strSubprojectsPath = root + "\\" + s.Id;
            string strSubprojectsPath = root + "\\P\\" + p.Id + "\\S\\"+ s.Id;
            if (debugMode) logger.Debug("The path for the new subproject will be:  " + strSubprojectsPath);

            System.IO.Directory.CreateDirectory(strSubprojectsPath);
            if (debugMode) logger.Debug("Just created folder for the new habSubproject:  " + s.Id);

            //return new HttpResponseMessage(HttpStatusCode.OK);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, s);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage SaveCorrespondenceEvent(JObject jsonData)
        {
            if (debugMode) logger.Debug("Inside SaveCorrespondenceEvent...");
            string strId = null;  // Delare this up here, so that all if/try blocks can see it.
            var db = ServicesContext.Current;
            if (debugMode) logger.Debug("db = " + db);

            dynamic json = jsonData;
            if (debugMode) logger.Debug("json = " + json);

            User me = AuthorizationManager.getCurrentUser();
            //if (debugMode) logger.Debug("me = " + me); // getCurrentUser displays the username; this is just machinestuff.

            int pId = json.ProjectId.ToObject<int>();
            if (debugMode) logger.Debug("pId = " + pId);

            Project p = db.Projects.Find(pId);
            if (debugMode) logger.Debug("p = " + p);
            if (p == null)
                throw new System.Exception("Configuration error.  Please try again.");


            if (!p.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");

            int spId = json.SubprojectId.ToObject<int>();
            if (debugMode) logger.Debug("spId = " + spId);

            CorrespondenceEvents ce = new CorrespondenceEvents();
            logger.Debug("Created ce...");


            /* Per this article:  http://stackoverflow.com/questions/24233104/extract-data-from-json-string
            *  the cause was that CorrespondenceEvent is an object, within the object.
            *  Usually, we are dealing with an object with several properties (like ProjectId and SubprojectId).
            *  This case is different, so we must access the "sub-object differently."
            *  In this case, we use the SelectToken technique seen below.
            *  Can we do this more dynamically using a dictionary?  Something to consider down the road...
            */

            // CorrespondenceDate is required.
            // First get the date as a string, so that we can easily check if it blank (null or empty).
            //string strCorrespondenceDate = jsonData.SelectToken(@"CorrespondenceEvent.CorrespondenceDate").Value<string>();
            //if (debugMode) logger.Debug("strCorrespondenceDate = " + strCorrespondenceDate);
            string strCorrespondenceDate = null;
            try
            {
                strCorrespondenceDate = jsonData.SelectToken(@"CorrespondenceEvent.CorrespondenceDate", true).Value<string>();
                // We know we have a date, so we just process it.
                try
                {
                    DateTime dtCorrespondenceDate = jsonData.SelectToken(@"CorrespondenceEvent.CorrespondenceDate").Value<DateTime>();
                    ce.CorrespondenceDate = dtCorrespondenceDate;

                    logger.Debug("ce.CorrespondenceDate = " + ce.CorrespondenceDate);
                }
                catch (System.Exception setdateException)
                {
                    logger.Debug("Ooops had an error setting the CorrespondenceDate: " + strCorrespondenceDate);
                    logger.Debug(setdateException.ToString());

                    throw setdateException;
                }
            }
            catch (System.Exception tokenException)
            {
                logger.Debug("Could not find the CorrespondenceDate in the JSON data: " + strCorrespondenceDate);
                logger.Debug(tokenException.ToString());

                throw tokenException;
            }

            // CorrespondenceType is optional
            try
            {
                string strCorrespondenceType = jsonData.SelectToken(@"CorrespondenceEvent.CorrespondenceType").Value<string>();
                if (debugMode) logger.Debug("strCorrespondenceType = " + strCorrespondenceType);

                if (!String.IsNullOrEmpty(strCorrespondenceType))
                {
                    ce.CorrespondenceType = strCorrespondenceType;
                    if (debugMode) logger.Debug("Processed ce.CorrespondenceType = " + ce.CorrespondenceType);
                }
                else
                {
                    if (debugMode) logger.Debug("CorrespondenceType is optional.  Skipping...");
                }
            }
            catch
            {
                logger.Debug("Could not locate CorrespondenceType (optional field).  Skipping...");
            }

            // ResponseDate is optional.
            string strResponseDate = null;
            try
            {
                strResponseDate = jsonData.SelectToken(@"CorrespondenceEvent.ResponseDate").Value<string>();
                if (!string.IsNullOrEmpty(strResponseDate))
                {
                    try
                    {
                        DateTime dtResponseDate = jsonData.SelectToken(@"CorrespondenceEvent.ResponseDate").Value<DateTime>();
                        ce.ResponseDate = dtResponseDate;

                        logger.Debug("ce.ResponseDate = " + ce.ResponseDate);
                    }
                    catch (System.Exception setdateException)
                    {
                        logger.Debug("Ooops had an error setting the ResponseDate: " + strResponseDate);
                        logger.Debug(setdateException.ToString());

                        throw setdateException;
                    }
                }
                else
                {
                    logger.Debug("ResponseDate is blank (null or empty) on optional.  Skipping...");
                }
            }
            catch
            {
                logger.Debug("ResponseDate is blank (null or empty) on optional.  Skipping...");
            }

            // Id is required and passed in programmatically.
            try
            {
                strId = jsonData.SelectToken(@"CorrespondenceEvent.Id").Value<string>();
                if (debugMode) logger.Debug("strId = " + strId);

                if ((!String.IsNullOrEmpty(strId)) && (Convert.ToInt32(strId) > 0))
                {
                    ce.Id = Convert.ToInt32(strId);
                    if (debugMode) logger.Debug("Processed ce.Id = " + ce.Id);
                }
                else
                {
                    if (String.IsNullOrEmpty(strId))
                        logger.Debug("Id cannot be blank (null or empty) = ");
                    else if (Convert.ToInt32(strId) == 0)
                        logger.Debug("Id = 0; excluding it from the new event record...");
                }
            }
            catch
            {
                logger.Debug("CorrespondenceEvent.Id is required but could not locate it.");
            }

            // NumberOfDays is optional
            try
            {
                string strNumberOfDays = jsonData.SelectToken(@"CorrespondenceEvent.NumberOfDays").Value<string>();
                if (debugMode) logger.Debug("strNumberOfDays = " + strNumberOfDays);

                if (!String.IsNullOrEmpty(strNumberOfDays))
                {
                    ce.NumberOfDays = Convert.ToInt32(strNumberOfDays);
                    if (debugMode) logger.Debug("Processed ce.NumberOfDays = " + ce.NumberOfDays);
                }
                else
                {
                    logger.Debug("NumberOfDays is blank (null or empty) on optional.  Skipping...");
                }
            }
            catch
            {
                logger.Debug("Could not locate NumberOfDays (optional field).  Skipping...");
            }

            // ResponseType is optional
            try
            {
                string strResponseType = jsonData.SelectToken(@"CorrespondenceEvent.ResponseType").Value<string>();
                if (debugMode) logger.Debug("strResponseType = " + strResponseType);

                if (!String.IsNullOrEmpty(strResponseType))
                {
                    ce.ResponseType = strResponseType;
                    if (debugMode) logger.Debug("Processed ce.ResponseType = " + ce.ResponseType);
                }
                else
                {
                    if (debugMode) logger.Debug("ResponseType is optional.  Skipping...");
                }
            }
            catch
            {
                logger.Debug("Could not locate ResponseType (optional field).  Skipping...");
            }

            // StaffMember is optional
            try
            {
                string strStaffMember = jsonData.SelectToken(@"CorrespondenceEvent.StaffMember").Value<string>();
                if (debugMode) logger.Debug("strStaffMember = " + strStaffMember);

                if (!String.IsNullOrEmpty(strStaffMember))
                {
                    ce.StaffMember = strStaffMember;
                    if (debugMode) logger.Debug("Processed ce.StaffMember = " + ce.StaffMember);
                }
                else
                {
                    if (debugMode) logger.Debug("StaffMember is optional.  Skipping...");
                }
            }
            catch
            {
                logger.Debug("Could not locate StaffMember (optional field).  Skipping...");
            }

            // Comments is optional
            try
            {
                string strComments = jsonData.SelectToken(@"CorrespondenceEvent.EventComments").Value<string>();
                if (debugMode) logger.Debug("strComments = " + strComments);

                if (!String.IsNullOrEmpty(strComments))
                {
                    ce.EventComments = strComments;
                    if (debugMode) logger.Debug("Processed ce.EventComments = " + ce.EventComments);
                }
                else
                {
                    if (debugMode) logger.Debug("EventComments is optional.  Skipping...");
                }
            }
            catch
            {
                logger.Debug("Could not locate Comments (optional field).  Skipping...");
            }

            // EventFiles is optional
            try
            {
                string strEventFiles = jsonData.SelectToken(@"CorrespondenceEvent.EventFiles.").Value<string>();
                //JObject joEventFiles = jsonData.SelectToken(@"CorrespondenceEvent.EventFiles.").Value<JObject>();
                //logger.Debug("Created joEventFiles");
                //dynamic jsonFiles = joEventFiles;
                //logger.Debug("jsonFiles = " + jsonFiles);
                //string strFile = null;

                if (debugMode) logger.Debug("strEventFiles = " + strEventFiles);

                if (!String.IsNullOrEmpty(strEventFiles))
                {
                    ce.EventFiles = strEventFiles;

                    if (debugMode) logger.Debug("Processed ce.EventFiles = " + ce.EventFiles);
                }
                else
                {
                    if (debugMode) logger.Debug("EventFiles is optional.  Skipping...");
                }
            }
            catch
            {
                logger.Debug("Could not locate EventFiles (optional field).  Skipping...");
            }

            /*try
            {
                // Comments is optional
                string strActionNeeded = jsonData.SelectToken(@"CorrespondenceEvent.ActionNeeded").Value<string>();
                if (debugMode) logger.Debug("strActionNeeded = " + strActionNeeded);

                if (!String.IsNullOrEmpty(strActionNeeded))
                {
                    ce.ActionNeeded = strActionNeeded;
                    if (debugMode) logger.Debug("Processed ce.ActionNeeded = " + ce.ActionNeeded);
                }
                else
                {
                    if (debugMode) logger.Debug("Comments is optional.  Skipping...");
                }
            }
            catch
            {
                logger.Debug("Could not locate Comments (optional field).  Skipping...");
            }
            */

            try
            {
                ce.SubprojectId = spId;
                ce.ByUserId = me.Id;
                ce.EffDt = DateTime.Now;
                // Now let's try to save the Correspondence Event.
                if ((String.IsNullOrEmpty(strId)) || (Convert.ToInt32(strId) == 0))
                {
                    // The Id field auto-increments and will not accept a 0; therefore, let's just leave it blank.

                    // Save changes before adding the CorrespondenceEvent.
                    db.SaveChanges();
                    logger.Debug("Save DB just before adding the new correspondence event.");

                    // Add the CorrespondenceEvent
                    db.CorrespondenceEvents().Add(ce);
                    logger.Debug("created new correspondence event");
                }
                else
                {
                    CorrespondenceEvents ce2 = db.CorrespondenceEvents().Find(ce.Id);

                    ce2.SubprojectId = ce.SubprojectId;
                    ce2.CorrespondenceDate = ce.CorrespondenceDate;
                    ce2.ResponseType = ce.ResponseType;
                    ce2.NumberOfDays = ce.NumberOfDays;
                    ce2.ResponseDate = ce.ResponseDate;
                    ce2.StaffMember = ce.StaffMember;
                    ce2.EventFiles = ce.EventFiles;
                    ce2.EventComments = ce.EventComments;
                    ce2.EffDt = ce.EffDt;
                    ce2.ByUserId = ce.ByUserId;
                    ce2.CorrespondenceType = ce.CorrespondenceType;

                    db.Entry(ce2).State = EntityState.Modified;
                    logger.Debug("updated correspondence event");
                }

                try
                {
                    db.SaveChanges();
                    if (debugMode) logger.Debug("Just saved the DB changes.");

                    // Now let's save the documents.

                    //string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads/subprojects");
                    //string root = System.Configuration.ConfigurationManager.AppSettings["PathToCrppProjectDocuments"] + ("\\uploads\\subprojects");
                    string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\";
                    logger.Debug("root = " + root);

                    string strSubprojectsFolder = root + p.Id + "\\S\\" + spId + "\\";
                    string strCleanupFolder = root;

                    // Let's do some clean-up first.
                    // If the file-move/copy fails for any reason, the Body-part file gets left behind.
                    //string strSubprojectsPath = System.Configuration.ConfigurationManager.AppSettings["PathToServices"] + "uploads\\subprojects\\";
                    logger.Debug("strSubprojectsFolder = " + strSubprojectsFolder);
                    logger.Debug("strCleanupFolder = " + strCleanupFolder);

                    //string[] filepaths = Directory.GetFiles(strSubprojectsFolder);
                    string[] filepaths = Directory.GetFiles(strCleanupFolder);
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
                            logger.Debug("Had a problem cleaning out files...");
                            logger.Debug("Exception Message:  " + ioException.Message);
                            logger.Debug("Inner Exception Message:  " + ioException.InnerException.Message);
                        }
                    }
                    

                    // Now let's continue with our save process.
                    //string strSubprojectsPath = root + "\\" + spId;
                    //string strSubprojectsPath = strSubprojectsFolder + spId;
                    //if (debugMode) logger.Debug("The path for the subproject folder is:  " + strSubprojectsPath);


                    //UploadSubprojectFile(pId, spId, ce.EventFiles);

                    //System.IO.Directory.CreateDirectory(strSubprojectsPath);
                    //if (debugMode) logger.Debug("Just created folder for the new subproject:  " + strSubprojectsPath);

                    // Check if the submitting person is the ProjectLead
                    // We have the CDMS UserId, but we need the UserName for the comparison.
                    User submittingUser = db.User.Find(ce.ByUserId);
                    logger.Debug("submittingUser = " + submittingUser.Username);

                    // Get the Project Lead's username from the subproject.
                    Subproject_Crpp subProj = db.Subproject_Crpp().Find(spId);
                    logger.Debug("subProject = " + subProj.ProjectName);
                    logger.Debug("subProj.ProjectLead = " + subProj.ProjectLead);
                    subProj.EffDt = DateTime.Now;
                    if (debugMode) logger.Debug("Set EffDt in Subprojects.");

                    db.SaveChanges();
                    if (debugMode) logger.Debug("Just saved the DB changes again.");

                    // Get the Project Lead's First and Last Name from the Users table.
                    // The find checks the Id field (an Int) and we are passing it a string; this throws an error.

                    User projLead = new User();
                    //var qUser = db.User.Where(u => u.Username == subProj.ProjectLead);
                    var qUser = db.User.Where(u => u.Fullname == subProj.ProjectLead);
                    foreach (var aUser in qUser)
                    {
                        projLead.Id = aUser.Id;
                        projLead.Fullname = aUser.Fullname;
                        projLead.Username = aUser.Username;
                    }

                    logger.Debug("submittingUser.Id = " + submittingUser.Id + ", projLead.Id = " + projLead.Id);
                    /* Comment this block out for Dev. */
                    if (submittingUser.Id != projLead.Id)
                    {
                        //SendEmailToProjectLead(projLead.Username, projLead.Fullname, subProj.ProjectName, me.Fullname);
                        SendEmailToProjectLead(projLead.Username, projLead.Fullname, subProj.ProjectName, me.Fullname, 
                            ce.ResponseDate, ce.NumberOfDays);
                    }
                    

                }
                catch (System.Exception dbSave)
                {
                    logger.Debug("An error occurred saving the database changes for ce.Id: " + ce.Id);
                    System.Exception currentException = dbSave;
                    do
                    {
                        logger.Debug("\n {0}", currentException.Message);
                        currentException = currentException.InnerException;
                    } while (currentException != null);

                    logger.Debug("Error: " + dbSave.Message);

                    throw dbSave;
                }
            }
            catch (System.Exception AddCe)
            {
                logger.Debug("Had problems adding CorrespondenceEvent to database.");
                logger.Debug("ce.Id = " + ce.Id + ", e.Message = " + AddCe.Message);

                db = ServicesContext.RestartCurrent;
                db.Configuration.AutoDetectChangesEnabled = true;
                db.Configuration.ValidateOnSaveEnabled = true;

                //ok, lets try to delete the correspondence event that went bad.
                db.CorrespondenceEvents().Remove(db.CorrespondenceEvents().Find(ce.Id));
                db.SaveChanges();

                logger.Debug("ok so we auto-deleted the correspondence event we created: " + ce.Id);

                throw AddCe; //rethrow so that it'll come back as an error in the client.
            }
            finally
            {
                db.Configuration.AutoDetectChangesEnabled = true;
                db.Configuration.ValidateOnSaveEnabled = true;
            }


            /*
            //check for duplicates.  If it is a duplicate, add it to our list and bail out.
            if (activity.isDuplicate())
            {
                duplicateActivities.Add(activity);
            }
            */

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        [HttpPost]
        public HttpResponseMessage SaveHabitatItem(JObject jsonData)
        {
            if (debugMode) logger.Debug("Inside SaveHabitatItem...");
            //string strId = null;  // Delare this up here, so that all if/try blocks can see it.
            var db = ServicesContext.Current;
            if (debugMode) logger.Debug("db = " + db);

            dynamic json = jsonData;
            if (debugMode) logger.Debug("json = " + json);

            User me = AuthorizationManager.getCurrentUser();
            //if (debugMode) logger.Debug("me = " + me); // getCurrentUser displays the username; this is just machinestuff.

            int pId = json.ProjectId.ToObject<int>();
            if (debugMode) logger.Debug("pId = " + pId);

            Project p = db.Projects.Find(pId);
            if (debugMode) logger.Debug("p = " + p);
            if (p == null)
                throw new System.Exception("Configuration error.  Please try again.");


            if (!p.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");

            int spId = json.SubprojectId.ToObject<int>();
            if (debugMode) logger.Debug("spId = " + spId);

            // Locate the associated subproject.
            Subproject_Hab spH1 = db.Subproject_Hab().Find(spId);
            Subproject_Hab spH2 = db.Subproject_Hab().Find(spId);
            HabitatItem hi = new HabitatItem();
            logger.Debug("Created hi...");


            /* Per this article:  http://stackoverflow.com/questions/24233104/extract-data-from-json-string
            *  the cause was that CorrespondenceEvent is an object, within the object.
            *  Usually, we are dealing with an object with several properties (like ProjectId and SubprojectId).
            *  This case is different, so we must access the "sub-object differently."
            *  In this case, we use the SelectToken technique seen below.
            *  Can we do this more dynamically using a dictionary?  Something to consider down the road...
            */

            // CorrespondenceDate is required.
            // First get the date as a string, so that we can easily check if it blank (null or empty).
            //string strCorrespondenceDate = jsonData.SelectToken(@"CorrespondenceEvent.CorrespondenceDate").Value<string>();
            //if (debugMode) logger.Debug("strCorrespondenceDate = " + strCorrespondenceDate);
            /*string strCorrespondenceDate = null;
            try
            {
                strCorrespondenceDate = jsonData.SelectToken(@"CorrespondenceEvent.CorrespondenceDate", true).Value<string>();
                // We know we have a date, so we just process it.
                try
                {
                    DateTime dtCorrespondenceDate = jsonData.SelectToken(@"CorrespondenceEvent.CorrespondenceDate").Value<DateTime>();
                    ce.CorrespondenceDate = dtCorrespondenceDate;

                    logger.Debug("ce.CorrespondenceDate = " + ce.CorrespondenceDate);
                }
                catch (Exception setdateException)
                {
                    logger.Debug("Ooops had an error setting the CorrespondenceDate: " + strCorrespondenceDate);
                    logger.Debug(setdateException.ToString());

                    throw setdateException;
                }
            }
            catch (Exception tokenException)
            {
                logger.Debug("Could not find the CorrespondenceDate in the JSON data: " + strCorrespondenceDate);
                logger.Debug(tokenException.ToString());

                throw tokenException;
            }
             */


            // Spin through the fields passed in; as we find the fields, we will capture the data.
            foreach (var item in json.HabitatItem)
            {
                logger.Debug("Inside foreach loop");
                //int newSubprojectId = 0;

                if (!(item is JProperty))
                {
                    throw new System.Exception("There is a problem with your request. Format error.");
                }

                var prop = item as JProperty;
                logger.Debug("Property name = " + prop.Name);
                int intPropNameLength = prop.Name.Length;
                logger.Debug("intPropNameLength = " + intPropNameLength);

                dynamic subproject_json = prop.Value;
                logger.Debug("Property value = " + subproject_json);

                if ((intPropNameLength == 2) && (prop.Name.IndexOf("Id") > -1))
                {

                    hi.Id = subproject_json;
                    logger.Debug("hi.Id = " + hi.Id);
                }
                /*else if ((intPropNameLength > 0) && (prop.Name.IndexOf("SubprojectId") > -1))
                {
                    hi.SubprojectId = subproject_json;
                    logger.Debug("hi.SubprojectId = " + hi.SubprojectId);
                }
                */
                else if ((intPropNameLength == 8) && (prop.Name.IndexOf("ItemName") > -1))
                {
                    hi.ItemName = subproject_json;
                    logger.Debug("hi.ItemName = " + hi.ItemName);
                }
                else if ((intPropNameLength == 8) && (prop.Name.IndexOf("ItemType") > -1))
                {
                    hi.ItemType = subproject_json;
                    logger.Debug("hi.ItemType = " + hi.ItemType);
                }
                else if ((intPropNameLength == 9) && (prop.Name.IndexOf("ItemFiles") > -1))
                {
                    hi.ItemFiles = subproject_json;
                    logger.Debug("hi.ItemFiles = " + hi.ItemFiles);
                }
                else if ((intPropNameLength == 13) && (prop.Name.IndexOf("ExternalLinks") > -1))
                {
                    hi.ExternalLinks = subproject_json;
                    logger.Debug("hi.ExternalLinks = " + hi.ExternalLinks);
                }
            }
            hi.SubprojectId = spId;
            hi.ByUserId = me.Id;
            hi.EffDt = DateTime.Now;

            spH2.ProjectName = spH1.ProjectName;
            spH2.ProjectSummary = spH1.ProjectSummary;
            spH2.ProjectDescription = spH1.ProjectDescription;
            spH2.ProjectStartDate = spH1.ProjectStartDate;
            spH2.ProjectEndDate = spH1.ProjectEndDate;
            spH2.FirstFoods = spH1.FirstFoods;
            spH2.RiverVisionTouchstone = spH1.RiverVisionTouchstone;
            spH2.HabitatObjectives = spH1.HabitatObjectives;
            spH2.NoaaEcologicalConcerns = spH1.NoaaEcologicalConcerns;
            spH2.NoaaEcologicalConcernsSubcategories = spH1.NoaaEcologicalConcernsSubcategories;
            spH2.LimitingFactors = spH1.LimitingFactors;
            spH2.Staff = spH1.Staff;
            spH2.Collaborators = spH1.Collaborators;
            spH2.Comments = spH1.Comments;
            spH2.ByUserId = spH1.ByUserId;
            spH2.ProjectId = spH1.ProjectId;
            spH2.LocationId = spH1.LocationId;
            spH2.OtherCollaborators = spH1.OtherCollaborators;
            spH2.FeatureImage = spH1.FeatureImage;
            spH2.EffDt = DateTime.Now;

            if (debugMode) logger.Debug(//"hi.ItemName = " + hi.ItemName);

                "hi.Id = " + hi.Id + "\n" +
                "hi.SubprojectId = " + hi.SubprojectId + "\n" +
                "hi.ItemType = " + hi.ItemType + "\n" +
                "hi.ItemName = " + hi.ItemName + "\n" +
                "hi.ExternalLinks = " + hi.ExternalLinks + "\n" +
                "hi.ItemFiles = " + hi.ItemFiles + "\n" +
                "hi.ByUserId = " + hi.ByUserId + "\n" +
                "hi.EffDt = " + hi.EffDt + "\n"
                );


            try
            {
                //hi.SubprojectId = spId;
                //hi.ByUserId = me.Id;
                //hi.EffDt = DateTime.Now;
                // Now let's try to save the Habita Item.
                if (hi.Id == 0)
                {
                    // The Id field auto-increments and will not accept a 0; therefore, let's just leave it blank.

                    // Save changes before adding the HabitatItem.
                    db.SaveChanges();
                    logger.Debug("Save DB just before adding the new habitat item.");

                    // Add the HabitatItem
                    db.HabitatItem().Add(hi);
                    logger.Debug("created new habitat item");
                    db.Entry(spH2).State = EntityState.Modified;
                }
                else
                {
                    HabitatItem hi2 = db.HabitatItem().Find(hi.Id);

                    hi2.Id = hi.Id;
                    hi2.SubprojectId = hi.SubprojectId;
                    hi2.ItemType = hi.ItemType;
                    hi2.ItemName = hi.ItemName;
                    hi2.ExternalLinks = hi.ExternalLinks;
                    hi2.ItemFiles = hi.ItemFiles;
                    hi2.ByUserId = hi.ByUserId;
                    hi2.EffDt = hi.EffDt;

                    db.Entry(hi2).State = EntityState.Modified;
                    logger.Debug("updated Habitat item");
                    db.Entry(spH2).State = EntityState.Modified;
                }

                try
                {
                    logger.Debug("Trying to save db again.");
                    db.SaveChanges();
                    if (debugMode) logger.Debug("Just saved the DB changes.");

                    // Now let's save the documents.

                    //string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads/subprojects");
                    //string root = System.Configuration.ConfigurationManager.AppSettings["PathToHabitatProjectDocuments"] + ("uploads\\subprojects");
                    //string root = System.Configuration.ConfigurationManager.AppSettings["PathToHabitatProjectDocuments"];
                    string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\";
                    logger.Debug("root = " + root);
                    string strCleanupFolder = root;

                    //string strSubprojectsFolder = root + "\\";
                    string strSubprojectsFolder = root + p.Id + "\\S\\" + hi.SubprojectId + "\\";
                    logger.Debug("strSubprojectsPath = " + strSubprojectsFolder);
                    logger.Debug("strCleanupFolder = " + strCleanupFolder);

                    // Let's do some clean-up first.
                    // If the file-move/copy fails for any reason, the Body-part file gets left behind.
                    //string strSubprojectsPath = System.Configuration.ConfigurationManager.AppSettings["PathToServices"] + "uploads\\subprojects\\";

                    // Create the folder path, if neceesary; if any folders are missing, they will be created.
                    System.IO.Directory.CreateDirectory(strSubprojectsFolder);
                    if (debugMode) logger.Debug("Just created folder for the new subproject:  " + strSubprojectsFolder);

                    //string[] filepaths = Directory.GetFiles(strSubprojectsFolder);
                    string[] filepaths = Directory.GetFiles(strCleanupFolder);

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
                            logger.Debug("Had a problem cleaning out files...");
                            logger.Debug("Exception Message:  " + ioException.Message);
                            logger.Debug("Inner Exception Message:  " + ioException.InnerException.Message);
                        }
                    }

                }
                catch (System.Exception dbSave)
                {
                    logger.Debug("An error occurred saving the database changes for hi.Id: " + hi.Id);
                    System.Exception currentException = dbSave;
                    do
                    {
                        logger.Debug("\n {0}", currentException.Message);
                        currentException = currentException.InnerException;
                    } while (currentException != null);

                    logger.Debug("Error: " + dbSave.Message);

                    throw dbSave;
                }
            }
            catch (System.Exception AddHi)
            {
                logger.Debug("Had problems adding Habitat item to database.");
                logger.Debug("hi.Id = " + hi.Id + ", e.Message = " + AddHi.Message);

                db = ServicesContext.RestartCurrent;
                db.Configuration.AutoDetectChangesEnabled = true;
                db.Configuration.ValidateOnSaveEnabled = true;

                //ok, lets try to delete the correspondence event that went bad.
                db.CorrespondenceEvents().Remove(db.CorrespondenceEvents().Find(hi.Id));
                db.SaveChanges();

                logger.Debug("ok so we auto-deleted the Habitat item we created: " + hi.Id);

                throw AddHi; //rethrow so that it'll come back as an error in the client.
            }
            finally
            {
                db.Configuration.AutoDetectChangesEnabled = true;
                db.Configuration.ValidateOnSaveEnabled = true;
            }


            /*
            //check for duplicates.  If it is a duplicate, add it to our list and bail out.
            if (activity.isDuplicate())
            {
                duplicateActivities.Add(activity);
            }
            */

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

        //private void SendEmailToProjectLead(string projectLeadUsername, string projectLeadFullName, string projectName, string updatingPerson)
        private void SendEmailToProjectLead(string projectLeadUsername, string projectLeadFullName, string projectName, string updatingPerson, 
            DateTime? responseDate, int numberOfDays)
        {
            logger.Debug("**********Inside SendEmailToProjectLead...**********");
            logger.Debug("projectLeadUsername = " + projectLeadUsername);
            logger.Debug("projectLeadFullName = " + projectLeadFullName);
            logger.Debug("projectName = " + projectName);
            logger.Debug("updatingPerson = " + updatingPerson);

            string strStatus = null;
            string strProjectLeadEmail = null;
            bool foundEmail = false;
            bool foundUsername = false;
            bool blnDoesUserExist = false;

            //String adConnectionString = WebConfigurationManager.ConnectionStrings["ADConnectionString"].ConnectionString;
            //logger.Debug("ContextType.Domain = " + ContextType.Domain);

            //logger.Debug("Environment.UserDomainName = " + Environment.UserDomainName);
            // If we get the Environment.UserDomainName from the system, it returns IIS APPPOOL, but we need it to be MAILCOMM.
            // Therefore, we just set it that way.

            if (DoesUserExist(projectLeadUsername))
            {
                blnDoesUserExist = true;
            }

            if (blnDoesUserExist)
            {
                logger.Debug("Checking Active Directory for the user's email address...");
                using (var context = new PrincipalContext(ContextType.Domain, "MAILCOMM"))
                {
                    //logger.Debug("Created context...");
                    using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                    {
                        //logger.Debug("Created searcher...");
                        foreach (var result in searcher.FindAll())
                        {
                            string strResult = result.ToString();
                            strResult = strResult.Trim();

                            if (strResult.Equals(projectLeadFullName))
                            {
                                DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                                System.DirectoryServices.PropertyCollection pc = de.Properties;
                                int theCount = 0;
                                logger.Debug("projectLeadUsername = " + projectLeadUsername);
                                foreach (PropertyValueCollection col in pc)
                                {
                                    //rtxStatusText.AppendText("Checking property name, looking for sAMAccountName...\n");
                                    //logger.Debug(col.PropertyName + " : " + col.Value);
                                    if ((col.PropertyName.Equals("sAMAccountName")) && (col.Value.Equals(projectLeadUsername)))
                                    {
                                        logger.Debug("*********************Found username:  " + projectLeadUsername);
                                        foundUsername = true;
                                        strStatus = "Success";
                                    }
                                    else if (col.PropertyName.Equals("mail"))
                                    {
                                        logger.Debug("*********************Found email:  " + col.Value);
                                        strProjectLeadEmail = col.Value.ToString();
                                        foundEmail = true;
                                        strStatus = "Success";
                                    }
                                    theCount++;

                                    if (foundUsername && foundEmail)
                                        break;
                                }
                                if ((theCount > 0) && (String.IsNullOrEmpty(strStatus)))
                                {
                                    strStatus = "Failed to find user.";
                                    logger.Debug(strStatus);
                                }
                            } // if strResult
                            if (foundUsername && foundEmail)
                                break;
                        } // foreach result
                    } // using searcher
                } // using context
            }

            //logger.Debug("strProjectLeadEmail = " + strProjectLeadEmail);

            /***** Leaving this in place, but commenting out for now. *****
            //***CREATE AN APPOINTMENT WITH REMINDER****
            Microsoft.Office.Interop.Outlook.Application olApp = null;
            Microsoft.Office.Interop.Outlook.AppointmentItem appointment = null;

            olApp = new Microsoft.Office.Interop.Outlook.Application();

            if (responseDate != null)
            {
                try
                {
                    appointment = (Microsoft.Office.Interop.Outlook.AppointmentItem)olApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olAppointmentItem);
                    //Microsoft.Office.Interop.Outlook.AppointmentItem appt = (Microsoft.Office.Interop.Outlook.AppointmentItem)appointment;

                    appointment.Subject = "Reminder for Response";
                    //appt.Subject = "Reminder for Response";

                    appointment.Body = "Project " + projectName + " needs a response.";
                    //appt.Body = "Project " + projectName + " needs a response.";

                    DateTime apptStart = (System.DateTime) responseDate;
                    apptStart.AddHours(8);
                    appointment.Start = apptStart;
                    //appt.Start = apptStart;

                    DateTime apptEnd = apptStart;
                    apptEnd.AddHours(1);
                    appointment.End = apptEnd;
                    //appt.End = apptEnd;

                    appointment.ReminderSet = true;
                    //appt.ReminderSet = true;

                    appointment.ReminderMinutesBeforeStart = 10080; // 7 days in minutes = (60 * 24 * 7)
                    //appt.ReminderMinutesBeforeStart = 10080; // 7 days in minutes = (60 * 24 * 7)

                    appointment.Importance = OlImportance.olImportanceHigh;
                    //appt.Importance = OlImportance.olImportanceHigh;

                    appointment.Save();
                    //appt.Save();

                    // Note:  Within the next few lines, some are marked at the end with //*.
                    // The compiler does not like these lines, saying 
                    // Ambiguity between method Microsoft.Office.Interop.Outlook._AppointmentItem.Send() 
                    // and non-method Microsoft.Office.Interop.Outlook.ItemEvents_10_Event.Send.  Using method group.
                    //

                    //appointment.Send(); //*
                    //appt.Send(); //*

                    Microsoft.Office.Interop.Outlook._MailItem mailItem = appointment.ForwardAsVcal();
                    mailItem.To = strProjectLeadEmail;
                    //((Microsoft.Office.Interop.Outlook._MailItem)mailItem).Send(); //*
                    mailItem.Send();

                }
                catch (System.Exception e)
                {
                    logger.Debug("Had a problem creating/sending the reminder:  " + e.Message + ",\n" + e.InnerException);
                }
            }
             */


            //***CREATE NEW MAIL MESSAGE****
            MailMessage message = new MailMessage();

            //***SET TO AND FROM MAIL PROPERTIES****
            //message.To.Add(new MailAddress(strProjectLeadEmail));  // Real email address.
            if (blnDoesUserExist)
                message.To.Add(new MailAddress(strProjectLeadEmail));  // Real email address.
            else
                message.To.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["CrppDefaultEmail"]));  // Real email address.

            message.From = new MailAddress("NO-REPLY@ctuir.org");
            //message.To.Add(new MailAddress("GeorgeClark@ctuir.org")); // Test email address.
            //message.From = new MailAddress("GeorgeClark@ctuir.org");

            //'***SET MAIL FORMAT TO HTML****
            message.IsBodyHtml = true;

            //***SET MAIL SUBJECT****
            string strSubject = "New or Updated  Correspondence Event for project named " + projectName;
            message.Subject = strSubject;

            //***SET MAIL BODY****
            //string strEmailBody = "<strong><font color=#000099>Project " + projectName + " was updated by " + updatingPerson + "." +
            //    "</font></strong><br><br>Courtesy email from the CDMS system...";
            string strEmailBody = "";
            if (blnDoesUserExist)
            {
                strEmailBody = "<strong><font color=#000099>Project " + projectName + " was updated by " + updatingPerson + "." +
                "</font></strong><br><br>Courtesy email from the CDMS system...";
            }
            else
            {
                strEmailBody = "<strong><font color=#ff0000>Project Lead " + projectLeadFullName + " is no longer with CTUIR.</font></strong>" +
                                "<br><br>" + 
                                "<strong><font color=#000099>Project " + projectName + " was updated by " + updatingPerson + "." + "</font></strong>" +
                                "<br><br>Courtesy email from the CDMS system...";
            }
            message.Body = strEmailBody;

            //***DECLARE SMTP CLIENT FOR MAIL SEND****
            SmtpClient client = new SmtpClient();

            //***SPECIFIY SMTP LOCAL SERVER****
            client.Host = "ctuir-exchg01.mailcomm.ctuir.com";

            try
            {
                client.Send(message);
                logger.Debug("Just sent email to project lead.");
                //return "Success";
            }
            catch (System.Exception e)
            {
                string strPath = Directory.GetCurrentDirectory() + "\\";
                string strApp = "SendEmailToProjectLead:  ";
                string strError = e.Message;
                string strMessage = strPath + strApp + strError;
                logger.Debug(strMessage);
                //return "Failed";
            }
            client.Dispose();
        }

        public bool DoesUserExist(string userName)
        {
            using (var domainContext = new PrincipalContext(ContextType.Domain, "MAILCOMM"))
            {
                using (var foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, userName))
                {
                    return foundUser != null;
                }
            }
        }
    }
}
