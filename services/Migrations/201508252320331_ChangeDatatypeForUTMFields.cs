namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDatatypeForUTMFields : DbMigration
    {
        public override void Up()
        {
            Sql(@"
    update fields        set ControlType = 'easting' where name = 'Easting (UTM)'
    update datasetfields set ControlType = 'easting' where label = 'Easting (UTM)'
                         
    update fields        set ControlType = 'northing' where name = 'Northing (UTM)'
    update datasetfields set ControlType = 'northing' where label = 'Northing (UTM)'
");
        }
        
        public override void Down()
        {
            Sql(@"
    update fields        set ControlType = 'number' where name = 'Easting (UTM)'
    update datasetfields set ControlType = 'number' where label = 'Easting (UTM)'
                                            
    update fields        set ControlType = 'number' where name = 'Northing (UTM)'
    update datasetfields set ControlType = 'number' where label = 'Northing (UTM)'
");
        }
    }
}
