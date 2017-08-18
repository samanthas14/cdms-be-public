namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixMicro : DbMigration
    {
        public override void Up()
        {
            Sql(@"
    update fields set PossibleValues = N'{""COLONIES (MPN)/100 ML"": ""Colonies (MPN)/100 mL"", ""COUNTS (MPN)/100 ML"": ""Counts (MPN)/100 mL"", ""MG/L"": ""mg/L"", ""MG/M3"": ""mg/m3"", ""NTU"": ""NTU"", ""SU"": ""SU"", ""UNITS"": ""Units"", ""ΜG/L"": ""µg/L"", ""ΜMHO/CM (MICRO MHO/CM)"":""µmho/cm (micro mho/cm)""}'
        where name = 'Result Units'

    update fields set possiblevalues = '{""YES"":""Yes"", ""NO"":""No""}'
	    where name = 'Lab Duplicate?' and fieldcategoryId in (select id from fieldcategories where name = 'Water Quality with Labs')

");
        }
        
        public override void Down()
        {
            // No undo
        }
    }
}
