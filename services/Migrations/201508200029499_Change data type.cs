namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changedatatype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WaterQuality_Detail", "Result", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WaterQuality_Detail", "Result", c => c.Single());
        }
    }
}
