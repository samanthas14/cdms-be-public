namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HabAddHiItemType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HabitatItems", "ItemType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HabitatItems", "ItemType");
        }
    }
}
