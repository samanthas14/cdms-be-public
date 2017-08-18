namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScrewTrapHubTypeFrIntToFloat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ScrewTrap_Header", "Hubometer", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ScrewTrap_Header", "Hubometer", c => c.Int());
        }
    }
}
