namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RedoFishScalesView : DbMigration
    {
        public override void Up()
        {
            Sql(@"
drop view FishScales_vw
go
create view FishScales_vw as
select
    h.RunYear, h.Technician,

    d.FieldScaleId, d.GumCardScaleID, d.ScaleCollectionDate, d.Species, d.LifeStage, d.Circuli, d.FreshwaterAge, d.SaltWaterAge, d.TotalAdultAge,
    d.SpawnCheck, d.Regeneration, d.Stock, d.RowId, d.RowStatusId, d.ScaleComments, d.BadScale, d.QAStatusId,

    ef.unit, st.HatNat, coalesce(st.ForkLength, ef.ForkLength) as ForkLength,
    coalesce(st.Weight, ef.Weight) as Weight,

    sgs.FinClips,
    sgs.Marks,
    sgs.MeHPLength,
    sgs.SpawningStatus as PercentRetained,
    sgs.Sex,


    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, a.CreateDate,   
    aq.QAStatusName as QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId,

    w.id as WaterbodyId, w.name as WaterbodyName, 
    l.id as LocationId, 

    l.name as LocationName

from FishScales_Detail d 
left join FishScales_Header h on d.ActivityId = h.ActivityId
left join activities a on a.id = h.ActivityId
left join locations l on l.id = a.locationid
left join waterbodies w on w.id = l.waterbodyid
left join screwTrap_vw st on st.textualcomments = d.FieldScaleID
left join electrofishing_vw ef on ef.textualcomments = d.FieldScaleID
left join spawninggroundsurvey_vw sgs on sgs.scaleId = d.FieldScaleID
left join ActivityQAs_VW AS aq ON aq.ActivityId = a.Id


");


        }

        public override void Down()
        {
            Sql(@"
drop view FishScales_vw
go
create view FishScales_vw as
select
    h.RunYear, h.Technician,

    d.FieldScaleId, d.GumCardScaleID, d.ScaleCollectionDate, d.Species, d.LifeStage, d.Circuli, d.FreshwaterAge, d.SaltWaterAge, d.TotalAdultAge,
    d.SpawnCheck, d.Regeneration, d.Stock, d.RowId, d.RowStatusId, d.ScaleComments, d.BadScale,

    ef.unit, st.HatNat, coalesce(st.ForkLength, ef.ForkLength) as ForkLength,
    coalesce(st.Weight, ef.Weight) as Weight,

    sgs.FinClips,
    sgs.Marks,
    sgs.MeHPLength,
    sgs.SpawningStatus as PercentRetained,
    sgs.Sex,


    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, 
    w.id as WaterbodyId, w.name as WaterbodyName, 
    l.id as LocationId, 

    l.name as LocationName

from FishScales_Detail d 
join FishScales_Header h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId
join locations l on l.id = a.locationid
join waterbodies w on w.id = l.waterbodyid
left join screwTrap_vw st on st.textualcomments = d.FieldScaleID
left join electrofishing_vw ef on ef.textualcomments = d.FieldScaleID
left join spawninggroundsurvey_vw sgs on sgs.scaleId = d.FieldScaleID




");

        }
    }
}
