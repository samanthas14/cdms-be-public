namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LabCleanOut : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LaboratoryCharacteristics", "LaboratoryId", "dbo.Laboratories");
            DropForeignKey("dbo.LaboratoryCharacteristics", "UserId", "dbo.Users");
            DropForeignKey("dbo.LaboratoryProjects", "Laboratory_Id", "dbo.Laboratories");
            DropForeignKey("dbo.LaboratoryProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Laboratories", "UserId", "dbo.Users");
            DropForeignKey("dbo.Activities", "LaboratoryId", "dbo.Laboratories");
            DropIndex("dbo.Laboratories", new[] { "UserId" });
            DropIndex("dbo.LaboratoryCharacteristics", new[] { "LaboratoryId" });
            DropIndex("dbo.LaboratoryCharacteristics", new[] { "UserId" });
            DropIndex("dbo.Activities", new[] { "LaboratoryId" });
            DropIndex("dbo.LaboratoryProjects", new[] { "Laboratory_Id" });
            DropIndex("dbo.LaboratoryProjects", new[] { "Project_Id" });
            DropColumn("dbo.Projects", "ShowLaboratories");
            DropTable("dbo.Laboratories");
            DropTable("dbo.LaboratoryCharacteristics");
            DropTable("dbo.LaboratoryProjects");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.LaboratoryProjects",
                c => new
                    {
                        Laboratory_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Laboratory_Id, t.Project_Id });
            
            CreateTable(
                "dbo.LaboratoryCharacteristics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LaboratoryId = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        Name = c.String(),
                        MDL = c.String(),
                        Units = c.String(),
                        MethodId = c.String(),
                        Context = c.String(),
                        Description = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Laboratories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreateDateTime = c.DateTime(nullable: false),
                        Name = c.String(),
                        Address = c.String(),
                        DateOfLimit = c.DateTime(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Projects", "ShowLaboratories", c => c.Boolean(nullable: false));
            CreateIndex("dbo.LaboratoryProjects", "Project_Id");
            CreateIndex("dbo.LaboratoryProjects", "Laboratory_Id");
            CreateIndex("dbo.Activities", "LaboratoryId");
            CreateIndex("dbo.LaboratoryCharacteristics", "UserId");
            CreateIndex("dbo.LaboratoryCharacteristics", "LaboratoryId");
            CreateIndex("dbo.Laboratories", "UserId");
            AddForeignKey("dbo.Activities", "LaboratoryId", "dbo.Laboratories", "Id");
            AddForeignKey("dbo.Laboratories", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.LaboratoryProjects", "Project_Id", "dbo.Projects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LaboratoryProjects", "Laboratory_Id", "dbo.Laboratories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LaboratoryCharacteristics", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.LaboratoryCharacteristics", "LaboratoryId", "dbo.Laboratories", "Id");
        }
    }
}
