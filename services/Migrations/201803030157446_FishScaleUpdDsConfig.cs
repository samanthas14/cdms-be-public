namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FishScaleUpdDsConfig : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.[Datasets]
set Config = '{""DataEntryPage"":{""HiddenFields"":[""Instrument"",""Location"",""BulkQaChange""]},""ActivitiesPage"":{""ShowFields"":[""headerdata.RunYear"",""Location.Label"",""User.Fullname""],""HiddenFields"":[""MapButton""]}}'
where[Id] in (select[Id] from dbo.Datasets where [Description] like '%Fish Scale%')
            ");
        }
        
        public override void Down()
        {
            Sql(@"
update dbo.[Datasets]
set Config = '{""DataEntryPage"":{""HiddenFields"":[""Instrument"",""Location"",""BulkQaChange""]},""ActivitiesPage"":{""ShowFields"":[""headerdata.RunYear"",""Location.Label"",""User.Fullname""]}}'
where[Id] in (select[Id] from dbo.Datasets where [Description] like '%Fish Scale%')
            ");
        }
    }
}
