namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CrppContractsAddCols : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CrppContracts_Header", "AwardAmount", c => c.Single());
            AddColumn("dbo.CrppContracts_Header", "FinalReportSubmitted", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CrppContracts_Header", "FinalReportSubmitted");
            DropColumn("dbo.CrppContracts_Header", "AwardAmount");
        }
    }
}
