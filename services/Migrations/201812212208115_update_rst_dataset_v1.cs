namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_rst_dataset_v1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NPT_RST_Detail", "StartDate", c => c.DateTime());
            AlterColumn("dbo.NPT_RST_Detail", "EndDate", c => c.DateTime());
            AlterColumn("dbo.NPT_RST_Detail", "StdError", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NPT_RST_Detail", "StdError", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.NPT_RST_Detail", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.NPT_RST_Detail", "StartDate", c => c.DateTime(nullable: false));
        }
    }
}
