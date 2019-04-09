CREATE TABLE [dbo].[VisitMetrics_Detail] (
    [Id] [int] NOT NULL IDENTITY,
    [Alkalinity] [int],
    [BankErosion] [decimal](18, 2),
    [BankfullArea] [decimal](18, 2),
    [BankfullChannelBraidedness] [decimal](18, 2),
    [BankfullChannelCount] [int],
    [BankfullChannelIslandCount] [int],
    [BankfullChannelQualifyingIslandArea] [decimal](18, 2),
    [BankfullChannelQualifyingIslandCount] [int],
    [BankfullChannelTotalLength] [decimal](18, 2),
    [BankfullDepthAvg] [decimal](18, 2),
    [BankfullDepthMax] [decimal](18, 2),
    [BankfullMainChannelPartCount] [int],
    [BankfullSideChannelWidth] [decimal](18, 2),
    [BankfullSideChannelWidthCV] [decimal](18, 2),
    [BankfullSideChannelWidthToDepthRatioAvg] [decimal](18, 2),
    [BankfullSideChannelWidthToDepthRatioCV] [decimal](18, 2),
    [BankfullSideChannelWidthToMaxDepthRatioAvg] [decimal](18, 2),
    [BankfullSideChannelWidthToMaxDepthRatioCV] [decimal](18, 2),
    [BankfullSiteLength] [decimal](18, 2),
    [BankfullVolume] [decimal](18, 2),
    [BankfullWidthAvg] [decimal](18, 2),
    [BankfullWidthCV] [decimal](18, 2),
    [BankfullWidthIntegrated] [decimal](18, 2),
    [BankfullWidthToDepthRatioAvg] [decimal](18, 2),
    [BankfullWidthToDepthRatioCV] [decimal](18, 2),
    [BankfullWidthToMaxDepthRatioAvg] [decimal](18, 2),
    [BankfullWidthToMaxDepthRatioCV] [decimal](18, 2),
    [BraidChannelRatio] [decimal](18, 2),
    [Conductivity] [decimal](18, 2),
    [ConstrainingFeatureHeightAverage] [decimal](18, 2),
    [CountOfBrooktrout] [int],
    [CountOfBulltrout] [int],
    [CountOfChinook] [int],
    [CountOfChum] [int],
    [CountOfCoho] [int],
    [CountOfCutthroat] [int],
    [CountOfJamLargeWoodyPieces] [int],
    [CountOfKeyLargeWoodyPieces] [int],
    [CountOfLamprey] [int],
    [CountOfLargeWoodyPieces] [int],
    [CountOfLeftBankLargeWoodyPieces] [int],
    [CountOfMidChannelLargeWoodyPieces] [int],
    [CountOfOmykiss] [int],
    [CountOfOtherSpecies] [int],
    [CountOfPink] [int],
    [CountOfPoolFormingLargeWoodyPieces] [int],
    [CountOfRightBankLargeWoodyPieces] [int],
    [CountOfSockeye] [int],
    [DensityOfBrooktrout] [decimal](18, 2),
    [DensityOfBulltrout] [decimal](18, 2),
    [DensityOfChinook] [decimal](18, 2),
    [DensityOfChum] [decimal](18, 2),
    [DensityOfCoho] [decimal](18, 2),
    [DensityOfCutthroat] [decimal](18, 2),
    [DensityOfLamprey] [decimal](18, 2),
    [DensityOfOmykiss] [decimal](18, 2),
    [DensityOfPink] [decimal](18, 2),
    [DensityOfSockeye] [decimal](18, 2),
    [DetrendedElevationSD] [decimal](18, 2),
    [Discharge] [decimal](18, 2),
    [DriftBiomass] [decimal](18, 2),
    [FastNonTurbulentArea] [decimal](18, 2),
    [FastNonTurbulentCount] [int],
    [FastNonTurbulentFrequency] [decimal](18, 2),
    [FastNonTurbulentPercent] [decimal](18, 2),
    [FastNonTurbulentVolume] [decimal](18, 2),
    [FastTurbulentArea] [decimal](18, 2),
    [FastTurbulentCount] [int],
    [FastTurbulentFrequency] [decimal](18, 2),
    [FastTurbulentPercent] [decimal](18, 2),
    [FastTurbulentVolume] [decimal](18, 2),
    [FishCoverAquaticVegetation] [decimal](18, 2),
    [FishCoverArtificial] [decimal](18, 2),
    [FishCoverLW] [decimal](18, 2),
    [FishCoverNone] [decimal](18, 2),
    [FishCoverTerrestrialVegetation] [decimal](18, 2),
    [FishCoverTotal] [decimal](18, 2),
    [FloodProneWidthAverage] [decimal](18, 2),
    [Gradient] [decimal](18, 2),
    [LargeWoodFrequencyBankfull] [decimal](18, 2),
    [LargeWoodFrequencyWetted] [decimal](18, 2),
    [PercentConstrained] [int],
    [PoolToTurbulentAreaRatio] [decimal](18, 2),
    [PercentUndercutByArea] [decimal](18, 2),
    [PercentUndercutByLength] [decimal](18, 2),
    [ResidualPoolDepth] [decimal](18, 2),
    [RiparianCoverBigTree] [decimal](18, 2),
    [RiparianCoverConiferous] [decimal](18, 2),
    [RiparianCoverGround] [decimal](18, 2),
    [RiparianCoverNoCanopy] [decimal](18, 2),
    [RiparianCoverNonWoody] [decimal](18, 2),
    [RiparianCoverUnderstory] [decimal](18, 2),
    [RiparianCoverWoody] [decimal](18, 2),
    [Sinuosity] [decimal](18, 2),
    [SinuosityViaCenterline] [decimal](18, 2),
    [SlowWaterArea] [decimal](18, 2),
    [SlowWaterCount] [int],
    [SlowWaterFrequency] [decimal](18, 2),
    [SlowWaterPercent] [decimal](18, 2),
    [SlowWaterVolume] [decimal](18, 2),
    [SolarAccessSummerAvg] [decimal](18, 2),
    [SubstrateLt2mm] [decimal](18, 2),
    [SubstrateLt6mm] [decimal](18, 2),
    [SubstrateEstBoulders] [decimal](18, 2),
    [SubstrateEstCoarseAndFineGravel] [decimal](18, 2),
    [SubstrateEstCobbles] [decimal](18, 2),
    [SubstrateEstSandAndFines] [decimal](18, 2),
    [SubstrateD16] [decimal](18, 2),
    [SubstrateD50] [decimal](18, 2),
    [SubstrateD84] [decimal](18, 2),
    [SubstrateEmbeddednessAvg] [decimal](18, 2),
    [SubstrateEmbeddednessSD] [decimal](18, 2),
    [ThalwegDepthAvg] [decimal](18, 2),
    [ThalwegDepthCV] [decimal](18, 2),
    [ThalwegSiteLength] [decimal](18, 2),
    [TotalUndercutArea] [decimal](18, 2),
    [ValleyWidth] [decimal](18, 2),
    [WettedArea] [decimal](18, 2),
    [WettedChannelBraidedness] [decimal](18, 2),
    [WettedChannelCount] [int],
    [WettedChannelIslandCount] [int],
    [WettedChannelMainstemArea] [decimal](18, 2),
    [WettedChannelMainstemLength] [decimal](18, 2),
    [WettedChannelMainstemSinuosity] [decimal](18, 2),
    [WettedChannelQualifyingIslandArea] [decimal](18, 2),
    [WettedChannelQualifyingIslandCount] [int],
    [WettedChannelTotalLength] [decimal](18, 2),
    [WettedDepthSD] [decimal](18, 2),
    [WettedLargeSideChannelArea] [decimal](18, 2),
    [WettedMainChannelPartCount] [int],
    [WettedSideChannelPercentByArea] [decimal](18, 2),
    [WettedSideChannelWidth] [decimal](18, 2),
    [WettedSideChannelWidthCV] [decimal](18, 2),
    [WettedSideChannelWidthToDepthRatioAvg] [decimal](18, 2),
    [WettedSideChannelWidthToDepthRatioCV] [decimal](18, 2),
    [WettedSideChannelWidthToMaxDepthRatioAvg] [decimal](18, 2),
    [WettedSideChannelWidthToMaxDepthRatioCV] [decimal](18, 2),
    [WettedSiteLength] [decimal](18, 2),
    [WettedSmallSideChannelArea] [decimal](18, 2),
    [WettedVolume] [decimal](18, 2),
    [WettedWidthAvg] [decimal](18, 2),
    [WettedWidthCV] [decimal](18, 2),
    [WettedWidthIntegrated] [decimal](18, 2),
    [WettedWidthToDepthRatioAvg] [decimal](18, 2),
    [WettedWidthToDepthRatioCV] [decimal](18, 2),
    [WettedWidthToMaxDepthRatioAvg] [decimal](18, 2),
    [WettedWidthToMaxDepthRatioCV] [decimal](18, 2),
    [RowId] [int] NOT NULL,
    [RowStatusId] [int] NOT NULL,
    [ActivityId] [int] NOT NULL,
    [ByUserId] [int] NOT NULL,
    [QAStatusId] [int] NOT NULL,
    [EffDt] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.VisitMetrics_Detail] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_ActivityId] ON [dbo].[VisitMetrics_Detail]([ActivityId])
