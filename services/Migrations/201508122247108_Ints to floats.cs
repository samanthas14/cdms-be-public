namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Intstofloats : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "Easting", c => c.Single());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "Northing", c => c.Single());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "EastingUTM", c => c.Single());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "NorthingUTM", c => c.Single());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "NorthingUTM", c => c.Int());
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "EastingUTM", c => c.Int());
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "Northing", c => c.Int());
            //AlterColumn("dbo.SpawningGroundSurvey_Detail", "Easting", c => c.Int());
        }
    }
}
