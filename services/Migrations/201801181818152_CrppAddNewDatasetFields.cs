namespace services.Migrations
{
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class CrppAddNewDatasetFields : DbMigration
    {
        public override void Up()
        {
            var sqlfile = "20180118b_CrppAddNewDatasetFields.sql";
            var filepath = Path.Combine(Configuration.GetMigrationBasePath(), sqlfile);
            SqlFile(filepath);
        }
        
        public override void Down()
        {
        }
    }
}
