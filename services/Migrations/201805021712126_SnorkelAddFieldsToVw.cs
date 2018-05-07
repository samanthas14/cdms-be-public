namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SnorkelAddFieldsToVw : DbMigration
    {
        public override void Up()
        {
            Sql(@"
Drop View dbo.[SnorkelFish_vw]
go
Create view dbo.[SnorkelFish_vw]
AS
SELECT        h.Team, h.NoteTaker, h.StartTime, h.EndTime, h.StartWaterTemp, h.Visibility, h.WeatherConditions, h.VisitId, h.Comments, h.CollectionType, h.DominantSpecies, 
                         h.CommonSpecies, h.RareSpecies, iif(h.IsAEM = 'YES', AEMHabitatType, ChannelUnitType) AS UnitType, h.IsAEM, h.HabitatVisitId, h.EndWaterTemp, h.Protocol, 
                         d.NoSnorklers, d.FishID, d.ChannelUnitNumber, d.Lane, d.Type, d.ChannelAverageDepth, d.ChannelLength, d.ChannelWidth, d.ChannelMaxDepth, d.Unit, 
                         d.ChannelLength * d.ChannelWidth AS ChannelArea, d.FishCount, d.Species, d.SizeClass, d.UnidentifiedSalmonID, d.OtherSpeciesPres, d.NaturalWoodUsed, 
                         d.PlacedWoodUsed, d.NaturalBoulderUsed, d.PlacedBoulderUsed, d.NaturalOffChannelUsed, d.CreatedOffChannelUsed, d.NewSideChannelUsed, 
                         d.NoStructureUsed, d.AmbientTemp, d.MinimumTemp, d.FieldNotes, d.ChannelUnitType, d.AEMHabitatType, d.AEMLength, d.RowId, d.RowStatusId, 
                         d.QAStatusId, a.id AS ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, a.CreateDate, w.id AS WaterbodyId, w.name AS WaterbodyName, 
                         l.id AS LocationId, l.Label AS LocationLabel, aq.QAStatusName AS QAStatusName, aq.Comments AS ActivityQAComments, 
                         aq.QAStatusId AS ActivityQAStatusId
FROM            dbo.Activities AS a INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.SnorkelFish_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.Datasets AS ds ON ds.Id = a.DatasetId INNER JOIN
                         dbo.Locations AS l ON l.Id = a.LocationId INNER JOIN
                         dbo.WaterBodies AS w ON w.Id = l.WaterBodyId RIGHT OUTER JOIN
                         dbo.SnorkelFish_Detail_VW AS d ON d.ActivityId = a.Id
            ");
        }
        
        public override void Down()
        {
            Sql(@"
Drop View dbo.[SnorkelFish_vw]
go
Create view dbo.[SnorkelFish_vw]
AS
SELECT        qa.QAStatusName AS ActivityQAStatus, ds.Name AS Dataset, YEAR(a.ActivityDate) AS SnorkelYear, w.Name AS Stream, l.Label AS Location, l.SdeObjectId, 
                         a.ActivityDate, h.VisitId, h.Team, h.NoteTaker, h.CollectionType, h.StartTime, h.StartWaterTemp, h.EndTime, h.EndWaterTemp, h.Visibility, h.WeatherConditions, 
                         h.DominantSpecies, h.CommonSpecies, h.RareSpecies, h.IsAEM, h.HabitatVisitId, h.Protocol, h.Comments AS ActivityComments, d.FishID, d.Unit, 
                         d.ChannelUnitNumber, d.Lane, d.FishCount, d.Species, d.SizeClass, d.NaturalWoodUsed, d.PlacedWoodUsed, d.NaturalBoulderUsed, d.PlacedBoulderUsed, 
                         d.NaturalOffChannelUsed, d.CreatedOffChannelUsed, d.NewSideChannelUsed, d.NoStructureUsed, d.FieldNotes, d.NoSnorklers, d.Type, d.ChannelAverageDepth, 
                         d.ChannelLength, d.ChannelWidth, d.ChannelMaxDepth, d.UnidentifiedSalmonID, d.OtherSpeciesPres, d.AmbientTemp, d.MinimumTemp, d.ChannelUnitType, 
                         d.AEMHabitatType, d.AEMLength
FROM            dbo.SnorkelFish_Header_VW AS h INNER JOIN
                         dbo.ActivityQAs_VW AS qa INNER JOIN
                         dbo.Activities AS a ON qa.ActivityId = a.Id INNER JOIN
                         dbo.Datasets AS ds ON a.DatasetId = ds.Id ON h.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id INNER JOIN
                         dbo.WaterBodies AS w ON l.WaterBodyId = w.Id RIGHT OUTER JOIN
                         dbo.SnorkelFish_Detail_VW AS d ON h.ActivityId = d.ActivityId
            ");
        }
    }
}
