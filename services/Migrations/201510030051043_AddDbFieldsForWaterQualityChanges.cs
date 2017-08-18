namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDbFieldsForWaterQualityChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterQuality_Detail", "SampleFraction", c => c.String());
            AddColumn("dbo.WaterQuality_Detail", "MethodSpeciation", c => c.String());
            AddColumn("dbo.WaterQuality_Detail", "DetectionLimit", c => c.String());
            AddColumn("dbo.WaterQuality_Detail", "ContextID", c => c.String());
            AddColumn("dbo.WaterQuality_Detail", "MethodID", c => c.String());
            AddColumn("dbo.WaterQuality_Detail", "LabName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterQuality_Detail", "LabName");
            DropColumn("dbo.WaterQuality_Detail", "MethodID");
            DropColumn("dbo.WaterQuality_Detail", "ContextID");
            DropColumn("dbo.WaterQuality_Detail", "DetectionLimit");
            DropColumn("dbo.WaterQuality_Detail", "MethodSpeciation");
            DropColumn("dbo.WaterQuality_Detail", "SampleFraction");
        }
    }
}
