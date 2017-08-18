namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseDefinitionForElectrofishing : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Electrofishing_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PassNumber = c.String(),
                        Sequence = c.Int(),
                        PitTagCode = c.String(),
                        SpeciesRunRearing = c.String(),
                        ForkLength = c.Double(),
                        Weight = c.Double(),
                        OtherSpecies = c.String(),
                        FishCount = c.Int(),
                        FishSize = c.String(),
                        ConditionalComment = c.String(),
                        TextualComments = c.String(),
                        Note = c.String(),
                        ReleaseLocation = c.String(),
                        Tag = c.String(),
                        Clip = c.String(),
                        OtolithID = c.String(),
                        GeneticID = c.String(),
                        OtherID = c.String(),
                        Disposition = c.String(),
                        FishComments = c.String(),
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
                "dbo.Electrofishing_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FishNumber = c.String(),
                        EventType = c.String(),
                        FileTitle = c.String(),
                        ClipFiles = c.String(),
                        TagDateTime = c.DateTime(),
                        ReleaseDateTime = c.DateTime(),
                        Tagger = c.String(),
                        Crew = c.String(),
                        CaptureMethod = c.String(),
                        MigratoryYear = c.Int(),
                        TaggingTemp = c.Double(),
                        ReleaseTemp = c.Double(),
                        TaggingMethod = c.String(),
                        Organization = c.String(),
                        CoordinatorID = c.String(),
                        Conductivity = c.Double(),
                        EFModel = c.String(),
                        SiteLength = c.Double(),
                        SiteWidth = c.Double(),
                        SiteDepth = c.Double(),
                        SiteArea = c.Double(),
                        HabitatType = c.String(),
                        Visibility = c.String(),
                        ActivityComments = c.String(),
                        ReleaseSite = c.String(),
                        Weather = c.String(),
                        ReleaseRiverKM = c.String(),
                        PassNumber = c.String(),
                        TimeBegin = c.String(),
                        TimeEnd = c.String(),
                        TotalSecondsEF = c.Double(),
                        WaterTempBegin = c.Double(),
                        WaterTempStop = c.Double(),
                        Hertz = c.Double(),
                        Freq = c.Double(),
                        Volts = c.Double(),
                        TotalFishCaptured = c.Int(),
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
            DropForeignKey("dbo.Electrofishing_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Electrofishing_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Electrofishing_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Electrofishing_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Electrofishing_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.Electrofishing_Header", new[] { "ByUserId" });
            DropIndex("dbo.Electrofishing_Header", new[] { "ActivityId" });
            DropIndex("dbo.Electrofishing_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Electrofishing_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Electrofishing_Detail", new[] { "ActivityId" });
            DropTable("dbo.Electrofishing_Header");
            DropTable("dbo.Electrofishing_Detail");
        }
    }
}
