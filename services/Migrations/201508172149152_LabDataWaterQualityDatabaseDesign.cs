namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LabDataWaterQualityDatabaseDesign : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WaterQuality_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CharacteristicName = c.String(),
                        Result = c.Single(),
                        ResultUnits = c.String(),
                        LabDuplicate = c.String(),
                        Comments = c.String(),
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
                "dbo.WaterQuality_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataType = c.String(),
                        SampleDate = c.DateTime(),
                        SampleID = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                        Laboratory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .ForeignKey("dbo.Laboratories", t => t.Laboratory_Id)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId)
                .Index(t => t.Laboratory_Id);
            
            CreateTable(
                "dbo.Laboratories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreateDateTime = c.DateTime(nullable: false),
                        Name = c.String(),
                        Address = c.String(),
                        DateOfLimit = c.DateTime(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.LaboratoryCharacteristics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LaboratoryId = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        Name = c.DateTime(nullable: false),
                        MDL = c.Single(),
                        MethodId = c.Int(nullable: false),
                        Context = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Laboratories", t => t.LaboratoryId)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.LaboratoryId)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Projects", "Laboratory_Id", c => c.Int());
            CreateIndex("dbo.Projects", "Laboratory_Id");
            AddForeignKey("dbo.Projects", "Laboratory_Id", "dbo.Laboratories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WaterQuality_Header", "Laboratory_Id", "dbo.Laboratories");
            DropForeignKey("dbo.Laboratories", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Projects", "Laboratory_Id", "dbo.Laboratories");
            DropForeignKey("dbo.LaboratoryCharacteristics", "User_Id", "dbo.Users");
            DropForeignKey("dbo.LaboratoryCharacteristics", "LaboratoryId", "dbo.Laboratories");
            DropForeignKey("dbo.WaterQuality_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.WaterQuality_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.WaterQuality_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.WaterQuality_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.WaterQuality_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.LaboratoryCharacteristics", new[] { "User_Id" });
            DropIndex("dbo.LaboratoryCharacteristics", new[] { "LaboratoryId" });
            DropIndex("dbo.Laboratories", new[] { "User_Id" });
            DropIndex("dbo.WaterQuality_Header", new[] { "Laboratory_Id" });
            DropIndex("dbo.WaterQuality_Header", new[] { "ByUserId" });
            DropIndex("dbo.WaterQuality_Header", new[] { "ActivityId" });
            DropIndex("dbo.WaterQuality_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.WaterQuality_Detail", new[] { "ByUserId" });
            DropIndex("dbo.WaterQuality_Detail", new[] { "ActivityId" });
            DropIndex("dbo.Projects", new[] { "Laboratory_Id" });
            DropColumn("dbo.Projects", "Laboratory_Id");
            DropTable("dbo.LaboratoryCharacteristics");
            DropTable("dbo.Laboratories");
            DropTable("dbo.WaterQuality_Header");
            DropTable("dbo.WaterQuality_Detail");
        }
    }
}
