namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SgsUpdViews : DbMigration
    {
        public override void Up()
        {
            Sql(@"
drop view dbo.[SpawningGroundSurvey_Detail_VW]
go
create view dbo.[SpawningGroundSurvey_Detail_VW]
as
SELECT        Id, FeatureID, FeatureType, Species, Time, Temp, Channel, ReddLocation, ReddHabitat, WaypointNumber, FishCount, FishLocation, Sex, 
                         REPLACE(REPLACE(REPLACE(FinClips, '[""', ''), '""]', ''), '"",""', ',') AS FinClips, REPLACE(REPLACE(REPLACE(Marks, '[""', ''), '""]', ''), '"",""', ',') AS Marks, SpawningStatus, 
                         ForkLength, MeHPLength, SnoutID, ScaleID, Tag, TagID, Comments, Ident, EastingUTM, NorthingUTM, DateTime, RowId, RowStatusId, ActivityId, ByUserId, 
                         QAStatusId, EffDt, GeneticID, KidneyID, EstimatedLocation, Origin, PercentRetained, NumberEggsRetained, MortalityType, ReddMeasurements
FROM            dbo.SpawningGroundSurvey_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SpawningGroundSurvey_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
go

drop view dbo.[SpawningGroundSurvey_vw]
go
create view dbo.[SpawningGroundSurvey_vw]
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
go
            ");
        }
        
        public override void Down()
        {
            Sql(@"
drop view dbo.[SpawningGroundSurvey_Detail_VW]
go
create view dbo.[SpawningGroundSurvey_Detail_VW]
as
SELECT        Id, FeatureID, FeatureType, Species, Time, Temp, Channel, ReddLocation, ReddHabitat, WaypointNumber, FishCount, FishLocation, Sex, 
                         REPLACE(REPLACE(REPLACE(FinClips, '[""', ''), '""]', ''), '"",""', ',') AS FinClips, REPLACE(REPLACE(REPLACE(Marks, '[""', ''), '""]', ''), '"",""', ',') AS Marks, SpawningStatus, 
                         ForkLength, MeHPLength, SnoutID, ScaleID, Tag, TagID, Comments, Ident, EastingUTM, NorthingUTM, DateTime, RowId, RowStatusId, ActivityId, ByUserId, 
                         QAStatusId, EffDt, GeneticID, KidneyID, EstimatedLocation, Origin
FROM            dbo.SpawningGroundSurvey_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SpawningGroundSurvey_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
go

drop view dbo.[SpawningGroundSurvey_vw]
go
create view dbo.[SpawningGroundSurvey_vw]
as
SELECT        h.Id AS HeaderID, h.Technicians, h.StartTime, h.EndTime, h.StartTemperature, h.EndTemperature, h.StartEasting, h.StartNorthing, h.EndEasting, h.EndNorthing, 
                         h.Flow, h.WaterVisibility, h.Weather, h.FlaggedRedds, h.NewRedds, h.HeaderComments, h.FieldsheetLink, h.ByUserId AS HeaderUserId, h.EffDt AS HeaderEffDt, 
                         h.TargetSpecies, 
                         d.Id AS DetailID, d.FeatureID, d.FeatureType, d.Species, d.Time, d.Temp, d.Channel, d.ReddLocation, d.ReddHabitat, d.Origin, d.WaypointNumber, 
                         d.FishCount, d.FishLocation, d.Sex, d.FinClips, d.Marks, d.SpawningStatus, d.ForkLength, d.MeHPLength, d.SnoutID, d.ScaleID, d.Tag, d.TagID, d.Comments, d.Ident, 
                         d.EastingUTM, d.NorthingUTM, d.DateTime, d.RowId, d.RowStatusId, d.ByUserId AS DetailUserId, d.QAStatusId, d.EffDt AS DetailEffDt, d.GeneticID, d.KidneyID, 
                         d.EstimatedLocation, 
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
go
            ");
        }
    }
}