CREATE INDEX [IX_ByUserId] ON [dbo].[VisitMetrics_Detail]([ByUserId])
CREATE INDEX [IX_QAStatusId] ON [dbo].[VisitMetrics_Detail]([QAStatusId])
CREATE TABLE [dbo].[VisitMetrics_Header] (
    [Id] [int] NOT NULL IDENTITY,
    [AemChampID] [int],
    [VisitYear] [int],
    [ProtocolID] [int],
    [SiteName] [nvarchar](max),
    [VisitID] [int],
    [ActivityId] [int] NOT NULL,
    [ByUserId] [int] NOT NULL,
    [EffDt] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.VisitMetrics_Header] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_ActivityId] ON [dbo].[VisitMetrics_Header]([ActivityId])
CREATE INDEX [IX_ByUserId] ON [dbo].[VisitMetrics_Header]([ByUserId])

ALTER TABLE [dbo].[VisitMetrics_Detail] ADD CONSTRAINT [FK_dbo.VisitMetrics_Detail_dbo.Activities_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activities] ([Id])
ALTER TABLE [dbo].[VisitMetrics_Detail] ADD CONSTRAINT [FK_dbo.VisitMetrics_Detail_dbo.Users_ByUserId] FOREIGN KEY ([ByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[VisitMetrics_Detail] ADD CONSTRAINT [FK_dbo.VisitMetrics_Detail_dbo.QAStatus_QAStatusId] FOREIGN KEY ([QAStatusId]) REFERENCES [dbo].[QAStatus] ([Id])
ALTER TABLE [dbo].[VisitMetrics_Header] ADD CONSTRAINT [FK_dbo.VisitMetrics_Header_dbo.Activities_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activities] ([Id])
ALTER TABLE [dbo].[VisitMetrics_Header] ADD CONSTRAINT [FK_dbo.VisitMetrics_Header_dbo.Users_ByUserId] FOREIGN KEY ([ByUserId]) REFERENCES [dbo].[Users] ([Id])
go

--Add the views
--Up
Drop view dbo.VisitMetrics_Detail_VW
go
create view dbo.VisitMetrics_Detail_VW
as
SELECT        Id, Alkalinity, BankErosion, BankfullArea, BankfullChannelBraidedness, BankfullChannelCount, BankfullChannelIslandCount, BankfullChannelQualifyingIslandArea, BankfullChannelQualifyingIslandCount, 
                         BankfullChannelTotalLength, BankfullDepthAvg, BankfullDepthMax, BankfullMainChannelPartCount, BankfullSideChannelWidth, BankfullSideChannelWidthCV, BankfullSideChannelWidthToDepthRatioAvg, 
                         BankfullSideChannelWidthToMaxDepthRatioAvg, BankfullSideChannelWidthToMaxDepthRatioCV, BankfullSiteLength, BankfullVolume, BankfullWidthAvg, BankfullWidthCV, BankfullWidthIntegrated, 
                         BankfullWidthToDepthRatioAvg, BankfullWidthToDepthRatioCV, BankfullWidthToMaxDepthRatioAvg, BankfullWidthToMaxDepthRatioCV, BraidChannelRatio, Conductivity, ConstrainingFeatureHeightAverage, CountOfBrooktrout, 
                         CountOfBulltrout, CountOfChinook, CountOfChum, CountOfCoho, CountOfCutthroat, CountOfJamLargeWoodyPieces, CountOfKeyLargeWoodyPieces, CountOfLamprey, CountOfLargeWoodyPieces, 
                         CountOfLeftBankLargeWoodyPieces, CountOfMidChannelLargeWoodyPieces, CountOfOmykiss, CountOfOtherSpecies, CountOfPink, CountOfPoolFormingLargeWoodyPieces, CountOfRightBankLargeWoodyPieces, 
                         CountOfSockeye, DensityOfBrooktrout, DensityOfBulltrout, DensityOfChinook, DensityOfChum, DensityOfCoho, DensityOfCutthroat, DensityOfLamprey, DensityOfOmykiss, DensityOfPink, DensityOfSockeye, 
                         DetrendedElevationSD, Discharge, DriftBiomass, FastNonTurbulentArea, FastNonTurbulentCount, FastNonTurbulentFrequency, FastNonTurbulentPercent, FastNonTurbulentVolume, FastTurbulentArea, FastTurbulentCount, 
                         FastTurbulentFrequency, FastTurbulentPercent, FastTurbulentVolume, FishCoverAquaticVegetation, FishCoverArtificial, FishCoverLW, FishCoverNone, FishCoverTerrestrialVegetation, FishCoverTotal, FloodProneWidthAverage, 
                         Gradient, LargeWoodFrequencyBankfull, LargeWoodFrequencyWetted, PercentConstrained, PoolToTurbulentAreaRatio, PercentUndercutByArea, PercentUndercutByLength, ResidualPoolDepth, RiparianCoverBigTree, 
                         RiparianCoverConiferous, RiparianCoverGround, RiparianCoverNoCanopy, RiparianCoverNonWoody, RiparianCoverUnderstory, RiparianCoverWoody, Sinuosity, SinuosityViaCenterline, SlowWaterArea, SlowWaterCount, 
                         SlowWaterFrequency, SlowWaterPercent, SlowWaterVolume, SolarAccessSummerAvg, SubstrateLt2mm, SubstrateLt6mm, SubstrateEstBoulders, SubstrateEstCoarseAndFineGravel, SubstrateEstCobbles, 
                         SubstrateEstSandAndFines, SubstrateD16, SubstrateD50, SubstrateD84, SubstrateEmbeddednessAvg, SubstrateEmbeddednessSD, ThalwegDepthAvg, ThalwegDepthCV, ThalwegSiteLength, TotalUnderCutArea, ValleyWidth, 
                         WettedArea, WettedChannelBraidedness, WettedChannelIslandCount, WettedChannelMainstemArea, WettedChannelMainstemLength, WettedChannelMainstemSinuosity, WettedChannelQualifyingIslandArea, 
                         WettedChannelQualifyingIslandCount, WettedChannelTotalLength, WettedDepthSD, WettedLargeSideChannelArea, WettedMainChannelPartCount, WettedSideChannelPercentByArea, WettedSideChannelWidth, 
                         WettedSideChannelWidthCV, WettedSideChannelWidthToDepthRatioAvg, WettedSideChannelWidthToDepthRatioCV, WettedSideChannelWidthToMaxDepthRatioAvg, WettedSideChannelWidthToMaxDepthRatioCV, 
                         WettedSiteLength, WettedSmallSideChannelArea, WettedVolume, WettedWidthAvg, WettedWidthCV, WettedWidthIntegrated, WettedWidthToDepthRatioAvg, WettedWidthToDepthRatioCv, 
                         WettedWidthToMaxDepthRatioAvg, WettedWidthToMaxDepthRatioCV, RowId, RowStatusId, ActivityId, ByUserId, EffDt, QAStatusId
FROM            dbo.VisitMetrics_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.VisitMetrics_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
go


drop view dbo.VisitMetrics_Header_VW
go
create view dbo.VisitMetrics_Header_VW
AS
SELECT        Id, AemChampID, VisitYear, ProtocolID, SiteName, VisitID, ByUserId, EffDt, ActivityId
FROM            dbo.VisitMetrics_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.VisitMetrics_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
go


drop view dbo.VisitMetrics_vw
go
create view dbo.VisitMetrics_vw
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, aq.UserId AS ActivityQAUserId, aq.Comments, aq.QAStatusName, aq.QAStatusId AS ActivityQAStatusId, h.Id, 
                         h.AemChampID, h.VisitYear, h.ProtocolID, h.SiteName, h.VisitID, h.ByUserId, h.EffDt, d.Id AS VisitMetrics_Detail_Id, d.Alkalinity, d.BankErosion, d.BankfullArea, d.BankfullChannelBraidedness, d.BankfullChannelCount, 
                         d.BankfullChannelIslandCount, d.BankfullChannelQualifyingIslandArea, d.BankfullChannelQualifyingIslandCount, d.BankfullChannelTotalLength, d.BankfullDepthAvg, d.BankfullDepthMax, d.BankfullMainChannelPartCount, 
                         d.BankfullSideChannelWidth, d.BankfullSideChannelWidthCV, d.BankfullSideChannelWidthToDepthRatioAvg, d.BankfullSideChannelWidthToMaxDepthRatioAvg, d.BankfullSideChannelWidthToMaxDepthRatioCV, 
                         d.BankfullSiteLength, d.BankfullVolume, d.BankfullWidthAvg, d.BankfullWidthCV, d.BankfullWidthIntegrated, d.BankfullWidthToDepthRatioAvg, d.BankfullWidthToDepthRatioCV, d.BankfullWidthToMaxDepthRatioAvg, 
                         d.BankfullWidthToMaxDepthRatioCV, d.BraidChannelRatio, d.Conductivity, d.ConstrainingFeatureHeightAverage, d.CountOfBrooktrout, d.CountOfBulltrout, d.CountOfChinook, d.CountOfChum, d.CountOfCoho, d.CountOfCutthroat, 
                         d.CountOfJamLargeWoodyPieces, d.CountOfKeyLargeWoodyPieces, d.CountOfLamprey, d.CountOfLargeWoodyPieces, d.CountOfLeftBankLargeWoodyPieces, d.CountOfMidChannelLargeWoodyPieces, d.CountOfOmykiss, 
                         d.CountOfOtherSpecies, d.CountOfPink, d.CountOfPoolFormingLargeWoodyPieces, d.CountOfRightBankLargeWoodyPieces, d.CountOfSockeye, d.DensityOfBrooktrout, d.DensityOfBulltrout, d.DensityOfChinook, d.DensityOfChum, 
                         d.DensityOfCoho, d.DensityOfCutthroat, d.DensityOfLamprey, d.DensityOfOmykiss, d.DensityOfPink, d.DensityOfSockeye, d.DetrendedElevationSD, d.Discharge, d.DriftBiomass, d.FastNonTurbulentArea, 
                         d.FastNonTurbulentCount, d.FastNonTurbulentFrequency, d.FastNonTurbulentPercent, d.FastNonTurbulentVolume, d.FastTurbulentArea, d.FastTurbulentCount, d.FastTurbulentFrequency, d.FastTurbulentPercent, 
                         d.FastTurbulentVolume, d.FishCoverAquaticVegetation, d.FishCoverArtificial, d.FishCoverLW, d.FishCoverNone, d.FishCoverTerrestrialVegetation, d.FishCoverTotal, d.FloodProneWidthAverage, d.Gradient, 
                         d.LargeWoodFrequencyBankfull, d.LargeWoodFrequencyWetted, d.PercentConstrained, d.PoolToTurbulentAreaRatio, d.PercentUndercutByArea, d.PercentUndercutByLength, d.ResidualPoolDepth, d.RiparianCoverBigTree, 
                         d.RiparianCoverConiferous, d.RiparianCoverGround, d.RiparianCoverNoCanopy, d.RiparianCoverNonWoody, d.RiparianCoverUnderstory, d.RiparianCoverWoody, d.Sinuosity, d.SinuosityViaCenterline, d.SlowWaterArea, 
                         d.SlowWaterCount, d.SlowWaterFrequency, d.SlowWaterPercent, d.SlowWaterVolume, d.SolarAccessSummerAvg, d.SubstrateLt2mm, d.SubstrateLt6mm, d.SubstrateEstBoulders, d.SubstrateEstCoarseAndFineGravel, 
                         d.SubstrateEstCobbles, d.SubstrateEstSandAndFines, d.SubstrateD16, d.SubstrateD50, d.SubstrateD84, d.SubstrateEmbeddednessAvg, d.SubstrateEmbeddednessSD, d.ThalwegDepthAvg, d.ThalwegDepthCV, 
                         d.ThalwegSiteLength, d.TotalUnderCutArea, d.ValleyWidth, d.WettedArea, d.WettedChannelBraidedness, d.WettedChannelIslandCount, d.WettedChannelMainstemArea, d.WettedChannelMainstemLength, 
                         d.WettedChannelMainstemSinuosity, d.WettedChannelQualifyingIslandArea, d.WettedChannelQualifyingIslandCount, d.WettedChannelTotalLength, d.WettedDepthSD, d.WettedLargeSideChannelArea, 
                         d.WettedMainChannelPartCount, d.WettedSideChannelPercentByArea, d.WettedSideChannelWidth, d.WettedSideChannelWidthCV, d.WettedSideChannelWidthToDepthRatioAvg, d.WettedSideChannelWidthToDepthRatioCV, 
                         d.WettedSideChannelWidthToMaxDepthRatioAvg, d.WettedSideChannelWidthToMaxDepthRatioCV, d.WettedSiteLength, d.WettedSmallSideChannelArea, d.WettedVolume, d.WettedWidthAvg, d.WettedWidthCV, 
                         d.WettedWidthIntegrated, d.WettedWidthToDepthRatioAvg, d.WettedWidthToDepthRatioCv, d.WettedWidthToMaxDepthRatioAvg, d.WettedWidthToMaxDepthRatioCV, d.RowId, d.ByUserId AS VisitMetrics_Detail_ByUserId, 
                         d.EffDt AS VisitMetrics_Detail_EffDt, d.QAStatusId, l.Label
FROM            dbo.Activities AS a INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId INNER JOIN
                         dbo.VisitMetrics_Header_VW AS h ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.VisitMetrics_Detail_VW AS d ON h.ActivityId = d.ActivityId INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id			   
go					   


--Add the data
declare @datasetBaseName as varchar(max) = 'Visit Metrics'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = @datasetBaseName

-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 2249 

-- Grab the project names
update #ProjectInfo set name = (select name from projects where projects.id = #ProjectInfo.id)

create table #NewLocationTypeIds (id int)

-- Add record to LocationTypes
INSERT into dbo.LocationTypes([Name], [Description])
OUTPUT INSERTED.id into #NewLocationTypeIds
values (@datasetBaseName, @datasetBaseName)

-- Add records to the Datastores
INSERT INTO dbo.Datastores (Name, Description, TablePrefix, OwnerUserId, LocationTypeId, DefaultConfig) 
SELECT
	Name               = @datastoreName, 
	Description        = NULL, 
	TablePrefix        = REPLACE(@datastoreName, ' ', ''), -- Strip spaces 
	OwnerUserId        = 1081,      -- George
	LocationTypeId     = lt.id,
	DefaultConfig	   = '{}'
from #NewLocationTypeIds as lt


CREATE TABLE #NewDatasetIds (id int)        -- This will contain a list of ids of all dataset records inserted below

-- Add record to the Datasets --> will create one record per project
INSERT INTO	Datasets (ProjectId, DefaultRowQAStatusId, StatusId, CreateDateTime, Name, Description, DefaultActivityQAStatusId, DatastoreId, Config)
OUTPUT INSERTED.id into #NewDatasetIds
SELECT 
    ProjectId                 = p.id,
    DefaultRowQAStatusId      = 1,
    StatusId                  = 1,
    CreateDateTime            = GetDate(),
    Name                      = @datasetBaseName,
    Description               = @datasetBaseName + ': ' + p.name,
    DefaultActivityQAStatusId = 6,
    DatastoreId               = (SELECT IDENT_CURRENT('dbo.Datastores')),
    Config                    = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}'
FROM #ProjectInfo as p


-- This will info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    

-----
-- Insert any new fields we'll need (George says don't reuse fields)
-- Header Fields
INSERT INTO dbo.Fields (Name, [Description], Units, Validation, DataType, PossibleValues, [Rule], DbColumnName, ControlType, DatastoreId)
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType, INSERTED.[Rule], NULL INTO #NewFieldInfo
SELECT
		Name = 'AemChampID',
		Description = 'AEM Champ Ident',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'AemChampID',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'VisitYear',
		Description = 'Year of the activity',
		Units = 'YYYY',
		Validation = 'i4',
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'VisitYear',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'ProtocolID',
		Description = 'Number of the protocol used for that visit',
		Units = NULL,
		Validation = NULL,
		DataType = 'select',
		PossibleValues = '[413, 806, 1870, 1875, 1877, 1955, 1966, 2020, 2030, 2038, 2044, 9999, 10001]',
		[Rule] = NULL,
		DbColumnName = 'ProtocolID',
		ControlType = 'select',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'SiteName',
		Description = 'AEM Champ Number plus Ctuir site name',
		Units = NULL,
		Validation = NULL,
		DataType = 'string',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SiteName',
		ControlType = 'text',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'VisitID',
		Description = 'Number assigned to the activity',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'VisitID',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))


