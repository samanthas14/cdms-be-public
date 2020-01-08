namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class release_fix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Release_Data_Header", "SpeciesRun", c => c.String());
            AddColumn("dbo.Release_Data_Header", "LastReleaseDate", c => c.String());
            DropColumn("dbo.Release_Data_Detail", "SpeciesRun");
            DropColumn("dbo.Release_Data_Detail", "FirstReleaseDate");
            DropColumn("dbo.Release_Data_Detail", "LastReleaseDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Release_Data_Detail", "LastReleaseDate", c => c.String());
            AddColumn("dbo.Release_Data_Detail", "FirstReleaseDate", c => c.String());
            AddColumn("dbo.Release_Data_Detail", "SpeciesRun", c => c.String());
            DropColumn("dbo.Release_Data_Header", "LastReleaseDate");
            DropColumn("dbo.Release_Data_Header", "SpeciesRun");
        }
    }
}
