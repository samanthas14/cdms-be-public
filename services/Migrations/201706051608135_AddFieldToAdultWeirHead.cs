namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldToAdultWeirHead : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Inactive", c => c.Int());
            AddColumn("dbo.AdultWeir_Header", "FieldSheetFile", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdultWeir_Header", "FieldSheetFile");
            DropColumn("dbo.Users", "Inactive");
        }
    }
}
