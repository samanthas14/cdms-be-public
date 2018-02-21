namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SnorkUpdViews : DbMigration
    {
        public override void Up()
        {
            Sql(@"
Drop view SnorkelFish_Header_VW
GO
Create view SnorkelFish_Header_VW
AS
SELECT        Id, Team, StartWaterTemp, Visibility, VisitId, Comments, CollectionType, ActivityId, ByUserId, EffDt, NoteTaker, StartTime, EndTime, WeatherConditions, 
                         DominantSpecies, CommonSpecies, RareSpecies, IsAEM, HabitatVisitId, EndWaterTemp, Protocol
FROM            dbo.SnorkelFish_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SnorkelFish_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO


Drop view SnorkelFish_vw
GO
Create view SnorkelFish_vw
AS
SELECT        h.Team, h.NoteTaker, h.StartTime, h.EndTime, h.StartWaterTemp, h.Visibility, h.WeatherConditions, h.VisitId, h.Comments, h.CollectionType, h.DominantSpecies, 
                         h.CommonSpecies, h.RareSpecies, iif(h.IsAEM = 'YES', AEMHabitatType, ChannelUnitType) AS UnitType, h.IsAEM, h.HabitatVisitId, h.EndWaterTemp, h.Protocol, d .NoSnorklers, 
                         d .FishID, d .ChannelUnitNumber, d .Lane, d .Type, d .ChannelAverageDepth, d .ChannelLength, d .ChannelWidth, d .ChannelMaxDepth, d .Unit, 
                         d .ChannelLength * d .ChannelWidth AS ChannelArea, d .FishCount, d .Species, d .SizeClass, d .UnidentifiedSalmonID, d .OtherSpeciesPres, d .NaturalWoodUsed, 
                         d .PlacedWoodUsed, d .NaturalBoulderUsed, d .PlacedBoulderUsed, d .NaturalOffChannelUsed, d .CreatedOffChannelUsed, d .NewSideChannelUsed, 
                         d .NoStructureUsed, d .AmbientTemp, d .MinimumTemp, d .FieldNotes, d .ChannelUnitType, d .AEMHabitatType, d .AEMLength, d .RowId, d .RowStatusId, 
                         d .QAStatusId, a.id AS ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, a.CreateDate, w.id AS WaterbodyId, w.name AS WaterbodyName, 
                         l.id AS LocationId, l.Label AS LocationLabel, aq.QAStatusName AS QAStatusName, aq.Comments AS ActivityQAComments, 
                         aq.QAStatusId AS ActivityQAStatusId
/* required*/ FROM SnorkelFish_Detail_VW d JOIN
                         SnorkelFish_Header_VW h ON d .ActivityId = h.ActivityId JOIN
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
Drop view SnorkelFish_Header_VW
GO
Create view SnorkelFish_Header_VW
AS
SELECT        Id, Team, StartWaterTemp, Visibility, VisitId, Comments, CollectionType, ActivityId, ByUserId, EffDt, NoteTaker, StartTime, EndTime, WeatherConditions, 
                         DominantSpecies, CommonSpecies, RareSpecies, IsAEM, HabitatVisitId, EndWaterTemp
FROM            dbo.SnorkelFish_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SnorkelFish_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO


Drop view SnorkelFish_vw
GO
Create view SnorkelFish_vw
AS
SELECT        h.Team, h.NoteTaker, h.StartTime, h.EndTime, h.StartWaterTemp, h.Visibility, h.WeatherConditions, h.VisitId, h.Comments, h.CollectionType, h.DominantSpecies, 
                         h.CommonSpecies, h.RareSpecies, iif(h.IsAEM = 'YES', AEMHabitatType, ChannelUnitType) AS UnitType, h.IsAEM, h.HabitatVisitId, h.EndWaterTemp, d .NoSnorklers, 
                         d .FishID, d .ChannelUnitNumber, d .Lane, d .Type, d .ChannelAverageDepth, d .ChannelLength, d .ChannelWidth, d .ChannelMaxDepth, d .Unit, 
                         d .ChannelLength * d .ChannelWidth AS ChannelArea, d .FishCount, d .Species, d .SizeClass, d .UnidentifiedSalmonID, d .OtherSpeciesPres, d .NaturalWoodUsed, 
                         d .PlacedWoodUsed, d .NaturalBoulderUsed, d .PlacedBoulderUsed, d .NaturalOffChannelUsed, d .CreatedOffChannelUsed, d .NewSideChannelUsed, 
                         d .NoStructureUsed, d .AmbientTemp, d .MinimumTemp, d .FieldNotes, d .ChannelUnitType, d .AEMHabitatType, d .AEMLength, d .RowId, d .RowStatusId, 
                         d .QAStatusId, a.id AS ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, a.CreateDate, w.id AS WaterbodyId, w.name AS WaterbodyName, 
                         l.id AS LocationId, l.name AS LocationLabel, aq.QAStatusName AS QAStatusName, aq.Comments AS ActivityQAComments, 
                         aq.QAStatusId AS ActivityQAStatusId
/* required*/ FROM SnorkelFish_Detail_VW d JOIN
                         SnorkelFish_Header_VW h ON d .ActivityId = h.ActivityId JOIN
                         activities a ON a.id = h.ActivityId JOIN
                         locations l ON l.id = a.locationid JOIN
                         waterbodies w ON w.id = l.waterbodyid JOIN
                         ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
GO
            ");
        }
    }
}
