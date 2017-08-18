namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HabAddHabFieldsToFiles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "FeatureImage", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "FeatureImage");
        }
    }
}
