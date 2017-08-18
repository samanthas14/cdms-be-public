namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SGSFixes : DbMigration
    {
        public override void Up()
        {
            Sql(@"
    update fields set PossibleValues = '{""B"":""Braided"", ""S"":""Single""}' where name = 'Channel'
    update fields set PossibleValues = '{""REDD"":""Redd"", ""LIVE"":""Live"", ""CARCASS"":""Carcass"", ""OBSERVATION"":""Observation""}' where name = 'Feature Type'
    update fields set PossibleValues = '{""M"":""Middle"", ""S"":""Side""}' where name = 'Redd Location'
    update fields set PossibleValues = '{""M"":""Male"",""F"":""Female"",""UNK"":""Unknown""}' where name = 'Sex'
    update fields set PossibleValues = '{""ONR"":""On Redd"", ""NR"":""Near Redd"", ""OR"":""Off Redd""}' where name = 'Fish Location'
    update fields set PossibleValues = '{""P"":""Pool"", ""TO"":""Pool Tail Out"", ""RI"":""Riffle"", ""GL"":""Glide""}' where name = 'Redd Habitat'
");
        }
        
        public override void Down()
        {
            Sql(@"
    update fields set PossibleValues = '[""Braided (B)"", ""Single (S)""]' where name = 'Channel'
    update fields set PossibleValues = '[""Redd"", ""Live"", ""Carcass"", ""Observation""]' where name = 'Feature Type'
    update fields set PossibleValues = '[""Middle (M)"", ""Side (S)""]' where name = 'Redd Location'
    update fields set PossibleValues = '[""M"",""F"",""UNK""]' where name = 'Sex'
    update fields set PossibleValues = '[""On Redd (OR)"", ""Near Redd (NR)"", ""Off Redd (OR)""]' where name = 'Fish Location'
    update fields set PossibleValues = '[""Pool (P)"", ""Pool Tail Out (TO)"", ""Riffle (RI)"", ""Glide (GL)""]' where name = 'Redd Habitat'
");
        }
    }
}



