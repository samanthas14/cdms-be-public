namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GenUpdDescWiSampleYear : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.Activities set [Description] = r.SampleYear
from
(
	select distinct ActivityId, SampleYear from dbo.Genetic_Detail_VW
	where ActivityId in 
		(
		select Id
		from dbo.Activities
		where DatasetId in (select Id from dbo.Datasets where DatastoreId in (select Id from dbo.Datastores where TablePrefix = 'Genetic'))
		)
	--order by ActivityId
) as r
where [Id] in 
(
	select distinct ActivityId from dbo.Genetic_Detail_VW
	where ActivityId in 
		(
		select Id
		from dbo.Activities
		where DatasetId in (select Id from dbo.Datasets where DatastoreId in (select Id from dbo.Datastores where TablePrefix = 'Genetic'))
		)
)
and dbo.Activities.Id = r.ActivityId

update dbo.Datasets set Config = '{""DataEntryPage"": {""HiddenFields"": [""Instrument"",""BulkQaChange""]},""ActivitiesPage"":{""ShowFields"":[""Description""]}}'
where[Id] in (select Id from dbo.Datasets where DatastoreId in (select Id from dbo.Datastores where TablePrefix = 'Genetic'))
            ");
        }
        
        public override void Down()
        {
            Sql(@"
update dbo.Activities set [Description] = null
from
(
	select distinct ActivityId, SampleYear from dbo.Genetic_Detail_VW
	where ActivityId in 
		(
		select Id
		from dbo.Activities
		where DatasetId in (select Id from dbo.Datasets where DatastoreId in (select Id from dbo.Datastores where TablePrefix = 'Genetic'))
		)
	--order by ActivityId
) as r
where [Id] in 
(
	select distinct ActivityId from dbo.Genetic_Detail_VW
	where ActivityId in 
		(
		select Id
		from dbo.Activities
		where DatasetId in (select Id from dbo.Datasets where DatastoreId in (select Id from dbo.Datastores where TablePrefix = 'Genetic'))
		)
)
and dbo.Activities.Id = r.ActivityId

update dbo.Datasets set Config = '{""DataEntryPage"": {""HiddenFields"": [""Instrument"",""BulkQaChange""]}}'
where[Id] in (select Id from dbo.Datasets where DatastoreId in (select Id from dbo.Datastores where TablePrefix = 'Genetic'))
            ");
        }
    }
}
