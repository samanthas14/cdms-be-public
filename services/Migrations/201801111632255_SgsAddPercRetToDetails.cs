namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SgsAddPercRetToDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpawningGroundSurvey_Detail", "PercentRetained", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpawningGroundSurvey_Detail", "PercentRetained");
        }
    }
}
