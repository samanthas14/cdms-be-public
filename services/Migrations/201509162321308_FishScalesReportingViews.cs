namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FishScalesReportingViews : DbMigration
    {
        public override void Up()
        {
            Sql(@"
create view FishScales_vw as
select
    h.RunYear, h.Technician,

    d.FieldScaleID, d.GumCardScaleID, d.ScaleCollectionDate, d.Species, d.LifeStage, d.Circuli, d.FreshwaterAge, d.SaltWaterAge, d.TotalAdultAge, d.SpawnCheck, 
    d.Regeneration, d.Stock, d.RowId, d.RowStatusId, d.ScaleComments, d.BadScale,

    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, w.id as WaterbodyId, w.name as WaterbodyName, l.id as LocationId, 
    l.name as LocationName

from FishScales_Detail d 
join FishScales_Header h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId
join locations l on l.id = a.locationid
join waterbodies w on w.id = l.waterbodyid



go
CREATE VIEW [dbo].[FishScales_Detail_VW]
AS
SELECT        *
FROM            dbo.FishScales_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.FishScales_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId)))
go

CREATE VIEW [dbo].[FishScales_Header_VW]
AS
SELECT        *
FROM            dbo.FishScales_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.FishScales_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)));
");
        }


        public override void Down()
        {
            Sql(@"
drop view FishScales_vw
go
drop view FishScales_Detail_VW
go
drop view FishScales_Header_VW
go
");
        }
    }
}