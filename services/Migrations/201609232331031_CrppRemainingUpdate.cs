namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CrppRemainingUpdate : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Files", name: "Subproject_CrppId", newName: "Subproject_CrppId");
            RenameIndex(table: "dbo.Files", name: "IX_Subproject_CrppId", newName: "IX_Subproject_CrppId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Files", name: "IX_Subproject_CrppId", newName: "IX_Subproject_Crpp_Id");
            RenameColumn(table: "dbo.Files", name: "Subproject_CrppId", newName: "Subproject_Crpp_Id");
        }
    }
}
