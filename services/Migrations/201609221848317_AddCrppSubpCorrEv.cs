namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCrppSubpCorrEv : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CorrespondenceEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubprojectId = c.Int(nullable: false),
                        CorrespondenceDate = c.DateTime(nullable: false),
                        ResponseType = c.String(),
                        NumberOfDays = c.Int(),
                        ResponseDate = c.DateTime(),
                        StaffMember = c.String(),
                        EventFiles = c.String(),
                        EventComments = c.String(),
                        EffDt = c.DateTime(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        CorrespondenceType = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subproject_Crpp", t => t.SubprojectId)
                .Index(t => t.SubprojectId);
            
            CreateTable(
                "dbo.Subproject_Crpp",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(),
                        Agency = c.String(),
                        ProjectProponent = c.String(),
                        TrackingNumber = c.String(),
                        YearDate = c.String(),
                        Closed = c.String(),
                        ProjectLead = c.String(),
                        EffDt = c.DateTime(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        County = c.String(),
                        ProjectDescription = c.String(),
                        UIR = c.String(),
                        OffResTribalFee = c.String(),
                        Comments = c.String(),
                        OtherAgency = c.String(),
                        OtherProjectProponent = c.String(),
                        OtherCounty = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Files", "Subproject_CrppId", c => c.Int());
            CreateIndex("dbo.Files", "Subproject_CrppId");
            AddForeignKey("dbo.Files", "Subproject_CrppId", "dbo.Subproject_Crpp", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "Subproject_CrppId", "dbo.Subproject_Crpp");
            DropForeignKey("dbo.CorrespondenceEvents", "SubprojectId", "dbo.Subproject_Crpp");
            DropIndex("dbo.CorrespondenceEvents", new[] { "SubprojectId" });
            DropIndex("dbo.Files", new[] { "Subproject_CrppId" });
            DropColumn("dbo.Files", "Subproject_CrppId");
            DropTable("dbo.Subproject_Crpp");
            DropTable("dbo.CorrespondenceEvents");
        }
    }
}
