namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreStreamNetFixes1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StreamNet_RperS_Detail", "ShadowId", c => c.Guid(nullable: false, defaultValueSql: "newid()"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StreamNet_RperS_Detail", "ShadowId");
        }
    }
}
