namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStreamnetFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StreamNet_NOSA_Detail", "BestValue", c => c.String());
            AddColumn("dbo.StreamNet_NOSA_Detail", "OtherDataSources", c => c.String());
            AddColumn("dbo.StreamNet_SAR_Detail", "BestValue", c => c.String());
            AddColumn("dbo.StreamNet_SAR_Detail", "OtherDataSources", c => c.String());
            AddColumn("dbo.StreamNet_SAR_Detail", "SmoltDef", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "RecruitDef", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "BestValue", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "OtherDataSources", c => c.String());
        }
        
        public override void Down()
        {
            
        }
    }
}
