namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGeneticIDAndKidneyIDToSGSDataset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpawningGroundSurvey_Detail", "GeneticID", c => c.String());
            AddColumn("dbo.SpawningGroundSurvey_Detail", "KidneyID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpawningGroundSurvey_Detail", "KidneyID");
            DropColumn("dbo.SpawningGroundSurvey_Detail", "GeneticID");
        }
    }
}
