namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tier1 : DbMigration
    {
        public override void Up()
        {

            
            CreateTable(
                "dbo.Tier1_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tier1 = c.Int(),
                        Area = c.Decimal(precision: 18, scale: 2),
                        Ct = c.Int(),
                        UnitSpacing = c.Decimal(precision: 18, scale: 2),
                        Freq = c.Decimal(precision: 18, scale: 2),
                        Vol = c.Decimal(precision: 18, scale: 2),
                        Pct = c.Decimal(precision: 18, scale: 2),
                        DpthThlwgMaxAvg = c.Decimal(precision: 18, scale: 2),
                        DpthThlwgExit = c.Decimal(precision: 18, scale: 2),
                        DpthResid = c.Decimal(precision: 18, scale: 2),
                        SubEstBldr = c.Decimal(precision: 18, scale: 2),
                        SubEstCbl = c.Decimal(precision: 18, scale: 2),
                        SubEstGrvl = c.Decimal(precision: 18, scale: 2),
                        SubEstSandFines = c.Decimal(precision: 18, scale: 2),
                        FishCovLW = c.Decimal(precision: 18, scale: 2),
                        FishCovTVeg = c.Decimal(precision: 18, scale: 2),
                        FishCovUcut = c.Decimal(precision: 18, scale: 2),
                        FishCovArt = c.Decimal(precision: 18, scale: 2),
                        FishCovAqVeg = c.Decimal(precision: 18, scale: 2),
                        FishCovNone = c.Decimal(precision: 18, scale: 2),
                        FishCovTotal = c.Decimal(precision: 18, scale: 2),
                        LWVolWetTier1 = c.Decimal(precision: 18, scale: 2),
                        LWVolBfTier1 = c.Decimal(precision: 18, scale: 2),
                        CountOfLWD = c.Int(),
                        CountOfChinook = c.Int(),
                        CountOfCoho = c.Int(),
                        CountOfSockeye = c.Int(),
                        CountOfChum = c.Int(),
                        CountOfOmykiss = c.Int(),
                        CountOfPink = c.Int(),
                        CountOfCutthroat = c.Int(),
                        CountOfBulltrout = c.Int(),
                        CountOfBrooktrout = c.Int(),
                        CountOfLamprey = c.Int(),
                        CountOfOtherSpecies = c.Int(),
                        DensityOfChinook = c.Decimal(precision: 18, scale: 2),
                        DensityOfCoho = c.Decimal(precision: 18, scale: 2),
                        DensityOfSockeye = c.Decimal(precision: 18, scale: 2),
                        DensityOfChum = c.Decimal(precision: 18, scale: 2),
                        DensityOfOmykiss = c.Decimal(precision: 18, scale: 2),
                        DensityOfPink = c.Decimal(precision: 18, scale: 2),
                        DensityOfCutthroat = c.Decimal(precision: 18, scale: 2),
                        DensityOfBulltrout = c.Decimal(precision: 18, scale: 2),
                        DensityOfBrooktrout = c.Decimal(precision: 18, scale: 2),
                        DensityOfLamprey = c.Decimal(precision: 18, scale: 2),
                        DensityOfOtherSpecies = c.Decimal(precision: 18, scale: 2),
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
                "dbo.Tier1_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramSiteID = c.Int(),
                        SiteName = c.String(),
                        WatershedID = c.Int(),
                        WatershedName = c.String(),
                        SampleDate = c.String(),
                        HitchName = c.String(),
                        CrewName = c.String(),
                        VisitYear = c.Int(),
                        IterationID = c.Int(),
                        CategoryName = c.String(),
                        PanelName = c.String(),
                        VisitID = c.Int(),
                        VisitDate = c.Int(),
                        ProtocolID = c.Int(),
                        ProgramID = c.Int(),
                        AEM = c.Int(),
                        Effectiveness = c.Int(),
                        HasFishData = c.Int(),
                        PrimaryVisit = c.Int(),
                        QCVisit = c.Int(),
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
            DropForeignKey("dbo.VisitMetrics_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.VisitMetrics_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.VisitMetrics_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.VisitMetrics_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.VisitMetrics_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Tier1_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Tier1_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Tier1_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Tier1_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Tier1_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.ChannelUnitMetrics_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.ChannelUnitMetrics_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.ChannelUnitMetrics_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.ChannelUnitMetrics_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.ChannelUnitMetrics_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.VisitMetrics_Header", new[] { "ByUserId" });
            DropIndex("dbo.VisitMetrics_Header", new[] { "ActivityId" });
            DropIndex("dbo.VisitMetrics_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.VisitMetrics_Detail", new[] { "ByUserId" });
            DropIndex("dbo.VisitMetrics_Detail", new[] { "ActivityId" });
            DropIndex("dbo.Tier1_Header", new[] { "ByUserId" });
            DropIndex("dbo.Tier1_Header", new[] { "ActivityId" });
            DropIndex("dbo.Tier1_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Tier1_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Tier1_Detail", new[] { "ActivityId" });
            DropIndex("dbo.ChannelUnitMetrics_Header", new[] { "ByUserId" });
            DropIndex("dbo.ChannelUnitMetrics_Header", new[] { "ActivityId" });
            DropIndex("dbo.ChannelUnitMetrics_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.ChannelUnitMetrics_Detail", new[] { "ByUserId" });
            DropIndex("dbo.ChannelUnitMetrics_Detail", new[] { "ActivityId" });
            AlterColumn("dbo.Leases", "LeaseStart", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LeaseRevisions", "LeaseStart", c => c.DateTime(nullable: false));
            DropColumn("dbo.LeaseInspections", "ViolationType");
            DropColumn("dbo.LeaseInspections", "ViolationOwnerType");
            DropColumn("dbo.LeaseInspections", "ViolationLandAreaCode");
            DropTable("dbo.VisitMetrics_Header");
            DropTable("dbo.VisitMetrics_Detail");
            DropTable("dbo.Tier1_Header");
            DropTable("dbo.Tier1_Detail");
            DropTable("dbo.ChannelUnitMetrics_Header");
            DropTable("dbo.ChannelUnitMetrics_Detail");
        }
    }
}
