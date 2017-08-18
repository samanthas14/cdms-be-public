namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class P4AddNewItemsToStAndEf : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Electrofishing_Detail", "EventTypeD", c => c.String());
            AddColumn("dbo.Electrofishing_Detail", "SecondPitTag", c => c.String());
            AddColumn("dbo.Electrofishing_Detail", "RacewayTransectTank", c => c.String());
            AddColumn("dbo.Electrofishing_Detail", "LifeStage", c => c.String());
            AddColumn("dbo.Electrofishing_Detail", "CodedWireTag", c => c.String());
            AddColumn("dbo.Electrofishing_Detail", "BroodYear", c => c.Int());
            AddColumn("dbo.Electrofishing_Detail", "MigrationYear", c => c.Int());
            AddColumn("dbo.Electrofishing_Detail", "SizeOfCount", c => c.String());
            AddColumn("dbo.Electrofishing_Detail", "ScaleId", c => c.String());
            AddColumn("dbo.Electrofishing_Detail", "Containment", c => c.String());
            AddColumn("dbo.ScrewTrap_Detail", "EventType", c => c.String());
            AddColumn("dbo.ScrewTrap_Detail", "SecondPitTag", c => c.String());
            AddColumn("dbo.ScrewTrap_Detail", "RacewayTransectTank", c => c.String());
            AddColumn("dbo.ScrewTrap_Detail", "LifeStage", c => c.String());
            AddColumn("dbo.ScrewTrap_Detail", "GeneticId", c => c.String());
            AddColumn("dbo.ScrewTrap_Detail", "CodedWireTag", c => c.String());
            AddColumn("dbo.ScrewTrap_Detail", "BroodYear", c => c.Int());
            AddColumn("dbo.ScrewTrap_Detail", "MigrationYear", c => c.Int());
            AddColumn("dbo.ScrewTrap_Detail", "SizeOfCount", c => c.String());
            AddColumn("dbo.ScrewTrap_Detail", "ScaleId", c => c.String());
            AddColumn("dbo.ScrewTrap_Detail", "Containment", c => c.String());
            AddColumn("dbo.ScrewTrap_Header", "Weather", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScrewTrap_Header", "Weather");
            DropColumn("dbo.ScrewTrap_Detail", "Containment");
            DropColumn("dbo.ScrewTrap_Detail", "ScaleId");
            DropColumn("dbo.ScrewTrap_Detail", "SizeOfCount");
            DropColumn("dbo.ScrewTrap_Detail", "MigrationYear");
            DropColumn("dbo.ScrewTrap_Detail", "BroodYear");
            DropColumn("dbo.ScrewTrap_Detail", "CodedWireTag");
            DropColumn("dbo.ScrewTrap_Detail", "GeneticId");
            DropColumn("dbo.ScrewTrap_Detail", "LifeStage");
            DropColumn("dbo.ScrewTrap_Detail", "RacewayTransectTank");
            DropColumn("dbo.ScrewTrap_Detail", "SecondPitTag");
            DropColumn("dbo.ScrewTrap_Detail", "EventType");
            DropColumn("dbo.Electrofishing_Detail", "Containment");
            DropColumn("dbo.Electrofishing_Detail", "ScaleId");
            DropColumn("dbo.Electrofishing_Detail", "SizeOfCount");
            DropColumn("dbo.Electrofishing_Detail", "MigrationYear");
            DropColumn("dbo.Electrofishing_Detail", "BroodYear");
            DropColumn("dbo.Electrofishing_Detail", "CodedWireTag");
            DropColumn("dbo.Electrofishing_Detail", "LifeStage");
            DropColumn("dbo.Electrofishing_Detail", "RacewayTransectTank");
            DropColumn("dbo.Electrofishing_Detail", "SecondPitTag");
            DropColumn("dbo.Electrofishing_Detail", "EventTypeD");
        }
    }
}
