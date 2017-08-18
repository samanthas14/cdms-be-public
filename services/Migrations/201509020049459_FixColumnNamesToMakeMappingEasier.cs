namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixColumnNamesToMakeMappingEasier : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update fields        set name  = 'Tagging Method' where name  = 'TaggingMethod'
update datasetfields set label = 'Tagging Method' where label = 'TaggingMethod'
");
        }
        
        public override void Down()
        {
            Sql(@"
update fields        set name  = 'TaggingMethod' where name  = 'Tagging Method'
update datasetfields set label = 'TaggingMethod' where label = 'Tagging Method'
");
        }
    }
}
