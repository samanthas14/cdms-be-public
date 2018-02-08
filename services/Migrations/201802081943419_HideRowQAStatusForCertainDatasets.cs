namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class HideRowQAStatusForCertainDatasets : DbMigration
    {
        public override void Up()
        {
            var sqlfile = "2018020819_HideRowQAStatusForCertainDatasets.sql";
            var filepath = Path.Combine(Configuration.GetMigrationBasePath(), sqlfile);
            SqlFile(filepath);
        }
        
        public override void Down()
        {
        }
    }
}
