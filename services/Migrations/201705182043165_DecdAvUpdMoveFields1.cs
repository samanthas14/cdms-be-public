namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DecdAvUpdMoveFields1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appraisal_Detail", "RegionalOfficeReviewFiles", c => c.String());
            AddColumn("dbo.Appraisal_Detail", "HighestAndBestUse", c => c.String());
            AddColumn("dbo.Appraisal_Detail", "LastAppraisalRequestDate", c => c.DateTime());
            AddColumn("dbo.Appraisal_Header", "OtherPermitLeases", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appraisal_Header", "OtherPermitLeases");
            DropColumn("dbo.Appraisal_Detail", "LastAppraisalRequestDate");
            DropColumn("dbo.Appraisal_Detail", "HighestAndBestUse");
            DropColumn("dbo.Appraisal_Detail", "RegionalOfficeReviewFiles");
        }
    }
}
