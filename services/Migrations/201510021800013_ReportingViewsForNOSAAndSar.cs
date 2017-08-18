namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReportingViewsForNOSAAndSar : DbMigration
    {
        public override void Up()
        {

            Sql(@"

    CREATE VIEW [dbo].[StreamNet_NOSA_detail_VW]
    AS
    SELECT        *
    FROM            dbo.StreamNet_NOSA_detail AS d
    WHERE        (EffDt =
                                 (SELECT        MAX(EffDt) AS MaxEffDt
                                   FROM            dbo.StreamNet_NOSA_detail AS dd
                                   WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId)))
    go

    CREATE VIEW [dbo].[StreamNet_NOSA_Header_VW]
    AS
    SELECT        *
    FROM            dbo.StreamNet_NOSA_Header AS h
    WHERE        (EffDt =
                                 (SELECT        MAX(EffDt) AS MaxEffDt
                                   FROM            dbo.StreamNet_NOSA_Header AS hh
                                   WHERE        (ActivityId = h.ActivityId)));
    go


    create view StreamNet_NOSA_vw as
    select
    d.Id, d.CommonName, d.Run, d.PopFit, d.WaterBody, d.SpawningYear, d.TRTmethod, d.ContactAgency, d.MethodNumber, d.NOSAIJ, d.NOSAEJ, 
    d.Comment, d.NullRecord, d.DataStatus, d.ContactPersonFirst, d.ContactPersonLast, d.ContactPhone, d.ContactEmail, d.RowId, d.RowStatusId, 
    d.ByUserId, d.QAStatusId, d.EffDt, d.ShadowId, d.Age10Prop, d.Age10PropLowerLimit, d.Age10PropUpperLimit, d.Age11PlusProp, 
    d.Age11PlusPropLowerLimit, d.Age11PlusPropUpperLimit, d.Age2Prop, d.Age2PropLowerLimit, d.Age2PropUpperLimit, d.Age3Prop, d.Age3PropLowerLimit, 
    d.Age3PropUpperLimit, d.Age4Prop, d.Age4PropLowerLimit, d.Age4PropUpperLimit, d.Age5Prop, d.Age5PropLowerLimit, d.Age5PropUpperLimit, 
    d.Age6Prop, d.Age6PropLowerLimit, d.Age6PropUpperLimit, d.Age7Prop, d.Age7PropLowerLimit, d.Age7PropUpperLimit, d.Age8Prop, d.Age8PropLowerLimit, 
    d.Age8PropUpperLimit, d.Age9Prop, d.Age9PropLowerLimit, d.Age9PropUpperLimit, d.AgePropAlpha, d.CBFWApopName, d.Comments, d.CommonPopName, 
    d.CompilerRecordID, d.DataEntry, d.DataEntryNotes, d.ESU_DPS, d.HOSJF, d.IndicatorLocation, d.LastUpdated, d.MajorPopGroup, d.MeasureLocation, 
    d.MetaComments, d.MethodAdjustments, d.MetricLocation, d.NOBroodStockRemoved, d.NOSAEJAlpha, d.NOSAEJLowerLimit, d.NOSAEJUpperLimit, d.NOSAIJAlpha, 
    d.NOSAIJLowerLimit, d.NOSAIJUpperLimit, d.NOSJF, d.NOSJFAlpha, d.NOSJFLowerLimit, d.NOSJFUpperLimit, d.PopFitNotes, d.PopID, d.ProtMethDocumentation, 
    d.ProtMethName, d.ProtMethURL, d.Publish, d.RecoveryDomain, d.RefID, d.SubmitAgency, d.TSAEJ, d.TSAEJAlpha, d.TSAEJLowerLimit, d.TSAEJUpperLimit, 
    d.TSAIJ, d.TSAIJAlpha, d.TSAIJLowerLimit, d.TSAIJUpperLimit, d.UpdDate, d.pHOSej, d.pHOSejAlpha, d.pHOSejLowerLimit, d.pHOSejUpperLimit, d.pHOSij, 
    d.pHOSijAlpha, d.pHOSijLowerLimit, d.pHOSijUpperLimit,

    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate

from StreamNet_NOSA_detail_VW d 
join StreamNet_NOSA_Header_VW h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId



go
CREATE VIEW [dbo].[StreamNet_SAR_detail_VW]
AS
SELECT        *
FROM            dbo.StreamNet_SAR_detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.StreamNet_SAR_detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId)))
go

