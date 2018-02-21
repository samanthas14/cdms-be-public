namespace services.Migrations
{
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class CrppCleanupFieldsAndDsFields : DbMigration
    {
        public override void Up()
        {
            var sqlfile = "20180118a_CrppCleanupFieldsAndDsFields.sql";
            var filepath = Path.Combine(Configuration.GetMigrationBasePath(), sqlfile);
            SqlFile(filepath);
        }
        
        public override void Down()
        {
        }
    }
}
