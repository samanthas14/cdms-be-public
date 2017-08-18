namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionToLabCharacteristic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LaboratoryCharacteristics", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LaboratoryCharacteristics", "Description");
        }
    }
}
