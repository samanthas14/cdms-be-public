namespace services.Migrations
{
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class CrppContractsAddData : DbMigration
    {
        public override void Up()
        {
            var sqlfile = "20180126a_CrppAddNewDatasetFields.sql";
            var filepath = Path.Combine(Configuration.GetMigrationBasePath(), sqlfile);
            SqlFile(filepath);
        }
        
        public override void Down()
        {
        }
    }
}
