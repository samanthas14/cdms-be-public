namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FloatToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LaboratoryCharacteristics", "MDL", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LaboratoryCharacteristics", "MDL", c => c.Single());
        }
    }
}
