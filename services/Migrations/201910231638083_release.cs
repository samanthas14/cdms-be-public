namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class release : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Release_Data_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TagCode_ReleaseID = c.String(),
                        TagType = c.String(),
                        FirstSequentialNumber = c.Int(nullable: false),
                        LastSequentialNumber = c.Int(nullable: false),
                        RelatedGroupType = c.String(),
                        SpeciesRun = c.String(),
                        BroodYear = c.Int(),
                        FirstReleaseDate = c.String(),
                        LastReleaseDate = c.String(),
                        HatcheryLocation = c.String(),
                        StockLocation = c.String(),
                        ReleaseStage = c.String(),
                        RearingType = c.String(),
                        StudyType = c.String(),
                        ReleaseStrategy = c.String(),
                        StudyIntegrity = c.String(),
                        CWT_Mark1 = c.String(),
                        CWT_Count1 = c.Int(nullable: false),
                        CWT_Mark2 = c.String(),
                        CWT_Count2 = c.Int(nullable: false),
                        NonCWT_Mark1 = c.String(),
                        NonCWT_Count1 = c.Int(nullable: false),
                        NonCWT_Mark2 = c.String(),
                        NonCWT_Count2 = c.Int(nullable: false),
                        CountingMethod = c.String(),
                        Comments = c.String(),
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
                "dbo.Release_Data_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RecordCode = c.String(),
                        FormatVersion = c.String(),
                        SubmissionDate = c.String(),
                        ReportingAgency = c.String(),
                        ReleaseAgency = c.String(),
                        Coordinator = c.String(),
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
            DropForeignKey("dbo.Release_Data_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Release_Data_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Release_Data_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Release_Data_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Release_Data_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.Release_Data_Header", new[] { "ByUserId" });
            DropIndex("dbo.Release_Data_Header", new[] { "ActivityId" });
            DropIndex("dbo.Release_Data_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Release_Data_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Release_Data_Detail", new[] { "ActivityId" });
            DropTable("dbo.Release_Data_Header");
            DropTable("dbo.Release_Data_Detail");
        }
    }
}
