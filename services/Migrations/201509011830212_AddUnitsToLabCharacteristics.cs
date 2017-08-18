namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUnitsToLabCharacteristics : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LaboratoryCharacteristics", "Units", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LaboratoryCharacteristics", "Units");
        }
    }
}
