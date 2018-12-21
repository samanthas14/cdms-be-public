namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_carcass_detail : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SGS_Carcass_Detail", "PercentSpawned", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SGS_Carcass_Detail", "PercentSpawned", c => c.Int(nullable: false));
        }
    }
}
