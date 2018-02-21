namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SnorkelFieldDbColumnNameFix : DbMigration
    {
        public override void Up()
        {
            Sql("update DatasetFields set DbColumnName = 'StartWaterTemp' where Id in (1688, 1711 );");
        }
        
        public override void Down()
        {
        }
    }
}
