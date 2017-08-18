namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSupportingElectroFishingFieldToDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Electrofishing_Detail", "TotalLength", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Electrofishing_Detail", "TotalLength");
        }
    }
}
