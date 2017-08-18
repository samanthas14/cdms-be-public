namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWatQualHeaderComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterQuality_Header", "HeaderComments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterQuality_Header", "HeaderComments");
        }
    }
}
