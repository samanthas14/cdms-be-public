namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeColumnNamesMoreConsistent : DbMigration
    {
        public override void Up()
        {
            // No undo
            Sql(@"update fields set name = 'Additional Positional Comments' where name = 'Additional Positional Comment'");
            Sql(@"update datasetfields set label = 'Additional Positional Comments' where label = 'Additional Positional Comment'");
        }
        
        public override void Down()
        {
        }
    }
}
