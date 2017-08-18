namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HideLabsForWaterQuality : DbMigration
    {
        public override void Up()
        {
            Sql(@"
    update datasets set config = '{""DataEntryPage"": {}}' where  name = 'Water Quality with Labs'
    update projects set showlaboratories = 0
");
        }
        
        public override void Down()
        {
            Sql(@"
    update datasets set config = '{""DataEntryPage"": {""ShowFields"": [""Laboratory""]}}' where  name = 'Water Quality with Labs'
    update projects set showlaboratories = 1 where id = 1199
");
        }
    }
}
