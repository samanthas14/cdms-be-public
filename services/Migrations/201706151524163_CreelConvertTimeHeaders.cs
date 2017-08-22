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
    using services.ExtensionMethods;
    
    public partial class CreelConvertTimeHeaders : DbMigration
    {
        public class CsDateTimeInfo
        {
            public int ActivityId = 0;
            public DateTime ActivityDate;
            public int intCreelSurvey_Header_ID = 0;
            public string strTimeStart = "";
            public DateTime dtTimeStart;
            public string strTimeEnd = "";
            public DateTime dtTimeEnd;
        }

        public override void Up()
        {
            System.IO.StreamWriter outFile = new System.IO.StreamWriter("c:\\gcPrograms\\MigrateOut\\migrateCreelH.txt");

            var db = ServicesContext.Current;

            List<CsDateTimeInfo> csDateTimeInfoList = new List<CsDateTimeInfo>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();
                //                           0         1               2         3            4          5         6
                //string queryCsView = "select a.Id Aid, a.ActivityDate, h.Id Hid, h.TimeStart, h.TimeEnd, d.Id Did, InterviewTime " +
                string queryCsView = "select a.Id Aid, a.ActivityDate, h.Id Hid, h.TimeStart, h.TimeEnd " +
                                    "FROM dbo.Activities a " +
                                    "left outer join dbo.CreelSurvey_HeaderBu h on h.ActivityId = a.Id " +
                                    "where h.Id is not null";

                outFile.WriteLine("SQL command = " + queryCsView);

                using (SqlCommand cmd = new SqlCommand(queryCsView, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CsDateTimeInfo csDateTimeInfo = new CsDateTimeInfo();
                            csDateTimeInfo.ActivityId = Convert.ToInt32(reader.GetValue(0));
                            csDateTimeInfo.ActivityDate = Convert.ToDateTime(reader.GetValue(1));
                            csDateTimeInfo.intCreelSurvey_Header_ID = Convert.ToInt32(reader.GetValue(2));
                            csDateTimeInfo.strTimeStart = reader.GetValue(3).ToString();
                            csDateTimeInfo.strTimeEnd = reader.GetValue(4).ToString();

                            csDateTimeInfoList.Add(csDateTimeInfo);
                            outFile.WriteLine("Aid = " + csDateTimeInfo.ActivityId + ", ADate = " + csDateTimeInfo.ActivityDate + ", Hid = " + csDateTimeInfo.intCreelSurvey_Header_ID + ", TimeStart = " + csDateTimeInfo.strTimeStart + ", TimeEnd = " + csDateTimeInfo.strTimeEnd);
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
                string strStartHour = "";
                string strStartMin = "";
                string strEndHour = "";
                string strEndMin = "";
                //string strIntTime = "";
                //string strIntHour = "";
                //int intIntHour = 0;
                //string strIntMin = "";
                //int IntIntMin = 0;
                string strFullStartTime = "";
                string strFullEndTime = "";
                //string strFullIntTime = "";

                foreach (var item in csDateTimeInfoList)
                {
                    CreelSurvey_Header csHeader = db.CreelSurvey_Header().Find(item.intCreelSurvey_Header_ID);

                    // First copy in all the old data, exept for the StartTime, EndTime.
                    csHeader.Id = csHeader.Id;
                    csHeader.Surveyor = csHeader.Surveyor;
                    csHeader.NumberAnglersObserved = csHeader.NumberAnglersObserved;
                    csHeader.NumberAnglersInterviewed = csHeader.NumberAnglersInterviewed;
                    csHeader.FieldSheetFile = csHeader.FieldSheetFile;
                    csHeader.ActivityId = csHeader.ActivityId;
                    csHeader.ByUser = csHeader.ByUser;
                    csHeader.EffDt = csHeader.EffDt;
                    csHeader.SurveySpecies = csHeader.SurveySpecies;
                    csHeader.WorkShift = csHeader.WorkShift;
                    csHeader.SurveyComments = csHeader.SurveyComments;
                    csHeader.Direction = csHeader.Direction;
                    csHeader.Dry = csHeader.Dry;

                    strYear = item.ActivityDate.Year.ToString();

                    strMonth = item.ActivityDate.Month.ToString();
                    intMonth = Convert.ToInt32(strMonth);
                    if (intMonth < 10)
                        strMonth = "0" + strMonth;

                    strDay = item.ActivityDate.Day.ToString();
                    intDay = Convert.ToInt32(strDay);
                    if (intDay < 10)
                        strDay = "0" + strDay;

                    if (item.strTimeStart.Length > 5)
                    {
                        strStartHour = item.strTimeStart.Substring(1, 2);
                        strStartMin = item.strTimeStart.Substring(4, 2);
                    }
                    else
                    {
                        strStartHour = item.strTimeStart.Substring(0, 2);
                        strStartMin = item.strTimeStart.Substring(3, 2);
                    }
                    strFullStartTime = strYear + "-" + strMonth + "-" + strDay + " " + strStartHour + ":" + strStartMin + ":00.000";
                    csHeader.TimeStart = Convert.ToDateTime(strFullStartTime);

                    if (!String.IsNullOrEmpty(item.strTimeEnd))
                    {
                        strEndHour = item.strTimeEnd.Substring(0, 2);
                        strEndMin = item.strTimeEnd.Substring(3, 2);
                        strFullEndTime = strYear + "-" + strMonth + "-" + strDay + " " + strEndHour + ":" + strEndMin + ":00.000";
                        csHeader.TimeEnd = Convert.ToDateTime(strFullEndTime);
                    }

                    db.Entry(csHeader).State = EntityState.Modified;
                    db.SaveChanges();

                    outFile.Write(csHeader.Id + ", ");
                }
                outFile.WriteLine();
                csDateTimeInfoList.Clear();
            }
            outFile.Close();
        }
        
        public override void Down()
        {
        }
    }
}
