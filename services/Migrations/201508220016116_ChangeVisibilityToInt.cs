namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeVisibilityToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SnorkelFish_Header", "Visibility", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SnorkelFish_Header", "Visibility", c => c.String());
        }
    }
}
