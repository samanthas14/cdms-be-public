namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JvRearAddTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JvRearing_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Action = c.String(),
                        Species = c.String(),
                        PointData = c.String(),
                        Result = c.Single(),
                        ResultUnit = c.String(),
                        ActionComments = c.String(),
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
                "dbo.JvRearing_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AcclimationYear = c.Int(),
                        HeaderComments = c.String(),
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
            DropForeignKey("dbo.JvRearing_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.JvRearing_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.JvRearing_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.JvRearing_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.JvRearing_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.JvRearing_Header", new[] { "ByUserId" });
            DropIndex("dbo.JvRearing_Header", new[] { "ActivityId" });
            DropIndex("dbo.JvRearing_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.JvRearing_Detail", new[] { "ByUserId" });
            DropIndex("dbo.JvRearing_Detail", new[] { "ActivityId" });
            DropTable("dbo.JvRearing_Header");
            DropTable("dbo.JvRearing_Detail");
        }
    }
}
