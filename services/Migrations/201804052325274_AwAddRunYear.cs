namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AwAddRunYear : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdultWeir_Detail", "RunYear", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdultWeir_Detail", "RunYear");
        }
    }
}
