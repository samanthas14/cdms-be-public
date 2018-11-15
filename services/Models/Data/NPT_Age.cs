using System;
using System.Collections.Generic;
using System.Linq;
using NLog; // For the logger.Debug

using services.Models;
using System.Data.Entity;
using services.Models.Data;
using services.ExtensionMethods;

/* 
 * These extension methods make it possible to use linq with ctx.SomeEntity_Header(). See below for example use.
 */
namespace services.ExtensionMethods
{
    public static class NPT_SamplesExtensions
    {
        //Extension method to give ServicesContext this property.
        public static DbSet<NPT_Age_Header> NPT_Age_Header(this ServicesContext ctx)
        {
            return ctx.GetDbSet("NPT_Age_Header").Cast<NPT_Age_Header>();
        }

        public static DbSet<NPT_Age_Detail> NPT_Age_Detail(this ServicesContext ctx)
        {
            return ctx.GetDbSet("NPT_Age_Detail").Cast<NPT_Age_Detail>();
        }
    }
}


namespace services.Models.Data
{
    public class NPT_Age : DatasetData
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Dataset Dataset { get; set; }
        public NPT_Age_Header Header { get; set; }
        public List<NPT_Age_Detail> Details { get; set; }

        public NPT_Age()
        {
            Details = new List<NPT_Age_Detail>();
        }


        // load an existing one
        public NPT_Age(int ActivityId)
        {
            var ndb = ServicesContext.Current;
            Details = new List<NPT_Age_Detail>();

            var headers_q = from h in ndb.NPT_Age_Header()
                            where h.ActivityId == ActivityId
                            join h2 in
                                (
                                    from hh in ndb.NPT_Age_Header()
                                    where hh.EffDt <= DateTime.Now
                                    where hh.ActivityId == ActivityId
                                    group hh by hh.ActivityId into cig
                                    select new { ActivityId = cig.Key, EffDt = cig.Max(ed => ed.EffDt) }
                                ) on new { h.ActivityId, h.EffDt } equals new { h2.ActivityId, h2.EffDt }
                            select h;

            Header = headers_q.SingleOrDefault();

            Dataset = Header.Activity.Dataset;

            //select detail by activityid (taking effdt into account)
            var details_q = from h in ndb.NPT_Age_Detail()
                            where h.ActivityId == ActivityId
                            where h.RowStatusId == DataDetail.ROWSTATUS_ACTIVE
                            join h2 in
                                (
                                    from hh in ndb.NPT_Age_Detail()
                                    where hh.EffDt <= DateTime.Now
                                    where hh.ActivityId == ActivityId
                                    group hh by new { hh.ActivityId, hh.RowId } into cig
                                    select new { ActivityId = cig.Key.ActivityId, RowId = cig.Key.RowId, EffDt = cig.Max(ed => ed.EffDt) }
                                ) on new { h.ActivityId, h.RowId, h.EffDt } equals new { h2.ActivityId, h2.RowId, h2.EffDt }
                            select h;

            foreach (var detail in details_q)
            {
                Details.Add(detail);
            }
        }
    }
}