namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.StreamNet_Detail", newName: "StreamNet_RperS_Detail");
            RenameTable(name: "dbo.StreamNet_Header", newName: "StreamNet_RperS_Header");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.StreamNet_RperS_Header", newName: "StreamNet_Header");
            RenameTable(name: "dbo.StreamNet_RperS_Detail", newName: "StreamNet_Detail");
        }
    }
}
