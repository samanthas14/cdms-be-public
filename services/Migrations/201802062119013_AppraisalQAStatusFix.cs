namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class AppraisalQAStatusFix : DbMigration
    {
        public override void Up()
        {
            var sqlfile = "20180206_appraisalqastatusfix.sql";
            var filepath = Path.Combine(Configuration.GetMigrationBasePath(), sqlfile);
            SqlFile(filepath);
        }
        
        public override void Down()
        {
        }
    }
}
