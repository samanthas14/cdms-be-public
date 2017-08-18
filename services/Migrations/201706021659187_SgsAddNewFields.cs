namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SgsAddNewFields : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.SpawningGroundSurvey_Detail", name: "SpawningStatus", newName: "PercentRetained");
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "PercentRetained", c => c.Single());

            AddColumn("dbo.SpawningGroundSurvey_Detail", "NumberEggsRetained", c => c.Int());
            AddColumn("dbo.SpawningGroundSurvey_Detail", "MortalityType", c => c.String());
            AddColumn("dbo.SpawningGroundSurvey_Detail", "ReddMeasurements", c => c.String());
            AddColumn("dbo.SpawningGroundSurvey_Detail", "SpawningStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpawningGroundSurvey_Detail", "SpawningStatus");
            DropColumn("dbo.SpawningGroundSurvey_Detail", "ReddMeasurements");
            DropColumn("dbo.SpawningGroundSurvey_Detail", "MortalityType");
            DropColumn("dbo.SpawningGroundSurvey_Detail", "NumberEggsRetained");

            RenameColumn(table: "dbo.SpawningGroundSurvey_Detail", name: "PercentRetained", newName: "SpawningStatus");
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "SpawningStatus", c => c.Int());
        }
    }
}
