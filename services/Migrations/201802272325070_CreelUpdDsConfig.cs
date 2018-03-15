namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreelUpdDsConfig : DbMigration
    {
        public override void Up()
        {
            Sql(@"
declare @DatasetId as numeric

set @DatasetId = (select Id from dbo.Datasets where Name = 'Creel-Harvest' and [Description] = 'Harvest')

update dbo.Datasets
set Config = '{""DataEntryPage"":{""HiddenFields"":[""Instrument"",""addNewRow"",""BulkQaChange""],""ShowFields"":[""Surveyor"",""addSection"",""addInterview"",""addFisherman"",""addAnotherFish""]},""ActivitiesPage"":{""ShowFields"":[""headerdata.TimeStart"",""Location.Label""]}}'
where[Id] = @DatasetId
            ");
        }
        
        public override void Down()
        {
            Sql(@"
declare @DatasetId as numeric

set @DatasetId = (select Id from dbo.Datasets where Name = 'Creel-Harvest' and [Description] = 'Harvest')

update dbo.Datasets
set Config = '{""DataEntryPage"":{""HiddenFields"":[""Instrument"",""addNewRow"",""BulkQaChange""],""ShowFields"":[""Surveyor"",""addSection"",""addInterview"",""addFisherman"",""addAnotherFish""]},""ActivitiesPage"":{""ShowFields"":[""ActivityDate"",""headerdata.TimeStart"",""Location.Label""]}}'
where[Id] = @DatasetId
            ");
        }
    }
}
             
