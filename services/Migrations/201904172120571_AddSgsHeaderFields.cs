namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSgsHeaderFields : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChannelUnitMetrics_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChUnitID = c.Int(),
                        ChUnitNum = c.Int(),
                        Tier1 = c.Int(),
                        Tier2 = c.Int(),
                        AreaTotal = c.Decimal(precision: 18, scale: 2),
                        PolyArea = c.Decimal(precision: 18, scale: 2),
                        TotalVol = c.Decimal(precision: 18, scale: 2),
                        DpthMax = c.Decimal(precision: 18, scale: 2),
                        DpthThlwgExit = c.Decimal(precision: 18, scale: 2),
                        DpthResid = c.Decimal(precision: 18, scale: 2),
                        CountOfLWD = c.Int(),
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
                "dbo.ChannelUnitMetrics_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramSiteID = c.Int(),
                        SiteName = c.String(),
                        WatershedID = c.Int(),
                        WatershedName = c.String(),
                        SampleDate = c.String(),
                        HitchName = c.String(),
                        CrewName = c.String(),
                        VisitYear = c.Int(),
                        IterationID = c.Int(),
                        CategoryName = c.String(),
                        PanelName = c.String(),
                        VisitID = c.Int(),
                        VisitDate = c.Int(),
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
                "dbo.Tier1_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tier1 = c.Int(),
                        Area = c.Decimal(precision: 18, scale: 2),
                        Ct = c.Int(),
                        UnitSpacing = c.Decimal(precision: 18, scale: 2),
                        Freq = c.Decimal(precision: 18, scale: 2),
                        Vol = c.Decimal(precision: 18, scale: 2),
                        Pct = c.Decimal(precision: 18, scale: 2),
                        DpthThlwgMaxAvg = c.Decimal(precision: 18, scale: 2),
                        DpthThlwgExit = c.Decimal(precision: 18, scale: 2),
                        DpthResid = c.Decimal(precision: 18, scale: 2),
                        SubEstBldr = c.Decimal(precision: 18, scale: 2),
                        SubEstCbl = c.Decimal(precision: 18, scale: 2),
                        SubEstGrvl = c.Decimal(precision: 18, scale: 2),
                        SubEstSandFines = c.Decimal(precision: 18, scale: 2),
                        FishCovLW = c.Decimal(precision: 18, scale: 2),
                        FishCovTVeg = c.Decimal(precision: 18, scale: 2),
                        FishCovUcut = c.Decimal(precision: 18, scale: 2),
                        FishCovArt = c.Decimal(precision: 18, scale: 2),
                        FishCovAqVeg = c.Decimal(precision: 18, scale: 2),
                        FishCovNone = c.Decimal(precision: 18, scale: 2),
                        FishCovTotal = c.Decimal(precision: 18, scale: 2),
                        LWVolWetTier1 = c.Decimal(precision: 18, scale: 2),
                        LWVolBfTier1 = c.Decimal(precision: 18, scale: 2),
                        CountOfLWD = c.Int(),
                        CountOfChinook = c.Int(),
                        CountOfCoho = c.Int(),
                        CountOfSockeye = c.Int(),
                        CountOfChum = c.Int(),
                        CountOfOmykiss = c.Int(),
                        CountOfPink = c.Int(),
                        CountOfCutthroat = c.Int(),
                        CountOfBulltrout = c.Int(),
                        CountOfBrooktrout = c.Int(),
                        CountOfLamprey = c.Int(),
                        CountOfOtherSpecies = c.Int(),
                        DensityOfChinook = c.Decimal(precision: 18, scale: 2),
                        DensityOfCoho = c.Decimal(precision: 18, scale: 2),
                        DensityOfSockeye = c.Decimal(precision: 18, scale: 2),
                        DensityOfChum = c.Decimal(precision: 18, scale: 2),
                        DensityOfOmykiss = c.Decimal(precision: 18, scale: 2),
                        DensityOfPink = c.Decimal(precision: 18, scale: 2),
                        DensityOfCutthroat = c.Decimal(precision: 18, scale: 2),
                        DensityOfBulltrout = c.Decimal(precision: 18, scale: 2),
                        DensityOfBrooktrout = c.Decimal(precision: 18, scale: 2),
                        DensityOfLamprey = c.Decimal(precision: 18, scale: 2),
                        DensityOfOtherSpecies = c.Decimal(precision: 18, scale: 2),
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
                "dbo.Tier1_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramSiteID = c.Int(),
                        SiteName = c.String(),
                        WatershedID = c.Int(),
                        WatershedName = c.String(),
                        SampleDate = c.String(),
                        HitchName = c.String(),
                        CrewName = c.String(),
                        VisitYear = c.Int(),
                        IterationID = c.Int(),
                        CategoryName = c.String(),
                        PanelName = c.String(),
                        VisitID = c.Int(),
                        VisitDate = c.Int(),
                        ProtocolID = c.Int(),
                        ProgramID = c.Int(),
                        AEM = c.Int(),
                        Effectiveness = c.Int(),
                        HasFishData = c.Int(),
                        PrimaryVisit = c.Int(),
                        QCVisit = c.Int(),
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
                "dbo.Tier2_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tier2 = c.String(),
                        Area = c.Decimal(precision: 18, scale: 2),
                        Ct = c.Int(),
                        UnitSpacing = c.Decimal(precision: 18, scale: 2),
                        Freq = c.Decimal(precision: 18, scale: 2),
                        Vol = c.Decimal(precision: 18, scale: 2),
                        Pct = c.Decimal(precision: 18, scale: 2),
                        DpthThlwgMaxAvg = c.Decimal(precision: 18, scale: 2),
                        DpthThlwgExit = c.Decimal(precision: 18, scale: 2),
                        DpthResid = c.Decimal(precision: 18, scale: 2),
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
                "dbo.Tier2_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramSiteID = c.Int(),
                        SiteName = c.String(),
                        WatershedID = c.Int(),
                        WatershedName = c.String(),
                        SampleDate = c.String(),
                        HitchName = c.String(),
                        CrewName = c.String(),
                        VisitYear = c.Int(),
                        IterationID = c.Int(),
                        CategoryName = c.String(),
                        PanelName = c.String(),
                        VisitID = c.Int(),
                        VisitDate = c.Int(),
                        ProtocolID = c.Int(),
                        ProgramID = c.Int(),
                        AEM = c.String(),
                        Effectiveness = c.String(),
                        HasFishData = c.String(),
                        PrimaryVisit = c.String(),
                        QCVisit = c.String(),
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
                "dbo.VisitMetrics_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Alkalinity = c.Int(),
                        BankErosion = c.Decimal(precision: 18, scale: 2),
                        BankfullArea = c.Decimal(precision: 18, scale: 2),
                        BankfullChannelBraidedness = c.Decimal(precision: 18, scale: 2),
                        BankfullChannelCount = c.Int(),
                        BankfullChannelIslandCount = c.Int(),
                        BankfullChannelQualifyingIslandArea = c.Decimal(precision: 18, scale: 2),
                        BankfullChannelQualifyingIslandCount = c.Int(),
                        BankfullChannelTotalLength = c.Decimal(precision: 18, scale: 2),
                        BankfullDepthAvg = c.Decimal(precision: 18, scale: 2),
                        BankfullDepthMax = c.Decimal(precision: 18, scale: 2),
                        BankfullMainChannelPartCount = c.Int(),
                        BankfullSideChannelWidth = c.Decimal(precision: 18, scale: 2),
                        BankfullSideChannelWidthCV = c.Decimal(precision: 18, scale: 2),
                        BankfullSideChannelWidthToDepthRatioAvg = c.Decimal(precision: 18, scale: 2),
                        BankfullSideChannelWidthToDepthRatioCV = c.Decimal(precision: 18, scale: 2),
                        BankfullSideChannelWidthToMaxDepthRatioAvg = c.Decimal(precision: 18, scale: 2),
                        BankfullSideChannelWidthToMaxDepthRatioCV = c.Decimal(precision: 18, scale: 2),
                        BankfullSiteLength = c.Decimal(precision: 18, scale: 2),
                        BankfullVolume = c.Decimal(precision: 18, scale: 2),
                        BankfullWidthAvg = c.Decimal(precision: 18, scale: 2),
                        BankfullWidthCV = c.Decimal(precision: 18, scale: 2),
                        BankfullWidthIntegrated = c.Decimal(precision: 18, scale: 2),
                        BankfullWidthToDepthRatioAvg = c.Decimal(precision: 18, scale: 2),
                        BankfullWidthToDepthRatioCV = c.Decimal(precision: 18, scale: 2),
                        BankfullWidthToMaxDepthRatioAvg = c.Decimal(precision: 18, scale: 2),
                        BankfullWidthToMaxDepthRatioCV = c.Decimal(precision: 18, scale: 2),
                        BraidChannelRatio = c.Decimal(precision: 18, scale: 2),
                        Conductivity = c.Decimal(precision: 18, scale: 2),
                        ConstrainingFeatureHeightAverage = c.Decimal(precision: 18, scale: 2),
                        CountOfBrooktrout = c.Int(),
                        CountOfBulltrout = c.Int(),
                        CountOfChinook = c.Int(),
                        CountOfChum = c.Int(),
                        CountOfCoho = c.Int(),
                        CountOfCutthroat = c.Int(),
                        CountOfJamLargeWoodyPieces = c.Int(),
                        CountOfKeyLargeWoodyPieces = c.Int(),
                        CountOfLamprey = c.Int(),
                        CountOfLargeWoodyPieces = c.Int(),
                        CountOfLeftBankLargeWoodyPieces = c.Int(),
                        CountOfMidChannelLargeWoodyPieces = c.Int(),
                        CountOfOmykiss = c.Int(),
                        CountOfOtherSpecies = c.Int(),
                        CountOfPink = c.Int(),
                        CountOfPoolFormingLargeWoodyPieces = c.Int(),
                        CountOfRightBankLargeWoodyPieces = c.Int(),
                        CountOfSockeye = c.Int(),
                        DensityOfBrooktrout = c.Decimal(precision: 18, scale: 2),
                        DensityOfBulltrout = c.Decimal(precision: 18, scale: 2),
                        DensityOfChinook = c.Decimal(precision: 18, scale: 2),
                        DensityOfChum = c.Decimal(precision: 18, scale: 2),
                        DensityOfCoho = c.Decimal(precision: 18, scale: 2),
                        DensityOfCutthroat = c.Decimal(precision: 18, scale: 2),
                        DensityOfLamprey = c.Decimal(precision: 18, scale: 2),
                        DensityOfOmykiss = c.Decimal(precision: 18, scale: 2),
                        DensityOfPink = c.Decimal(precision: 18, scale: 2),
                        DensityOfSockeye = c.Decimal(precision: 18, scale: 2),
                        DetrendedElevationSD = c.Decimal(precision: 18, scale: 2),
                        Discharge = c.Decimal(precision: 18, scale: 2),
                        DriftBiomass = c.Decimal(precision: 18, scale: 2),
                        FastNonTurbulentArea = c.Decimal(precision: 18, scale: 2),
                        FastNonTurbulentCount = c.Int(),
                        FastNonTurbulentFrequency = c.Decimal(precision: 18, scale: 2),
                        FastNonTurbulentPercent = c.Decimal(precision: 18, scale: 2),
                        FastNonTurbulentVolume = c.Decimal(precision: 18, scale: 2),
                        FastTurbulentArea = c.Decimal(precision: 18, scale: 2),
                        FastTurbulentCount = c.Int(),
                        FastTurbulentFrequency = c.Decimal(precision: 18, scale: 2),
                        FastTurbulentPercent = c.Decimal(precision: 18, scale: 2),
                        FastTurbulentVolume = c.Decimal(precision: 18, scale: 2),
                        FishCoverAquaticVegetation = c.Decimal(precision: 18, scale: 2),
                        FishCoverArtificial = c.Decimal(precision: 18, scale: 2),
                        FishCoverLW = c.Decimal(precision: 18, scale: 2),
                        FishCoverNone = c.Decimal(precision: 18, scale: 2),
                        FishCoverTerrestrialVegetation = c.Decimal(precision: 18, scale: 2),
                        FishCoverTotal = c.Decimal(precision: 18, scale: 2),
                        FloodProneWidthAverage = c.Decimal(precision: 18, scale: 2),
                        Gradient = c.Decimal(precision: 18, scale: 2),
                        LargeWoodFrequencyBankfull = c.Decimal(precision: 18, scale: 2),
                        LargeWoodFrequencyWetted = c.Decimal(precision: 18, scale: 2),
                        PercentConstrained = c.Int(),
                        PoolToTurbulentAreaRatio = c.Decimal(precision: 18, scale: 2),
                        PercentUndercutByArea = c.Decimal(precision: 18, scale: 2),
                        PercentUndercutByLength = c.Decimal(precision: 18, scale: 2),
                        ResidualPoolDepth = c.Decimal(precision: 18, scale: 2),
                        RiparianCoverBigTree = c.Decimal(precision: 18, scale: 2),
                        RiparianCoverConiferous = c.Decimal(precision: 18, scale: 2),
                        RiparianCoverGround = c.Decimal(precision: 18, scale: 2),
                        RiparianCoverNoCanopy = c.Decimal(precision: 18, scale: 2),
                        RiparianCoverNonWoody = c.Decimal(precision: 18, scale: 2),
                        RiparianCoverUnderstory = c.Decimal(precision: 18, scale: 2),
                        RiparianCoverWoody = c.Decimal(precision: 18, scale: 2),
                        Sinuosity = c.Decimal(precision: 18, scale: 2),
                        SinuosityViaCenterline = c.Decimal(precision: 18, scale: 2),
                        SlowWaterArea = c.Decimal(precision: 18, scale: 2),
                        SlowWaterCount = c.Int(),
                        SlowWaterFrequency = c.Decimal(precision: 18, scale: 2),
                        SlowWaterPercent = c.Decimal(precision: 18, scale: 2),
                        SlowWaterVolume = c.Decimal(precision: 18, scale: 2),
                        SolarAccessSummerAvg = c.Decimal(precision: 18, scale: 2),
                        SubstrateLt2mm = c.Decimal(precision: 18, scale: 2),
                        SubstrateLt6mm = c.Decimal(precision: 18, scale: 2),
                        SubstrateEstBoulders = c.Decimal(precision: 18, scale: 2),
                        SubstrateEstCoarseAndFineGravel = c.Decimal(precision: 18, scale: 2),
                        SubstrateEstCobbles = c.Decimal(precision: 18, scale: 2),
                        SubstrateEstSandAndFines = c.Decimal(precision: 18, scale: 2),
                        SubstrateD16 = c.Decimal(precision: 18, scale: 2),
                        SubstrateD50 = c.Decimal(precision: 18, scale: 2),
                        SubstrateD84 = c.Decimal(precision: 18, scale: 2),
                        SubstrateEmbeddednessAvg = c.Decimal(precision: 18, scale: 2),
                        SubstrateEmbeddednessSD = c.Decimal(precision: 18, scale: 2),
                        ThalwegDepthAvg = c.Decimal(precision: 18, scale: 2),
                        ThalwegDepthCV = c.Decimal(precision: 18, scale: 2),
                        ThalwegSiteLength = c.Decimal(precision: 18, scale: 2),
                        TotalUndercutArea = c.Decimal(precision: 18, scale: 2),
                        ValleyWidth = c.Decimal(precision: 18, scale: 2),
                        WettedArea = c.Decimal(precision: 18, scale: 2),
                        WettedChannelBraidedness = c.Decimal(precision: 18, scale: 2),
                        WettedChannelCount = c.Int(),
                        WettedChannelIslandCount = c.Int(),
                        WettedChannelMainstemArea = c.Decimal(precision: 18, scale: 2),
                        WettedChannelMainstemLength = c.Decimal(precision: 18, scale: 2),
                        WettedChannelMainstemSinuosity = c.Decimal(precision: 18, scale: 2),
                        WettedChannelQualifyingIslandArea = c.Decimal(precision: 18, scale: 2),
                        WettedChannelQualifyingIslandCount = c.Int(),
                        WettedChannelTotalLength = c.Decimal(precision: 18, scale: 2),
                        WettedDepthSD = c.Decimal(precision: 18, scale: 2),
                        WettedLargeSideChannelArea = c.Decimal(precision: 18, scale: 2),
                        WettedMainChannelPartCount = c.Int(),
                        WettedSideChannelPercentByArea = c.Decimal(precision: 18, scale: 2),
                        WettedSideChannelWidth = c.Decimal(precision: 18, scale: 2),
                        WettedSideChannelWidthCV = c.Decimal(precision: 18, scale: 2),
                        WettedSideChannelWidthToDepthRatioAvg = c.Decimal(precision: 18, scale: 2),
                        WettedSideChannelWidthToDepthRatioCV = c.Decimal(precision: 18, scale: 2),
                        WettedSideChannelWidthToMaxDepthRatioAvg = c.Decimal(precision: 18, scale: 2),
                        WettedSideChannelWidthToMaxDepthRatioCV = c.Decimal(precision: 18, scale: 2),
                        WettedSiteLength = c.Decimal(precision: 18, scale: 2),
                        WettedSmallSideChannelArea = c.Decimal(precision: 18, scale: 2),
                        WettedVolume = c.Decimal(precision: 18, scale: 2),
                        WettedWidthAvg = c.Decimal(precision: 18, scale: 2),
                        WettedWidthCV = c.Decimal(precision: 18, scale: 2),
                        WettedWidthIntegrated = c.Decimal(precision: 18, scale: 2),
                        WettedWidthToDepthRatioAvg = c.Decimal(precision: 18, scale: 2),
                        WettedWidthToDepthRatioCV = c.Decimal(precision: 18, scale: 2),
                        WettedWidthToMaxDepthRatioAvg = c.Decimal(precision: 18, scale: 2),
                        WettedWidthToMaxDepthRatioCV = c.Decimal(precision: 18, scale: 2),
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
                "dbo.VisitMetrics_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AemChampID = c.Int(),
                        VisitYear = c.Int(),
                        ProtocolID = c.Int(),
                        SiteName = c.String(),
                        VisitID = c.Int(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            AddColumn("dbo.LeaseInspections", "ViolationLandAreaCode", c => c.String());
            AddColumn("dbo.LeaseInspections", "ViolationOwnerType", c => c.String());
            AddColumn("dbo.LeaseInspections", "ViolationType", c => c.String());
            AddColumn("dbo.SpawningGroundSurvey_Header", "StrandingIssues", c => c.String());
            AddColumn("dbo.SpawningGroundSurvey_Header", "StrandingComments", c => c.String());
            AlterColumn("dbo.LeaseRevisions", "LeaseStart", c => c.DateTime());
            AlterColumn("dbo.Leases", "LeaseStart", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VisitMetrics_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.VisitMetrics_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.VisitMetrics_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.VisitMetrics_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.VisitMetrics_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Tier2_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Tier2_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Tier2_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Tier2_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Tier2_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Tier1_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Tier1_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Tier1_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Tier1_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Tier1_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.ChannelUnitMetrics_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.ChannelUnitMetrics_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.ChannelUnitMetrics_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.ChannelUnitMetrics_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.ChannelUnitMetrics_Detail", "ActivityId", "dbo.Activities");
            DropIndex("dbo.VisitMetrics_Header", new[] { "ByUserId" });
            DropIndex("dbo.VisitMetrics_Header", new[] { "ActivityId" });
            DropIndex("dbo.VisitMetrics_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.VisitMetrics_Detail", new[] { "ByUserId" });
            DropIndex("dbo.VisitMetrics_Detail", new[] { "ActivityId" });
            DropIndex("dbo.Tier2_Header", new[] { "ByUserId" });
            DropIndex("dbo.Tier2_Header", new[] { "ActivityId" });
            DropIndex("dbo.Tier2_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Tier2_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Tier2_Detail", new[] { "ActivityId" });
            DropIndex("dbo.Tier1_Header", new[] { "ByUserId" });
            DropIndex("dbo.Tier1_Header", new[] { "ActivityId" });
            DropIndex("dbo.Tier1_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Tier1_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Tier1_Detail", new[] { "ActivityId" });
            DropIndex("dbo.ChannelUnitMetrics_Header", new[] { "ByUserId" });
            DropIndex("dbo.ChannelUnitMetrics_Header", new[] { "ActivityId" });
            DropIndex("dbo.ChannelUnitMetrics_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.ChannelUnitMetrics_Detail", new[] { "ByUserId" });
            DropIndex("dbo.ChannelUnitMetrics_Detail", new[] { "ActivityId" });
            AlterColumn("dbo.Leases", "LeaseStart", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LeaseRevisions", "LeaseStart", c => c.DateTime(nullable: false));
            DropColumn("dbo.SpawningGroundSurvey_Header", "StrandingComments");
            DropColumn("dbo.SpawningGroundSurvey_Header", "StrandingIssues");
            DropColumn("dbo.LeaseInspections", "ViolationType");
            DropColumn("dbo.LeaseInspections", "ViolationOwnerType");
            DropColumn("dbo.LeaseInspections", "ViolationLandAreaCode");
            DropTable("dbo.VisitMetrics_Header");
            DropTable("dbo.VisitMetrics_Detail");
            DropTable("dbo.Tier2_Header");
            DropTable("dbo.Tier2_Detail");
            DropTable("dbo.Tier1_Header");
            DropTable("dbo.Tier1_Detail");
            DropTable("dbo.ChannelUnitMetrics_Header");
            DropTable("dbo.ChannelUnitMetrics_Detail");
        }
    }
}
