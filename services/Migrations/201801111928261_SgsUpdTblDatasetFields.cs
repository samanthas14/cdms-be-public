namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SgsUpdTblDatasetFields : DbMigration
    {
        public override void Up()
        {
            /* For the following datasets (WW, GR, Bio), the 'Percent Retained' label maps to DbColumnName 'SpawningStatus'
             * This migration changes DbColumnName to 'PercentRetained'
            */
            Sql(@"
update dbo.DatasetFields
set DbColumnName = 'PercentRetained'
where DatasetId in (select Id from dbo.Datasets where Name like '%Spawning Ground%')
and Label = 'Percent Retained'
            ");
        }
        
        public override void Down()
        {
            Sql(@"
update dbo.DatasetFields
set DbColumnName = 'SpawningStatus'
where DatasetId in (select Id from dbo.Datasets where Name like '%Spawning Ground%')
and Label = 'Percent Retained'
            ");
        }
    }
}
