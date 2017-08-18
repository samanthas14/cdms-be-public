namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUnusedElectrofishingField : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Electrofishing_Detail", "PassNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Electrofishing_Detail", "PassNumber", c => c.String());
        }
    }
}
