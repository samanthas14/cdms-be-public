namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DM_issues_dataset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DM_Issues_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentDate = c.DateTime(nullable: false),
                        Commenter = c.String(),
                        Comment = c.String(),
                        PossibleOption = c.String(),
                        Feasibility = c.String(),
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
                "dbo.DM_Issues_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubmittedBy = c.String(),
                        Project = c.String(),
                        Application = c.String(),
                        IssueType = c.String(),
                        Keywords = c.String(),
                        IssueDetails = c.String(),
                        DMTTPriority = c.String(),
                        ExpectedCompletion = c.DateTime(nullable: false),
                        CompletedBy = c.String(),
                        DateCompleted = c.DateTime(nullable: false),
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
            DropForeignKey("dbo.DM_Issues_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.DM_Issues_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.DM_Issues_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.DM_Issues_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.DM_Issues_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.DM_Issues_Header", new[] { "ByUserId" });
            DropIndex("dbo.DM_Issues_Header", new[] { "ActivityId" });
            DropIndex("dbo.DM_Issues_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.DM_Issues_Detail", new[] { "ByUserId" });
            DropIndex("dbo.DM_Issues_Detail", new[] { "ActivityId" });
            DropTable("dbo.DM_Issues_Header");
            DropTable("dbo.DM_Issues_Detail");
        }
    }
}
