namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changed_name : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.StreamNet_JuvOutmigrationDetail_Detail", newName: "StreamNet_JuvOutmigrantsDetail_Detail");
            RenameTable(name: "dbo.StreamNet_JuvOutmigrationDetail_Header", newName: "StreamNet_JuvOutmigrantsDetail_Header");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.StreamNet_JuvOutmigrantsDetail_Header", newName: "StreamNet_JuvOutmigrationDetail_Header");
            RenameTable(name: "dbo.StreamNet_JuvOutmigrantsDetail_Detail", newName: "StreamNet_JuvOutmigrationDetail_Detail");
        }
    }
}
