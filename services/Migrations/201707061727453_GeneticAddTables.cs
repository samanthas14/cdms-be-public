namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GeneticAddTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Genetic_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SampleYear = c.Int(),
                        GeneticId = c.String(),
                        LifeStage = c.String(),
                        JuvenileAge = c.String(),
                        ForkLength = c.Int(),
                        P1_Id = c.String(),
                        P1CollectYear = c.Int(),
                        P1CollectLoc = c.String(),
                        P1Sex = c.String(),
                        P1Origin = c.String(),
                        P2_Id = c.String(),
                        P2CollectYear = c.Int(),
                        P2CollectLoc = c.String(),
                        P2Sex = c.String(),
                        P2Origin = c.String(),
                        GeneticComment = c.String(),
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
                "dbo.Genetic_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderComments = c.String(),
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
            DropForeignKey("dbo.Genetic_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Genetic_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Genetic_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Genetic_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Genetic_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.Genetic_Header", new[] { "ByUserId" });
            DropIndex("dbo.Genetic_Header", new[] { "ActivityId" });
            DropIndex("dbo.Genetic_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Genetic_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Genetic_Detail", new[] { "ActivityId" });
            DropTable("dbo.Genetic_Header");
            DropTable("dbo.Genetic_Detail");
        }
    }
}
