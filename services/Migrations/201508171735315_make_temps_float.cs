namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class make_temps_float : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "Temp", c => c.Single());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "StartTemperature", c => c.Single());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "EndTemperature", c => c.Single());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SpawningGroundSurvey_Header", "EndTemperature", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "StartTemperature", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "Temp", c => c.Int());
        }
    }
}
