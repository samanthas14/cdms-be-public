using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using services.Models;
using services.Resources;

/*
 * DatastoreController
 * Think of a "datastore" as a "master dataset" -- The datastore defines the table(s) where the dataset is stored and the information
 * about the master dataset. The "fields" table defines the fields in this master dataset. 
 * Then a "dataset" can be defined for a project that has a subset of those fields (or all of them) defined 
 * for that particular project's version of that "master dataset". 
 */ 

namespace services.Controllers
{
    [System.Web.Http.Authorize]
    public class DatastoreController : CDMSController
    {
     
        [HttpGet]
        public IEnumerable<Datastore> GetAllDatastores()
        {
            var db = ServicesContext.Current;
            return db.Datastores.AsEnumerable();
        }

        /*
         * Get all fields for a field category
         */
        [HttpGet]
        public IEnumerable<Field> GetAllFields(int Id)
        {
            var db = ServicesContext.Current;
            logger.Info("Getting all fields...where FieldCategoryId = " + Id);
            return db.Fields.Where(o => o.FieldCategoryId == Id).OrderBy(o => o.Name).AsEnumerable();
        }


        /*
         * Get a datastore by id
         */ 
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

        /*
         * Get a datastore's locations
         */ 
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

        /*
         * Returns a datastore's fields 
         * Think of this as the list of master fields for the master dataset
         */ 
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

        /*
         * Returns the projects associated with a datastore 
         */ 
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

        /*
         * Returns the children datasets of a datastore
         */ 
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

        /*
         * Add a field (from the list of master fields) to a project's dataset
         */ 
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

        /*
         * Remove a field from a project's version of the dataset
         * (does not remove the master field).
         */ 
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

        /*
         * update a master field's information (a field on a datastore)
         */ 
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

        

       
    }
}
