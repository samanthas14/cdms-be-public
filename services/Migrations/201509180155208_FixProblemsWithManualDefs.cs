namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test1 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.SnorkelFish_Detail", "ChannelUnitType", c => c.String());
            //AddColumn("dbo.SnorkelFish_Detail", "AEMHabitatType", c => c.String());
            //AddColumn("dbo.SnorkelFish_Detail", "AEMLength", c => c.Int());
            //AddColumn("dbo.SnorkelFish_Header", "IsAEM", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.SnorkelFish_Header", "IsAEM");
            //DropColumn("dbo.SnorkelFish_Detail", "AEMLength");
            //DropColumn("dbo.SnorkelFish_Detail", "AEMHabitatType");
            //DropColumn("dbo.SnorkelFish_Detail", "ChannelUnitType");
        }
    }
}
