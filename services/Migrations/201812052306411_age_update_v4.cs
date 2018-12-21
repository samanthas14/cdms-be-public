namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class age_update_v4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NPT_Age_Detail", "Regenerated", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NPT_Age_Detail", "Regenerated");
        }
    }
}
