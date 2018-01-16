namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SgsDropPercRetFromDetails : DbMigration
    {
        public override void Up()
        {
            /* Somehow, the database got out of sync with the C# code.
             * The database table SpawningGroundSurvey_Detail contains the column PercentRetained,
             * but the C# code does not.  Usually, such a difference causes lots of problems.
             * In this case, it seems to only prevent PercentRetained from being saved (quite understandable).
             * 
             * To fix the problem, we make a backup of the table, drop the column, and then add the column
             * back in via a Visual Studio migration.
            */

            Sql(@"
                -- Drop any backup copy of the table first, if it exists.
                if OBJECT_ID('dbo.SpawningGroundSurvey_Detail_bu', 'U') IS NOT NULL 
                  DROP TABLE dbo.SpawningGroundSurvey_Detail_bu;

                -- Make a new backup of SpawningGroundSurvey_Detail
                select * into dbo.SpawningGroundSurvey_Detail_bu from dbo.SpawningGroundSurvey_Detail

                -- Drop the PercentRetained column
                alter table dbo.SpawningGroundSurvey_Detail drop column PercentRetained
            ");
        }
        
        public override void Down()
        {
        }
    }
}
