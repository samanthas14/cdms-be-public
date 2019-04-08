using System;
using System.Collections.Generic;
using System.Linq;
using NLog; // For the logger.Debug
using services.Models;
using System.Data.Entity;
using services.Models.Data;
using services.ExtensionMethods;

namespace services.ExtensionMethods
{
    public static class VisitMetricsExtensions
    {
        //Extension method to give ServicesContext this property.
        public static DbSet<VisitMetrics_Header> VisitMetrics_Header(this ServicesContext ctx)
        {
            return ctx.GetDbSet("VisitMetrics_Header").Cast<VisitMetrics_Header>();
        }

        public static DbSet<VisitMetrics_Detail> VisitMetrics_Detail(this ServicesContext ctx)
        {
            return ctx.GetDbSet("VisitMetrics_Detail").Cast<VisitMetrics_Detail>();
        }
    }
}

namespace services.Models.Data
{
    public class VisitMetrics : DatasetData
    {
        private Boolean debugMode = true;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Dataset Dataset { get; set; }
        public VisitMetrics_Header Header { get; set; }
        public List<VisitMetrics_Detail> Details { get; set; }

        public VisitMetrics()
        {
            Details = new List<VisitMetrics_Detail>();
        }


        // load an existing one
        public VisitMetrics(int ActivityId)
        {
            var db = ServicesContext.Current;
            if (debugMode) logger.Info("db = " + db);
            Details = new List<VisitMetrics_Detail>();

            //select header by activityid (taking effdt into account)
            var headers_q = from h in db.VisitMetrics_Header()
                            where h.ActivityId == ActivityId
                            join h2 in
                                (
                                    from hh in db.VisitMetrics_Header()
                                    where hh.EffDt <= DateTime.Now
                                    where hh.ActivityId == ActivityId
                                    group hh by hh.ActivityId into cig
                                    select new { ActivityId = cig.Key, EffDt = cig.Max(ed => ed.EffDt) }
                                ) on new { h.ActivityId, h.EffDt } equals new { h2.ActivityId, h2.EffDt }
                            select h;
            //if (debugMode) logger.Info("headers_q = " + headers_q);

            //should only be 1 -- if more than one, this will give the last one.
            //if (debugMode) logger.Info("Header is next...");
            Header = headers_q.SingleOrDefault();
            if (debugMode) logger.Info("Header = " + Header);

            //set the dataset now from the relationship via the activity.
            //if (debugMode) logger.Info("Dataset is next...");
            Dataset = Header.Activity.Dataset;
            if (debugMode) logger.Info("Dataset = " + Dataset);

            //select detail by activityid (taking effdt into account)
            var details_q = from h in db.VisitMetrics_Detail()
                            where h.ActivityId == ActivityId
                            where h.RowStatusId == DataDetail.ROWSTATUS_ACTIVE
                            join h2 in
                                (
                                    from hh in db.VisitMetrics_Detail()
                                    where hh.EffDt <= DateTime.Now
                                    where hh.ActivityId == ActivityId
                                    group hh by new { hh.ActivityId, hh.RowId } into cig
                                    select new { ActivityId = cig.Key.ActivityId, RowId = cig.Key.RowId, EffDt = cig.Max(ed => ed.EffDt) }
                                ) on new { h.ActivityId, h.RowId, h.EffDt } equals new { h2.ActivityId, h2.RowId, h2.EffDt }
                            select h;
            //if (debugMode) logger.Info("details_q = " + details_q);

            foreach (var detail in details_q)
            {
                //if (debugMode) logger.Info("About to add " + detail + " to Datails...");
                Details.Add(detail);
                //if (debugMode) logger.Info("Added the detail...");
            }
            if (debugMode) logger.Info("Details = " + Details);
        }
    }
}