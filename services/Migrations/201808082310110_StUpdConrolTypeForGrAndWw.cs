namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StUpdConrolTypeForGrAndWw : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.DatasetFields set ControlType = 'time' where DatasetId in (1214, 1215) and DbColumnName = 'ArrivalTime'
            ");
        }
        
        public override void Down()
        {
            Sql(@"
update dbo.DatasetFields set ControlType = 'text' where DatasetId in (1214, 1215) and DbColumnName = 'ArrivalTime'
            ");
        }
    }
}
