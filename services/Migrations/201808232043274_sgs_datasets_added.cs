namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sgs_datasets_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SGS_Carcass_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SampleNumber = c.String(),
                        HistoricSampleNumber = c.String(),
                        CarcassSpecies = c.String(),
                        Sex = c.String(),
                        ForkLength = c.Int(),
                        SpawnedOut = c.String(),
                        PercentSpawned = c.Int(nullable: false),
                        OpercleLeft = c.String(),
                        OpercleRight = c.String(),
                        PITScanned = c.String(),
                        PITCode = c.String(),
                        AdiposeFinClipped = c.String(),
                        CWTScanned = c.String(),
                        SnoutCollected = c.String(),
                        DNACollecetd = c.String(),
                        Fins = c.String(),
                        Scales = c.String(),
                        Otolith = c.String(),
                        TargetFish = c.String(),
                        Recapture = c.String(),
                        VerifiedOrigin = c.String(),
                        Count = c.Int(),
                        CarcassComments = c.String(),
                        WPTName = c.String(),
                        Datum = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        TransmitterType = c.String(),
                        Vendor = c.String(),
                        SerialNumber = c.String(),
                        Frequency = c.String(),
                        Channel = c.String(),
                        Code = c.String(),
                        TagsFloy = c.String(),
                        TagsVIE = c.String(),
                        TagsJaw = c.String(),
                        TagsStaple = c.String(),
                        TagsSpaghetti = c.String(),
                        TagsStreamer = c.String(),
                        TagsPetersonDisc = c.String(),
                        MarksAnalFin = c.String(),
                        MarksCaudalFin = c.String(),
                        MarksPectoralFin = c.String(),
                        MarksVentralFin = c.String(),
                        MarksMaxillary = c.String(),
                        MarksFreezeBrand = c.String(),
                        MarksGRIT = c.String(),
                        MarksOTC = c.String(),
                        MarksDorsalScar = c.String(),
                        Notes = c.String(),
                        UDF1 = c.String(),
                        UDF2 = c.String(),
                        UDF3 = c.String(),
                        UDF4 = c.String(),
                        UDF5 = c.String(),
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
                "dbo.SGS_Carcass_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TargetSpecies = c.String(),
                        Pass = c.Int(nullable: false),
                        StartSurvey = c.String(),
                        EndSurvey = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Observers = c.String(),
                        SurveyType = c.String(),
                        SurveyMethod = c.String(),
                        GPSUnit = c.String(),
                        Weather = c.String(),
                        Visibility = c.String(),
                        SurveyComments = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.SGS_Redd_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReddSpecies = c.String(),
                        ReddType = c.String(),
                        Count = c.Int(),
                        ReddComments = c.String(),
                        WPTName = c.String(),
                        Datum = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        UDF1 = c.String(),
                        UDF2 = c.String(),
                        UDF3 = c.String(),
                        UDF4 = c.String(),
                        UDF5 = c.String(),
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
                "dbo.SGS_Redd_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TargetSpecies = c.String(),
                        Pass = c.Int(nullable: false),
                        StartSurvey = c.String(),
                        EndSurvey = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Observers = c.String(),
                        SurveyType = c.String(),
                        SurveyMethod = c.String(),
                        GPSUnit = c.String(),
                        Weather = c.String(),
                        Visibility = c.String(),
                        SurveyComments = c.String(),
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
            DropForeignKey("dbo.SGS_Redd_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.SGS_Redd_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.SGS_Redd_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.SGS_Redd_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.SGS_Redd_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.SGS_Carcass_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.SGS_Carcass_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.SGS_Carcass_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.SGS_Carcass_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.SGS_Carcass_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.SGS_Redd_Header", new[] { "ByUserId" });
            DropIndex("dbo.SGS_Redd_Header", new[] { "ActivityId" });
            DropIndex("dbo.SGS_Redd_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.SGS_Redd_Detail", new[] { "ByUserId" });
            DropIndex("dbo.SGS_Redd_Detail", new[] { "ActivityId" });
            DropIndex("dbo.SGS_Carcass_Header", new[] { "ByUserId" });
            DropIndex("dbo.SGS_Carcass_Header", new[] { "ActivityId" });
            DropIndex("dbo.SGS_Carcass_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.SGS_Carcass_Detail", new[] { "ByUserId" });
            DropIndex("dbo.SGS_Carcass_Detail", new[] { "ActivityId" });
            DropTable("dbo.SGS_Redd_Header");
            DropTable("dbo.SGS_Redd_Detail");
            DropTable("dbo.SGS_Carcass_Header");
            DropTable("dbo.SGS_Carcass_Detail");
        }
    }
}
