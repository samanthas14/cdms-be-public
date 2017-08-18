namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeScrewTrapTimeTypes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ScrewTrap_Header", "ArrivalTime", c => c.String());
            AlterColumn("dbo.ScrewTrap_Header", "DepartTime", c => c.String());
            AlterColumn("dbo.ScrewTrap_Header", "HubometerTime", c => c.String());
            AlterColumn("dbo.ScrewTrap_Header", "TrapStopped", c => c.String());
            AlterColumn("dbo.ScrewTrap_Header", "TrapStarted", c => c.String());
            AlterColumn("dbo.ScrewTrap_Header", "FishCollected", c => c.String());
            AlterColumn("dbo.ScrewTrap_Header", "FishReleased", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ScrewTrap_Header", "FishReleased", c => c.DateTime());
            AlterColumn("dbo.ScrewTrap_Header", "FishCollected", c => c.DateTime());
            AlterColumn("dbo.ScrewTrap_Header", "TrapStarted", c => c.DateTime());
            AlterColumn("dbo.ScrewTrap_Header", "TrapStopped", c => c.DateTime());
            AlterColumn("dbo.ScrewTrap_Header", "HubometerTime", c => c.DateTime());
            AlterColumn("dbo.ScrewTrap_Header", "DepartTime", c => c.DateTime());
            AlterColumn("dbo.ScrewTrap_Header", "ArrivalTime", c => c.DateTime());
        }
    }
}
