namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lamprey_dataset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lamprey_Data_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Length = c.Single(nullable: false),
                        Weight = c.Single(nullable: false),
                        LifeStage = c.String(),
                        Sample = c.String(),
                        SampleNumber = c.String(),
                        SampleComment = c.String(),
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
                "dbo.Lamprey_Data_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SpeciesRun = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        WaterTemperature = c.Single(nullable: false),
                        PlotLength = c.Single(nullable: false),
                        PlotWidth = c.Single(nullable: false),
                        PredominantHabitatType = c.String(),
                        FirstPassTime = c.Int(nullable: false),
                        FirstPassCount = c.Int(nullable: false),
                        SecondPassTime = c.Int(nullable: false),
                        SecondPassCount = c.Int(nullable: false),
                        ThirdPassTime = c.Int(nullable: false),
                        ThirdPassCount = c.Int(nullable: false),
                        FishReleasedNoData = c.String(),
                        Collector = c.String(),
                        SurveyComment = c.String(),
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
            DropForeignKey("dbo.Lamprey_Data_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Lamprey_Data_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Lamprey_Data_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Lamprey_Data_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Lamprey_Data_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.Lamprey_Data_Header", new[] { "ByUserId" });
            DropIndex("dbo.Lamprey_Data_Header", new[] { "ActivityId" });
            DropIndex("dbo.Lamprey_Data_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Lamprey_Data_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Lamprey_Data_Detail", new[] { "ActivityId" });
            DropTable("dbo.Lamprey_Data_Header");
            DropTable("dbo.Lamprey_Data_Detail");
        }
    }
}
