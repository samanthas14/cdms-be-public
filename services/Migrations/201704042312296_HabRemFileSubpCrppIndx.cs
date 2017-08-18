namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HabRemFileSubpCrppIndx : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "Subproject_CrppId", "dbo.Subproject_Crpp");
            DropIndex("dbo.Files", new[] { "Subproject_CrppId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Files", "Subproject_CrppId");
            AddForeignKey("dbo.Files", "Subproject_CrppId", "dbo.Subproject_Crpp", "Id");
        }
    }
}
