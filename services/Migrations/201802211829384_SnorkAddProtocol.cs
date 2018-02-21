namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SnorkAddProtocol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SnorkelFish_Header", "Protocol", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SnorkelFish_Header", "Protocol");
        }
    }
}
