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
    public class MetadataController : CDMSController
    {
        // GET api/v1/metadata/getmetadataproperties
        public IEnumerable<MetadataProperty> GetMetadataProperties()
        {
            var db = ServicesContext.Current;
            var metadataproperty = db.MetadataProperty.Include(m => m.MetadataEntity);
            return metadataproperty.AsEnumerable();
        }

        // GET api/Metadata/5
        public MetadataProperty GetMetadataProperty(int id)
        {
            var db = ServicesContext.Current;
            MetadataProperty metadataproperty = db.MetadataProperty.Find(id);
            if (metadataproperty == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return metadataproperty;
        }

        // PUT api/Metadata/5
        public HttpResponseMessage PutMetadataProperty(int id, MetadataProperty metadataproperty)
        {
            var db = ServicesContext.Current;
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != metadataproperty.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(metadataproperty).State = EntityState.Modified;

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

        // POST api/Metadata
        public HttpResponseMessage PostMetadataProperty(MetadataProperty metadataproperty)
        {
            var db = ServicesContext.Current;
            if (ModelState.IsValid)
            {
                db.MetadataProperty.Add(metadataproperty);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, metadataproperty);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = metadataproperty.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Metadata/5
        public HttpResponseMessage DeleteMetadataProperty(int id)
        {
            var db = ServicesContext.Current;
            MetadataProperty metadataproperty = db.MetadataProperty.Find(id);
            if (metadataproperty == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.MetadataProperty.Remove(metadataproperty);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, metadataproperty);
        }

        //TODO: not sure why this is a POST?

        // POST /api/v1/metadata/getmetadatafor
        [HttpPost]
        public IEnumerable<MetadataValue> GetMetadataFor(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;

            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            int EntityTypeId = json.EntityTypeId.ToObject<int>();

            if (project == null || me == null)
                throw new Exception("GetMetadataFor: Configuration error. Please try again.");

            return MetadataHelper.getMetadata(project.Id, EntityTypeId).AsEnumerable();

        }

        // POST /api/v1/metadata/setdatasetmetadata
        [HttpPost]
        public HttpResponseMessage SetDatasetMetadata(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;

            Dataset dataset = db.Datasets.Find(json.DatasetId.ToObject<int>());
            if (dataset == null)
                throw new Exception("SetDatasetMetadata: Configuration error.");

            Project project = db.Projects.Find(dataset.ProjectId);

            User me = AuthorizationManager.getCurrentUser();
            if (!project.isOwnerOrEditor(me))
                throw new Exception("SetDatasetMetadata: Configuration error.");

            //Now save metadata
            List<MetadataValue> metadata = new List<MetadataValue>();

            foreach (var jmv in json.Metadata)
            {
                var mv = jmv.ToObject<MetadataValue>();
                mv.UserId = me.Id;
                metadata.Add(mv);
                //logger.Debug("Found new metadata: " + mv.MetadataPropertyId + " + + " + mv.Values);
            }

            //fire setMetdata which will handle persisting the metadata
            dataset.Metadata = metadata;

            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);

        }
    }
}