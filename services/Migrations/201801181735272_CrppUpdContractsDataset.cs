namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CrppUpdContractsDataset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CrppContracts_Detail", "SurveyAcres", c => c.Double());
            AddColumn("dbo.CrppContracts_Detail", "TestSites", c => c.Int());
            AddColumn("dbo.CrppContracts_Detail", "SHRENum", c => c.String());
            AddColumn("dbo.CrppContracts_Detail", "SHCSNum", c => c.String());
            AddColumn("dbo.CrppContracts_Detail", "HPRCSIT", c => c.String());
            AddColumn("dbo.CrppContracts_Header", "ProjectLead", c => c.String());
            AddColumn("dbo.CrppContracts_Header", "CostCenter", c => c.String());
            AddColumn("dbo.CrppContracts_Header", "ProjectName", c => c.String());
            AddColumn("dbo.CrppContracts_Header", "Client", c => c.String());
            AddColumn("dbo.CrppContracts_Header", "AgreeNumb", c => c.String());
            AddColumn("dbo.CrppContracts_Header", "DateExec", c => c.DateTime());
            AddColumn("dbo.CrppContracts_Header", "DraftDue", c => c.DateTime());
            AddColumn("dbo.CrppContracts_Header", "FinalDue", c => c.DateTime());
            AddColumn("dbo.CrppContracts_Header", "ContractEnd", c => c.DateTime());
            AddColumn("dbo.CrppContracts_Header", "ModExtDate", c => c.DateTime());
            AddColumn("dbo.CrppContracts_Header", "DocumentLink", c => c.String());
            AddColumn("dbo.CrppContracts_Header", "ActivityComments", c => c.String());
            DropColumn("dbo.CrppContracts_Detail", "AcresSurveyed");
            DropColumn("dbo.CrppContracts_Detail", "HprcsitsRecorded");
            DropColumn("dbo.CrppContracts_Detail", "ShpoReportNumber");
            DropColumn("dbo.CrppContracts_Detail", "ShpoCaseNumber");
            DropColumn("dbo.CrppContracts_Header", "CrppPersonnel");
            DropColumn("dbo.CrppContracts_Header", "ActivityTypeId");
            DropColumn("dbo.CrppContracts_Header", "Agency");
            DropColumn("dbo.CrppContracts_Header", "PermitNumber");
            DropColumn("dbo.CrppContracts_Header", "DateReceived");
            DropColumn("dbo.CrppContracts_Header", "DateOfAction");
            DropColumn("dbo.CrppContracts_Header", "ActionTaken");
            DropColumn("dbo.CrppContracts_Header", "ActivityNotes");
            DropColumn("dbo.CrppContracts_Header", "AttachedDocument");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CrppContracts_Header", "AttachedDocument", c => c.String());
            AddColumn("dbo.CrppContracts_Header", "ActivityNotes", c => c.String());
            AddColumn("dbo.CrppContracts_Header", "ActionTaken", c => c.String());
            AddColumn("dbo.CrppContracts_Header", "DateOfAction", c => c.DateTime());
            AddColumn("dbo.CrppContracts_Header", "DateReceived", c => c.DateTime());
            AddColumn("dbo.CrppContracts_Header", "PermitNumber", c => c.String());
            AddColumn("dbo.CrppContracts_Header", "Agency", c => c.String());
            AddColumn("dbo.CrppContracts_Header", "ActivityTypeId", c => c.Int());
            AddColumn("dbo.CrppContracts_Header", "CrppPersonnel", c => c.String());
            AddColumn("dbo.CrppContracts_Detail", "ShpoCaseNumber", c => c.String());
            AddColumn("dbo.CrppContracts_Detail", "ShpoReportNumber", c => c.String());
            AddColumn("dbo.CrppContracts_Detail", "HprcsitsRecorded", c => c.String());
            AddColumn("dbo.CrppContracts_Detail", "AcresSurveyed", c => c.Double());
            DropColumn("dbo.CrppContracts_Header", "ActivityComments");
            DropColumn("dbo.CrppContracts_Header", "DocumentLink");
            DropColumn("dbo.CrppContracts_Header", "ModExtDate");
            DropColumn("dbo.CrppContracts_Header", "ContractEnd");
            DropColumn("dbo.CrppContracts_Header", "FinalDue");
            DropColumn("dbo.CrppContracts_Header", "DraftDue");
            DropColumn("dbo.CrppContracts_Header", "DateExec");
            DropColumn("dbo.CrppContracts_Header", "AgreeNumb");
            DropColumn("dbo.CrppContracts_Header", "Client");
            DropColumn("dbo.CrppContracts_Header", "ProjectName");
            DropColumn("dbo.CrppContracts_Header", "CostCenter");
            DropColumn("dbo.CrppContracts_Header", "ProjectLead");
            DropColumn("dbo.CrppContracts_Detail", "HPRCSIT");
            DropColumn("dbo.CrppContracts_Detail", "SHCSNum");
            DropColumn("dbo.CrppContracts_Detail", "SHRENum");
            DropColumn("dbo.CrppContracts_Detail", "TestSites");
            DropColumn("dbo.CrppContracts_Detail", "SurveyAcres");
        }
    }
}
