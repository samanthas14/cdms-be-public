using services.Models;
using services.Models.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace services.Controllers
{
    public class AnalyticsController : CDMSController
    {
        // GET /api/v1/analytics/UserLoginsPastMonth
        [HttpGet]
        public DataTable UserLoginsPastMonth()
        {
            var db = ServicesContext.Current;
            DataTable datatable = new DataTable();

            var query = @"
                select 
                convert(varchar(10), RequestTimestamp, 120) as RequestDate,
                count(*) as Logins
                from __Analytics 
                where 
                RequestTimestamp >= dateadd(Day, -30, GetDate())
                and
                action = 'login'
                and username != ''
                group by convert(varchar(10), RequestTimestamp, 120)";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.CommandTimeout = 120; // 2 minutes in seconds.

                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.SelectCommand.CommandTimeout = 120; // 2 minutes in seconds
                    da.Fill(datatable);
                }
                catch (SqlException e)
                {
                    logger.Debug("Query sql failed:" + e.Message);
                    logger.Debug(e.InnerException);
                }
            }

            return datatable;
        }

        // GET /api/v1/analytics/UserRequestsTotalPastMonth
        [HttpGet]
        public DataTable UserRequestsTotalPastMonth()
        {
            var db = ServicesContext.Current;
            DataTable datatable = new DataTable();

            var query = @"
                select top 10
                u.Fullname, 
                d.Name,
                count(*) as Logins
                from __Analytics a
                join users u on u.username = a.username
                join departments d on u.DepartmentId = d.Id
                where 
                a.RequestTimestamp >= dateadd(Day, -30, GetDate())
                and a.username != ''
                group by u.Fullname, d.Name order by Logins DESC";


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.CommandTimeout = 120; // 2 minutes in seconds.

                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.SelectCommand.CommandTimeout = 120; // 2 minutes in seconds
                    da.Fill(datatable);
                }
                catch (SqlException e)
                {
                    logger.Debug("Query sql failed:" + e.Message);
                    logger.Debug(e.InnerException);
                }
            }

            return datatable;
        }


        // GET /api/v1/analytics/DatasetRequestsTotalPastMonth
        [HttpGet]
        public DataTable DatasetRequestsTotalPastMonth()
        {
            var db = ServicesContext.Current;
            DataTable datatable = new DataTable();

            var query = @"
                select top 10 
                d.Name,
                count(*) as Requests
                from __Analytics a
                join datasets d on a.target = d.Id
                where
                a.route = 'dataset' and
                a.RequestTimestamp >= dateadd(Day, -30, GetDate())
                group by d.Name order by Requests DESC";


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.CommandTimeout = 120; // 2 minutes in seconds.

                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.SelectCommand.CommandTimeout = 120; // 2 minutes in seconds
                    da.Fill(datatable);
                }
                catch (SqlException e)
                {
                    logger.Debug("Query sql failed:" + e.Message);
                    logger.Debug(e.InnerException);
                }
            }

            return datatable;
        }

        // GET /api/v1/analytics/LastUpdatedDatasets
        [HttpGet]
        public DataTable LastUpdatedDatasets()
        {
            var db = ServicesContext.Current;
            DataTable datatable = new DataTable();

            var query = @"select top 10 * from LastUpdatedDatasets_VW order by CreateDate DESC";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.CommandTimeout = 120; // 2 minutes in seconds.

                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.SelectCommand.CommandTimeout = 120; // 2 minutes in seconds
                    da.Fill(datatable);
                }
                catch (SqlException e)
                {
                    logger.Debug("Query sql failed:" + e.Message);
                    logger.Debug(e.InnerException);
                }
            }

            return datatable;
        }


    }

}
