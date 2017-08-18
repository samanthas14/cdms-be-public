namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removefield : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WaterQuality_Header", "Laboratory_Id", "dbo.Laboratories");
            DropIndex("dbo.WaterQuality_Header", new[] { "Laboratory_Id" });
            DropColumn("dbo.WaterQuality_Header", "Laboratory_Id");

            Sql("delete from DatasetFields where label = 'Laboratory'");
            Sql("delete from Fields        where name  = 'Laboratory'");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WaterQuality_Header", "Laboratory_Id", c => c.Int());
            CreateIndex("dbo.WaterQuality_Header", "Laboratory_Id");
            AddForeignKey("dbo.WaterQuality_Header", "Laboratory_Id", "dbo.Laboratories", "Id");
        }
    }
}
