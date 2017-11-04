using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using services.Models;
using services.Models.Data;
using services.Resources;
using services.ExtensionMethods;
using System.Net.Mail;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace services.Controllers
{
    // this class is a PRIVATE extension of DataActionController. 
    //  it should be excluded from the main CTUIR release of cdms

    [Authorize]
    public class CRPPSubProjectController : CDMSController
    {
        // POST /api/v1/crppsubproject/uploadcrppsubprojectfile
        public Task<HttpResponseMessage> UploadCrppSubprojectFile()
        {
            logger.Debug("Inside UploadSubprojectFile...");
            logger.Debug("starting to process incoming subproject files.");

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            //string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads/subprojects");
            //string prefix = @"";
            //string root = System.Configuration.ConfigurationManager.AppSettings["PathToCrppProjectDocuments"] + ("\\uploads\\subprojects");
            string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\";
            //string root = System.IO.Path.Combine(prefix, strPath);
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
                logger.Debug("P dir already exists...");

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
                logger.Debug("And we think the ProjectId === " + ProjectId);

                Int32 SubprojectId = Convert.ToInt32(provider.FormData.Get("SubprojectId"));
                logger.Debug("And we think the Subprojectid === " + SubprojectId);

                string strDatastoreTablePrefix = provider.FormData.Get("DatastoreTablePrefix");
                logger.Debug("And we think the DatastoreTablePrefix = " + strDatastoreTablePrefix);

                Project project = db.Projects.Find(ProjectId);

                Subproject_Crpp subproject = db.Subproject_Crpp().Find(SubprojectId);

                if (project == null)
                    throw new Exception("Project ID not found: " + ProjectId);

                if (!project.isOwnerOrEditor(me))
                    throw new Exception("Authorization error.");

                if (subproject == null)
                    throw new Exception("Subproject ID not found: " + SubprojectId);

                if (strDatastoreTablePrefix == null)
                    throw new Exception("DatastoreTablePrefix not found: " + strDatastoreTablePrefix);

                //If the project/dataset folder does not exist, create it.
                string subprojectPath = root + project.Id + "\\S\\" + subproject.Id;
                //DirectoryInfo datasetDirInfo = new DirectoryInfo(@root);
                DirectoryInfo subprojectDirInfo = new DirectoryInfo(subprojectPath);
                if (!subprojectDirInfo.Exists)
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
                            //var newFileName = ActionController.relocateSubprojectFile(
                            //                file.LocalFileName,
                            //                SubprojectId,
                            //                filename,
                            //                false);

                            var newFileName = FileController.relocateSubprojectFile(
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
                                                     //newFile.Link = rootUrl + "/services/uploads/subprojects/" + SubprojectId + "/" + info.Name; //file.LocalFileName;
                                                     //newFile.Link = rootUrl + "/" + System.Configuration.ConfigurationManager.AppSettings["ExecutingEnvironment"] + "uploads/subprojects/" + SubprojectId + "/" + info.Name;
                                                     //if (strDatastoreTablePrefix == "CrppContracts")
                                                     //{
                                                     //newFile.Link = System.Configuration.ConfigurationManager.AppSettings["PathToCrppProjectDocuments"] + "\\uploads\\" + SubprojectId + "\\" + info.Name;
                                                     //    newFile.Link = System.Configuration.ConfigurationManager.AppSettings["PathToCrppProjectDocuments"] + "\\" + SubprojectId + "\\" + info.Name;
                                                     //}
                                                     //else
                                                     //    newFile.Link = rootUrl + "/" + System.Configuration.ConfigurationManager.AppSettings["ExecutingEnvironment"] + "uploads/" + ProjectId + "/" + info.Name;

                            //newFile.Link = rootUrl + "/" + System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\" + ProjectId + "\\S\\" + SubprojectId + "\\" + info.Name;
                            newFile.Link = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\" + ProjectId + "\\S\\" + SubprojectId + "\\" + info.Name;



                            newFile.Size = (info.Length / 1024).ToString(); //file.Headers.ContentLength.ToString();
                            newFile.FileTypeId = FileType.getFileTypeFromFilename(info);
                            newFile.UserId = me.Id;
                            newFile.ProjectId = ProjectId;
                            newFile.DatasetId = null; // No datasetId for subproject files.
                            newFile.Subproject_CrppId = SubprojectId;
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

                logger.Debug("Done saving subproject files.");
                var result = JsonConvert.SerializeObject(thefiles);
                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(result, System.Text.Encoding.UTF8, "text/plain");  //to stop IE from being stupid.

                return resp;
            });

            return task;
        }

        // GET /api/v1/crppsubproject/getcrppsubprojectfiles/5
        [System.Web.Http.HttpGet]
        public IEnumerable<Models.File> GetCRPPSubprojectFiles(int Id)
        {
            logger.Debug("Inside SubprojectFiles...");
            logger.Debug("Fetching Files for Project " + Id);
            var result = new List<Models.File>();

            var ndb = ServicesContext.Current;

            result = (from item in ndb.Files
                      where item.ProjectId == Id
                      where item.Subproject_CrppId != null
                      orderby item.ProjectId, item.Subproject_CrppId
                      select item).ToList();

            return result;
        }

        // GET /api/v1/crppsubproject/getcrppsubprojects
        [HttpGet]
        public IEnumerable<Subproject_Crpp> GetCrppSubprojects()
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

        // POST /api/v1/crppsubproject/deletecorreseventfile
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

        //POST /api/v1/crppsubproject/removecrppsubproject
        [HttpPost]
        public HttpResponseMessage RemoveCrppSubproject(JObject jsonData)
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
            logger.Debug("The path for the subproject is:  " + strSubprojectsPath);

            if (Directory.Exists(strSubprojectsPath))
            {
                System.IO.Directory.Delete(strSubprojectsPath, true);
                logger.Debug("Just deleted documents folder and contents for this subproject:  " + crppSubproject.Id);
            }
            else
            {
                logger.Debug("Could not find folder: " + strSubprojectsPath);
            }

            db.Subproject_Crpp().Remove(crppSubproject);
            logger.Debug("Just removed this subproject from table CrppSubprojects:  " + crppSubproject.Id);

            //db.CrppSubprojects.State = EntityState.Modified;
            db.SaveChanges();
            logger.Debug("Changes saved...");

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        // POST /api/v1/crppsubproject/removecorrespondenceevent
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
            logger.Debug("The path for the subproject is:  " + strSubprojectsPath);

            System.IO.Directory.Delete(strSubprojectsPath, true);
            logger.Debug("Just deleted documents folder and contents for this subproject:  " + crppCorrespondenceEvent.Id);
            */

            db.CorrespondenceEvents().Remove(correspondenceEvent);
            logger.Debug("Just removed this event from table CorrespondenceEvents:  " + correspondenceEvent.Id);

            //db.CrppSubprojects.State = EntityState.Modified;
            db.SaveChanges();
            logger.Debug("Changes saved...");

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // POST /api/v1/crppsubproject/savecrppsubproject
        [HttpPost]
        //public HttpResponseMessage SaveSubproject(JObject jsonData)
        public HttpResponseMessage SaveCrppSubproject(JObject jsonData)
        //public int SaveSubproject(JObject jsonData)
        {
            logger.Debug("Inside SaveCrppSubproject...");
            var db = ServicesContext.Current;
            logger.Debug("db = " + db);

            dynamic json = jsonData;
            logger.Debug("json = " + json);

            User me = AuthorizationManager.getCurrentUser();
            logger.Debug("me = " + me);

            int pId = json.ProjectId.ToObject<int>();
            logger.Debug("pId = " + pId);

            Project p = db.Projects.Find(pId);
            logger.Debug("p = " + p);
            if (p == null)
                throw new System.Exception("Configuration error.  Please try again.");

            logger.Debug("p.isOwnerOrEditor(me) = " + p.isOwnerOrEditor(me));
            if (!p.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");

            Subproject_Crpp s = json.Subproject.ToObject<Subproject_Crpp>();

            logger.Debug("Found Subproject in incoming data...");

            if (s.OtherAgency == "undefined")
                s.OtherAgency = null;

            if (s.OtherProjectProponent == "undefined")
                s.OtherProjectProponent = null;

            if (s.OtherCounty == "undefined")
                s.OtherCounty = null;

            logger.Debug(
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
                "s.ByUserId = " + s.ByUserId + "\n" +
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
            logger.Debug("Just saved the DB changes.");

            //string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads/subprojects");
            //string root = System.Configuration.ConfigurationManager.AppSettings["PathToCrppProjectDocuments"] + ("\\uploads\\subprojects");
            //string root = System.Configuration.ConfigurationManager.AppSettings["PathToCrppProjectDocuments"];
            string root = System.Configuration.ConfigurationManager.AppSettings["PathToCdmsShare"] + "\\P\\" + p.Id + "\\S\\";
            logger.Debug("root = " + root);

            //string strSubprojectsPath = root + "\\" + s.Id;
            string strSubprojectsPath = root + s.Id;
            logger.Debug("The path for the new subproject will be:  " + strSubprojectsPath);

            System.IO.Directory.CreateDirectory(strSubprojectsPath);
            logger.Debug("Just created folder for the new subproject:  " + s.Id);

            //return new HttpResponseMessage(HttpStatusCode.OK);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, s);
            return response;
        }

        // POST /api/v1/crppsubproject/savecorrespondenceevent
        [HttpPost]
        public HttpResponseMessage SaveCorrespondenceEvent(JObject jsonData)
        {
            logger.Debug("Inside SaveCorrespondenceEvent...");
            string strId = null;  // Delare this up here, so that all if/try blocks can see it.
            string strTmp = "";

            var db = ServicesContext.Current;
            logger.Debug("db = " + db);

            dynamic json = jsonData;
            logger.Debug("json = " + json);

            User me = AuthorizationManager.getCurrentUser();
            //logger.Debug("me = " + me); // getCurrentUser displays the username; this is just machinestuff.

            int pId = json.ProjectId.ToObject<int>();
            logger.Debug("pId = " + pId);

            Project p = db.Projects.Find(pId);
            logger.Debug("p = " + p);
            if (p == null)
                throw new System.Exception("Configuration error.  Please try again.");


            if (!p.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");

            int spId = json.SubprojectId.ToObject<int>();
            logger.Debug("spId = " + spId);

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
            //logger.Debug("strCorrespondenceDate = " + strCorrespondenceDate);

            ce.SubprojectId = spId;
            logger.Debug("ce.SubprojectId = " + ce.SubprojectId);

            ce.ByUserId = me.Id;
            logger.Debug("ce.ByUserId = " + ce.ByUserId);

            ce.EffDt = DateTime.Now;
            logger.Debug("ce.EffDt = " + ce.EffDt);

            foreach (var item in json.CorrespondenceEvent)
            {
                logger.Debug("----------");

                if (!(item is JProperty))
                {
                    throw new System.Exception("There is a problem with your request. Format error.");
                }

                var prop = item as JProperty;
                logger.Debug("Property name = " + prop.Name);

                //int intPropNameLength = prop.Name.Length;
                //logger.Debug("intPropNameLength = " + intPropNameLength);

                strTmp = item.ToObject<string>(); // We will use to determine if the value is blank, regardless of what the ultimate value is.
                logger.Debug("strTmp = " + strTmp);

                if (prop.Name == "NumberOfDays") // Optional
                {
                    logger.Debug("Found NumberOfDays");

                    if (!String.IsNullOrEmpty(strTmp))
                    {
                        ce.NumberOfDays = item.ToObject<int>();
                        logger.Debug("ce.NumberOfDays = " + ce.NumberOfDays);
                    }
                    else
                    {
                        logger.Debug("NumberOfDays null, blank, or empty; Optional -- skipping...");
                    }
                }
                else if (prop.Name == "CorrespondenceDate") // Required
                {
                    logger.Debug("Found CorrespondenceDate");

                    if (!String.IsNullOrEmpty(strTmp))
                    {
                        try
                        {
                            DateTime dtCorrespondenceDate = item.ToObject<DateTime>();
                            ce.CorrespondenceDate = item.ToObject<DateTime>();
                            logger.Debug("ce.CorrespondenceDate = " + ce.CorrespondenceDate);
                        }
                        catch (SystemException setdateException)
                        {
                            logger.Debug("Ooops had an error setting the CorrespondenceDate: " + strTmp);
                            logger.Debug(setdateException.ToString());

                            throw setdateException;
                        }
                    }
                    else
                    {
                        logger.Debug("CorrespondenceDate null, blank, or empty; required -- error!");
                    }
                }
                else if (prop.Name == "CorrespondenceType") // Optional
                {
                    logger.Debug("Found CorrespondenceType");

                    if (!String.IsNullOrEmpty(strTmp))
                    {
                        ce.CorrespondenceType = item.ToObject<string>();
                        logger.Debug("ce.CorrespondenceType = " + ce.CorrespondenceType);
                    }
                    else
                    {
                        logger.Debug("CorrespondenceType null, blank, or empty; Optional -- skipping...");
                    }
                }
                else if (prop.Name == "ResponseDate") // Optional
                {
                    logger.Debug("Found ResponseDate");

                    if (!String.IsNullOrEmpty(strTmp))
                    {
                        try
                        {
                            ce.ResponseDate = item.ToObject<DateTime>();
                            logger.Debug("ce.ResponseDate = " + ce.ResponseDate);
                        }
                        catch (System.Exception setdateException)
                        {
                            logger.Debug("Ooops had an error setting the ResponseDate: " + strTmp);
                            logger.Debug(setdateException.ToString());

                            throw setdateException;
                        }
                    }
                    else
                    {
                        logger.Debug("ResponseDate null, blank, or empty; Optional -- skipping...");
                    }
                }
                else if (prop.Name == "ResponseType") // Optional
                {
                    logger.Debug("Found ResponseType");

                    if (!String.IsNullOrEmpty(strTmp))
                    {
                        ce.ResponseType = item.ToObject<string>();
                        logger.Debug("ce.ResponseType = " + ce.ResponseType);
                    }
                    else
                    {
                        logger.Debug("ResponseType null, blank, or empty; Optional -- skipping...");
                    }
                }
                else if (prop.Name == "StaffMember") // Required
                {
                    logger.Debug("Found StaffMember");

                    if (!String.IsNullOrEmpty(strTmp))
                    {
                        ce.StaffMember = item.ToObject<string>();
                        logger.Debug("ce.StaffMember = " + ce.StaffMember);
                    }
                    else
                    {
                        logger.Debug("StaffMember null, blank, or empty; required -- error!");
                    }
                }
                else if (prop.Name == "EventComments") // Optional
                {
                    logger.Debug("Found EventComments");

                    if (!String.IsNullOrEmpty(strTmp))
                    {
                        ce.EventComments = item.ToObject<string>();
                        logger.Debug("ce.EventComments = " + ce.EventComments);
                    }
                    else
                    {
                        logger.Debug("EventComments null, blank, or empty; Optional -- skipping...");
                    }
                }
                else if (prop.Name == "EventFiles") // Optional
                {
                    logger.Debug("Found EventFiles");

                    if (!String.IsNullOrEmpty(strTmp))
                    {
                        ce.EventFiles = item.ToObject<string>();
                        logger.Debug("ce.EventFiles = " + ce.EventFiles);
                    }
                    else
                    {
                        logger.Debug("EventFiles null, blank, or empty; Optional -- skipping...");
                    }
                }
                else if (prop.Name == "Id") // Optional
                {
                    logger.Debug("Found Id");

                    if (!String.IsNullOrEmpty(strTmp))
                    {
                        strId = strTmp;
                        ce.Id = item.ToObject<int>();
                        logger.Debug("ce.Id = " + ce.Id);
                    }
                    else
                    {
                        logger.Debug("Id null, blank, or empty; Required -- error!");
                    }
                }
            }

            // We have reviewed/collected the passed in fields, now let's try to save the Correspondence Event.
            try
            {
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
                    logger.Debug("Just saved the DB changes.");

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

                    // Turning this off for now; it deletes ALL files in the folder with a date older than today.
                    /*foreach (string filePath in filepaths)
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
                    */

                    // Now let's continue with our save process.
                    //string strSubprojectsPath = root + "\\" + spId;
                    //string strSubprojectsPath = strSubprojectsFolder + spId;
                    //logger.Debug("The path for the subproject folder is:  " + strSubprojectsPath);


                    //UploadSubprojectFile(pId, spId, ce.EventFiles);

                    //System.IO.Directory.CreateDirectory(strSubprojectsPath);
                    //logger.Debug("Just created folder for the new subproject:  " + strSubprojectsPath);

                    // Check if the submitting person is the ProjectLead
                    // We have the CDMS UserId, but we need the UserName for the comparison.
                    User submittingUser = db.User.Find(ce.ByUserId);
                    logger.Debug("submittingUser = " + submittingUser.Username);

                    // Get the Project Lead's username from the subproject.
                    Subproject_Crpp subProj = db.Subproject_Crpp().Find(spId);
                    logger.Debug("subProject = " + subProj.ProjectName);
                    logger.Debug("subProj.ProjectLead = " + subProj.ProjectLead);
                    subProj.EffDt = DateTime.Now;
                    logger.Debug("Set EffDt in Subprojects.");

                    db.SaveChanges();
                    logger.Debug("Just saved the DB changes again.");

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