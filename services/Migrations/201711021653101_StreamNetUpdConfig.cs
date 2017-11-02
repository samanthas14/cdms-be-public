namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StreamNetUpdConfig : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.[Datasets] set Config = '{""DataEntryPage"": {""HiddenFields"": [""Instrument""]}}' where ProjectId = 9999 and Name like 'StreamNet%'
            ");
        }
        
        public override void Down()
        {
            Sql(@"
update dbo.[Datasets] set Config = null where ProjectId = 9999 and Name like 'StreamNet%'
            ");
        }
    }
}
