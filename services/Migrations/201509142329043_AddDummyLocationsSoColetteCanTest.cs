using System.Collections.Generic;

namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDummyLocationsSoColetteCanTest : DbMigration
    {
        public override void Up()
        {
            Sql(GetUp("Electrofishing", 2249, 101));
            Sql(GetUp("Electrofishing", 2232, 101));
            Sql(GetUp("Electrofishing", 1135, 101));
            Sql(GetUp("Electrofishing", 1161, 101));
            Sql(GetUp("Electrofishing", 1140, 101));
            Sql(GetUp("Electrofishing", 1188, 101));

            Sql(GetUp("Snorkel Fish", 1135, 102));
            Sql(GetUp("Snorkel Fish", 2249, 102));

            Sql(GetUp("Screw Trap", 1135, 103));
            Sql(GetUp("Screw Trap", 1188, 103));
            Sql(GetUp("Screw Trap", 1177, 103));
            Sql(GetUp("Screw Trap", 1161, 103));

            Sql(GetUp("Fish Scales", 1135, 104));
            Sql(GetUp("Fish Scales", 1177, 104));
            Sql(GetUp("Fish Scales", 1188, 104));

            Sql(GetUp("Water Quality with Labs", 1199, 105));
        }
        
        public override void Down()
        {
            Sql(GetDown("Electrofishing", 2249, 101));
            Sql(GetDown("Electrofishing", 2232, 101));
            Sql(GetDown("Electrofishing", 1135, 101));
            Sql(GetDown("Electrofishing", 1161, 101));
            Sql(GetDown("Electrofishing", 1140, 101));
            Sql(GetDown("Electrofishing", 1188, 101));

            Sql(GetDown("Snorkel Fish", 1135, 102));
            Sql(GetDown("Snorkel Fish", 2249, 102));

            Sql(GetDown("Screw Trap", 1135, 103));
            Sql(GetDown("Screw Trap", 1188, 103));
            Sql(GetDown("Screw Trap", 1177, 103));
            Sql(GetDown("Screw Trap", 1161, 103));

            Sql(GetDown("Fish Scales", 1135, 104));
            Sql(GetDown("Fish Scales", 1177, 104));
            Sql(GetDown("Fish Scales", 1188, 104));

            Sql(GetDown("Water Quality with Labs", 1199, 105));
      }



        static Dictionary<string, bool>  seen = new Dictionary<string, bool>();

        string GetUp(string name, int projectId, int datasetId)
        {
            var locName = "Siktaroo " + projectId + " " + datasetId;
            var sql = 
                @"
-- " + datasetId + @", " + projectId + @"
        SET IDENTITY_INSERT  locationtypes on
"+
    (seen.ContainsKey(name) ? "" :  "insert into locationtypes (name, id) values('" + name + @"'," + datasetId.ToString() + ")") 
+ @"
        INSERT INTO dbo.Locations(LocationTypeId, SdeFeatureClassId, SdeObjectId, Label, CreateDateTime)
        SELECT
          LocationTypeId        = (select id from locationtypes where name = '" + name + @"'),
          SdeFeatureClassId     = 2,
          SdeObjectId           = 33, 
          Label                 = '" + locName + @"',
          CreateDateTime        = GetDate()

       insert into LocationProjects (location_id, project_id) values((select id from Locations  where label = '" + locName + @"'), " + projectId.ToString() + @")

        go
        SET IDENTITY_INSERT  locationtypes off
        go
";

            seen[name] = true;

            return sql;
        }


        string GetDown(string name, int projectId, int datasetId)
        {
            var locName = "Siktaroo " + projectId + " " + datasetId;

            return
                @"
  delete from LocationProjects where location_id = (select id from Locations where label = '" + locName + @"') and project_id = " + projectId.ToString() + @"
  delete from Locations where LocationTypeId = (select id from locationtypes where name = '" + name + @"')
  delete from locationtypes where  name = '" + name + @"'
";
        }
    }
}
