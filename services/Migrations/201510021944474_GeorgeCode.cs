namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GeorgeCode : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update [dbo].[Datasets]
set [Config] = '{""DataEntryPage"": {""HiddenFields"": [""Instrument"",""Location""]}}'
where [Name] = 'Fish Scales'
");
        }
        
        public override void Down()
        {
                        Sql(@"
update [dbo].[Datasets]
set [Config] = '{""DataEntryPage"": {""HiddenFields"": [""Instrument""]}}'
where [Name] = 'Fish Scales'
");
           
        }
    }
}
