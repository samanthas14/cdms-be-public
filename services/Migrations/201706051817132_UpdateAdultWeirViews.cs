namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAdultWeirViews : DbMigration
    {
        public override void Up()
        {
            Sql(@"
drop view dbo.[AdultWeir_Header_VW]
go
create view dbo.[AdultWeir_Header_VW]
as
SELECT        Id, AirTemperature, AirTemperatureF, WaterTemperature, WaterTemperatureF, TimeStart, TimeEnd, Technicians, WaterFlow, Comments, ActivityId, ByUserId, EffDt, 
                         CollectionType, FieldSheetFile
FROM            dbo.AdultWeir_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.AdultWeir_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
go

drop view dbo.[AdultWeir_vw]
go
create view dbo.[AdultWeir_vw]
as
SELECT        TOP (100) PERCENT h.CollectionType, dbo.Locations.Label AS LocationLabel, CONVERT(VARCHAR(10), a.ActivityDate, 101) AS WWVideoDate, a.ActivityDate, 
                         h.AirTemperature, h.AirTemperatureF, h.WaterTemperature, h.WaterTemperatureF, h.TimeStart, h.TimeEnd, h.Technicians, h.WaterFlow, h.Comments, 
                         h.FieldSheetFile,
                         d.RunYear, d.FishNumber, d.Species, d.Sex, d.Origin, d.LifeStage, d.FinClip, CASE WHEN d .Mark IS NULL AND d .Species IS NOT NULL 
                         THEN 'NONE' ELSE d .Mark END AS Mark, d.TotalFishRepresented, d.ForkLength, d.TotalLength, d.Weight, d.Disposition, d.Recapture, d.GeneticSampleId, 
                         d.OtolithNumber, d.Tag, d.PITTagId, d.RadioTagId, d.ScaleId, d.SnoutId, d.OtherTagId, d.FishComments, d.Solution, d.SolutionDosage, d.OtolithGenetics, 
                         d.AgePITTag, d.AgeCWT, d.AgeScale, d.AgeLength, d.HatcheryType, d.Stock, d.TransportFrom, d.TransportTankUnit, d.ReleaseSite, d.Ripeness, d.PassageLocation, 
                         d.PercentSpawned, d.Girth, d.TrapLocation, d.BroodProgram, d.PassageTime, 
                         aq.QAStatusName, aq.Comments AS ActivityQAComments, 
                         h.ByUserId AS AdultWeir_Header_ByUserId, 
                         a.Id AS ActivityId, a.UserId AS Activity_UserId, a.LocationId, a.ActivityTypeId, a.CreateDate, 
                         h.Id AS AdultWeir_Header_Id, 
                         aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, 
                         h.EffDt AS AdultWeir_Header_EffDt, d.Id AS AdultWeir_Detail_Id, 
                         a.DatasetId, d.RowId, d.RowStatusId, d.ByUserId AS AdultWeir_Detail_ByUserId, d.QAStatusId AS RowQAStatusId, d.QAStatusId, d.EffDt AS AdultWeir_Detail_EffDt, 
                         a.SourceId
FROM            dbo.Activities AS a INNER JOIN
                         dbo.AdultWeir_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.AdultWeir_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations ON a.LocationId = dbo.Locations.Id
go
            ");
        }
        
        public override void Down()
        {
            Sql(@"
drop view dbo.[AdultWeir_Header_VW]
go
create view dbo.[AdultWeir_Header_VW]
as
SELECT        Id, AirTemperature, AirTemperatureF, WaterTemperature, WaterTemperatureF, TimeStart, TimeEnd, Technicians, WaterFlow, Comments, ActivityId, ByUserId, EffDt, 
                         CollectionType
FROM            dbo.AdultWeir_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.AdultWeir_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
go

drop view dbo.[AdultWeir_vw]
go
create view dbo.[AdultWeir_vw]
as
SELECT        TOP (100) PERCENT h.CollectionType, dbo.Locations.Label AS LocationLabel, CONVERT(VARCHAR(10), a.ActivityDate, 101) AS WWVideoDate, a.ActivityDate, 
                         h.AirTemperature, h.AirTemperatureF, h.WaterTemperature, h.WaterTemperatureF, h.TimeStart, h.TimeEnd, h.Technicians, h.WaterFlow, h.Comments, d.RunYear, 
                         d.FishNumber, d.Species, d.Sex, d.Origin, d.LifeStage, d.FinClip, CASE WHEN d .Mark IS NULL AND d .Species IS NOT NULL 
                         THEN 'NONE' ELSE d .Mark END AS Mark, d.TotalFishRepresented, d.ForkLength, d.TotalLength, d.Weight, d.Disposition, d.Recapture, d.GeneticSampleId, 
                         d.OtolithNumber, d.Tag, d.PITTagId, d.RadioTagId, d.ScaleId, d.SnoutId, d.OtherTagId, d.FishComments, d.Solution, d.SolutionDosage, d.OtolithGenetics, 
                         d.AgePITTag, d.AgeCWT, d.AgeScale, d.AgeLength, d.HatcheryType, d.Stock, d.TransportFrom, d.TransportTankUnit, d.ReleaseSite, d.Ripeness, d.PassageLocation, 
                         d.PercentSpawned, d.Girth, d.TrapLocation, d.BroodProgram, d.PassageTime, aq.QAStatusName, aq.Comments AS ActivityQAComments, 
                         h.ByUserId AS AdultWeir_Header_ByUserId, a.Id AS ActivityId, a.UserId AS Activity_UserId, a.LocationId, a.ActivityTypeId, a.CreateDate, 
                         h.Id AS AdultWeir_Header_Id, aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, h.EffDt AS AdultWeir_Header_EffDt, d.Id AS AdultWeir_Detail_Id, 
                         a.DatasetId, d.RowId, d.RowStatusId, d.ByUserId AS AdultWeir_Detail_ByUserId, d.QAStatusId AS RowQAStatusId, d.QAStatusId, d.EffDt AS AdultWeir_Detail_EffDt, 
                         a.SourceId
FROM            dbo.Activities AS a INNER JOIN
                         dbo.AdultWeir_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.AdultWeir_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations ON a.LocationId = dbo.Locations.Id
go
            ");
        }
    }
}
