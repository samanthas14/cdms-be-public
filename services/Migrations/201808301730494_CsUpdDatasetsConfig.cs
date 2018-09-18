namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CsUpdDatasetsConfig : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.Datasets
set Config = '{""DataEntryPage"":{""HiddenFields"":[""Instrument"",""addNewRow"",""BulkQaChange""],""ShowFields"":[""Surveyor"",""addSection"",""addInterview"",""addFisherman"",""addAnotherFish""]},""ActivitiesPage"":{""ShowFields"":[""headerdata.TimeStart"",""Location.Label""]},""SeasonsPage"":{""Species"":[""BUT"",""CHS"",""STS""]}}'
where[Description] = 'Harvest'
            ");
        }
        
        public override void Down()
        {
            Sql(@"
update dbo.Datasets
set Config = '{""DataEntryPage"":{""HiddenFields"":[""Instrument"",""addNewRow"",""BulkQaChange""],""ShowFields"":[""Surveyor"",""addSection"",""addInterview"",""addFisherman"",""addAnotherFish""]},""ActivitiesPage"":{""ShowFields"":[""headerdata.TimeStart"",""Location.Label""]}}'
where[Description] = 'Harvest'
            ");
        }
    }
}
