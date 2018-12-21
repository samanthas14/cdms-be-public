namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class age_dataset1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NPT_Age_Detail", "SpeciesRun", c => c.String());
            AddColumn("dbo.NPT_Age_Detail", "OtherIdType", c => c.String());
            AddColumn("dbo.NPT_Age_Detail", "SecondSpawnerAge", c => c.Int(nullable: false));
            AddColumn("dbo.NPT_Age_Detail", "TotalAge", c => c.Int(nullable: false));
            DropColumn("dbo.NPT_Age_Detail", "HistoricSampleNumber");
            DropColumn("dbo.NPT_Age_Detail", "Species");
            DropColumn("dbo.NPT_Age_Detail", "OtherId_Type");
            DropColumn("dbo.NPT_Age_Detail", "SecondOcean");
            DropColumn("dbo.NPT_Age_Detail", "Age");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NPT_Age_Detail", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.NPT_Age_Detail", "SecondOcean", c => c.Int(nullable: false));
            AddColumn("dbo.NPT_Age_Detail", "OtherId_Type", c => c.String());
            AddColumn("dbo.NPT_Age_Detail", "Species", c => c.String());
            AddColumn("dbo.NPT_Age_Detail", "HistoricSampleNumber", c => c.String());
            DropColumn("dbo.NPT_Age_Detail", "TotalAge");
            DropColumn("dbo.NPT_Age_Detail", "SecondSpawnerAge");
            DropColumn("dbo.NPT_Age_Detail", "OtherIdType");
            DropColumn("dbo.NPT_Age_Detail", "SpeciesRun");
        }
    }
}
