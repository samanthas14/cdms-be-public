namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class FixMissingActivitiesPageInConfigs : DbMigration
    {
        public override void Up()
        {
            var sqlfile = "20180208_updateconfig_activitiespage.sql";
            var filepath = Path.Combine(Configuration.GetMigrationBasePath(), sqlfile);
            SqlFile(filepath);
        }
        
        public override void Down()
        {
        }
    }
}
