namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCrppContractsTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CrppContracts_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AcresSurveyed = c.Double(),
                        Testing = c.String(),
                        NewSites = c.Int(),
                        MonitoredSites = c.Int(),
                        SitesEvaluated = c.Int(),
                        UpdatedSites = c.Int(),
                        NewIsolates = c.Int(),
                        Evaluation = c.String(),
                        HprcsitsRecorded = c.String(),
                        Monitoring = c.String(),
                        Notes = c.String(),
                        ShpoReportNumber = c.String(),
                        ShpoCaseNumber = c.String(),
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
                "dbo.CrppContracts_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CrppPersonnel = c.String(),
                        ActivityTypeId = c.Int(),
                        Agency = c.String(),
                        ProjectProponent = c.String(),
                        PermitNumber = c.String(),
                        DateReceived = c.DateTime(),
                        DateOfAction = c.DateTime(),
                        ActionTaken = c.String(),
                        ActivityNotes = c.String(),
                        AttachedDocument = c.String(),
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
            DropForeignKey("dbo.CrppContracts_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.CrppContracts_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.CrppContracts_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.CrppContracts_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.CrppContracts_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.CrppContracts_Header", new[] { "ByUserId" });
            DropIndex("dbo.CrppContracts_Header", new[] { "ActivityId" });
            DropIndex("dbo.CrppContracts_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.CrppContracts_Detail", new[] { "ByUserId" });
            DropIndex("dbo.CrppContracts_Detail", new[] { "ActivityId" });
            DropTable("dbo.CrppContracts_Header");
            DropTable("dbo.CrppContracts_Detail");
        }
    }
}
