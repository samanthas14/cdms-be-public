namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WqUpdateDatasetConfig : DbMigration
    {
        public override void Up()
        {
            Sql(@"
declare @DatasetId as int

set @DatasetId = (select Id from dbo.Datasets where Name like '%Water Chemistry%')

update dbo.[Datasets]
set Config = '{""DataEntryPage"":{""HiddenFields"": [""BulkQaChange"",""ActivityDate""], ""ShowFields"":[""Instrument""]},""ActivitiesPage"":{""ShowFields"":[""Description"",""Location.Label"",""headerdata.DataType"",""User.Fullname""]}}'
where[Id] = @DatasetId
            ");
        }
        
        public override void Down()
        {
            Sql(@"
declare @DatasetId as int

set @DatasetId = (select Id from dbo.Datasets where Name like '%Water Chemistry%')

update dbo.[Datasets]
set Config = '{""DataEntryPage"":{""HiddenFields"": [""BulkQaChange""]},""ActivitiesPage"":{""ShowFields"":[""Description"",""Location.Label"",""headerdata.DataType"",""User.Fullname""]}}'
where[Id] = @DatasetId
            ");
        }
    }
}
