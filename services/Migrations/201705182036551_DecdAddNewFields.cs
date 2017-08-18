namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DecdAddNewFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appraisal_Detail", "RequestNumber", c => c.String());
            AddColumn("dbo.Appraisal_Detail", "OtherPermitLeases", c => c.String());
            AddColumn("dbo.Appraisal_Detail", "NwroComments", c => c.String());
            AddColumn("dbo.Appraisal_Header", "HighestAndBestUse", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appraisal_Header", "HighestAndBestUse");
            DropColumn("dbo.Appraisal_Detail", "NwroComments");
            DropColumn("dbo.Appraisal_Detail", "OtherPermitLeases");
            DropColumn("dbo.Appraisal_Detail", "RequestNumber");
        }
    }
}
