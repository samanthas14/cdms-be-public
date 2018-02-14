namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTblCountyAndUpdCrppSp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Counties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SubprojectId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Subproject_Crpp", "ProjectId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subproject_Crpp", "ProjectId");
            DropTable("dbo.Counties");
        }
    }
}
