namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEstimatedLocationFieldToSGS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpawningGroundSurvey_Detail", "EstimatedLocation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpawningGroundSurvey_Detail", "EstimatedLocation");
        }
    }
}
