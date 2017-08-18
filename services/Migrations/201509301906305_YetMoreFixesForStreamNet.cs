namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YetMoreFixesForStreamNet : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StreamNet_RperS_Detail", "ShadowId", c => c.String(nullable: false, defaultValueSql: "newid()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StreamNet_RperS_Detail", "ShadowId", c => c.Guid(nullable: false));
        }
    }
}
