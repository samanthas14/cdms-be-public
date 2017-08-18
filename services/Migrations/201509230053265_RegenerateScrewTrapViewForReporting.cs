namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegenerateScrewTrapViewForReporting : DbMigration
    {
        public override void Up()
        {
            Sql(@"
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
         WHERE TABLE_NAME = 'Screwtrap_vw')
    drop view Screwtrap_vw
go

create view Screwtrap_vw as
select
    h.FileTitle, h.ClipFiles, h.Tagger, h.LivewellTemp, h.TaggingTemp, h.PostTaggingTemp, 
    h.ReleaseTemp, h.ArrivalTime, h.DepartTime, h.ArrivalRPMs, h.DepartureRPMs, h.Hubometer, h.HubometerTime, 
    h.TrapStopped, h.TrapStarted, h.FishCollected, h.FishReleased, h.Flow, h.Turbitity, h.TrapDebris, h.RiverDebris, h.ActivityComments, h.Unit,

    d.Sequence, d.PitTagCode, d.ForkLength, d.Weight, d.OtherSpecies, d.FishCount, d.ConditionalComment, d.TextualComments, d.Note, 
    d.ReleaseLocation, d.FishComments, d.RowId, d.RowStatusId, d.TagStatus, d.ClipStatus, d.AdditionalPositionalComments,

    case 
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

    l.id as LocationId, l.name as LocationName,

    aq.QAStatusName as QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId  -- required
from Screwtrap_Detail d 
join Screwtrap_Header h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId
join locations l on l.id = a.locationid
join waterbodies w on w.id = l.waterbodyid
join ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
  
");
        }


        public override void Down()
        {
            Sql(@"
drop view Screwtrap_vw
go

-- create view Screwtrap_vw as
-- select
--     h.FileTitle, h.ClipFiles, h.TagDateTime, h.ReleaseDateTime, h.Tagger, h.CaptureMethod, h.MigratoryYear, h.LivewellTemp, h.TaggingTemp, h.PostTaggingTemp, 
--     h.ReleaseTemp, h.TaggingMethod, h.Organization, h.CoordinatorID, h.ArrivalTime, h.DepartTime, h.ArrivalRPMs, h.DepartureRPMs, h.Hubometer, h.HubometerTime, 
--     h.TrapStopped, h.TrapStarted, h.FishCollected, h.FishReleased, h.Flow, h.Turbitity, h.TrapDebris, h.RiverDebris, h.ActivityComments, h.ReleaseSite, 
--     h.ReleaseRiverKM, h.Unit,
-- 
--     d.Sequence, d.PitTagCode, d.SpeciesRunRearing, d.ForkLength, d.Weight, d.OtherSpecies, d.FishCount, d.ConditionalComment, d.TextualComments, d.Note, 
--     d.ReleaseLocation, d.FishComments, d.RowId, d.RowStatusId, d.TagStatus, d.ClipStatus,
-- 
--     a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, 
--     a.activitydate, a.CreateDate,       -- required
-- 
--     w.id as WaterbodyId, w.name as WaterbodyName, 
-- 
--     l.id as LocationId,l.name as LocationName,
-- 
--     aq.QAStatusName as QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId  -- required
-- from Screwtrap_Detail d 
-- join Screwtrap_Header h on d.ActivityId = h.ActivityId
-- join activities a on a.id = h.ActivityId
-- join locations l on l.id = a.locationid
-- join waterbodies w on w.id = l.waterbodyid
-- join ActivityQAs_VW AS aq ON aq.ActivityId = a.Id

");
        }
    }
}
