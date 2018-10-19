namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFieldCategoryId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Fields", "FieldCategoryId", "dbo.FieldCategories");
            DropIndex("dbo.Fields", new[] { "FieldCategoryId" });
            DropColumn("dbo.Datastores", "FieldCategoryId");
            DropColumn("dbo.Fields", "FieldCategoryId");
            DropTable("dbo.FieldCategories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FieldCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Fields", "FieldCategoryId", c => c.Int(nullable: false));
            AddColumn("dbo.Datastores", "FieldCategoryId", c => c.Int());
            CreateIndex("dbo.Fields", "FieldCategoryId");
            AddForeignKey("dbo.Fields", "FieldCategoryId", "dbo.FieldCategories", "Id");
        }
    }
}
