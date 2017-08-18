namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SnrkelFishFirstPass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SnorkelFish_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FishID = c.Int(),
                        ChannelUnit = c.String(),
                        ChannelUnitNumber = c.Int(),
                        Lane = c.Int(),
                        HabitatType = c.String(),
                        FishCount = c.Int(),
                        Species = c.String(),
                        SizeClass = c.Int(),
                        NaturalWoodUsed = c.Boolean(nullable: false),
                        PlacedWoodUsed = c.Boolean(nullable: false),
                        NaturalBoulderUsed = c.Boolean(nullable: false),
                        PlacedBoulderUsed = c.Boolean(nullable: false),
                        NaturalOffChannelUsed = c.Boolean(nullable: false),
                        CreatedOffChannelUsed = c.Boolean(nullable: false),
                        NewSideChannelUsed = c.Boolean(nullable: false),
                        NoStructureUsed = c.Boolean(nullable: false),
                        FieldNotes = c.String(),
                        RowId = c.Int(nullable: false),
                        RowStatusId = c.Int(nullable: false),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        QAStatusId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .ForeignKey("dbo.QAStatus", t => t.QAStatusId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId)
                .Index(t => t.QAStatusId);
            
            CreateTable(
                "dbo.SnorkelFish_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Team = c.String(),
                        WaterTemperature = c.Double(),
                        Visibility = c.String(),
                        VisitId = c.Int(),
                        Comments = c.String(),
                        CollectionType = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SnorkelFish_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.SnorkelFish_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.SnorkelFish_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.SnorkelFish_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.SnorkelFish_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.SnorkelFish_Header", new[] { "ByUserId" });
            DropIndex("dbo.SnorkelFish_Header", new[] { "ActivityId" });
            DropIndex("dbo.SnorkelFish_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.SnorkelFish_Detail", new[] { "ByUserId" });
            DropIndex("dbo.SnorkelFish_Detail", new[] { "ActivityId" });
            DropTable("dbo.SnorkelFish_Header");
            DropTable("dbo.SnorkelFish_Detail");
        }
    }
}
