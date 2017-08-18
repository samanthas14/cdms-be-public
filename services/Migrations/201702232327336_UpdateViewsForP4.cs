namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateViewsForP4 : DbMigration
    {
        public override void Up()
        {
            Sql(@"
drop view [dbo].[Screwtrap_Header_VW]
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


drop view [dbo].[Screwtrap_Detail_VW]
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

drop view [dbo].[Screwtrap_vw]
GO
CREATE VIEW [dbo].[Screwtrap_vw]
AS
SELECT        h.FileTitle, h.ClipFiles, h.Tagger, h.LivewellTemp, h.TaggingTemp, h.PostTaggingTemp, h.ReleaseTemp, h.ArrivalTime, h.DepartTime, h.ArrivalRPMs, 
                         h.DepartureRPMs, h.Hubometer, h.HubometerTime, h.TrapStopped, h.TrapStarted, h.FishCollected, h.FishReleased, h.Flow, h.Turbidity, h.TrapDebris, h.RiverDebris, 
                         h.ActivityComments, h.Unit, h.DailyFinClips, h.Crew, h.TrapStatus, h.Weather,
                         d.Sequence, d.PitTagCode, d.ForkLength, d.Weight, d.OtherSpecies, d.FishCount, 
                         d.ConditionalComment, d.TextualComments, d.Note, d.ReleaseLocation, d.FishComments, d.RowId, d.RowStatusId, d.TagStatus, d.ClipStatus, 
                         d.AdditionalPositionalComments, d.EventType, d.SecondPitTag, 
                         d.RacewayTransectTank, d.LifeStage, d.GeneticId, d.CodedWireTag, d.BroodYear, d.MigrationYear, d.SizeOfCount, d.ScaleId, d.Containment,
CASE WHEN SpeciesRunRearing IN ('00U', '25H', '25W', '3RH', '3RW', '7RW', '90U', 'A0W', 'D0W', 'ERU', 'G0W') 
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
                         THEN 'NAT' ELSE 'UNK' END AS HatNat, a.Id AS ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, a.CreateDate, w.Id AS WaterbodyId, 
                         w.Name AS WaterbodyName, l.Id AS LocationId, l.Name AS LocationLabel, aq.QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId, 
                         aq.QAStatusId AS ActivityQAStatusId
FROM            dbo.Screwtrap_Detail_VW AS d INNER JOIN
                         dbo.Screwtrap_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.Locations AS l ON l.Id = a.LocationId LEFT OUTER JOIN
                         dbo.WaterBodies AS w ON w.Id = l.WaterBodyId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
GO


--Electrofishing views
drop view [dbo].[Electrofishing_Detail_VW]
GO
CREATE VIEW [dbo].[Electrofishing_Detail_VW]
AS
SELECT        Id, Sequence, PitTagCode, SpeciesRunRearing, ForkLength, Weight, OtherSpecies, FishCount, ConditionalComment, TextualComments, Note, OtolithID, GeneticID, 
                         OtherID, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, TagStatus, ClipStatus, SizeCategory, TotalLength, AdditionalPositionalComment, 
                         ChannelUnitType, EventTypeD, SecondPitTag, RacewayTransectTank, LifeStage, CodedWireTag, BroodYear, MigrationYear, SizeOfCount, ScaleId, Containment
FROM            dbo.Electrofishing_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Electrofishing_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
GO

drop view [dbo].[Electrofishing_vw]
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
                         h.Pass6Hertz, h.Pass6Freq, h.Pass6Volts, h.FieldsheetLink, 
d.Sequence, d.PitTagCode, d.SpeciesRunRearing, d.ForkLength, d.Weight, d.OtherSpecies, d.FishCount, 
                         d.ConditionalComment, d.TextualComments, d.Note, d.OtolithID, d.GeneticID, d.OtherID, d.RowId, d.RowStatusId, d.ByUserId, d.QAStatusId, d.TagStatus, d.ClipStatus, 
                         d.SizeCategory, d.ChannelUnitType, d.AdditionalPositionalComment, d.TotalLength, d.EventTypeD, d.SecondPitTag, d.RacewayTransectTank, d.LifeStage, d.CodedWireTag, 
d.BroodYear, d.MigrationYear, d.SizeOfCount, d.ScaleId, d.Containment,
a.Id AS ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, 
                         a.CreateDate, w.Id AS WaterbodyId, w.Name AS WaterbodyName, l.Id AS LocationId, l.Name AS LocationLabel, aq.QAStatusName, 
                         aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId
FROM            dbo.Electrofishing_Detail_VW AS d INNER JOIN
                         dbo.Electrofishing_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId INNER JOIN
                         dbo.Locations AS l ON l.Id = a.LocationId INNER JOIN
                         dbo.WaterBodies AS w ON w.Id = l.WaterBodyId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
GO
            ");
        }

        public override void Down()
        {
            Sql(@"
-- ScrewTrap Views
drop view [dbo].[Screwtrap_Header_VW]
GO
CREATE VIEW [dbo].[Screwtrap_Header_VW]
AS
SELECT        Id, FileTitle, ClipFiles, Tagger, LivewellTemp, TaggingTemp, PostTaggingTemp, ReleaseTemp, ArrivalTime, DepartTime, ArrivalRPMs, DepartureRPMs, Hubometer, 
                         HubometerTime, TrapStopped, TrapStarted, FishCollected, FishReleased, Flow, Turbidity, TrapDebris, RiverDebris, ActivityComments, ActivityId, ByUserId, EffDt, 
                         Unit, DailyFinClips, Crew, TrapStatus
FROM            dbo.ScrewTrap_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.ScrewTrap_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO


drop view [dbo].[Screwtrap_Detail_VW]
GO
CREATE VIEW [dbo].[Screwtrap_Detail_VW]
AS
SELECT        Id, Sequence, PitTagCode, SpeciesRunRearing, ForkLength, Weight, OtherSpecies, FishCount, ConditionalComment, TextualComments, Note, ReleaseLocation, 
                         FishComments, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, TagStatus, ClipStatus, AdditionalPositionalComments
FROM            dbo.ScrewTrap_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.ScrewTrap_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
GO


drop view [dbo].[Screwtrap_vw]
GO
CREATE VIEW [dbo].[Screwtrap_vw]
AS
SELECT        h.FileTitle, h.ClipFiles, h.Tagger, h.LivewellTemp, h.TaggingTemp, h.PostTaggingTemp, h.ReleaseTemp, h.ArrivalTime, h.DepartTime, h.ArrivalRPMs, 
                         h.DepartureRPMs, h.Hubometer, h.HubometerTime, h.TrapStopped, h.TrapStarted, h.FishCollected, h.FishReleased, h.Flow, h.Turbidity, h.TrapDebris, h.RiverDebris, 
                         h.ActivityComments, h.Unit, h.DailyFinClips, h.Crew, h.TrapStatus, d.Sequence, d.PitTagCode, d.ForkLength, d.Weight, d.OtherSpecies, d.FishCount, 
                         d.ConditionalComment, d.TextualComments, d.Note, d.ReleaseLocation, d.FishComments, d.RowId, d.RowStatusId, d.TagStatus, d.ClipStatus, 
                         d.AdditionalPositionalComments, CASE WHEN SpeciesRunRearing IN ('00U', '25H', '25W', '3RH', '3RW', '7RW', '90U', 'A0W', 'D0W', 'ERU', 'G0W') 
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
                         THEN 'NAT' ELSE 'UNK' END AS HatNat, a.Id AS ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, a.CreateDate, w.Id AS WaterbodyId, 
                         w.Name AS WaterbodyName, l.Id AS LocationId, l.Name AS LocationLabel, aq.QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId, 
                         aq.QAStatusId AS ActivityQAStatusId
FROM            dbo.Screwtrap_Detail_VW AS d INNER JOIN
                         dbo.Screwtrap_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.Locations AS l ON l.Id = a.LocationId LEFT OUTER JOIN
                         dbo.WaterBodies AS w ON w.Id = l.WaterBodyId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
GO

--Electrofishing views
drop view [dbo].[Electrofishing_Detail_VW]
GO
CREATE VIEW [dbo].[Electrofishing_Detail_VW]
AS
SELECT        Id, Sequence, PitTagCode, SpeciesRunRearing, ForkLength, Weight, OtherSpecies, FishCount, ConditionalComment, TextualComments, Note, OtolithID, GeneticID, 
                         OtherID, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, TagStatus, ClipStatus, SizeCategory, TotalLength, AdditionalPositionalComment, 
                         ChannelUnitType
FROM            dbo.Electrofishing_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Electrofishing_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
GO


drop view [dbo].[Electrofishing_vw]
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
                         d.SizeCategory, d.ChannelUnitType, d.AdditionalPositionalComment, d.TotalLength, a.Id AS ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, 
                         a.CreateDate, w.Id AS WaterbodyId, w.Name AS WaterbodyName, l.Id AS LocationId, l.Name AS LocationLabel, aq.QAStatusName, 
                         aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId
FROM            dbo.Electrofishing_Detail_VW AS d INNER JOIN
                         dbo.Electrofishing_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId INNER JOIN
                         dbo.Locations AS l ON l.Id = a.LocationId INNER JOIN
                         dbo.WaterBodies AS w ON w.Id = l.WaterBodyId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
GO
            ");
        }
    }
}
