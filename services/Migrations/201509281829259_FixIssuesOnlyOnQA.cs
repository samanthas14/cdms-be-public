namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixIssuesOnlyOnQA : DbMigration
    {
        public override void Up()
        {
            Sql(@"
    delete from datasetfields where fieldid in (select id from fields where description = 'AEM average size of the group of fish species observed in the current unit (deliniated every 10 cm)')
    delete from fields where description = 'AEM average size of the group of fish species observed in the current unit (deliniated every 10 cm)'

    delete from datasetfields where fieldid in (select id from fields where name =  'Length (mm)')
    delete from fields where name =  'Length (mm)'
");
        }
        
        public override void Down()
        {
        }
    }
}