update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header


--Details Fields

INSERT INTO dbo.Fields(Name, [Description], Units, Validation, DataType, PossibleValues, [Rule], DbColumnName, ControlType, DatastoreId)
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType, INSERTED.[Rule], NULL INTO #NewFieldInfo
SELECT
		Name = 'Alkalinity',
		Description = 'Measure of the capacity of water to neutralize an acid (buffering capacity).',
		Units = 'ppm',
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'Alkalinity',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bank Erosion',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankErosion',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Area',
		Description = 'The total bankfull area of a site.',
		Units = 'm2',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullArea',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Channel Braidedness',
		Description = 'Ratio of the total length of the bankfull mainstem plus side channels and the length of the mainstem channel.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullChannelBraidedness',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Channel Count',
		Description = 'Total number of channel segments (mainstem + side channels) identified in the bankfull channel.',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullChannelCount',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Channel Island Count',
		Description = 'The total number of islands in the bankfull polygon.',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullChannelIslandCount',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Channel Qualifying Island Area',
		Description = 'The total area of all qualifying islands in the bankfull  polygon.',
		Units = 'm2',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullChannelQualifyingIslandArea',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Channel Qualifying Island Count',
		Description = 'The number of qualifying islands in the bankfull polygon.',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullChannelQualifyingIslandCount',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Channel Total Length',
		Description = 'Total length of the main channel and all side channels at bankfull.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullChannelTotalLength',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Depth Avg',
		Description = 'Average depth of the bankfull channel.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullDepthAvg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Depth Max',
		Description = 'Maximum depth of the bankfull channel.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullDepthMax',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Main Channel Part Count',
		Description = 'Total count of bankfull mainstem channel segments.',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullMainChannelPartCount',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Side Channel Width',
		Description = 'Average bankfull width of side channel(s) measured from cross-sections.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullSideChannelWidth',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Side Channel Width CV',
		Description = 'Coefficient of Variation of qualifying side channel bankfull widths measured from cross-sections.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullSideChannelWidthCV',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Side Channel Width to Depth Ratio Avg',
		Description = 'Width to average depth ratio in side channels at bankfull.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullSideChannelWidthToDepthRatioAvg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Side Channel Width to Depth Ratio CV',
		Description = 'Coefficient of Variation of width to average depth ratio in side channels at bankfull.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullSideChannelWidthToDepthRatioCV',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Side Channel Width to Max Depth Ratio Avg',
		Description = 'Width to max depth ratio in side channels at bankfull.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullSideChannelWidthToMaxDepthRatioAvg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Side Channel Width to Max Depth Ratio CV',
		Description = 'Coefficient of Variation of width to max depth ratio in side channels at bankfull.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullSideChannelWidthToMaxDepthRatioCV',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Site Length',
		Description = 'The total length of the bankfull centerline derived from the bankfull polygon.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullSiteLength',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Volume',
		Description = 'Total bankfull volume at a site.',
		Units = 'm3',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullVolume',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Width Avg',
		Description = 'Average width of the bankfull channel measured from cross-sections.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullWidthAvg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Width CV',
		Description = 'Coefficient of Variation of the bankfull channel width measured from cross-sections.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullWidthCV',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Width Integrated',
		Description = 'Average width of the bankfull polygon for a site.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullWidthIntegrated',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Width To Depth Ratio Avg',
		Description = 'Average width to depth ratios of the bankfull channel measured from cross-sections. Depths represent an average of 10 depths along each cross-section.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullWidthToDepthRatioAvg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Width To Depth Ratio CV',
		Description = 'Coefficient of Variation of width to depth ratios of the bankfull channel measured from cross-sections. Depths represent an average of 10 depths along each cross-section.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullWidthToDepthRatioCV',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Width to Max Depth Ratio Avg',
		Description = 'Average width to depth ratios of the bankfull channel measured from cross-sections. Depth represents the maximum depth along a cross-section.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullWidthToMaxDepthRatioAvg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Bankfull Width to Max Depth Ratio CV',
		Description = 'Coefficient of Variation of width to depth ratios of the bankfull channel measured from cross-sections. Depth represents the maximum depth along a cross-section.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BankfullWidthToMaxDepthRatioCV',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Braid Channel Ratio',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'BraidChannelRatio',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Conductivity',
		Description = 'Measure of the concentration of ionized materials in water, or the ability of water to conduct electrical current.',
		Units = 'µS/m',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'Conductivity',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Constraining Feature Height Average',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'ConstrainingFeatureHeightAverage',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Brooktrout',
		Description = 'from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfBrooktrout',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Bulltrout',
		Description = 'from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfBulltrout',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Chinook',
		Description = 'from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfChinook',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Chum',
		Description = 'from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfChum',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Coho',
		Description = 'from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfCoho',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Cutthroat',
		Description = 'from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfCutthroat',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Jam Large Woody Pieces',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfJamLargeWoodyPieces',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Key Large Woody Pieces',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfKeyLargeWoodyPieces',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Lamprey',
		Description = 'from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfLamprey',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Large Woody Pieces',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfLargeWoodyPieces',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Left Bank Large Woody Pieces',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfLeftBankLargeWoodyPieces',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Mid Channel Large Woody Pieces',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfMidChannelLargeWoodyPieces',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Omykiss',
		Description = 'from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfOmykiss',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Other Species',
		Description = 'from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfOtherSpecies',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Pink',
		Description = 'from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfPink',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Pool Forming Large Woody Pieces',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfPoolFormingLargeWoodyPieces',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Right Bank Large Woody Pieces',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfRightBankLargeWoodyPieces',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Count of Sockeye',
		Description = 'from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfSockeye',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Density of Brooktrout',
		Description = 'calculated from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfBrooktrout',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Density of Bulltrout',
		Description = 'calculated from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfBulltrout',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Density of Chinook',
		Description = 'calculated from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfChinook',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Density of Chum',
		Description = 'calculated from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfChum',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Density of Coho',
		Description = 'calculated from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfCoho',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Density of Cutthroat',
		Description = 'calculated from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfCutthroat',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Density of Lamprey',
		Description = 'calculated from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfLamprey',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Density of Omykiss',
		Description = 'calculated from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfOmykiss',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Density of Pink',
		Description = 'calculated from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfPink',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Density of Sockeye',
		Description = 'calculated from Snorkel',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfSockeye',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Detrended Elevation SD',
		Description = 'Standard Deviation of all cells for the detrended DEM.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DetrendedElevationSD',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Discharge',
		Description = 'The sum of station discharge across all  stations.  Station discharge is calculated as depth x velocity x station increment for all stations except first and last.  Station discharge for first and last station is 0.5 x station width x depth x velocity.',
		Units = 'm3/s',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'Discharge',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Drift Biomass',
		Description = 'Total biomass of invertebrates from drift samples.',
		Units = 'g/m3',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DriftBiomass',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Fast NonTurbulent Area',
		Description = 'Total wetted surface area identified as Fast Water Non-Turbulent channel units.',
		Units = 'm2',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FastNonTurbulentArea',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Fast NonTurbulent Count',
		Description = 'Number of  Fast Water Non-Turbulent channel units at a site.',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FastNonTurbulentCount',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Fast NonTurbulent Frequency',
		Description = 'Number of Fast Water Non-Turbulent channel units per 100 meters.',
		Units = 'count/100m',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FastNonTurbulentFrequency',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Fast NonTurbulent Percent',
		Description = 'Percent of wetted area identified as Fast Water Non-Turbulent channel units.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FastNonTurbulentPercent',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Fast NonTurbulent Volume',
		Description = 'Total wetted volume of all Fast Water Non-Turbulent channel units at a site.',
		Units = 'm3',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FastNonTurbulentVolume',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Fast Turbulent Area',
		Description = 'Total wetted surface area identified as Fast Water Turbulent channel units.',
		Units = 'm2',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FastTurbulentArea',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Fast Turbulent Count',
		Description = 'Number of  Fast Water Turbulent channel units at a site.',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FastTurbulentCount',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Fast Turbulent Frequency',
		Description = 'Number of Fast Water Turbulent channel units per 100 meters.',
		Units = 'count/100m',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FastTurbulentFrequency',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Fast Turbulent Percent',
		Description = 'Percent of wetted area identified as Fast Water Turbulent channel units.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FastTurbulentPercent',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Fast Turbulent Volume',
		Description = 'Total wetted volume of all Fast Water Turbulent channel units at a site.',
		Units = 'm3',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FastTurbulentVolume',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Fish Cover: Aquatic Vegetation',
		Description = 'Percent of wetted area that has aquatic vegetation as fish cover.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FishCoverAquaticVegetation',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Fish Cover: Artificial',
		Description = 'Percent of wetted area that has artificial structure as fish cover.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FishCoverArtificial',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Fish Cover: LW',
		Description = 'Percent of wetted area that has woody debris as fish cover.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FishCoverLW',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Fish Cover: None',
		Description = 'Percent of channel unit with no fish cover.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FishCoverNone',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Fish Cover: Terrestrial Vegetation',
		Description = 'Percent of wetted area that has live terrestrial vegetation as fish cover.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FishCoverTerrestrialVegetation',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Fish Cover: Total',
		Description = 'Percent of wetted area with the following types of cover: aquatic vegetation, artificial, woody debris, and terrestrial vegetation.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FishCoverTotal',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Flood Prone Width Average',
		Description = 'The Stream width at a discharge level defined as twice the maximum Bankfull Depth',
		Units = 'm2',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FloodProneWidthAverage',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Gradient',
		Description = 'Site water surface gradient is calculated as the difference between the top of site (upstream) and bottom of site (downstream) water surface elevations divided by thalweg length.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'Gradient',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Large Wood Frequency: Bankfull',
		Description = 'Number of  large wood pieces per 100 meters within the bankfull channel.',
		Units = 'count/100m',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'LargeWoodFrequencyBankfull',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Large Wood Frequency: Wetted',
		Description = 'Number of  large wood pieces per 100 meters within the wetted channel.',
		Units = 'count/100m',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'LargeWoodFrequencyWetted',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Percent Constrained',
		Description = 'Need definition',
		Units = '%',
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'PercentConstrained',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Pool-To-Turbulent Area Ratio',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'PoolToTurbulentAreaRatio',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Percent Undercut by Area',
		Description = 'The percent of the wetted channel area with undercut banks.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'PercentUndercutByArea',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Percent Undercut by Length',
		Description = 'The percent of the wetted streambank length that is undercut.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'PercentUndercutByLength',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Residual Pool Depth',
		Description = 'The average difference between the maximum depth and downstream end depth of all Slow Water Pool channel units.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'ResidualPoolDepth',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Riparian Cover: Big Tree',
		Description = 'Percent aerial coverage from big trees (>0.3 m DBH) in the canopy.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'RiparianCoverBigTree',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Riparian Cover: Coniferous',
		Description = 'Percent of coniferous cover in the canopy and understory.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'RiparianCoverConiferous',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Riparian Cover: Ground',
		Description = 'Percent  of groundcover vegetation.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'RiparianCoverGround',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Riparian Cover: No Canopy',
		Description = 'Percent of riparian canopy devoid of vegetation.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'RiparianCoverNoCanopy',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Riparian Cover: Non-Woody',
		Description = 'Percent of non-woody vegetation cover in the understory and groundcover.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'RiparianCoverNonWoody',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Riparian Cover: Understory',
		Description = 'Percent of understory vegetation cover.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'RiparianCoverUnderstory',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Riparian Cover: Woody',
		Description = 'Percent of woody vegetation cover in the canopy, understory, and groundcover.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'RiparianCoverWoody',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Sinuosity',
		Description = 'Ratio of the thalweg length to the straight line distance between the start and end points of the thalweg.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'Sinuosity',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Sinuosity Via Centerline',
		Description = 'Ratio of the wetted centerline length (Site Length Wetted) and the straight line distance between the start and end points of the wetted centerline. Generated by the River Bathymetry Toolkit (RBT)',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SinuosityViaCenterline',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Slow Water Area',
		Description = 'Total wetted surface area identified as Slow Water Pool channel units.',
		Units = 'm2',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SlowWaterArea',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Slow Water Count',
		Description = 'Number of  Slow Water Pool channel units at a site.',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SlowWaterCount',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Slow Water Frequency',
		Description = 'Number of Slow Water Poolt channel units per 100 meters.',
		Units = 'count/100m',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SlowWaterFrequency',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Slow Water Percent',
		Description = 'Percent of wetted area identified asSlow Water Pool channel units.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SlowWaterPercent',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Slow Water Volume',
		Description = 'Total wetted volume of all Slow Water Pool channel units at a site.',
		Units = 'm3',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SlowWaterVolume',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Solar Access: Summer Avg',
		Description = 'Average available solar radiation from July-September.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SolarAccessSummerAvg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Substrate < 2mm',
		Description = 'Average percentage of pool tail substrates comprised of fine sediment <2 mm.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SubstrateLt2mm',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Substrate < 6mm',
		Description = 'Average percentage of pool tail substrates comprised of sediment <6 mm.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SubstrateLt6mm',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Substrate Est: Boulders',
		Description = 'Percent of boulders (256-4000 mm) within the wetted site area.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SubstrateEstBoulders',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Substrate Est: Coarse and Fine Gravel',
		Description = 'Percent of coarse and fine gravel (2-64 mm) within the wetted site area.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SubstrateEstCoarseAndFineGravel',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Substrate Est: Cobbles',
		Description = 'Percent of cobbles (64-256 mm) within the wetted site area.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SubstrateEstCobbles',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Substrate Est: Sand and Fines',
		Description = 'Percent of sand and fine sediment (0.01-2 mm) within the wetted site area.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SubstrateEstSandAndFines',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Substrate: D16',
		Description = 'Diameter of the 16th percentile particle derived from pebble counts.',
		Units = 'mm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SubstrateD16',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Substrate: D50',
		Description = 'Diameter of the 50th percentile particle derived from pebble counts.',
		Units = 'mm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SubstrateD50',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Substrate: D84',
		Description = 'Diameter of the 84th percentile particle derived from pebble counts.',
		Units = 'mm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SubstrateD84',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Substrate: Embeddedness Avg',
		Description = 'Average cobble embeddedness.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SubstrateEmbeddednessAvg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Substrate: Embeddedness SD',
		Description = 'Standard Deviation of cobble embeddedness.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SubstrateEmbeddednessSD',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Thalweg Depth Avg',
		Description = 'Average thalweg depth of the wetted channel.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'ThalwegDepthAvg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Thalweg Depth CV',
		Description = 'Coefficient of Variation (CV) of thalweg depths at a site.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'ThalwegDepthCV',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Thalweg Site Length',
		Description = 'The total length of the thalweg at a site.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'ThalwegSiteLength',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Total Undercut Area',
		Description = 'The total area of undercut banks.  Measured as the length*width of each undercut, then summed across the site',
		Units = 'm2',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'TotalUndercutArea',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Valley Width',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'ValleyWidth',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Area',
		Description = 'The total wetted area of a site.',
		Units = 'm2',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedArea',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Channel Braidedness',
		Description = 'Ratio of the total length of the wetted mainstem channel plus side channels and the length of the mainstem channel.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedChannelBraidedness',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Channel Count',
		Description = 'Total number of channel segments (mainstem + side channels) identified in the wetted channel.',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedChannelCount',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Channel Island Count',
		Description = 'The total number of islands in the wetted polygon.',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedChannelIslandCount',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Channel Mainstem Area',
		Description = 'Sum of the wetted channel unit areas for all channel units that do not possess a type of "small side channel" or "large side channel".',
		Units = 'm2',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedChannelMainstemArea',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Channel Mainstem Length',
		Description = 'Length of the main wetted channel.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedChannelMainstemLength',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Channel Mainstem Sinuosity',
		Description = 'Length of the wetted main channel divided by the straight line length between the top and bottom endpoints of the wetted main channel centerline.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedChannelMainstemSinuosity',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Channel Qualifying Island Area',
		Description = 'The total area of all qualifying islands in the wetted polygon.',
		Units = 'm2',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedChannelQualifyingIslandArea',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Channel Qualifying Island Count',
		Description = 'The number of qualifying islands in the wetted polygon.',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedChannelQualifyingIslandCount',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Channel Total Length',
		Description = 'Total length of wetted main and side channels.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedChannelTotalLength',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Depth SD',
		Description = 'Standard Deviation of water depths within the wetted channel.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedDepthSD',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Large Side Channel Area',
		Description = 'Sum of the large side channel unit areas.',
		Units = 'm2',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedLargeSideChannelArea',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Main Channel Part Count',
		Description = 'Total count of wetted mainstem channel segments.',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedMainChannelPartCount',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Side Channel Percent By Area',
		Description = 'Ratio of the total area of side channel unit areas (both small and large) divided by the total area of channel unit polygons.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedSideChannelPercentByArea',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Side Channel Width',
		Description = 'Average wetted width of side channel(s)  measured from cross-sections.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedSideChannelWidth',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Side Channel Width CV',
		Description = 'Coefficient of Variation of qualifying side channel wetted widths measured from cross-sections.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedSideChannelWidthCV',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Side Channel Width to Depth Ratio Avg',
		Description = 'Average wetted width to depth ratio of qualifying side channels measured from from cross-sections.  Depths represent the average depth of 10 stations along each cross-section.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedSideChannelWidthToDepthRatioAvg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Side Channel Width to Depth Ratio CV',
		Description = 'Coefficient of variation of qualifying wetted side channel width to average depth ratios measured from cross sections.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedSideChannelWidthToDepthRatioCV',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Side Channel Width to Max Depth Ratio Avg',
		Description = 'Average wetted width to max depth ratio of qualifying side channels measured from cross-sections. Depths represent the max depth of 10 stations along each cross-section.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedSideChannelWidthToMaxDepthRatioAvg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Side Channel Width to Max Depth Ratio CV',
		Description = 'Coefficient of variation of  qualifying wetted width to max depth ratios of side channels measured from cross-sections.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedSideChannelWidthToMaxDepthRatioCV',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Site Length',
		Description = 'The total length of the wetted centerline derived from the wetted polygon.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedSiteLength',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Small Side Channel Area',
		Description = 'Sum of all small side channel unit areas at a site.',
		Units = 'm2',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedSmallSideChannelArea',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Volume',
		Description = 'Total wetted volume at a site.',
		Units = 'm3',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedVolume',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Width Avg',
		Description = 'Average width of the wetted channel measured from cross-sections.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedWidthAvg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Width CV',
		Description = 'Coefficient of Variation of the wetted channel width measured from cross-sections.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedWidthCV',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Width Integrated',
		Description = 'Average width of the wetted polygon for a site.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedWidthIntegrated',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Width To Depth Ratio Avg',
		Description = 'Average width to depth ratio of the wetted channel measured from cross-sections. Depths represent an average of depths along each cross-section.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedWidthToDepthRatioAvg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Width To Depth Ratio CV',
		Description = 'Retired. Coefficient of Variation of wetted width to depth ratios derived from cross-sections.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedWidthToDepthRatioCV',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Width to Max Depth Ratio Avg',
		Description = 'Average width to depth ratios of the wetted channel measured from cross-sections. Depth represents the maximum depth along a cross-section.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedWidthToMaxDepthRatioAvg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Wetted Width to Max Depth Ratio CV',
		Description = 'Coefficient of Variation of width to depth ratios of the wetted channel measured from cross-sections. Depth represents the maximum depth along a cross-section.',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WettedWidthToMaxDepthRatioCV',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))


