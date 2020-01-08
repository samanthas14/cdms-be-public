namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nosa_fix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StreamNet_NOSA_Detail", "NOSADefinition", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StreamNet_NOSA_Detail", "NOSADefinition");
        }
    }
}
