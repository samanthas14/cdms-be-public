namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDatatype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LaboratoryCharacteristics", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LaboratoryCharacteristics", "Name", c => c.DateTime(nullable: false));
        }
    }
}
