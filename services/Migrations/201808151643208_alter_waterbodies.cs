namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_waterbodies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterBodies", "StreamFull_Id", c => c.Int(nullable: false));
            AddColumn("dbo.WaterBodies", "StreamName", c => c.String());
            AddColumn("dbo.WaterBodies", "TribToName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterBodies", "TribToName");
            DropColumn("dbo.WaterBodies", "StreamName");
            DropColumn("dbo.WaterBodies", "StreamFull_Id");
        }
    }
}
