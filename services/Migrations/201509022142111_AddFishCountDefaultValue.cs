namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFishCountDefaultValue : DbMigration
    {
        public override void Up()
        {
            Sql(@"
    update fields        set [rule] = '{""DefaultValue"":""1""}' where name  = 'FishCount';
    update datasetfields set [rule] = '{""DefaultValue"":""1""}' where label = 'FishCount';
");
        }
        
        public override void Down()
        {
            Sql(@"
    update fields        set [rule] = NULL where name  = 'FishCount';
    update datasetfields set [rule] = NULL where label = 'FishCount';
");
        }
    }
}
