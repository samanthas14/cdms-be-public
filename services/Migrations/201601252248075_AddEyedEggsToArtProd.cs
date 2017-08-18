namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEyedEggsToArtProd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArtificialProduction_Detail", "EyedEggs", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArtificialProduction_Detail", "EyedEggs");
        }
    }
}
