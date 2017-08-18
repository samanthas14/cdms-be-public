using System.Collections.Generic;

namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
using System.Collections.Generic;
    
    public partial class AnotherDummyLocation : DbMigration
    {

        public override void Up()
        {
            Sql(GetUp("StreamNet_RperS", 9999, 106));
        }


        public override void Down()
        {
            Sql(GetDown("StreamNet_RperS", 9999, 106));
        }



        static Dictionary<string, bool>  seen = new Dictionary<string, bool>();

        string GetUp(string name, int projectId, int datasetId)
        {
            var locName = "StreamNetLoc " + projectId + " " + datasetId;
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

