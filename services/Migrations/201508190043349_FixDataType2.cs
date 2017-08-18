namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDataType2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LaboratoryCharacteristics", "MethodId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LaboratoryCharacteristics", "MethodId", c => c.Int(nullable: false));
        }
    }
}
