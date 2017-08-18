namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreStreamNetFixes : DbMigration
    {
        public override void Up()
        {
            Sql(@"
    ALTER TABLE streamnet_rpers_detail ADD CONSTRAINT DF_rpsers_detail DEFAULT 'CTUIR' FOR SubmitAgency;
    go
    ALTER TABLE streamnet_rpers_detail ADD CONSTRAINT  DF_rpers_detail_publish  DEFAULT 'Yes' FOR Publish;
    go
    update streamnet_rpers_detail set SubmitAgency = 'CTUIR'
    update streamnet_rpers_detail set Publish = 'Yes'

    ALTER TABLE streamnet_rpers_detail ALTER COLUMN SubmitAgency nvarchar(max) NOT NULL
    ALTER TABLE streamnet_rpers_detail ALTER COLUMN Publish      nvarchar(max) NOT NULL

");
        }
        
        public override void Down()
        {
            Sql(@"
    alter table streamnet_rpers_detail drop constraint DF_rpsers_detail
    alter table streamnet_rpers_detail drop constraint DF_rpers_detail_publish

    ALTER TABLE streamnet_rpers_detail ALTER COLUMN SubmitAgency nvarchar(max) 
    ALTER TABLE streamnet_rpers_detail ALTER COLUMN Publish      nvarchar(max) 

");
        }
    }
}
