namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SGSFieldUpdates : DbMigration
    {
        public override void Up()
        {
            Sql(@"
    update fields        set [rule] = '{""DefaultValue"":""No""}' where name  = 'Estimated Location'
    update datasetfields set [rule] = '{""DefaultValue"":""No""}' where label = 'Estimated Location'
");
        }
        
        public override void Down()
        {
            Sql(@"
    update fields        set [rule] = NULL where name  = 'Estimated Location'
    update datasetfields set [rule] = NULL where label = 'Estimated Location'
");
        }
    }
}
