namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReRedoLostsOfViews : DbMigration
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

drop view Screwtrap_vw
go
drop view Screwtrap_Header_VW
go
drop view Screwtrap_Detail_VW
go


CREATE VIEW [dbo].[Screwtrap_Header_VW]
AS
SELECT        *
FROM            dbo.Screwtrap_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Screwtrap_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)));
go

CREATE VIEW [dbo].[Screwtrap_Detail_VW]
AS
SELECT        *
FROM            dbo.Screwtrap_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Screwtrap_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId)))
go
create view Screwtrap_vw as
select
    h.FileTitle, h.ClipFiles, h.Tagger, h.LivewellTemp, h.TaggingTemp, h.PostTaggingTemp, 
    h.ReleaseTemp, h.ArrivalTime, h.DepartTime, h.ArrivalRPMs, h.DepartureRPMs, h.Hubometer, h.HubometerTime, 
    h.TrapStopped, h.TrapStarted, h.FishCollected, h.FishReleased, h.Flow, h.Turbitity, h.TrapDebris, h.RiverDebris, h.ActivityComments, h.Unit,

    d.Sequence, d.PitTagCode, d.ForkLength, d.Weight, d.OtherSpecies, d.FishCount, d.ConditionalComment, d.TextualComments, d.Note, 
    d.ReleaseLocation, d.FishComments, d.RowId, d.RowStatusId, d.TagStatus, d.ClipStatus, d.AdditionalPositionalComments,

    case 
        when SpeciesRunRearing in('00U','25H','25W','3RH','3RW','7RW','90U','A0W','D0W','ERU','G0W') then '' -- All species that are not Chinook or Steelhead
        when d.ForkLength < 100 then 'Small'
        when d.ForkLength > 120 then 'Large'
        else 'Medium'
    end as SizeClass,

    case
	    when SpeciesRunRearing = '00U' then 'Unknown (fish not observed)'
	    when SpeciesRunRearing = '11H' then 'Hat. Spring Chinook'
	    when SpeciesRunRearing = '11U' then 'Spring Chinook (unknown r/t)'
	    when SpeciesRunRearing = '11W' then 'Wild Spring Chinook'
	    when SpeciesRunRearing = '13H' then 'Hat. Fall Chinook'
	    when SpeciesRunRearing = '13W' then 'Wild Fall Chinook'
	    when SpeciesRunRearing = '25H' then 'Hat. Coho'
	    when SpeciesRunRearing = '25W' then 'Wild Coho'
	    when SpeciesRunRearing = '32H' then 'Hat. Summer Steelhead'
	    when SpeciesRunRearing = '32W' then 'Wild Summer Steelhead'
	    when SpeciesRunRearing = '3RH' then 'Hat. Rainbow Trout'
	    when SpeciesRunRearing = '3RW' then 'Wild Rainbow Trout'
	    when SpeciesRunRearing = '7RW' then 'Bull Trout'
	    when SpeciesRunRearing = '90U' then 'Other'
	    when SpeciesRunRearing = 'A0W' then 'Lamprey'
	    when SpeciesRunRearing = 'D0W' then 'Northern Pikeminnow'
	    when SpeciesRunRearing = 'ERU' then 'Brook Trout'
	    when SpeciesRunRearing = 'G0W' then 'Mountain Whitefish'
	    else 'Other'
    end as SpeciesRunRearing,

    case
        when SpeciesRunRearing in ('11H', '13H', '25H', '32H', '3RH') then 'HAT'
        when OtherSpecies in ('HATCHERY CHS', 'HATCHERY STS') then 'HAT'
        when SpeciesRunRearing in ('11U', '11W', '13W', '25W', '32W', '3RW') then 'NAT'
        when OtherSpecies in ('CHINOOK', 'STEELHEAD') then 'NAT'
        else 'UNK'
    end as HatNat,


    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, 
    a.ActivityDate, a.CreateDate,       -- required

    w.id as WaterbodyId, w.name as WaterbodyName, 

    l.id as LocationId, l.name as LocationLabel,

    aq.QAStatusName as QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId  -- required
from Screwtrap_Detail_vw d 
join Screwtrap_Header_vw h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId
join locations l on l.id = a.locationid
join waterbodies w on w.id = l.waterbodyid
join ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
go


drop view WaterQuality_Header_VW
drop view WaterQuality_vw
go

CREATE VIEW [dbo].[WaterQuality_Header_VW]
AS
SELECT        *
FROM            dbo.WaterQuality_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.WaterQuality_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)));

go


create view WaterQuality_vw as
select
    h.DataType, h.fieldsheetlink,

    d.CharacteristicName, d.Result, d.ResultUnits, d.LabDuplicate, d.Comments, d.RowId, d.RowStatusId, d.MdlResults, d.SampleDate, d.SampleID,
    d.SampleFraction, d.MethodSpeciation, d.DetectionLimit, d.ContextID, d.MethodID, d.LabName,

    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, 
    a.ActivityDate, a.CreateDate,       -- required

    loc.OtherAgencyId,
    loc.id as LocationId,


    aq.QAStatusName as QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId  -- required
from WaterQuality_Detail_vw d 
join WaterQuality_Header_vw h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId

join locations loc on loc.id = a.locationid
--join waterbodies w on w.id = l.waterbodyid
join ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
go

drop view SnorkelFish_vw
go
drop view SnorkelFish_Detail_VW
go

CREATE VIEW [dbo].[SnorkelFish_Detail_VW]
AS
SELECT        *
FROM            dbo.SnorkelFish_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SnorkelFish_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId)))
go


