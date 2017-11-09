namespace services.Migrations
{
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class datasetconfigaddactivitiespage : DbMigration
    {
        public override void Up()
        {
            var sqlfile = "20170929_datasetconfigupdates.sql";
            var filepath = Path.Combine(Configuration.GetMigrationBasePath(), sqlfile);
            SqlFile(filepath);
        }
        
        public override void Down()
        {
            //there is no going back!
        }
    }
}
