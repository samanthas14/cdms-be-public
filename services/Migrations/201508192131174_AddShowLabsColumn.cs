namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShowLabsColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ShowLaboratories", c => c.Boolean(nullable: false, defaultValue: false));
            Sql("update Projects set ShowLaboratories = 0");
            Sql("update Projects set ShowLaboratories = 1 where id = 1199");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "ShowLaboratories");
        }
    }
}
