USE [CDMS_KEN]
GO
/****** Object:  View [dbo].[tmpAppraisal_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Create tmp views for the job
CREATE VIEW [dbo].[tmpAppraisal_Detail_VW]
AS
SELECT        Id, ActivityId, EffDt, OtherPermitLeases, RegionalOfficeReviewFiles, HighestAndBestUse, LastAppraisalRequestDate
FROM            dbo.Appraisal_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Appraisal_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[tmpAppraisal_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[tmpAppraisal_Header_VW]
AS
SELECT        Id, ActivityId, Allotment, RegionalOfficeReviewFiles, HighestAndBestUse, LastAppraisalRequestDate, OtherPermitLeases, EffDt
FROM            dbo.Appraisal_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Appraisal_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))

GO
/****** Object:  View [dbo].[tmpAppraisal_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[tmpAppraisal_VW]
AS
SELECT			a.Id AS ActivityId,
				h.Id AS hId, h.Allotment, 
				h.RegionalOfficeReviewFiles as hRegionalOfficeReviewFiles, 
				h.HighestAndBestUse as hHighestAndBestUse, 
				h.LastAppraisalRequestDate as hLastAppraisalRequestDate,  
				h.OtherPermitLeases as hOtherPermitLeases, 
				h.EffDt as hEffDt, 
                d.Id AS dId, d.EffDt AS dEffDt, 
				d.RegionalOfficeReviewFiles as dRegionalOfficeReviewFiles, 
				d.HighestAndBestUse as dHighestAndBestUse,
				d.LastAppraisalRequestDate as dLastAppraisalRequestDate, 
				d.OtherPermitLeases as dOtherPermitLeases
                
FROM            dbo.Activities AS a INNER JOIN
                         dbo.tmpAppraisal_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.tmpAppraisal_Detail_VW AS d ON d.ActivityId = a.Id --INNER JOIN
                         --dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id

GO
/****** Object:  View [dbo].[ActivityQAs_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ActivityQAs_VW]
AS
SELECT        a.Id, ActivityId, QAStatusId, UserId, EffDt, Comments, q.Name as QAStatusName, q.Description as QAStatusDescription
FROM            dbo.ActivityQAs AS a
	JOIN 		dbo.QAStatus q ON a.QAStatusId = q.Id
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS Expr1
                               FROM            dbo.ActivityQAs AS aa
                               WHERE        (ActivityId = a.ActivityId)));
GO
/****** Object:  View [dbo].[WaterTemp_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterTemp_Detail_VW]
AS
SELECT        Id, ReadingDateTime, GMTReadingDateTime, AirTemperature, AirTemperatureF, WaterTemperature, WaterTemperatureF, WaterLevel, TempAToD, BatteryVolts, 
                         Conductivity, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, PSI, AbsolutePressure
FROM            dbo.WaterTemp_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.WaterTemp_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[WaterTemp_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterTemp_Header_VW]
AS
SELECT        Id, Technicians, Comments, CollectionType, DepthToWater, PSI, StaticWaterLevel, WeatherConditions, FieldActivityType,
                         SamplePeriod, SampleTempUnit, DeployTime, ActivityId, ByUserId, EffDt
FROM            dbo.WaterTemp_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.WaterTemp_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO
/****** Object:  View [dbo].[WaterTempDL_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterTempDL_vw]
AS
SELECT        DATEPART(yyyy, a.ActivityDate) AS EnteredYear, DATEPART(mm, a.ActivityDate) AS EnteredMonth, a.ActivityDate AS DateEnterCDMS, L.Label AS SiteName, 
                         h.CollectionType, h.FieldActivityType, DATEPART(yyyy, d.ReadingDateTime) AS LoggerYear, d.ReadingDateTime, d.AirTemperature, d.WaterTemperature, 
                         h.Technicians, h.Comments AS LoggerMetadata, d.QAStatusId AS QARowStatus, aq.Comments AS ActivityQAComments, aq.QAStatusName AS ActivityQAStatus, 
                         a.DatasetId, a.InstrumentId, L.WaterBodyId, h.EffDt, d.EffDt AS DetailEffDt
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS L ON a.LocationId = L.Id

GO
/****** Object:  View [dbo].[Appraisal_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[Appraisal_Detail_VW]
as
SELECT        Id, AppraisalYear, AppraisalFiles, AppraisalPhotos, AppraisalComments, AppraisalStatus, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, AppraisalType,
                          AppraisalLogNumber, AppraisalValue, AppraisalValuationDate, Appraiser, TypeOfTransaction, PartiesInvolved, AppraisalProjectType, RequestNumber, 
                          NwroComments, RegionalOfficeReviewFiles, HighestAndBestUse, LastAppraisalRequestDate
FROM            dbo.Appraisal_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Appraisal_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[Appraisal_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[Appraisal_Header_VW]
as
SELECT        Id, Allotment, AllotmentPhotoFiles, FarmingLeaseFiles, UpdatedTSRFile, GrazingLeaseFiles, MapFiles, TimberAppraisalFiles, TSRFiles, 
                         AllotmentStatus, AllotmentName, AllotmentDescription, AllotmentComments, CobellAppraisalWave, LeaseTypes, HasTimber, IsMappable, 
                         Acres, PriorityType, LegalDescription, ActivityId, ByUserId, EffDt, OtherPermitLeases
FROM            dbo.Appraisal_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Appraisal_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))

GO
/****** Object:  View [dbo].[Appraisal_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[Appraisal_VW]
as
SELECT        h.Id AS Appraisal_Header_Id, h.Allotment, h.AllotmentStatus, h.AllotmentName, h.UpdatedTSRFile, h.AllotmentDescription, h.AllotmentComments, 
                         h.HasTimber, h.IsMappable, h.Acres, h.PriorityType, h.LegalDescription, h.AllotmentPhotoFiles, h.FarmingLeaseFiles, h.OtherPermitLeases,
                         h.GrazingLeaseFiles, h.MapFiles, h.TimberAppraisalFiles, h.TSRFiles, h.CobellAppraisalWave, h.LeaseTypes, h.ByUserId, h.EffDt, 
                         d.Id AS Appraisal_Detail_Id, d.AppraisalYear, d.AppraisalFiles, d.AppraisalPhotos, d.AppraisalComments, d.AppraisalStatus, d.AppraisalType, 
                         d.AppraisalLogNumber, d.AppraisalValue, d.AppraisalValuationDate, d.Appraiser, d.TypeOfTransaction, d.PartiesInvolved, d.AppraisalProjectType, d.RowId, 
                         d.RowStatusId, d.ByUserId AS Appraisal_Detail_ByUserId, d.QAStatusId, d.EffDt AS Appraisal_Detail_EffDt, d.RequestNumber, 
                         d.NwroComments, d.RegionalOfficeReviewFiles, d.HighestAndBestUse, d.LastAppraisalRequestDate,
                         aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId, a.SourceId, a.LocationId, 
                         a.UserId AS Activity_UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, a.Id AS ActivityId, aq.QAStatusName
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Appraisal_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.Appraisal_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id

GO
/****** Object:  View [dbo].[SpawningGroundSurvey_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[SpawningGroundSurvey_Detail_VW]
as
SELECT        Id, FeatureID, FeatureType, Species, Time, Temp, Channel, ReddLocation, ReddHabitat, WaypointNumber, FishCount, FishLocation, Sex, 
                         REPLACE(REPLACE(REPLACE(FinClips, '["', ''), '"]', ''), '","', ',') AS FinClips, REPLACE(REPLACE(REPLACE(Marks, '["', ''), '"]', ''), '","', ',') AS Marks, SpawningStatus, 
                         ForkLength, MeHPLength, SnoutID, ScaleID, Tag, TagID, Comments, Ident, EastingUTM, NorthingUTM, DateTime, RowId, RowStatusId, ActivityId, ByUserId, 
                         QAStatusId, EffDt, GeneticID, KidneyID, EstimatedLocation, Origin, PercentRetained, NumberEggsRetained, MortalityType, ReddMeasurements
FROM            dbo.SpawningGroundSurvey_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SpawningGroundSurvey_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[SpawningGroundSurvey_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SpawningGroundSurvey_Header_VW]
AS
SELECT        *
FROM            dbo.SpawningGroundSurvey_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SpawningGroundSurvey_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)));

GO
/****** Object:  View [dbo].[SpawningGroundSurvey_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[SpawningGroundSurvey_vw]
as
SELECT        h.Id AS HeaderID, h.Technicians, h.StartTime, h.EndTime, h.StartTemperature, h.EndTemperature, h.StartEasting, h.StartNorthing, h.EndEasting, h.EndNorthing, 
                         h.Flow, h.WaterVisibility, h.Weather, h.FlaggedRedds, h.NewRedds, h.HeaderComments, h.FieldsheetLink, h.ByUserId AS HeaderUserId, h.EffDt AS HeaderEffDt, 
                         h.TargetSpecies, 
                         d.Id AS DetailID, d.FeatureID, d.FeatureType, d.Species, d.Time, d.Temp, d.Channel, d.ReddLocation, d.ReddHabitat, d.Origin, d.WaypointNumber, 
                         d.FishCount, d.FishLocation, d.Sex, d.FinClips, d.Marks, d.SpawningStatus, d.ForkLength, d.MeHPLength, d.SnoutID, d.ScaleID, d.Tag, d.TagID, d.Comments, d.Ident, 
                         d.EastingUTM, d.NorthingUTM, d.DateTime, d.RowId, d.RowStatusId, d.ByUserId AS DetailUserId, d.QAStatusId, d.EffDt AS DetailEffDt, d.GeneticID, d.KidneyID, 
                         d.EstimatedLocation, d.PercentRetained, d.NumberEggsRetained, d.MortalityType, d.ReddMeasurements,
                         a.Id AS ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, a.CreateDate, 
                         w.Id AS WaterbodyId, w.Name AS WaterbodyName, 
                         l.Id AS LocationId, l.Label AS LocationLabel, (CASE species WHEN 'STS' THEN year(dateadd(month, 3, activitydate)) ELSE year(activitydate) END) AS runyear, 
                         aq.QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId, w.Description AS Basin 
FROM            dbo.SpawningGroundSurvey_Detail_VW AS d INNER JOIN
                         dbo.SpawningGroundSurvey_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId INNER JOIN
                         dbo.Locations AS l ON l.Id = a.LocationId INNER JOIN
                         dbo.WaterBodies AS w ON w.Id = l.WaterBodyId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id

GO
/****** Object:  View [dbo].[ScrewTrap_DetailUM_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ScrewTrap_DetailUM_VW]
AS
SELECT        TOP (100) PERCENT Id, Sequence, EventType, PitTagCode, SpeciesRunRearing, AdditionalPositionalComments, ConditionalComment, TextualComments, 
                         AdditionalPositionalComments AS FinClip, CASE WHEN AdditionalPositionalComments LIKE ('%RE%') OR
                         ConditionalComment LIKE ('%RE%') THEN 'Recapture' ELSE 'Original' END AS CaptureStatus, CASE WHEN SpeciesRunRearing = 'A0W' AND 
                         ConditionalComment LIKE ('M%') THEN 'Mortality (Trapping)' WHEN SpeciesRunRearing = 'A0W' AND ConditionalComment LIKE ('%SM%') 
                         THEN 'Mortality (Handling)' WHEN SpeciesRunRearing = 'A0W' AND ConditionalComment LIKE ('% M%') 
                         THEN 'Mortality (Trapping)' WHEN SpeciesRunRearing = 'A0W' AND AdditionalPositionalComments IS NULL 
                         THEN 'Not Marked' WHEN PitTagCode LIKE ('3%%.%%%%%%%%%%') AND ConditionalComment LIKE ('M%') 
                         THEN 'Mortality (Trapping)' WHEN PitTagCode LIKE ('3%%.%%%%%%%%%%') AND ConditionalComment LIKE ('%SM%') 
                         THEN 'Mortality (Handling)' WHEN PitTagCode LIKE ('3%%.%%%%%%%%%%') AND ConditionalComment LIKE ('% M%') 
                         THEN 'Mortality (Trapping)' WHEN PitTagCode LIKE ('3%%.%%%%%%%%%%') AND AdditionalPositionalComments IS NOT NULL 
                         THEN 'PIT Tagged/Clipped' WHEN PitTagCode LIKE ('3%%.%%%%%%%%%%') THEN 'PIT Tagged' WHEN ConditionalComment LIKE ('M%') 
                         THEN 'Mortality (Trapping)' WHEN ConditionalComment LIKE ('% M%') THEN 'Mortality (Trapping)' WHEN ConditionalComment LIKE ('%SM%') 
                         THEN 'Mortality (Handling)' WHEN TextualComments LIKE ('%Orphaned%') THEN 'PIT Tagged' WHEN PitTagCode LIKE ('%..........%') AND 
                         ConditionalComment LIKE ('%NF%') THEN 'PIT Tagged' WHEN PitTagCode LIKE ('%..........%') AND ConditionalComment LIKE ('%RE L%') 
                         THEN 'Lost Tag' WHEN PitTagCode LIKE ('%..........%') AND AdditionalPositionalComments IS NULL THEN 'Not Marked' WHEN PitTagCode LIKE ('%..........%') 
                         THEN 'Clipped' ELSE 'QA NEEDED' END AS Status, CASE WHEN TextualComments LIKE ('%Containment%') 
                         THEN 'Containment' WHEN TextualComments LIKE ('%L%') AND AdditionalPositionalComments LIKE ('%D') THEN 'Containment' WHEN TextualComments LIKE ('%L%')
                          AND AdditionalPositionalComments LIKE ('D %') THEN 'Containment' WHEN AdditionalPositionalComments LIKE ('%D') AND 
                         AdditionalPositionalComments LIKE ('%RE%') THEN 'Containment' WHEN AdditionalPositionalComments LIKE ('%D ') AND 
                         AdditionalPositionalComments LIKE ('%RE%') THEN 'Containment' ELSE '' END AS ContainStudy, ForkLength, Weight, OtherSpecies, FishCount, TagStatus, 
                         ClipStatus, ReleaseLocation, Note, FishComments, ActivityId, LifeStage, MigrationYear, SizeOfCount, Containment
FROM            dbo.ScrewTrap_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.ScrewTrap_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[Screwtrap_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Screwtrap_Header_VW]
AS
SELECT        Id, FileTitle, ClipFiles, Tagger, LivewellTemp, TaggingTemp, PostTaggingTemp, ReleaseTemp, ArrivalTime, DepartTime, ArrivalRPMs, DepartureRPMs, Hubometer, 
                         HubometerTime, TrapStopped, TrapStarted, FishCollected, FishReleased, Flow, Turbidity, TrapDebris, RiverDebris, ActivityComments, ActivityId, ByUserId, EffDt, 
                         Unit, DailyFinClips, Crew, TrapStatus, Weather
FROM            dbo.ScrewTrap_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.ScrewTrap_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))

GO
/****** Object:  View [dbo].[ScrewTrapUM_Export_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ScrewTrapUM_Export_VW]
AS
SELECT        TOP (100) PERCENT CASE WHEN Month(ActivityDate) >= 8 THEN Year(ActivityDate) + 1 WHEN Month(ActivityDate) <= 7 THEN Year(ActivityDate) 
                         ELSE ' ' END AS OutmigrationYear, w.Name AS WaterbodyName, l.Label AS LocationLabel, a.ActivityDate, h.ArrivalTime, 
                         CASE WHEN h.FileTitle LIKE '%"NAME":"%' THEN SUBSTRING(h.FileTitle, PATINDEX('%"NAME":"%', h.FileTitle), 21) ELSE h.FileTitle END AS FileName, h.ClipFiles, 
                         h.Crew, h.Tagger, h.LivewellTemp, h.TaggingTemp, h.PostTaggingTemp, h.ReleaseTemp, h.DepartTime, h.ArrivalRPMs, h.DepartureRPMs, h.HubometerTime, 
                         h.Hubometer, h.TrapStatus, h.TrapStopped, h.TrapStarted, h.FishCollected, h.FishReleased, h.Flow, h.Turbidity, h.TrapDebris, h.RiverDebris, h.ActivityComments, 
                         h.DailyFinClips, d.Sequence, d.SpeciesRunRearing AS P3Species, d.PitTagCode, d.ForkLength, d.Weight, d.OtherSpecies, d.FishCount, 
                         d.AdditionalPositionalComments, d.ConditionalComment, d.TextualComments, d.Note, d.ReleaseLocation, d.FishComments, d.TagStatus, d.ClipStatus, 
                         CASE WHEN SpeciesRunRearing IN ('00U', '25H', '25W', '3RH', '3RW', '7RW', '90U', 'A0W', 'D0W', 'ERU', 'G0W') 
                         THEN '' WHEN d .ForkLength < 100 THEN 'Small' WHEN d .ForkLength > 120 THEN 'Large' WHEN d .FishCount IS NULL THEN NULL 
                         ELSE 'Medium' END AS SizeClass, 
                         CASE WHEN SpeciesRunRearing = '00U' THEN 'Unknown (fish not observed)' WHEN SpeciesRunRearing = '11H' THEN 'Hat. Spring Chinook' WHEN SpeciesRunRearing
                          = '11U' THEN 'Spring Chinook (unknown r/t)' WHEN SpeciesRunRearing = '11W' THEN 'Wild Spring Chinook' WHEN SpeciesRunRearing = '13H' THEN 'Hat. Fall Chinook'
                          WHEN SpeciesRunRearing = '13W' THEN 'Wild Fall Chinook' WHEN SpeciesRunRearing = '25H' THEN 'Hat. Coho' WHEN SpeciesRunRearing = '25W' THEN 'Wild Coho'
                          WHEN SpeciesRunRearing = '32H' THEN 'Hat. Summer Steelhead' WHEN SpeciesRunRearing = '32W' THEN 'Wild Summer Steelhead' WHEN SpeciesRunRearing =
                          '3RH' THEN 'Hat. Rainbow Trout' WHEN SpeciesRunRearing = '3RW' THEN 'Wild Rainbow Trout' WHEN SpeciesRunRearing = '7RW' THEN 'Bull Trout' WHEN SpeciesRunRearing
                          = '90U' THEN 'Other' WHEN SpeciesRunRearing = 'A0W' THEN 'Lamprey' WHEN SpeciesRunRearing = 'D0W' THEN 'Northern Pikeminnow' WHEN SpeciesRunRearing
                          = 'ERU' THEN 'Brook Trout' WHEN SpeciesRunRearing = 'G0W' THEN 'Mountain Whitefish' WHEN d .FishCount IS NULL THEN NULL 
                         ELSE 'Other' END AS SpeciesRunRearing, CASE WHEN SpeciesRunRearing IN ('11H', '13H', '25H', '32H', '3RH') 
                         THEN 'HAT' WHEN OtherSpecies IN ('HATCHERY CHS', 'HATCHERY STS') THEN 'HAT' WHEN SpeciesRunRearing IN ('11U', '11W', '13W', '25W', '32W', '3RW') 
                         THEN 'NAT' WHEN OtherSpecies IN ('CHINOOK', 'STEELHEAD') THEN 'NAT' WHEN d .FishCount IS NULL THEN NULL ELSE 'UNK' END AS HatNat, d.FinClip, 
                         d.CaptureStatus, d.Status, d.ContainStudy, aq.QAStatusName, aq.Comments AS ActivityQAComments, d.SizeOfCount, d.Containment, d.EventType
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Screwtrap_Header_VW AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id LEFT OUTER JOIN
                         dbo.ScrewTrap_DetailUM_VW AS d ON h.ActivityId = d.ActivityId LEFT OUTER JOIN
                         dbo.Locations AS l ON l.Id = a.LocationId LEFT OUTER JOIN
                         dbo.WaterBodies AS w ON w.Id = l.WaterBodyId
WHERE        (a.DatasetId = 1215)

GO
/****** Object:  View [dbo].[AdultWeir_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[AdultWeir_Detail_VW]
AS
SELECT        Id, Sex, REPLACE(REPLACE(REPLACE(Mark, '["', ''), '"]', ''), '","', ',') AS Mark, ForkLength, TotalLength, Weight, ScaleId, GeneticSampleId, SnoutId, Disposition, 
                         LifeStage, Origin, Species, RadioTagId, Solution, SolutionDosage, REPLACE(REPLACE(REPLACE(FinClip, '["', ''), '"]', ''), '","', ',') AS FinClip, 
                         CASE WHEN TotalFishRepresented IS NULL AND Species IS NOT NULL THEN 1 ELSE TotalFishRepresented END AS TotalFishRepresented, FishComments, RowId, 
                         ActivityId, ByUserId, QAStatusId, EffDt, OtolithNumber, FishNumber, RowStatusId, Recapture, PITTagId, ReleaseSite, Ripeness, REPLACE(REPLACE(REPLACE(Tag, 
                         '["', ''), '"]', ''), '","', ',') AS Tag, OtolithGenetics, PercentSpawned, OtherTagId, RunYear, TransportTankUnit, Girth, TrapLocation, PassageLocation, PassageTime, 
                         AgePITTag, AgeCWT, AgeScale, AgeLength, BroodProgram, TransportFrom, HatcheryType, Stock
FROM            dbo.AdultWeir_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.AdultWeir_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[AdultWeir_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[AdultWeir_Header_VW]
as
SELECT        Id, AirTemperature, AirTemperatureF, WaterTemperature, WaterTemperatureF, TimeStart, TimeEnd, Technicians, WaterFlow, Comments, ActivityId, ByUserId, EffDt, 
                         CollectionType, FieldSheetFile
FROM            dbo.AdultWeir_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.AdultWeir_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))

GO
/****** Object:  View [dbo].[AdultWeir_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[AdultWeir_vw]
as
SELECT        TOP (100) PERCENT h.CollectionType, dbo.Locations.Label AS LocationLabel, CONVERT(VARCHAR(10), a.ActivityDate, 101) AS WWVideoDate, a.ActivityDate, 
                         h.AirTemperature, h.AirTemperatureF, h.WaterTemperature, h.WaterTemperatureF, h.TimeStart, h.TimeEnd, h.Technicians, h.WaterFlow, h.Comments, 
                         h.FieldSheetFile,
                         d.RunYear, d.FishNumber, d.Species, d.Sex, d.Origin, d.LifeStage, d.FinClip, CASE WHEN d .Mark IS NULL AND d .Species IS NOT NULL 
                         THEN 'NONE' ELSE d .Mark END AS Mark, d.TotalFishRepresented, d.ForkLength, d.TotalLength, d.Weight, d.Disposition, d.Recapture, d.GeneticSampleId, 
                         d.OtolithNumber, d.Tag, d.PITTagId, d.RadioTagId, d.ScaleId, d.SnoutId, d.OtherTagId, d.FishComments, d.Solution, d.SolutionDosage, d.OtolithGenetics, 
                         d.AgePITTag, d.AgeCWT, d.AgeScale, d.AgeLength, d.HatcheryType, d.Stock, d.TransportFrom, d.TransportTankUnit, d.ReleaseSite, d.Ripeness, d.PassageLocation, 
                         d.PercentSpawned, d.Girth, d.TrapLocation, d.BroodProgram, d.PassageTime, 
                         aq.QAStatusName, aq.Comments AS ActivityQAComments, 
                         h.ByUserId AS AdultWeir_Header_ByUserId, 
                         a.Id AS ActivityId, a.UserId AS Activity_UserId, a.LocationId, a.ActivityTypeId, a.CreateDate, 
                         h.Id AS AdultWeir_Header_Id, 
                         aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, 
                         h.EffDt AS AdultWeir_Header_EffDt, d.Id AS AdultWeir_Detail_Id, 
                         a.DatasetId, d.RowId, d.RowStatusId, d.ByUserId AS AdultWeir_Detail_ByUserId, d.QAStatusId AS RowQAStatusId, d.QAStatusId, d.EffDt AS AdultWeir_Detail_EffDt, 
                         a.SourceId
FROM            dbo.Activities AS a INNER JOIN
                         dbo.AdultWeir_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.AdultWeir_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations ON a.LocationId = dbo.Locations.Id

GO
/****** Object:  View [dbo].[SO_WTQueryReports_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SO_WTQueryReports_vw]
AS
SELECT        dbo.Locations.Label AS LoggerSite, d.ReadingDateTime, d.WaterTemperature, CONVERT(char(10), d.ReadingDateTime, 101) AS ReadingDate, 
                         YEAR(d.ReadingDateTime) AS ReadingYear, d.QAStatusId, dbo.Locations.Id AS LocationId
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.Locations ON a.LocationId = dbo.Locations.Id
WHERE        (a.DatasetId IN (1196, 1198)) AND (d.QAStatusId = 1)

GO
/****** Object:  View [dbo].[CreelSurvey_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CreelSurvey_Detail_VW]
AS
SELECT        d.InterviewTime, d.InterviewComments, d.GPSEasting, d.GPSNorthing, d.RowId, d.RowStatusId, d.ActivityId, d.ByUserId, d.QAStatusId, d.EffDt, d.Id, d.FishermanId, 
                         d.TotalTimeFished, d.FishCount, d.Species, d.MethodCaught, d.Disposition, d.Sex, d.Origin, d.FinClip, d.Tag, d.OtherTagId, d.Marks, d.ForkLength, d.MeHPLength, d.SnoutId, 
                         d.ScaleId, d.CarcassComments, dbo.Fishermen.FirstName, dbo.Fishermen.Aka, dbo.Fishermen.LastName, dbo.Fishermen.PhoneNumber, dbo.Fishermen.DateAdded, 
                         dbo.Fishermen.DateInactive, dbo.Fishermen.FullName, dbo.Fishermen.FishermanComments, dbo.Fishermen.StatusId, dbo.Fishermen.Id AS FishermenId
FROM            dbo.CreelSurvey_Detail AS d INNER JOIN
                         dbo.Fishermen ON d.FishermanId = dbo.Fishermen.Id
WHERE        (d.EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.CreelSurvey_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (d.RowStatusId = 0)

GO
/****** Object:  View [dbo].[CreelSurvey_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CreelSurvey_Header_VW]
AS
SELECT        Id, Surveyor, NumberAnglersObserved, NumberAnglersInterviewed, FieldSheetFile, ByUserId, EffDt, SurveySpecies, WorkShift, WeatherConditions, TimeStart, 
                         TimeEnd, SurveyComments, Direction, ActivityId, Dry
FROM            dbo.CreelSurvey_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.CreelSurvey_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))

GO
/****** Object:  View [dbo].[CreelFishermanStats_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CreelFishermanStats_vw]
AS
SELECT        CASE WHEN h.SurveySpecies = 'STS' AND Month(a.ActivityDate) > 9 THEN (Year(a.ActivityDate) + 1) ELSE Year(a.ActivityDate) END AS SeasonYear, 
                         w.Name AS Stream, l.Label AS StreamReach, h.SurveySpecies, a.ActivityDate, h.NumberAnglersObserved AS Observed, h.NumberAnglersInterviewed AS Interviewed,
                          h.TimeStart AS SecStart, h.TimeEnd AS SecStop, d.InterviewTime, d.FullName, d.TotalTimeFished AS FishTime, d.FishCount, 
                         CASE WHEN d .Species = 'RBT' THEN 'Rainbow Trout' WHEN d .Species = 'CHS' THEN 'Spring Chinook Salmon' WHEN d .Species = 'BUT' THEN 'Bull Trout' WHEN d
                          .Species = 'STS' THEN 'Summer Steelhead' WHEN d .Species = 'MWF' THEN 'Moutain White Fish' ELSE NULL END AS SpeciesName, d.Species, 
                         CASE WHEN d .Origin = 'NAT' THEN 'Natural' WHEN d .Origin = 'HAT' THEN 'Hatchery' WHEN d .Origin = 'UNK' THEN 'Unknown' ELSE NULL END AS Origin, d.Sex, 
                         CASE WHEN d .Disposition = 'KEPT' THEN 'Kept' WHEN d .Disposition = 'RELEASED' THEN 'Released' ELSE NULL END AS Disposition, h.Id AS HeaderId
FROM            dbo.CreelSurvey_Detail_VW AS d FULL OUTER JOIN
                         dbo.CreelSurvey_Header_VW AS h ON d.ActivityId = h.ActivityId FULL OUTER JOIN
                         dbo.Activities AS a INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id INNER JOIN
                         dbo.WaterBodies AS w ON l.WaterBodyId = w.Id ON h.ActivityId = a.Id
WHERE        (a.DatasetId = 1230)

GO
/****** Object:  View [dbo].[CreelEstimate1_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CreelEstimate1_vw]
AS
SELECT        a.Id AS ActivityId, h.SurveySpecies, CASE WHEN Month(a.ActivityDate) > 8 THEN Year(a.ActivityDate) + 1 ELSE Year(a.ActivityDate) END AS SeasonYear, 
                         CAST(LEFT(a.ActivityDate, 11) AS datetime) AS SurveyDate, l.Label AS SurveySection, w.Name AS StreamName, h.NumberAnglersObserved AS Observed, 
                         h.NumberAnglersInterviewed AS Interviewed, h.TimeStart, h.TimeEnd, h.Dry, CAST(LEFT(a.ActivityDate, 11) AS datetime) + CAST(h.TimeStart AS datetime) 
                         AS ComStart, CAST(LEFT(a.ActivityDate, 11) AS datetime) + CAST(h.TimeEnd AS datetime) AS ComEnd
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id INNER JOIN
                         dbo.WaterBodies AS w ON l.WaterBodyId = w.Id INNER JOIN
                         dbo.CreelSurvey_Header AS h ON a.Id = h.ActivityId
WHERE        (a.DatasetId = 1230) AND (h.EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.CreelSurvey_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))

GO
/****** Object:  View [dbo].[CreelEstimate2_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CreelEstimate2_vw]
AS
SELECT        sd.Season, sd.Species, ah.StreamName, ah.SurveySection, sd.OpenDate AS SeasonOpen, sd.CloseDate AS SeasonClosed, sd.TotalDays AS SeasonDays, 
                         ah.ComStart AS SurveyStartTime, ah.ComEnd AS SurveyEndTime, ah.SurveyDate, ah.Observed, ah.Interviewed, ah.TimeStart, ah.TimeEnd, d.InterviewTime, 
                         d.FishermanId, d.TotalTimeFished, d.FishCount, d.Species AS SpeciesCaught, ah.SurveySpecies, SUM(ct.Daylight) AS DaylightHours
FROM            dbo.CreelEstimate1_vw AS ah INNER JOIN
                         dbo.SeasonDates AS sd ON ah.SurveySpecies = sd.Species AND ah.SeasonYear = sd.Season LEFT OUTER JOIN
                         dbo.CreelSurvey_Detail AS d ON ah.ActivityId = d.ActivityId CROSS JOIN
                         dbo.CivilianTwilight AS ct
WHERE        (ct.RecordDate BETWEEN sd.OpenDate AND sd.CloseDate)
GROUP BY sd.Season, sd.Species, ah.StreamName, ah.SurveySection, sd.OpenDate, sd.CloseDate, sd.TotalDays, ah.ComStart, ah.ComEnd, ah.SurveyDate, ah.Observed, 
                         ah.Interviewed, ah.TimeStart, ah.TimeEnd, d.InterviewTime, d.FishermanId, d.TotalTimeFished, d.FishCount, d.Species, ah.SurveySpecies

GO
/****** Object:  View [dbo].[StreamNet_RperS_detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[StreamNet_RperS_detail_VW]
AS
SELECT        Id, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, CommonName, Run, PopFit, SpawnerLocation, RecruitLocation, BroodYear, RperStype, TRTmethod, 
                         ContactAgency, MethodNumber, RperS, Comments, NullRecord, DataStatus, ContactPersonFirst, ContactPersonLast, ContactPhone, ContactEmail, Age10Adults, 
                         Age11PlusAdults, Age1Juvs, Age2Adults, Age2Juvs, Age3Adults, Age3Juvs, Age4Adults, Age4PlusJuvs, Age5Adults, Age6Adults, Age7Adults, Age8Adults, Age9Adults, 
                         CBFWApopName, CommonPopName, CompilerRecordID, DataEntry, DataEntryNotes, ESU_DPS, HarvestAdj, HatcherySpawners, HatcherySpawnersAlpha, 
                         HatcherySpawnersLowerLimit, HatcherySpawnersUpperLimit, IndicatorLocation, LastUpdated, MainstemHarvest, MajorPopGroup, MeasureLocation, MetaComments, 
                         MethodAdjustments, MetricLocation, NOBroodStockRemoved, OceanHarvest, PopFitNotes, PopID, ProtMethDocumentation, ProtMethName, ProtMethURL, Publish, 
                         RecoveryDomain, Recruits, RecruitsAlpha, RecruitsLowerLimit, RecruitsMissing, RecruitsMissingExplanation, RecruitsUpperLimit, RefID, RperSAlpha, 
                         RperSLowerLimit, RperSUpperLimit, SubmitAgency, TotalSpawners, TotalSpawnersAlpha, TotalSpawnersLowerLimit, TotalSpawnersUpperLimit, TribHarvest, 
                         UpdDate, YOY, ShadowId
FROM            dbo.StreamNet_RperS_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.StreamNet_RperS_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[StreamNet_RperS_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[StreamNet_RperS_Header_VW]
AS
SELECT        *
FROM            dbo.StreamNet_RperS_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.StreamNet_RperS_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)));

GO
/****** Object:  View [dbo].[StreamNet_RperS_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[StreamNet_RperS_vw] as
    select
    d.Id, d.CommonName, d.Run, d.PopFit, 
    d.SpawnerLocation, d.RecruitLocation, d.BroodYear, d.RperStype, d.TRTmethod, d.ContactAgency, d.MethodNumber, d.RperS, d.Comments, 
    d.NullRecord, d.DataStatus, d.ContactPersonFirst, d.ContactPersonLast, d.ContactPhone, d.ContactEmail, d.Age10Adults, d.Age11PlusAdults, 
    d.Age1Juvs, d.Age2Adults, d.Age2Juvs, d.Age3Adults, d.Age3Juvs, d.Age4Adults, d.Age4PlusJuvs, d.Age5Adults, d.Age6Adults, d.Age7Adults, 
    d.Age8Adults, d.Age9Adults, d.CBFWApopName, d.CommonPopName, d.CompilerRecordID, d.DataEntry, d.DataEntryNotes, d.ESU_DPS, d.HarvestAdj, 
    d.HatcherySpawners, d.HatcherySpawnersAlpha, d.HatcherySpawnersLowerLimit, d.HatcherySpawnersUpperLimit, d.IndicatorLocation, d.LastUpdated, 
    d.MainstemHarvest, d.MajorPopGroup, d.MeasureLocation, d.MetaComments, d.MethodAdjustments, d.MetricLocation, d.NOBroodStockRemoved, 
    d.OceanHarvest, d.PopFitNotes, d.PopID, d.ProtMethDocumentation, d.ProtMethName, d.ProtMethURL, d.Publish, d.RecoveryDomain, d.Recruits, 
    d.RecruitsAlpha, d.RecruitsLowerLimit, d.RecruitsMissing, d.RecruitsMissingExplanation, d.RecruitsUpperLimit, d.RefID, d.RperSAlpha,
    d.RperSLowerLimit, d.RperSUpperLimit, d.SubmitAgency, d.TotalSpawners, d.TotalSpawnersAlpha, d.TotalSpawnersLowerLimit, d.TotalSpawnersUpperLimit,
    d.TribHarvest, d.UpdDate, d.YOY, d.ShadowId,

    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate 

from StreamNet_RperS_detail_vw d 
join StreamNet_RperS_Header_vw h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId


GO
/****** Object:  View [dbo].[MetadataValues_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[MetadataValues_vw]
AS
SELECT        Id, MetadataPropertyId, RelationId, [Values], UserId, EffDt
FROM            dbo.MetadataValues AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.MetadataValues AS dd
                               WHERE        (RelationId = d.RelationId) AND (MetadataPropertyId = d.MetadataPropertyId)))

GO
/****** Object:  View [dbo].[Project_RV_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Project_RV_VW]
AS
SELECT        dbo.Projects.Name, dbo.Projects.Description, dbo.MetadataProperties.Name AS MetadataProperties_Name, dbo.MetadataProperties.Id AS MetadataId, 
                         dbo.Projects.Id AS ProjectId, dbo.MetadataValues_vw.[Values], dbo.MetadataValues_vw.EffDt
FROM            dbo.MetadataValues_vw INNER JOIN
                         dbo.MetadataProperties ON dbo.MetadataValues_vw.MetadataPropertyId = dbo.MetadataProperties.Id RIGHT OUTER JOIN
                         dbo.Projects ON dbo.MetadataValues_vw.RelationId = dbo.Projects.Id

GO
/****** Object:  View [dbo].[FishScales_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FishScales_Detail_VW]
AS
SELECT        Id, FieldScaleID, GumCardScaleID, ScaleCollectionDate, Species, LifeStage, Circuli, FreshwaterAge, SaltWaterAge, TotalAdultAge, SpawnCheck, Regeneration, Stock, RowId, RowStatusId, ActivityId, ByUserId, 
                         QAStatusId, EffDt, ScaleComments, BadScale, TotalAge
FROM            dbo.FishScales_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.FishScales_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId)))

GO
/****** Object:  View [dbo].[FishScales_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FishScales_Header_VW]
AS
SELECT        *
FROM            dbo.FishScales_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.FishScales_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)));

GO
/****** Object:  View [dbo].[GRME_VSPTesting_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[GRME_VSPTesting_vw]
AS
SELECT        h.RunYear, d.Species, d.FreshwaterAge, d.SaltWaterAge, d.TotalAdultAge, d.TotalAge, COALESCE (sgs.FinClips, aw.FinClip) AS FinClip, COALESCE (sgs.Marks, 
                         aw.Mark) AS Marks, COALESCE (aw.Origin, sgs.Origin) AS Origin, a.DatasetId
FROM            dbo.FishScales_Detail_VW AS d LEFT OUTER JOIN
                         dbo.FishScales_Header_VW AS h ON d.ActivityId = h.ActivityId LEFT OUTER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.SpawningGroundSurvey_vw AS sgs ON sgs.ScaleID = d.FieldScaleID LEFT OUTER JOIN
                         dbo.AdultWeir_vw AS aw ON aw.ScaleId = d.FieldScaleID
WHERE        (a.DatasetId = 1224) AND (d.Species = 'CHS')

GO
/****** Object:  View [dbo].[FishScaleGRreport_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FishScaleGRreport_vw]
AS
SELECT        a.DatasetId, d.FieldScaleID, d.GumCardScaleID, d.ScaleCollectionDate, d.Species, d.Circuli, d.FreshwaterAge, d.SaltWaterAge, d.TotalAdultAge, d.SpawnCheck, 
                         d.Regeneration, d.ScaleComments, d.BadScale, d.TotalAge, aw.LifeStage, COALESCE (sgs.SpawningStatus, aw.PercentSpawned) AS PercentRetained, 
                         COALESCE (sgs.Sex, aw.Sex) AS Sex, aw.FishNumber, aw.BroodProgram, aw.HatcheryType, aw.Recapture, aw.ReleaseSite, aw.Ripeness, aw.OtolithGenetics, 
                         COALESCE (aw.FishComments, sgs.Comments) AS FishComments, aw.OtherTagId, aw.Disposition, aw.RadioTagId, aw.GeneticSampleId, aw.PITTagId, 
                         COALESCE (sgs.SnoutID, aw.SnoutId) AS SnoutId, COALESCE (d.Stock, aw.Stock) AS Stock, COALESCE (sgs.Marks, aw.Mark) AS Marks, COALESCE (sgs.ForkLength, 
                         aw.ForkLength) AS ForkLength, COALESCE (aw.LocationLabel, sgs.LocationLabel) AS LocationLabel, COALESCE (aw.RunYear, sgs.runyear) AS RunYear, 
                         COALESCE (aw.Origin, sgs.Origin) AS Origin, COALESCE (aw.FinClip, sgs.FinClips) AS FinClips, COALESCE (aw.Tag, sgs.Tag) AS Tag, aw.Weight
FROM            dbo.FishScales_Detail_VW AS d INNER JOIN
                         dbo.Activities AS a ON d.ActivityId = a.Id LEFT OUTER JOIN
                         dbo.SpawningGroundSurvey_vw AS sgs ON sgs.ScaleID = d.FieldScaleID LEFT OUTER JOIN
                         dbo.AdultWeir_vw AS aw ON aw.ScaleId = d.FieldScaleID
WHERE        (a.DatasetId = 1224)

GO
/****** Object:  View [dbo].[CreelSurvey_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CreelSurvey_vw]
AS
SELECT        w.Name AS StreamName, l.Label AS LocationLabel, CASE WHEN Month(a.ActivityDate) > 9 AND h.SurveySpecies = 'STS' THEN Year(a.ActivityDate) 
                         + 1 ELSE Year(a.ActivityDate) END AS RunYear, a.ActivityDate, h.WorkShift, h.Direction, h.SurveySpecies, h.Surveyor, h.NumberAnglersObserved, 
                         h.NumberAnglersInterviewed, h.FieldSheetFile, h.WeatherConditions, h.TimeStart, h.TimeEnd, h.SurveyComments, aq.Comments AS ActivityQAComments, h.Dry, 
                         d.FullName, d.InterviewTime, d.InterviewComments, d.GPSEasting, d.GPSNorthing, d.TotalTimeFished, d.FishermanComments, d.FishCount, d.Species, 
                         d.MethodCaught, d.Disposition, d.Sex, d.Origin, d.FinClip, d.Marks, d.ForkLength, d.MeHPLength, d.Tag, d.OtherTagId, d.SnoutId, d.ScaleId, d.CarcassComments, 
                         aq.QAStatusName, d.StatusId, d.FishermanId, d.FirstName, d.Aka, d.LastName, d.PhoneNumber, d.DateAdded, d.DateInactive, a.Id AS ActivityId, d.QAStatusId, 
                         a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, h.Id AS CreelSurvey_Header_ID, h.ByUserId, h.EffDt, d.Id AS CreelSurvey_Detail_ID, 
                         d.EffDt AS CreelSurvey_Detail_EffDt, aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, d.ByUserId AS CreelSurvey_Detail_ByUserId
FROM            dbo.Activities AS a LEFT OUTER JOIN
                         dbo.CreelSurvey_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id INNER JOIN
                         dbo.WaterBodies AS w ON l.WaterBodyId = w.Id FULL OUTER JOIN
                         dbo.CreelSurvey_Header_VW AS h ON a.Id = h.ActivityId

GO
/****** Object:  View [dbo].[SGSAllExport_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SGSAllExport_vw]
AS
SELECT        CASE WHEN h.TargetSpecies = 'STS' AND Month(a.ActivityDate) > 8 THEN Year(a.ActivityDate) + 1 ELSE Year(a.ActivityDate) END AS RunYear, 
                         dbo.Datasets.Name AS DatasetName, w.Name AS WaterbodyName, l.Label AS LocationLabel, a.ActivityDate, h.Technicians, h.TargetSpecies, h.StartTime, 
                         h.EndTime, h.StartTemperature, h.EndTemperature, h.StartEasting, h.StartNorthing, h.EndEasting, h.EndNorthing, 
                         CASE WHEN h.Flow = 'D' THEN 'DRY' WHEN h.Flow = 'L' THEN 'Low' WHEN h.Flow = 'M' THEN 'Moderate' WHEN h.Flow = 'H' THEN 'High' WHEN h.Flow = 'F' THEN 'Flooding'
                          ELSE NULL END AS Flow, 
                         CASE WHEN h.WaterVisibility = 1 THEN 'Riffles and Pools' WHEN h.WaterVisibility = 2 THEN 'Riffles' WHEN h.WaterVisibility = 3 THEN 'Neither Riffles nor Pools' ELSE
                          NULL END AS WaterVisibility, 
                         CASE WHEN h.Weather = 'C' THEN 'Clear' WHEN h.Weather = 'O' THEN 'Overcast' WHEN h.Weather = 'R' THEN 'Rain' WHEN h.Weather = 'S' THEN 'Snow' WHEN h.Weather
                          = 'F' THEN 'Foggy' WHEN h.Weather = 'P' THEN 'Partly Cloudy' ELSE NULL END AS Weather, h.FlaggedRedds, h.NewRedds, 
                         h.HeaderComments AS SurveyComments, d.FeatureID AS FeatureNumber, d.FeatureType, d.Time, d.Temp, d.EastingUTM, d.NorthingUTM, d.Species, d.FishCount, 
                         CASE WHEN d .FishLocation = 'ONR' THEN 'On Redd' WHEN d .FishLocation = 'NR' THEN 'Near Redd' WHEN d .FishLocation = 'OR' THEN 'Off Redd' ELSE NULL 
                         END AS FishLocation, CASE WHEN d .Channel = 'S' THEN 'Single' WHEN d .Channel = 'B' THEN 'Braid' ELSE NULL END AS Channel, 
                         CASE WHEN d .ReddLocation = 'S' THEN 'Side' WHEN d .ReddLocation = 'M' THEN 'Middle' ELSE NULL END AS ReddLocation, 
                         CASE WHEN d .ReddHabitat = 'P' THEN 'Pool' WHEN d .ReddHabitat = 'TO' THEN 'Pool Tail Out' WHEN d .ReddHabitat = 'RI' THEN 'Riffle' WHEN d .ReddHabitat = 'GL'
                          THEN 'Glide' ELSE NULL END AS ReddHabitat, d.Sex, d.FinClips, d.Marks, d.Origin, d.SpawningStatus, d.ForkLength, d.MeHPLength, d.SnoutID, d.ScaleID, d.Tag, 
                         d.TagID, d.Comments, d.GeneticID, d.KidneyID, d.EstimatedLocation, a.DatasetId
FROM            dbo.Locations AS l INNER JOIN
                         dbo.Activities AS a ON l.Id = a.LocationId INNER JOIN
                         dbo.WaterBodies AS w ON w.Id = l.WaterBodyId INNER JOIN
                         dbo.Datasets ON a.DatasetId = dbo.Datasets.Id LEFT OUTER JOIN
                         dbo.SpawningGroundSurvey_Detail_VW AS d INNER JOIN
                         dbo.SpawningGroundSurvey_Header_VW AS h ON d.ActivityId = h.ActivityId ON a.Id = h.ActivityId
WHERE        (l.LocationTypeId = 7)

GO
/****** Object:  View [dbo].[WaterTempGR_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterTempGR_vw]
AS
SELECT        dbo.WaterBodies.Name AS Stream, dbo.Locations.Label AS LoggerSite, 'rm ' + CONVERT(VARCHAR(10), dbo.Locations.RiverMile) 
                         + '-' + dbo.Locations.Label AS TitleSite, dbo.Locations.RiverMile AS RM, a.DatasetId, a.LocationId, a.InstrumentId, d.ReadingDateTime, d.AirTemperature, 
                         d.WaterTemperature, d.QAStatusId, YEAR(d.ReadingDateTime) AS Year, RIGHT('0' + CONVERT(VARCHAR(2), DATEPART(MONTH, d.ReadingDateTime)), 2) 
                         + '/' + RIGHT('0' + CONVERT(VARCHAR(2), DATEPART(DAY, d.ReadingDateTime)), 2) + ' ' + RIGHT('0' + CONVERT(VARCHAR(2), DATEPART(HOUR, 
                         d.ReadingDateTime)), 2) + ':00' AS DayHour, DATENAME(MONTH, d.ReadingDateTime) AS MonthName, MONTH(d.ReadingDateTime) AS MonthSort, 
                         DATEPART(DAYOFYEAR, d.ReadingDateTime) AS DayOfYear, { fn HOUR(d.ReadingDateTime) } AS Hour, CONVERT(char(10), d.ReadingDateTime, 101) 
                         AS ReadingDate
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.Locations ON a.LocationId = dbo.Locations.Id INNER JOIN
                         dbo.WaterBodies ON dbo.Locations.WaterBodyId = dbo.WaterBodies.Id
WHERE        (a.DatasetId = 1194) AND (d.QAStatusId = 1)

GO
/****** Object:  View [dbo].[WaterTempLine_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterTempLine_vw]
AS
SELECT        dbo.WaterBodies.Name AS Stream, dbo.Locations.Label AS LoggerSite, d.ReadingDateTime, d.AirTemperature, d.WaterTemperature, CONVERT(char(10), 
                         d.ReadingDateTime, 101) AS ReadingDate, dbo.QAStatus.Name AS RowQAStatus, a.InstrumentId, YEAR(d.ReadingDateTime) AS Year
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.Locations ON a.LocationId = dbo.Locations.Id INNER JOIN
                         dbo.WaterBodies ON dbo.Locations.WaterBodyId = dbo.WaterBodies.Id INNER JOIN
                         dbo.QAStatus ON d.QAStatusId = dbo.QAStatus.Id
WHERE        (a.DatasetId = 1198) AND (dbo.Locations.Label LIKE '%Line%' OR
                         dbo.Locations.Label LIKE '%529%')

GO
/****** Object:  View [dbo].[JvRearing_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create view [dbo].[JvRearing_Detail_VW]
AS
SELECT        Id, Action, Species, PointData, Result, ResultUnit, ActionComments, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt
FROM            dbo.JvRearing_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.JvRearing_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[JvRearing_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[JvRearing_Header_VW]
AS
SELECT        Id, AcclimationYear, HeaderComments, ActivityId, ByUserId, EffDt
FROM            dbo.JvRearing_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.JvRearing_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))

GO
/****** Object:  View [dbo].[JvRearing_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[JvRearing_vw]
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, h.Id AS JvRearing_Header_Id, h.AcclimationYear, 
                         h.HeaderComments, h.ByUserId, h.EffDt, d.Id AS JvRearing_Detail_Id, d.Action, d.Species, d.PointData, d.Result, d.ResultUnit, d.ActionComments, d.RowId, 
                         d.RowStatusId, d.ByUserId AS JvRearing_Detail_ByUserId, d.QAStatusId, d.EffDt AS JvRearing_Detail_EffDt, q.QAStatusId AS ActivityQAStatusId, 
                         q.UserId AS ActivityQAs_UserId, q.Comments, q.QAStatusName, l.Label AS LocationLabel
FROM            dbo.Activities AS a INNER JOIN
                         dbo.JvRearing_Header_VW AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.JvRearing_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS q ON a.Id = q.ActivityId INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id

GO
/****** Object:  View [dbo].[WTActivities_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WTActivities_vw]
AS
SELECT        TOP (100) PERCENT LEFT(a.Description, 4) AS CollectionYear, a.Description, a.DatasetId, a.LocationId, l.OtherAgencyId, l.Label, a.UserId, a.CreateDate, 
                         a.ActivityDate, h.FieldActivityType
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id RIGHT OUTER JOIN
                         dbo.WaterTemp_Header_VW AS h ON a.Id = h.ActivityId
WHERE        (l.LocationTypeId = 6) AND (h.FieldActivityType = 'Data File Upload')

GO
/****** Object:  View [dbo].[BSample_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create view [dbo].[BSample_Detail_VW]
AS
SELECT        Id, Sex, Mark, ForkLength, TotalLength, Weight, GeneticSampleId, ScaleId, SnoutId, LifeStage, Origin, Species, PITTagId, Tag, RadioTagId, FishComments, 
                         OtherTagId, KidneyId, PercentRetained, FinClip, TotalCount, RecordNumber, MEHPLength, SubSample, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, 
                         EffDt
FROM            dbo.BSample_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.BSample_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[BSample_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[BSample_Header_VW]
AS
SELECT        Id, SampleYear, Technicians, HeaderComments, CollectionType, ByUserId, EffDt, ActivityId
FROM            dbo.BSample_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.BSample_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))

GO
/****** Object:  View [dbo].[BSample_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[BSample_vw]
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, h.Id AS BSample_Header_Id, h.SampleYear, h.Technicians, 
                         h.HeaderComments, h.CollectionType, h.ByUserId AS BSample_Header_ByUserId, h.EffDt, d.Id AS BSample_Detail_Id, d.Sex, d.Mark, d.ForkLength, d.TotalLength, 
                         d.Weight, d.GeneticSampleId, d.ScaleId, d.SnoutId, d.LifeStage, d.Origin, d.Species, d.PITTagId, d.RadioTagId, d.FishComments, d.OtherTagId, d.KidneyId, d.Tag, 
                         d.PercentRetained, d.FinClip, d.TotalCount, d.RecordNumber, d.MEHPLength, d.SubSample, d.RowId, d.RowStatusId, d.ByUserId, d.QAStatusId, 
                         d.EffDt AS BSample_Detail_EffDt, q.QAStatusId AS ActivityQAStatusId, q.Comments, q.QAStatusName, q.UserId AS ActivityQAUserId, l.Label AS LocationLabel
FROM            dbo.Activities AS a INNER JOIN
                         dbo.BSample_Header_VW AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.BSample_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS q ON a.Id = q.ActivityId INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id

GO
/****** Object:  View [dbo].[WaterTemp_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterTemp_VW]
AS
SELECT        h.Id AS WaterTemp_Header_Id, h.Technicians, h.Comments, h.CollectionType, h.DepthToWater, h.FieldActivityType, h.StaticWaterLevel, h.WeatherConditions, 
                         h.SamplePeriod, h.SampleTempUnit, h.DeployTime, h.ByUserId, h.EffDt, d.Id AS WaterTemp_Detail_Id, d.ReadingDateTime, d.GMTReadingDateTime, 
                         d.WaterTemperature, d.WaterTemperatureF, d.AirTemperature, d.AirTemperatureF, d.WaterLevel, d.TempAToD, d.BatteryVolts, d.Conductivity, d.RowId, 
                         d.RowStatusId, d.ByUserId AS WaterTemp_Detail_ByUserId, d.QAStatusId, d.EffDt AS WaterTemp_Detail_EffDt, d.PSI, d.AbsolutePressure, 
                         aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, 
                         a.DatasetId, a.SourceId, a.LocationId, a.UserId AS Activity_UserId, a.ActivityTypeId, 
                         a.CreateDate, a.Id AS ActivityId, aq.QAStatusName, a.InstrumentId, a.ActivityDate, 
                         L.Label, w.Name, L.WaterBodyId, L.LocationTypeId, 
                         CASE WHEN d .QAStatusId > 1 THEN '' ELSE d .WaterTemperature END AS CorrectTemp
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS L ON a.LocationId = L.Id INNER JOIN
                         dbo.WaterBodies AS w ON L.WaterBodyId = w.Id
            
GO
/****** Object:  View [dbo].[HabitatSubprojects_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[HabitatSubprojects_vw]
AS
SELECT        p.Name, p.StartDate, p.EndDate, p.OwnerId, p.Description, mp.Name AS AttributeName, dbo.MetadataValues_vw.[Values], dbo.Users.Fullname
FROM            dbo.MetadataProperties AS mp INNER JOIN
                         dbo.MetadataValues_vw ON mp.Id = dbo.MetadataValues_vw.MetadataPropertyId RIGHT OUTER JOIN
                         dbo.Users INNER JOIN
                         dbo.Projects AS p ON dbo.Users.Id = p.OwnerId ON dbo.MetadataValues_vw.RelationId = p.Id

GO
/****** Object:  View [dbo].[ScrewTrapUM_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ScrewTrapUM_vw]
AS
SELECT        TOP (100) PERCENT h.FileTitle, h.ClipFiles, h.Tagger, h.LivewellTemp, h.TaggingTemp, h.PostTaggingTemp, h.ReleaseTemp, h.ArrivalTime, h.DepartTime, 
                         h.DepartureRPMs, h.Hubometer, h.HubometerTime, h.TrapStopped, h.TrapStarted, h.FishCollected, h.FishReleased, h.Flow, h.Turbidity, h.TrapDebris, h.RiverDebris, 
                         h.ActivityComments, h.DailyFinClips, d.Sequence, d.PitTagCode, d.ForkLength, d.Weight, d.OtherSpecies, d.FishCount, d.ConditionalComment, d.TextualComments, 
                         d.Note, d.ReleaseLocation, d.FishComments, d.TagStatus, d.ClipStatus, CASE WHEN SpeciesRunRearing IN ('00U', '25H', '25W', '3RH', '3RW', '7RW', '90U', 'A0W', 
                         'D0W', 'ERU', 'G0W') THEN '' WHEN d .ForkLength < 100 THEN 'Small' WHEN d .ForkLength > 120 THEN 'Large' WHEN d .ForkLength IS NULL THEN NULL 
                         ELSE 'Medium' END AS SizeClass, 
                         CASE WHEN SpeciesRunRearing = '00U' THEN 'Unknown' WHEN SpeciesRunRearing = '11H' THEN 'Hat. Spring Chinook' WHEN SpeciesRunRearing = '11U' THEN 'Spring Chinook (unknown r/t)'
                          WHEN SpeciesRunRearing = '11W' THEN 'Wild Spring Chinook' WHEN SpeciesRunRearing = '13H' THEN 'Hat. Fall Chinook' WHEN SpeciesRunRearing = '13W' THEN
                          'Wild Fall Chinook' WHEN SpeciesRunRearing = '25H' THEN 'Hat. Coho' WHEN SpeciesRunRearing = '25W' THEN 'Wild Coho' WHEN SpeciesRunRearing = '32H' THEN
                          'Hat. Summer Steelhead' WHEN SpeciesRunRearing = '32W' THEN 'Wild Summer Steelhead' WHEN SpeciesRunRearing = '3RH' THEN 'Hat. Rainbow Trout' WHEN SpeciesRunRearing
                          = '3RW' THEN 'Wild Rainbow Trout' WHEN SpeciesRunRearing = '7RW' THEN 'Bull Trout' WHEN SpeciesRunRearing = '90U' THEN 'Other' WHEN SpeciesRunRearing
                          = 'A0W' THEN 'Lamprey' WHEN SpeciesRunRearing = 'D0W' THEN 'Northern Pikeminnow' WHEN SpeciesRunRearing = 'ERU' THEN 'Brook Trout' WHEN SpeciesRunRearing
                          = 'G0W' THEN 'Mountain Whitefish' WHEN SpeciesRunRearing IS NULL THEN NULL ELSE 'Other' END AS SpeciesRunRearing, 
                         CASE WHEN SpeciesRunRearing IN ('11H', '13H', '25H', '32H', '3RH') THEN 'HAT' WHEN OtherSpecies IN ('HATCHERY CHS', 'HATCHERY STS') 
                         THEN 'HAT' WHEN SpeciesRunRearing IN ('11U', '11W', '13W', '25W', '32W', '3RW') THEN 'NAT' WHEN OtherSpecies IN ('CHINOOK', 'STEELHEAD') 
                         THEN 'NAT' WHEN FishCount IS NULL THEN NULL ELSE 'UNK' END AS HatNat, a.Id AS ActivityId, a.DatasetId, a.ActivityDate, w.Id AS WaterbodyId, 
                         w.Name AS WaterbodyName, l.Label AS LocationLabel, aq.QAStatusName, aq.Comments AS ActivityQAComments, h.ArrivalRPMs, 
                         d.SpeciesRunRearing AS SRRCode, d.FinClip, d.CaptureStatus, d.Status, d.ContainStudy, CASE WHEN Month(ActivityDate) >= 8 THEN Year(ActivityDate) 
                         + 1 WHEN Month(ActivityDate) <= 7 THEN Year(ActivityDate) ELSE ' ' END AS OutmigrationYear, d.EventType, d.AdditionalPositionalComments, d.SizeOfCount, 
                         d.Containment, d.MigrationYear, { fn WEEK(a.ActivityDate) } AS Week
FROM            dbo.ScrewTrap_DetailUM_VW AS d INNER JOIN
                         dbo.Screwtrap_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.Locations AS l ON l.Id = a.LocationId LEFT OUTER JOIN
                         dbo.WaterBodies AS w ON w.Id = l.WaterBodyId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
WHERE        (a.DatasetId = 1215)

GO
/****** Object:  View [dbo].[WaterTempSOhours_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterTempSOhours_vw]
AS
SELECT        dbo.WaterBodies.Name AS Stream, dbo.Locations.Label AS LoggerSite, d.ReadingDateTime, d.AirTemperature, d.WaterTemperature, CONVERT(char(10), 
                         d.ReadingDateTime, 101) AS ReadingDate, dbo.QAStatus.Name AS RowQAStatus, YEAR(d.ReadingDateTime) AS ReadingYear
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.Locations ON a.LocationId = dbo.Locations.Id INNER JOIN
                         dbo.WaterBodies ON dbo.Locations.WaterBodyId = dbo.WaterBodies.Id INNER JOIN
                         dbo.QAStatus ON d.QAStatusId = dbo.QAStatus.Id
WHERE        (a.DatasetId = 1196)

GO
/****** Object:  View [dbo].[WQXAirH20_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WQXAirH20_vw]
AS
SELECT        h.CollectionType, h.FieldActivityType, CAST(d.ReadingDateTime AS DATE) AS ReadingDate, MAX(d.WaterTemperature) AS MaxH20, MAX(d.AirTemperature) 
                         AS MaxAir, MIN(d.WaterTemperature) AS MinH20, MIN(d.AirTemperature) AS MinAir, AVG(d.WaterTemperature) AS MeanH20, AVG(d.AirTemperature) AS MeanAir, 
                         d.QAStatusId, a.LocationId, a.InstrumentId, loc.Label, REPLACE(loc.OtherAgencyId, 'CTUIR_', '') AS MonitoringLocationID, CONVERT(nvarchar(50), 
                         REPLACE(loc.OtherAgencyId, 'CTUIR_', '')) + CONVERT(nvarchar(10), d.ReadingDateTime, 112) AS ActivityId, loc.WaterBodyId, loc.LocationTypeId
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS loc ON loc.Id = a.LocationId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
GROUP BY h.CollectionType, h.FieldActivityType, CAST(d.ReadingDateTime AS DATE), d.QAStatusId, aq.QAStatusId, a.DatasetId, a.LocationId, aq.QAStatusName, a.InstrumentId, 
                         loc.Label, loc.OtherAgencyId, REPLACE(loc.OtherAgencyId, 'CTUIR_', ''), CONVERT(nvarchar(50), REPLACE(loc.OtherAgencyId, 'CTUIR_', '')) + CONVERT(nvarchar(10), 
                         d.ReadingDateTime, 112), loc.WaterBodyId, loc.LocationTypeId
HAVING        (d.QAStatusId = 1)

GO
/****** Object:  View [dbo].[WaterTempArcGIS_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterTempArcGIS_vw]
AS
SELECT        YEAR(d.ReadingDateTime) AS ReadingYear, w.Name AS WaterBody, h.CollectionType, L.OtherAgencyId AS AgencyId, L.Label AS LoggerSite, h.FieldActivityType, 
                         d.ReadingDateTime, d.WaterTemperature AS WaterTempC, d.AirTemperature AS AirTempC, dbo.QAStatus.Name AS RowQA, h.Comments, L.Id AS LocationId
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS L ON a.LocationId = L.Id INNER JOIN
                         dbo.WaterBodies AS w ON L.WaterBodyId = w.Id INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON h.ActivityId = d.ActivityId INNER JOIN
                         dbo.QAStatus ON d.QAStatusId = dbo.QAStatus.Id
WHERE        (L.LocationTypeId = 6)

GO
/****** Object:  View [dbo].[WaterTempAllLogger_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterTempAllLogger_vw]
AS
SELECT        YEAR(d.ReadingDateTime) AS ReadingYear, w.Name AS WaterBody, h.CollectionType, L.OtherAgencyId AS AgencyId, L.Label AS LoggerSite, h.FieldActivityType, 
                         d.ReadingDateTime, d.WaterTemperature AS WaterTempC, d.AirTemperature AS AirTempC, dbo.QAStatus.Name AS RowQA, h.Comments, L.Id AS LocationId
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS L ON a.LocationId = L.Id INNER JOIN
                         dbo.WaterBodies AS w ON L.WaterBodyId = w.Id INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON h.ActivityId = d.ActivityId INNER JOIN
                         dbo.QAStatus ON dbo.QAStatus.Id = d.QAStatusId
WHERE        (L.LocationTypeId = 6)

GO
/****** Object:  View [dbo].[WaterTempLineJoin_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterTempLineJoin_vw]
AS
SELECT DISTINCT TOP (100) PERCENT dbo.Locations.Label AS LoggerSite, a.InstrumentId, YEAR(d.ReadingDateTime) AS Year
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.Locations ON a.LocationId = dbo.Locations.Id INNER JOIN
                         dbo.WaterBodies ON dbo.Locations.WaterBodyId = dbo.WaterBodies.Id INNER JOIN
                         dbo.QAStatus ON d.QAStatusId = dbo.QAStatus.Id
WHERE        (a.DatasetId = 1198) AND (dbo.Locations.Label LIKE '%Line%' OR
                         dbo.Locations.Label LIKE '%529%') AND (a.InstrumentId IS NOT NULL)
ORDER BY LoggerSite, Year

GO
/****** Object:  View [dbo].[WaterTempInstruQA_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterTempInstruQA_vw]
AS
SELECT DISTINCT 
                         dbo.Instruments.SerialNumber, dbo.InstrumentAccuracyChecks.CheckDate, dbo.InstrumentAccuracyChecks.CheckMethod, 
                         dbo.InstrumentAccuracyChecks.Bath1Grade, dbo.InstrumentAccuracyChecks.Bath2Grade, dbo.InstrumentAccuracyChecks.Comments, dbo.Instruments.Id, 
                         YEAR(dbo.InstrumentAccuracyChecks.CheckDate) AS Year
FROM            dbo.Instruments LEFT OUTER JOIN
                         dbo.InstrumentAccuracyChecks ON dbo.Instruments.Id = dbo.InstrumentAccuracyChecks.InstrumentId
WHERE        (dbo.Instruments.SerialNumber IS NOT NULL) AND (dbo.InstrumentAccuracyChecks.CheckDate IS NOT NULL)

GO
/****** Object:  View [dbo].[WaterTempLineQAFinal_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterTempLineQAFinal_vw]
AS
SELECT        TOP (100) PERCENT dbo.WaterTempLineJoin_vw.LoggerSite, dbo.WaterTempLineJoin_vw.Year, dbo.WaterTempInstruQA_vw.SerialNumber, 
                         dbo.WaterTempInstruQA_vw.CheckDate, dbo.WaterTempInstruQA_vw.CheckMethod, dbo.WaterTempInstruQA_vw.Bath1Grade, 
                         dbo.WaterTempInstruQA_vw.Bath2Grade, dbo.WaterTempInstruQA_vw.Comments
FROM            dbo.WaterTempLineJoin_vw INNER JOIN
                         dbo.WaterTempInstruQA_vw ON dbo.WaterTempLineJoin_vw.InstrumentId = dbo.WaterTempInstruQA_vw.Id AND 
                         dbo.WaterTempLineJoin_vw.Year = dbo.WaterTempInstruQA_vw.Year
ORDER BY dbo.WaterTempLineJoin_vw.LoggerSite, dbo.WaterTempLineJoin_vw.Year

GO
/****** Object:  View [dbo].[WaterQuality_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterQuality_Detail_VW]
AS
SELECT        Id, CharacteristicName, Result, ResultUnits, LabDuplicate, Comments, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, MdlResults, SampleDate, 
                         SampleID, SampleFraction, MethodSpeciation, DetectionLimit, ContextID, MethodID, LabName
FROM            dbo.WaterQuality_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.WaterQuality_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[WaterQuality_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[WaterQuality_Header_VW]
as
SELECT        Id, DataType, ActivityId, ByUserId, EffDt, FieldsheetLink, HeaderComments
FROM            dbo.WaterQuality_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.WaterQuality_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))

GO
/****** Object:  View [dbo].[WaterQuality_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterQuality_vw]
AS
SELECT        TOP (100) PERCENT loc.OtherAgencyId, d.SampleDate, d.CharacteristicName, d.Result, 
                         CASE d .ResultUnits WHEN 'FT' THEN 'ft' WHEN 'G/L' THEN 'g/L' WHEN 'INHG' THEN 'inHg' WHEN 'M' THEN 'm' WHEN 'MG/L' THEN 'mg/L' WHEN 'MG/M3' THEN 'mg/m3'
                          WHEN 'MMHG' THEN 'mmHg' WHEN 'NG/KG' THEN 'ng/kg' WHEN 'UG/KG' THEN 'ug/kg' WHEN 'UG/L' THEN 'ug/L' WHEN 'UMHO/CM' THEN 'umho/cm' WHEN 'CFU/100ML'
                          THEN 'cfu/100mL' WHEN 'NG/L' THEN 'ng/L' WHEN 'US/CM' THEN 'uS/cm' ELSE ResultUnits END AS ResultUnits, d.MdlResults, d.SampleFraction, 
                         d.MethodSpeciation, d.ContextID, d.MethodID, d.DetectionLimit, CASE WHEN (Result IS NULL) AND DataType = 'ISCO' THEN 'Ignore' WHEN (Result IS NULL) AND 
                         DataType = 'Handheld' THEN 'Not Reported' WHEN (Result IS NULL) AND MdlResults LIKE ('%>%') THEN 'Present Above Quantification Limit' WHEN (Result IS NULL)
                          AND MdlResults LIKE ('%<%') THEN 'Present Below Quantification Limit' WHEN (Result IS NULL) AND MdlResults LIKE ('%Present%') 
                         THEN 'Present Below Quantification Limit' WHEN (Result IS NULL) THEN 'Not Detected' ELSE '' END AS ResultDetectionCondition, d.LabDuplicate, d.LabName, 
                         d.SampleID, d.Comments, h.DataType, h.FieldsheetLink, h.HeaderComments, a.Id AS ActivityId, a.DatasetId, a.InstrumentId, a.ActivityDate, a.CreateDate, 
                         loc.Id AS LocationId, aq.QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId, aq.QAStatusId AS ActivityQAStatusId, d.RowId, d.RowStatusId, 
                         CASE DataType WHEN 'ISCO' THEN 'Field Msr/Obs-Portable Data Logger' WHEN 'Handheld' THEN 'Field Msr/Obs-Portable Data Logger' WHEN 'Lab' THEN 'Sample-Routine'
                          ELSE '' END AS ActivityType, 
                         CASE CharacteristicName WHEN 'Conductivity-TDS' THEN 'Calculated' WHEN 'Conductivity-Spec Cond' THEN 'Calculated' ELSE 'Actual' END AS ResultValueType, 
                         CASE d .ResultUnits WHEN 'FT' THEN 'ft' WHEN 'G/L' THEN 'g/L' WHEN 'INHG' THEN 'inHg' WHEN 'M' THEN 'm' WHEN 'MG/L' THEN 'mg/L' WHEN 'MG/M3' THEN 'mg/m3'
                          WHEN 'MMHG' THEN 'mmHg' WHEN 'NG/KG' THEN 'ng/kg' WHEN 'UG/KG' THEN 'ug/kg' WHEN 'UG/L' THEN 'ug/L' WHEN 'UMHO/CM' THEN 'umho/cm' WHEN 'CFU/100ML'
                          THEN 'cfu/100mL' WHEN 'NG/L' THEN 'ng/L' WHEN 'US/CM' THEN 'uS/cm' ELSE d .ResultUnits END AS ResultDetectionUnit
FROM            dbo.WaterQuality_Detail_VW AS d INNER JOIN
                         dbo.WaterQuality_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId INNER JOIN
                         dbo.Locations AS loc ON loc.Id = a.LocationId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id

GO
/****** Object:  View [dbo].[WaterTempUploadCheck_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterTempUploadCheck_VW]
AS
SELECT        h.FieldActivityType AS ActivityType, a.Id, s.Name AS DataSet, l.Label AS Location, h.CollectionType, a.Description AS DateRange
FROM            dbo.Datasets AS s INNER JOIN
                         dbo.Activities AS a ON s.Id = a.DatasetId INNER JOIN
                         dbo.WaterTemp_Header_VW AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id
WHERE        (h.FieldActivityType = 'Data File Upload')

GO
/****** Object:  View [dbo].[ScrewTrapWW_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ScrewTrapWW_vw]
AS
SELECT        TOP (100) PERCENT d.MigrationYear, w.Name AS WaterbodyName, l.Label AS LocationLabel, a.ActivityDate, d.Sequence, d.SpeciesRunRearing AS SRRCode, 
                         d.PitTagCode, d.ForkLength, d.Weight, d.FishCount, d.OtherSpecies, d.AdditionalPositionalComments, d.ConditionalComment, d.TextualComments, 
                         d.ReleaseLocation, d.TagStatus, d.ClipStatus, CASE WHEN SpeciesRunRearing IN ('00U', '25H', '25W', '3RH', '3RW', '7RW', '90U', 'A0W', 'D0W', 'ERU', 'G0W') 
                         THEN '' WHEN d .ForkLength < 100 THEN 'Small' WHEN d .ForkLength > 120 THEN 'Large' ELSE 'Medium' END AS SizeClass, 
                         CASE WHEN SpeciesRunRearing = '00U' THEN 'Unknown (fish not observed)' WHEN SpeciesRunRearing = '11H' THEN 'Hat. Spring Chinook' WHEN SpeciesRunRearing
                          = '11U' THEN 'Spring Chinook (unknown r/t)' WHEN SpeciesRunRearing = '11W' THEN 'Wild Spring Chinook' WHEN SpeciesRunRearing = '13H' THEN 'Hat. Fall Chinook'
                          WHEN SpeciesRunRearing = '13W' THEN 'Wild Fall Chinook' WHEN SpeciesRunRearing = '25H' THEN 'Hat. Coho' WHEN SpeciesRunRearing = '25W' THEN 'Wild Coho'
                          WHEN SpeciesRunRearing = '32H' THEN 'Hat. Summer Steelhead' WHEN SpeciesRunRearing = '32W' THEN 'Wild Summer Steelhead' WHEN SpeciesRunRearing =
                          '3RH' THEN 'Hat. Rainbow Trout' WHEN SpeciesRunRearing = '3RW' THEN 'Wild Rainbow Trout' WHEN SpeciesRunRearing = '7RW' THEN 'Bull Trout' WHEN SpeciesRunRearing
                          = '90U' THEN 'Other' WHEN SpeciesRunRearing = 'A0W' THEN 'Lamprey' WHEN SpeciesRunRearing = 'D0W' THEN 'Northern Pikeminnow' WHEN SpeciesRunRearing
                          = 'ERU' THEN 'Brook Trout' WHEN SpeciesRunRearing = 'G0W' THEN 'Mountain Whitefish' ELSE 'Other' END AS SpeciesRunRearing, 
                         CASE WHEN SpeciesRunRearing IN ('11H', '13H', '25H', '32H', '3RH') THEN 'HAT' WHEN OtherSpecies IN ('HATCHERY CHS', 'HATCHERY STS') 
                         THEN 'HAT' WHEN SpeciesRunRearing IN ('11U', '11W', '13W', '25W', '32W', '3RW') THEN 'NAT' WHEN OtherSpecies IN ('CHINOOK', 'STEELHEAD') 
                         THEN 'NAT' ELSE 'UNK' END AS HatNat, d.FinClip, d.CaptureStatus, d.Status, d.ContainStudy, CASE WHEN Month(ActivityDate) >= 8 THEN Year(ActivityDate) 
                         + 1 WHEN Month(ActivityDate) <= 7 THEN Year(ActivityDate) ELSE ' ' END AS TrappingSeason, d.EventType, d.LifeStage, d.SizeOfCount, d.Containment
FROM            dbo.ScrewTrap_DetailUM_VW AS d INNER JOIN
                         dbo.Screwtrap_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.Locations AS l ON l.Id = a.LocationId LEFT OUTER JOIN
                         dbo.WaterBodies AS w ON w.Id = l.WaterBodyId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
WHERE        (a.DatasetId = 1216)

GO
/****** Object:  View [dbo].[WaterTemp_VWold]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterTemp_VWold]
AS
SELECT        h.Technicians, h.Comments, h.CollectionType, h.FieldActivityType, d.ReadingDateTime, d.WaterTemperature, d.WaterTemperatureF, d.AirTemperature, 
                         d.AirTemperatureF, aq.Comments AS ActivityQAComments, a.LocationId
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id

GO
/****** Object:  View [dbo].[Metrics_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Metrics_Detail_VW]
AS
SELECT        Id, WorkElementName, Measure, PlannedValue, ActualValue, Comments, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt
FROM            dbo.Metrics_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Metrics_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[Metrics_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Metrics_Header_VW]
AS
SELECT        Id, YearReported, ActivityId, ByUserId, EffDt
FROM            dbo.Metrics_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Metrics_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))

GO
/****** Object:  View [dbo].[Metrics_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Metrics_vw]
AS
SELECT        h.Id, h.YearReported, h.ByUserId, h.EffDt, a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, d.Id AS Metrics_Detail_Id, 
                         d.WorkElementName, d.Measure, d.PlannedValue, d.ActualValue, d.Comments, d.RowId, d.RowStatusId, d.ByUserId AS Metrics_Detail_ByUserId, d.QAStatusId, 
                         d.EffDt AS Metrics_Detail_EffDt
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Metrics_Header_VW AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.Metrics_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId INNER JOIN
                         dbo.Locations ON a.LocationId = dbo.Locations.Id

GO
/****** Object:  View [dbo].[Electrofishing_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Electrofishing_Detail_VW]
AS
SELECT        Id, Sequence, PitTagCode, SpeciesRunRearing, ForkLength, Weight, OtherSpecies, FishCount, ConditionalComment, TextualComments, Note, OtolithID, GeneticID, 
                         OtherID, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, TagStatus, ClipStatus, SizeCategory, TotalLength, AdditionalPositionalComment, 
                         ChannelUnitType, EventTypeD, SecondPitTag, RacewayTransectTank, LifeStage, CodedWireTag, BroodYear, MigrationYear, SizeOfCount, ScaleId, Containment, 
                         PassNumber
FROM            dbo.Electrofishing_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Electrofishing_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[Electrofishing_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Electrofishing_Header_VW]
AS
SELECT        *
FROM            dbo.Electrofishing_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Electrofishing_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)));

GO
/****** Object:  View [dbo].[Electrofishing_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Electrofishing_vw]
AS
SELECT        h.Id, h.FishNumber, h.EventType, h.FileTitle, h.ClipFiles, h.Crew, h.ReleaseTemp, h.Conductivity, h.EFModel, h.SiteLength, h.SiteWidth, h.SiteDepth, h.SiteArea, 
                         h.HabitatType, h.Visibility, h.ActivityComments, h.ReleaseSite, h.Weather, h.ReleaseRiverKM, h.TotalFishCaptured, h.ReleaseLocation, h.VisitID, h.Unit, h.StartTime, 
                         h.EndTime, h.ReleaseTime, h.StartTemp, h.Pass1TimeBegin, h.Pass1TimeEnd, h.Pass1TotalSecondsEF, h.Pass1WaterTempBegin, h.Pass1WaterTempStop, 
                         h.Pass1Hertz, h.Pass1Freq, h.Pass1Volts, h.Pass2TimeBegin, h.Pass2TimeEnd, h.Pass2TotalSecondsEF, h.Pass2WaterTempBegin, h.Pass2WaterTempStop, 
                         h.Pass2Hertz, h.Pass2Freq, h.Pass2Volts, h.Pass3TimeBegin, h.Pass3TimeEnd, h.Pass3TotalSecondsEF, h.Pass3WaterTempBegin, h.Pass3WaterTempStop, 
                         h.Pass3Hertz, h.Pass3Freq, h.Pass3Volts, h.Pass4TimeBegin, h.Pass4TimeEnd, h.Pass4TotalSecondsEF, h.Pass4WaterTempBegin, h.Pass4WaterTempStop, 
                         h.Pass4Hertz, h.Pass4Freq, h.Pass4Volts, h.Pass5TimeBegin, h.Pass5TimeEnd, h.Pass5TotalSecondsEF, h.Pass5WaterTempBegin, h.Pass5WaterTempStop, 
                         h.Pass5Hertz, h.Pass5Freq, h.Pass5Volts, h.Pass6TimeBegin, h.Pass6TimeEnd, h.Pass6TotalSecondsEF, h.Pass6WaterTempBegin, h.Pass6WaterTempStop, 
                         h.Pass6Hertz, h.Pass6Freq, h.Pass6Volts, h.FieldsheetLink, d.Sequence, d.PitTagCode, d.SpeciesRunRearing, d.ForkLength, d.Weight, d.OtherSpecies, d.FishCount, 
                         d.ConditionalComment, d.TextualComments, d.Note, d.OtolithID, d.GeneticID, d.OtherID, d.RowId, d.RowStatusId, d.ByUserId, d.QAStatusId, d.TagStatus, d.ClipStatus, 
                         d.SizeCategory, d.ChannelUnitType, d.AdditionalPositionalComment, d.TotalLength, d.EventTypeD, d.SecondPitTag, d.RacewayTransectTank, d.LifeStage, 
                         d.CodedWireTag, d.BroodYear, d.MigrationYear, d.SizeOfCount, d.ScaleId, d.Containment, d.PassNumber, a.Id AS ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, 
                         a.ActivityDate, a.CreateDate, w.Id AS WaterbodyId, w.Name AS WaterbodyName, l.Id AS LocationId, l.Name AS LocationLabel, aq.QAStatusName, 
                         aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId
FROM            dbo.Electrofishing_Detail_VW AS d INNER JOIN
                         dbo.Electrofishing_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId INNER JOIN
                         dbo.Locations AS l ON l.Id = a.LocationId INNER JOIN
                         dbo.WaterBodies AS w ON w.Id = l.WaterBodyId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id

GO
/****** Object:  View [dbo].[ArtificialProduction_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ArtificialProduction_Detail_VW]
AS
SELECT        Id, RunYear, Species, Origin, Sex, Disposition, TotalFishRepresented, LifeStage, FinClip, Tag, NumberEggsTaken, ReleaseSite, FishComments, RowId, RowStatusId, 
                         ActivityId, ByUserId, QAStatusId, EffDt, ProgramGroup, EyedEggs
FROM            dbo.ArtificialProduction_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.ArtificialProduction_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[ArtificialProduction_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ArtificialProduction_Header_VW]
AS
SELECT        Id, Comments, FieldSheetFile, ActivityId, ByUserId, EffDt
FROM            dbo.ArtificialProduction_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.ArtificialProduction_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))

GO
/****** Object:  View [dbo].[ArtificialProduction_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ArtificialProduction_VW]
AS
SELECT        h.Id AS ArtificialProduction_Header_Id, h.Comments, h.FieldSheetFile, h.ByUserId, h.EffDt, d.Id AS ArtificialProduction_Detail_Id, d.RunYear, d.Species, d.Origin, 
                         d.Sex, d.Disposition, d.TotalFishRepresented, d.LifeStage, REPLACE(REPLACE(d.FinClip, '"]', ''), '["', '') AS FinClip, REPLACE(REPLACE(d.Tag, '"]', ''), '["', '') AS Tag, 
                         d.NumberEggsTaken, d.ReleaseSite, d.FishComments, d.RowId, d.RowStatusId, d.ByUserId AS ArtificialProduction_Detail_ByUserId, d.ProgramGroup, d.QAStatusId, 
                         d.EffDt AS ArtificialProduction_Detail_EffDt, aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId, 
                         a.SourceId, a.LocationId, a.UserId AS Activity_UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, a.Id AS ActivityId, aq.QAStatusName, dbo.Locations.Label, 
                         d.EyedEggs
FROM            dbo.Activities AS a INNER JOIN
                         dbo.ArtificialProduction_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.ArtificialProduction_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations ON a.LocationId = dbo.Locations.Id

GO
/****** Object:  View [dbo].[WQWeb_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WQWeb_vw]
AS
SELECT        TOP (100) PERCENT YEAR(d.SampleDate) AS ReadingYear, 'Water Chemistry' AS DataType, h.DataType AS CollectionMethod, w.Name AS Stream, loc.Label AS Site, 
                         loc.OtherAgencyId AS [Agency Id], d.SampleDate, d.CharacteristicName, d.Result, 
                         CASE d .ResultUnits WHEN 'FT' THEN 'ft' WHEN 'G/L' THEN 'g/L' WHEN 'INHG' THEN 'inHg' WHEN 'M' THEN 'm' WHEN 'MG/L' THEN 'mg/L' WHEN 'MG/M3' THEN 'mg/m3'
                          WHEN 'MMHG' THEN 'mmHg' WHEN 'NG/KG' THEN 'ng/kg' WHEN 'UG/KG' THEN 'ug/kg' WHEN 'UG/L' THEN 'ug/L' WHEN 'UMHO/CM' THEN 'umho/cm (micro mho/cm)'
                          WHEN 'CFU/100ML' THEN 'cfu/100mL' WHEN 'NG/L' THEN 'ng/L' WHEN 'US/CM' THEN 'uS/cm' ELSE ResultUnits END AS ResultUnits, d.SampleFraction, 
                         d.MethodSpeciation, d.ContextID AS Context, d.MethodID AS Method, d.DetectionLimit, CASE WHEN (Result IS NULL) AND 
                         DataType = 'ISCO' THEN 'Ignore' WHEN (Result IS NULL) AND DataType = 'Handheld' THEN 'Not Reported' WHEN (Result IS NULL) AND MdlResults LIKE ('%>%') 
                         THEN 'Present Above Quantification Limit' WHEN (Result IS NULL) AND MdlResults LIKE ('%<%') THEN 'Present Below Quantification Limit' WHEN (Result IS NULL) 
                         AND MdlResults LIKE ('%Present%') THEN 'Present Below Quantification Limit' WHEN (Result IS NULL) 
                         THEN 'Not Detected' ELSE '' END AS ResultDetectionCondition, d.LabDuplicate, d.LabName, d.Comments AS [Analyte Comments], 'OK' AS RowQA, 
                         h.HeaderComments AS [Activity Comment], loc.GPSEasting, loc.GPSNorthing, loc.Projection, loc.UTMZone, w.GNIS_ID
FROM            dbo.WaterQuality_Detail_VW AS d INNER JOIN
                         dbo.WaterQuality_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId INNER JOIN
                         dbo.Locations AS loc ON loc.Id = a.LocationId INNER JOIN
                         dbo.WaterBodies AS w ON loc.WaterBodyId = w.Id

GO
/****** Object:  View [dbo].[WTDetailsWeb_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WTDetailsWeb_vw]
AS
SELECT        Id, ReadingDateTime, AirTemperature, WaterTemperature, QAStatusId, EffDt, ActivityId
FROM            dbo.WaterTemp_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.WaterTemp_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[WTWeb_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WTWeb_VW]
AS
SELECT        YEAR(d .ReadingDateTime) AS ReadingYear, 'Water Temperature' AS DataType, 'Logger' AS CollectionMethod, w.Name AS Stream, L.Label AS Site, 
                         L.OtherAgencyId AS AgencyId, d .ReadingDateTime AS SampleDateTime, 'WaterTemperature' AS CharacterisiticName, 
                         d .WaterTemperature AS Result, 'C' AS ResultUnits, 'NULL' AS SampleFraction, 'NULL' AS MethodSpeciation, 
                         'NULL' AS Context, 'NULL' AS Method, 'NULL' AS DetectionLimit, 'NULL' AS ResultDetectionCondition, 'NULL' AS LabDuplicate, 'NULL' AS LabName, 
                         'NULL' AS AnalyteComments, dbo.QAStatus.Name AS RowQA, h.Comments AS ActivityComments, L.GPSEasting, L.GPSNorthing, L.Projection, L.UTMZone, 
                         w.GNIS_Id
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS L ON a.LocationId = L.Id INNER JOIN
                         dbo.WaterBodies AS w ON L.WaterBodyId = w.Id INNER JOIN
                         dbo.WTDetailsWeb_vw AS d ON h.ActivityId = d .ActivityId INNER JOIN
                         dbo.QAStatus ON dbo.QAStatus.Id = d .QAStatusId
WHERE        (L.LocationTypeId = 6) AND d .WaterTemperature IS NOT NULL
UNION ALL
SELECT        YEAR(d .ReadingDateTime) AS ReadingYear, 'Air Temperature' AS DataType, 'Logger' AS CollectionMethod, w.Name AS Stream, L.Label AS Site, 
                         L.OtherAgencyId AS AgencyId, d .ReadingDateTime AS SampleDateTime, 'AirTemperature' AS CharacterisiticName, d .AirTemperature AS Result, 'C' AS ResultUnits, 'NULL' AS SampleFraction, 'NULL' AS MethodSpeciation, 'NULL' AS Context, 'NULL' AS Method, 
                         'NULL' AS DetectionLimit, 'NULL' AS ResultDetectionCondition, 'NULL' AS LabDuplicate, 'NULL' AS LabName, 'NULL' AS AnalyteComments, 
                         dbo.QAStatus.Name AS RowQA, h.Comments AS ActivityComments, L.GPSEasting, L.GPSNorthing, L.Projection, L.UTMZone, w.GNIS_Id
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS L ON a.LocationId = L.Id INNER JOIN
                         dbo.WaterBodies AS w ON L.WaterBodyId = w.Id INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON h.ActivityId = d .ActivityId INNER JOIN
                         dbo.QAStatus ON dbo.QAStatus.Id = d .QAStatusId
WHERE        (L.LocationTypeId = 6) AND d .AirTemperature IS NOT NULL

GO
/****** Object:  View [dbo].[SnorkelFish_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SnorkelFish_Detail_VW]
AS
SELECT        Id, FishID, ChannelUnitNumber, Lane, FishCount, Species, SizeClass, NaturalWoodUsed, PlacedWoodUsed, NaturalBoulderUsed, PlacedBoulderUsed, 
                         NaturalOffChannelUsed, CreatedOffChannelUsed, NewSideChannelUsed, NoStructureUsed, FieldNotes, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, 
                         NoSnorklers, Type, ChannelAverageDepth, ChannelLength, ChannelWidth, ChannelMaxDepth, UnidentifiedSalmonID, OtherSpeciesPres, AmbientTemp, 
                         MinimumTemp, ChannelUnitType, AEMHabitatType, AEMLength, Unit
FROM            dbo.SnorkelFish_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SnorkelFish_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[SnorkelFish_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SnorkelFish_Header_VW]
AS
SELECT        Id, Team, StartWaterTemp, Visibility, VisitId, Comments, CollectionType, ActivityId, ByUserId, EffDt, NoteTaker, StartTime, EndTime, WeatherConditions, 
              DominantSpecies, CommonSpecies, RareSpecies, IsAEM, HabitatVisitId, EndWaterTemp
FROM            dbo.SnorkelFish_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SnorkelFish_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))

GO
/****** Object:  View [dbo].[SnorkelFish_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SnorkelFish_vw]
AS
SELECT        h.Team, h.NoteTaker, h.StartTime, h.EndTime, h.StartWaterTemp, h.Visibility, h.WeatherConditions, h.VisitId, h.Comments, h.CollectionType, h.DominantSpecies, 
                         h.CommonSpecies, h.RareSpecies, iif(h.IsAEM = 'YES', AEMHabitatType, ChannelUnitType) AS UnitType, h.IsAEM, h.HabitatVisitId, h.EndWaterTemp,
                         d.NoSnorklers, d.FishID, d.ChannelUnitNumber, d.Lane, d.Type, d.ChannelAverageDepth, d.ChannelLength, d.ChannelWidth, d.ChannelMaxDepth, d.Unit,
                         d.ChannelLength * d.ChannelWidth AS ChannelArea, d.FishCount, d.Species, d.SizeClass, d.UnidentifiedSalmonID, d.OtherSpeciesPres, d.NaturalWoodUsed, 
                         d.PlacedWoodUsed, d.NaturalBoulderUsed, d.PlacedBoulderUsed, d.NaturalOffChannelUsed, d.CreatedOffChannelUsed, d.NewSideChannelUsed, 
                         d.NoStructureUsed, d.AmbientTemp, d.MinimumTemp, d.FieldNotes, d.ChannelUnitType, d.AEMHabitatType, d.AEMLength, d.RowId, d.RowStatusId, 
                         d.QAStatusId, a.id AS ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, a.CreateDate, w.id AS WaterbodyId, w.name AS WaterbodyName, 
                         l.id AS LocationId, l.name AS LocationLabel, aq.QAStatusName AS QAStatusName, aq.Comments AS ActivityQAComments, 
                         aq.QAStatusId AS ActivityQAStatusId
/* required*/ FROM SnorkelFish_Detail_VW d JOIN
                         SnorkelFish_Header_VW h ON d.ActivityId = h.ActivityId JOIN
                         activities a ON a.id = h.ActivityId JOIN
                         locations l ON l.id = a.locationid JOIN
                         waterbodies w ON w.id = l.waterbodyid JOIN
                         ActivityQAs_VW AS aq ON aq.ActivityId = a.Id

