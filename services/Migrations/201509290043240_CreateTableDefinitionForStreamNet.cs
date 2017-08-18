namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableDefinitionForStreamNet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StreamNet_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Species = c.String(),
                        Location = c.String(),
                        Spawning = c.String(),
                        Origin = c.String(),
                        Age = c.String(),
                        EstimatedNumber = c.String(),
                        BroodStok = c.String(),
                        Reference = c.String(),
                        CIPercent = c.String(),
                        Lower = c.String(),
                        Upper = c.String(),
                        Comment = c.String(),
                        LastModifiedOn = c.DateTime(),
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
                "dbo.StreamNet_Header",
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
            DropForeignKey("dbo.StreamNet_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.StreamNet_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.StreamNet_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.StreamNet_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.StreamNet_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.StreamNet_Header", new[] { "ByUserId" });
            DropIndex("dbo.StreamNet_Header", new[] { "ActivityId" });
            DropIndex("dbo.StreamNet_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.StreamNet_Detail", new[] { "ByUserId" });
            DropIndex("dbo.StreamNet_Detail", new[] { "ActivityId" });
            DropTable("dbo.StreamNet_Header");
            DropTable("dbo.StreamNet_Detail");
        }
    }
}
