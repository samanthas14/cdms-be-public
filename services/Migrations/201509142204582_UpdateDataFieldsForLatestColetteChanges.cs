namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataFieldsForLatestColetteChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Electrofishing_Header", "StartTime", c => c.DateTime());
            AddColumn("dbo.Electrofishing_Header", "EndTime", c => c.DateTime());
            AddColumn("dbo.Electrofishing_Header", "ReleaseTime", c => c.DateTime());
            AddColumn("dbo.Electrofishing_Header", "StartTemp", c => c.Double());
            DropColumn("dbo.Electrofishing_Header", "TagDateTime");
            DropColumn("dbo.Electrofishing_Header", "ReleaseDateTime");
            DropColumn("dbo.Electrofishing_Header", "Tagger");
            DropColumn("dbo.Electrofishing_Header", "CaptureMethod");
            DropColumn("dbo.Electrofishing_Header", "MigratoryYear");
            DropColumn("dbo.Electrofishing_Header", "TaggingTemp");
            DropColumn("dbo.Electrofishing_Header", "TaggingMethod");
            DropColumn("dbo.Electrofishing_Header", "Organization");
            DropColumn("dbo.Electrofishing_Header", "CoordinatorID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Electrofishing_Header", "CoordinatorID", c => c.String());
            AddColumn("dbo.Electrofishing_Header", "Organization", c => c.String());
            AddColumn("dbo.Electrofishing_Header", "TaggingMethod", c => c.String());
            AddColumn("dbo.Electrofishing_Header", "TaggingTemp", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "MigratoryYear", c => c.Int());
            AddColumn("dbo.Electrofishing_Header", "CaptureMethod", c => c.String());
            AddColumn("dbo.Electrofishing_Header", "Tagger", c => c.String());
            AddColumn("dbo.Electrofishing_Header", "ReleaseDateTime", c => c.DateTime());
            AddColumn("dbo.Electrofishing_Header", "TagDateTime", c => c.DateTime());
            DropColumn("dbo.Electrofishing_Header", "StartTemp");
            DropColumn("dbo.Electrofishing_Header", "ReleaseTime");
            DropColumn("dbo.Electrofishing_Header", "EndTime");
            DropColumn("dbo.Electrofishing_Header", "StartTime");
        }
    }
}
