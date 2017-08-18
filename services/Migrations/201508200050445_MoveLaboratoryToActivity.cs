namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveLaboratoryToActivity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "LaboratoryId", c => c.Int());
            CreateIndex("dbo.Activities", "LaboratoryId");
            AddForeignKey("dbo.Activities", "LaboratoryId", "dbo.Laboratories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "LaboratoryId", "dbo.Laboratories");
            DropIndex("dbo.Activities", new[] { "LaboratoryId" });
            DropColumn("dbo.Activities", "LaboratoryId");
        }
    }
}
