namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeintsoptionalII : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "FeatureID", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "Temp", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "Easting", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "Northing", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "WaypointNumber", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "FishCount", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "SpawningStatus", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "ForkLength", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "MeHPLength", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "Ident", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "EastingUTM", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "NorthingUTM", c => c.Int());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "NorthingUTM", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "EastingUTM", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "Ident", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "MeHPLength", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "ForkLength", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "SpawningStatus", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "FishCount", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "WaypointNumber", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "Northing", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "Easting", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "Temp", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "FeatureID", c => c.Int(nullable: false));
        }
    }
}
