using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.WebPages;
using Newtonsoft.Json.Linq;
using NLog;
using services.Models;
using services.Models.Data;
using services.Resources;

namespace services.Controllers
{
    /*
     * This controller provides a place for RPC-type calls.
     * Ken Burcham 8/9/2013
     */
    [Authorize]
    public class ActionController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [AllowAnonymous]
        [HttpPost]
        public string SystemLog(JObject jsonData)
        {
            dynamic json = jsonData;
            
            if(jsonData.GetValue("Type").ToString() == "AUDIT")
            {
                logger.Info(jsonData.GetValue("Message")); 
            }
            else{
                logger.Error(jsonData.GetValue("Message"));
            }

            return "{Message: 'Success'}";
        }

        /*
         * TODO: We'd like to refactor this and the following controller methods:
         *   1 - move entity logic into the entity
         *   2 - return just the list of ints
         *   3 - maybe use linq instead of raw sql queries:
               (from s in Screwtrap_vw
                where s.DatasetId == 1215 
                select s.MigrationYear).Distinct()

             4 - And maybe best: explore making this soft so that you can ask for these kinds of things without
                 special methods... or maybe the dataset entity class can define/expose possible lists of itself.
         */


        //returns empty list if none found...
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<ScrewTrap_Detail> MigrationYears(int Id)
        {
            var db = ServicesContext.Current;

            List<ScrewTrap_Detail> stDetailList = new List<ScrewTrap_Detail>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "";

                query = "SET QUOTED_IDENTIFIER OFF; SELECT DISTINCT MigrationYear FROM dbo.[Screwtrap_vw] WHERE DatasetId = " + Id + " AND MigrationYear is not null ORDER BY MigrationYear desc";
                logger.Debug("SQL command = " + query);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            //logger.Debug("We have rows...");
                            while (reader.Read())
                            {
                                ScrewTrap_Detail stDet = new ScrewTrap_Detail();
                                stDet.MigrationYear = Convert.ToInt32(reader.GetValue(0).ToString());
                                //logger.Debug("stDet.MigrationYear = " + stDet.MigrationYear);

                                stDetailList.Add(stDet);
                                //logger.Debug("Added record to stDetailList...");
                            }
                        }
                        reader.Close();
                    }
                    cmd.Dispose();
                }
                con.Close();
            }

            return stDetailList.AsEnumerable();
        }

        //returns empty list if none found...
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<AdultWeir_Detail> RunYears(int Id)
        {
            List<AdultWeir_Detail> awDetailList = new List<AdultWeir_Detail>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "";

                query = "SET QUOTED_IDENTIFIER OFF; SELECT DISTINCT RunYear from dbo.[AdultWeir_vw] WHERE DatasetId = " + Id + " AND RunYear is not null ORDER BY RunYear desc";
                logger.Debug("SQL command = " + query);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                AdultWeir_Detail awDet = new AdultWeir_Detail();
                                awDet.RunYear = reader.GetValue(0).ToString();
                                awDetailList.Add(awDet);
                            }
                        }
                        reader.Close();
                    }
                    cmd.Dispose();
                }
                con.Close();
            }   

            return awDetailList.AsEnumerable();
        }

        //returns empty list if none found...
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Metrics_Header> ReportYears(int Id)
        {
            List<Metrics_Header> ryHeaderList = new List<Metrics_Header>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "";

                query = "SET QUOTED_IDENTIFIER OFF; SELECT DISTINCT YearReported FROM dbo.[Metrics_vw] WHERE DatasetId = " + Id + " AND YearReported is not null ORDER BY YearReported desc";
                logger.Debug("SQL command = " + query);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Metrics_Header ryHead = new Metrics_Header();
                                ryHead.YearReported = Convert.ToInt32(reader.GetValue(0).ToString());
                                ryHeaderList.Add(ryHead);
                            }
                        }
                        reader.Close();
                    }
                    cmd.Dispose();
                }
                con.Close();
            }

            return ryHeaderList.AsEnumerable();
        }

        //returns empty list if none found...
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<StreamNet_NOSA_Detail> SpawningYears(int Id)
        {
            List<StreamNet_NOSA_Detail> syDetailList = new List<StreamNet_NOSA_Detail>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "";

                query = "SET QUOTED_IDENTIFIER OFF; SELECT DISTINCT SpawningYear FROM dbo.[StreamNet_NOSA_vw] WHERE DatasetId = " + Id + " AND SpawningYear is not null ORDER BY SpawningYear desc";
                logger.Debug("SQL command = " + query);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                StreamNet_NOSA_Detail syDet = new StreamNet_NOSA_Detail();
                                syDet.SpawningYear = reader.GetValue(0).ToString();
                                syDetailList.Add(syDet);
                            }
                        }
                        reader.Close();
                    }
                    cmd.Dispose();
                }
                con.Close();
            }

            return syDetailList.AsEnumerable();
        }

        //returns empty list if none found...
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<StreamNet_RperS_Detail> BroodYears(int Id)
        {
            List<StreamNet_RperS_Detail> byDetailList = new List<StreamNet_RperS_Detail>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "";

                query = "SET QUOTED_IDENTIFIER OFF; SELECT DISTINCT BroodYear FROM dbo.[StreamNet_RperS_vw] WHERE DatasetId = " + Id + " AND BroodYear is not null ORDER BY BroodYear desc";
                logger.Debug("SQL command = " + query);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                StreamNet_RperS_Detail byDet = new StreamNet_RperS_Detail();
                                byDet.BroodYear = reader.GetValue(0).ToString();
                                byDetailList.Add(byDet);
                            }
                        }
                        reader.Close();
                    }
                    cmd.Dispose();
                }
                con.Close();
            }

            return byDetailList.AsEnumerable();
        }

        //returns empty list if none found...
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<StreamNet_SAR_Detail> OutmigrationYears(int Id)
        {
            List<StreamNet_SAR_Detail> syDetailList = new List<StreamNet_SAR_Detail>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "";

                query = "SET QUOTED_IDENTIFIER OFF; SELECT DISTINCT OutmigrationYear FROM dbo.[StreamNet_SAR_vw] WHERE DatasetId = " + Id + " AND OutmigrationYear is not null ORDER BY OutmigrationYear desc";
                logger.Debug("SQL command = " + query);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                StreamNet_SAR_Detail oyDet = new StreamNet_SAR_Detail();
                                oyDet.OutmigrationYear = reader.GetValue(0).ToString();
                                syDetailList.Add(oyDet);
                            }
                        }
                        reader.Close();
                    }
                    cmd.Dispose();
                }
                con.Close();
            }

            return syDetailList.AsEnumerable();
        }

        //returns empty list if none found...
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Dataset> ProjectDatasets(int Id)
        {
            var result = new List<Dataset>();

            var ndb = ServicesContext.Current;

             var datasets = ndb.Datasets.Where(o => o.ProjectId == Id);

            return datasets;
        }


        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Funding> ProjectFunders(int Id)
        {
            logger.Debug("Inside ProjectFunders...");
            logger.Debug("Fetching Funders for Project " + Id);
            var result = new List<Funding>();

            var ndb = ServicesContext.Current;

            //var datasets = ndb.Datasets.Where(o => o.ProjectId == Id);
            var f = (from item in ndb.Funding
                     //where item.Id > 1
                     where item.ProjectId == Id
                     orderby item.ProjectId, item.SubprojectId
                     select item).ToList();

            //return datasets;
            return f;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Collaborator> ProjectCollaborators(int Id)
        {
            logger.Debug("Inside ProjectCollaborators...");
            logger.Debug("Fetching Collaborators for Project " + Id);
            var result = new List<Collaborator>();

            var ndb = ServicesContext.Current;

            //var datasets = ndb.Datasets.Where(o => o.ProjectId == Id);
            var c = (from item in ndb.Collaborators
                     //where item.Id > 1
                     where item.ProjectId == Id
                     orderby item.ProjectId, item.SubprojectId
                     select item).ToList();

            //return datasets;
            return c;
        }

        //returns empty list if none found...
        [HttpGet]
        //public List<File> ProjectFiles(int ProjectId)
        public List<File> ProjectFiles(int Id)
        {
            //var result = new List<File>();

            var db = ServicesContext.Current;

            Project project = db.Projects.Find(Id);
            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            //result = project.Files; 
            List<File> result = (from item in db.Files
                                 where item.ProjectId == Id
                                 where item.DatasetId == null
                                 orderby item.Id
                                 select item).ToList();

            //foreach (var item in result)
            //{
            //    logger.Debug("project file Id:  " + item.Id + ", Name:  " + item.Name);
            //}

            if (result.Count == 0)
            {
                logger.Debug("No project files for project " + Id);
            }
            return result;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<File> DatasetFiles(int Id)
        {
            //var result = new List<File>();

            var db = ServicesContext.Current;

            Dataset dataset = db.Datasets.Find(Id);
            if (dataset == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            Project project = db.Projects.Find(dataset.ProjectId);
            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            List<File> result = (from item in db.Files
                                 where item.DatasetId == Id
                                 orderby item.Id
                                 select item).ToList();

            return result;
        }

        

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<File> SubprojectFiles(int Id)
        //public IEnumerable<File> SubprojectFiles(JObject jsonData)
        {
            logger.Debug("Inside SubprojectFiles...");
            logger.Debug("Fetching Files for Project " + Id);
            var result = new List<File>();

            var ndb = ServicesContext.Current;

            result = (from item in ndb.Files
                     //where item.Id > 1
                     where item.ProjectId == Id
                     where item.Subproject_CrppId != null
                     orderby item.ProjectId, item.Subproject_CrppId
                     select item).ToList();

            return result;

            //var result = (from item in db.Files
            //              //where item.Id > 1
            //              where item.ProjectId == p.Id
            //              where item.Subproject_CrppId == sp.Id
            //              orderby item.ProjectId, item.Subproject_CrppId
            //              select item).ToList();

            //return result;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Activity> DatasetActivities(int Id)
        {
            var result = new List<Activity>();

            var ndb = ServicesContext.Current;

            var activities = ndb.Activities.Where(o => o.DatasetId == Id);

            return activities;
        }


        [AllowAnonymous]
        [HttpGet]
        public dynamic DatasetData(int Id)
        {
            logger.Debug("Inside DatasetData.  Need data for this activity:  " + Id);
            var db = ServicesContext.Current;
            Activity activity = db.Activities.Find(Id);
            logger.Debug("Did we find anything...?  activity.Id = " + activity.Id);

            if (activity == null)
                throw new Exception("Configuration Error");

            /* The next commands gets the name of the dataset from the Datastore table prefix.
             * This is how we determine which dataset class we are working with.
            */
            logger.Debug("activity.Dataset.Datastore.TablePrefix = " + activity.Dataset.Datastore.TablePrefix);
            System.Type type = db.GetTypeFor(activity.Dataset.Datastore.TablePrefix);

            //instantiate by name
            return Activator.CreateInstance(type, activity.Id);
        }


        [AllowAnonymous]
        [HttpGet]
        public List<string> SyncToStreamNet()
        {
            logger.Debug("Inside SyncToStreamNet...");
            var pathToStreamNetSyncScript = System.Configuration.ConfigurationManager.AppSettings["PathToStreamNetSyncScript"];

            if (pathToStreamNetSyncScript.IsEmpty())
                return new List<string>() { "Need to specify path to StreamNet sync script in your web.config!"};

            if (System.IO.File.Exists(pathToStreamNetSyncScript))
                logger.Debug("The script file exists...");
            else
                logger.Debug("The script file does not exist or is inaccessible...");

            var proc = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "python.exe",
                    Arguments = pathToStreamNetSyncScript,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            string strPath = Environment.GetEnvironmentVariable("Path");
            if (strPath.IndexOf("python") < 0)
            {
                logger.Debug("The system environment variable Path does not include python...correcting.");
                strPath = strPath + ";C:\\python35\\Scripts\\;C:\\python35\\";
                Environment.SetEnvironmentVariable("Path", strPath);
            }

            string strWebException = "";
            // Note:  The following url works to verify StreamNet is up and running, but it is not the actual url we will use to do the update.
            HttpWebRequest thisRequest = (HttpWebRequest)WebRequest.Create("https://api.streamnet.org/api/test/users?email=ColetteCoiner@ctuir.org&password=8JnGDynP");
            thisRequest.Timeout = 5000;
            // If we use this the line below, it does not retrieve enough or the right stuff, so throws an exception.
            //thisRequest.Method = "HEAD"; // We do not want to actually download anything right now.
            try
            {
                using (HttpWebResponse thisResponse = (HttpWebResponse)thisRequest.GetResponse())
                {
                    //logger.Debug("thisResponse code = " + (int)thisResponse.StatusCode);
                    logger.Debug("Connected to StreamNet OK...");
                }
            }
            catch (WebException)
            {
                logger.Debug("Had a problem creating the connection to the website api.streamnet.org");
                strWebException = "Had a problem creating the connection to the website api.streamnet.org";
            }

            logger.Debug("About to start Python process...");
            proc.Start();
            logger.Debug("Python process started...");

            var outputLines = new List<string>();


            while (!proc.StandardOutput.EndOfStream)
                outputLines.Add(proc.StandardOutput.ReadLine());

            while (!proc.StandardError.EndOfStream)
            {
                outputLines.Add(proc.StandardError.ReadLine());
            }
            if (strWebException.Length > 0)
                outputLines.Add(strWebException);

            logger.Debug("Finished sync process.  Result will be displayed on screen...");

            return outputLines;
        }

        //we will overwrite any of the keys that exist in the request
        [HttpPost]
        public HttpResponseMessage SaveUserPreference(JObject jsonData)
        {
            //string result = "{message: 'Success'}"; //TODO!
            //var resp = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK);
            //resp.Content = new System.Net.Http.StringContent(result, System.Text.Encoding.UTF8, "text/plain");

            var ndb = ServicesContext.Current;

            dynamic json = jsonData;
            JObject jpref = json.UserPreference;
            var pref = jpref.ToObject<UserPreference>();

            logger.Debug("Hey we have a user preference save!" + pref.Name + " = " + pref.Value);

            User me = AuthorizationManager.getCurrentUser();

            logger.Debug("Userid = " + me.Id);

            pref.UserId = me.Id; // you can only save preferences that are your own.

            //fetch user with preferences from the database -- really want a round-trip here.
            me = ndb.User.Find(me.Id);
                
            logger.Debug("Number of existing prefs for user = " + me.UserPreferences.Count());

            UserPreference match = me.UserPreferences.Where(x => x.Name == pref.Name).SingleOrDefault();

            if (match != null)
            {
                match.Value = pref.Value;
                ndb.Entry(match).State = EntityState.Modified; 
            }
            else
            { 
                me.UserPreferences.Add(pref);
            }

            try
            {
                ndb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.Debug("Something went wrong saving the preference: " + e.Message);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        [HttpPost]
        public Project SaveProjectDetails(JObject jsonData)
        {
            Project project = null;

            var db = ServicesContext.Current;

            dynamic json = jsonData;
            JObject jproject = json.Project;
            JObject jlocation = json.Location;

            var in_project = jproject.ToObject<Project>();
            var in_location = jlocation.ToObject<Location>();

            if (in_project.Id == 0 || in_location.SdeFeatureClassId == 0 || in_location.SdeObjectId == 0)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            logger.Debug("incoming location objectid == " + in_location.SdeObjectId);

            project = db.Projects.Find(in_project.Id);
            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var locations = from loc in db.Location
                            where loc.SdeFeatureClassId == in_location.SdeFeatureClassId
                                && loc.SdeObjectId == in_location.SdeObjectId
                            select loc;

            Location location = locations.FirstOrDefault();

            if (location == null)
            {
                //then try to add it to the system so we can add it to our project
                logger.Debug("incoming Location doesn't exist, we will create it and link to it.");
                location = new Location();
                location.SdeFeatureClassId = in_location.SdeFeatureClassId;
                location.SdeObjectId = in_location.SdeObjectId;
                location.LocationTypeId = LocationType.PROJECT_TYPE;
                db.Location.Add(location);
                db.SaveChanges(); //we save the changes so that we have the id.
                logger.Debug("Saved a new location with id: " + location.Id);
            }

            logger.Debug(" and the locationid we are linking to will be " + location.Id);

            //link our project to that location if it isn't already
            if (project.Locations.Where(o => o.Id == location.Id).SingleOrDefault() == null)
            {
                logger.Debug("Project didn't have that location ... adding it.");
                project.Locations.Add(location);
            }
            else
            {
                logger.Debug("Project already has that location... why do we even bother?! (" + location.Id + ")");
            }

            User me = AuthorizationManager.getCurrentUser();

            //set project owner
            //project.OwnerId = me.Id; //this shouldn't be done here, but rather when we initially create the project.

            //db.Entry(project).State = EntityState.Modified; //shouldn't be necessary...
                
            //Now save metadata
            List<MetadataValue> metadata = new List<MetadataValue>();

            foreach (var jmv in json.Metadata)
            {
                var mv = jmv.ToObject<MetadataValue>();
                mv.UserId = me.Id;
                metadata.Add(mv);
                logger.Debug("Found new metadata: " + mv.MetadataPropertyId + " + + " + mv.Values);
            }

            //fire setMetdata which will handle persisting the metadata
            project.Metadata = metadata;

            db.SaveChanges();

            //need to refetch project -- otherwise it is old data
            //db.Entry(project).Reload();

            //logger.Debug("ok we saved now we are reloading...");

            db = ServicesContext.RestartCurrent;
            project = db.Projects.Where(o => o.Id == in_project.Id).SingleOrDefault();
            db = ServicesContext.RestartCurrent;
            project = db.Projects.Where(o => o.Id == in_project.Id).SingleOrDefault();
            db = ServicesContext.RestartCurrent;
            project = db.Projects.Where(o => o.Id == in_project.Id).SingleOrDefault();

            
            foreach (var mv in project.Metadata)
            {
                logger.Debug(" out --> " + mv.MetadataPropertyId + " === " + mv.Values);
            }
            

            //logger.Debug(JsonConvert.SerializeObject(project));

            return project; // JsonConvert.SerializeObject(project); //return our newly setup project.

        }

        /**
         * Handle uploaded files
         * IEnumerable<File>
         */
        public Task<HttpResponseMessage> PostFiles()
        {
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

            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(o =>
                {
                    
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

                    //TODO: collaborators?
                    //security check :: you can only edit your own projects
                    if (project.Owner.Id != me.Id)
                    {
                        throw new Exception("NotAuthorized: You can only edit projects you own.");
                    }


                    //Now iterate through the files that just came in
                    List<File> files = new List<File>();

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
                                var newFileName = relocateProjectFile(
                                                file.LocalFileName,
                                                ProjectId,
                                                filename);

                                var info = new System.IO.FileInfo(newFileName);

                                File newFile = new File();
                                newFile.Title = provider.FormData.Get("Title_" + fileIndex); //"Title_1, etc.
                                logger.Debug("Title = " + newFile.Title);

                                newFile.Description = provider.FormData.Get("Description_" + fileIndex); //"Description_1, etc.
                                logger.Debug("Desc = " + newFile.Description);

                                newFile.Name = info.Name;//.Headers.ContentDisposition.FileName;
                                newFile.Link = rootUrl + "/services/uploads/" + ProjectId + "/" + info.Name; //file.LocalFileName;
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

                    //Add files to database for this project.
                    if (files.Count() > 0)
                    {
                        logger.Debug("woot -- we have file objects to save");
                        foreach (var file in files)
                        {
                            project.Files.Add(file);
                        }
                        db.Entry(project).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    logger.Debug("Done saving files.");

                    //TODO: actual error/success message handling
                    string result = "{message: 'Success'}";

                    HttpResponseMessage resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    resp.Content = new System.Net.Http.StringContent(result, System.Text.Encoding.UTF8, "text/plain");

                    return resp;

                });

            return task;

        }

        //"uploadedfile0" -> 0
        public static object getFileIndex(string name)
        {
            var fileIndex = name.Replace("\"", string.Empty);
            fileIndex = fileIndex.Replace("uploadedfile", string.Empty);
            return fileIndex;
        }

        /**
         * takes current filename, project id and original filename and moves the file
         * from something like: D:\WebSites\GISInternet\services\uploads\BodyPart_c0a2f6f8-446b-42ee-88ab-2f2f3ace1e75
         * to D:\WebSites\GISInternet\services\uploads\3729\originalFilename.pdf 
         * where 3729 is the projectid folder that will be created if it doesn't exist.
         */
        // makeUnique comes in false from DataActionController.cs, uploadProjectFile, true from uploadImportFile
        public static string relocateProjectFile(string current_fullfile, int ProjectId, string orig_fullfile, bool makeUnique = false)
        {
            string new_filename = current_fullfile;

            orig_fullfile = orig_fullfile.Replace("\"", string.Empty);

            if (String.IsNullOrEmpty(orig_fullfile))
                throw new Exception("Original filename path is not given.");
            
            logger.Debug("Incoming current: " + current_fullfile);
            logger.Debug("Original file: " + orig_fullfile);
                    
            string directory = System.IO.Path.GetDirectoryName(current_fullfile);
            string orig_filename = System.IO.Path.GetFileName(orig_fullfile);

            //unless we want to make a UNIQUE filename (like for importing)
            if (makeUnique)
            {
                orig_filename = System.IO.Path.GetFileNameWithoutExtension(orig_fullfile) + "_" + System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + System.IO.Path.GetExtension(orig_fullfile);
            }

            string project_directory = directory + @"\" + ProjectId;

            logger.Debug("New target file: " + orig_filename);

            //first, ensure we have a projectid folder (will auto-create if necessary)
            logger.Debug("Creating (if necessary) project directory: " + project_directory);
            System.IO.Directory.CreateDirectory(project_directory);

            //now move the file from where it is to the project directory with the new name.
            new_filename = project_directory + @"\" + orig_filename;
            logger.Debug("Moving uploaded file to: " + new_filename);
            System.IO.File.Move(current_fullfile, new_filename);
            
            return new_filename;

        }

        /**
         * takes current filename, project id and original filename and moves the file
         * from something like: D:\WebSites\GISInternet\services\uploads\BodyPart_c0a2f6f8-446b-42ee-88ab-2f2f3ace1e75
         * to D:\WebSites\GISInternet\services\uploads\3729\originalFilename.pdf 
         * where 3729 is the projectid folder that will be created if it doesn't exist.
         */
        // makeUnique comes in false from DataActionController.cs, uploadDatasetFile
        //public static string relocateDatasetFile(string current_fullfile, int DatasetId, string orig_fullfile, bool makeUnique = false)
        public static string relocateDatasetFile(string current_fullfile, int ProjectId, int DatasetId, string orig_fullfile, bool makeUnique = false)
        {
            logger.Debug("Inside relocateDatasetFile...");
            logger.Debug("current_fullfile = " + current_fullfile);
            logger.Debug("DatasetId = " + DatasetId);
            logger.Debug("orig_fullfile = " + orig_fullfile);

            string new_filename = current_fullfile;
            logger.Debug("new_filename = " + new_filename);

            orig_fullfile = orig_fullfile.Replace("\"", string.Empty);

            if (String.IsNullOrEmpty(orig_fullfile))
                throw new Exception("Original filename path is not given.");

            logger.Debug("Incoming current: " + current_fullfile);
            logger.Debug("Original file: " + orig_fullfile);

            string directory = System.IO.Path.GetDirectoryName(current_fullfile);
            logger.Debug("directory = " + directory);
            string orig_filename = System.IO.Path.GetFileName(orig_fullfile);
            logger.Debug("orig_filename = " + orig_filename);

            //unless we want to make a UNIQUE filename (like for importing)
            if (makeUnique)
            {
                orig_filename = System.IO.Path.GetFileNameWithoutExtension(orig_fullfile) + "_" + System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + System.IO.Path.GetExtension(orig_fullfile);
            }

            //string dataset_directory = directory + @"\" + DatasetId;
            string dataset_directory = directory + "\\" + ProjectId + @"\D\" + DatasetId;

            logger.Debug("New target file: " + orig_filename);

            //first, ensure we have a projectid folder (will auto-create if necessary)
            logger.Debug("Creating (if necessary) dataset directory: " + dataset_directory);
            System.IO.Directory.CreateDirectory(dataset_directory);

            //now move the file from where it is to the project directory with the new name.
            new_filename = dataset_directory + @"\" + orig_filename;
            logger.Debug("Moving uploaded file to: " + new_filename);
            System.IO.File.Move(current_fullfile, new_filename);

            return new_filename;

        }

        /* This works like relocateProjectFile, except that it puts the subproject files here...
         * from something like: D:\WebSites\GISInternet\services\uploads\subprojects\BodyPart_c0a2f6f8-446b-42ee-88ab-2f2f3ace1e75 (local)
         * to D:\WebSites\GISInternet\services\uploads\subprojects\123\originalFilename.pdf (local)
         * or from \\gis-data01\Fisheries-GIS-Share\CDMS\Dev\BodyPart_c0a2f6f8-446b-42ee-88ab-2f2f3ace1e75
         * to \\gis-data01\Fisheries-GIS-Share\CDMS\Dev\123\originalFilename.pdf (local)
         */
        // makeUnique comes in false from DataActionController.cs, uploadSubprojectFile
        //public static string relocateSubprojectFile(string current_fullfile, int SubprojectId, string orig_fullfile, bool makeUnique = false)
        public static string relocateSubprojectFile(string current_fullfile, int ProjectId, int SubprojectId, string orig_fullfile, bool makeUnique = false)
        {
            logger.Debug("Inside relocateSubprojectFile...");
            logger.Debug("current_fullfile = " + current_fullfile);
            logger.Debug("SubprojectId = " + SubprojectId);
            logger.Debug("orig_fullfile = " + orig_fullfile);

            string new_filename = current_fullfile;
            logger.Debug("new_filename = " + new_filename);

            orig_fullfile = orig_fullfile.Replace("\"", string.Empty);

            if (String.IsNullOrEmpty(orig_fullfile))
                throw new Exception("Original filename path is not given.");

            logger.Debug("Incoming current: " + current_fullfile);
            logger.Debug("Original file: " + orig_fullfile);

            string directory = System.IO.Path.GetDirectoryName(current_fullfile);
            logger.Debug("directory = " + directory);
            string orig_filename = System.IO.Path.GetFileName(orig_fullfile);
            logger.Debug("orig_filename = " + orig_filename);

            //unless we want to make a UNIQUE filename (like for importing)
            if (makeUnique)
            {
                orig_filename = System.IO.Path.GetFileNameWithoutExtension(orig_fullfile) + "_" + System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + System.IO.Path.GetExtension(orig_fullfile);
            }

            string subproject_directory = directory + @"\" + ProjectId + "\\S\\" + SubprojectId;

            logger.Debug("New target file: " + orig_filename);

            //first, ensure we have a subprojectid folder (will auto-create if necessary)
            logger.Debug("Creating (if necessary) subproject directory: " + subproject_directory);
            System.IO.Directory.CreateDirectory(subproject_directory);

            //now move the file from where it is to the project directory with the new name.
            new_filename = subproject_directory + @"\" + orig_filename;
            logger.Debug("Moving uploaded file to: " + new_filename);
            System.IO.File.Move(current_fullfile, new_filename);

            return new_filename;
        }

        [HttpGet]
        [AllowAnonymous]
        [System.Web.Mvc.OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public User WhoAmI()
        {
            logger.Debug("whoami?");

            logger.Debug("might be --> " + System.Web.HttpContext.Current.Request.LogonUserIdentity.Name);
            if (User.Identity.IsAuthenticated)
                logger.Debug("  it says we are authenticated.");

            logger.Debug("Can we get our user?");

            User me = AuthorizationManager.getCurrentUser();

            if (me == null)
            {
                logger.Debug("nope");
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            }

            logger.Debug("yep! you are "+me.Username);
            
            var ndb = ServicesContext.Current;
            me = ndb.User.Find(me.Id);

            return me;
        }

        



    }
}
