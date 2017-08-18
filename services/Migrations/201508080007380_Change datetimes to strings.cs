namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changedatetimestostrings : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "Time", c => c.String());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "DateTime", c => c.String());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "StartTime", c => c.String());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "EndTime", c => c.String());
            DropColumn("dbo.SpawningGroundSurvey_Header", "ActivityDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SpawningGroundSurvey_Header", "ActivityDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Header", "EndTime", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Header", "StartTime", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "DateTime", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "Time", c => c.DateTime(nullable: false));
        }
    }
}
