namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveDateandIdFieldsFromHeaderToDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterQuality_Detail", "SampleDate", c => c.DateTime());
            AddColumn("dbo.WaterQuality_Detail", "SampleID", c => c.String());
            DropColumn("dbo.WaterQuality_Header", "SampleDate");
            DropColumn("dbo.WaterQuality_Header", "SampleID");

            Sql(@"
    update datasetFields set fieldroleid = 2 where fieldid = (select id from  fields where name  = 'Sample ID')
    update datasetFields set fieldroleid = 2 where fieldid = (select id from  fields where name  = 'Sample Date')

    update fields        set controltype = 'datetime' where name  = 'Sample Date'
    update datasetFields set controltype = 'datetime' where fieldid = (select id from  fields where name  = 'Sample Date')


    update fields set PossibleValues = '{""COLONIES (MPN)/100 ML"": ""Colonies (MPN)/100 mL"", ""COUNTS (MPN)/100 ML"": ""Counts (MPN)/100 mL"", ""MG/L"": ""mg/L"", ""MG/M3"": ""mg/m3"", ""NTU"": ""NTU"", ""SU"": ""SU"", ""UNITS"": ""Units"", ""ΜG/L"": ""µg/L"", ""ΜMHO/CM (MICRO MHO/CM)"":""µmho/cm (micro mho/cm)""}'
                      where name  = 'Result Units';



");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WaterQuality_Header", "SampleID", c => c.String());
            AddColumn("dbo.WaterQuality_Header", "SampleDate", c => c.DateTime());
            DropColumn("dbo.WaterQuality_Detail", "SampleID");
            DropColumn("dbo.WaterQuality_Detail", "SampleDate");

            Sql(@"
    update datasetFields set fieldroleid = 1 where fieldid = (select id from  fields where name  = 'Sample ID')
    update datasetFields set fieldroleid = 1 where fieldid = (select id from  fields where name  = 'Sample Date')

    update fields        set controltype = 'date' where name  = 'Sample Date'
    update datasetFields set controltype = 'date' where fieldid = (select id from  fields where name  = 'Sample Date')

    update fields set PossibleValues = '[""Colonies (MPN)/100 mL"", ""Counts (MPN)/100 mL"", ""mg/L"", ""mg/m3"", ""NTU"", ""SU"", ""Units"", ""µg/L"", ""µmho/cm (micro mho/cm)""]'
                      where name  = 'Result Units';


");

        }
    }
}
