namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HabAddSubpHiTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HabitatItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubprojectId = c.Int(nullable: false),
                        ItemName = c.String(),
                        ItemFiles = c.String(),
                        ExternalLinks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subproject_Hab", t => t.SubprojectId)
                .Index(t => t.SubprojectId);
            
            CreateTable(
                "dbo.Subproject_Hab",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(),
                        ProjectSummary = c.String(),
                        ProjectDescription = c.String(),
                        ProjectStartDate = c.DateTime(),
                        ProjectEndDate = c.DateTime(),
                        FirstFoods = c.String(),
                        RiverVisionTouchstone = c.String(),
                        HabitatObjectives = c.String(),
                        NoaaEcologicalConcernsSubcategories = c.String(),
                        NoaaEcologicalConcerns = c.String(),
                        LimitingFactors = c.String(),
                        Staff = c.String(),
                        Collaborators = c.String(),
                        Comments = c.String(),
                        EffDt = c.DateTime(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        LocationId = c.Int(),
                        OtherCollaborators = c.String(),
                        FeatureImage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HabitatItems", "SubprojectId", "dbo.Subproject_Hab");
            DropIndex("dbo.HabitatItems", new[] { "SubprojectId" });
            DropTable("dbo.Subproject_Hab");
            DropTable("dbo.HabitatItems");
        }
    }
}
