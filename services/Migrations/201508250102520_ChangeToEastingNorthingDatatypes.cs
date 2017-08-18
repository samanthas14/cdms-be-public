namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeToEastingNorthingDatatypes : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update fields set controltype = 'easting' where name in ('Start Easting', 'End Easting') 
update datasetfields set controltype = 'easting' where label in ('Start Easting', 'End Easting')
update fields set controltype = 'northing' where name in ('Start Northing', 'End Northing') 
update datasetfields set controltype = 'northing' where label in ('Start Northing', 'End Northing')
");
        }
        
        public override void Down()
        {
            Sql(@"
update fields set controltype = 'number' where name in ('Start Easting', 'End Easting') 
update datasetfields set controltype = 'number' where label in ('Start Easting', 'End Easting')
update fields set controltype = 'number' where name in ('Start Northing', 'End Northing') 
update datasetfields set controltype = 'number' where label in ('Start Northing', 'End Northing')
");
        }
    }
}



