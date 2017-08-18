namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.LaboratoryCharacteristics", new[] { "User_Id" });
            RenameColumn(table: "dbo.LaboratoryCharacteristics", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.LaboratoryCharacteristics", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.LaboratoryCharacteristics", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.LaboratoryCharacteristics", new[] { "UserId" });
            AlterColumn("dbo.LaboratoryCharacteristics", "UserId", c => c.Int());
            RenameColumn(table: "dbo.LaboratoryCharacteristics", name: "UserId", newName: "User_Id");
            CreateIndex("dbo.LaboratoryCharacteristics", "User_Id");
        }
    }
}
