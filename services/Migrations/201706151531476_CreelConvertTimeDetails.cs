namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Configuration;
    using System.Collections.Generic;
    using services.Models;
    using services.Models.Data;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.IO;
    
    public partial class CreelConvertTimeDetails : DbMigration
    {
        public class CsDateTimeInfo
        {
            public int ActivityId = 0;
            public DateTime ActivityDate;

            public int intCreelSurvey_Detail_ID = 0;
            public string strInterviewTime = "";
            public DateTime dtInterviewTime;
        }

        public override void Up()
        {
            System.IO.StreamWriter outFile = new System.IO.StreamWriter("c:\\gcPrograms\\MigrateOut\\migrateCreelD.txt");

            var db = ServicesContext.Current;

            List<CsDateTimeInfo> csDateTimeInfoList = new List<CsDateTimeInfo>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();
                //                           0         1               2         3            
                string queryCsView = "select a.Id Aid, a.ActivityDate, d.Id Did, InterviewTime " +
                                    "FROM dbo.Activities a " +
                                    "left outer join dbo.CreelSurvey_DetailBu d on d.ActivityId = a.Id " +
                                    "where d.Id is not null";

                outFile.WriteLine("SQL command = " + queryCsView);

                using (SqlCommand cmd = new SqlCommand(queryCsView, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        string strCreelSurvey_Detail_ID = "";
                        while (reader.Read())
                        {
                            CsDateTimeInfo csDateTimeInfo = new CsDateTimeInfo();
                            csDateTimeInfo.ActivityId = Convert.ToInt32(reader.GetValue(0));
                            csDateTimeInfo.ActivityDate = Convert.ToDateTime(reader.GetValue(1));

                            strCreelSurvey_Detail_ID = reader.GetValue(2).ToString();
                            if (!String.IsNullOrEmpty(strCreelSurvey_Detail_ID))
                            {
                                csDateTimeInfo.intCreelSurvey_Detail_ID = Convert.ToInt32(strCreelSurvey_Detail_ID);
                                csDateTimeInfo.strInterviewTime = reader.GetValue(3).ToString();

                                csDateTimeInfoList.Add(csDateTimeInfo);
                            }
                        }
                        reader.Close();
                    }
                    cmd.Dispose();
                }

                string strYear = "";
                string strMonth = "";
                int intMonth = 0;
                string strDay = "";
                int intDay = 0;

                string strIntTime = "";
                string strIntHour = "";
                string strIntMin = "";
                string strFullIntTime = "";

                foreach (var item in csDateTimeInfoList)
                {
                    strIntTime = item.strInterviewTime;
                    if (!String.IsNullOrEmpty(strIntTime))
                    {
                        strIntHour = strIntTime.Substring(0, 2);
                        strIntMin = strIntTime.Substring(3, 2);

                        CreelSurvey_Detail csDetail = db.CreelSurvey_Detail.Find(item.intCreelSurvey_Detail_ID);

                        csDetail.Id = csDetail.Id;
                        csDetail.InterviewComments = csDetail.InterviewComments;
                        csDetail.GPSEasting = csDetail.GPSEasting;
                        csDetail.GPSNorthing = csDetail.GPSNorthing;
                        csDetail.RowId = csDetail.RowId;
                        csDetail.RowStatusId = csDetail.RowStatusId;
                        csDetail.ActivityId = csDetail.ActivityId;
                        csDetail.ByUserId = csDetail.ByUserId;
                        csDetail.QAStatusId = csDetail.QAStatusId;
                        csDetail.EffDt = csDetail.EffDt;
                        csDetail.DetailLocationId = csDetail.DetailLocationId;
                        csDetail.FishermanId = csDetail.FishermanId;
                        csDetail.TotalTimeFished = csDetail.TotalTimeFished;
                        csDetail.FishCount = csDetail.FishCount;
                        csDetail.Species = csDetail.Species;
                        csDetail.MethodCaught = csDetail.MethodCaught;
                        csDetail.Disposition = csDetail.Disposition;
                        csDetail.Sex = csDetail.Sex;
                        csDetail.Origin = csDetail.Origin;
                        csDetail.FinClip = csDetail.FinClip;
                        csDetail.Marks = csDetail.Marks;
                        csDetail.ForkLength = csDetail.ForkLength;
                        csDetail.MeHPLength = csDetail.MeHPLength;
                        csDetail.SnoutId = csDetail.SnoutId;
                        csDetail.ScaleId = csDetail.ScaleId;
                        csDetail.CarcassComments = csDetail.CarcassComments;
                        csDetail.Tag = csDetail.Tag;
                        csDetail.OtherTagId = csDetail.OtherTagId;

                        strYear = item.ActivityDate.Year.ToString();

                        strMonth = item.ActivityDate.Month.ToString();
                        intMonth = Convert.ToInt32(strMonth);
                        if (intMonth < 10)
                            strMonth = "0" + strMonth;

                        strDay = item.ActivityDate.Day.ToString();
                        intDay = Convert.ToInt32(strDay);
                        if (intDay < 10)
                            strDay = "0" + strDay;

                        strIntHour = item.strInterviewTime.Substring(0, 2);
                        strIntMin = item.strInterviewTime.Substring(3, 2);

                        strFullIntTime = strYear + "-" + strMonth + "-" + strDay + " " + strIntHour + ":" + strIntMin + ":00.000";
                        csDetail.InterviewTime = Convert.ToDateTime(strFullIntTime);

                        db.Entry(csDetail).State = EntityState.Modified;
                        db.SaveChanges();

                        outFile.Write(csDetail.Id + ", ");
                    }
                }
            }
            outFile.Close();
        }
        
        public override void Down()
        {
        }
    }
}