update #NewFieldInfo set FieldRoleId = 2 where FieldRoleId is NULL   -- 2 == details

-- Assign new fields to the datasets -- this will insert a new row for each combination of datasetId and fieldId for the records inserted above
INSERT INTO dbo.DatasetFields(DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, Validation, SourceId, InstrumentId, OrderIndex, ControlType, [Rule])
SELECT
    DatasetId      = d.id,
    FieldId        = f.id,
    FieldRoleId    = f.FieldRoleId,
    CreateDateTime = GetDate(),
    Label          = f.fieldName,
    DbColumnName   = f.DbColumnName,
    Validation     = f.Validation,
    SourceId       = 1,
    InstrumentId   = NULL,
    OrderIndex     = f.OrderIndex * 10,     -- x10 to make it easier to insert intermediate orders
    ControlType    = f.ControlType,
    [Rule]         = f.[Rule]
FROM #NewDatasetIds as d, #NewFieldInfo as f

-- Add some new DatasetQAStatus records for our new datasets
CREATE TABLE #QaStatusIds (id int)
INSERT INTO #QaStatusIds (id) 
          SELECT id = 5     -- Approved
UNION ALL SELECT id = 6     -- Ready for QA


INSERT INTO dbo.DatasetQAStatus(Dataset_Id, QAStatus_id)
SELECT
    Dataset_Id  = d.id,
    QAStatus_id = q.id
FROM #NewDatasetIds as d, #QaStatusIds as q


INSERT INTO dbo.DatasetQAStatus1(Dataset_Id, QAStatus_id)
SELECT
    Dataset_Id  = d.id,
    QAStatus_id = q.id
FROM #NewDatasetIds as d, #QaStatusIds as q

-- Cleanup
drop table #ProjectInfo
drop table #NewLocationTypeIds
drop table #NewFieldInfo
drop table #QaStatusIds