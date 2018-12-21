namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class included_datasheet_link : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SGS_Carcass_Header", "FieldsheetLink", c => c.String());
            AddColumn("dbo.SGS_Redd_Header", "FieldsheetLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SGS_Redd_Header", "FieldsheetLink");
            DropColumn("dbo.SGS_Carcass_Header", "FieldsheetLink");
        }
    }
}
