namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDataTypeForCirculi : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FishScales_Detail", "Circuli", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FishScales_Detail", "Circuli", c => c.Int());
        }
    }
}
