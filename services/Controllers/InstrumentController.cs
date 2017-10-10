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
    public class InstrumentController : CDMSController
    {
        [System.Web.Http.HttpGet]
        public IEnumerable<Instrument> GetAllInstruments()
        {
            var db = ServicesContext.Current;
            return db.Instruments.OrderBy(o => o.Name).ThenBy(o => o.SerialNumber).AsEnumerable();
        }

        [System.Web.Http.HttpGet]
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

        [System.Web.Http.HttpGet]
        public IEnumerable<InstrumentType> GetInstrumentTypes()
        {
            var db = ServicesContext.Current;
            return db.InstrumentType.AsEnumerable();
        }

        [System.Web.Http.HttpPost]
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

        [System.Web.Http.HttpPost]
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

        [System.Web.Http.HttpPost]
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

        [System.Web.Http.HttpPost]
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
    }
}
