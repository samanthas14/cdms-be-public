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
    public static class StreamNet_RperSExtensions
    {
        //Extension method to give ServicesContext this property.
        public static DbSet<StreamNet_RperS_Header> StreamNet_RperS_Header(this ServicesContext ctx)
        {
            return ctx.GetDbSet("StreamNet_RperS_Header").Cast<StreamNet_RperS_Header>();
        }

        public static DbSet<StreamNet_RperS_Detail> StreamNet_RperS_Detail(this ServicesContext ctx)
        {
            return ctx.GetDbSet("StreamNet_RperS_Detail").Cast<StreamNet_RperS_Detail>();
        }
    }
}

namespace services.Models.Data
{
    public class StreamNet_RperS : DatasetData
    {
        public Dataset Dataset { get; set; }
        public StreamNet_RperS_Header Header { get; set; }
        public List<StreamNet_RperS_Detail> Details { get; set; }

        public StreamNet_RperS()
        {
            Details = new List<StreamNet_RperS_Detail>();
        }


        // load an existing one
        public StreamNet_RperS(int ActivityId)
        {
            var ndb = ServicesContext.Current;
            Details = new List<StreamNet_RperS_Detail>();

            //select header by activityid (taking effdt into account)
            var headers_q = from h in ndb.StreamNet_RperS_Header()
                            where h.ActivityId == ActivityId
                            join h2 in
                                (
                                    from hh in ndb.StreamNet_RperS_Header()
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
            var details_q = from h in ndb.StreamNet_RperS_Detail()
                            where h.ActivityId == ActivityId
                            where h.RowStatusId == DataDetail.ROWSTATUS_ACTIVE
                            join h2 in
                                (
                                    from hh in ndb.StreamNet_RperS_Detail()
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