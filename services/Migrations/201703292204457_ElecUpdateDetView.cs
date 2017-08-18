namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ElecUpdateDetView : DbMigration
    {
        public override void Up()
        {
            Sql(@"
--Electrofishing views
drop view [dbo].[Electrofishing_Detail_VW]
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
                ");
        }
        
        public override void Down()
        {
            Sql(@"
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
                         h.Pass6Hertz, h.Pass6Freq, h.Pass6Volts, h.FieldsheetLink, d.Sequence, d.PitTagCode, d.SpeciesRunRearing, d.ForkLength, d.Weight, d.OtherSpecies, d.FishCount, 
                         d.ConditionalComment, d.TextualComments, d.Note, d.OtolithID, d.GeneticID, d.OtherID, d.RowId, d.RowStatusId, d.ByUserId, d.QAStatusId, d.TagStatus, d.ClipStatus, 
                         d.SizeCategory, d.ChannelUnitType, d.AdditionalPositionalComment, d.TotalLength, d.EventTypeD, d.SecondPitTag, d.RacewayTransectTank, d.LifeStage, 
                         d.CodedWireTag, d.BroodYear, d.MigrationYear, d.SizeOfCount, d.ScaleId, d.Containment, a.Id AS ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, 
                         a.ActivityDate, a.CreateDate, w.Id AS WaterbodyId, w.Name AS WaterbodyName, l.Id AS LocationId, l.Name AS LocationLabel, aq.QAStatusName, 
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
