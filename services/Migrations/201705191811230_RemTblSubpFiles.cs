namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemTblSubpFiles : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.SubprojectFiles");
        }
        
        public override void Down()
        {
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
    }
}
