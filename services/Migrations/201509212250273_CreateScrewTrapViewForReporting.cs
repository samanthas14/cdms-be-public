namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateScrewTrapViewForReporting : DbMigration
    {
        public override void Up()
        {
            Sql(@"
create view Screwtrap_vw as
select
    h.FileTitle, h.ClipFiles, h.TagDateTime, h.ReleaseDateTime, h.Tagger, h.CaptureMethod, h.MigratoryYear, h.LivewellTemp, h.TaggingTemp, h.PostTaggingTemp, 
    h.ReleaseTemp, h.TaggingMethod, h.Organization, h.CoordinatorID, h.ArrivalTime, h.DepartTime, h.ArrivalRPMs, h.DepartureRPMs, h.Hubometer, h.HubometerTime, 
    h.TrapStopped, h.TrapStarted, h.FishCollected, h.FishReleased, h.Flow, h.Turbitity, h.TrapDebris, h.RiverDebris, h.ActivityComments, h.ReleaseSite, 
    h.ReleaseRiverKM, h.Unit,

    d.Sequence, d.PitTagCode, d.SpeciesRunRearing, d.ForkLength, d.Weight, d.OtherSpecies, d.FishCount, d.ConditionalComment, d.TextualComments, d.Note, 
    d.ReleaseLocation, d.FishComments, d.RowId, d.RowStatusId, d.TagStatus, d.ClipStatus,

    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, 
    a.activitydate, a.CreateDate,       -- required

    w.id as WaterbodyId, w.name as WaterbodyName, 

    l.id as LocationId,l.name as LocationName,

    aq.QAStatusName as QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId  -- required
from Screwtrap_Detail d 
join Screwtrap_Header h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId
join locations l on l.id = a.locationid
join waterbodies w on w.id = l.waterbodyid
join ActivityQAs_VW AS aq ON aq.ActivityId = a.Id

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

CREATE VIEW [dbo].[Screwtrap_Header_VW]
AS
SELECT        *
FROM            dbo.Screwtrap_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Screwtrap_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)));

go

update fields        set dbcolumnname = 'ReleaseTime' where  NAME  = 'Release Time'     
update datasetfields set dbcolumnname = 'ReleaseTime' where  LABEL = 'Release Time'     
");
        }

        public override void Down()
        {
            Sql(@"
drop view Screwtrap_vw
go
drop view Screwtrap_Detail_VW
go
drop view Screwtrap_Header_VW
go


");
        }
    }
}
