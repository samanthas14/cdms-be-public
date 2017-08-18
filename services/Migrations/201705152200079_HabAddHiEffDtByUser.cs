namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HabAddHiEffDtByUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HabitatItems", "EffDt", c => c.DateTime(nullable: false));
            AddColumn("dbo.HabitatItems", "ByUserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HabitatItems", "ByUserId");
            DropColumn("dbo.HabitatItems", "EffDt");
        }
    }
}
