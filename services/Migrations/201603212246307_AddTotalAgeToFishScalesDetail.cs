namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTotalAgeToFishScalesDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FishScales_Detail", "TotalAge", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FishScales_Detail", "TotalAge");
        }
    }
}
