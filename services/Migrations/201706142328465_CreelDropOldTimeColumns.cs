namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreelDropOldTimeColumns : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CreelSurvey_Detail", "InterviewTime");
            DropColumn("dbo.CreelSurvey_Header", "TimeStart");
            DropColumn("dbo.CreelSurvey_Header", "TimeEnd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CreelSurvey_Header", "TimeEnd", c => c.String());
            AddColumn("dbo.CreelSurvey_Header", "TimeStart", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "InterviewTime", c => c.String());
        }
    }
}
