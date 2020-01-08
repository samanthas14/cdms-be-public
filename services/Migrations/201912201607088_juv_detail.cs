namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class juv_detail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StreamNet_JuvOutmigrationDetail_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JuvenileOutmigrantsID = c.String(),
                        Location = c.String(),
                        LocPTcode = c.String(),
                        LifeStage = c.String(),
                        TotalNatural = c.Int(nullable: false),
                        TotalNaturalLowerLimit = c.Int(nullable: false),
                        TotalNaturalUpperLimit = c.Int(nullable: false),
                        TotalNaturalAlpha = c.Int(nullable: false),
                        SurvivalRate = c.Int(nullable: false),
                        SurvivalRateLowerLimit = c.Int(nullable: false),
                        SurvivalRateUpperLimit = c.Int(nullable: false),
                        SurvivalRateAlpha = c.Int(nullable: false),
                        ContactAgency = c.String(),
                        Comments = c.String(),
                        JMXID = c.String(),
                        NullRecord = c.String(),
                        LastUpdated = c.String(),
                        MetricLocation = c.String(),
                        MeasureLocation = c.String(),
                        SubmitAgency = c.String(),
                        RefID = c.String(),
                        UpdDate = c.String(),
                        DataEntry = c.String(),
                        DataEntryNotes = c.String(),
                        CompilerRecordID = c.String(),
                        Publish = c.String(),
                        ShadowId = c.String(),
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
                "dbo.StreamNet_JuvOutmigrationDetail_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
            DropForeignKey("dbo.StreamNet_JuvOutmigrationDetail_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.StreamNet_JuvOutmigrationDetail_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.StreamNet_JuvOutmigrationDetail_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.StreamNet_JuvOutmigrationDetail_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.StreamNet_JuvOutmigrationDetail_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.StreamNet_JuvOutmigrationDetail_Header", new[] { "ByUserId" });
            DropIndex("dbo.StreamNet_JuvOutmigrationDetail_Header", new[] { "ActivityId" });
            DropIndex("dbo.StreamNet_JuvOutmigrationDetail_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.StreamNet_JuvOutmigrationDetail_Detail", new[] { "ByUserId" });
            DropIndex("dbo.StreamNet_JuvOutmigrationDetail_Detail", new[] { "ActivityId" });
            DropTable("dbo.StreamNet_JuvOutmigrationDetail_Header");
            DropTable("dbo.StreamNet_JuvOutmigrationDetail_Detail");
        }
    }
}
