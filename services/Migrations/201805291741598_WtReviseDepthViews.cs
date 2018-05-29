namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WtReviseDepthViews : DbMigration
    {
        public override void Up()
        {
            Sql(@"
drop view dbo.WaterTemp_Detail_VW
go
create view dbo.WaterTemp_Detail_VW
as
SELECT        d.Id, d.ReadingDateTime, d.GMTReadingDateTime, d.AirTemperature, d.AirTemperatureF, d.WaterTemperature, d.WaterTemperatureF, d.WaterLevel, d.TempAToD, 
                         d.BatteryVolts, d.Conductivity, d.RowId, d.RowStatusId, d.ActivityId, d.ByUserId, d.QAStatusId, d.EffDt, d.PSI, d.AbsolutePressure, d.Depth, 
qa.Name AS RowQAStatus
FROM            dbo.WaterTemp_Detail AS d INNER JOIN
                         dbo.QAStatus AS qa ON d.QAStatusId = qa.Id
WHERE        (d.EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.WaterTemp_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (d.RowStatusId = 0)
go


drop view dbo.WaterTemp_Header_VW
go
create view dbo.WaterTemp_Header_VW
as
SELECT        Id, Technicians, Comments, CollectionType, PSI, StaticWaterLevel, WeatherConditions, FieldActivityType, SamplePeriod, SampleTempUnit, 
                         DeployTime, ActivityId, ByUserId, EffDt
FROM            dbo.WaterTemp_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.WaterTemp_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
go


drop view dbo.WaterTemp_VW
go
create view dbo.WaterTemp_VW
as
SELECT        h.Id AS WaterTemp_Header_Id, h.Technicians, h.Comments, h.CollectionType, h.FieldActivityType, h.StaticWaterLevel, h.WeatherConditions, 
                         h.SamplePeriod, h.SampleTempUnit, h.DeployTime, h.ByUserId, h.EffDt, d.Id AS WaterTemp_Detail_Id, d.ReadingDateTime, d.GMTReadingDateTime, 
                         d.WaterTemperature, d.WaterTemperatureF, d.AirTemperature, d.AirTemperatureF, d.WaterLevel, d.TempAToD, d.BatteryVolts, d.Conductivity, d.RowId, 
                         d.RowStatusId, d.ByUserId AS WaterTemp_Detail_ByUserId, d.QAStatusId, d.EffDt AS WaterTemp_Detail_EffDt, d.PSI, d.AbsolutePressure, d.Depth,
                         aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId, a.SourceId, a.LocationId, 
                         a.UserId AS Activity_UserId, a.ActivityTypeId, a.CreateDate, a.Id AS ActivityId, aq.QAStatusName, a.InstrumentId, a.ActivityDate, L.Label, w.Name, L.WaterBodyId, 
                         L.LocationTypeId, CASE WHEN d .QAStatusId > 1 THEN '' ELSE d .WaterTemperature END AS CorrectTemp, L.OtherAgencyId
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS L ON a.LocationId = L.Id INNER JOIN
                         dbo.WaterBodies AS w ON L.WaterBodyId = w.Id
go
            ");
        }

        public override void Down()
        {
            Sql(@"
drop view dbo.WaterTemp_Detail_VW
go
create view dbo.WaterTemp_Detail_VW
as
SELECT        d.Id, d.ReadingDateTime, d.GMTReadingDateTime, d.AirTemperature, d.AirTemperatureF, d.WaterTemperature, d.WaterTemperatureF, d.WaterLevel, d.TempAToD, 
                         d.BatteryVolts, d.Conductivity, d.RowId, d.RowStatusId, d.ActivityId, d.ByUserId, d.QAStatusId, d.EffDt, d.PSI, d.AbsolutePressure, qa.Name AS RowQAStatus
FROM            dbo.WaterTemp_Detail AS d INNER JOIN
                         dbo.QAStatus AS qa ON d.QAStatusId = qa.Id
WHERE        (d.EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.WaterTemp_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (d.RowStatusId = 0)
go


drop view dbo.WaterTemp_Header_VW
go
create view dbo.WaterTemp_Header_VW
as
SELECT        Id, Technicians, Comments, CollectionType, DepthToWater, PSI, StaticWaterLevel, WeatherConditions, FieldActivityType, SamplePeriod, SampleTempUnit, 
                         DeployTime, ActivityId, ByUserId, EffDt
FROM            dbo.WaterTemp_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.WaterTemp_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
go


drop view dbo.WaterTemp_VW
go
create view dbo.WaterTemp_VW
as
SELECT        h.Id AS WaterTemp_Header_Id, h.Technicians, h.Comments, h.CollectionType, h.DepthToWater, h.FieldActivityType, h.StaticWaterLevel, h.WeatherConditions, 
                         h.SamplePeriod, h.SampleTempUnit, h.DeployTime, h.ByUserId, h.EffDt, d.Id AS WaterTemp_Detail_Id, d.ReadingDateTime, d.GMTReadingDateTime, 
                         d.WaterTemperature, d.WaterTemperatureF, d.AirTemperature, d.AirTemperatureF, d.WaterLevel, d.TempAToD, d.BatteryVolts, d.Conductivity, d.RowId, 
                         d.RowStatusId, d.ByUserId AS WaterTemp_Detail_ByUserId, d.QAStatusId, d.EffDt AS WaterTemp_Detail_EffDt, d.PSI, d.AbsolutePressure, 
                         aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId, a.SourceId, a.LocationId, 
                         a.UserId AS Activity_UserId, a.ActivityTypeId, a.CreateDate, a.Id AS ActivityId, aq.QAStatusName, a.InstrumentId, a.ActivityDate, L.Label, w.Name, L.WaterBodyId, 
                         L.LocationTypeId, CASE WHEN d .QAStatusId > 1 THEN '' ELSE d .WaterTemperature END AS CorrectTemp, L.OtherAgencyId
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS L ON a.LocationId = L.Id INNER JOIN
                         dbo.WaterBodies AS w ON L.WaterBodyId = w.Id
go
            ");
        }
    }
}
