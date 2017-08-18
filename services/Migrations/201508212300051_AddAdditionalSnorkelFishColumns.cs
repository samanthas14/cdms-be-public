namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdditionalSnorkelFishColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SnorkelFish_Detail", "NoSnorklers", c => c.String());
            AddColumn("dbo.SnorkelFish_Detail", "Type", c => c.String());
            AddColumn("dbo.SnorkelFish_Detail", "ChannelAverageDepth", c => c.Double());
            AddColumn("dbo.SnorkelFish_Detail", "ChannelLength", c => c.Double());
            AddColumn("dbo.SnorkelFish_Detail", "ChannelWidth", c => c.Double());
            AddColumn("dbo.SnorkelFish_Detail", "ChannelMaxDepth", c => c.Double());
            AddColumn("dbo.SnorkelFish_Detail", "Length", c => c.String());
            AddColumn("dbo.SnorkelFish_Detail", "UnidentifiedSalmonID", c => c.String());
            AddColumn("dbo.SnorkelFish_Detail", "OtherSpeciesPres", c => c.String());
            AddColumn("dbo.SnorkelFish_Detail", "AmbientTemp", c => c.Double());
            AddColumn("dbo.SnorkelFish_Detail", "MinimumTemp", c => c.Double());
            AddColumn("dbo.SnorkelFish_Header", "NoteTaker", c => c.String());
            AddColumn("dbo.SnorkelFish_Header", "StartTime", c => c.String());
            AddColumn("dbo.SnorkelFish_Header", "EndTime", c => c.String());
            AddColumn("dbo.SnorkelFish_Header", "WeatherConditions", c => c.String());
            AddColumn("dbo.SnorkelFish_Header", "DominantSpecies", c => c.Double());
            AddColumn("dbo.SnorkelFish_Header", "CommonSpecies", c => c.Double());
            AddColumn("dbo.SnorkelFish_Header", "RareSpecies", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SnorkelFish_Header", "RareSpecies");
            DropColumn("dbo.SnorkelFish_Header", "CommonSpecies");
            DropColumn("dbo.SnorkelFish_Header", "DominantSpecies");
            DropColumn("dbo.SnorkelFish_Header", "WeatherConditions");
            DropColumn("dbo.SnorkelFish_Header", "EndTime");
            DropColumn("dbo.SnorkelFish_Header", "StartTime");
            DropColumn("dbo.SnorkelFish_Header", "NoteTaker");
            DropColumn("dbo.SnorkelFish_Detail", "MinimumTemp");
            DropColumn("dbo.SnorkelFish_Detail", "AmbientTemp");
            DropColumn("dbo.SnorkelFish_Detail", "OtherSpeciesPres");
            DropColumn("dbo.SnorkelFish_Detail", "UnidentifiedSalmonID");
            DropColumn("dbo.SnorkelFish_Detail", "Length");
            DropColumn("dbo.SnorkelFish_Detail", "ChannelMaxDepth");
            DropColumn("dbo.SnorkelFish_Detail", "ChannelWidth");
            DropColumn("dbo.SnorkelFish_Detail", "ChannelLength");
            DropColumn("dbo.SnorkelFish_Detail", "ChannelAverageDepth");
            DropColumn("dbo.SnorkelFish_Detail", "Type");
            DropColumn("dbo.SnorkelFish_Detail", "NoSnorklers");
        }
    }
}
