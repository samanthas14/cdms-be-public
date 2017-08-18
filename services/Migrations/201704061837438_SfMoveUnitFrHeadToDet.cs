namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SfMoveUnitFrHeadToDet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SnorkelFish_Detail", "Unit", c => c.String());
            DropColumn("dbo.SnorkelFish_Header", "Unit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SnorkelFish_Header", "Unit", c => c.String());
            DropColumn("dbo.SnorkelFish_Detail", "Unit");
        }
    }
}
