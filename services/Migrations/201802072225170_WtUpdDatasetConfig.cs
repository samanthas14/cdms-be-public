namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WtUpdDatasetConfig : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.Datasets
set Config = '{""DataEntryPage"":{""HiddenFields"":[""ActivityDate""],""ShowFields"":[""Timezone"",""Instrument""]},""ActivitiesPage"":{""ShowFields"":[""Location.Label"",""headerdata.FieldActivityType"",""Description"",""User.Fullname""]}}'
where[Description] like '%Water%' and[Description] != 'Water Chemistry' and [Name] != 'Temperature'
            ");
        }
        
        public override void Down()
        {
            Sql(@"
update dbo.Datasets
set Config = '{""DataEntryPage"":{""HiddenFields"":[""ActivityDate""],""ShowFields"":[""Timezone""]},""ActivitiesPage"":{""ShowFields"":[""Location.Label"",""headerdata.FieldActivityType"",""Description"",""User.Fullname""]}}'
where[Description] like '%Water%' and[Description] != 'Water Chemistry' and [Name] != 'Temperature'
            ");
        }
    }
}
