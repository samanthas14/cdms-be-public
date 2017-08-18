namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WtAddPressureFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterTemp_Detail", "PSI", c => c.Double());
            AddColumn("dbo.WaterTemp_Detail", "AbsolutePressure", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterTemp_Detail", "AbsolutePressure");
            DropColumn("dbo.WaterTemp_Detail", "PSI");
        }
    }
}
