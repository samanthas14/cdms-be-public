namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReAddFieldsJustRemoved : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StreamNet_NOSA_Detail", "ShadowId", c => c.String(nullable: false, defaultValueSql: "newid()"));
            AddColumn("dbo.StreamNet_SAR_Detail", "ShadowId", c => c.String(nullable: false, defaultValueSql: "newid()"));

            AlterColumn("dbo.StreamNet_NOSA_Detail", "ShadowId", c => c.String(nullable: false, defaultValueSql: "newid()"));
            AlterColumn("dbo.StreamNet_SAR_Detail", "ShadowId", c => c.String(nullable: false, defaultValueSql: "newid()"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StreamNet_SAR_Detail", "ShadowId");
            DropColumn("dbo.StreamNet_NOSA_Detail", "ShadowId");
        }
    }
}
