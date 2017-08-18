namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ElecAddPassNum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Electrofishing_Detail", "PassNumber", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Electrofishing_Detail", "PassNumber");
        }
    }
}
