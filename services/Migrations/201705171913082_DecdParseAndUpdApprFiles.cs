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
    using services.ExtensionMethods;
    
    public partial class DecdParseAndUpdApprFiles : DbMigration
    {
        public class AppraisalRecord
        {
            public int Id = 0;
            public string AppraisalFiles = "";
        }

        public class FileRecordsOldAndNew
        {
            public int FileNewId = 0;
            public string FileName1 = "";
            public int FileOldId = 0;
            public string FileName2 = "";
        }

        //private static Logger logger = LogManager.GetCurrentClassLogger();

        public override void Up()
        {
            System.IO.StreamWriter outFile = new System.IO.StreamWriter("c:\\gcPrograms\\MigrateOut\\migrate.txt");

            Sql(@"
                select * into Appraisal_DetailBu from Appraisal_Detail
            ");

            var db = ServicesContext.Current;

            List<Appraisal_Detail> appraisalDetail = db.Appraisal_Detail().AsEnumerable().ToList();
            foreach (var aDet in appraisalDetail)
            {
                outFile.WriteLine("aDet.Id = " + aDet.Id + ", aDet.AppraisalFiles = " + aDet.AppraisalFiles);
            }
            outFile.WriteLine("appraisalDetail record count = " + appraisalDetail.Count());

            List<AppraisalRecord> appraisalFiles = new List<AppraisalRecord>();
            List<FileRecordsOldAndNew> fileRecordsOldAndNew = new List<FileRecordsOldAndNew>();
            //List<Appraisal_Detail> appraisalDetail = new List<Appraisal_Detail>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                // Fetch the data from the temporary table containing the pertinent items from the new records.
                //var queryAppraisals = "SELECT * FROM dbo.[AppraisalRecords]";
                var queryAppraisals = "SELECT * FROM dbo.[AppraisalFiles]";
                //logger.Debug("SQL command = " + queryAppraisals);
                outFile.WriteLine("SQL command = " + queryAppraisals);

                using (SqlCommand cmd = new SqlCommand(queryAppraisals, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        AppraisalRecord fileRecord = new AppraisalRecord();
                        while (reader.Read())
                        {
                            fileRecord.Id = Convert.ToInt32(reader.GetValue(0));
                            fileRecord.AppraisalFiles = reader.GetValue(1).ToString();

                            //logger.Debug("DetailId = " + fileRecord.DetailsId + ", FileData = " + fileRecord.AppraisalFiles);
                            outFile.WriteLine("Id = " + fileRecord.Id + ", FileData = " + fileRecord.AppraisalFiles);

                            appraisalFiles.Add(fileRecord);
                        }
                        reader.Close();
                    }


                    //logger.Debug("Appraisal Record count = " + appraisalFiles.Count);
                    outFile.WriteLine("Appraisal Record count = " + appraisalFiles.Count);

                    cmd.Dispose();
                }

                // Fetch the data from the temporary table containing items from old and new records, to compare each other against.
                var queryDetailsOldNew = "select f.Id FId, f.Name FName, fn.Id fnId, fn.Name fnName " +
                            "from dbo.Files f " +
                            "inner join dbo.FilesNewDecd fn on fn.UserId = f.UserId and fn.Name = f.Name and fn.Title = f.Title and fn.Description = f.Description";
                //logger.Debug("SQL command = " + queryDetailsOldNew);
                outFile.WriteLine("SQL command = " + queryDetailsOldNew);

                using (SqlCommand cmd = new SqlCommand(queryDetailsOldNew, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FileRecordsOldAndNew froan = new FileRecordsOldAndNew();
                            froan.FileNewId = Convert.ToInt32(reader.GetValue(0));
                            froan.FileName1 = reader.GetValue(1).ToString();
                            froan.FileOldId = Convert.ToInt32(reader.GetValue(2));
                            froan.FileName2 = reader.GetValue(3).ToString();
                            //logger.Debug("froan.FileNewId = " + froan.FileNewId + ", froan.FileOldId = " + froan.FileOldId + ", froan.FileName1 = " + froan.FileName1);

                            fileRecordsOldAndNew.Add(froan);
                        }
                        reader.Close();
                    }

                    //file.WriteLine("Appraisal Record count = " + appraisalFiles.Count);
                    outFile.WriteLine("Appraisal fileRecordsOldAndNew count = " + fileRecordsOldAndNew.Count);

                    cmd.Dispose();
                }



                // Now check the results and update the Detail AppraisalFiles.
                foreach (var fileRec in fileRecordsOldAndNew)
                {
                    foreach (var apprRec in appraisalFiles)
                    {
                        string str1 = apprRec.AppraisalFiles.Substring(7);
                        string strFileId = str1.Substring(0, 4);
                        outFile.WriteLine("strFileId = " + strFileId + ", fileRec.Id = " + fileRec.FileOldId);
                        if (strFileId.IndexOf(fileRec.FileOldId.ToString()) > -1)
                        {
                            outFile.WriteLine("Matched one...");
                            int intFileId = Convert.ToInt32(strFileId);

                            foreach (var detRec in appraisalDetail)
                            {
                                if (!String.IsNullOrEmpty(detRec.AppraisalFiles))
                                {
                                    string strD1 = detRec.AppraisalFiles.Substring(7);
                                    string strDFileId = strD1.Substring(0, 4);
                                    //var db = ServicesContext.Current;

                                    if (strDFileId == strFileId)
                                    {
                                        outFile.WriteLine("Detail " + detRec.Id + " has this file:  " + strFileId + ".  Updating...");

                                        var theAppraisal_Detail = db.Appraisal_Detail().Find(detRec.Id);

                                        string str3 = str1.Substring(4);
                                        string newString = "[{\"Id\":" + fileRec.FileNewId.ToString() + str3;

                                        //Appraisal_Detail d1 = new Appraisal_Detail();

                                        Appraisal_Detail d1 = db.Appraisal_Detail().Find(detRec.Id);

                                        //d1.Id = theAppraisal_Detail.Id;
                                        d1.AppraisalYear = theAppraisal_Detail.AppraisalYear;

                                        d1.AppraisalFiles = newString;

                                        d1.AppraisalPhotos = theAppraisal_Detail.AppraisalPhotos;
                                        d1.AppraisalComments = theAppraisal_Detail.AppraisalComments;
                                        d1.AppraisalStatus = theAppraisal_Detail.AppraisalStatus;
                                        d1.RowId = theAppraisal_Detail.RowId;
                                        d1.RowStatusId = theAppraisal_Detail.RowStatusId;
                                        d1.ActivityId = theAppraisal_Detail.ActivityId;
                                        d1.ByUserId = theAppraisal_Detail.ByUserId;
                                        d1.QAStatusId = theAppraisal_Detail.QAStatusId;
                                        d1.EffDt = theAppraisal_Detail.EffDt;
                                        d1.AppraisalType = theAppraisal_Detail.AppraisalType;
                                        d1.AppraisalLogNumber = theAppraisal_Detail.AppraisalLogNumber;
                                        d1.AppraisalValue = theAppraisal_Detail.AppraisalValue;
                                        d1.AppraisalValuationDate = theAppraisal_Detail.AppraisalValuationDate;
                                        d1.Appraiser = theAppraisal_Detail.Appraiser;
                                        d1.TypeOfTransaction = theAppraisal_Detail.TypeOfTransaction;
                                        d1.PartiesInvolved = theAppraisal_Detail.PartiesInvolved;
                                        d1.AppraisalProjectType = theAppraisal_Detail.AppraisalProjectType;

                                        db.Entry(d1).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
                //logger.Debug("updated Appraisal_Detail...");
                outFile.WriteLine("updated Appraisal_Detail...");
                con.Close();
            }
            outFile.Close();
        }
        
        public override void Down()
        {
        }
    }
}
