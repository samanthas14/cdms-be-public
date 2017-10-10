using Newtonsoft.Json.Linq;
using services.Models;
using services.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace services.Controllers
{
    public class FishermenController : CDMSController
    {
        [System.Web.Http.HttpGet]
        public IEnumerable<Fisherman> GetProjectFishermen(int Id)
        {
            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();

            var project = db.Projects.Find(Id);
            if (project == null)
                throw new System.Exception("Configuration error: Project not recognized");

            return project.Fishermen;
        }

        [System.Web.Http.HttpGet]
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

        [System.Web.Http.HttpPost]
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

        [System.Web.Http.HttpPost]
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

        [System.Web.Http.HttpPost]
        public HttpResponseMessage SaveFisherman(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            User me = AuthorizationManager.getCurrentUser();

            int pId = json.ProjectId.ToObject<int>(); // Getting stuck on this line.

            Project p = db.Projects.Find(pId);

            if (p == null)
                throw new System.Exception("Configuration error.  Please try again.");

            if (!p.isOwnerOrEditor(me))
                throw new System.Exception("Authorization error.");

            Fisherman f = json.Fisherman.ToObject<Fisherman>();

            DateTime? theDateInactive = null;

            f.DateInactive = theDateInactive;

            logger.Debug(
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
            logger.Debug("Just saved the DB changes.");

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
