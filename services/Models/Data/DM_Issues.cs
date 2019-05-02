using System;
using System.Collections.Generic;
using System.Linq;
using services.Models;
using System.Data.Entity;
using services.Models.Data;
using services.ExtensionMethods;

/* 
 * These extension methods make it possible to use linq with ctx.SomeEntity_Header(). See below for example use.
 */
namespace services.ExtensionMethods
{
    public static class DM_IssuesExtensions
    {
        //Extension method to give ServicesContext this property.
        public static DbSet<DM_Issues_Header> DM_Issues_Header(this ServicesContext ctx)
        {
            return ctx.GetDbSet("DM_Issues_Header").Cast<DM_Issues_Header>();
        }

        public static DbSet<DM_Issues_Detail> DM_Issues_Detail(this ServicesContext ctx)
        {
            return ctx.GetDbSet("DM_Issues_Detail").Cast<DM_Issues_Detail>();
        }
    }
}

namespace services.Models.Data
{
    public class DM_Issues : DatasetData
    {
        public Dataset Dataset { get; set; }
        public DM_Issues_Header Header { get; set; }
        public List<DM_Issues_Detail> Details { get; set; }

        public DM_Issues()
        {
            Details = new List<DM_Issues_Detail>();
        }


        // load an existing one
        public DM_Issues(int ActivityId)
        {
            var ndb = ServicesContext.Current;
            Details = new List<DM_Issues_Detail>();

            //select header by activityid (taking effdt into account)
            var headers_q = from h in ndb.DM_Issues_Header()
                            where h.ActivityId == ActivityId
                            join h2 in
                                (
                                    from hh in ndb.DM_Issues_Header()
                                    where hh.EffDt <= DateTime.Now
                                    where hh.ActivityId == ActivityId
                                    group hh by hh.ActivityId into cig
                                    select new { ActivityId = cig.Key, EffDt = cig.Max(ed => ed.EffDt) }
                                ) on new { h.ActivityId, h.EffDt } equals new { h2.ActivityId, h2.EffDt }
                            select h;

            //should only be 1 -- if more than one, this will give the last one.
            Header = headers_q.SingleOrDefault();

            //set the dataset now from the relationship via the activity.
            Dataset = Header.Activity.Dataset;

            //select detail by activityid (taking effdt into account)
            var details_q = from h in ndb.DM_Issues_Detail()
                            where h.ActivityId == ActivityId
                            where h.RowStatusId == DataDetail.ROWSTATUS_ACTIVE
                            join h2 in
                                (
                                    from hh in ndb.DM_Issues_Detail()
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