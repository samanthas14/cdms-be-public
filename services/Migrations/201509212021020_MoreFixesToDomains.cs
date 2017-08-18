namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreFixesToDomains : DbMigration
    {
        public override void Up()
        {
            Sql(@"
    update fields set PossibleValues = '{""D"":""Dry"", ""L"":""Low"", ""M"":""Moderate"", ""H"":""High"", ""F"":""Flooding""}' where name = 'Flow'
    update fields set PossibleValues = '{""1"":""Riffles and Pools"", ""2"":""Riffles"", ""3"":""Neither Riffles nor Pools""}' where name = 'water visibility'
    update fields set PossibleValues = '{""C"":""Clear"", ""O"":""Overcast"", ""R"":""Rain"", ""S"":""Snow"", ""F"":""Foggy"", ""P"":""Partly Cloudy""}' 
            where name = 'weather' and fieldcategoryid in (select id from FieldCategories where name = 'Spawning Ground Survey')
");
        }
        
        public override void Down()
        {
            Sql(@"
    update fields set PossibleValues = '[""Dry"", ""Low"", ""Moderate"", ""High"", ""Flooding""]' where name = 'Flow'
    update fields set PossibleValues = '[""1 - Riffles and Pools"",""2 - Riffles"",""3 - Neither Riffles nor Pools""]' where name = 'water visibility'
    update fields set PossibleValues = '[""Clear (C)"", ""Overcast (O)"", ""Rain (R)"", ""Snow (S)"", ""Foggy (F)"", ""Partly Cloudy (P)""]' where name = 'weather' and fieldcategoryid in (select id from FieldCategories where name = 'Spawning Ground Survey')
");        }
    }
}
