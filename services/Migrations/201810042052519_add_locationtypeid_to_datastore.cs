namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_locationtypeid_to_datastore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Datastores", "LocationTypeId", c => c.String());
            DropColumn("dbo.Datastores", "DatastoreDatasetId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Datastores", "DatastoreDatasetId", c => c.String());
            DropColumn("dbo.Datastores", "LocationTypeId");
        }
    }
}
