namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreDatabaseTweaks : DbMigration
    {
        public override void Up()
        {
            Sql(@"
    update fields set PossibleValues = '[""Colonies (MPN)/100 mL"", ""Counts (MPN)/100 mL"", ""mg/L"", ""mg/m3"", ""NTU"", ""SU"", ""Units"", ""µg/L"", ""µmho/cm (micro mho/cm)""]',
                      ControlType = 'select' where name  = 'Result Units';

    update DatasetFields set ControlType = 'select' where label  = 'Result Units';

");
        }
        
        public override void Down()
        {
            Sql(@"
    update fields        set PossibleValues = NULL, ControlType = 'text'  where name  = 'Result Units';
    update datasetfields set                        ControlType = 'text'  where label = 'Result Units';
");
        }
    }
}
