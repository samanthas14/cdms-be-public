namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreelAddDryAndTagFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CreelSurvey_Detail", "Tag", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "OtherTagId", c => c.String());
            AddColumn("dbo.CreelSurvey_Header", "Dry", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CreelSurvey_Header", "Dry");
            DropColumn("dbo.CreelSurvey_Detail", "OtherTagId");
            DropColumn("dbo.CreelSurvey_Detail", "Tag");
        }
    }
}
