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
using System.Data;

namespace services.Controllers
{
    public class LookupTableController : CDMSController
    {
        //GET /api/v1/lookuptable/forproject/5
        [System.Web.Http.HttpGet]
        public dynamic ForProject(int Id)
        {
            var ndb = ServicesContext.Current;
            return ndb.Projects.Find(Id).LookupTables.AsEnumerable();

        }


        //GET /api/v1/lookuptable/items/5
        [System.Web.Http.HttpGet]
        public dynamic GetItems(int DatastoreId)
        {
            var ndb = ServicesContext.Current;
            Datastore lookuptable = ndb.Datastores.Find(DatastoreId);
    
            if(lookuptable == null)
                throw new System.Exception("Configuration error.");

            var sql = "SELECT * FROM " + lookuptable.TablePrefix;

            DataTable items = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                //using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(items);
                }
            }

            return items;

        }

/*
 * { DatastoreId: 7, Item: { item } } 
*/ 


        // POST /api/v1/item/saveitem
        public HttpResponseMessage SaveItem(JObject jsonData)
        {
            logger.Debug("Inside SaveItem...");
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            int DatastoreId = json.DatastoreId.ToObject<int>();

            Datastore lookuptable = db.Datastores.Find(DatastoreId);

            if (lookuptable == null)
                throw new System.Exception("Configuration error, no DatastoreId in payload.");

            User me = AuthorizationManager.getCurrentUser();

            var dbset = db.GetDbSet(lookuptable.TablePrefix);
            var lookuptable_type = db.GetTypeFor(lookuptable.TablePrefix);

            var incoming_item = json.Item.ToObject(lookuptable_type);

            if (incoming_item.Id == 0)
            {
                logger.Debug("About to add new item...");
                dbset.Add(incoming_item);

                db.SaveChanges();
            }
            else
            {
                logger.Debug("About to update item...");
//                var update_item = dbset.Find(incoming_item.Id);

//                if (update_item == null)
//                    throw new Exception("Item to update with that ID was not found.");

                db.Entry(incoming_item).State = EntityState.Modified;

                db.SaveChanges();
                logger.Debug("updated existing item...");

            }
            
            return Request.CreateResponse(HttpStatusCode.Created, (Object)incoming_item);
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