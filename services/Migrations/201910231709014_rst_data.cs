namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rst_data : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RST_Data_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BroodYear = c.Int(nullable: false),
                        ConditionalComments = c.String(),
                        EventType = c.String(),
                        Length = c.Single(nullable: false),
                        Lifestage = c.String(),
                        MarkMethod = c.String(),
                        MarkTemperature = c.Single(nullable: false),
                        MigrationYear = c.Int(nullable: false),
                        PITTag = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        ReleaseSite = c.String(),
                        ReleaseTemperature = c.Single(nullable: false),
                        SpeciesRun = c.String(),
                        Origin = c.String(),
                        Tagger = c.String(),
                        TextComments = c.String(),
                        Weight = c.Single(nullable: false),
                        AdditionalPositional = c.String(),
                        NFish = c.Int(nullable: false),
                        Disposition = c.String(),
                        Girth = c.Single(nullable: false),
                        InterDorsal = c.String(),
                        SampleType = c.String(),
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
                "dbo.RST_Data_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MRRProject = c.String(),
                        SessionNote = c.String(),
                        EventSite = c.String(),
                        RSTrpm = c.Single(nullable: false),
                        OperationalCondition = c.String(),
                        StaffGauge = c.Single(nullable: false),
                        Weather = c.String(),
                        WaterClarity = c.String(),
                        SillDepth = c.Single(nullable: false),
                        TrapPosition = c.String(),
                        TrapStartDateTime = c.DateTime(nullable: false),
                        TrapEndDateTime = c.DateTime(nullable: false),
                        HoursSampled = c.Single(nullable: false),
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
            DropForeignKey("dbo.RST_Data_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.RST_Data_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.RST_Data_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.RST_Data_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.RST_Data_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.RST_Data_Header", new[] { "ByUserId" });
            DropIndex("dbo.RST_Data_Header", new[] { "ActivityId" });
            DropIndex("dbo.RST_Data_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.RST_Data_Detail", new[] { "ByUserId" });
            DropIndex("dbo.RST_Data_Detail", new[] { "ActivityId" });
            DropTable("dbo.RST_Data_Header");
            DropTable("dbo.RST_Data_Detail");
        }
    }
}
