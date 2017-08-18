namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changedatatypes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SpawningGroundSurvey_Header", "WaterVisibility", c => c.String());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.SpawningGroundSurvey_Header", "WaterVisibility", c => c.Int(nullable: false));
        }
    }
}
