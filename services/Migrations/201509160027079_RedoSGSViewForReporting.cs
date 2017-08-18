namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RedoSGSViewForReporting : DbMigration
    {
        public override void Up()
        {
            Sql(@"
drop view SpawningGroundSurvey_vw
go
create view SpawningGroundSurvey_vw as
select h.Id as HeaderID, h.Technicians, h.StartTime, h.EndTime, h.StartTemperature, h.EndTemperature, h.StartEasting, h.StartNorthing, h.EndEasting, h.EndNorthing, h.Flow, h.WaterVisibility, 
    h.Weather, h.FlaggedRedds, h.NewRedds, h.HeaderComments, h.FieldsheetLink, h.ByUserId as HeaderUserId, h.EffDt as HeaderEffDt, h.TargetSpecies, d.Id as DetailID, d.FeatureID, d.FeatureType, 
    d.Species, d.Time, d.Temp, d.Channel, d.ReddLocation, d.ReddHabitat, d.Origin, d.WaypointNumber, d.FishCount, d.FishLocation, d.Sex, d.FinClips, d.Marks, d.SpawningStatus, d.ForkLength, d.MeHPLength, 
    d.SnoutID, d.ScaleID, d.Tag, d.TagID, d.Comments, d.Ident, d.EastingUTM, d.NorthingUTM, d.DateTime, d.RowId, d.RowStatusId, d.ByUserId as DetailUserId, d.QAStatusId, d.EffDt as DetailEffDt, 
    d.GeneticID, d.KidneyID, d.EstimatedLocation, 
    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.activitydate, a.CreateDate,  -- these last 2 are required
    w.id as WaterbodyId, w.name as WaterbodyName, l.id as LocationId, 
    l.name as LocationName, (case species when 'STS' then year(dateadd(month,3,activitydate)) else year(activitydate)end) as runyear,
    aq.QAStatusName as QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId  -- these are required
from SpawningGroundSurvey_Detail d 
join SpawningGroundSurvey_Header h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId
join locations l on l.id = a.locationid
join waterbodies w on w.id = l.waterbodyid
join ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
");
        }

        public override void Down()
        {
            Sql(@"
drop view SpawningGroundSurvey_vw
go
create view SpawningGroundSurvey_vw as
select h.Id as HeaderID, h.Technicians, h.StartTime, h.EndTime, h.StartTemperature, h.EndTemperature, h.StartEasting, h.StartNorthing, h.EndEasting, h.EndNorthing, h.Flow, h.WaterVisibility, 
    h.Weather, h.FlaggedRedds, h.NewRedds, h.HeaderComments, h.FieldsheetLink, h.ByUserId as HeaderUserId, h.EffDt as HeaderEffDt, h.TargetSpecies, d.Id as DetailID, d.FeatureID, d.FeatureType, 
    d.Species, d.Time, d.Temp, d.Channel, d.ReddLocation, d.ReddHabitat, d.Origin, d.WaypointNumber, d.FishCount as TotalFishRepresented, d.FishLocation, d.Sex, d.FinClips, d.Marks, d.SpawningStatus, d.ForkLength, d.MeHPLength, 
    d.SnoutID, d.ScaleID, d.Tag, d.TagID, d.Comments, d.Ident, d.EastingUTM, d.NorthingUTM, d.DateTime, d.RowId, d.RowStatusId, d.ByUserId as DetailUserId, d.QAStatusId, d.EffDt as DetailEffDt, 
    d.GeneticID, d.KidneyID, d.EstimatedLocation, a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.activitydate, w.id as WaterbodyId, w.name as WaterbodyName, l.id as LocationId, 
    l.name as LocationName, (case species when 'STS' then year(dateadd(month,3,activitydate)) else year(activitydate)end) as runyear
from SpawningGroundSurvey_Detail d 
join SpawningGroundSurvey_Header h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId
join locations l on l.id = a.locationid
join waterbodies w on w.id = l.waterbodyid
go
");
        }
    }
}