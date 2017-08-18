namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DecdAvUpdMoveFields3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Appraisal_Detail", "OtherPermitLeases");
            DropColumn("dbo.Appraisal_Header", "LastAppraisalRequestDate");
            DropColumn("dbo.Appraisal_Header", "RegionalOfficeReviewFiles");
            DropColumn("dbo.Appraisal_Header", "HighestAndBestUse");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appraisal_Header", "HighestAndBestUse", c => c.String());
            AddColumn("dbo.Appraisal_Header", "RegionalOfficeReviewFiles", c => c.String());
            AddColumn("dbo.Appraisal_Header", "LastAppraisalRequestDate", c => c.DateTime());
            AddColumn("dbo.Appraisal_Detail", "OtherPermitLeases", c => c.String());
        }
    }
}
