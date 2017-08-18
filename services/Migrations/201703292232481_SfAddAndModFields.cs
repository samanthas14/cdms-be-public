namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SfAddAndModFields : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.SnorkelFish_Header", "StartWaterTemp", c => c.Double());
            RenameColumn("dbo.SnorkelFish_Header", "WaterTemperature", "StartWaterTemp");
            AddColumn("dbo.SnorkelFish_Header", "HabitatVisitId", c => c.Int());
            AddColumn("dbo.SnorkelFish_Header", "EndWaterTemp", c => c.Double());

        }
        
        public override void Down()
        {
            //AddColumn("dbo.SnorkelFish_Header", "WaterTemperature", c => c.Double());
            RenameColumn("dbo.SnorkelFish_Header", "StartWaterTemp", "WaterTemperature");
            DropColumn("dbo.SnorkelFish_Header", "EndWaterTemp");
            DropColumn("dbo.SnorkelFish_Header", "HabitatVisitId");
            DropColumn("dbo.SnorkelFish_Header", "StartWaterTemp");
        }
    }
}
