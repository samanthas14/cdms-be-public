namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlreadyRemovedTables : DbMigration
    {
        public override void Up()
        {
            return; //disable this migration since these changes are already present in the database
            DropForeignKey("dbo.CrppContracts_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.CrppContracts_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.CrppContracts_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.CrppContracts_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.CrppContracts_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Appraisal_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Appraisal_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Appraisal_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Appraisal_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Appraisal_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.CorrespondenceEvents", "SubprojectId", "dbo.Subproject_Crpp");
            DropForeignKey("dbo.Files", "Subproject_CrppId", "dbo.Subproject_Crpp");
            DropForeignKey("dbo.HabitatItems", "SubprojectId", "dbo.Subproject_Hab");
            DropIndex("dbo.Files", new[] { "Subproject_CrppId" });
            DropIndex("dbo.CrppContracts_Detail", new[] { "ActivityId" });
            DropIndex("dbo.CrppContracts_Detail", new[] { "ByUserId" });
            DropIndex("dbo.CrppContracts_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.CrppContracts_Header", new[] { "ActivityId" });
            DropIndex("dbo.CrppContracts_Header", new[] { "ByUserId" });
            DropIndex("dbo.Appraisal_Detail", new[] { "ActivityId" });
            DropIndex("dbo.Appraisal_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Appraisal_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Appraisal_Header", new[] { "ActivityId" });
            DropIndex("dbo.Appraisal_Header", new[] { "ByUserId" });
            DropIndex("dbo.CorrespondenceEvents", new[] { "SubprojectId" });
            DropIndex("dbo.HabitatItems", new[] { "SubprojectId" });
            AddColumn("dbo.StreamNet_NOSA_Detail", "BestValue", c => c.String());
            AddColumn("dbo.StreamNet_NOSA_Detail", "OtherDataSources", c => c.String());
            AddColumn("dbo.StreamNet_SAR_Detail", "SmoltDef", c => c.String());
            AddColumn("dbo.StreamNet_SAR_Detail", "BestValue", c => c.String());
            AddColumn("dbo.StreamNet_SAR_Detail", "OtherDataSources", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "RecruitDef", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "BestValue", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "OtherDataSources", c => c.String());
            DropTable("dbo.CrppContracts_Detail");
            DropTable("dbo.CrppContracts_Header");
            DropTable("dbo.Appraisal_Detail");
            DropTable("dbo.Appraisal_Header");
            DropTable("dbo.Subproject_Crpp");
            DropTable("dbo.CorrespondenceEvents");
            DropTable("dbo.Subproject_Hab");
            DropTable("dbo.HabitatItems");
        }
        
        public override void Down()
        {
            return;
            CreateTable(
                "dbo.HabitatItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubprojectId = c.Int(nullable: false),
                        ItemName = c.String(),
                        ItemFiles = c.String(),
                        ExternalLinks = c.String(),
                        ItemType = c.String(),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subproject_Hab",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(),
                        ProjectSummary = c.String(),
                        ProjectDescription = c.String(),
                        ProjectStartDate = c.DateTime(),
                        ProjectEndDate = c.DateTime(),
                        FirstFoods = c.String(),
                        RiverVisionTouchstone = c.String(),
                        HabitatObjectives = c.String(),
                        NoaaEcologicalConcernsSubcategories = c.String(),
                        NoaaEcologicalConcerns = c.String(),
                        LimitingFactors = c.String(),
                        Staff = c.String(),
                        Collaborators = c.String(),
                        Comments = c.String(),
                        ProjectId = c.Int(nullable: false),
                        LocationId = c.Int(),
                        OtherCollaborators = c.String(),
                        FeatureImage = c.String(),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CorrespondenceEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubprojectId = c.Int(nullable: false),
                        CorrespondenceDate = c.DateTime(nullable: false),
                        ResponseType = c.String(),
                        NumberOfDays = c.Int(nullable: false),
                        ResponseDate = c.DateTime(),
                        StaffMember = c.String(),
                        EventFiles = c.String(),
                        EventComments = c.String(),
                        CorrespondenceType = c.String(),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        County = c.String(),
                        ProjectDescription = c.String(),
                        UIR = c.String(),
                        OffResTribalFee = c.String(),
                        Comments = c.String(),
                        OtherAgency = c.String(),
                        OtherProjectProponent = c.String(),
                        OtherCounty = c.String(),
                        ProjectId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Appraisal_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Allotment = c.String(),
                        AllotmentStatus = c.String(),
                        AllotmentName = c.String(),
                        AllotmentDescription = c.String(),
                        AllotmentComments = c.String(),
                        CobellAppraisalWave = c.String(),
                        LeaseTypes = c.String(),
                        MapFiles = c.String(),
                        TSRFiles = c.String(),
                        UpdatedTSRFile = c.String(),
                        FarmingLeaseFiles = c.String(),
                        TimberAppraisalFiles = c.String(),
                        GrazingLeaseFiles = c.String(),
                        AllotmentPhotoFiles = c.String(),
                        HasTimber = c.String(),
                        IsMappable = c.String(),
                        Acres = c.Double(),
                        PriorityType = c.String(),
                        LegalDescription = c.String(),
                        OtherPermitLeases = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Appraisal_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppraisalYear = c.String(),
                        AppraisalFiles = c.String(),
                        AppraisalPhotos = c.String(),
                        AppraisalComments = c.String(),
                        AppraisalStatus = c.String(),
                        AppraisalType = c.String(),
                        AppraisalLogNumber = c.String(),
                        AppraisalValue = c.Decimal(precision: 9, scale: 2),
                        AppraisalValuationDate = c.DateTime(),
                        Appraiser = c.String(),
                        TypeOfTransaction = c.String(),
                        PartiesInvolved = c.String(),
                        AppraisalProjectType = c.String(),
                        RequestNumber = c.String(),
                        NwroComments = c.String(),
                        RegionalOfficeReviewFiles = c.String(),
                        HighestAndBestUse = c.String(),
                        LastAppraisalRequestDate = c.DateTime(),
                        RowId = c.Int(nullable: false),
                        RowStatusId = c.Int(nullable: false),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        QAStatusId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CrppContracts_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectProponent = c.String(),
                        ProjectLead = c.String(),
                        CostCenter = c.String(),
                        ProjectName = c.String(),
                        Client = c.String(),
                        AgreeNumb = c.String(),
                        DateExec = c.DateTime(),
                        DraftDue = c.DateTime(),
                        FinalDue = c.DateTime(),
                        ContractEnd = c.DateTime(),
                        ModExtDate = c.DateTime(),
                        DocumentLink = c.String(),
                        ActivityComments = c.String(),
                        AwardAmount = c.Single(),
                        FinalReportSubmitted = c.DateTime(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CrppContracts_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Testing = c.String(),
                        NewSites = c.Int(),
                        MonitoredSites = c.Int(),
                        SitesEvaluated = c.Int(),
                        UpdatedSites = c.Int(),
                        NewIsolates = c.Int(),
                        Evaluation = c.String(),
                        Monitoring = c.String(),
                        Notes = c.String(),
                        SurveyAcres = c.Double(),
                        TestSites = c.Int(),
                        SHRENum = c.String(),
                        SHCSNum = c.String(),
                        HPRCSIT = c.String(),
                        RowId = c.Int(nullable: false),
                        RowStatusId = c.Int(nullable: false),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        QAStatusId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.StreamNet_RperS_Detail", "OtherDataSources");
            DropColumn("dbo.StreamNet_RperS_Detail", "BestValue");
            DropColumn("dbo.StreamNet_RperS_Detail", "RecruitDef");
            DropColumn("dbo.StreamNet_SAR_Detail", "OtherDataSources");
            DropColumn("dbo.StreamNet_SAR_Detail", "BestValue");
            DropColumn("dbo.StreamNet_SAR_Detail", "SmoltDef");
            DropColumn("dbo.StreamNet_NOSA_Detail", "OtherDataSources");
            DropColumn("dbo.StreamNet_NOSA_Detail", "BestValue");
            CreateIndex("dbo.HabitatItems", "SubprojectId");
            CreateIndex("dbo.CorrespondenceEvents", "SubprojectId");
            CreateIndex("dbo.Appraisal_Header", "ByUserId");
            CreateIndex("dbo.Appraisal_Header", "ActivityId");
            CreateIndex("dbo.Appraisal_Detail", "QAStatusId");
            CreateIndex("dbo.Appraisal_Detail", "ByUserId");
            CreateIndex("dbo.Appraisal_Detail", "ActivityId");
            CreateIndex("dbo.CrppContracts_Header", "ByUserId");
            CreateIndex("dbo.CrppContracts_Header", "ActivityId");
            CreateIndex("dbo.CrppContracts_Detail", "QAStatusId");
            CreateIndex("dbo.CrppContracts_Detail", "ByUserId");
            CreateIndex("dbo.CrppContracts_Detail", "ActivityId");
            CreateIndex("dbo.Files", "Subproject_CrppId");
            AddForeignKey("dbo.HabitatItems", "SubprojectId", "dbo.Subproject_Hab", "Id");
            AddForeignKey("dbo.Files", "Subproject_CrppId", "dbo.Subproject_Crpp", "Id");
            AddForeignKey("dbo.CorrespondenceEvents", "SubprojectId", "dbo.Subproject_Crpp", "Id");
            AddForeignKey("dbo.Appraisal_Header", "ByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Appraisal_Header", "ActivityId", "dbo.Activities", "Id");
            AddForeignKey("dbo.Appraisal_Detail", "QAStatusId", "dbo.QAStatus", "Id");
            AddForeignKey("dbo.Appraisal_Detail", "ByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Appraisal_Detail", "ActivityId", "dbo.Activities", "Id");
            AddForeignKey("dbo.CrppContracts_Header", "ByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.CrppContracts_Header", "ActivityId", "dbo.Activities", "Id");
            AddForeignKey("dbo.CrppContracts_Detail", "QAStatusId", "dbo.QAStatus", "Id");
            AddForeignKey("dbo.CrppContracts_Detail", "ByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.CrppContracts_Detail", "ActivityId", "dbo.Activities", "Id");
        }
    }
}
