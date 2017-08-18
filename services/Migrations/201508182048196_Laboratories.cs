namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Laboratories : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "Laboratory_Id", "dbo.Laboratories");
            DropIndex("dbo.Projects", new[] { "Laboratory_Id" });
            DropIndex("dbo.Laboratories", new[] { "User_Id" });
            RenameColumn(table: "dbo.Laboratories", name: "User_Id", newName: "UserId");
            CreateTable(
                "dbo.LaboratoryProjects",
                c => new
                    {
                        Laboratory_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Laboratory_Id, t.Project_Id })
                .ForeignKey("dbo.Laboratories", t => t.Laboratory_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.Laboratory_Id)
                .Index(t => t.Project_Id);
            
            AlterColumn("dbo.Laboratories", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Laboratories", "UserId");
            DropColumn("dbo.Projects", "Laboratory_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "Laboratory_Id", c => c.Int());
            DropForeignKey("dbo.LaboratoryProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.LaboratoryProjects", "Laboratory_Id", "dbo.Laboratories");
            DropIndex("dbo.LaboratoryProjects", new[] { "Project_Id" });
            DropIndex("dbo.LaboratoryProjects", new[] { "Laboratory_Id" });
            DropIndex("dbo.Laboratories", new[] { "UserId" });
            AlterColumn("dbo.Laboratories", "UserId", c => c.Int());
            DropTable("dbo.LaboratoryProjects");
            RenameColumn(table: "dbo.Laboratories", name: "UserId", newName: "User_Id");
            CreateIndex("dbo.Laboratories", "User_Id");
            CreateIndex("dbo.Projects", "Laboratory_Id");
            AddForeignKey("dbo.Projects", "Laboratory_Id", "dbo.Laboratories", "Id");
        }
    }
}
