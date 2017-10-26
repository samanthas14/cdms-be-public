namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BenChgFieldNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Benthic_Detail", "MsPollutionTolerant", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Benthic_Detail", "MsPredator", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Benthic_Detail", "MsDominantTaxa3", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Benthic_Detail", "MsPollutionTolerantPercent");
            DropColumn("dbo.Benthic_Detail", "MsPredatorPercent");
            DropColumn("dbo.Benthic_Detail", "MsDominantTaxa3Percent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Benthic_Detail", "MsDominantTaxa3Percent", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Benthic_Detail", "MsPredatorPercent", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Benthic_Detail", "MsPollutionTolerantPercent", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Benthic_Detail", "MsDominantTaxa3");
            DropColumn("dbo.Benthic_Detail", "MsPredator");
            DropColumn("dbo.Benthic_Detail", "MsPollutionTolerant");
        }
    }
}
