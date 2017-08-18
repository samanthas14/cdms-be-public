namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameSpeciesToTargetSpeciesColumnInSGS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpawningGroundSurvey_Header", "TargetSpecies", c => c.String());
            DropColumn("dbo.SpawningGroundSurvey_Header", "Species");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SpawningGroundSurvey_Header", "Species", c => c.String());
            DropColumn("dbo.SpawningGroundSurvey_Header", "TargetSpecies");
        }
    }
}