create view SnorkelFish_vw as
select
    h.Team, h.NoteTaker, h.StartTime, h.EndTime, h.WaterTemperature, h.Visibility, h.WeatherConditions, h.VisitId, h.Comments, h.CollectionType, 
    h.DominantSpecies, h.CommonSpecies, h.RareSpecies, h.Unit, iif(h.IsAEM='YES',AEMHabitatType,ChannelUnitType) as UnitType,

    d.NoSnorklers, d.FishID, d.ChannelUnitNumber, d.Lane, d.Type, d.ChannelAverageDepth, d.ChannelLength, 
    d.ChannelWidth, d.ChannelMaxDepth, d.ChannelLength *  d.ChannelWidth as ChannelArea, d.FishCount, d.Species, d.SizeClass, 
    d.UnidentifiedSalmonID, d.OtherSpeciesPres, d.NaturalWoodUsed, 
    d.PlacedWoodUsed, d.NaturalBoulderUsed, d.PlacedBoulderUsed, d.NaturalOffChannelUsed, d.CreatedOffChannelUsed, d.NewSideChannelUsed, 
    d.NoStructureUsed, d.AmbientTemp, d.MinimumTemp, d.FieldNotes, d.ChannelUnitType, d.AEMHabitatType, d.AEMLength,

    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, w.id as WaterbodyId, w.name as WaterbodyName, l.id as LocationId, 

    l.name as LocationLabel

from SnorkelFish_Detail_vw d 
join SnorkelFish_Header_vw h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId
join locations l on l.id = a.locationid
join waterbodies w on w.id = l.waterbodyid

go

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
    l.name as LocationLabel, (case species when 'STS' then year(dateadd(month,3,activitydate)) else year(activitydate)end) as runyear,
    aq.QAStatusName as QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId  -- these are required
from SpawningGroundSurvey_Detail_vw d 
join SpawningGroundSurvey_Header_vw h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId
join locations l on l.id = a.locationid
join waterbodies w on w.id = l.waterbodyid
join ActivityQAs_VW AS aq ON aq.ActivityId = a.Id

go

drop view Electrofishing_Detail_VW

go

CREATE VIEW [dbo].[Electrofishing_Detail_VW]
AS
SELECT        *
FROM            dbo.Electrofishing_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Electrofishing_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId)))
go

drop view Electrofishing_vw

go

create view Electrofishing_vw as
select
    h.Id, h.FishNumber, h.EventType, h.FileTitle, h.ClipFiles, h.Crew, h.ReleaseTemp, 
    h.Conductivity, h.EFModel, h.SiteLength, h.SiteWidth, h.SiteDepth, h.SiteArea, h.HabitatType,
    h.Visibility, h.ActivityComments, h.ReleaseSite, h.Weather, h.ReleaseRiverKM, h.TotalFishCaptured, 
    h.ReleaseLocation, h.VisitID, h.Unit, h.StartTime, h.EndTime, 
    h.ReleaseTime, h.StartTemp, h.Pass1TimeBegin, h.Pass1TimeEnd, h.Pass1TotalSecondsEF, h.Pass1WaterTempBegin, 
    h.Pass1WaterTempStop, h.Pass1Hertz, h.Pass1Freq, h.Pass1Volts, h.Pass2TimeBegin, h.Pass2TimeEnd, h.Pass2TotalSecondsEF, 
    h.Pass2WaterTempBegin, h.Pass2WaterTempStop, h.Pass2Hertz, h.Pass2Freq, h.Pass2Volts, h.Pass3TimeBegin, 
    h.Pass3TimeEnd, h.Pass3TotalSecondsEF, h.Pass3WaterTempBegin, h.Pass3WaterTempStop, h.Pass3Hertz, 
    h.Pass3Freq, h.Pass3Volts, h.Pass4TimeBegin, h.Pass4TimeEnd, h.Pass4TotalSecondsEF, h.Pass4WaterTempBegin, 
    h.Pass4WaterTempStop, h.Pass4Hertz, h.Pass4Freq, h.Pass4Volts, h.Pass5TimeBegin, h.Pass5TimeEnd, 
    h.Pass5TotalSecondsEF, h.Pass5WaterTempBegin, h.Pass5WaterTempStop, h.Pass5Hertz, h.Pass5Freq, h.Pass5Volts, 
    h.Pass6TimeBegin, h.Pass6TimeEnd, h.Pass6TotalSecondsEF, h.Pass6WaterTempBegin, h.Pass6WaterTempStop, h.Pass6Hertz, 
    h.Pass6Freq, h.Pass6Volts,

    d.Sequence, d.PitTagCode, d.SpeciesRunRearing, d.ForkLength, d.Weight, d.OtherSpecies, d.FishCount, d.ConditionalComment, d.TextualComments, d.Note, 
    d.OtolithID, d.GeneticID, d.OtherID, d.RowId, d.RowStatusId, d.ByUserId, d.QAStatusId, d.TagStatus, d.ClipStatus, d.SizeCategory,

    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, 
    a.activitydate, a.CreateDate,       -- required

    w.id as WaterbodyId, w.name as WaterbodyName, 

    l.id as LocationId,l.name as LocationLabel,

    aq.QAStatusName as QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId  -- required
from Electrofishing_Detail_vw d 
join Electrofishing_Header_vw h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId
join locations l on l.id = a.locationid
join waterbodies w on w.id = l.waterbodyid
join ActivityQAs_VW AS aq ON aq.ActivityId = a.Id

go




");
        }
        
        public override void Down()
        {
            // No down... running the above will leave you in essentially the same state as when you began
        }
    }
}
