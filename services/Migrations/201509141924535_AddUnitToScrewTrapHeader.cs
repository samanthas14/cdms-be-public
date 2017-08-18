namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUnitToScrewTrapHeader : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScrewTrap_Header", "Unit", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScrewTrap_Header", "Unit");
        }
    }
}
