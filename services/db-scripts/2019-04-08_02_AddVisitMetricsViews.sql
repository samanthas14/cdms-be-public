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
						   