namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreelCreateNewTimeColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CreelSurvey_Detail", "InterviewTime", c => c.DateTime());
            AddColumn("dbo.CreelSurvey_Header", "TimeStart", c => c.DateTime());
            AddColumn("dbo.CreelSurvey_Header", "TimeEnd", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CreelSurvey_Header", "TimeEnd");
            DropColumn("dbo.CreelSurvey_Header", "TimeStart");
            DropColumn("dbo.CreelSurvey_Detail", "InterviewTime");
        }
    }
}
