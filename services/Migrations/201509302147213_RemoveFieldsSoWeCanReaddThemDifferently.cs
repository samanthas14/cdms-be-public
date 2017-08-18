namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFieldsSoWeCanReaddThemDifferently : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.StreamNet_NOSA_Detail", "ShadowId");
            DropColumn("dbo.StreamNet_SAR_Detail", "ShadowId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StreamNet_SAR_Detail", "ShadowId", c => c.String());
            AddColumn("dbo.StreamNet_NOSA_Detail", "ShadowId", c => c.String());
        }
    }
}
