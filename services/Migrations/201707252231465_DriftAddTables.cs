namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DriftAddTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drift_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SpeciesGroup = c.String(),
                        Taxon = c.String(),
                        LifeStage = c.String(),
                        SizeClass = c.String(),
                        TaxonCount = c.Int(),
                        Qualifier = c.String(),
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
                "dbo.Drift_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SampleYear = c.Int(),
                        PrePost = c.String(),
                        VisitId = c.Int(),
                        SampleId = c.String(),
                        SampleClientId = c.String(),
                        TotalJars = c.Int(),
                        AquaticTareMass = c.Single(),
                        AquaticTareDryMass = c.Single(),
                        AquaticDryMassFinal = c.Single(),
                        ATTareMass = c.Single(),
                        ATTareDryMass = c.Single(),
                        ATDryMassFinal = c.Single(),
                        TerrTareMass = c.Single(),
                        TerrTareDryMass = c.Single(),
                        TerrDryMassFinal = c.Single(),
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
            DropForeignKey("dbo.Drift_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Drift_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Drift_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Drift_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Drift_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.Drift_Header", new[] { "ByUserId" });
            DropIndex("dbo.Drift_Header", new[] { "ActivityId" });
            DropIndex("dbo.Drift_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Drift_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Drift_Detail", new[] { "ActivityId" });
            DropTable("dbo.Drift_Header");
            DropTable("dbo.Drift_Detail");
        }
    }
}
