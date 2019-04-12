namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChannelUnitMetricsTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChannelUnitMetrics_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChUnitID = c.Int(),
                        ChUnitNum = c.Int(),
                        Tier1 = c.Int(),
                        Tier2 = c.Int(),
                        AreaTotal = c.Decimal(precision: 18, scale: 2),
                        PolyArea = c.Decimal(precision: 18, scale: 2),
                        TotalVol = c.Decimal(precision: 18, scale: 2),
                        DpthMax = c.Decimal(precision: 18, scale: 2),
                        DpthThlwgExit = c.Decimal(precision: 18, scale: 2),
                        DpthResid = c.Decimal(precision: 18, scale: 2),
                        CountOfLWD = c.Int(),
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
                "dbo.ChannelUnitMetrics_Header",
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
            DropForeignKey("dbo.ChannelUnitMetrics_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.ChannelUnitMetrics_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.ChannelUnitMetrics_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.ChannelUnitMetrics_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.ChannelUnitMetrics_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.ChannelUnitMetrics_Header", new[] { "ByUserId" });
            DropIndex("dbo.ChannelUnitMetrics_Header", new[] { "ActivityId" });
            DropIndex("dbo.ChannelUnitMetrics_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.ChannelUnitMetrics_Detail", new[] { "ByUserId" });
            DropIndex("dbo.ChannelUnitMetrics_Detail", new[] { "ActivityId" });
            DropTable("dbo.ChannelUnitMetrics_Header");
            DropTable("dbo.ChannelUnitMetrics_Detail");
        }
    }
}
