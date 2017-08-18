namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePossibleValuesForFishScales : DbMigration
    {
        public override void Up()
        {
            Sql(@"update fields        set possiblevalues = '{""YES"":""Yes"", ""NO"":""No""}', [Rule] = '{""DefaultValue"":""NO""}' where name  = 'Regeneration';");
            Sql(@"update datasetFields set                                                      [Rule] = '{""DefaultValue"":""NO""}' where label = 'Regeneration';");
        }
        
        public override void Down()
        {
            Sql(@"update fields        set possiblevalues = '[""Yes"", ""No""]', [Rule] = '{""DefaultValue"":""No""}' where name  = 'Regeneration';");
            Sql(@"update datasetFields set                                       [Rule] = '{""DefaultValue"":""No""}' where label = 'Regeneration';");
        }
    }
}
