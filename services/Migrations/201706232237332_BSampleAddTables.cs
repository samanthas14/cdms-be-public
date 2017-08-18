namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BSampleAddTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BSample_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sex = c.String(),
                        Mark = c.String(),
                        ForkLength = c.Int(),
                        TotalLength = c.Int(),
                        Weight = c.Int(),
                        ScaleId = c.String(),
                        GeneticSampleId = c.String(),
                        SnoutId = c.String(),
                        LifeStage = c.String(),
                        Origin = c.String(),
                        Species = c.String(),
                        PITTagId = c.String(),
                        Tag = c.String(),
                        RadioTagId = c.String(),
                        FishComments = c.String(),
                        OtherTagId = c.String(),
                        KidneyId = c.String(),
                        PercentRetained = c.String(),
                        FinClip = c.String(),
                        TotalCount = c.Int(),
                        RecordNumber = c.String(),
                        MEHPLength = c.Int(),
                        SubSample = c.String(),
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
                "dbo.BSample_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SampleYear = c.Int(),
                        Technicians = c.String(),
                        HeaderComments = c.String(),
                        CollectionType = c.String(),
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
            DropForeignKey("dbo.BSample_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.BSample_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.BSample_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.BSample_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.BSample_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.BSample_Header", new[] { "ByUserId" });
            DropIndex("dbo.BSample_Header", new[] { "ActivityId" });
            DropIndex("dbo.BSample_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.BSample_Detail", new[] { "ByUserId" });
            DropIndex("dbo.BSample_Detail", new[] { "ActivityId" });
            DropTable("dbo.BSample_Header");
            DropTable("dbo.BSample_Detail");
        }
    }
}