CREATE VIEW [dbo].[StreamNet_SAR_Header_VW]
AS
SELECT        *
FROM            dbo.StreamNet_SAR_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.StreamNet_SAR_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)));

go

    create view StreamNet_SAR_vw as
    select
    d.Id, d.CommonName, d.Run, d.PopFit, d.PopFitNotes, d.PopAggregation, d.SmoltLocation, d.AdultLocation, d.SARtype, d.OutmigrationYear, 
    d.TRTmethod, d.ContactAgency, d.MethodNumber, d.SAR, d.RearingType, d.Comments, d.NullRecord, d.DataStatus, d.ContactPersonFirst, 
    d.ContactPersonLast, d.ContactPhone, d.ContactEmail, d.RowId, d.RowStatusId, d.ByUserId, d.QAStatusId, d.EffDt, 
    d.ShadowId, d.BroodStockRemoved, d.CBFWApopName, d.CommonPopName, d.CompilerRecordID, d.DataEntry, d.DataEntryNotes, d.ESU_DPS, 
    d.HarvestAdj, d.IndicatorLocation, d.LastUpdated, d.MainstemHarvest, d.MajorPopGroup, d.MeasureLocation, d.MetaComments, d.MethodAdjustments, 
    d.MetricLocation, d.OceanHarvest, d.PopID, d.ProtMethDocumentation, d.ProtMethName, d.ProtMethURL, d.Publish, d.RecoveryDomain, d.RefID, 
    d.ReturnDef, d.ReturnsMissing, d.ReturnsMissingExplanation, d.SARAlpha, d.SARLowerLimit, d.SARUpperLimit, d.ScopeOfInference, d.SmoltLocPTcode, 
    d.SubmitAgency, d.TAR, d.TARAlpha, d.TARLowerLimit, d.TARUpperLimit, d.TSO, d.TSOAlpha, d.TSOLowerLimit, d.TSOUpperLimit, d.TribHarvest, 
    d.UpdDate,

    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate

from StreamNet_SAR_detail_VW d 
join StreamNet_SAR_Header_VW h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId


go
drop view StreamNet_RperS_Header_VW
go
drop view StreamNet_RperS_Detail_vw
go
drop view StreamNet_RperS_vw
go


CREATE VIEW [dbo].[StreamNet_RperS_detail_VW]
AS
SELECT        *
FROM            dbo.StreamNet_RperS_detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.StreamNet_RperS_detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId)))
go

CREATE VIEW [dbo].[StreamNet_RperS_Header_VW]
AS
SELECT        *
FROM            dbo.StreamNet_RperS_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.StreamNet_RperS_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)));
go

    create view StreamNet_RperS_vw as
    select
    d.Id, d.CommonName, d.Run, d.PopFit, 
    d.SpawnerLocation, d.RecruitLocation, d.BroodYear, d.RperStype, d.TRTmethod, d.ContactAgency, d.MethodNumber, d.RperS, d.Comments, 
    d.NullRecord, d.DataStatus, d.ContactPersonFirst, d.ContactPersonLast, d.ContactPhone, d.ContactEmail, d.Age10Adults, d.Age11PlusAdults, 
    d.Age1Juvs, d.Age2Adults, d.Age2Juvs, d.Age3Adults, d.Age3Juvs, d.Age4Adults, d.Age4PlusJuvs, d.Age5Adults, d.Age6Adults, d.Age7Adults, 
    d.Age8Adults, d.Age9Adults, d.CBFWApopName, d.CommonPopName, d.CompilerRecordID, d.DataEntry, d.DataEntryNotes, d.ESU_DPS, d.HarvestAdj, 
    d.HatcherySpawners, d.HatcherySpawnersAlpha, d.HatcherySpawnersLowerLimit, d.HatcherySpawnersUpperLimit, d.IndicatorLocation, d.LastUpdated, 
    d.MainstemHarvest, d.MajorPopGroup, d.MeasureLocation, d.MetaComments, d.MethodAdjustments, d.MetricLocation, d.NOBroodStockRemoved, 
    d.OceanHarvest, d.PopFitNotes, d.PopID, d.ProtMethDocumentation, d.ProtMethName, d.ProtMethURL, d.Publish, d.RecoveryDomain, d.Recruits, 
    d.RecruitsAlpha, d.RecruitsLowerLimit, d.RecruitsMissing, d.RecruitsMissingExplanation, d.RecruitsUpperLimit, d.RefID, d.RperSAlpha,
    d.RperSLowerLimit, d.RperSUpperLimit, d.SubmitAgency, d.TotalSpawners, d.TotalSpawnersAlpha, d.TotalSpawnersLowerLimit, d.TotalSpawnersUpperLimit,
    d.TribHarvest, d.UpdDate, d.YOY, d.ShadowId,

    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate 

