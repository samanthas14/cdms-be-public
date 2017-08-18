namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HabAddMetricsTbls : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Metrics_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkElementName = c.String(),
                        Measure = c.String(),
                        PlannedValue = c.Double(),
                        ActualValue = c.Double(),
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
                "dbo.Metrics_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        YearReported = c.Int(),
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
            DropForeignKey("dbo.Metrics_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Metrics_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Metrics_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Metrics_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Metrics_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.Metrics_Header", new[] { "ByUserId" });
            DropIndex("dbo.Metrics_Header", new[] { "ActivityId" });
            DropIndex("dbo.Metrics_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Metrics_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Metrics_Detail", new[] { "ActivityId" });
            DropTable("dbo.Metrics_Header");
            DropTable("dbo.Metrics_Detail");
        }
    }
}
