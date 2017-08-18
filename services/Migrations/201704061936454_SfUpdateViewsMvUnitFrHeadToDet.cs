namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SfUpdateViewsMvUnitFrHeadToDet : DbMigration
    {
        public override void Up()
        {
            Sql(@"
--SnorkelFish views
drop view [dbo].[SnorkelFish_Detail_VW]
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

drop view [dbo].[SnorkelFish_Header_VW]
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

drop view [dbo].[SnorkelFish_vw]
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
            ");
        }

        public override void Down()
        {
            Sql(@"
--SnorkelFish views
drop view [dbo].[SnorkelFish_Detail_VW]
GO
CREATE VIEW [dbo].[SnorkelFish_Detail_VW]
AS
SELECT        Id, FishID, ChannelUnitNumber, Lane, FishCount, Species, SizeClass, NaturalWoodUsed, PlacedWoodUsed, NaturalBoulderUsed, PlacedBoulderUsed, 
                         NaturalOffChannelUsed, CreatedOffChannelUsed, NewSideChannelUsed, NoStructureUsed, FieldNotes, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, 
                         NoSnorklers, Type, ChannelAverageDepth, ChannelLength, ChannelWidth, ChannelMaxDepth, UnidentifiedSalmonID, OtherSpeciesPres, AmbientTemp, 
                         MinimumTemp, ChannelUnitType, AEMHabitatType, AEMLength
FROM            dbo.SnorkelFish_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SnorkelFish_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
GO

drop view [dbo].[SnorkelFish_Header_VW]
GO
CREATE VIEW [dbo].[SnorkelFish_Header_VW]
AS
SELECT        Id, Team, WaterTemperature, Visibility, VisitId, Comments, CollectionType, ActivityId, ByUserId, EffDt, NoteTaker, StartTime, EndTime, WeatherConditions, 
                         DominantSpecies, CommonSpecies, RareSpecies, IsAEM, Unit
FROM            dbo.SnorkelFish_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SnorkelFish_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO

drop view [dbo].[SnorkelFish_vw]
GO
CREATE VIEW [dbo].[SnorkelFish_vw]
AS
SELECT        h.Team, h.NoteTaker, h.StartTime, h.EndTime, h.WaterTemperature, h.Visibility, h.WeatherConditions, h.VisitId, h.Comments, h.CollectionType, h.DominantSpecies, 
                         h.CommonSpecies, h.RareSpecies, h.Unit, iif(h.IsAEM = 'YES', AEMHabitatType, ChannelUnitType) AS UnitType, h.IsAEM, d.NoSnorklers, d.FishID, 
                         d.ChannelUnitNumber, d.Lane, d.Type, d.ChannelAverageDepth, d.ChannelLength, d.ChannelWidth, d.ChannelMaxDepth,
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
            ");
        }
    }
}
