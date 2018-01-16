namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SgsDetPercRetAllowNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "PercentRetained", c => c.Single());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "PercentRetained", c => c.Single(nullable: false));
        }
    }
}
