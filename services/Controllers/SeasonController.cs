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
using System.Configuration;
using System.Data.SqlClient;

namespace services.Controllers
{
    public class SeasonController : CDMSController
    {
        /*
         * get list of seasons for this datasetid (returns an empty list if none)
         */
        //GET /api/v1/season/getseasons/5
        [System.Web.Http.HttpGet]
        public IEnumerable<Seasons> GetSeasons(int id)
        {
            logger.Debug("Inside GetSeasons...");
            logger.Debug("id = " + id);
            var ndb = ServicesContext.Current;
            return ndb.Seasons.Where(o => o.DatasetId == id).ToList();
        }

        // POST /api/v1/season/saveseason
        public HttpResponseMessage SaveSeason(JObject jsonData)
        {
            logger.Debug("Inside SaveSeason...");
            var db = ServicesContext.Current;
            logger.Debug("db = " + db);

            dynamic json = jsonData;
            logger.Debug("json = " + json);

            //string strJson = "[" + json + "]";

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

            logger.Debug("About to check incoming data for Season...");
            Seasons s = new Seasons();

            s.Id = json.Id.ToObject<int>();
            s.Species = json.Species.ToObject<string>();
            s.Season = json.Season.ToObject<int>();
            s.OpenDate = json.OpenDate.ToObject<DateTime>();
            s.CloseDate = json.CloseDate.ToObject<DateTime>();
            s.TotalDays = json.TotalDays.ToObject<int>();
            s.DatasetId = json.DatasetId.ToObject<int>();


            logger.Debug("\n" +
                "s.Id = " + s.Id + "\n" +
                "s.Species = " + s.Species + "\n" +
                "s.Season = " + s.Season + "\n" +
                "s.OpenDate = " + s.OpenDate + "\n" +
                "s.CloseDate = " + s.CloseDate + "\n" +
                "s.TotalDays = " + s.TotalDays + "\n" +
                "s.DatasetId = " + s.DatasetId + "\n"
                );

            if (s.Id == 0)
            {
                logger.Debug("About to add new season...");
                db.Seasons.Add(s);
                logger.Debug("created new season...");

                db.SaveChanges();
            }
            else
            {
                logger.Debug("About to update season...");
                db.Entry(s).State = EntityState.Modified;

                db.SaveChanges();
                logger.Debug("updated existing season...");

            }
            db.SaveChanges();

            int newId = s.Id;
            logger.Debug("newId = " + s.Id);

            //return new HttpResponseMessage(HttpStatusCode.OK);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, s);
            return response;
        }

        // POST /api/v1/season/removeseason
        [System.Web.Http.HttpPost]
        public HttpResponseMessage RemoveSeason(JObject jsonData)
        {
            logger.Debug("Inside RemoveSeason...");

            var db = ServicesContext.Current;
            dynamic json = jsonData;
            logger.Debug("json = " + json);

            User me = AuthorizationManager.getCurrentUser();
            Project p = db.Projects.Find(json.ProjectId.ToObject<int>());
            logger.Debug("p = " + p.Id);

            int d = json.DatasetId.ToObject<int>();

            if ((!p.isOwnerOrEditor(me)) || (!me.Roles.Contains("Admin")))
                throw new System.Exception("Authorization error.");

            Seasons season = db.Seasons.Find(json.SeasonId.ToObject<int>());
            //logger.Debug("season = " + season.Id);

            if (p == null || season == null)
                throw new System.Exception("Configuration error.  Please try again.");
            
            // The following comands would not work for some reason.
            // The code executed, but when I checked the database, the record was still present.
            // Reverted to the SQL command (farther down) and that did the trick.
            //db.Seasons.Remove(season);
            //db.Entry(season).State = EntityState.Modified;
            //db.SaveChanges();

            //string strTable = ConfigurationManager.AppSettings["SeasonsTbl"];

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                //var query = "DELETE FROM dbo." + strTable + " WHERE DatasetId = " + d + " and Id = " + season.Id;
                var query = "DELETE FROM dbo.Seasons WHERE DatasetId = " + d + " and Id = " + season.Id;
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    logger.Debug(query);
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }

            logger.Debug("Removed season #" + season.Id);

            return new HttpResponseMessage(HttpStatusCode.OK);

        }
    }
}