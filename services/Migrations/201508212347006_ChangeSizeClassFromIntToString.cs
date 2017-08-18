namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSizeClassFromIntToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SnorkelFish_Detail", "SizeClass", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SnorkelFish_Detail", "SizeClass", c => c.Int());
        }
    }
}
