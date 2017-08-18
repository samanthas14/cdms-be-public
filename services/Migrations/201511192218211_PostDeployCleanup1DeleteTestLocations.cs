namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostDeployCleanup1DeleteTestLocations : DbMigration
    {
        public override void Up()
        {
            // These locations were in the test data and must be removed.
            Sql(@"
--delete   FROM [dbo].[Locations]
--where [Id] > 3148 and [Id] < 3220

delete FROM dbo.[Locations] where LocationTypeId = 1 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Buckaroo 1'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 34 and Label = 'Buckaroo 2'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 42 and Label = 'East Meacham 1'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 32 and Label = 'Isqúulktpe 2'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 55 and Label = 'Little Lookingglass Creek - Unit 4'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 51 and Label = 'Lookingglass Creek - Unit 1'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 52 and Label = 'Lookingglass Creek - Unit 2'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 53 and Label = 'Lookingglass Creek - Unit 3L'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 54 and Label = 'Lookingglass Creek - Unit 3U'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 36 and Label = 'Meacham 1'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 37 and Label = 'Meacham 2'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 39 and Label = 'Meacham 4'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 40 and Label = 'Meacham 5'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 41 and Label = 'Meacham 6'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 43 and Label = 'Meacham 7'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 44 and Label = 'Meacham 8'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 35 and Label = 'Moonshine 1'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 29 and Label = 'NF Umatilla 1'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 30 and Label = 'NF Umatilla 2'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 49 and Label = 'Pearson 1'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 50 and Label = 'Pearson 2'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 27 and Label = 'SF Umatilla 1'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 28 and Label = 'Thomas Creek 1'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 1 and Label = 'Umatilla 1'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 2 and Label = 'Umatilla 2'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 3 and Label = 'Umatilla 3'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 4 and Label = 'Umatilla 4'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 5 and Label = 'Umatilla 5'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 6 and Label = 'Umatilla 6'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 7 and Label = 'Umatilla 7'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 8 and Label = 'Umatilla 8'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 9 and Label = 'Umatilla 9'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 10 and Label = 'Umatilla 10'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 11 and Label = 'Umatilla 11'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 12 and Label = 'Umatilla 12'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 13 and Label = 'Umatilla 13'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 14 and Label = 'Umatilla 14'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 15 and Label = 'Umatilla 15'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 16 and Label = 'Umatilla 16'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 17 and Label = 'Umatilla 17'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 18 and Label = 'Umatilla 18'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 19 and Label = 'Umatilla 19'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 20 and Label = 'Umatilla 20'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 21 and Label = 'Umatilla 21'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 22 and Label = 'Umatilla 22'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 23 and Label = 'Umatilla 23'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 24 and Label = 'Umatilla 24'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 25 and Label = 'Umatilla 25'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 26 and Label = 'Umatilla 26'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 45 and Label = 'Willdhorse 1'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 46 and Label = 'Willdhorse 2'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 47 and Label = 'Willdhorse 3'
go
delete FROM dbo.[Locations] where LocationTypeId = 7 and SdeFeatureClassId = 2 and SdeObjectId = 48 and Label = 'Willdhorse 4'
go
delete FROM dbo.[Locations] where LocationTypeId = 101 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Siktaroo 2249 101'
go
delete FROM dbo.[Locations] where LocationTypeId = 101 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Siktaroo 2232 101'
go
delete FROM dbo.[Locations] where LocationTypeId = 101 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Siktaroo 1135 101'
go
delete FROM dbo.[Locations] where LocationTypeId = 101 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Siktaroo 1161 101'
go
delete FROM dbo.[Locations] where LocationTypeId = 101 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Siktaroo 1140 101'
go
delete FROM dbo.[Locations] where LocationTypeId = 101 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Siktaroo 1188 101'
go
delete FROM dbo.[Locations] where LocationTypeId = 102 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Siktaroo 1135 102'
go
delete FROM dbo.[Locations] where LocationTypeId = 102 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Siktaroo 2249 102'
go
delete FROM dbo.[Locations] where LocationTypeId = 103 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Siktaroo 1135 103'
go
delete FROM dbo.[Locations] where LocationTypeId = 103 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Siktaroo 1188 103'
go
delete FROM dbo.[Locations] where LocationTypeId = 103 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Siktaroo 1177 103'
go
delete FROM dbo.[Locations] where LocationTypeId = 103 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Siktaroo 1161 103'
go
delete FROM dbo.[Locations] where LocationTypeId = 104 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Siktaroo 1135 104'
go
delete FROM dbo.[Locations] where LocationTypeId = 104 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Siktaroo 1177 104'
go
delete FROM dbo.[Locations] where LocationTypeId = 104 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Siktaroo 1188 104'
go
delete FROM dbo.[Locations] where LocationTypeId = 105 and SdeFeatureClassId = 2 and SdeObjectId = 33 and Label = 'Siktaroo 1199 105'
go
            ");
        }
        
        public override void Down()
        {
        }
    }
}
