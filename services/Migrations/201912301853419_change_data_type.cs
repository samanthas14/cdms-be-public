namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_data_type : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StreamNet_JuvOutmigrationDetail_Detail", "TotalNaturalAlpha", c => c.Single(nullable: false));
            AlterColumn("dbo.StreamNet_JuvOutmigrationDetail_Detail", "SurvivalRate", c => c.Single(nullable: false));
            AlterColumn("dbo.StreamNet_JuvOutmigrationDetail_Detail", "SurvivalRateLowerLimit", c => c.Single(nullable: false));
            AlterColumn("dbo.StreamNet_JuvOutmigrationDetail_Detail", "SurvivalRateUpperLimit", c => c.Single(nullable: false));
            AlterColumn("dbo.StreamNet_JuvOutmigrationDetail_Detail", "SurvivalRateAlpha", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StreamNet_JuvOutmigrationDetail_Detail", "SurvivalRateAlpha", c => c.Int(nullable: false));
            AlterColumn("dbo.StreamNet_JuvOutmigrationDetail_Detail", "SurvivalRateUpperLimit", c => c.Int(nullable: false));
            AlterColumn("dbo.StreamNet_JuvOutmigrationDetail_Detail", "SurvivalRateLowerLimit", c => c.Int(nullable: false));
            AlterColumn("dbo.StreamNet_JuvOutmigrationDetail_Detail", "SurvivalRate", c => c.Int(nullable: false));
            AlterColumn("dbo.StreamNet_JuvOutmigrationDetail_Detail", "TotalNaturalAlpha", c => c.Int(nullable: false));
        }
    }
}
