namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JuvOut_migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StreamNet_JuvOutmigrants_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommonName = c.String(),
                        Run = c.String(),
                        RecoveryDomain = c.String(),
                        ESU_DPS = c.String(),
                        MajorPopGroup = c.String(),
                        PopID = c.String(),
                        CBFWApopName = c.String(),
                        CommonPopName = c.String(),
                        PopFit = c.String(),
                        PopFitNotes = c.String(),
                        SmoltEqLocation = c.String(),
                        SmoltEqLocPTcode = c.String(),
                        OutmigrationYear = c.String(),
                        ContactAgency = c.String(),
                        MethodNumber = c.String(),
                        BestValue = c.String(),
                        TotalNatural = c.String(),
                        TotalNaturalLowerLimit = c.String(),
                        TotalNaturalUpperLimit = c.String(),
                        TotalNaturalAlpha = c.String(),
                        Age0Prop = c.String(),
                        Age0PropLowerLimit = c.String(),
                        Age0PropUpperLimit = c.String(),
                        Age1Prop = c.String(),
                        Age1PropLowerLimit = c.String(),
                        Age1PropUpperLimit = c.String(),
                        Age2Prop = c.String(),
                        Age2PropLowerLimit = c.String(),
                        Age2PropUpperLimit = c.String(),
                        Age3Prop = c.String(),
                        Age3PropLowerLimit = c.String(),
                        Age3PropUpperLimit = c.String(),
                        Age4PlusProp = c.String(),
                        Age4PlusPropLowerLimit = c.String(),
                        Age4PlusPropUpperLimit = c.String(),
                        AgePropAlpha = c.String(),
                        ProtMethName = c.String(),
                        ProtMethURL = c.String(),
                        ProtMethDocumentation = c.String(),
                        MethodAdjustments = c.String(),
                        OtherDataSources = c.String(),
                        Comments = c.String(),
                        NullRecord = c.String(),
                        DataStatus = c.String(),
                        LastUpdated = c.String(),
                        IndicatorLocation = c.String(),
                        ContactPersonFirst = c.String(),
                        ContactPersonLast = c.String(),
                        ContactPhone = c.String(),
                        ContactEmail = c.String(),
                        MetaComments = c.String(),
                        SubmitAgency = c.String(),
                        RefID = c.String(),
                        UpdDate = c.String(),
                        DataEntry = c.String(),
                        DataEntryNotes = c.String(),
                        CompilerRecordID = c.String(),
                        Publish = c.String(),
                        ShadowId = c.String(),
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
                "dbo.StreamNet_JuvOutmigrants_Header",
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
            DropForeignKey("dbo.StreamNet_JuvOutmigrants_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.StreamNet_JuvOutmigrants_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.StreamNet_JuvOutmigrants_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.StreamNet_JuvOutmigrants_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.StreamNet_JuvOutmigrants_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.StreamNet_JuvOutmigrants_Header", new[] { "ByUserId" });
            DropIndex("dbo.StreamNet_JuvOutmigrants_Header", new[] { "ActivityId" });
            DropIndex("dbo.StreamNet_JuvOutmigrants_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.StreamNet_JuvOutmigrants_Detail", new[] { "ByUserId" });
            DropIndex("dbo.StreamNet_JuvOutmigrants_Detail", new[] { "ActivityId" });
            DropTable("dbo.StreamNet_JuvOutmigrants_Header");
            DropTable("dbo.StreamNet_JuvOutmigrants_Detail");
        }
    }
}
