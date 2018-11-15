namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class age_dataset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NPT_Age_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SampleNumber = c.String(),
                        HistoricSampleNumber = c.String(),
                        CollectionDate = c.DateTime(nullable: false),
                        Species = c.String(),
                        Origin = c.String(),
                        TargetFish = c.String(),
                        Sex = c.String(),
                        ForkLength = c.Int(),
                        Lifestage = c.String(),
                        LifeHistory = c.String(),
                        PITCode = c.String(),
                        CWTCode = c.String(),
                        OtherId = c.String(),
                        OtherId_Type = c.String(),
                        CollectionRepository = c.String(),
                        AgeDetermination = c.String(),
                        AnalysisId = c.String(),
                        AgeOrigin = c.String(),
                        StreamAge = c.Int(nullable: false),
                        OceanAge = c.Int(nullable: false),
                        RepeatSpawner = c.String(),
                        SecondOcean = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        AgeingComment = c.String(),
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
                "dbo.NPT_Age_Header",
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
            
            AddColumn("dbo.SGS_Carcass_Detail", "UDF1", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "UDF2", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "UDF3", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "UDF4", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "UDF5", c => c.String());
            AddColumn("dbo.SGS_Redd_Detail", "UDF1", c => c.String());
            AddColumn("dbo.SGS_Redd_Detail", "UDF2", c => c.String());
            AddColumn("dbo.SGS_Redd_Detail", "UDF3", c => c.String());
            AddColumn("dbo.SGS_Redd_Detail", "UDF4", c => c.String());
            AddColumn("dbo.SGS_Redd_Detail", "UDF5", c => c.String());
            AlterColumn("dbo.SGS_Carcass_Header", "StartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SGS_Carcass_Header", "EndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SGS_Redd_Header", "StartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SGS_Redd_Header", "EndTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NPT_Age_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.NPT_Age_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.NPT_Age_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.NPT_Age_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.NPT_Age_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.NPT_Age_Header", new[] { "ByUserId" });
            DropIndex("dbo.NPT_Age_Header", new[] { "ActivityId" });
            DropIndex("dbo.NPT_Age_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.NPT_Age_Detail", new[] { "ByUserId" });
            DropIndex("dbo.NPT_Age_Detail", new[] { "ActivityId" });
            AlterColumn("dbo.SGS_Redd_Header", "EndTime", c => c.String());
            AlterColumn("dbo.SGS_Redd_Header", "StartTime", c => c.String());
            AlterColumn("dbo.SGS_Carcass_Header", "EndTime", c => c.String());
            AlterColumn("dbo.SGS_Carcass_Header", "StartTime", c => c.String());
            DropColumn("dbo.SGS_Redd_Detail", "UDF5");
            DropColumn("dbo.SGS_Redd_Detail", "UDF4");
            DropColumn("dbo.SGS_Redd_Detail", "UDF3");
            DropColumn("dbo.SGS_Redd_Detail", "UDF2");
            DropColumn("dbo.SGS_Redd_Detail", "UDF1");
            DropColumn("dbo.SGS_Carcass_Detail", "UDF5");
            DropColumn("dbo.SGS_Carcass_Detail", "UDF4");
            DropColumn("dbo.SGS_Carcass_Detail", "UDF3");
            DropColumn("dbo.SGS_Carcass_Detail", "UDF2");
            DropColumn("dbo.SGS_Carcass_Detail", "UDF1");
            DropTable("dbo.NPT_Age_Header");
            DropTable("dbo.NPT_Age_Detail");
        }
    }
}
