namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSpawingGroundSurvey : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SpawningGroundSurvey_Header", "Species", c => c.String());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "Flow", c => c.String());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.SpawningGroundSurvey_Header", "Flow", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Header", "Species", c => c.Int(nullable: false));
        }
    }
}
