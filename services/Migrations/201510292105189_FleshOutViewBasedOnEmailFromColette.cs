namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FleshOutViewBasedOnEmailFromColette : DbMigration
    {
        public override void Up()
        {
            Sql(@"
drop view FishScales_vw
go
create view FishScales_vw as
select
    h.RunYear, h.Technician,

    d.FieldScaleId, d.GumCardScaleID, d.ScaleCollectionDate, d.Species, d.Circuli, d.FreshwaterAge, d.SaltWaterAge, d.TotalAdultAge,
    d.SpawnCheck, d.Regeneration, d.Stock, d.RowId, d.RowStatusId, d.ScaleComments, d.BadScale, d.QAStatusId,

    ef.unit, 
    coalesce(st.HatNat, sgs.origin, aw.origin) as HatNat,
    coalesce(st.ForkLength, ef.ForkLength, sgs.ForkLength, aw.ForkLength) as ForkLength,
             aw.LifeStage as LifeStage,
    coalesce(st.Weight, ef.Weight, aw.Weight) as Weight,

    coalesce(st.AdditionalPositionalComments, ef.AdditionalPositionalComment, sgs.FinClips, aw.FinClip) as FinClips,
    coalesce(sgs.Marks, aw.Mark) as Marks,
            sgs.MeHPLength as MeHPLength,
    coalesce(sgs.SpawningStatus, aw.PercentSpawned) as PercentRetained,
    coalesce(sgs.Sex, aw.Sex) as Sex,


    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, a.CreateDate,   
    aq.QAStatusName as QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId,
 
    coalesce(st.LocationLabel, ef.LocationLabel, sgs.LocationLabel--, aw.LocationLabel  <--- AdultWeir LocationLabel goes here
        ) as LocationLabel,
    coalesce(st.LocationId, ef.LocationId, sgs.LocationId--, aw.LocationId  <--- AdultWeir LocationId goes here
        ) as LocationId
from FishScales_Detail_vw d 
left join FishScales_Header_vw h on d.ActivityId = h.ActivityId
left join activities a on a.id = h.ActivityId

-- External tables
left join screwTrap_vw st on st.textualcomments = d.FieldScaleID
left join electrofishing_vw ef on ef.textualcomments = d.FieldScaleID
left join spawninggroundsurvey_vw sgs on sgs.scaleId = d.FieldScaleID
left join adultWeir_vw aw on aw.ScaleId = d.FieldScaleId

left join ActivityQAs_VW AS aq ON aq.ActivityId = a.Id


go

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

    l.name as LocationLabel

from FishScales_Detail_vw d 
left join FishScales_Header_vw h on d.ActivityId = h.ActivityId
left join activities a on a.id = h.ActivityId
left join locations l on l.id = a.locationid
left join waterbodies w on w.id = l.waterbodyid
left join screwTrap_vw st on st.textualcomments = d.FieldScaleID
left join electrofishing_vw ef on ef.textualcomments = d.FieldScaleID
left join spawninggroundsurvey_vw sgs on sgs.scaleId = d.FieldScaleID
left join ActivityQAs_VW AS aq ON aq.ActivityId = a.Id

go

");
        }
    }
}
