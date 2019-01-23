namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_rst_dataset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NPT_RST_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lifestage = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Period = c.String(),
                        Unmarked = c.Int(),
                        Marked = c.Int(),
                        Recapture = c.Int(),
                        Abundance = c.Int(),
                        StdError = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                "dbo.NPT_RST_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SpeciesRun = c.String(),
                        Origin = c.String(),
                        BroodYear = c.Int(),
                        MigratoryYear = c.Int(nullable: false),
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
            DropForeignKey("dbo.NPT_RST_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.NPT_RST_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.NPT_RST_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.NPT_RST_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.NPT_RST_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.NPT_RST_Header", new[] { "ByUserId" });
            DropIndex("dbo.NPT_RST_Header", new[] { "ActivityId" });
            DropIndex("dbo.NPT_RST_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.NPT_RST_Detail", new[] { "ByUserId" });
            DropIndex("dbo.NPT_RST_Detail", new[] { "ActivityId" });
            DropTable("dbo.NPT_RST_Header");
            DropTable("dbo.NPT_RST_Detail");
        }
    }
}