GO
/****** Object:  View [dbo].[HabitatWebmapCollaborators_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[HabitatWebmapCollaborators_vw]
AS
SELECT [ProjectId], [SubprojectId], STUFF((SELECT ', ' + A.[Name] FROM dbo.Fundings A
WHERE A.[SubprojectId]=B.[SubprojectId] FOR XML PATH('')),1,1,'') As [Collaborator]
FROM dbo.Fundings B
Group By [ProjectId], [SubprojectId]

GO
/****** Object:  View [dbo].[HabitatWebMapFunding_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[HabitatWebMapFunding_vw]
AS
SELECT [ProjectId], [SubprojectId], STUFF((SELECT ', ' + A.[Name] + ': $' + REPLACE(CONVERT(VARCHAR,   CONVERT(MONEY, Amount), 1), '.00', '') FROM dbo.Fundings A
WHERE A.[SubprojectId]=B.[SubprojectId] FOR XML PATH('')),1,1,'') As [Funding]
FROM dbo.Fundings B
Group By [ProjectId], [SubprojectId]



GO
/****** Object:  View [dbo].[HabitatWebMap_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[HabitatWebMap_vw]
AS
SELECT        w.Description AS Basin, w.Name AS Stream, sp.ProjectName, sp.ProjectSummary, sp.ProjectStartDate AS [Start Date], sp.ProjectEndDate AS [End Date], f.Funding, 
                         c.Collaborator, CASE WHEN sp.FirstFoods LIKE '%Water%' THEN 1 ELSE 0 END AS Water, 
                         CASE WHEN sp.FirstFoods LIKE '%Salmon%' THEN 1 ELSE 0 END AS Salmon, CASE WHEN sp.FirstFoods LIKE '%Deer%' THEN 1 ELSE 0 END AS Deer, 
                         CASE WHEN sp.FirstFoods LIKE '%Cous%' THEN 1 ELSE 0 END AS Cous, CASE WHEN sp.FirstFoods LIKE '%Huckle%' THEN 1 ELSE 0 END AS Huckleberry, 
                         CASE WHEN sp.RiverVisionTouchstone LIKE '%Aquatic%' THEN 1 ELSE 0 END AS AquaticBiota, 
                         CASE WHEN sp.RiverVisionTouchstone LIKE '%Connectivity%' THEN 1 ELSE 0 END AS Connectivity, 
                         CASE WHEN sp.RiverVisionTouchstone LIKE '%Geomorph%' THEN 1 ELSE 0 END AS Geomorphology, 
                         CASE WHEN sp.RiverVisionTouchstone LIKE '%Riparian%' THEN 1 ELSE 0 END AS RiparianVegetation, 
                         CASE WHEN sp.RiverVisionTouchstone LIKE '%Water%' THEN 1 ELSE 0 END AS RVWater, l.GPSEasting, l.GPSNorthing, sp.FeatureImage
FROM            dbo.Subproject_Hab AS sp INNER JOIN
                         dbo.Locations AS l ON sp.LocationId = l.Id INNER JOIN
                         dbo.WaterBodies AS w ON l.WaterBodyId = w.Id LEFT OUTER JOIN
                         dbo.HabitatWebMapFunding_vw AS f ON sp.Id = f.SubprojectId LEFT OUTER JOIN
                         dbo.HabitatWebmapCollaborators_vw AS c ON sp.Id = c.SubprojectId

GO
/****** Object:  View [dbo].[FishTransport_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FishTransport_Detail_VW]
AS
SELECT        *
FROM            dbo.FishTransport_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.FishTransport_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId)))
AND RowStatusId = 0;
GO
/****** Object:  View [dbo].[FishTransport_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FishTransport_Header_VW]
AS
SELECT        *
FROM            dbo.FishTransport_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.FishTransport_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)));
GO
/****** Object:  View [dbo].[FishTransport_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FishTransport_VW]
AS
SELECT        h.Id as FishTransport_Header_Id, h.Comments, h.ByUserId, h.EffDt, d.Id AS FishTransport_Detail_Id, d.ReleaseSite, d.TotalFishRepresented, d.ReleaseSiteComments, d.TransportTankUnit, 
                         d.TransportReleaseTemperature, d.TransportReleaseTemperatureF, d.TransportMortality, d.RowId, d.RowStatusId, d.ByUserId AS FishTransport_Detail_ByUserId, 
                         d.QAStatusId, d.EffDt AS FishTransport_Detail_EffDt, 
						 aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments,
						 a.DatasetId, a.SourceId, 
                         a.LocationId, a.UserId AS Activity_UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, a.Id AS ActivityId, aq.QAStatusName as QAStatusName
						 
FROM            dbo.Activities AS a INNER JOIN
                         dbo.FishTransport_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.FishTransport_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id;
GO
/****** Object:  View [dbo].[StreamNet_NOSA_detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[StreamNet_NOSA_detail_VW]
AS
SELECT        Id, CommonName, Run, PopFit, WaterBody, SpawningYear, TRTmethod, ContactAgency, MethodNumber, NOSAIJ, NOSAEJ, Comment, NullRecord, DataStatus, 
                         ContactPersonFirst, ContactPersonLast, ContactPhone, ContactEmail, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, ShadowId, Age10Prop, 
                         Age10PropLowerLimit, Age10PropUpperLimit, Age11PlusProp, Age11PlusPropLowerLimit, Age11PlusPropUpperLimit, Age2Prop, Age2PropLowerLimit, 
                         Age2PropUpperLimit, Age3Prop, Age3PropLowerLimit, Age3PropUpperLimit, Age4Prop, Age4PropLowerLimit, Age4PropUpperLimit, Age5Prop, Age5PropLowerLimit, 
                         Age5PropUpperLimit, Age6Prop, Age6PropLowerLimit, Age6PropUpperLimit, Age7Prop, Age7PropLowerLimit, Age7PropUpperLimit, Age8Prop, Age8PropLowerLimit, 
                         Age8PropUpperLimit, Age9Prop, Age9PropLowerLimit, Age9PropUpperLimit, AgePropAlpha, CBFWApopName, Comments, CommonPopName, CompilerRecordID, 
                         DataEntry, DataEntryNotes, ESU_DPS, HOSJF, IndicatorLocation, LastUpdated, MajorPopGroup, MeasureLocation, MetaComments, MethodAdjustments, 
                         MetricLocation, NOBroodStockRemoved, NOSAEJAlpha, NOSAEJLowerLimit, NOSAEJUpperLimit, NOSAIJAlpha, NOSAIJLowerLimit, NOSAIJUpperLimit, NOSJF, 
                         NOSJFAlpha, NOSJFLowerLimit, NOSJFUpperLimit, PopFitNotes, PopID, ProtMethDocumentation, ProtMethName, ProtMethURL, Publish, RecoveryDomain, RefID, 
                         SubmitAgency, TSAEJ, TSAEJAlpha, TSAEJLowerLimit, TSAEJUpperLimit, TSAIJ, TSAIJAlpha, TSAIJLowerLimit, TSAIJUpperLimit, UpdDate, pHOSej, pHOSejAlpha, 
                         pHOSejLowerLimit, pHOSejUpperLimit, pHOSij, pHOSijAlpha, pHOSijLowerLimit, pHOSijUpperLimit
FROM            dbo.StreamNet_NOSA_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.StreamNet_NOSA_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[StreamNet_NOSA_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[StreamNet_NOSA_Header_VW]
    AS
    SELECT        *
    FROM            dbo.StreamNet_NOSA_Header AS h
    WHERE        (EffDt =
                                 (SELECT        MAX(EffDt) AS MaxEffDt
                                   FROM            dbo.StreamNet_NOSA_Header AS hh
                                   WHERE        (ActivityId = h.ActivityId)));

GO
/****** Object:  View [dbo].[StreamNet_NOSA_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[StreamNet_NOSA_vw] as
    select
    d.Id, d.CommonName, d.Run, d.PopFit, d.WaterBody, d.SpawningYear, d.TRTmethod, d.ContactAgency, d.MethodNumber, d.NOSAIJ, d.NOSAEJ, 
    d.Comment, d.NullRecord, d.DataStatus, d.ContactPersonFirst, d.ContactPersonLast, d.ContactPhone, d.ContactEmail, d.RowId, d.RowStatusId, 
    d.ByUserId, d.QAStatusId, d.EffDt, d.ShadowId, d.Age10Prop, d.Age10PropLowerLimit, d.Age10PropUpperLimit, d.Age11PlusProp, 
    d.Age11PlusPropLowerLimit, d.Age11PlusPropUpperLimit, d.Age2Prop, d.Age2PropLowerLimit, d.Age2PropUpperLimit, d.Age3Prop, d.Age3PropLowerLimit, 
    d.Age3PropUpperLimit, d.Age4Prop, d.Age4PropLowerLimit, d.Age4PropUpperLimit, d.Age5Prop, d.Age5PropLowerLimit, d.Age5PropUpperLimit, 
    d.Age6Prop, d.Age6PropLowerLimit, d.Age6PropUpperLimit, d.Age7Prop, d.Age7PropLowerLimit, d.Age7PropUpperLimit, d.Age8Prop, d.Age8PropLowerLimit, 
    d.Age8PropUpperLimit, d.Age9Prop, d.Age9PropLowerLimit, d.Age9PropUpperLimit, d.AgePropAlpha, d.CBFWApopName, d.Comments, d.CommonPopName, 
    d.CompilerRecordID, d.DataEntry, d.DataEntryNotes, d.ESU_DPS, d.HOSJF, d.IndicatorLocation, d.LastUpdated, d.MajorPopGroup, d.MeasureLocation, 
    d.MetaComments, d.MethodAdjustments, d.MetricLocation, d.NOBroodStockRemoved, d.NOSAEJAlpha, d.NOSAEJLowerLimit, d.NOSAEJUpperLimit, d.NOSAIJAlpha, 
    d.NOSAIJLowerLimit, d.NOSAIJUpperLimit, d.NOSJF, d.NOSJFAlpha, d.NOSJFLowerLimit, d.NOSJFUpperLimit, d.PopFitNotes, d.PopID, d.ProtMethDocumentation, 
    d.ProtMethName, d.ProtMethURL, d.Publish, d.RecoveryDomain, d.RefID, d.SubmitAgency, d.TSAEJ, d.TSAEJAlpha, d.TSAEJLowerLimit, d.TSAEJUpperLimit, 
    d.TSAIJ, d.TSAIJAlpha, d.TSAIJLowerLimit, d.TSAIJUpperLimit, d.UpdDate, d.pHOSej, d.pHOSejAlpha, d.pHOSejLowerLimit, d.pHOSejUpperLimit, d.pHOSij, 
    d.pHOSijAlpha, d.pHOSijLowerLimit, d.pHOSijUpperLimit,

    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate

from StreamNet_NOSA_detail_VW d 
join StreamNet_NOSA_Header_VW h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId

GO
/****** Object:  View [dbo].[StreamNet_SAR_detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[StreamNet_SAR_detail_VW]
AS
SELECT        Id, CommonName, Run, PopFit, PopFitNotes, PopAggregation, SmoltLocation, AdultLocation, SARtype, OutmigrationYear, TRTmethod, ContactAgency, MethodNumber,
                          SAR, RearingType, Comments, NullRecord, DataStatus, ContactPersonFirst, ContactPersonLast, ContactPhone, ContactEmail, RowId, RowStatusId, ActivityId, 
                         ByUserId, QAStatusId, EffDt, ShadowId, BroodStockRemoved, CBFWApopName, CommonPopName, CompilerRecordID, DataEntry, DataEntryNotes, ESU_DPS, 
                         HarvestAdj, IndicatorLocation, LastUpdated, MainstemHarvest, MajorPopGroup, MeasureLocation, MetaComments, MethodAdjustments, MetricLocation, 
                         OceanHarvest, PopID, ProtMethDocumentation, ProtMethName, ProtMethURL, Publish, RecoveryDomain, RefID, ReturnDef, ReturnsMissing, 
                         ReturnsMissingExplanation, SARAlpha, SARLowerLimit, SARUpperLimit, ScopeOfInference, SmoltLocPTcode, SubmitAgency, TAR, TARAlpha, TARLowerLimit, 
                         TARUpperLimit, TSO, TSOAlpha, TSOLowerLimit, TSOUpperLimit, TribHarvest, UpdDate
FROM            dbo.StreamNet_SAR_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.StreamNet_SAR_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[StreamNet_SAR_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[StreamNet_SAR_Header_VW]
AS
SELECT        *
FROM            dbo.StreamNet_SAR_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.StreamNet_SAR_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)));

GO
/****** Object:  View [dbo].[StreamNet_SAR_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[StreamNet_SAR_vw] as
    select
    d.Id, d.CommonName, d.Run, d.PopFit, d.PopFitNotes, d.PopAggregation, d.SmoltLocation, d.AdultLocation, d.SARtype, d.OutmigrationYear, 
    d.TRTmethod, d.ContactAgency, d.MethodNumber, d.SAR, d.RearingType, d.Comments, d.NullRecord, d.DataStatus, d.ContactPersonFirst, 
    d.ContactPersonLast, d.ContactPhone, d.ContactEmail, d.RowId, d.RowStatusId, d.ByUserId, d.QAStatusId, d.EffDt, 
    d.ShadowId, d.BroodStockRemoved, d.CBFWApopName, d.CommonPopName, d.CompilerRecordID, d.DataEntry, d.DataEntryNotes, d.ESU_DPS, 
    d.HarvestAdj, d.IndicatorLocation, d.LastUpdated, d.MainstemHarvest, d.MajorPopGroup, d.MeasureLocation, d.MetaComments, d.MethodAdjustments, 
    d.MetricLocation, d.OceanHarvest, d.PopID, d.ProtMethDocumentation, d.ProtMethName, d.ProtMethURL, d.Publish, d.RecoveryDomain, d.RefID, 
    d.ReturnDef, d.ReturnsMissing, d.ReturnsMissingExplanation, d.SARAlpha, d.SARLowerLimit, d.SARUpperLimit, d.ScopeOfInference, d.SmoltLocPTcode, 
    d.SubmitAgency, d.TAR, d.TARAlpha, d.TARLowerLimit, d.TARUpperLimit, d.TSO, d.TSOAlpha, d.TSOLowerLimit, d.TSOUpperLimit, d.TribHarvest, 
    d.UpdDate,

    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate

from StreamNet_SAR_detail_VW d 
join StreamNet_SAR_Header_VW h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId

GO
/****** Object:  View [dbo].[WTLoc_ActiveInactive_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WTLoc_ActiveInactive_vw]
AS
SELECT        TOP (100) PERCENT Cast(LEFT(a.Description, 10) as datetime)AS MinMaxDate, L.OtherAgencyId AS AgencyId, L.Label,SUBSTRING(L.OtherAgencyId, 7, 4) AS SiteNumber
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Locations AS L ON a.LocationId = L.Id
WHERE        (L.LocationTypeId = 6)
UNION ALL
SELECT        TOP (100) PERCENT CAST(RIGHT(a.Description, 10)as datetime) AS MinMaxDate, L.OtherAgencyId AS AgencyId, L.Label,SUBSTRING(L.OtherAgencyId, 7, 4) AS SiteNumber

FROM            dbo.Activities AS a INNER JOIN
                         dbo.Locations AS L ON a.LocationId = L.Id
WHERE        (L.LocationTypeId = 6)
ORDER BY AgencyId

GO
/****** Object:  View [dbo].[WT_ActiveInactive2_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WT_ActiveInactive2_VW]
AS
SELECT DISTINCT 
                         TOP (100) PERCENT CAST(ai.SiteNumber AS INT) AS Site, ai.AgencyId, ai.Label, CONVERT(varchar(10), MIN(ai.MinMaxDate), 101) AS StartDate, CONVERT(varchar(10),
                          MAX(ai.MinMaxDate), 101) AS EndDate, l.GPSEasting, l.GPSNorthing, CASE WHEN MAX(MinMaxDate) 
                         LIKE '%2016%' THEN 'Active' ELSE 'Inactive' END AS ActiveInactive
FROM            dbo.WTLoc_ActiveInactive_vw AS ai INNER JOIN
                         dbo.Locations AS l ON ai.AgencyId = l.OtherAgencyId
GROUP BY ai.AgencyId, ai.Label, l.GPSEasting, l.GPSNorthing, CAST(ai.SiteNumber AS INT)
ORDER BY Site

GO
/****** Object:  View [dbo].[FishScales_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FishScales_vw]
AS
SELECT        h.RunYear, h.Technician, d.FieldScaleID, d.GumCardScaleID, d.ScaleCollectionDate, d.Species, d.Circuli, d.FreshwaterAge, d.SaltWaterAge, d.TotalAdultAge, 
                         d.SpawnCheck, d.Regeneration, d.RowId, d.RowStatusId, d.ScaleComments, d.BadScale, d.TotalAge, d.QAStatusId, a.Id AS ActivityId, a.DatasetId, a.ActivityDate, 
                         a.CreateDate, aq.QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId, d.Stock, d.LifeStage, a.LocationId
FROM            dbo.FishScales_Detail_VW AS d LEFT OUTER JOIN
                         dbo.FishScales_Header_VW AS h ON d.ActivityId = h.ActivityId LEFT OUTER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id

GO
/****** Object:  View [dbo].[Screwtrap_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Screwtrap_Detail_VW]
AS
SELECT        Id, Sequence, PitTagCode, SpeciesRunRearing, ForkLength, Weight, OtherSpecies, FishCount, ConditionalComment, TextualComments, Note, ReleaseLocation, 
                         FishComments, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, TagStatus, ClipStatus, AdditionalPositionalComments, EventType, SecondPitTag, 
                         RacewayTransectTank, LifeStage, GeneticId, CodedWireTag, BroodYear, MigrationYear, SizeOfCount, ScaleId, Containment
FROM            dbo.ScrewTrap_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.ScrewTrap_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[Screwtrap_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Screwtrap_vw]
AS
SELECT        h.FileTitle, h.ClipFiles, h.Tagger, h.LivewellTemp, h.TaggingTemp, h.PostTaggingTemp, h.ReleaseTemp, h.ArrivalTime, h.DepartTime, h.ArrivalRPMs, 
                         h.DepartureRPMs, h.Hubometer, h.HubometerTime, h.TrapStopped, h.TrapStarted, h.FishCollected, h.FishReleased, h.Flow, h.Turbidity, h.TrapDebris, h.RiverDebris, 
                         h.ActivityComments, h.Unit, h.DailyFinClips, h.Crew, h.TrapStatus, h.Weather, d.Sequence, d.PitTagCode, d.ForkLength, d.Weight, d.OtherSpecies, d.FishCount, 
                         d.ConditionalComment, d.TextualComments, d.Note, d.ReleaseLocation, d.FishComments, d.RowId, d.RowStatusId, d.TagStatus, d.ClipStatus, 
                         d.AdditionalPositionalComments, d.EventType, d.SecondPitTag, d.RacewayTransectTank, d.LifeStage, d.GeneticId, d.CodedWireTag, d.BroodYear, d.MigrationYear, 
                         d.SizeOfCount, d.ScaleId, d.Containment, CASE WHEN SpeciesRunRearing IN ('00U', '25H', '25W', '3RH', '3RW', '7RW', '90U', 'A0W', 'D0W', 'ERU', 'G0W') 
                         THEN '' WHEN d .ForkLength < 100 THEN 'Small' WHEN d .ForkLength > 120 THEN 'Large' ELSE 'Medium' END AS SizeClass, 
                         CASE WHEN SpeciesRunRearing = '00U' THEN 'Unknown' WHEN SpeciesRunRearing = '11H' THEN 'Hat. Spring Chinook' WHEN SpeciesRunRearing = '11U' THEN 'Spring Chinook (unknown r/t)'
                          WHEN SpeciesRunRearing = '11W' THEN 'Wild Spring Chinook' WHEN SpeciesRunRearing = '13H' THEN 'Hat. Fall Chinook' WHEN SpeciesRunRearing = '13W' THEN
                          'Wild Fall Chinook' WHEN SpeciesRunRearing = '25H' THEN 'Hat. Coho' WHEN SpeciesRunRearing = '25W' THEN 'Wild Coho' WHEN SpeciesRunRearing = '32H' THEN
                          'Hat. Summer Steelhead' WHEN SpeciesRunRearing = '32W' THEN 'Wild Summer Steelhead' WHEN SpeciesRunRearing = '3RH' THEN 'Hat. Rainbow Trout' WHEN SpeciesRunRearing
                          = '3RW' THEN 'Wild Rainbow Trout' WHEN SpeciesRunRearing = '7RW' THEN 'Bull Trout' WHEN SpeciesRunRearing = '90U' THEN 'Other' WHEN SpeciesRunRearing
                          = 'A0W' THEN 'Lamprey' WHEN SpeciesRunRearing = 'D0W' THEN 'Northern Pikeminnow' WHEN SpeciesRunRearing = 'ERU' THEN 'Brook Trout' WHEN SpeciesRunRearing
                          = 'G0W' THEN 'Mountain Whitefish' ELSE 'Other' END AS SpeciesRunRearing, CASE WHEN SpeciesRunRearing IN ('11H', '13H', '25H', '32H', '3RH') 
                         THEN 'HAT' WHEN OtherSpecies IN ('HATCHERY CHS', 'HATCHERY STS') THEN 'HAT' WHEN SpeciesRunRearing IN ('11U', '11W', '13W', '25W', '32W', '3RW') 
                         THEN 'NAT' WHEN OtherSpecies IN ('CHINOOK', 'STEELHEAD') THEN 'NAT' ELSE 'UNK' END AS HatNat, a.Id AS ActivityId, a.DatasetId, a.InstrumentId, 
                         a.LaboratoryId, a.ActivityDate, a.CreateDate, w.Id AS WaterbodyId, w.Name AS WaterbodyName, l.Id AS LocationId, aq.QAStatusName, 
                         aq.Comments AS ActivityQAComments, aq.QAStatusId, aq.QAStatusId AS ActivityQAStatusId, l.Label AS LocationLabel, d.SpeciesRunRearing AS SRRCode
FROM            dbo.Screwtrap_Detail_VW AS d INNER JOIN
                         dbo.Screwtrap_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.Locations AS l ON l.Id = a.LocationId LEFT OUTER JOIN
                         dbo.WaterBodies AS w ON w.Id = l.WaterBodyId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id

GO
/****** Object:  View [dbo].[FishScalesReport_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FishScalesReport_vw]
AS
SELECT        h.RunYear, h.Technician, d.FieldScaleID, d.GumCardScaleID, d.ScaleCollectionDate, d.Species, d.Circuli, d.FreshwaterAge, d.SaltWaterAge, d.TotalAdultAge, 
                         d.SpawnCheck, d.Regeneration, COALESCE (d.Stock, aw.Stock) AS Stock, d.ScaleComments, d.BadScale, d.TotalAge, ef.Unit, COALESCE (st.HatNat, sgs.Origin, 
                         aw.Origin) AS Origin, COALESCE (st.ForkLength, ef.ForkLength, sgs.ForkLength, aw.ForkLength) AS ForkLength, aw.LifeStage, COALESCE (st.Weight, ef.Weight, 
                         aw.Weight) AS Weight, COALESCE (st.AdditionalPositionalComments, ef.AdditionalPositionalComment, sgs.FinClips, aw.FinClip) AS FinClips, COALESCE (sgs.Marks, 
                         aw.Mark) AS Marks, sgs.MeHPLength, COALESCE (sgs.SpawningStatus, aw.PercentSpawned) AS PercentRetained, COALESCE (sgs.Sex, aw.Sex) AS Sex, a.DatasetId, 
                         COALESCE (aw.LocationLabel, st.LocationLabel, ef.LocationLabel, sgs.LocationLabel) AS LocationLabel, COALESCE (aw.LocationId, st.LocationId, ef.LocationId, 
                         sgs.LocationId) AS LocationId, aw.FishNumber, aw.BroodProgram, aw.HatcheryType, aw.Recapture, aw.ReleaseSite, aw.Ripeness, aw.Tag, aw.OtolithGenetics, 
                         aw.OtherTagId, aw.Disposition, aw.RadioTagId, aw.GeneticSampleId, aw.PITTagId, COALESCE (sgs.SnoutID, aw.SnoutId) AS SnoutId, COALESCE (sgs.Comments, 
                         aw.FishComments, st.FishComments) AS FishComments
FROM            dbo.FishScales_Detail_VW AS d LEFT OUTER JOIN
                         dbo.FishScales_Header_VW AS h ON d.ActivityId = h.ActivityId LEFT OUTER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.Screwtrap_vw AS st ON st.TextualComments = d.FieldScaleID LEFT OUTER JOIN
                         dbo.Electrofishing_vw AS ef ON ef.TextualComments = d.FieldScaleID LEFT OUTER JOIN
                         dbo.SpawningGroundSurvey_vw AS sgs ON sgs.ScaleID = d.FieldScaleID LEFT OUTER JOIN
                         dbo.AdultWeir_vw AS aw ON aw.ScaleId = d.FieldScaleID LEFT OUTER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id

GO
/****** Object:  View [dbo].[WaterTemp_VW2]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterTemp_VW2]
AS
SELECT        a.Id, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, a.InstrumentId, h.Id AS Expr1, h.Technicians, h.Comments, 
                         h.CollectionType, h.DepthToWater, h.PSI, h.StaticWaterLevel, h.WeatherConditions, h.FieldActivityType, h.SamplePeriod, h.SampleTempUnit, h.DeployTime, 
                         h.ByUserId, h.EffDt, d.Conductivity, d.ReadingDateTime, d.GMTReadingDateTime, d.AirTemperature, d.AirTemperatureF, d.WaterTemperature, d.WaterTemperatureF, 
                         d.WaterLevel, d.TempAToD, d.BatteryVolts, d.RowId, d.RowStatusId, d.ByUserId AS Expr2, d.QAStatusId, d.EffDt AS Expr3, aq.QAStatusId AS Expr4, 
                         aq.UserId AS Expr5, aq.Comments AS Expr6, aq.QAStatusName, dbo.WaterBodies.Name
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations ON a.LocationId = dbo.Locations.Id INNER JOIN
                         dbo.WaterBodies ON dbo.Locations.WaterBodyId = dbo.WaterBodies.Id

GO
/****** Object:  View [dbo].[WQWTWeb_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WQWTWeb_vw]
AS
SELECT        dbo.WTWeb_vw.*
FROM            dbo.WTWeb_vw
UNION ALL
SELECT        dbo.WQWeb_vw.*
FROM            dbo.WQWeb_vw

GO
/****** Object:  View [dbo].[WTCDMSvsOld_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WTCDMSvsOld_vw]
AS
SELECT        TOP (100) PERCENT l.OtherAgencyId, LEFT(a.Description, 11) AS StartDate, RIGHT(a.Description, 11) AS EndDate, h.CollectionType, u.Username, qa.QAStatusName, 
                         d.Name
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id INNER JOIN
                         dbo.Users AS u ON a.UserId = u.Id INNER JOIN
                         dbo.Datasets AS d ON a.DatasetId = d.Id LEFT OUTER JOIN
                         dbo.ActivityQAs_VW AS qa ON a.Id = qa.ActivityId RIGHT OUTER JOIN
                         dbo.WaterTemp_Header_VW AS h ON a.Id = h.ActivityId
WHERE        (l.LocationTypeId = 6) AND (h.FieldActivityType = 'Data File Upload')

GO
/****** Object:  View [dbo].[ScrewTrapPL_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ScrewTrapPL_vw]
AS
SELECT        d.ForkLength, d.Weight, d.OtherSpecies, d.FishCount, d.ConditionalComment, d.TextualComments, d.Note, d.AdditionalPositionalComments, a.DatasetId, 
                         a.ActivityDate, w.Id AS WaterbodyId, w.Name AS WaterbodyName, l.Id AS LocationId, aq.QAStatusName, aq.QAStatusId, l.Label AS LocationLabel
FROM            dbo.Screwtrap_Detail_VW AS d INNER JOIN
                         dbo.Screwtrap_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.Locations AS l ON l.Id = a.LocationId LEFT OUTER JOIN
                         dbo.WaterBodies AS w ON w.Id = l.WaterBodyId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
WHERE        (a.DatasetId = 1217)

GO
/****** Object:  View [dbo].[ElectrofishingPL_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ElectrofishingPL_vw]
AS
SELECT        h.Id, h.EventType, h.SiteArea, h.HabitatType, h.Visibility, h.ActivityComments, h.ReleaseSite, h.ReleaseRiverKM, h.Pass1TotalSecondsEF, h.Pass2TotalSecondsEF, 
                         h.Pass3TotalSecondsEF, h.Pass4TotalSecondsEF, d.SpeciesRunRearing, d.ForkLength, d.Weight, d.OtherSpecies, d.FishCount, d.ConditionalComment, 
                         d.TextualComments, d.Note, d.AdditionalPositionalComment, d.TotalLength, a.Id AS ActivityId, a.DatasetId, a.ActivityDate, l.Id AS LocationId, aq.QAStatusId, w.Name, 
                         l.Label AS LocationLabel
FROM            dbo.Electrofishing_Detail_VW AS d INNER JOIN
                         dbo.Electrofishing_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId INNER JOIN
                         dbo.Locations AS l ON l.Id = a.LocationId INNER JOIN
                         dbo.WaterBodies AS w ON w.Id = l.WaterBodyId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
WHERE        (a.DatasetId = 1221)

GO
/****** Object:  View [dbo].[ActivityQAs_VW2]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ActivityQAs_VW2]
AS
SELECT        Id, ActivityId, QAStatusId, UserId, EffDt, Comments, QAStatusName, QAStatusDescription
FROM            dbo.ActivityQAs AS a
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.ActivityQAs AS aa
                               WHERE        (ActivityId = a.ActivityId)))

GO
/****** Object:  View [dbo].[CrppContracts_Detail_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CrppContracts_Detail_VW]
AS
SELECT        Id, AcresSurveyed, Testing, NewSites, MonitoredSites, SitesEvaluated, UpdatedSites, NewIsolates, Evaluation, HprcsitsRecorded, Monitoring, Notes, 
                         ShpoReportNumber, ShpoCaseNumber, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt
FROM            dbo.CrppContracts_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.CrppContracts_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

GO
/****** Object:  View [dbo].[CrppContracts_Header_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CrppContracts_Header_VW]
AS
SELECT        Id, CrppPersonnel, ActivityTypeId, Agency, ProjectProponent, PermitNumber, DateReceived, DateOfAction, ActionTaken, ActivityNotes, AttachedDocument, ActivityId, 
                         ByUserId, EffDt
FROM            dbo.CrppContracts_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.CrppContracts_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))

GO
/****** Object:  View [dbo].[CrppContracts_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CrppContracts_VW]
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, h.Id, h.CrppPersonnel, h.Agency, h.ProjectProponent, 
                         h.PermitNumber, h.DateReceived, h.DateOfAction, h.ActionTaken, h.ActivityNotes, h.AttachedDocument, h.ByUserId, h.EffDt, d.Id AS CrppContracts_Detail_Id, 
                         d.AcresSurveyed, d.Testing, d.NewSites, d.MonitoredSites, d.SitesEvaluated, d.UpdatedSites, d.NewIsolates, d.Evaluation, d.HprcsitsRecorded, d.Monitoring, 
                         d.Notes, d.ShpoReportNumber, d.ShpoCaseNumber, d.RowId, d.RowStatusId, d.ByUserId AS CrppContracts_Detail_ByUserId, d.QAStatusId, 
                         d.EffDt AS CrppContracts_Detail_EffDt, aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments, aq.QAStatusName
FROM            dbo.Activities AS a INNER JOIN
                         dbo.CrppContracts_Header AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.CrppContracts_Detail AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs AS aq ON a.Id = aq.ActivityId

GO
/****** Object:  View [dbo].[CRPPCorrespondence_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRPPCorrespondence_vw]
AS
SELECT        sp.ProjectLead, ce.StaffMember, CASE WHEN sp.UIR = 'YES' THEN 1 ELSE 0 END AS YesUIR, CASE WHEN sp.UIR = 'No' THEN 1 ELSE 0 END AS NoUIR, 
                         ce.CorrespondenceType, CASE WHEN ce.CorrespondenceDate IS NULL THEN 0 ELSE 1 END AS Recieved, ce.ResponseType, ce.NumberOfDays, ce.ResponseDate, 
                         CASE WHEN ce.ResponseType IS NULL THEN 0 ELSE 1 END AS Completed, YEAR(ce.CorrespondenceDate) AS Year, { fn QUARTER(ce.CorrespondenceDate) 
                         } AS QuarterYear, CASE WHEN { fn QUARTER(ce.CorrespondenceDate) } IN (1, 2) THEN 1 ELSE 2 END AS HalfYear, ce.SubprojectId, sp.ProjectName, ce.Id AS CEId, 
                         ce.CorrespondenceDate, CASE WHEN sp.OffResTribalFee = 'Yes' THEN 1 ELSE 0 END AS OffResFee, CASE WHEN Month(ce.CorrespondenceDate) 
                         > 9 THEN Year(ce.CorrespondenceDate) + 1 ELSE Year(ce.CorrespondenceDate) END AS FiscalYear
FROM            dbo.CorrespondenceEvents AS ce INNER JOIN
                         dbo.Subproject_Crpp AS sp ON ce.SubprojectId = sp.Id

GO
/****** Object:  View [dbo].[DNRProjects_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[DNRProjects_vw]
AS
SELECT        dbo.Locations.GPSEasting, dbo.Locations.GPSNorthing, dbo.Locations.SdeObjectId, dbo.Locations.Id, dbo.Locations.Label, dbo.LocationProjects.Location_Id, 
                         dbo.LocationProjects.Project_Id, dbo.Locations.LocationTypeId, dbo.Projects.Name, dbo.Projects.StartDate, dbo.Projects.EndDate, dbo.Projects.OrganizationId, 
                         dbo.Organizations.Name AS Organization, dbo.Departments.Name AS Department
FROM            dbo.LocationProjects INNER JOIN
                         dbo.Locations ON dbo.LocationProjects.Location_Id = dbo.Locations.Id INNER JOIN
                         dbo.Projects ON dbo.LocationProjects.Project_Id = dbo.Projects.Id INNER JOIN
                         dbo.Organizations ON dbo.Projects.OrganizationId = dbo.Organizations.Id INNER JOIN
                         dbo.Departments ON dbo.Organizations.Id = dbo.Departments.OrganizationId
WHERE        (dbo.Locations.LocationTypeId = 3) AND (dbo.Projects.OrganizationId = 1) AND (dbo.Departments.Name = N'DNR')

GO
/****** Object:  View [dbo].[gcCleanLocProj]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*Exclude Appraisals*/
CREATE VIEW [dbo].[gcCleanLocProj]
AS
SELECT        dbo.LocationProjects.Location_Id, dbo.Locations.LocationTypeId, dbo.Locations.SdeFeatureClassId, dbo.Locations.SdeObjectId, dbo.Locations.Label, 
                         dbo.LocationProjects.Project_Id
