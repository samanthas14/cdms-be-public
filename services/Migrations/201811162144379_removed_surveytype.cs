namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removed_surveytype : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SGS_Carcass_Header", "SurveyType");
            DropColumn("dbo.SGS_Redd_Header", "SurveyType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SGS_Redd_Header", "SurveyType", c => c.String());
            AddColumn("dbo.SGS_Carcass_Header", "SurveyType", c => c.String());
        }
    }
}