from StreamNet_RperS_detail_vw d 
join StreamNet_RperS_Header_vw h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId

");
        }


        public override void Down()
        {
            Sql(@"
drop view StreamNet_NOSA_Detail_VW
go
drop view StreamNet_NOSA_Header_VW
go
drop view StreamNet_NOSA_vw
go


drop view StreamNet_SAR_VW
go
drop view StreamNet_SAR_Header_VW
go
drop view StreamNet_SAR_Detail_vw
go

drop view StreamNet_RperS_vw
go
drop view StreamNet_RperS_Header_VW
go
drop view StreamNet_RperS_Detail_vw
go

    create view StreamNet_RperS_vw as
    select
    d.Id, d.CommonName, d.Run, d.PopFit, 
    d.SpawnerLocation, d.RecruitLocation, d.BroodYear, d.RperStype, d.TRTmethod, d.ContactAgency, d.MethodNumber, d.RperS, d.Comments, 
    d.NullRecord, d.DataStatus, d.ContactPersonFirst, d.ContactPersonLast, d.ContactPhone, d.ContactEmail, d.Age10Adults, d.Age11PlusAdults, 
    d.Age1Juvs, d.Age2Adults, d.Age2Juvs, d.Age3Adults, d.Age3Juvs, d.Age4Adults, d.Age4PlusJuvs, d.Age5Adults, d.Age6Adults, d.Age7Adults, 
    d.Age8Adults, d.Age9Adults, d.CBFWApopName, d.CommonPopName, d.CompilerRecordID, d.DataEntry, d.DataEntryNotes, d.ESU_DPS, d.HarvestAdj, 
    d.HatcherySpawners, d.HatcherySpawnersAlpha, d.HatcherySpawnersLowerLimit, d.HatcherySpawnersUpperLimit, d.IndicatorLocation, d.LastUpdated, 
    d.MainstemHarvest, d.MajorPopGroup, d.MeasureLocation, d.MetaComments, d.MethodAdjustments, d.MetricLocation, d.NOBroodStockRemoved, 
    d.OceanHarvest, d.PopFitNotes, d.PopID, d.ProtMethDocumentation, d.ProtMethName, d.ProtMethURL, d.Publish, d.RecoveryDomain, d.Recruits, 
    d.RecruitsAlpha, d.RecruitsLowerLimit, d.RecruitsMissing, d.RecruitsMissingExplanation, d.RecruitsUpperLimit, d.RefID, d.RperSAlpha,
    d.RperSLowerLimit, d.RperSUpperLimit, d.SubmitAgency, d.TotalSpawners, d.TotalSpawnersAlpha, d.TotalSpawnersLowerLimit, d.TotalSpawnersUpperLimit,
    d.TribHarvest, d.UpdDate, d.YOY, d.ShadowId,

    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, w.id as WaterbodyId, w.name as WaterbodyName, l.id as LocationId, 
    l.name as LocationName

from StreamNet_RperS_detail d 
join StreamNet_RperS_Header h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId
join locations l on l.id = a.locationid
join waterbodies w on w.id = l.waterbodyid

go
CREATE VIEW [dbo].[StreamNet_RperS_detail_VW]
AS
SELECT        *
FROM            dbo.StreamNet_RperS_detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.StreamNet_RperS_detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId)))
go

CREATE VIEW [dbo].[StreamNet_RperS_Header_VW]
AS
SELECT        *
FROM            dbo.StreamNet_RperS_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.StreamNet_RperS_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)));
go


");
        }
    }
}
