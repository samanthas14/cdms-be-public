namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseDefinitionForScrewTrap : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ScrewTrap_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sequence = c.Int(),
                        PitTagCode = c.String(),
                        SpeciesRunRearing = c.String(),
                        ForkLength = c.Double(),
                        Weight = c.Double(),
                        OtherSpecies = c.String(),
                        FishCount = c.Int(),
                        ConditionalComment = c.String(),
                        TextualComments = c.String(),
                        Note = c.String(),
                        ReleaseLocation = c.String(),
                        Tag = c.String(),
                        Clip = c.String(),
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
                "dbo.ScrewTrap_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileTitle = c.String(),
                        ClipFiles = c.String(),
                        TagDateTime = c.DateTime(),
                        ReleaseDateTime = c.DateTime(),
                        Tagger = c.String(),
                        CaptureMethod = c.String(),
                        MigratoryYear = c.Int(),
                        LivewellTemp = c.Double(),
                        TaggingTemp = c.Double(),
                        PostTaggingTemp = c.Double(),
                        ReleaseTemp = c.Double(),
                        TaggingMethod = c.String(),
                        Organization = c.String(),
                        CoordinatorID = c.String(),
                        ArrivalTime = c.DateTime(),
                        DepartTime = c.DateTime(),
                        ArrivalRPMs = c.Double(),
                        DepartureRPMs = c.Double(),
                        Hubometer = c.Int(),
                        HubometerTime = c.DateTime(),
                        TrapStopped = c.DateTime(),
                        TrapStarted = c.DateTime(),
                        FishCollected = c.DateTime(),
                        FishReleased = c.DateTime(),
                        Flow = c.String(),
                        Turbitity = c.String(),
                        TrapDebris = c.String(),
                        RiverDebris = c.String(),
                        Task = c.String(),
                        ActivityComments = c.String(),
                        ReleaseSite = c.String(),
                        ReleaseRiverKM = c.String(),
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
            DropForeignKey("dbo.ScrewTrap_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.ScrewTrap_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.ScrewTrap_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.ScrewTrap_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.ScrewTrap_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.ScrewTrap_Header", new[] { "ByUserId" });
            DropIndex("dbo.ScrewTrap_Header", new[] { "ActivityId" });
            DropIndex("dbo.ScrewTrap_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.ScrewTrap_Detail", new[] { "ByUserId" });
            DropIndex("dbo.ScrewTrap_Detail", new[] { "ActivityId" });
            DropTable("dbo.ScrewTrap_Header");
            DropTable("dbo.ScrewTrap_Detail");
        }
    }
}
