namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HabAddPidSpidToLocations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "ProjectId", c => c.Int());
            AddColumn("dbo.Locations", "SubprojectId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "SubprojectId");
            DropColumn("dbo.Locations", "ProjectId");
        }
    }
}
