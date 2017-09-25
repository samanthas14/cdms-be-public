namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            /*CreateTable(
                "dbo.InstrumentAccuracyChecks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstrumentId = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        CheckDate = c.DateTime(nullable: false),
                        CheckMethod = c.Int(nullable: false),
                        Bath1Grade = c.String(),
                        Bath2Grade = c.String(),
                        Comments = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Instruments", t => t.InstrumentId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.InstrumentId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Instruments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        SerialNumber = c.String(),
                        Manufacturer = c.String(),
                        Model = c.String(),
                        PurchasingProgramProjectId = c.Int(),
                        OwningDepartmentId = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        PurchaseDate = c.DateTime(),
                        EnteredService = c.DateTime(),
                        EndedService = c.DateTime(),
                        InstrumentTypeId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InstrumentTypes", t => t.InstrumentTypeId)
                .ForeignKey("dbo.Departments", t => t.OwningDepartmentId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.OwningDepartmentId)
                .Index(t => t.InstrumentTypeId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.InstrumentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrganizationId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectTypeId = c.Int(nullable: false),
                        OrganizationId = c.Int(),
                        OwnerId = c.Int(),
                        Name = c.String(),
                        Description = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId)
                .ForeignKey("dbo.Users", t => t.OwnerId)
                .ForeignKey("dbo.ProjectTypes", t => t.ProjectTypeId)
                .Index(t => t.ProjectTypeId)
                .Index(t => t.OrganizationId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrganizationId = c.Int(),
                        DepartmentId = c.Int(),
                        GUID = c.String(),
                        Fullname = c.String(),
                        Roles = c.String(),
                        ProfileImageUrl = c.String(),
                        Username = c.String(),
                        Description = c.String(),
                        LastLogin = c.DateTime(nullable: false),
                        Inactive = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId)
                .Index(t => t.OrganizationId)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.UserPreferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        UploadDate = c.DateTime(nullable: false),
                        Size = c.String(),
                        Link = c.String(),
                        FileTypeId = c.Int(nullable: false),
                        Subproject_CrppId = c.Int(),
                        FeatureImage = c.Int(),
                        DatasetId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileTypes", t => t.FileTypeId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Subproject_Crpp", t => t.Subproject_CrppId)
                .Index(t => t.ProjectId)
                .Index(t => t.UserId)
                .Index(t => t.FileTypeId)
                .Index(t => t.Subproject_CrppId);
            
            CreateTable(
                "dbo.FileTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Extensions = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fishermen",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        Aka = c.String(),
                        LastName = c.String(),
                        PhoneNumber = c.String(),
                        DateAdded = c.DateTime(nullable: false),
                        DateInactive = c.DateTime(),
                        FullName = c.String(),
                        FishermanComments = c.String(),
                        StatusId = c.Int(nullable: false),
                        OkToCallId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Label = c.String(),
                        Description = c.String(),
                        LocationTypeId = c.Int(nullable: false),
                        WaterBodyId = c.Int(),
                        SdeFeatureClassId = c.Int(),
                        SdeObjectId = c.Int(),
                        CreateDateTime = c.DateTime(nullable: false),
                        UserId = c.Int(),
                        Elevation = c.Int(),
                        Status = c.Int(nullable: false),
                        GPSEasting = c.Decimal(precision: 18, scale: 8),
                        GPSNorthing = c.Decimal(precision: 18, scale: 8),
                        Projection = c.String(),
                        UTMZone = c.String(),
                        Latitude = c.Decimal(precision: 18, scale: 13),
                        Longitude = c.Decimal(precision: 18, scale: 13),
                        OtherAgencyId = c.String(),
                        ImageLink = c.String(),
                        WettedWidth = c.Single(),
                        WettedDepth = c.Single(),
                        RiverMile = c.Decimal(precision: 5, scale: 2),
                        StudyDesign = c.String(),
                        ProjectId = c.Int(),
                        SubprojectId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LocationTypes", t => t.LocationTypeId)
                .ForeignKey("dbo.SdeFeatureClasses", t => t.SdeFeatureClassId)
                .ForeignKey("dbo.WaterBodies", t => t.WaterBodyId)
                .Index(t => t.LocationTypeId)
                .Index(t => t.WaterBodyId)
                .Index(t => t.SdeFeatureClassId);
            
            CreateTable(
                "dbo.LocationTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SdeFeatureClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Database = c.String(),
                        Service = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WaterBodies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        GNIS_ID = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        DatasetId = c.Int(nullable: false),
                        SourceId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ActivityTypeId = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        ActivityDate = c.DateTime(nullable: false),
                        InstrumentId = c.Int(),
                        LaboratoryId = c.Int(),
                        AccuracyCheckId = c.Int(),
                        PostAccuracyCheckId = c.Int(),
                        Timezone = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InstrumentAccuracyChecks", t => t.AccuracyCheckId)
                .ForeignKey("dbo.ActivityTypes", t => t.ActivityTypeId)
                .ForeignKey("dbo.Datasets", t => t.DatasetId)
                .ForeignKey("dbo.Instruments", t => t.InstrumentId)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.InstrumentAccuracyChecks", t => t.PostAccuracyCheckId)
                .ForeignKey("dbo.Sources", t => t.SourceId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.DatasetId)
                .Index(t => t.SourceId)
                .Index(t => t.LocationId)
                .Index(t => t.UserId)
                .Index(t => t.ActivityTypeId)
                .Index(t => t.InstrumentId)
                .Index(t => t.AccuracyCheckId)
                .Index(t => t.PostAccuracyCheckId);
            
            CreateTable(
                "dbo.ActivityTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Datasets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        DefaultRowQAStatusId = c.Int(nullable: false),
                        DefaultActivityQAStatusId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        DatastoreId = c.Int(),
                        Config = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Datastores", t => t.DatastoreId)
                .ForeignKey("dbo.QAStatus", t => t.DefaultRowQAStatusId)
                .Index(t => t.DefaultRowQAStatusId)
                .Index(t => t.DatastoreId);
            
            CreateTable(
                "dbo.Datastores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        TablePrefix = c.String(),
                        DatastoreDatasetId = c.String(),
                        OwnerUserId = c.Int(nullable: false),
                        FieldCategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QAStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DatasetFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DatasetId = c.Int(nullable: false),
                        FieldId = c.Int(nullable: false),
                        FieldRoleId = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        Label = c.String(),
                        DbColumnName = c.String(),
                        ControlType = c.String(),
                        Validation = c.String(),
                        Rule = c.String(),
                        SourceId = c.Int(),
                        InstrumentId = c.Int(),
                        OrderIndex = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fields", t => t.FieldId)
                .ForeignKey("dbo.FieldRoles", t => t.FieldRoleId)
                .ForeignKey("dbo.Instruments", t => t.InstrumentId)
                .ForeignKey("dbo.Sources", t => t.SourceId)
                .ForeignKey("dbo.Datasets", t => t.DatasetId)
                .Index(t => t.DatasetId)
                .Index(t => t.FieldId)
                .Index(t => t.FieldRoleId)
                .Index(t => t.SourceId)
                .Index(t => t.InstrumentId);
            
            CreateTable(
                "dbo.Fields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldCategoryId = c.Int(nullable: false),
                        TechnicalName = c.String(),
                        DbColumnName = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        Units = c.String(),
                        DataSource = c.String(),
                        PossibleValues = c.String(),
                        Validation = c.String(),
                        Rule = c.String(),
                        DataType = c.String(),
                        ControlType = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FieldCategories", t => t.FieldCategoryId)
                .Index(t => t.FieldCategoryId);
            
            CreateTable(
                "dbo.FieldCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FieldRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ActivityQAs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityId = c.Int(nullable: false),
                        QAStatusId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                        Comments = c.String(),
                        QAStatusName = c.String(),
                        QAStatusDescription = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QAStatus", t => t.QAStatusId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.QAStatusId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AuditJournals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Action = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        Result = c.String(),
                        Module = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Collaborators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SubprojectId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fundings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Amount = c.Double(),
                        SubprojectId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MetadataEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MetadataProperties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MetadataEntityId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        DataType = c.String(),
                        ControlType = c.String(),
                        PossibleValues = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MetadataEntities", t => t.MetadataEntityId)
                .Index(t => t.MetadataEntityId);
            
            CreateTable(
                "dbo.MetadataValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MetadataPropertyId = c.Int(nullable: false),
                        RelationId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                        Values = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Benthic_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MetricTaxaRichness = c.Decimal(precision: 18, scale: 2),
                        MetricHilsenhoffBioticIndex = c.Decimal(precision: 18, scale: 2),
                        MetricChironomidae = c.Decimal(precision: 18, scale: 2),
                        MetricColeoptera = c.Decimal(precision: 18, scale: 2),
                        MetricDiptera = c.Decimal(precision: 18, scale: 2),
                        MetricEphemeroptera = c.Decimal(precision: 18, scale: 2),
                        MetricLepidoptera = c.Decimal(precision: 18, scale: 2),
                        MetricMegaloptera = c.Decimal(precision: 18, scale: 2),
                        MetricOdonata = c.Decimal(precision: 18, scale: 2),
                        MetricOligochaeta = c.Decimal(precision: 18, scale: 2),
                        MetricOtherNonInsect = c.Decimal(precision: 18, scale: 2),
                        MetricPlecoptera = c.Decimal(precision: 18, scale: 2),
                        MetricTrichoptera = c.Decimal(precision: 18, scale: 2),
                        MvTaxaRichness = c.Int(),
                        MvERichness = c.Int(),
                        MvPRichness = c.Int(),
                        MvTRichness = c.Int(),
                        MvPollutionSensitiveRichness = c.Int(),
                        MvClingerRichness = c.Int(),
                        MvSemivoltineRichness = c.Int(),
                        MvPollutionTolerantPercent = c.Decimal(precision: 18, scale: 2),
                        MvPredatorPercent = c.Decimal(precision: 18, scale: 2),
                        MvDominantTaxa3Percent = c.Decimal(precision: 18, scale: 2),
                        MsTaxaRichness = c.Int(),
                        MsERichness = c.Int(),
                        MsPRichness = c.Int(),
                        MsTRichness = c.Int(),
                        MsPollutionSensitiveRichness = c.Int(),
                        MsClingerRichness = c.Int(),
                        MsSemivoltineRichness = c.Int(),
                        MsPollutionTolerantPercent = c.Decimal(precision: 18, scale: 2),
                        MsPredatorPercent = c.Decimal(precision: 18, scale: 2),
                        MsDominantTaxa3Percent = c.Decimal(precision: 18, scale: 2),
                        BIbiScore = c.Int(),
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
                "dbo.Benthic_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SampleYear = c.Int(),
                        PrePost = c.String(),
                        VisitId = c.Int(),
                        SampleId = c.String(),
                        SampleClientId = c.String(),
                        TareMass = c.Decimal(precision: 18, scale: 2),
                        DryMass = c.Decimal(precision: 18, scale: 2),
                        DryMassFinal = c.Decimal(precision: 18, scale: 2),
                        FieldComments = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.BSample_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sex = c.String(),
                        Mark = c.String(),
                        ForkLength = c.Int(),
                        TotalLength = c.Int(),
                        Weight = c.Int(),
                        ScaleId = c.String(),
                        GeneticSampleId = c.String(),
                        SnoutId = c.String(),
                        LifeStage = c.String(),
                        Origin = c.String(),
                        Species = c.String(),
                        PITTagId = c.String(),
                        Tag = c.String(),
                        RadioTagId = c.String(),
                        FishComments = c.String(),
                        OtherTagId = c.String(),
                        KidneyId = c.String(),
                        PercentRetained = c.String(),
                        FinClip = c.String(),
                        TotalCount = c.Int(),
                        RecordNumber = c.String(),
                        MEHPLength = c.Int(),
                        SubSample = c.String(),
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
                "dbo.BSample_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SampleYear = c.Int(),
                        Technicians = c.String(),
                        HeaderComments = c.String(),
                        CollectionType = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.CrppContracts_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AcresSurveyed = c.Double(),
                        Testing = c.String(),
                        NewSites = c.Int(),
                        MonitoredSites = c.Int(),
                        SitesEvaluated = c.Int(),
                        UpdatedSites = c.Int(),
                        NewIsolates = c.Int(),
                        Evaluation = c.String(),
                        HprcsitsRecorded = c.String(),
                        Monitoring = c.String(),
                        Notes = c.String(),
                        ShpoReportNumber = c.String(),
                        ShpoCaseNumber = c.String(),
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
                "dbo.CrppContracts_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CrppPersonnel = c.String(),
                        ActivityTypeId = c.Int(),
                        Agency = c.String(),
                        ProjectProponent = c.String(),
                        PermitNumber = c.String(),
                        DateReceived = c.DateTime(),
                        DateOfAction = c.DateTime(),
                        ActionTaken = c.String(),
                        ActivityNotes = c.String(),
                        AttachedDocument = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.Drift_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SpeciesGroup = c.String(),
                        Taxon = c.String(),
                        LifeStage = c.String(),
                        SizeClass = c.String(),
                        TaxonCount = c.Int(),
                        Qualifier = c.String(),
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
                "dbo.Drift_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SampleYear = c.Int(),
                        PrePost = c.String(),
                        VisitId = c.Int(),
                        SampleId = c.String(),
                        SampleClientId = c.String(),
                        TotalJars = c.Int(),
                        AquaticTareMass = c.Single(),
                        AquaticTareDryMass = c.Single(),
                        AquaticDryMassFinal = c.Single(),
                        ATTareMass = c.Single(),
                        ATTareDryMass = c.Single(),
                        ATDryMassFinal = c.Single(),
                        TerrTareMass = c.Single(),
                        TerrTareDryMass = c.Single(),
                        TerrDryMassFinal = c.Single(),
                        FieldComments = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.FishScales_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldScaleID = c.String(),
                        GumCardScaleID = c.String(),
                        ScaleCollectionDate = c.DateTime(),
                        Species = c.String(),
                        LifeStage = c.String(),
                        Circuli = c.Double(),
                        FreshwaterAge = c.Double(),
                        SaltWaterAge = c.Double(),
                        TotalAdultAge = c.Double(),
                        SpawnCheck = c.String(),
                        Regeneration = c.String(),
                        Stock = c.String(),
                        ScaleComments = c.String(),
                        BadScale = c.String(),
                        TotalAge = c.Int(),
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
                "dbo.FishScales_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RunYear = c.Int(),
                        Technician = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.AdultWeir_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FishNumber = c.String(),
                        Sex = c.String(),
                        Mark = c.String(),
                        ForkLength = c.Int(),
                        TotalLength = c.Int(),
                        Weight = c.Int(),
                        ScaleId = c.String(),
                        GeneticSampleId = c.String(),
                        SnoutId = c.String(),
                        Disposition = c.String(),
                        Recapture = c.String(),
                        LifeStage = c.String(),
                        Origin = c.String(),
                        Species = c.String(),
                        PITTagId = c.String(),
                        TransportTankUnit = c.String(),
                        ReleaseSite = c.String(),
                        Ripeness = c.String(),
                        Tag = c.String(),
                        RadioTagId = c.String(),
                        Solution = c.String(),
                        SolutionDosage = c.Int(),
                        TotalFishRepresented = c.Int(),
                        FishComments = c.String(),
                        OtolithNumber = c.String(),
                        RunYear = c.String(),
                        OtherTagId = c.String(),
                        PercentSpawned = c.String(),
                        OtolithGenetics = c.String(),
                        FinClip = c.String(),
                        Girth = c.String(),
                        TrapLocation = c.String(),
                        PassageLocation = c.String(),
                        PassageTime = c.DateTime(),
                        AgePITTag = c.Int(),
                        AgeCWT = c.Int(),
                        AgeScale = c.Int(),
                        AgeLength = c.Int(),
                        BroodProgram = c.String(),
                        TransportFrom = c.String(),
                        HatcheryType = c.String(),
                        Stock = c.String(),
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
                "dbo.AdultWeir_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AirTemperature = c.Double(),
                        AirTemperatureF = c.Double(),
                        WaterTemperature = c.Double(),
                        WaterTemperatureF = c.Double(),
                        TimeStart = c.String(),
                        TimeEnd = c.String(),
                        Technicians = c.String(),
                        WaterFlow = c.Double(),
                        Comments = c.String(),
                        CollectionType = c.String(),
                        FieldSheetFile = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.Genetic_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SampleYear = c.Int(),
                        GeneticId = c.String(),
                        LifeStage = c.String(),
                        JuvenileAge = c.String(),
                        ForkLength = c.Int(),
                        P1_Id = c.String(),
                        P1CollectYear = c.Int(),
                        P1CollectLoc = c.String(),
                        P1Sex = c.String(),
                        P1Origin = c.String(),
                        P2_Id = c.String(),
                        P2CollectYear = c.Int(),
                        P2CollectLoc = c.String(),
                        P2Sex = c.String(),
                        P2Origin = c.String(),
                        GeneticComment = c.String(),
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
                "dbo.Genetic_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeaderComments = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.JvRearing_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Action = c.String(),
                        Species = c.String(),
                        PointData = c.String(),
                        Result = c.Single(),
                        ResultUnit = c.String(),
                        ActionComments = c.String(),
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
                "dbo.JvRearing_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AcclimationYear = c.Int(),
                        HeaderComments = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.Metrics_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkElementName = c.String(),
                        Measure = c.String(),
                        PlannedValue = c.Double(),
                        ActualValue = c.Double(),
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
                "dbo.Metrics_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        YearReported = c.Int(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.ScrewTrap_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sequence = c.Int(),
                        PitTagCode = c.String(),
                        SpeciesRunRearing = c.String(),
                        ForkLength = c.Double(),
                        Weight = c.Double(),
                        OtherSpecies = c.String(),
                        FishCount = c.Int(),
                        ConditionalComment = c.String(),
                        TextualComments = c.String(),
                        Note = c.String(),
                        ReleaseLocation = c.String(),
                        TagStatus = c.String(),
                        ClipStatus = c.String(),
                        FishComments = c.String(),
                        AdditionalPositionalComments = c.String(),
                        EventType = c.String(),
                        SecondPitTag = c.String(),
                        RacewayTransectTank = c.String(),
                        LifeStage = c.String(),
                        GeneticId = c.String(),
                        CodedWireTag = c.String(),
                        BroodYear = c.Int(),
                        MigrationYear = c.Int(),
                        SizeOfCount = c.String(),
                        ScaleId = c.String(),
                        Containment = c.String(),
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
                "dbo.ScrewTrap_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileTitle = c.String(),
                        ClipFiles = c.String(),
                        Tagger = c.String(),
                        LivewellTemp = c.Double(),
                        TaggingTemp = c.Double(),
                        PostTaggingTemp = c.Double(),
                        ReleaseTemp = c.Double(),
                        ArrivalTime = c.String(),
                        DepartTime = c.String(),
                        ArrivalRPMs = c.Double(),
                        DepartureRPMs = c.Double(),
                        Hubometer = c.Double(),
                        HubometerTime = c.String(),
                        TrapStopped = c.String(),
                        TrapStarted = c.String(),
                        FishCollected = c.String(),
                        FishReleased = c.String(),
                        DailyFinClips = c.String(),
                        Flow = c.String(),
                        Turbidity = c.String(),
                        TrapDebris = c.String(),
                        RiverDebris = c.String(),
                        ActivityComments = c.String(),
                        Unit = c.String(),
                        Crew = c.String(),
                        TrapStatus = c.String(),
                        Weather = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.SnorkelFish_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NoSnorklers = c.String(),
                        FishID = c.Int(),
                        ChannelUnitNumber = c.Int(),
                        ChannelUnitType = c.String(),
                        Lane = c.Int(),
                        Type = c.String(),
                        ChannelAverageDepth = c.Double(),
                        ChannelLength = c.Double(),
                        ChannelWidth = c.Double(),
                        ChannelMaxDepth = c.Double(),
                        FishCount = c.Int(),
                        Species = c.String(),
                        SizeClass = c.String(),
                        UnidentifiedSalmonID = c.String(),
                        OtherSpeciesPres = c.String(),
                        NaturalWoodUsed = c.String(),
                        PlacedWoodUsed = c.String(),
                        NaturalBoulderUsed = c.String(),
                        PlacedBoulderUsed = c.String(),
                        NaturalOffChannelUsed = c.String(),
                        CreatedOffChannelUsed = c.String(),
                        NewSideChannelUsed = c.String(),
                        NoStructureUsed = c.String(),
                        AmbientTemp = c.Double(),
                        MinimumTemp = c.Double(),
                        FieldNotes = c.String(),
                        AEMHabitatType = c.String(),
                        AEMLength = c.Int(),
                        Unit = c.String(),
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
                "dbo.Electrofishing_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sequence = c.Int(),
                        PitTagCode = c.String(),
                        SpeciesRunRearing = c.String(),
                        ForkLength = c.Double(),
                        Weight = c.Double(),
                        TotalLength = c.Double(),
                        OtherSpecies = c.String(),
                        FishCount = c.Int(),
                        SizeCategory = c.String(),
                        ConditionalComment = c.String(),
                        TextualComments = c.String(),
                        Note = c.String(),
                        TagStatus = c.String(),
                        ClipStatus = c.String(),
                        OtolithID = c.String(),
                        GeneticID = c.String(),
                        OtherID = c.String(),
                        AdditionalPositionalComment = c.String(),
                        ChannelUnitType = c.String(),
                        EventTypeD = c.String(),
                        SecondPitTag = c.String(),
                        RacewayTransectTank = c.String(),
                        LifeStage = c.String(),
                        CodedWireTag = c.String(),
                        BroodYear = c.Int(),
                        MigrationYear = c.Int(),
                        SizeOfCount = c.String(),
                        ScaleId = c.String(),
                        Containment = c.String(),
                        PassNumber = c.Int(),
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
                "dbo.SnorkelFish_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Team = c.String(),
                        NoteTaker = c.String(),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        StartWaterTemp = c.Double(),
                        Visibility = c.String(),
                        WeatherConditions = c.String(),
                        VisitId = c.Int(),
                        Comments = c.String(),
                        CollectionType = c.String(),
                        DominantSpecies = c.String(),
                        CommonSpecies = c.String(),
                        RareSpecies = c.String(),
                        IsAEM = c.String(),
                        HabitatVisitId = c.Int(),
                        EndWaterTemp = c.Double(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.Electrofishing_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FishNumber = c.String(),
                        EventType = c.String(),
                        FileTitle = c.String(),
                        ClipFiles = c.String(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        ReleaseTime = c.DateTime(),
                        ReleaseLocation = c.String(),
                        VisitID = c.String(),
                        Unit = c.String(),
                        Crew = c.String(),
                        StartTemp = c.Double(),
                        ReleaseTemp = c.Double(),
                        Conductivity = c.Double(),
                        EFModel = c.String(),
                        SiteLength = c.Double(),
                        SiteWidth = c.Double(),
                        SiteDepth = c.Double(),
                        SiteArea = c.Double(),
                        HabitatType = c.String(),
                        Visibility = c.String(),
                        ActivityComments = c.String(),
                        ReleaseSite = c.String(),
                        Weather = c.String(),
                        ReleaseRiverKM = c.String(),
                        Pass1TimeBegin = c.DateTime(),
                        Pass1TimeEnd = c.DateTime(),
                        Pass1TotalSecondsEF = c.Double(),
                        Pass1WaterTempBegin = c.Double(),
                        Pass1WaterTempStop = c.Double(),
                        Pass1Hertz = c.Double(),
                        Pass1Freq = c.Double(),
                        Pass1Volts = c.Double(),
                        Pass2TimeBegin = c.DateTime(),
                        Pass2TimeEnd = c.DateTime(),
                        Pass2TotalSecondsEF = c.Double(),
                        Pass2WaterTempBegin = c.Double(),
                        Pass2WaterTempStop = c.Double(),
                        Pass2Hertz = c.Double(),
                        Pass2Freq = c.Double(),
                        Pass2Volts = c.Double(),
                        Pass3TimeBegin = c.DateTime(),
                        Pass3TimeEnd = c.DateTime(),
                        Pass3TotalSecondsEF = c.Double(),
                        Pass3WaterTempBegin = c.Double(),
                        Pass3WaterTempStop = c.Double(),
                        Pass3Hertz = c.Double(),
                        Pass3Freq = c.Double(),
                        Pass3Volts = c.Double(),
                        Pass4TimeBegin = c.DateTime(),
                        Pass4TimeEnd = c.DateTime(),
                        Pass4TotalSecondsEF = c.Double(),
                        Pass4WaterTempBegin = c.Double(),
                        Pass4WaterTempStop = c.Double(),
                        Pass4Hertz = c.Double(),
                        Pass4Freq = c.Double(),
                        Pass4Volts = c.Double(),
                        Pass5TimeBegin = c.DateTime(),
                        Pass5TimeEnd = c.DateTime(),
                        Pass5TotalSecondsEF = c.Double(),
                        Pass5WaterTempBegin = c.Double(),
                        Pass5WaterTempStop = c.Double(),
                        Pass5Hertz = c.Double(),
                        Pass5Freq = c.Double(),
                        Pass5Volts = c.Double(),
                        Pass6TimeBegin = c.DateTime(),
                        Pass6TimeEnd = c.DateTime(),
                        Pass6TotalSecondsEF = c.Double(),
                        Pass6WaterTempBegin = c.Double(),
                        Pass6WaterTempStop = c.Double(),
                        Pass6Hertz = c.Double(),
                        Pass6Freq = c.Double(),
                        Pass6Volts = c.Double(),
                        TotalFishCaptured = c.Int(),
                        FieldsheetLink = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
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
                        AppraisalValue = c.Int(),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .ForeignKey("dbo.QAStatus", t => t.QAStatusId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId)
                .Index(t => t.QAStatusId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.ArtificialProduction_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RunYear = c.String(),
                        Species = c.String(),
                        Origin = c.String(),
                        Sex = c.String(),
                        Disposition = c.String(),
                        TotalFishRepresented = c.Int(),
                        LifeStage = c.String(),
                        FinClip = c.String(),
                        Tag = c.String(),
                        NumberEggsTaken = c.Int(),
                        ReleaseSite = c.String(),
                        FishComments = c.String(),
                        ProgramGroup = c.String(),
                        EyedEggs = c.Int(),
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
                "dbo.ArtificialProduction_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comments = c.String(),
                        FieldSheetFile = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.CreelSurvey_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InterviewTime = c.DateTime(),
                        FishermanId = c.Int(),
                        TotalTimeFished = c.Int(),
                        FishCount = c.Int(),
                        InterviewComments = c.String(),
                        GPSEasting = c.Decimal(precision: 18, scale: 2),
                        GPSNorthing = c.Decimal(precision: 18, scale: 2),
                        DetailLocationId = c.Int(),
                        Species = c.String(),
                        MethodCaught = c.String(),
                        Disposition = c.String(),
                        Sex = c.String(),
                        Origin = c.String(),
                        FinClip = c.String(),
                        Marks = c.String(),
                        ForkLength = c.Int(),
                        MeHPLength = c.Int(),
                        SnoutId = c.String(),
                        ScaleId = c.String(),
                        CarcassComments = c.String(),
                        Tag = c.String(),
                        OtherTagId = c.String(),
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
                .ForeignKey("dbo.Locations", t => t.DetailLocationId)
                .ForeignKey("dbo.QAStatus", t => t.QAStatusId)
                .Index(t => t.DetailLocationId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId)
                .Index(t => t.QAStatusId);
            
            CreateTable(
                "dbo.CreelSurvey_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveySpecies = c.String(),
                        WorkShift = c.String(),
                        Surveyor = c.String(),
                        WeatherConditions = c.String(),
                        TimeStart = c.DateTime(),
                        TimeEnd = c.DateTime(),
                        NumberAnglersObserved = c.Int(),
                        NumberAnglersInterviewed = c.Int(),
                        SurveyComments = c.String(),
                        FieldSheetFile = c.String(),
                        Direction = c.String(),
                        Dry = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.FishTransport_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReleaseSite = c.String(),
                        TotalFishRepresented = c.Int(),
                        ReleaseSiteComments = c.String(),
                        TransportTankUnit = c.String(),
                        TransportReleaseTemperature = c.Double(),
                        TransportReleaseTemperatureF = c.Double(),
                        TransportMortality = c.Int(),
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
                "dbo.FishTransport_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comments = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.StreamNet_NOSA_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommonName = c.String(),
                        Run = c.String(),
                        PopFit = c.String(),
                        WaterBody = c.String(),
                        SpawningYear = c.String(),
                        TRTmethod = c.String(),
                        MethodNumber = c.String(),
                        NOSAIJ = c.String(),
                        NOSAEJ = c.String(),
                        Comment = c.String(),
                        NullRecord = c.String(),
                        DataStatus = c.String(),
                        ContactPersonFirst = c.String(),
                        ContactPersonLast = c.String(),
                        ContactPhone = c.String(),
                        ContactEmail = c.String(),
                        Age10Prop = c.String(),
                        Age10PropLowerLimit = c.String(),
                        Age10PropUpperLimit = c.String(),
                        Age11PlusProp = c.String(),
                        Age11PlusPropLowerLimit = c.String(),
                        Age11PlusPropUpperLimit = c.String(),
                        Age2Prop = c.String(),
                        Age2PropLowerLimit = c.String(),
                        Age2PropUpperLimit = c.String(),
                        Age3Prop = c.String(),
                        Age3PropLowerLimit = c.String(),
                        Age3PropUpperLimit = c.String(),
                        Age4Prop = c.String(),
                        Age4PropLowerLimit = c.String(),
                        Age4PropUpperLimit = c.String(),
                        Age5Prop = c.String(),
                        Age5PropLowerLimit = c.String(),
                        Age5PropUpperLimit = c.String(),
                        Age6Prop = c.String(),
                        Age6PropLowerLimit = c.String(),
                        Age6PropUpperLimit = c.String(),
                        Age7Prop = c.String(),
                        Age7PropLowerLimit = c.String(),
                        Age7PropUpperLimit = c.String(),
                        Age8Prop = c.String(),
                        Age8PropLowerLimit = c.String(),
                        Age8PropUpperLimit = c.String(),
                        Age9Prop = c.String(),
                        Age9PropLowerLimit = c.String(),
                        Age9PropUpperLimit = c.String(),
                        AgePropAlpha = c.String(),
                        CBFWApopName = c.String(),
                        Comments = c.String(),
                        CommonPopName = c.String(),
                        CompilerRecordID = c.String(),
                        DataEntry = c.String(),
                        DataEntryNotes = c.String(),
                        ESU_DPS = c.String(),
                        HOSJF = c.String(),
                        IndicatorLocation = c.String(),
                        LastUpdated = c.String(),
                        MajorPopGroup = c.String(),
                        MeasureLocation = c.String(),
                        MetaComments = c.String(),
                        MethodAdjustments = c.String(),
                        MetricLocation = c.String(),
                        NOBroodStockRemoved = c.String(),
                        NOSAEJAlpha = c.String(),
                        NOSAEJLowerLimit = c.String(),
                        NOSAEJUpperLimit = c.String(),
                        NOSAIJAlpha = c.String(),
                        NOSAIJLowerLimit = c.String(),
                        NOSAIJUpperLimit = c.String(),
                        NOSJF = c.String(),
                        NOSJFAlpha = c.String(),
                        NOSJFLowerLimit = c.String(),
                        NOSJFUpperLimit = c.String(),
                        PopFitNotes = c.String(),
                        PopID = c.String(),
                        ProtMethDocumentation = c.String(),
                        ProtMethName = c.String(),
                        ProtMethURL = c.String(),
                        Publish = c.String(),
                        RecoveryDomain = c.String(),
                        RefID = c.String(),
                        SubmitAgency = c.String(),
                        TSAEJ = c.String(),
                        TSAEJAlpha = c.String(),
                        TSAEJLowerLimit = c.String(),
                        TSAEJUpperLimit = c.String(),
                        TSAIJ = c.String(),
                        TSAIJAlpha = c.String(),
                        TSAIJLowerLimit = c.String(),
                        TSAIJUpperLimit = c.String(),
                        UpdDate = c.String(),
                        pHOSej = c.String(),
                        pHOSejAlpha = c.String(),
                        pHOSejLowerLimit = c.String(),
                        pHOSejUpperLimit = c.String(),
                        pHOSij = c.String(),
                        pHOSijAlpha = c.String(),
                        pHOSijLowerLimit = c.String(),
                        pHOSijUpperLimit = c.String(),
                        ContactAgency = c.String(),
                        ShadowId = c.String(),
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
                "dbo.StreamNet_SAR_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommonName = c.String(),
                        Run = c.String(),
                        PopFit = c.String(),
                        PopFitNotes = c.String(),
                        PopAggregation = c.String(),
                        SmoltLocation = c.String(),
                        AdultLocation = c.String(),
                        SARtype = c.String(),
                        OutmigrationYear = c.String(),
                        TRTmethod = c.String(),
                        ContactAgency = c.String(),
                        MethodNumber = c.String(),
                        SAR = c.String(),
                        RearingType = c.String(),
                        Comments = c.String(),
                        NullRecord = c.String(),
                        DataStatus = c.String(),
                        ContactPersonFirst = c.String(),
                        ContactPersonLast = c.String(),
                        ContactPhone = c.String(),
                        ContactEmail = c.String(),
                        BroodStockRemoved = c.String(),
                        CBFWApopName = c.String(),
                        CommonPopName = c.String(),
                        CompilerRecordID = c.String(),
                        DataEntry = c.String(),
                        DataEntryNotes = c.String(),
                        ESU_DPS = c.String(),
                        HarvestAdj = c.String(),
                        IndicatorLocation = c.String(),
                        LastUpdated = c.String(),
                        MainstemHarvest = c.String(),
                        MajorPopGroup = c.String(),
                        MeasureLocation = c.String(),
                        MetaComments = c.String(),
                        MethodAdjustments = c.String(),
                        MetricLocation = c.String(),
                        OceanHarvest = c.String(),
                        PopID = c.String(),
                        ProtMethDocumentation = c.String(),
                        ProtMethName = c.String(),
                        ProtMethURL = c.String(),
                        Publish = c.String(),
                        RecoveryDomain = c.String(),
                        RefID = c.String(),
                        ReturnDef = c.String(),
                        ReturnsMissing = c.String(),
                        ReturnsMissingExplanation = c.String(),
                        SARAlpha = c.String(),
                        SARLowerLimit = c.String(),
                        SARUpperLimit = c.String(),
                        ScopeOfInference = c.String(),
                        SmoltLocPTcode = c.String(),
                        SubmitAgency = c.String(),
                        TAR = c.String(),
                        TARAlpha = c.String(),
                        TARLowerLimit = c.String(),
                        TARUpperLimit = c.String(),
                        TSO = c.String(),
                        TSOAlpha = c.String(),
                        TSOLowerLimit = c.String(),
                        TSOUpperLimit = c.String(),
                        TribHarvest = c.String(),
                        UpdDate = c.String(),
                        ShadowId = c.String(),
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
                "dbo.StreamNet_RperS_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommonName = c.String(),
                        Run = c.String(),
                        PopFit = c.String(),
                        SpawnerLocation = c.String(),
                        RecruitLocation = c.String(),
                        BroodYear = c.String(),
                        RperStype = c.String(),
                        TRTmethod = c.String(),
                        ContactAgency = c.String(),
                        MethodNumber = c.String(),
                        RperS = c.String(),
                        Comments = c.String(),
                        NullRecord = c.String(),
                        DataStatus = c.String(),
                        ContactPersonFirst = c.String(),
                        ContactPersonLast = c.String(),
                        ContactPhone = c.String(),
                        ContactEmail = c.String(),
                        Age10Adults = c.String(),
                        Age11PlusAdults = c.String(),
                        Age1Juvs = c.String(),
                        Age2Adults = c.String(),
                        Age2Juvs = c.String(),
                        Age3Adults = c.String(),
                        Age3Juvs = c.String(),
                        Age4Adults = c.String(),
                        Age4PlusJuvs = c.String(),
                        Age5Adults = c.String(),
                        Age6Adults = c.String(),
                        Age7Adults = c.String(),
                        Age8Adults = c.String(),
                        Age9Adults = c.String(),
                        CBFWApopName = c.String(),
                        CommonPopName = c.String(),
                        CompilerRecordID = c.String(),
                        DataEntry = c.String(),
                        DataEntryNotes = c.String(),
                        ESU_DPS = c.String(),
                        HarvestAdj = c.String(),
                        HatcherySpawners = c.String(),
                        HatcherySpawnersAlpha = c.String(),
                        HatcherySpawnersLowerLimit = c.String(),
                        HatcherySpawnersUpperLimit = c.String(),
                        IndicatorLocation = c.String(),
                        LastUpdated = c.String(),
                        MainstemHarvest = c.String(),
                        MajorPopGroup = c.String(),
                        MeasureLocation = c.String(),
                        MetaComments = c.String(),
                        MethodAdjustments = c.String(),
                        MetricLocation = c.String(),
                        NOBroodStockRemoved = c.String(),
                        OceanHarvest = c.String(),
                        PopFitNotes = c.String(),
                        PopID = c.String(),
                        ProtMethDocumentation = c.String(),
                        ProtMethName = c.String(),
                        ProtMethURL = c.String(),
                        Publish = c.String(),
                        RecoveryDomain = c.String(),
                        Recruits = c.String(),
                        RecruitsAlpha = c.String(),
                        RecruitsLowerLimit = c.String(),
                        RecruitsMissing = c.String(),
                        RecruitsMissingExplanation = c.String(),
                        RecruitsUpperLimit = c.String(),
                        RefID = c.String(),
                        RperSAlpha = c.String(),
                        RperSLowerLimit = c.String(),
                        RperSUpperLimit = c.String(),
                        SubmitAgency = c.String(),
                        TotalSpawners = c.String(),
                        TotalSpawnersAlpha = c.String(),
                        TotalSpawnersLowerLimit = c.String(),
                        TotalSpawnersUpperLimit = c.String(),
                        TribHarvest = c.String(),
                        UpdDate = c.String(),
                        YOY = c.String(),
                        ShadowId = c.String(),
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
                "dbo.StreamNet_NOSA_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.StreamNet_SAR_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.StreamNet_RperS_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subproject_Crpp", t => t.SubprojectId)
                .Index(t => t.SubprojectId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subproject_Hab", t => t.SubprojectId)
                .Index(t => t.SubprojectId);
            
            CreateTable(
                "dbo.WaterQuality_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CharacteristicName = c.String(),
                        Result = c.Decimal(precision: 18, scale: 2),
                        ResultUnits = c.String(),
                        LabDuplicate = c.String(),
                        Comments = c.String(),
                        MdlResults = c.String(),
                        SampleDate = c.DateTime(),
                        SampleID = c.String(),
                        SampleFraction = c.String(),
                        MethodSpeciation = c.String(),
                        DetectionLimit = c.String(),
                        ContextID = c.String(),
                        MethodID = c.String(),
                        LabName = c.String(),
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
                "dbo.SpawningGroundSurvey_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FeatureID = c.Int(),
                        FeatureType = c.String(),
                        Species = c.String(),
                        Time = c.String(),
                        Temp = c.Single(),
                        Channel = c.String(),
                        ReddLocation = c.String(),
                        ReddHabitat = c.String(),
                        WaypointNumber = c.Int(),
                        FishCount = c.Int(),
                        FishLocation = c.String(),
                        Sex = c.String(),
                        FinClips = c.String(),
                        Marks = c.String(),
                        ForkLength = c.Int(),
                        MeHPLength = c.Int(),
                        SnoutID = c.String(),
                        ScaleID = c.String(),
                        Tag = c.String(),
                        TagID = c.String(),
                        GeneticID = c.String(),
                        KidneyID = c.String(),
                        Origin = c.String(),
                        Comments = c.String(),
                        Ident = c.Int(),
                        EastingUTM = c.Double(),
                        NorthingUTM = c.Double(),
                        EstimatedLocation = c.String(),
                        DateTime = c.String(),
                        NumberEggsRetained = c.Int(),
                        MortalityType = c.String(),
                        ReddMeasurements = c.String(),
                        SpawningStatus = c.String(),
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
                        FieldsheetLink = c.String(),
                        HeaderComments = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.SpawningGroundSurvey_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TargetSpecies = c.String(),
                        Technicians = c.String(),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        StartTemperature = c.Single(),
                        EndTemperature = c.Single(),
                        StartEasting = c.Double(),
                        StartNorthing = c.Double(),
                        EndEasting = c.Double(),
                        EndNorthing = c.Double(),
                        Flow = c.String(),
                        WaterVisibility = c.String(),
                        Weather = c.String(),
                        FlaggedRedds = c.Int(),
                        NewRedds = c.Int(),
                        HeaderComments = c.String(),
                        FieldsheetLink = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.WaterTemp_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReadingDateTime = c.DateTime(nullable: false),
                        GMTReadingDateTime = c.DateTime(),
                        WaterTemperature = c.Double(),
                        WaterTemperatureF = c.Double(),
                        AirTemperature = c.Double(),
                        AirTemperatureF = c.Double(),
                        WaterLevel = c.Double(),
                        TempAToD = c.Double(),
                        BatteryVolts = c.Double(),
                        Conductivity = c.Double(),
                        PSI = c.Double(),
                        AbsolutePressure = c.Double(),
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
                "dbo.WaterTemp_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Technicians = c.String(),
                        Comments = c.String(),
                        CollectionType = c.String(),
                        DepthToWater = c.Double(),
                        PSI = c.Double(),
                        StaticWaterLevel = c.Double(),
                        WeatherConditions = c.String(),
                        SamplePeriod = c.String(),
                        SampleTempUnit = c.String(),
                        FieldActivityType = c.String(),
                        DeployTime = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.ProjectUsers",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.User_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.FishermanProjects",
                c => new
                    {
                        Fisherman_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Fisherman_Id, t.Project_Id })
                .ForeignKey("dbo.Fishermen", t => t.Fisherman_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.Fisherman_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.ProjectInstruments",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        Instrument_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Instrument_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Instruments", t => t.Instrument_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.Instrument_Id);
            
            CreateTable(
                "dbo.LocationProjects",
                c => new
                    {
                        Location_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Location_Id, t.Project_Id })
                .ForeignKey("dbo.Locations", t => t.Location_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.Location_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.DatasetQAStatus",
                c => new
                    {
                        Dataset_Id = c.Int(nullable: false),
                        QAStatus_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Dataset_Id, t.QAStatus_Id })
                .ForeignKey("dbo.Datasets", t => t.Dataset_Id, cascadeDelete: true)
                .ForeignKey("dbo.QAStatus", t => t.QAStatus_Id, cascadeDelete: true)
                .Index(t => t.Dataset_Id)
                .Index(t => t.QAStatus_Id);
            
            CreateTable(
                "dbo.DatasetQAStatus1",
                c => new
                    {
                        Dataset_Id = c.Int(nullable: false),
                        QAStatus_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Dataset_Id, t.QAStatus_Id })
                .ForeignKey("dbo.Datasets", t => t.Dataset_Id, cascadeDelete: true)
                .ForeignKey("dbo.QAStatus", t => t.QAStatus_Id, cascadeDelete: true)
                .Index(t => t.Dataset_Id)
                .Index(t => t.QAStatus_Id);
            */
        }
        
        public override void Down()
        {
            /*DropForeignKey("dbo.WaterTemp_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.WaterTemp_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.WaterTemp_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.WaterTemp_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.WaterTemp_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.SpawningGroundSurvey_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.SpawningGroundSurvey_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.WaterQuality_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.WaterQuality_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.SpawningGroundSurvey_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.SpawningGroundSurvey_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.SpawningGroundSurvey_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.WaterQuality_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.WaterQuality_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.WaterQuality_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.HabitatItems", "SubprojectId", "dbo.Subproject_Hab");
            DropForeignKey("dbo.Files", "Subproject_CrppId", "dbo.Subproject_Crpp");
            DropForeignKey("dbo.CorrespondenceEvents", "SubprojectId", "dbo.Subproject_Crpp");
            DropForeignKey("dbo.StreamNet_RperS_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.StreamNet_RperS_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.StreamNet_SAR_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.StreamNet_SAR_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.StreamNet_NOSA_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.StreamNet_NOSA_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.StreamNet_RperS_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.StreamNet_RperS_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.StreamNet_RperS_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.StreamNet_SAR_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.StreamNet_SAR_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.StreamNet_SAR_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.StreamNet_NOSA_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.StreamNet_NOSA_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.StreamNet_NOSA_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.FishTransport_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.FishTransport_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.FishTransport_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.FishTransport_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.FishTransport_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.CreelSurvey_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.CreelSurvey_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.CreelSurvey_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.CreelSurvey_Detail", "DetailLocationId", "dbo.Locations");
            DropForeignKey("dbo.CreelSurvey_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.CreelSurvey_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.ArtificialProduction_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.ArtificialProduction_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.ArtificialProduction_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.ArtificialProduction_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.ArtificialProduction_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Appraisal_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Appraisal_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Appraisal_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Appraisal_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Appraisal_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Electrofishing_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Electrofishing_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.SnorkelFish_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.SnorkelFish_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Electrofishing_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Electrofishing_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Electrofishing_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.SnorkelFish_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.SnorkelFish_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.SnorkelFish_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.ScrewTrap_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.ScrewTrap_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.ScrewTrap_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.ScrewTrap_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.ScrewTrap_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Metrics_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Metrics_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Metrics_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Metrics_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Metrics_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.JvRearing_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.JvRearing_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.JvRearing_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.JvRearing_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.JvRearing_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Genetic_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Genetic_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Genetic_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Genetic_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Genetic_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.AdultWeir_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.AdultWeir_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.AdultWeir_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.AdultWeir_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.AdultWeir_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.FishScales_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.FishScales_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.FishScales_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.FishScales_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.FishScales_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Drift_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Drift_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Drift_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Drift_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Drift_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.CrppContracts_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.CrppContracts_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.CrppContracts_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.CrppContracts_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.CrppContracts_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.BSample_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.BSample_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.BSample_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.BSample_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.BSample_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Benthic_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Benthic_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Benthic_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Benthic_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Benthic_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.MetadataProperties", "MetadataEntityId", "dbo.MetadataEntities");
            DropForeignKey("dbo.ActivityQAs", "UserId", "dbo.Users");
            DropForeignKey("dbo.ActivityQAs", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Activities", "UserId", "dbo.Users");
            DropForeignKey("dbo.Activities", "SourceId", "dbo.Sources");
            DropForeignKey("dbo.Activities", "PostAccuracyCheckId", "dbo.InstrumentAccuracyChecks");
            DropForeignKey("dbo.Activities", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Activities", "InstrumentId", "dbo.Instruments");
            DropForeignKey("dbo.Activities", "DatasetId", "dbo.Datasets");
            DropForeignKey("dbo.DatasetQAStatus1", "QAStatus_Id", "dbo.QAStatus");
            DropForeignKey("dbo.DatasetQAStatus1", "Dataset_Id", "dbo.Datasets");
            DropForeignKey("dbo.DatasetQAStatus", "QAStatus_Id", "dbo.QAStatus");
            DropForeignKey("dbo.DatasetQAStatus", "Dataset_Id", "dbo.Datasets");
            DropForeignKey("dbo.DatasetFields", "DatasetId", "dbo.Datasets");
            DropForeignKey("dbo.DatasetFields", "SourceId", "dbo.Sources");
            DropForeignKey("dbo.DatasetFields", "InstrumentId", "dbo.Instruments");
            DropForeignKey("dbo.DatasetFields", "FieldRoleId", "dbo.FieldRoles");
            DropForeignKey("dbo.DatasetFields", "FieldId", "dbo.Fields");
            DropForeignKey("dbo.Fields", "FieldCategoryId", "dbo.FieldCategories");
            DropForeignKey("dbo.Datasets", "DefaultRowQAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Datasets", "DatastoreId", "dbo.Datastores");
            DropForeignKey("dbo.Activities", "ActivityTypeId", "dbo.ActivityTypes");
            DropForeignKey("dbo.Activities", "AccuracyCheckId", "dbo.InstrumentAccuracyChecks");
            DropForeignKey("dbo.InstrumentAccuracyChecks", "UserId", "dbo.Users");
            DropForeignKey("dbo.Instruments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Projects", "ProjectTypeId", "dbo.ProjectTypes");
            DropForeignKey("dbo.Projects", "OwnerId", "dbo.Users");
            DropForeignKey("dbo.Projects", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.Locations", "WaterBodyId", "dbo.WaterBodies");
            DropForeignKey("dbo.Locations", "SdeFeatureClassId", "dbo.SdeFeatureClasses");
            DropForeignKey("dbo.LocationProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.LocationProjects", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Locations", "LocationTypeId", "dbo.LocationTypes");
            DropForeignKey("dbo.ProjectInstruments", "Instrument_Id", "dbo.Instruments");
            DropForeignKey("dbo.ProjectInstruments", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.FishermanProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.FishermanProjects", "Fisherman_Id", "dbo.Fishermen");
            DropForeignKey("dbo.Files", "UserId", "dbo.Users");
            DropForeignKey("dbo.Files", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Files", "FileTypeId", "dbo.FileTypes");
            DropForeignKey("dbo.ProjectUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ProjectUsers", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.UserPreferences", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.Users", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Instruments", "OwningDepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Departments", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.Instruments", "InstrumentTypeId", "dbo.InstrumentTypes");
            DropForeignKey("dbo.InstrumentAccuracyChecks", "InstrumentId", "dbo.Instruments");
            DropIndex("dbo.DatasetQAStatus1", new[] { "QAStatus_Id" });
            DropIndex("dbo.DatasetQAStatus1", new[] { "Dataset_Id" });
            DropIndex("dbo.DatasetQAStatus", new[] { "QAStatus_Id" });
            DropIndex("dbo.DatasetQAStatus", new[] { "Dataset_Id" });
            DropIndex("dbo.LocationProjects", new[] { "Project_Id" });
            DropIndex("dbo.LocationProjects", new[] { "Location_Id" });
            DropIndex("dbo.ProjectInstruments", new[] { "Instrument_Id" });
            DropIndex("dbo.ProjectInstruments", new[] { "Project_Id" });
            DropIndex("dbo.FishermanProjects", new[] { "Project_Id" });
            DropIndex("dbo.FishermanProjects", new[] { "Fisherman_Id" });
            DropIndex("dbo.ProjectUsers", new[] { "User_Id" });
            DropIndex("dbo.ProjectUsers", new[] { "Project_Id" });
            DropIndex("dbo.WaterTemp_Header", new[] { "ByUserId" });
            DropIndex("dbo.WaterTemp_Header", new[] { "ActivityId" });
            DropIndex("dbo.WaterTemp_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.WaterTemp_Detail", new[] { "ByUserId" });
            DropIndex("dbo.WaterTemp_Detail", new[] { "ActivityId" });
            DropIndex("dbo.SpawningGroundSurvey_Header", new[] { "ByUserId" });
            DropIndex("dbo.SpawningGroundSurvey_Header", new[] { "ActivityId" });
            DropIndex("dbo.WaterQuality_Header", new[] { "ByUserId" });
            DropIndex("dbo.WaterQuality_Header", new[] { "ActivityId" });
            DropIndex("dbo.SpawningGroundSurvey_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.SpawningGroundSurvey_Detail", new[] { "ByUserId" });
            DropIndex("dbo.SpawningGroundSurvey_Detail", new[] { "ActivityId" });
            DropIndex("dbo.WaterQuality_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.WaterQuality_Detail", new[] { "ByUserId" });
            DropIndex("dbo.WaterQuality_Detail", new[] { "ActivityId" });
            DropIndex("dbo.HabitatItems", new[] { "SubprojectId" });
            DropIndex("dbo.CorrespondenceEvents", new[] { "SubprojectId" });
            DropIndex("dbo.StreamNet_RperS_Header", new[] { "ByUserId" });
            DropIndex("dbo.StreamNet_RperS_Header", new[] { "ActivityId" });
            DropIndex("dbo.StreamNet_SAR_Header", new[] { "ByUserId" });
            DropIndex("dbo.StreamNet_SAR_Header", new[] { "ActivityId" });
            DropIndex("dbo.StreamNet_NOSA_Header", new[] { "ByUserId" });
            DropIndex("dbo.StreamNet_NOSA_Header", new[] { "ActivityId" });
            DropIndex("dbo.StreamNet_RperS_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.StreamNet_RperS_Detail", new[] { "ByUserId" });
            DropIndex("dbo.StreamNet_RperS_Detail", new[] { "ActivityId" });
            DropIndex("dbo.StreamNet_SAR_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.StreamNet_SAR_Detail", new[] { "ByUserId" });
            DropIndex("dbo.StreamNet_SAR_Detail", new[] { "ActivityId" });
            DropIndex("dbo.StreamNet_NOSA_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.StreamNet_NOSA_Detail", new[] { "ByUserId" });
            DropIndex("dbo.StreamNet_NOSA_Detail", new[] { "ActivityId" });
            DropIndex("dbo.FishTransport_Header", new[] { "ByUserId" });
            DropIndex("dbo.FishTransport_Header", new[] { "ActivityId" });
            DropIndex("dbo.FishTransport_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.FishTransport_Detail", new[] { "ByUserId" });
            DropIndex("dbo.FishTransport_Detail", new[] { "ActivityId" });
            DropIndex("dbo.CreelSurvey_Header", new[] { "ByUserId" });
            DropIndex("dbo.CreelSurvey_Header", new[] { "ActivityId" });
            DropIndex("dbo.CreelSurvey_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.CreelSurvey_Detail", new[] { "ByUserId" });
            DropIndex("dbo.CreelSurvey_Detail", new[] { "ActivityId" });
            DropIndex("dbo.CreelSurvey_Detail", new[] { "DetailLocationId" });
            DropIndex("dbo.ArtificialProduction_Header", new[] { "ByUserId" });
            DropIndex("dbo.ArtificialProduction_Header", new[] { "ActivityId" });
            DropIndex("dbo.ArtificialProduction_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.ArtificialProduction_Detail", new[] { "ByUserId" });
            DropIndex("dbo.ArtificialProduction_Detail", new[] { "ActivityId" });
            DropIndex("dbo.Appraisal_Header", new[] { "ByUserId" });
            DropIndex("dbo.Appraisal_Header", new[] { "ActivityId" });
            DropIndex("dbo.Appraisal_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Appraisal_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Appraisal_Detail", new[] { "ActivityId" });
            DropIndex("dbo.Electrofishing_Header", new[] { "ByUserId" });
            DropIndex("dbo.Electrofishing_Header", new[] { "ActivityId" });
            DropIndex("dbo.SnorkelFish_Header", new[] { "ByUserId" });
            DropIndex("dbo.SnorkelFish_Header", new[] { "ActivityId" });
            DropIndex("dbo.Electrofishing_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Electrofishing_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Electrofishing_Detail", new[] { "ActivityId" });
            DropIndex("dbo.SnorkelFish_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.SnorkelFish_Detail", new[] { "ByUserId" });
            DropIndex("dbo.SnorkelFish_Detail", new[] { "ActivityId" });
            DropIndex("dbo.ScrewTrap_Header", new[] { "ByUserId" });
            DropIndex("dbo.ScrewTrap_Header", new[] { "ActivityId" });
            DropIndex("dbo.ScrewTrap_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.ScrewTrap_Detail", new[] { "ByUserId" });
            DropIndex("dbo.ScrewTrap_Detail", new[] { "ActivityId" });
            DropIndex("dbo.Metrics_Header", new[] { "ByUserId" });
            DropIndex("dbo.Metrics_Header", new[] { "ActivityId" });
            DropIndex("dbo.Metrics_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Metrics_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Metrics_Detail", new[] { "ActivityId" });
            DropIndex("dbo.JvRearing_Header", new[] { "ByUserId" });
            DropIndex("dbo.JvRearing_Header", new[] { "ActivityId" });
            DropIndex("dbo.JvRearing_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.JvRearing_Detail", new[] { "ByUserId" });
            DropIndex("dbo.JvRearing_Detail", new[] { "ActivityId" });
            DropIndex("dbo.Genetic_Header", new[] { "ByUserId" });
            DropIndex("dbo.Genetic_Header", new[] { "ActivityId" });
            DropIndex("dbo.Genetic_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Genetic_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Genetic_Detail", new[] { "ActivityId" });
            DropIndex("dbo.AdultWeir_Header", new[] { "ByUserId" });
            DropIndex("dbo.AdultWeir_Header", new[] { "ActivityId" });
            DropIndex("dbo.AdultWeir_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.AdultWeir_Detail", new[] { "ByUserId" });
            DropIndex("dbo.AdultWeir_Detail", new[] { "ActivityId" });
            DropIndex("dbo.FishScales_Header", new[] { "ByUserId" });
            DropIndex("dbo.FishScales_Header", new[] { "ActivityId" });
            DropIndex("dbo.FishScales_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.FishScales_Detail", new[] { "ByUserId" });
            DropIndex("dbo.FishScales_Detail", new[] { "ActivityId" });
            DropIndex("dbo.Drift_Header", new[] { "ByUserId" });
            DropIndex("dbo.Drift_Header", new[] { "ActivityId" });
            DropIndex("dbo.Drift_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Drift_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Drift_Detail", new[] { "ActivityId" });
            DropIndex("dbo.CrppContracts_Header", new[] { "ByUserId" });
            DropIndex("dbo.CrppContracts_Header", new[] { "ActivityId" });
            DropIndex("dbo.CrppContracts_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.CrppContracts_Detail", new[] { "ByUserId" });
            DropIndex("dbo.CrppContracts_Detail", new[] { "ActivityId" });
            DropIndex("dbo.BSample_Header", new[] { "ByUserId" });
            DropIndex("dbo.BSample_Header", new[] { "ActivityId" });
            DropIndex("dbo.BSample_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.BSample_Detail", new[] { "ByUserId" });
            DropIndex("dbo.BSample_Detail", new[] { "ActivityId" });
            DropIndex("dbo.Benthic_Header", new[] { "ByUserId" });
            DropIndex("dbo.Benthic_Header", new[] { "ActivityId" });
            DropIndex("dbo.Benthic_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Benthic_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Benthic_Detail", new[] { "ActivityId" });
            DropIndex("dbo.MetadataProperties", new[] { "MetadataEntityId" });
            DropIndex("dbo.ActivityQAs", new[] { "UserId" });
            DropIndex("dbo.ActivityQAs", new[] { "QAStatusId" });
            DropIndex("dbo.Fields", new[] { "FieldCategoryId" });
            DropIndex("dbo.DatasetFields", new[] { "InstrumentId" });
            DropIndex("dbo.DatasetFields", new[] { "SourceId" });
            DropIndex("dbo.DatasetFields", new[] { "FieldRoleId" });
            DropIndex("dbo.DatasetFields", new[] { "FieldId" });
            DropIndex("dbo.DatasetFields", new[] { "DatasetId" });
            DropIndex("dbo.Datasets", new[] { "DatastoreId" });
            DropIndex("dbo.Datasets", new[] { "DefaultRowQAStatusId" });
            DropIndex("dbo.Activities", new[] { "PostAccuracyCheckId" });
            DropIndex("dbo.Activities", new[] { "AccuracyCheckId" });
            DropIndex("dbo.Activities", new[] { "InstrumentId" });
            DropIndex("dbo.Activities", new[] { "ActivityTypeId" });
            DropIndex("dbo.Activities", new[] { "UserId" });
            DropIndex("dbo.Activities", new[] { "LocationId" });
            DropIndex("dbo.Activities", new[] { "SourceId" });
            DropIndex("dbo.Activities", new[] { "DatasetId" });
            DropIndex("dbo.Locations", new[] { "SdeFeatureClassId" });
            DropIndex("dbo.Locations", new[] { "WaterBodyId" });
            DropIndex("dbo.Locations", new[] { "LocationTypeId" });
            DropIndex("dbo.Files", new[] { "Subproject_CrppId" });
            DropIndex("dbo.Files", new[] { "FileTypeId" });
            DropIndex("dbo.Files", new[] { "UserId" });
            DropIndex("dbo.Files", new[] { "ProjectId" });
            DropIndex("dbo.UserPreferences", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "DepartmentId" });
            DropIndex("dbo.Users", new[] { "OrganizationId" });
            DropIndex("dbo.Projects", new[] { "OwnerId" });
            DropIndex("dbo.Projects", new[] { "OrganizationId" });
            DropIndex("dbo.Projects", new[] { "ProjectTypeId" });
            DropIndex("dbo.Departments", new[] { "OrganizationId" });
            DropIndex("dbo.Instruments", new[] { "UserId" });
            DropIndex("dbo.Instruments", new[] { "InstrumentTypeId" });
            DropIndex("dbo.Instruments", new[] { "OwningDepartmentId" });
            DropIndex("dbo.InstrumentAccuracyChecks", new[] { "UserId" });
            DropIndex("dbo.InstrumentAccuracyChecks", new[] { "InstrumentId" });
            DropTable("dbo.DatasetQAStatus1");
            DropTable("dbo.DatasetQAStatus");
            DropTable("dbo.LocationProjects");
            DropTable("dbo.ProjectInstruments");
            DropTable("dbo.FishermanProjects");
            DropTable("dbo.ProjectUsers");
            DropTable("dbo.WaterTemp_Header");
            DropTable("dbo.WaterTemp_Detail");
            DropTable("dbo.SpawningGroundSurvey_Header");
            DropTable("dbo.WaterQuality_Header");
            DropTable("dbo.SpawningGroundSurvey_Detail");
            DropTable("dbo.WaterQuality_Detail");
            DropTable("dbo.HabitatItems");
            DropTable("dbo.Subproject_Hab");
            DropTable("dbo.CorrespondenceEvents");
            DropTable("dbo.Subproject_Crpp");
            DropTable("dbo.StreamNet_RperS_Header");
            DropTable("dbo.StreamNet_SAR_Header");
            DropTable("dbo.StreamNet_NOSA_Header");
            DropTable("dbo.StreamNet_RperS_Detail");
            DropTable("dbo.StreamNet_SAR_Detail");
            DropTable("dbo.StreamNet_NOSA_Detail");
            DropTable("dbo.FishTransport_Header");
            DropTable("dbo.FishTransport_Detail");
            DropTable("dbo.CreelSurvey_Header");
            DropTable("dbo.CreelSurvey_Detail");
            DropTable("dbo.ArtificialProduction_Header");
            DropTable("dbo.ArtificialProduction_Detail");
            DropTable("dbo.Appraisal_Header");
            DropTable("dbo.Appraisal_Detail");
            DropTable("dbo.Electrofishing_Header");
            DropTable("dbo.SnorkelFish_Header");
            DropTable("dbo.Electrofishing_Detail");
            DropTable("dbo.SnorkelFish_Detail");
            DropTable("dbo.ScrewTrap_Header");
            DropTable("dbo.ScrewTrap_Detail");
            DropTable("dbo.Metrics_Header");
            DropTable("dbo.Metrics_Detail");
            DropTable("dbo.JvRearing_Header");
            DropTable("dbo.JvRearing_Detail");
            DropTable("dbo.Genetic_Header");
            DropTable("dbo.Genetic_Detail");
            DropTable("dbo.AdultWeir_Header");
            DropTable("dbo.AdultWeir_Detail");
            DropTable("dbo.FishScales_Header");
            DropTable("dbo.FishScales_Detail");
            DropTable("dbo.Drift_Header");
            DropTable("dbo.Drift_Detail");
            DropTable("dbo.CrppContracts_Header");
            DropTable("dbo.CrppContracts_Detail");
            DropTable("dbo.BSample_Header");
            DropTable("dbo.BSample_Detail");
            DropTable("dbo.Benthic_Header");
            DropTable("dbo.Benthic_Detail");
            DropTable("dbo.MetadataValues");
            DropTable("dbo.MetadataProperties");
            DropTable("dbo.MetadataEntities");
            DropTable("dbo.Fundings");
            DropTable("dbo.Collaborators");
            DropTable("dbo.AuditJournals");
            DropTable("dbo.ActivityQAs");
            DropTable("dbo.Sources");
            DropTable("dbo.FieldRoles");
            DropTable("dbo.FieldCategories");
            DropTable("dbo.Fields");
            DropTable("dbo.DatasetFields");
            DropTable("dbo.QAStatus");
            DropTable("dbo.Datastores");
            DropTable("dbo.Datasets");
            DropTable("dbo.ActivityTypes");
            DropTable("dbo.Activities");
            DropTable("dbo.ProjectTypes");
            DropTable("dbo.WaterBodies");
            DropTable("dbo.SdeFeatureClasses");
            DropTable("dbo.LocationTypes");
            DropTable("dbo.Locations");
            DropTable("dbo.Fishermen");
            DropTable("dbo.FileTypes");
            DropTable("dbo.Files");
            DropTable("dbo.UserPreferences");
            DropTable("dbo.Users");
            DropTable("dbo.Projects");
            DropTable("dbo.Organizations");
            DropTable("dbo.Departments");
            DropTable("dbo.InstrumentTypes");
            DropTable("dbo.Instruments");
            DropTable("dbo.InstrumentAccuracyChecks");
            */
        }
    }
}
