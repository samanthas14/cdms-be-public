namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreelMakeBackups : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                --Getting an error that the table exists, so lets delete the old backups first.
                --drop table dbo.[CreelSurvey_HeaderBu]
                --drop table dbo.[CreelSurvey_DetailBu]

                --Create a backup before we make the updates.
                select * into dbo.[CreelSurvey_HeaderBu] from dbo.[CreelSurvey_Header]
                select * into dbo.[CreelSurvey_DetailBu] from dbo.[CreelSurvey_Detail]
            ");
        }
        
        public override void Down()
        {
            Sql(@"
                if OBJECT_ID('dbo.CreelSurvey_HeaderBu', 'U') is not null drop table dbo.CreelSurvey_HeaderBu
                if OBJECT_ID('dbo.CreelSurvey_DetailBu', 'U') is not null drop table dbo.CreelSurvey_DetailBu
            ");
        }
    }
}
