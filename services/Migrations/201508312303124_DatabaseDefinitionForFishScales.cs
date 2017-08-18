namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseDefinitionForFishScales : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FishScales_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldScaleID = c.String(),
                        GumCardScaleID = c.String(),
                        ScaleCollectionDate = c.DateTime(),
                        Species = c.String(),
                        LifeStage = c.String(),
                        Circuli = c.Int(),
                        JuvenileAge = c.String(),
                        FreshwaterAge = c.Int(),
                        SaltWaterAge = c.Int(),
                        TotalAdultAge = c.Int(),
                        SpawnCheck = c.String(),
                        Regeneration = c.String(),
                        Stock = c.String(),
                        FishComments = c.String(),
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
                "dbo.FishScales_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RunYear = c.Int(),
                        Technician = c.String(),
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
            DropForeignKey("dbo.FishScales_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.FishScales_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.FishScales_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.FishScales_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.FishScales_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.FishScales_Header", new[] { "ByUserId" });
            DropIndex("dbo.FishScales_Header", new[] { "ActivityId" });
            DropIndex("dbo.FishScales_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.FishScales_Detail", new[] { "ByUserId" });
            DropIndex("dbo.FishScales_Detail", new[] { "ActivityId" });
            DropTable("dbo.FishScales_Header");
            DropTable("dbo.FishScales_Detail");
        }
    }
}
