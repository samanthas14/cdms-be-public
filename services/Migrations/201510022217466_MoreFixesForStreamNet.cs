namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreFixesForStreamNet : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update datasetfields set validation = 'Required' where label in ('Publish', 'ContactAgency', 'SubmitAgency','contactEmail','ContactPersonFirst','ContactPersonLast','ContactPhone','DataStatus','MethodNumber','NulLRecord','SpawningYear','TRTmethod','WaterBody')
update fields set validation = 'Required' where name in ('Publish', 'ContactAgency', 'SubmitAgency','contactEmail','ContactPersonFirst','ContactPersonLast','ContactPhone','DataStatus','MethodNumber','NulLRecord','SpawningYear','TRTmethod','WaterBody')

update fields set [rule] = '{""DefaultValue"":""CTUIR""}' where name = 'ContactAgency'
update datasetfields set [rule] = '{""DefaultValue"":""CTUIR""}' where label = 'ContactAgency'

update fields set possiblevalues = '[""CTUIR""]' where name = 'ContactAgency'
update fields set validation = NULL where validation = 'NULL'

update datasetfields set validation = 'Required' where label in ('Publish', 'ContactAgency', 'SubmitAgency')
update fields set validation = 'Required' where name in ('Publish', 'ContactAgency', 'SubmitAgency')

");
        }
        
        public override void Down()
        {
            Sql(@"
update datasetfields set validation = NULL where label in ('Publish', 'ContactAgency', 'SubmitAgency','contactEmail','ContactPersonFirst','ContactPersonLast','ContactPhone','DataStatus','MethodNumber','NulLRecord','SpawningYear','TRTmethod','WaterBody')
update fields set validation = NULL where name in ('Publish', 'ContactAgency', 'SubmitAgency','contactEmail','ContactPersonFirst','ContactPersonLast','ContactPhone','DataStatus','MethodNumber','NulLRecord','SpawningYear','TRTmethod','WaterBody')
update fields set possiblevalues = NULL where name = 'ContactAgency'
update fields set [rule] = NULL where name = 'ContactAgency'
update datasetfields set [rule] = NULL where label = 'ContactAgency'

");        
        }
    }
}
