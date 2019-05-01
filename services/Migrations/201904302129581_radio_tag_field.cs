namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class radio_tag_field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SGS_Carcass_Detail", "RadioTag", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SGS_Carcass_Detail", "RadioTag");
        }
    }
}
