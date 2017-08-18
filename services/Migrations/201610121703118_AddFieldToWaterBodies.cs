namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldToWaterBodies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterBodies", "GNIS_ID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterBodies", "GNIS_ID");
        }
    }
}
