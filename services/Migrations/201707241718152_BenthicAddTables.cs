namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BenthicAddTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Benthic_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MetricTaxaRichness = c.Decimal(precision: 18, scale: 2),
                        MetricHilsenhoffBioticIndex = c.Decimal(precision: 18, scale: 2),
                        MetricChironomidae = c.Decimal(precision: 18, scale: 2),
                        MetricColeoptera = c.Decimal(precision: 18, scale: 2),
                        MetricDiptera = c.Decimal(precision: 18, scale: 2),
                        MetricEphemeroptera = c.Decimal(precision: 18, scale: 2),
                        MetricLepidoptera = c.Decimal(precision: 18, scale: 2),
                        MetricMegaloptera = c.Decimal(precision: 18, scale: 2),
                        MetricOdonata = c.Decimal(precision: 18, scale: 2),
                        MetricOligochaeta = c.Decimal(precision: 18, scale: 2),
                        MetricOtherNonInsect = c.Decimal(precision: 18, scale: 2),
                        MetricPlecoptera = c.Decimal(precision: 18, scale: 2),
                        MetricTrichoptera = c.Decimal(precision: 18, scale: 2),
                        MvTaxaRichness = c.Int(),
                        MvERichness = c.Int(),
                        MvPRichness = c.Int(),
                        MvTRichness = c.Int(),
                        MvPollutionSensitiveRichness = c.Int(),
                        MvClingerRichness = c.Int(),
                        MvSemivoltineRichness = c.Int(),
                        MvPollutionTolerantPercent = c.Decimal(precision: 18, scale: 2),
                        MvPredatorPercent = c.Decimal(precision: 18, scale: 2),
                        MvDominantTaxa3Percent = c.Decimal(precision: 18, scale: 2),
                        MsTaxaRichness = c.Int(),
                        MsERichness = c.Int(),
                        MsPRichness = c.Int(),
                        MsTRichness = c.Int(),
                        MsPollutionSensitiveRichness = c.Int(),
                        MsClingerRichness = c.Int(),
                        MsSemivoltineRichness = c.Int(),
                        MsPollutionTolerantPercent = c.Decimal(precision: 18, scale: 2),
                        MsPredatorPercent = c.Decimal(precision: 18, scale: 2),
                        MsDominantTaxa3Percent = c.Decimal(precision: 18, scale: 2),
                        BIbiScore = c.Int(),
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
                "dbo.Benthic_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SampleYear = c.Int(),
                        PrePost = c.String(),
                        VisitId = c.Int(),
                        SampleId = c.String(),
                        SampleClientId = c.String(),
                        TareMass = c.Decimal(precision: 18, scale: 4),
                        DryMass = c.Decimal(precision: 18, scale: 4),
                        DryMassFinal = c.Decimal(precision: 18, scale: 4),
                        FieldComments = c.String(),
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
            DropForeignKey("dbo.Benthic_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Benthic_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Benthic_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Benthic_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Benthic_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.Benthic_Header", new[] { "ByUserId" });
            DropIndex("dbo.Benthic_Header", new[] { "ActivityId" });
            DropIndex("dbo.Benthic_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Benthic_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Benthic_Detail", new[] { "ActivityId" });
            DropTable("dbo.Benthic_Header");
            DropTable("dbo.Benthic_Detail");
        }
    }
}
