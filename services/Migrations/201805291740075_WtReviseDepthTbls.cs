namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WtReviseDepthTbls : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterTemp_Detail", "Depth", c => c.Double());
            DropColumn("dbo.WaterTemp_Header", "DepthToWater");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WaterTemp_Header", "DepthToWater", c => c.Double());
            DropColumn("dbo.WaterTemp_Detail", "Depth");
        }
    }
}
