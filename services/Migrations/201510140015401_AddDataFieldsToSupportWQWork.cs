namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataFieldsToSupportWQWork : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterQuality_Header", "FieldsheetLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterQuality_Header", "FieldsheetLink");
        }
    }
}
