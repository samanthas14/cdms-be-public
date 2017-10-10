using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using services.Models;
using services.Resources;
using NLog;
using System.Web.Mvc;
using services.Resources.Filters;
using Newtonsoft.Json.Linq;

/**
 * Project Controller
 */ 

namespace services.Controllers
{
    [System.Web.Http.Authorize]
    public class ProjectController : CDMSController
    {
        // GET api/v1/project/projects
        //[ProjectAuth]
        [System.Web.Http.HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public IEnumerable<Project> GetProjects()
        {
            var db = ServicesContext.Current;
            //logger.Info("GetProjects called!");

            //this is one way to neck down what gets returned... loading all the files is very time consuming for the big list...
            /*
            var results = db.Projects.Select(p => new
            {
                p.Id,
                p.ProjectType,
                p.Name,
                p.Description,
                p.CreateDateTime,
                p.OrganizationId,
                p.OwnerId,
                p.Metadata
            });
            */
            //return results.AsEnumerable();

            return db.Projects.OrderBy(o => o.Name).AsEnumerable();
        }

        // GET api/Projects/5
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public Project GetProject(int id)
        {
            var db = ServicesContext.Current;
            logger.Info("GetProject called with id: " + id);
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return project;
        }

        // PUT api/Projects/5
        [ProjectAuth(AuthorizationManager.EDIT_PERMISSION)]
        public HttpResponseMessage PutProject(int id, Project project)
        {
            var db = ServicesContext.Current;
            logger.Info("PutProject called with id: " + id);

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != project.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var dbx = new ServicesContext(); //stupid that we have to do this.  maybe i'm stupid and there is a better way?! //TODO (we get ObjectStateManager error if we lookup and then change a project by id)
            var ownerid = dbx.Projects.Where(o => o.Id == id).FirstOrDefault().OwnerId;
            dbx.Dispose();

            //handle copying the ownerid from the existing project.
            project.OwnerId = ownerid;
            logger.Debug("PUT with owner id = " + ownerid + " for project " + project.Id);

            db.Entry(project).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Projects
        [ProjectAuth(AuthorizationManager.EDIT_PERMISSION)]
        public HttpResponseMessage PostProject(Project project)
        {
            var db = ServicesContext.Current;
            logger.Info("PostProject called with project id: " + project.Id);
            project.CreateDateTime = DateTime.Now;

            User me = AuthorizationManager.getCurrentUser();
            project.OwnerId = me.Id;

            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, project);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = project.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Projects/5
        [ProjectAuth(AuthorizationManager.EDIT_PERMISSION)]
        public HttpResponseMessage DeleteProject(int id)
        {
            var db = ServicesContext.Current;
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            project.deleteRelatedData();
            db.Projects.Remove(project);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }


            HttpResponseMessage response =  Request.CreateResponse(HttpStatusCode.OK, "Success");

            return response;
        }


        //returns empty list if none found...
        [System.Web.Http.HttpGet]
        public IEnumerable<Dataset> ProjectDatasets(int Id)
        {
            var result = new List<Dataset>();

            var ndb = ServicesContext.Current;

            var datasets = ndb.Datasets.Where(o => o.ProjectId == Id);

            return datasets;
        }


        [System.Web.Http.HttpGet]
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

        [System.Web.Http.HttpGet]
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
        [System.Web.Http.HttpGet]
        public List<File> ProjectFiles(int Id)
        {
            //var result = new List<File>();

            var db = ServicesContext.Current;

            Project project = db.Projects.Find(Id);
            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            List<File> result = (from item in db.Files
                                 where item.ProjectId == Id
                                 where item.DatasetId == null
                                 orderby item.Id
                                 select item).ToList();


            if (result.Count == 0)
            {
                logger.Debug("No project files for project " + Id);
            }
            return result;
        }


        //TODO: Refactor the system to have nested/children projects instead of static "subprojects"

        [System.Web.Http.HttpGet]
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

        [System.Web.Http.HttpPost]
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


        [System.Web.Http.HttpPost]
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

        [System.Web.Http.HttpPost]
        public HttpResponseMessage SetProjectEditors(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            if (project == null)
                throw new Exception("SetProjectEditors: Configuration error.");

            User me = AuthorizationManager.getCurrentUser();
            if (me == null)
                throw new Exception("SetProjectEditors: Configuration error.");

            //verify that the sender is the project owner. 
            if (!project.isOwnerOrEditor(me))
                throw new Exception("SetProjectEditors: Authorization error.");

            //First -- remove all editors from this project.
            project.Editors.RemoveAll(o => o.Id > 0);
            db.SaveChanges();

            foreach (var item in json.Editors)
            {
                User user = db.User.Find(item.Id.ToObject<int>());
                if (user == null)
                    logger.Debug("SetProjectEditors: Wow -- user not found! i guess we can't add them: " + item.Id);
                else
                {
                    logger.Debug("Adding: " + item.Id);
                    project.Editors.Add(user);
                }
            }

            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);

        }
    }
}