namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using services.Models;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Collections.Generic;

    public partial class CrppReviseDbData : DbMigration
    {
        public class countyData
        {
            public int intId = 0;
            public int intPid = 0;
            public string County = "";
            public string otherCounty = "";
        }
        public override void Up()
        {
            //System.IO.StreamWriter outFile = new System.IO.StreamWriter("c:\\gcPrograms\\MigrateOut\\migrate2.txt");
            //outFile.WriteLine("Test up");

            var db = ServicesContext.Current;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "select Id, ProjectId, County, OtherCounty from dbo.Subproject_Crpp where Id > 1";

                County cty;
                //List<County> lstCounties = new List<County>();
                countyData cData;
                List<countyData> lstCounties = new List<countyData>();

                string strCounty = "";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cData = new countyData();
                            cData.intId = Convert.ToInt32(reader.GetValue(0));
                            //cData.intPid = Convert.ToInt32(reader.GetValue(1));
                            cData.intPid = 2247;
                            cData.County = reader.GetValue(2).ToString();
                            cData.otherCounty = reader.GetValue(3).ToString();

                            lstCounties.Add(cData);
                        }
                        reader.Close();
                    }
                    cmd.Dispose();
                }

                List<string> listOfCounties = new List<string>();
                listOfCounties.Add("Asotin");
                listOfCounties.Add("Baker");
                listOfCounties.Add("Benton");
                listOfCounties.Add("Clark");
                listOfCounties.Add("Columbia");
                listOfCounties.Add("Franklin");
                listOfCounties.Add("Garfield");
                listOfCounties.Add("Gilliam");
                listOfCounties.Add("Grant, WA");
                listOfCounties.Add("Grant, OR");
                listOfCounties.Add("Hood River");
                listOfCounties.Add("Klickitat");
                listOfCounties.Add("Malheur");
                listOfCounties.Add("Morrow");
                listOfCounties.Add("Multnomah");
                listOfCounties.Add("Other");
                listOfCounties.Add("Sherman");
                listOfCounties.Add("Skamania");
                listOfCounties.Add("Umatilla");
                listOfCounties.Add("Union");
                listOfCounties.Add("Walla Walla");
                listOfCounties.Add("Wallowa");
                listOfCounties.Add("Wasco");
                listOfCounties.Add("Wheeler");
                listOfCounties.Add("Whitman");

                foreach (var c in lstCounties)
                {
                    if (!String.IsNullOrEmpty(c.County))
                    {
                        // Has Grant, OR in the field (may or may not be multiple)
                        // Because of the comma in "Grant, OR", it requires special handling.
                        // We CANNOT just split on the comma, and send the items to the Counties table.
                        if (c.County.IndexOf("Grant, OR") > -1)
                        {
                            cty = new County();

                            if (c.County.Length == 9) // Grant, OR only
                            {
                                cty.ProjectId = c.intPid;
                                cty.SubprojectId = c.intId;
                                cty.Name = c.County;

                                db.Counties.Add(cty);
                                db.SaveChanges();
                            }
                            else // County has multiple counties, and "Grant, OR" is one of them.
                            {
                                // First we will handle the text "Grant, OR".
                                strCounty = c.County;

                                // Just remove "Grant, OR" from the string.
                                strCounty = strCounty.Replace("Grant, OR,", ""); // Remove the trailing comma also.

                                // Build out county object, and set the name to "Grant, OR"
                                cty.ProjectId = c.intPid;
                                cty.SubprojectId = c.intId;
                                cty.Name = "Grant, OR";

                                db.Counties.Add(cty);
                                db.SaveChanges();

                                // After removing "Grant, OR" from the string, we can not just split on the commas.
                                var lstCounty = strCounty.Split(',');
                                foreach (var item in lstCounty)
                                {
                                    cty = new County();

                                    cty.ProjectId = c.intPid;
                                    cty.SubprojectId = c.intId;
                                    cty.Name = item.Trim();

                                    db.Counties.Add(cty);
                                    db.SaveChanges();
                                }

                            }
                        }
                        // OK, "Grant, OR" is not in the field.  Does the field have multiple counties?
                        else if (c.County.IndexOf(",") > -1)
                        {
                            // Yes.  We can just split on the comma.
                            var lstCounty = c.County.Split(',');
                            foreach (var item in lstCounty)
                            {
                                cty = new County();

                                cty.ProjectId = c.intPid;
                                cty.SubprojectId = c.intId;
                                cty.Name = item.Trim();

                                db.Counties.Add(cty);
                                db.SaveChanges();
                            }
                        }
                        // OK.  The field is not null, has no "Grant, OR", and is single county.
                        // We can just save this one.
                        else
                        {
                            cty = new County();

                            cty.ProjectId = c.intPid;
                            cty.SubprojectId = c.intId;
                            cty.Name = c.County.Trim();

                            db.Counties.Add(cty);
                            db.SaveChanges();
                        }

                        // Now let's check the OtherCounty field.
                        // Note:  If the items in OtherCounty are already in the County List, we can remove those.
                        if (c.otherCounty != null)
                        {
                            // The field has something.
                            // Does the field have multiple counties?
                            if (c.otherCounty.IndexOf(",") > -1)
                            {
                                // The field has multiple counties.
                                var lstCounty = c.otherCounty.Split(',');
                                foreach (var item in lstCounty)
                                {
                                    // Is this item in listOfCounties?
                                    if (listOfCounties.IndexOf(item) > -1)
                                    {
                                        // This item is already in the list, so we can skip it.
                                    }
                                    else
                                    {
                                        // This item is not in the list, so we keep it.
                                        /*cty = new County();

                                        cty.ProjectId = c.intPid;
                                        cty.SubprojectId = c.intId;
                                        cty.Name = item.Trim();

                                        db.Counties.Add(cty);
                                        db.SaveChanges();
                                        */
                                    }
                                }
                            }
                            /*else
                            {
                                // OtherCounty has only one item.
                                // Is this item in listOfCounties?
                                if (listOfCounties.IndexOf(c.otherCounty) > -1)
                                {
                                    // This item is already in the list, so we can skip it.
                                }
                                else
                                {
                                    // This item is not in the list, so we keep it.
                                    cty = new County();

                                    cty.ProjectId = c.intPid;
                                    cty.SubprojectId = c.intId;
                                    cty.Name = c.otherCounty.Trim();

                                    db.Counties.Add(cty);
                                    db.SaveChanges();
                                }
                            }
                            */
                        }
                    }
                }
            }
        }

        public override void Down()
        {
        }
    }
}
