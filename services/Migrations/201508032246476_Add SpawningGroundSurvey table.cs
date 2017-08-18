namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSpawningGroundSurveytable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SpawningGroundSurvey_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FeatureID = c.Int(nullable: false),
                        FeatureType = c.String(),
                        Species = c.String(),
                        Time = c.DateTime(nullable: false),
                        Temp = c.Int(nullable: false),
                        Easting = c.Int(nullable: false),
                        Northing = c.Int(nullable: false),
                        Channel = c.String(),
                        ReddLocation = c.String(),
                        ReddHabitat = c.String(),
                        WaypointNumber = c.Int(nullable: false),
                        FishCount = c.Int(nullable: false),
                        FishLocation = c.String(),
                        Sex = c.String(),
                        FinClips = c.String(),
                        Marks = c.String(),
                        SpawningStatus = c.Int(nullable: false),
                        ForkLength = c.Int(nullable: false),
                        MeHPLength = c.Int(nullable: false),
                        SnoutID = c.String(),
                        ScaleID = c.String(),
                        Tag = c.String(),
                        TagID = c.String(),
                        Comments = c.String(),
                        Ident = c.Int(nullable: false),
                        EastingUTM = c.Int(nullable: false),
                        NorthingUTM = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        RowId = c.Int(nullable: false),
                        RowStatusId = c.Int(nullable: false),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        QAStatusId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .ForeignKey("dbo.QAStatus", t => t.QAStatusId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId)
                .Index(t => t.QAStatusId);
            
            CreateTable(
                "dbo.SpawningGroundSurvey_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityDate = c.DateTime(nullable: false),
                        Species = c.Int(nullable: false),
                        Technicians = c.String(),
                        Location = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        StartTemperature = c.Int(nullable: false),
                        EndTemperature = c.Int(nullable: false),
                        StartEasting = c.Int(nullable: false),
                        StartNorthing = c.Int(nullable: false),
                        EndEasting = c.Int(nullable: false),
                        EndNorthing = c.Int(nullable: false),
                        Flow = c.Int(nullable: false),
                        WaterVisibility = c.Int(nullable: false),
                        Weather = c.String(),
                        FlaggedRedds = c.Int(nullable: false),
                        NewRedds = c.Int(nullable: false),
                        HeaderComments = c.String(),
                        FieldsheetLink = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SpawningGroundSurvey_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.SpawningGroundSurvey_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.SpawningGroundSurvey_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.SpawningGroundSurvey_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.SpawningGroundSurvey_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.SpawningGroundSurvey_Header", new[] { "ByUserId" });
            DropIndex("dbo.SpawningGroundSurvey_Header", new[] { "ActivityId" });
            DropIndex("dbo.SpawningGroundSurvey_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.SpawningGroundSurvey_Detail", new[] { "ByUserId" });
            DropIndex("dbo.SpawningGroundSurvey_Detail", new[] { "ActivityId" });
            DropTable("dbo.SpawningGroundSurvey_Header");
            DropTable("dbo.SpawningGroundSurvey_Detail");
        }
    }
}
