namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFishScalesDataTypes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FishScales_Detail", "FreshwaterAge", c => c.Double());
            AlterColumn("dbo.FishScales_Detail", "SaltWaterAge", c => c.Double());
            AlterColumn("dbo.FishScales_Detail", "TotalAdultAge", c => c.Double());
            AlterColumn("dbo.FishScales_Detail", "Circuli", c => c.Double());

            Sql(@"
    update fields set datatype = 'float' where name = 'Freshwater Age'
    update fields set datatype = 'float' where name = 'Salt Water Age'
    update fields set datatype = 'float' where name = 'Total Adult Age'
    update fields set datatype = 'float' where name = 'Circuli'
");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FishScales_Detail", "TotalAdultAge", c => c.Int());
            AlterColumn("dbo.FishScales_Detail", "SaltWaterAge", c => c.Int());
            AlterColumn("dbo.FishScales_Detail", "FreshwaterAge", c => c.Int());
            AlterColumn("dbo.FishScales_Detail", "Circuli", c => c.Int());

            Sql(@"
    update fields set datatype = 'int' where name = 'Freshwater Age'
    update fields set datatype = 'int' where name = 'Salt Water Age'
    update fields set datatype = 'int' where name = 'Total Adult Age'
    update fields set datatype = 'int' where name = 'Circuli'
");
        }
    }
}
