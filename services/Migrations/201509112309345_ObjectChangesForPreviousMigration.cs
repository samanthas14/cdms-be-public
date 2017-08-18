namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObjectChangesForPreviousMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Electrofishing_Detail", "TagStatus", c => c.String());
            AddColumn("dbo.Electrofishing_Detail", "ClipStatus", c => c.String());
            AddColumn("dbo.Electrofishing_Header", "ReleaseLocation", c => c.String());
            AddColumn("dbo.Electrofishing_Header", "VisitID", c => c.String());
            AddColumn("dbo.Electrofishing_Header", "Unit", c => c.String());
            AddColumn("dbo.FishScales_Detail", "ScaleComments", c => c.String());
            AddColumn("dbo.FishScales_Detail", "BadScale", c => c.String());
            AddColumn("dbo.ScrewTrap_Detail", "TagStatus", c => c.String());
            AddColumn("dbo.ScrewTrap_Detail", "ClipStatus", c => c.String());
            AddColumn("dbo.SnorkelFish_Header", "Unit", c => c.String());
            DropColumn("dbo.Electrofishing_Detail", "ReleaseLocation");
            DropColumn("dbo.Electrofishing_Detail", "Tag");
            DropColumn("dbo.Electrofishing_Detail", "Clip");
            DropColumn("dbo.Electrofishing_Detail", "Disposition");
            DropColumn("dbo.FishScales_Detail", "JuvenileAge");
            DropColumn("dbo.FishScales_Detail", "FishComments");
            DropColumn("dbo.ScrewTrap_Detail", "Tag");
            DropColumn("dbo.ScrewTrap_Detail", "Clip");
            DropColumn("dbo.ScrewTrap_Header", "Task");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ScrewTrap_Header", "Task", c => c.String());
            AddColumn("dbo.ScrewTrap_Detail", "Clip", c => c.String());
            AddColumn("dbo.ScrewTrap_Detail", "Tag", c => c.String());
            AddColumn("dbo.FishScales_Detail", "FishComments", c => c.String());
            AddColumn("dbo.FishScales_Detail", "JuvenileAge", c => c.String());
            AddColumn("dbo.Electrofishing_Detail", "Disposition", c => c.String());
            AddColumn("dbo.Electrofishing_Detail", "Clip", c => c.String());
            AddColumn("dbo.Electrofishing_Detail", "Tag", c => c.String());
            AddColumn("dbo.Electrofishing_Detail", "ReleaseLocation", c => c.String());
            DropColumn("dbo.SnorkelFish_Header", "Unit");
            DropColumn("dbo.ScrewTrap_Detail", "ClipStatus");
            DropColumn("dbo.ScrewTrap_Detail", "TagStatus");
            DropColumn("dbo.FishScales_Detail", "BadScale");
            DropColumn("dbo.FishScales_Detail", "ScaleComments");
            DropColumn("dbo.Electrofishing_Header", "Unit");
            DropColumn("dbo.Electrofishing_Header", "VisitID");
            DropColumn("dbo.Electrofishing_Header", "ReleaseLocation");
            DropColumn("dbo.Electrofishing_Detail", "ClipStatus");
            DropColumn("dbo.Electrofishing_Detail", "TagStatus");
        }
    }
}
