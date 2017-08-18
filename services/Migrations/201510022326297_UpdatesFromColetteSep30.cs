namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatesFromColetteSep30 : DbMigration
    {
        public override void Up()
        {
            // Fish Scales
            // Change the attribute name :
            // Total Adult Age (years) to European Age

            Sql(@"
update fields set name = 'European Age' where name = 'Total Adult Age'
update datasetfields set label = 'European Age' where label = 'Total Adult Age'
");
        }
        

        public override void Down()
        {
            Sql(@"
update fields set name = 'Total Adult Age' where name = 'European Age'
update datasetfields set label = 'Total Adult Age' where label = 'European Age'
");
        }
    }
}
