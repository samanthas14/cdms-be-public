namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removelocationfield : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SpawningGroundSurvey_Header", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SpawningGroundSurvey_Header", "Location", c => c.String());
        }
    }
}
