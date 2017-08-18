namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDatatypeToBetterFitIncomingData : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SnorkelFish_Detail", "NaturalWoodUsed", c => c.String());
            AlterColumn("dbo.SnorkelFish_Detail", "PlacedWoodUsed", c => c.String());
            AlterColumn("dbo.SnorkelFish_Detail", "NaturalBoulderUsed", c => c.String());
            AlterColumn("dbo.SnorkelFish_Detail", "PlacedBoulderUsed", c => c.String());
            AlterColumn("dbo.SnorkelFish_Detail", "NaturalOffChannelUsed", c => c.String());
            AlterColumn("dbo.SnorkelFish_Detail", "CreatedOffChannelUsed", c => c.String());
            AlterColumn("dbo.SnorkelFish_Detail", "NewSideChannelUsed", c => c.String());
            AlterColumn("dbo.SnorkelFish_Detail", "NoStructureUsed", c => c.String());
        }
        
        public override void Down()
        {
            Sql(@"
delete from snorkelFish_header
delete from snorkelFish_detail"
                );
            AlterColumn("dbo.SnorkelFish_Detail", "NoStructureUsed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SnorkelFish_Detail", "NewSideChannelUsed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SnorkelFish_Detail", "CreatedOffChannelUsed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SnorkelFish_Detail", "NaturalOffChannelUsed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SnorkelFish_Detail", "PlacedBoulderUsed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SnorkelFish_Detail", "NaturalBoulderUsed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SnorkelFish_Detail", "PlacedWoodUsed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SnorkelFish_Detail", "NaturalWoodUsed", c => c.Boolean(nullable: false));
        }
    }
}
