namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class project_config : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LookupTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Label = c.String(),
                        Description = c.String(),
                        DatasetId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Datasets", t => t.DatasetId)
                .Index(t => t.DatasetId);
            
            AddColumn("dbo.Projects", "Config", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LookupTables", "DatasetId", "dbo.Datasets");
            DropIndex("dbo.LookupTables", new[] { "DatasetId" });
            DropColumn("dbo.Projects", "Config");
            DropTable("dbo.LookupTables");
        }
    }
}
