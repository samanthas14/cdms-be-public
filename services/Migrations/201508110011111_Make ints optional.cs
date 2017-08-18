namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Makeintsoptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SpawningGroundSurvey_Header", "StartTemperature", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "EndTemperature", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "StartEasting", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "StartNorthing", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "EndEasting", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "EndNorthing", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "FlaggedRedds", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "NewRedds", c => c.Int());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.SpawningGroundSurvey_Header", "NewRedds", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Header", "FlaggedRedds", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Header", "EndNorthing", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Header", "EndEasting", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Header", "StartNorthing", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Header", "StartEasting", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Header", "EndTemperature", c => c.Int(nullable: false));
            //AlterColumn("dbo.SpawningGroundSurvey_Header", "StartTemperature", c => c.Int(nullable: false));
        }
    }
}
