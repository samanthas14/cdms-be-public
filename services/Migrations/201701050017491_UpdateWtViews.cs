namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWtViews : DbMigration
    {
        public override void Up()
        {
            Sql(@"
drop view [dbo].[WaterTemp_Detail_VW]
GO
CREATE VIEW [dbo].[WaterTemp_Detail_VW]
AS
SELECT        Id, ReadingDateTime, GMTReadingDateTime, AirTemperature, AirTemperatureF, WaterTemperature, WaterTemperatureF, WaterLevel, TempAToD, BatteryVolts, 
                         Conductivity, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, PSI, AbsolutePressure
FROM            dbo.WaterTemp_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.WaterTemp_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
GO

drop view [dbo].[WaterTemp_VW]
GO
CREATE VIEW [dbo].[WaterTemp_VW]
AS
SELECT        h.Id AS WaterTemp_Header_Id, h.Technicians, h.Comments, h.CollectionType, h.DepthToWater, h.FieldActivityType, h.StaticWaterLevel, h.WeatherConditions, 
                         h.SamplePeriod, h.SampleTempUnit, h.DeployTime, h.ByUserId, h.EffDt, d.Id AS WaterTemp_Detail_Id, d.ReadingDateTime, d.GMTReadingDateTime, 
                         d.WaterTemperature, d.WaterTemperatureF, d.AirTemperature, d.AirTemperatureF, d.WaterLevel, d.TempAToD, d.BatteryVolts, d.Conductivity, d.RowId, 
                         d.RowStatusId, d.ByUserId AS WaterTemp_Detail_ByUserId, d.QAStatusId, d.EffDt AS WaterTemp_Detail_EffDt, d.PSI, d.AbsolutePressure, 
                         aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, 
                         a.DatasetId, a.SourceId, a.LocationId, a.UserId AS Activity_UserId, a.ActivityTypeId, 
                         a.CreateDate, a.Id AS ActivityId, aq.QAStatusName, a.InstrumentId, a.ActivityDate, 
                         L.Label, w.Name, L.WaterBodyId, L.LocationTypeId, 
                         CASE WHEN d .QAStatusId > 1 THEN '' ELSE d .WaterTemperature END AS CorrectTemp
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS L ON a.LocationId = L.Id INNER JOIN
                         dbo.WaterBodies AS w ON L.WaterBodyId = w.Id
            ");
        }
        
        public override void Down()
        {
            Sql(@"
drop view [dbo].[WaterTemp_Detail_VW]
GO
CREATE VIEW [dbo].[WaterTemp_Detail_VW]
AS
SELECT        Id, ReadingDateTime, GMTReadingDateTime, AirTemperature, AirTemperatureF, WaterTemperature, WaterTemperatureF, WaterLevel, TempAToD, BatteryVolts, 
                         Conductivity, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt
FROM            dbo.WaterTemp_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.WaterTemp_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
GO

drop view [dbo].[WaterTemp_VW]
GO
CREATE VIEW [dbo].[WaterTemp_VW]
AS
SELECT        h.Id AS WaterTemp_Header_Id, h.Technicians, h.Comments, h.CollectionType, h.DepthToWater, h.PSI, h.FieldActivityType, h.StaticWaterLevel, h.WeatherConditions, 
                         h.SamplePeriod, h.SampleTempUnit, h.DeployTime, h.ByUserId, h.EffDt, d.Id AS WaterTemp_Detail_Id, d.ReadingDateTime, d.GMTReadingDateTime, 
                         d.WaterTemperature, d.WaterTemperatureF, d.AirTemperature, d.AirTemperatureF, d.WaterLevel, d.TempAToD, d.BatteryVolts, d.Conductivity, d.RowId, 
                         d.RowStatusId, d.ByUserId AS WaterTemp_Detail_ByUserId, d.QAStatusId, d.EffDt AS WaterTemp_Detail_EffDt, aq.QAStatusId AS ActivityQAStatusId, 
                         aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId, a.SourceId, a.LocationId, a.UserId AS Activity_UserId, a.ActivityTypeId, 
                         a.CreateDate, a.Id AS ActivityId, aq.QAStatusName, a.InstrumentId, a.ActivityDate, L.Label, w.Name, L.WaterBodyId, L.LocationTypeId, 
                         CASE WHEN d .QAStatusId > 1 THEN '' ELSE d .WaterTemperature END AS CorrectTemp
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterTemp_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.WaterTemp_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS L ON a.LocationId = L.Id INNER JOIN
                         dbo.WaterBodies AS w ON L.WaterBodyId = w.Id
            ");
        }
    }
}
