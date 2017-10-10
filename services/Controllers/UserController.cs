using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using services.Models;
using Newtonsoft.Json.Linq;
using services.Resources;

namespace services.Controllers
{
    [System.Web.Http.Authorize]
    public class UserController : CDMSController
    {
        // GET api/Users
        public IEnumerable<User> GetUsers()
        {
            var db = ServicesContext.Current;
            //var user = db.User.Include(u => u.Organization);
            List<User> userList = (from u in db.User
                            where u.Inactive == null
                            orderby u.Fullname
                            select u).Include(u => u.Organization).ToList();

            //return userAsEnumerable();
            return userList.AsEnumerable();
        }

        // GET api/Users/5
        [System.Web.Mvc.OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public User GetUser(int id)
        {
            var db = ServicesContext.Current;
            User user = db.User.Find(id);
            if (user == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return user;
        }

        // PUT api/Users/5
        public HttpResponseMessage PutUser(int id, User user)
        {
            var db = ServicesContext.Current;
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != user.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(user).State = EntityState.Modified;

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

        // POST api/Users
        public HttpResponseMessage PostUser(User user)
        {
            var db = ServicesContext.Current;
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, user);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = user.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Users/5
        public HttpResponseMessage DeleteUser(int id)
        {
            var db = ServicesContext.Current;
            User user = db.User.Find(id);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.User.Remove(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, user);
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
                logger.Debug("GetMyDatasets: Couldn't get your datasets -- probably don't have any favorites.");
                logger.Debug(e);
            }

            var datasets = db.Datasets.SqlQuery("SELECT * FROM Datasets WHERE Id in (" + mydatasets + ") ORDER BY Name");

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
                logger.Debug("GetMyProjects: Couldn't get your projects -- probably don't have any favorites.");
                logger.Debug(e);
            }

            var myprojects = db.Projects.SqlQuery("SELECT * FROM Projects WHERE Id in (" + my_projects + ") ORDER BY Name");

            return myprojects;
        }
    }
}