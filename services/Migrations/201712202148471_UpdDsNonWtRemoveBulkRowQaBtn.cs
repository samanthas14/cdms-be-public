namespace services.Migrations
{
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class UpdDsNonWtRemoveBulkRowQaBtn : DbMigration
    {
        public override void Up()
        {
            var sqlfile = "20171220_UpdDsNonWtRemoveBulkRowQaBtn.sql";
            var filepath = Path.Combine(Configuration.GetMigrationBasePath(), sqlfile);
            SqlFile(filepath);
        }
        
        public override void Down()
        {
        }
    }
}
