namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lookuptables : DbMigration
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
                        DatasetId = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Datasets", t => t.DatasetId)
                .Index(t => t.DatasetId);
            
            CreateTable(
                "dbo.LookupTableProjects",
                c => new
                    {
                        LookupTable_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LookupTable_Id, t.Project_Id })
                .ForeignKey("dbo.LookupTables", t => t.LookupTable_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.LookupTable_Id)
                .Index(t => t.Project_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LookupTableProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.LookupTableProjects", "LookupTable_Id", "dbo.LookupTables");
            DropForeignKey("dbo.LookupTables", "DatasetId", "dbo.Datasets");
            DropIndex("dbo.LookupTableProjects", new[] { "Project_Id" });
            DropIndex("dbo.LookupTableProjects", new[] { "LookupTable_Id" });
            DropIndex("dbo.LookupTables", new[] { "DatasetId" });
            DropTable("dbo.LookupTableProjects");
            DropTable("dbo.LookupTables");
        }
    }
}
