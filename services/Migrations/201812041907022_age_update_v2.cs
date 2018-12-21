namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class age_update_v2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NPT_Age_Detail", "UniqueFishID", c => c.String());
            AddColumn("dbo.NPT_Age_Detail", "EuropeanAge", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NPT_Age_Detail", "EuropeanAge");
            DropColumn("dbo.NPT_Age_Detail", "UniqueFishID");
        }
    }
}
