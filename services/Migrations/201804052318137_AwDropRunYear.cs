namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AwDropRunYear : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AdultWeir_Detail", "RunYear");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdultWeir_Detail", "RunYear", c => c.String());
        }
    }
}