FROM            dbo.LocationProjects INNER JOIN
                         dbo.Locations ON dbo.LocationProjects.Location_Id = dbo.Locations.Id
WHERE        (dbo.Locations.LocationTypeId <> 8)

GO
/****** Object:  View [dbo].[LocationsToDatasets_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[LocationsToDatasets_vw]
AS
SELECT        dbo.LocationProjects.Location_Id, dbo.Locations.LocationTypeId, dbo.Locations.Label, dbo.Locations.OtherAgencyId, dbo.Locations.WaterBodyId, 
                         dbo.LocationProjects.Project_Id, dbo.Projects.Name, dbo.Locations.Description, dbo.Locations.GPSEasting, dbo.Locations.GPSNorthing, 
                         dbo.Locations.RiverMile
FROM            dbo.Locations INNER JOIN
                         dbo.LocationProjects ON dbo.Locations.Id = dbo.LocationProjects.Location_Id INNER JOIN
                         dbo.Projects ON dbo.LocationProjects.Project_Id = dbo.Projects.Id

GO
/****** Object:  View [dbo].[ProjectInstruments_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ProjectInstruments_vw]
AS
SELECT        dbo.Projects.Name AS Project, dbo.Instruments.Name AS Instrument, dbo.Instruments.SerialNumber, dbo.Instruments.Model, dbo.Instruments.Id, 
                         dbo.ProjectInstruments.Project_Id, dbo.InstrumentAccuracyChecks.CheckDate, dbo.InstrumentAccuracyChecks.CheckMethod, 
                         dbo.InstrumentAccuracyChecks.Bath1Grade, dbo.InstrumentAccuracyChecks.Bath2Grade
FROM            dbo.Instruments INNER JOIN
                         dbo.ProjectInstruments ON dbo.Instruments.Id = dbo.ProjectInstruments.Instrument_Id INNER JOIN
                         dbo.Projects ON dbo.ProjectInstruments.Project_Id = dbo.Projects.Id LEFT OUTER JOIN
                         dbo.InstrumentAccuracyChecks ON dbo.Instruments.Id = dbo.InstrumentAccuracyChecks.InstrumentId

GO
/****** Object:  View [dbo].[ProjectOwners_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ProjectOwners_vw]
AS
SELECT        dbo.Projects.Id, dbo.Projects.Name, dbo.Projects.Description, dbo.Projects.StartDate, dbo.Projects.EndDate, dbo.Users.Fullname
FROM            dbo.Projects INNER JOIN
                         dbo.Users ON dbo.Projects.OwnerId = dbo.Users.Id

GO
/****** Object:  View [dbo].[ScrewTrap_HeaderUM_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ScrewTrap_HeaderUM_VW]
AS
SELECT        h.EffDt, dbo.Activities.ActivityDate, h.FileTitle, h.Id AS HeaderID, h.ArrivalRPMs, h.DepartureRPMs, h.Hubometer, h.HubometerTime, h.TrapStopped, h.TrapStarted, 
                         h.FishCollected, h.FishReleased, h.Flow, h.Turbidity, h.TrapDebris, h.RiverDebris, h.ActivityComments AS ActivityComment, h.DailyFinClips, h.Crew, h.TrapStatus, 
                         dbo.Locations.Label, h.ClipFiles, h.Tagger, h.LivewellTemp, h.TaggingTemp, h.PostTaggingTemp, h.ReleaseTemp, h.ArrivalTime, h.DepartTime, 
                         CASE WHEN Month(ActivityDate) >= 8 THEN Year(ActivityDate) + 1 WHEN Month(ActivityDate) <= 7 THEN Year(ActivityDate) ELSE ' ' END AS MigrationYear, 
                         YEAR(dbo.Activities.ActivityDate) AS Year, MONTH(dbo.Activities.ActivityDate) AS Month, dbo.Activities.DatasetId
FROM            dbo.ScrewTrap_Header AS h INNER JOIN
                         dbo.Activities ON h.ActivityId = dbo.Activities.Id INNER JOIN
                         dbo.Locations ON dbo.Activities.LocationId = dbo.Locations.Id
WHERE        (h.EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.ScrewTrap_Header AS hh
                               WHERE        (ActivityId = h.ActivityId))) AND (dbo.Activities.DatasetId = 1215)

GO
/****** Object:  View [dbo].[ScrewTrap_HeaderWW_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ScrewTrap_HeaderWW_VW]
AS
SELECT        h.Id AS HeaderID, h.ArrivalRPMs, h.DepartureRPMs, h.Hubometer, h.HubometerTime, h.TrapStopped, h.TrapStarted, h.FishCollected, h.FishReleased, h.Flow, 
                         h.Turbidity, h.TrapDebris, h.RiverDebris, h.ActivityComments AS ActivityComment, h.EffDt, h.DailyFinClips, h.Crew, h.TrapStatus, dbo.Locations.Label, 
                         dbo.Activities.ActivityDate, h.FileTitle, h.ClipFiles, h.Tagger, h.LivewellTemp, h.TaggingTemp, h.PostTaggingTemp, h.ReleaseTemp, h.ArrivalTime, h.DepartTime, 
                         CASE WHEN Month(ActivityDate) >= 8 THEN Year(ActivityDate) + 1 WHEN Month(ActivityDate) <= 7 THEN Year(ActivityDate) ELSE ' ' END AS TrappingSeason, 
                         YEAR(dbo.Activities.ActivityDate) AS Year, MONTH(dbo.Activities.ActivityDate) AS Month, dbo.Activities.DatasetId
FROM            dbo.ScrewTrap_Header AS h INNER JOIN
                         dbo.Activities ON h.ActivityId = dbo.Activities.Id INNER JOIN
                         dbo.Locations ON dbo.Activities.LocationId = dbo.Locations.Id
WHERE        (h.EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.ScrewTrap_Header AS hh
                               WHERE        (ActivityId = h.ActivityId))) AND (dbo.Activities.DatasetId = 1216)

GO
/****** Object:  View [dbo].[WaterTempLocations_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WaterTempLocations_vw]
AS
SELECT DISTINCT 
                         TOP (100) PERCENT loc.OtherAgencyId AS SiteId, loc.Label AS SiteName, dbo.WaterBodies.Name AS Waterbody, loc.LocationTypeId, loc.GPSEasting, 
                         loc.GPSNorthing, dbo.Projects.Name AS ProjectName, dbo.LocationProjects.Project_Id AS ProjectId, loc.SdeObjectId, 
                         dbo.LocationProjects.Location_Id AS LocationId
FROM            dbo.Locations AS loc INNER JOIN
                         dbo.WaterBodies ON loc.WaterBodyId = dbo.WaterBodies.Id INNER JOIN
                         dbo.LocationProjects ON loc.Id = dbo.LocationProjects.Location_Id INNER JOIN
                         dbo.Projects ON dbo.LocationProjects.Project_Id = dbo.Projects.Id
WHERE        (loc.LocationTypeId = 6)

GO
/****** Object:  View [dbo].[WTStreams_vw]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WTStreams_vw]
AS
SELECT DISTINCT dbo.Locations.LocationTypeId, dbo.WaterBodies.Name AS Stream, dbo.Locations.Id AS LocationId, dbo.Locations.Label AS LoggerSite
FROM            dbo.Locations INNER JOIN
                         dbo.WaterBodies ON dbo.Locations.WaterBodyId = dbo.WaterBodies.Id
WHERE        (dbo.Locations.LocationTypeId = 6)

GO
/****** Object:  View [dbo].[WTWebYearlyExportZip_VW]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WTWebYearlyExportZip_VW]
AS
SELECT        CASE WHEN Month(wt.ReadingDateTime) > 9 THEN Year(wt.ReadingDateTime) + 1 ELSE YEAR(wt.ReadingDateTime) END AS WaterYear, wt.WaterBody, 
                         wt.CollectionType, wt.LoggerSite, wt.AgencyId, wt.FieldActivityType, wt.ReadingDateTime, wt.WaterTempC, wt.AirTempC, wt.RowQA, wt.Comments, lc.GPSEasting, 
                         lc.GPSNorthing
FROM            dbo.CDMS_WTReportTable AS wt INNER JOIN
                         dbo.Locations AS lc ON wt.LocationId = lc.Id
WHERE        (CASE WHEN Month(wt.ReadingDateTime) > 9 THEN Year(wt.ReadingDateTime) + 1 ELSE YEAR(wt.ReadingDateTime) END = 2016)

GO
/****** Object:  View [dbo].[WTWQLocationProject]    Script Date: 7/10/2017 1:43:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WTWQLocationProject]
AS
SELECT        p.Name AS FundingProject, wb.Description AS WaterShed, wb.Name AS Stream, l.OtherAgencyId, l.Label AS SiteLabel, l.Name AS SiteName, 
                         l.Description AS SiteDescription, l.Elevation, l.GPSEasting, l.GPSNorthing, l.Projection, l.UTMZone, l.ImageLink, l.WettedWidth, l.WettedDepth, l.RiverMile
FROM            dbo.WaterBodies AS wb INNER JOIN
                         dbo.Locations AS l ON wb.Id = l.WaterBodyId INNER JOIN
                         dbo.LocationProjects AS lp ON l.Id = lp.Location_Id INNER JOIN
                         dbo.Projects AS p ON lp.Project_Id = p.Id
WHERE        (l.LocationTypeId = 6)

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 236
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ActivityQAs_VW2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ActivityQAs_VW2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 240
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 50
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 135' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'AdultWeir_Detail_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'0
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'AdultWeir_Detail_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'AdultWeir_Detail_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 13
               Left = 339
               Bottom = 331
               Right = 543
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 4
               Left = 564
               Bottom = 133
               Right = 734
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 15
               Left = 971
               Bottom = 332
               Right = 1173
            End
            DisplayFlags = 280
            TopColumn = 6
         End
         Begin Table = "aq"
            Begin Extent = 
               Top = 6
               Left = 31
               Bottom = 206
               Right = 229
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Locations"
            Begin Extent = 
               Top = 178
               Left = 620
               Bottom = 307
               Right = 804
            End
            DisplayFlags = 280
            TopColumn = 2
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 39
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Wid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ArtificialProduction_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'th = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ArtificialProduction_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ArtificialProduction_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[43] 4[22] 2[18] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 370
               Bottom = 355
               Right = 574
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 18
               Left = 605
               Bottom = 335
               Right = 789
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "w"
            Begin Extent = 
               Top = 43
               Left = 834
               Bottom = 296
               Right = 1004
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 16
               Left = 89
               Bottom = 312
               Right = 323
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 11
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CreelEstimate1_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CreelEstimate1_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CreelEstimate1_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[43] 4[16] 2[19] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ah"
            Begin Extent = 
               Top = 10
               Left = 392
               Bottom = 283
               Right = 562
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "sd"
            Begin Extent = 
               Top = 19
               Left = 186
               Bottom = 267
               Right = 312
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 18
               Left = 628
               Bottom = 217
               Right = 820
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ct"
            Begin Extent = 
               Top = 23
               Left = 11
               Bottom = 152
               Right = 181
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 66
         Width = 284
         Width = 1500
         Width = 2400
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CreelEstimate2_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1920
         Alias = 2415
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CreelEstimate2_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CreelEstimate2_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 7
               Left = 868
               Bottom = 320
               Right = 1071
            End
            DisplayFlags = 280
            TopColumn = 20
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 19
               Left = 606
               Bottom = 294
               Right = 840
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "a"
            Begin Extent = 
               Top = 28
               Left = 379
               Bottom = 286
               Right = 583
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 29
               Left = 178
               Bottom = 311
               Right = 362
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "w"
            Begin Extent = 
               Top = 28
               Left = 3
               Bottom = 208
               Right = 173
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 38
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CreelFishermanStats_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1980
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CreelFishermanStats_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CreelFishermanStats_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[51] 4[12] 2[18] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 11
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 121
               Left = 755
               Bottom = 343
               Right = 958
            End
            DisplayFlags = 280
            TopColumn = 29
         End
         Begin Table = "aq"
            Begin Extent = 
               Top = 9
               Left = 547
               Bottom = 138
               Right = 745
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 6
               Left = 280
               Bottom = 135
               Right = 464
            End
            DisplayFlags = 280
            TopColumn = 21
         End
         Begin Table = "w"
            Begin Extent = 
               Top = 175
               Left = 264
               Bottom = 304
               Right = 434
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 188
               Left = 38
               Bottom = 338
               Right = 272
            End
            DisplayFlags = 280
            TopColumn = 11
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 67
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CreelSurvey_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1110
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CreelSurvey_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CreelSurvey_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[13] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ce"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 260
               Right = 240
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "sp"
            Begin Extent = 
               Top = 6
               Left = 278
               Bottom = 250
               Right = 490
            End
            DisplayFlags = 280
            TopColumn = 8
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 18
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2670
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CRPPCorrespondence_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CRPPCorrespondence_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "LocationProjects"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 208
               Right = 239
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Locations"
            Begin Extent = 
               Top = 7
               Left = 290
               Bottom = 262
               Right = 536
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Projects"
            Begin Extent = 
               Top = 7
               Left = 553
               Bottom = 251
               Right = 809
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "Organizations"
            Begin Extent = 
               Top = 49
               Left = 902
               Bottom = 238
               Right = 1105
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Departments"
            Begin Extent = 
               Top = 258
               Left = 598
               Bottom = 419
               Right = 792
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 15
         Width = 284
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1590
         Width =' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'DNRProjects_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' 1200
         Width = 1200
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'DNRProjects_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'DNRProjects_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 272
               Right = 288
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 6
               Left = 326
               Bottom = 271
               Right = 536
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 574
               Bottom = 295
               Right = 778
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 6
               Left = 816
               Bottom = 296
               Right = 1000
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "w"
            Begin Extent = 
               Top = 6
               Left = 1038
               Bottom = 118
               Right = 1208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "aq"
            Begin Extent = 
               Top = 120
               Left = 1038
               Bottom = 306
               Right = 1236
            End
            DisplayFlags = 280
            TopColumn = 1
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 113
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ElectrofishingPL_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'= 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ElectrofishingPL_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ElectrofishingPL_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 26
               Left = 371
               Bottom = 289
               Right = 565
            End
            DisplayFlags = 280
            TopColumn = 10
         End
         Begin Table = "a"
            Begin Extent = 
               Top = 43
               Left = 67
               Bottom = 172
               Right = 271
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "sgs"
            Begin Extent = 
               Top = 30
               Left = 1026
               Bottom = 271
               Right = 1231
            End
            DisplayFlags = 280
            TopColumn = 61
         End
         Begin Table = "aw"
            Begin Extent = 
               Top = 8
               Left = 707
               Bottom = 296
               Right = 943
            End
            DisplayFlags = 280
            TopColumn = 28
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 41
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'FishScaleGRreport_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'FishScaleGRreport_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'FishScaleGRreport_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[49] 4[13] 2[17] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 232
               Right = 232
            End
            DisplayFlags = 280
            TopColumn = 13
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 6
               Left = 270
               Bottom = 197
               Right = 440
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "a"
            Begin Extent = 
               Top = 238
               Left = 26
               Bottom = 367
               Right = 230
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "aq"
            Begin Extent = 
               Top = 225
               Left = 271
               Bottom = 354
               Right = 469
            End
            DisplayFlags = 280
            TopColumn = 4
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 13
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 4215
         Alias = 2250
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'FishScales_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'        Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'FishScales_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'FishScales_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 232
            End
            DisplayFlags = 280
            TopColumn = 13
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 6
               Left = 270
               Bottom = 135
               Right = 440
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 478
               Bottom = 135
               Right = 682
            End
            DisplayFlags = 280
            TopColumn = 11
         End
         Begin Table = "st"
            Begin Extent = 
               Top = 6
               Left = 720
               Bottom = 135
               Right = 975
            End
            DisplayFlags = 280
            TopColumn = 34
         End
         Begin Table = "ef"
            Begin Extent = 
               Top = 6
               Left = 1013
               Bottom = 135
               Right = 1263
            End
            DisplayFlags = 280
            TopColumn = 119
         End
         Begin Table = "sgs"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 317
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 38
         End
         Begin Table = "aw"
            Begin Extent = 
               Top = 138
               Left = 281
               Bottom = 267
               Right = 517
            End
            DisplayFlags = 280
            TopColumn = 29
     ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'FishScalesReport_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'    End
         Begin Table = "aq"
            Begin Extent = 
               Top = 203
               Left = 905
               Bottom = 332
               Right = 1103
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'FishScalesReport_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'FishScalesReport_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "LocationProjects"
            Begin Extent = 
               Top = 11
               Left = 347
               Bottom = 106
               Right = 530
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Locations"
            Begin Extent = 
               Top = 10
               Left = 58
               Bottom = 234
               Right = 255
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'gcCleanLocProj'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'gcCleanLocProj'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[27] 2[14] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 21
               Left = 460
               Bottom = 150
               Right = 654
            End
            DisplayFlags = 280
            TopColumn = 18
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 35
               Left = 243
               Bottom = 164
               Right = 413
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "a"
            Begin Extent = 
               Top = 29
               Left = 7
               Bottom = 158
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "sgs"
            Begin Extent = 
               Top = 15
               Left = 1012
               Bottom = 288
               Right = 1217
            End
            DisplayFlags = 280
            TopColumn = 56
         End
         Begin Table = "aw"
            Begin Extent = 
               Top = 55
               Left = 743
               Bottom = 304
               Right = 979
            End
            DisplayFlags = 280
            TopColumn = 20
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 21
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'GRME_VSPTesting_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2550
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'GRME_VSPTesting_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'GRME_VSPTesting_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[56] 4[11] 2[12] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "mp"
            Begin Extent = 
               Top = 79
               Left = 953
               Bottom = 285
               Right = 1143
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MetadataValues_vw"
            Begin Extent = 
               Top = 32
               Left = 688
               Bottom = 239
               Right = 882
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "Users"
            Begin Extent = 
               Top = 229
               Left = 253
               Bottom = 358
               Right = 424
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "p"
            Begin Extent = 
               Top = 6
               Left = 468
               Bottom = 265
               Right = 651
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 18
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1875
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append =' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'HabitatSubprojects_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'HabitatSubprojects_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'HabitatSubprojects_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[24] 4[19] 2[37] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "sp"
            Begin Extent = 
               Top = 1
               Left = 449
               Bottom = 353
               Right = 744
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 9
               Left = 203
               Bottom = 180
               Right = 387
            End
            DisplayFlags = 280
            TopColumn = 8
         End
         Begin Table = "w"
            Begin Extent = 
               Top = 13
               Left = 10
               Bottom = 142
               Right = 180
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 34
               Left = 826
               Bottom = 146
               Right = 996
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "f"
            Begin Extent = 
               Top = 183
               Left = 826
               Bottom = 295
               Right = 996
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 24
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'HabitatWebMap_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'HabitatWebMap_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'HabitatWebMap_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[23] 4[9] 2[13] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Collaborators"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 165
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'HabitatWebmapCollaborators_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'HabitatWebmapCollaborators_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "f"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 177
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 2880
         Width = 1500
         Width = 7425
         Width = 7275
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'HabitatWebMapFunding_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'HabitatWebMapFunding_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[12] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Locations"
            Begin Extent = 
               Top = 9
               Left = 575
               Bottom = 266
               Right = 759
            End
            DisplayFlags = 280
            TopColumn = 11
         End
         Begin Table = "LocationProjects"
            Begin Extent = 
               Top = 16
               Left = 278
               Bottom = 171
               Right = 448
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Projects"
            Begin Extent = 
               Top = 32
               Left = 29
               Bottom = 240
               Right = 212
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 27
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1680
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2955
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LocationsToDatasets_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LocationsToDatasets_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LocationsToDatasets_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[14] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 239
               Right = 232
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'MetadataValues_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'MetadataValues_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[42] 4[11] 2[13] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Projects"
            Begin Extent = 
               Top = 28
               Left = 173
               Bottom = 286
               Right = 347
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MetadataProperties"
            Begin Extent = 
               Top = 30
               Left = 874
               Bottom = 233
               Right = 1053
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MetadataValues_vw"
            Begin Extent = 
               Top = 48
               Left = 506
               Bottom = 262
               Right = 700
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 14
         Width = 284
         Width = 3135
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Project_RV_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Project_RV_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[38] 4[24] 2[4] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Instruments"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 357
               Right = 279
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ProjectInstruments"
            Begin Extent = 
               Top = 9
               Left = 534
               Bottom = 297
               Right = 704
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Projects"
            Begin Extent = 
               Top = 63
               Left = 760
               Bottom = 313
               Right = 943
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "InstrumentAccuracyChecks"
            Begin Extent = 
               Top = 145
               Left = 308
               Bottom = 389
               Right = 482
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 17
         Width = 284
         Width = 4755
         Width = 1500
         Width = 1740
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
        ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ProjectInstruments_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ProjectInstruments_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ProjectInstruments_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[10] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Projects"
            Begin Extent = 
               Top = 16
               Left = 474
               Bottom = 284
               Right = 657
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Users"
            Begin Extent = 
               Top = 30
               Left = 718
               Bottom = 274
               Right = 889
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 2595
         Width = 2925
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ProjectOwners_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ProjectOwners_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[27] 4[19] 2[34] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -96
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 303
               Right = 293
            End
            DisplayFlags = 280
            TopColumn = 17
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 27
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 3870
         Width = 1980
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrap_DetailUM_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrap_DetailUM_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[15] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "h"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 311
               Right = 226
            End
            DisplayFlags = 280
            TopColumn = 17
         End
         Begin Table = "Activities"
            Begin Extent = 
               Top = 66
               Left = 404
               Bottom = 311
               Right = 608
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "Locations"
            Begin Extent = 
               Top = 74
               Left = 713
               Bottom = 282
               Right = 897
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 12
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrap_HeaderUM_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrap_HeaderUM_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "h"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 226
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Activities"
            Begin Extent = 
               Top = 6
               Left = 264
               Bottom = 135
               Right = 468
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Locations"
            Begin Extent = 
               Top = 6
               Left = 506
               Bottom = 135
               Right = 690
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrap_HeaderWW_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrap_HeaderWW_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[37] 4[13] 2[37] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 7
               Left = 12
               Bottom = 385
               Right = 267
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 7
               Left = 305
               Bottom = 384
               Right = 493
            End
            DisplayFlags = 280
            TopColumn = 13
         End
         Begin Table = "a"
            Begin Extent = 
               Top = 12
               Left = 543
               Bottom = 141
               Right = 747
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 8
               Left = 817
               Bottom = 171
               Right = 1001
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "w"
            Begin Extent = 
               Top = 14
               Left = 1074
               Bottom = 143
               Right = 1244
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "aq"
            Begin Extent = 
               Top = 190
               Left = 803
               Bottom = 319
               Right = 1001
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
  ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Screwtrap_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'       Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Screwtrap_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Screwtrap_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 293
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 6
               Left = 331
               Bottom = 135
               Right = 519
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 557
               Bottom = 135
               Right = 761
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 6
               Left = 799
               Bottom = 248
               Right = 983
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "w"
            Begin Extent = 
               Top = 6
               Left = 1021
               Bottom = 118
               Right = 1191
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "aq"
            Begin Extent = 
               Top = 120
               Left = 1021
               Bottom = 249
               Right = 1219
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 60
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrapPL_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1740
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrapPL_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrapPL_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[64] 4[7] 2[12] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 507
               Bottom = 135
               Right = 711
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 6
               Left = 281
               Bottom = 363
               Right = 469
            End
            DisplayFlags = 280
            TopColumn = 13
         End
         Begin Table = "aq"
            Begin Extent = 
               Top = 175
               Left = 774
               Bottom = 304
               Right = 972
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 9
               Left = 9
               Bottom = 343
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 6
               Left = 749
               Bottom = 135
               Right = 933
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "w"
            Begin Extent = 
               Top = 296
               Left = 892
               Bottom = 408
               Right = 1062
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 55
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 15' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrapUM_Export_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'00
         Width = 1500
         Width = 5610
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 765
         Width = 540
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1635
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrapUM_Export_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrapUM_Export_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[43] 4[8] 2[34] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 284
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 14
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 6
               Left = 331
               Bottom = 303
               Right = 519
            End
            DisplayFlags = 280
            TopColumn = 14
         End
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 557
               Bottom = 318
               Right = 761
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 6
               Left = 799
               Bottom = 135
               Right = 983
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "w"
            Begin Extent = 
               Top = 6
               Left = 1021
               Bottom = 143
               Right = 1191
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "aq"
            Begin Extent = 
               Top = 179
               Left = 884
               Bottom = 308
               Right = 1082
            End
            DisplayFlags = 280
            TopColumn = 4
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 56
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrapUM_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrapUM_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrapUM_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[42] 4[9] 2[22] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 323
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 6
               Left = 281
               Bottom = 294
               Right = 469
            End
            DisplayFlags = 280
            TopColumn = 18
         End
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 507
               Bottom = 328
               Right = 711
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 6
               Left = 749
               Bottom = 282
               Right = 933
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "w"
            Begin Extent = 
               Top = 6
               Left = 971
               Bottom = 162
               Right = 1141
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "aq"
            Begin Extent = 
               Top = 191
               Left = 1016
               Bottom = 320
               Right = 1214
            End
            DisplayFlags = 280
            TopColumn = 4
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 15' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrapWW_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'00
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1980
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrapWW_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ScrewTrapWW_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[21] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "h"
            Begin Extent = 
               Top = 6
               Left = 260
               Bottom = 268
               Right = 446
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 487
               Bottom = 313
               Right = 691
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 6
               Left = 729
               Bottom = 135
               Right = 913
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "w"
            Begin Extent = 
               Top = 2
               Left = 992
               Bottom = 131
               Right = 1162
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Datasets"
            Begin Extent = 
               Top = 162
               Left = 780
               Bottom = 321
               Right = 1006
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 277
               Right = 225
            End
            DisplayFlags = 280
            TopColumn = 25
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 72
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Wid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'SGSAllExport_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'th = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1545
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'SGSAllExport_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'SGSAllExport_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 4
               Left = 233
               Bottom = 279
               Right = 437
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 14
               Left = 490
               Bottom = 289
               Right = 699
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Locations"
            Begin Extent = 
               Top = 14
               Left = 0
               Bottom = 266
               Right = 184
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'SO_WTQueryReports_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'SO_WTQueryReports_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 266
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'StreamNet_NOSA_detail_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'StreamNet_NOSA_detail_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 284
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'StreamNet_RperS_detail_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'StreamNet_RperS_detail_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 269
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'StreamNet_SAR_detail_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'StreamNet_SAR_detail_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[21] 2[19] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 231
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 6
               Left = 269
               Bottom = 135
               Right = 455
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "a"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 267
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "loc"
            Begin Extent = 
               Top = 138
               Left = 280
               Bottom = 267
               Right = 464
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "aq"
            Begin Extent = 
               Top = 270
               Left = 38
               Bottom = 399
               Right = 236
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterQuality_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterQuality_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[48] 4[13] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 212
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 6
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 6
               Left = 280
               Bottom = 235
               Right = 471
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 509
               Bottom = 229
               Right = 718
            End
            DisplayFlags = 280
            TopColumn = 8
         End
         Begin Table = "aq"
            Begin Extent = 
               Top = 6
               Left = 756
               Bottom = 135
               Right = 954
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "WaterBodies"
            Begin Extent = 
               Top = 10
               Left = 1136
               Bottom = 122
               Right = 1306
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Locations"
            Begin Extent = 
               Top = 137
               Left = 772
               Bottom = 266
               Right = 956
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Ta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTemp_VW2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'ble = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTemp_VW2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTemp_VW2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4[30] 2[40] 3) )"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2[66] 3) )"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4[50] 3) )"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4[60] 2) )"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = -983
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 5
               Left = 245
               Bottom = 166
               Right = 479
            End
            DisplayFlags = 280
            TopColumn = 9
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 147
               Left = 481
               Bottom = 308
               Right = 703
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 8
               Left = 726
               Bottom = 169
               Right = 972
            End
            DisplayFlags = 280
            TopColumn = 11
         End
         Begin Table = "aq"
            Begin Extent = 
               Top = 125
               Left = 22
               Bottom = 286
               Right = 254
            End
            DisplayFlags = 280
            TopColumn = 2
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 45
         Width = 284
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 120' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTemp_VWold'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'0
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTemp_VWold'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTemp_VWold'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 45
               Left = 442
               Bottom = 280
               Right = 646
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 43
               Left = 677
               Bottom = 172
               Right = 868
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "L"
            Begin Extent = 
               Top = 26
               Left = 204
               Bottom = 192
               Right = 388
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "w"
            Begin Extent = 
               Top = 24
               Left = 13
               Bottom = 136
               Right = 183
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 49
               Left = 911
               Bottom = 178
               Right = 1120
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "QAStatus"
            Begin Extent = 
               Top = 51
               Left = 1155
               Bottom = 163
               Right = 1325
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 11' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempAllLogger_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'70
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempAllLogger_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempAllLogger_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[49] 4[33] 2[10] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 12
               Left = 424
               Bottom = 338
               Right = 628
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 10
               Left = 676
               Bottom = 337
               Right = 867
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "L"
            Begin Extent = 
               Top = 19
               Left = 201
               Bottom = 184
               Right = 385
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "w"
            Begin Extent = 
               Top = 15
               Left = 4
               Bottom = 127
               Right = 174
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 12
               Left = 901
               Bottom = 366
               Right = 1110
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "QAStatus"
            Begin Extent = 
               Top = 75
               Left = 1211
               Bottom = 241
               Right = 1381
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 20
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempArcGIS_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempArcGIS_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempArcGIS_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 327
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 6
               Left = 280
               Bottom = 311
               Right = 471
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 509
               Bottom = 324
               Right = 718
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "aq"
            Begin Extent = 
               Top = 6
               Left = 756
               Bottom = 228
               Right = 954
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "L"
            Begin Extent = 
               Top = 6
               Left = 992
               Bottom = 135
               Right = 1176
            End
            DisplayFlags = 280
            TopColumn = 16
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 15
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   B' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempDL_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'egin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 2145
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempDL_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempDL_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 29
               Left = 484
               Bottom = 327
               Right = 688
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 19
               Left = 745
               Bottom = 283
               Right = 954
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "Locations"
            Begin Extent = 
               Top = 41
               Left = 245
               Bottom = 292
               Right = 429
            End
            DisplayFlags = 280
            TopColumn = 12
         End
         Begin Table = "WaterBodies"
            Begin Extent = 
               Top = 46
               Left = 33
               Bottom = 178
               Right = 203
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 18
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2040
         Alias = 1380
         Table = 1170
         Output = 720
         Append = 14' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempGR_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'00
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempGR_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempGR_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Instruments"
            Begin Extent = 
               Top = 40
               Left = 75
               Bottom = 298
               Right = 316
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "InstrumentAccuracyChecks"
            Begin Extent = 
               Top = 64
               Left = 474
               Bottom = 280
               Right = 648
            End
            DisplayFlags = 280
            TopColumn = 1
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 11
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempInstruQA_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempInstruQA_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[16] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 308
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 280
               Bottom = 306
               Right = 489
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Locations"
            Begin Extent = 
               Top = 6
               Left = 527
               Bottom = 135
               Right = 711
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WaterBodies"
            Begin Extent = 
               Top = 6
               Left = 749
               Bottom = 118
               Right = 919
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "QAStatus"
            Begin Extent = 
               Top = 167
               Left = 644
               Bottom = 279
               Right = 814
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 19
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempLine_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempLine_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempLine_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[18] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 21
               Left = 399
               Bottom = 324
               Right = 603
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 646
               Bottom = 308
               Right = 855
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "Locations"
            Begin Extent = 
               Top = 23
               Left = 189
               Bottom = 324
               Right = 373
            End
            DisplayFlags = 280
            TopColumn = 9
         End
         Begin Table = "WaterBodies"
            Begin Extent = 
               Top = 16
               Left = 4
               Bottom = 155
               Right = 174
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "QAStatus"
            Begin Extent = 
               Top = 36
               Left = 905
               Bottom = 169
               Right = 1075
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 10
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Ali' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempLineJoin_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'as = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempLineJoin_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempLineJoin_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[12] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "WaterTempLineJoin_vw"
            Begin Extent = 
               Top = 72
               Left = 142
               Bottom = 253
               Right = 312
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WaterTempInstruQA_vw"
            Begin Extent = 
               Top = 26
               Left = 441
               Bottom = 290
               Right = 611
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempLineQAFinal_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempLineQAFinal_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[45] 4[24] 2[12] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "loc"
            Begin Extent = 
               Top = 20
               Left = 549
               Bottom = 358
               Right = 733
            End
            DisplayFlags = 280
            TopColumn = 7
         End
         Begin Table = "WaterBodies"
            Begin Extent = 
               Top = 0
               Left = 799
               Bottom = 171
               Right = 969
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "LocationProjects"
            Begin Extent = 
               Top = 28
               Left = 283
               Bottom = 170
               Right = 453
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Projects"
            Begin Extent = 
               Top = 24
               Left = 44
               Bottom = 263
               Right = 227
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 12
         Width = 284
         Width = 1500
         Width = 1500
         Width = 2160
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter =' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempLocations_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' 1350
         Or = 1530
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempLocations_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempLocations_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 280
               Bottom = 135
               Right = 489
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Locations"
            Begin Extent = 
               Top = 6
               Left = 527
               Bottom = 135
               Right = 711
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WaterBodies"
            Begin Extent = 
               Top = 6
               Left = 749
               Bottom = 118
               Right = 919
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "QAStatus"
            Begin Extent = 
               Top = 6
               Left = 957
               Bottom = 118
               Right = 1127
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempSOhours_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempSOhours_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempSOhours_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "h"
            Begin Extent = 
               Top = 40
               Left = 705
               Bottom = 334
               Right = 896
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "a"
            Begin Extent = 
               Top = 13
               Left = 438
               Bottom = 338
               Right = 642
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 146
               Left = 115
               Bottom = 325
               Right = 299
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "s"
            Begin Extent = 
               Top = 11
               Left = 81
               Bottom = 140
               Right = 307
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 2385
         Width = 1500
         Width = 1500
         Width = 3255
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempUploadCheck_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WaterTempUploadCheck_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[46] 4[11] 2[21] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 7
               Left = 113
               Bottom = 377
               Right = 306
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 12
               Left = 342
               Bottom = 233
               Right = 528
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "a"
            Begin Extent = 
               Top = 11
               Left = 541
               Bottom = 360
               Right = 745
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "loc"
            Begin Extent = 
               Top = 7
               Left = 769
               Bottom = 358
               Right = 953
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "w"
            Begin Extent = 
               Top = 22
               Left = 975
               Bottom = 161
               Right = 1145
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 35
         Width = 284
         Width = 1500
         Width = 1500
         Width = 3480
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 15' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WQWeb_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'00
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2025
         Alias = 2640
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WQWeb_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WQWeb_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WQWTWeb_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WQWTWeb_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 156
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 6
               Left = 280
               Bottom = 135
               Right = 471
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 509
               Bottom = 135
               Right = 718
            End
            DisplayFlags = 280
            TopColumn = 13
         End
         Begin Table = "aq"
            Begin Extent = 
               Top = 6
               Left = 756
               Bottom = 135
               Right = 954
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "loc"
            Begin Extent = 
               Top = 177
               Left = 219
               Bottom = 306
               Right = 403
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 24
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 150' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WQXAirH20_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'0
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1635
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WQXAirH20_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WQXAirH20_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[15] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "l"
            Begin Extent = 
               Top = 26
               Left = 328
               Bottom = 282
               Right = 512
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "ai"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 182
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 10
         Width = 284
         Width = 1110
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WT_ActiveInactive2_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WT_ActiveInactive2_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[42] 4[19] 2[14] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 19
               Left = 301
               Bottom = 316
               Right = 505
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 16
               Left = 42
               Bottom = 278
               Right = 226
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 0
               Left = 590
               Bottom = 291
               Right = 781
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 11
         Width = 284
         Width = 2220
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTActivities_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTActivities_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[49] 4[21] 2[18] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 12
               Left = 465
               Bottom = 317
               Right = 669
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 308
               Left = 791
               Bottom = 510
               Right = 975
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "u"
            Begin Extent = 
               Top = 241
               Left = 233
               Bottom = 370
               Right = 404
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "qa"
            Begin Extent = 
               Top = 11
               Left = 210
               Bottom = 209
               Right = 408
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 0
               Left = 787
               Bottom = 309
               Right = 978
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 125
               Left = 14
               Bottom = 370
               Right = 240
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width =' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTCDMSvsOld_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTCDMSvsOld_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTCDMSvsOld_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 288
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTDetailsWeb_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTDetailsWeb_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[11] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 13
         Width = 284
         Width = 2370
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTLoc_ActiveInactive_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTLoc_ActiveInactive_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Locations"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 311
               Right = 222
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "WaterBodies"
            Begin Extent = 
               Top = 6
               Left = 260
               Bottom = 286
               Right = 430
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 2655
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTStreams_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTStreams_vw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTWeb_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTWeb_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[12] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "wt"
            Begin Extent = 
               Top = 11
               Left = 127
               Bottom = 309
               Right = 344
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "lc"
            Begin Extent = 
               Top = 19
               Left = 420
               Bottom = 310
               Right = 604
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTWebYearlyExportZip_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTWebYearlyExportZip_VW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[16] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "wb"
            Begin Extent = 
               Top = 119
               Left = 241
               Bottom = 308
               Right = 411
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 9
               Left = 442
               Bottom = 239
               Right = 626
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "lp"
            Begin Extent = 
               Top = 7
               Left = 651
               Bottom = 132
               Right = 821
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "p"
            Begin Extent = 
               Top = 124
               Left = 831
               Bottom = 300
               Right = 1014
            End
            DisplayFlags = 280
            TopColumn = 3
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 18
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         New' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTWQLocationProject'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'Value = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTWQLocationProject'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WTWQLocationProject'
GO
