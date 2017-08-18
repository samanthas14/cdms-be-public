namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileStoreAddDatasetIdToFiles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "DatasetId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "DatasetId");
        }
    }
}
