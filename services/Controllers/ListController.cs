using services.Models;
using services.Models.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace services.Controllers
{
    public class ListController : CDMSController
    {


        /*
        * TODO: this is ripe for refactoring
        * We'd like to redesign this and the following controller methods:
        *   1 - move entity logic into the entity
        *   2 - return just the list of ints
        *   3 - maybe use linq instead of raw sql queries:
              (from s in Screwtrap_vw
               where s.DatasetId == 1215 
               select s.MigrationYear).Distinct()

            4 - And maybe best: explore making this soft so that you can ask for these kinds of things without
                special methods... or maybe the dataset entity class can define/expose possible lists of itself.
        */


        //returns empty list if none found...
        // GET /api/v1/list/getmigrationyears/5
        [HttpGet]
        public IEnumerable<ScrewTrap_Detail> GetMigrationYears(int Id)
        {
            var db = ServicesContext.Current;

            List<ScrewTrap_Detail> stDetailList = new List<ScrewTrap_Detail>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "";

                query = "SET QUOTED_IDENTIFIER OFF; SELECT DISTINCT MigrationYear FROM dbo.[Screwtrap_vw] WHERE DatasetId = " + Id + " AND MigrationYear is not null ORDER BY MigrationYear desc";
                logger.Debug("SQL command = " + query);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            //logger.Debug("We have rows...");
                            while (reader.Read())
                            {
                                ScrewTrap_Detail stDet = new ScrewTrap_Detail();
                                stDet.MigrationYear = Convert.ToInt32(reader.GetValue(0).ToString());
                                //logger.Debug("stDet.MigrationYear = " + stDet.MigrationYear);

                                stDetailList.Add(stDet);
                                //logger.Debug("Added record to stDetailList...");
                            }
                        }
                        reader.Close();
                    }
                    cmd.Dispose();
                }
                con.Close();
            }

            return stDetailList.AsEnumerable();
        }

        // GET /api/v1/list/getrunyears/5
        //returns empty list if none found...
        [HttpGet]
        public IEnumerable<AdultWeir_Detail> GetRunYears(int Id)
        {
            List<AdultWeir_Detail> awDetailList = new List<AdultWeir_Detail>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "";

                query = "SET QUOTED_IDENTIFIER OFF; SELECT DISTINCT RunYear from dbo.[AdultWeir_vw] WHERE DatasetId = " + Id + " AND RunYear is not null ORDER BY RunYear desc";
                logger.Debug("SQL command = " + query);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                AdultWeir_Detail awDet = new AdultWeir_Detail();
                                //awDet.RunYear = reader.GetValue(0).ToString();
                                awDet.RunYear = Convert.ToInt32(reader.GetValue(0).ToString());
                                awDetailList.Add(awDet);
                            }
                        }
                        reader.Close();
                    }
                    cmd.Dispose();
                }
                con.Close();
            }

            return awDetailList.AsEnumerable();
        }
        

        // GET /api/v1/list/getreportyears/5
        //returns empty list if none found...
        [HttpGet]
        public IEnumerable<Metrics_Header> GetReportYears(int Id)
        {
            List<Metrics_Header> ryHeaderList = new List<Metrics_Header>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "";

                query = "SET QUOTED_IDENTIFIER OFF; SELECT DISTINCT YearReported FROM dbo.[Metrics_vw] WHERE DatasetId = " + Id + " AND YearReported is not null ORDER BY YearReported desc";
                logger.Debug("SQL command = " + query);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Metrics_Header ryHead = new Metrics_Header();
                                ryHead.YearReported = Convert.ToInt32(reader.GetValue(0).ToString());
                                ryHeaderList.Add(ryHead);
                            }
                        }
                        reader.Close();
                    }
                    cmd.Dispose();
                }
                con.Close();
            }

            return ryHeaderList.AsEnumerable();
        }

        // GET /api/v1/list/getspawningyears/5
        //returns empty list if none found...
        [HttpGet]
        public IEnumerable<StreamNet_NOSA_Detail> GetSpawningYears(int Id)
        {
            List<StreamNet_NOSA_Detail> syDetailList = new List<StreamNet_NOSA_Detail>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "";

                query = "SET QUOTED_IDENTIFIER OFF; SELECT DISTINCT SpawningYear FROM dbo.[StreamNet_NOSA_vw] WHERE DatasetId = " + Id + " AND SpawningYear is not null ORDER BY SpawningYear desc";
                logger.Debug("SQL command = " + query);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                StreamNet_NOSA_Detail syDet = new StreamNet_NOSA_Detail();
                                syDet.SpawningYear = reader.GetValue(0).ToString();
                                syDetailList.Add(syDet);
                            }
                        }
                        reader.Close();
                    }
                    cmd.Dispose();
                }
                con.Close();
            }

            return syDetailList.AsEnumerable();
        }

        // GET /api/v1/list/getbroodyears/5
        //returns empty list if none found...
        [HttpGet]
        public IEnumerable<StreamNet_RperS_Detail> GetBroodYears(int Id)
        {
            List<StreamNet_RperS_Detail> byDetailList = new List<StreamNet_RperS_Detail>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "";

                query = "SET QUOTED_IDENTIFIER OFF; SELECT DISTINCT BroodYear FROM dbo.[StreamNet_RperS_vw] WHERE DatasetId = " + Id + " AND BroodYear is not null ORDER BY BroodYear desc";
                logger.Debug("SQL command = " + query);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                StreamNet_RperS_Detail byDet = new StreamNet_RperS_Detail();
                                byDet.BroodYear = reader.GetValue(0).ToString();
                                byDetailList.Add(byDet);
                            }
                        }
                        reader.Close();
                    }
                    cmd.Dispose();
                }
                con.Close();
            }

            return byDetailList.AsEnumerable();
        }

        // GET /api/v1/list/getoutmigrationyears/5
        //returns empty list if none found...
        [HttpGet]
        public IEnumerable<StreamNet_SAR_Detail> GetOutmigrationYears(int Id)
        {
            List<StreamNet_SAR_Detail> syDetailList = new List<StreamNet_SAR_Detail>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "";

                query = "SET QUOTED_IDENTIFIER OFF; SELECT DISTINCT OutmigrationYear FROM dbo.[StreamNet_SAR_vw] WHERE DatasetId = " + Id + " AND OutmigrationYear is not null ORDER BY OutmigrationYear desc";
                logger.Debug("SQL command = " + query);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                StreamNet_SAR_Detail oyDet = new StreamNet_SAR_Detail();
                                oyDet.OutmigrationYear = reader.GetValue(0).ToString();
                                syDetailList.Add(oyDet);
                            }
                        }
                        reader.Close();
                    }
                    cmd.Dispose();
                }
                con.Close();
            }

            return syDetailList.AsEnumerable();
        }

        // GET /api/v1/list/gettimezones
        [HttpGet]
        public IEnumerable<TimeZoneInfo> GetTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones();
        }

        // GET /api/v1/list/getwaterbodies
        [HttpGet]
        public IEnumerable<WaterBody> GetWaterBodies()
        {
            var db = ServicesContext.Current;
            return db.WaterBodies.OrderBy(o => o.Name).AsEnumerable();
        }

        // GET /api/v1/list/getsources
        [HttpGet]
        public IEnumerable<Source> GetSources()
        {
            var db = ServicesContext.Current;
            return db.Sources.AsEnumerable();
        }

        // GET /api/v1/list/getbenthicsampleyears/5
        //returns empty list if none found...
        [HttpGet]
        public IEnumerable<Benthic_Header> GetBenthicSampleYears(int Id)
        {
            List<Benthic_Header> ryHeaderList = new List<Benthic_Header>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "";

                query = "SET QUOTED_IDENTIFIER OFF; SELECT DISTINCT SampleYear FROM dbo.[Benthic_vw] WHERE DatasetId = " + Id + " AND SampleYear is not null ORDER BY SampleYear desc";
                logger.Debug("SQL command = " + query);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Benthic_Header ryHead = new Benthic_Header();
                                ryHead.SampleYear = Convert.ToInt32(reader.GetValue(0).ToString());
                                ryHeaderList.Add(ryHead);
                            }
                        }
                        reader.Close();
                    }
                    cmd.Dispose();
                }
                con.Close();
            }

            return ryHeaderList.AsEnumerable();
        }

        // GET /api/v1/list/getdriftsampleyears/5
        //returns empty list if none found...
        [HttpGet]
        public IEnumerable<Drift_Header> GetDriftSampleYears(int Id)
        {
            List<Drift_Header> ryHeaderList = new List<Drift_Header>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "";

                query = "SET QUOTED_IDENTIFIER OFF; SELECT DISTINCT SampleYear FROM dbo.[Drift_vw] WHERE DatasetId = " + Id + " AND SampleYear is not null ORDER BY SampleYear desc";
                logger.Debug("SQL command = " + query);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Drift_Header ryHead = new Drift_Header();
                                ryHead.SampleYear = Convert.ToInt32(reader.GetValue(0).ToString());
                                ryHeaderList.Add(ryHead);
                            }
                        }
                        reader.Close();
                    }
                    cmd.Dispose();
                }
                con.Close();
            }

            return ryHeaderList.AsEnumerable();
        }
    }
}
