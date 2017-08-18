namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFieldAlreadyHaveDateByDefault : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SnorkelFish_Header", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SnorkelFish_Header", "Date", c => c.DateTime(nullable: false));
        }
    }
}
