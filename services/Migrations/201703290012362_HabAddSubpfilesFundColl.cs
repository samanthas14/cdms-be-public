namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HabAddSubpfilesFundColl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Collaborators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SubprojectId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fundings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Amount = c.Double(),
                        SubprojectId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubprojectFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        SubprojectId = c.Int(nullable: false),
                        FileId = c.Int(nullable: false),
                        FileName = c.String(),
                        FeatureImage = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SubprojectFiles");
            DropTable("dbo.Fundings");
            DropTable("dbo.Collaborators");
        }
    }
}
