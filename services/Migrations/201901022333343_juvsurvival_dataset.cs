namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class juvsurvival_dataset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NPT_JuvSurvival_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TagSite = c.String(),
                        TagDate = c.DateTime(),
                        RearingVessel = c.String(),
                        ReleaseType = c.String(),
                        ReleaseGroup = c.String(),
                        ReleaseStartDate = c.DateTime(),
                        ReleaseEndDate = c.DateTime(),
                        AdClipped = c.String(),
                        Lifestage = c.String(),
                        SurvivalTo = c.String(),
                        Survival = c.Int(),
                        StdError = c.Decimal(precision: 18, scale: 2),
                        Lower95 = c.Int(),
                        Upper95 = c.Int(),
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
                "dbo.NPT_JuvSurvival_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SpeciesRun = c.String(),
                        Origin = c.String(),
                        Hatchery = c.String(),
                        BroodYear = c.Int(),
                        MigratoryYear = c.Int(nullable: false),
                        SampleType = c.String(),
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
            DropForeignKey("dbo.NPT_JuvSurvival_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.NPT_JuvSurvival_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.NPT_JuvSurvival_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.NPT_JuvSurvival_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.NPT_JuvSurvival_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.NPT_JuvSurvival_Header", new[] { "ByUserId" });
            DropIndex("dbo.NPT_JuvSurvival_Header", new[] { "ActivityId" });
            DropIndex("dbo.NPT_JuvSurvival_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.NPT_JuvSurvival_Detail", new[] { "ByUserId" });
            DropIndex("dbo.NPT_JuvSurvival_Detail", new[] { "ActivityId" });
            DropTable("dbo.NPT_JuvSurvival_Header");
            DropTable("dbo.NPT_JuvSurvival_Detail");
        }
    }
}
