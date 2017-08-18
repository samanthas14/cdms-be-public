namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addmdlresultstowaterqualitytable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterQuality_Detail", "MdlResults", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterQuality_Detail", "MdlResults");
        }
    }
}
