namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WaterTemp_AddDischargeField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterTemp_Detail", "Discharge", c => c.Double());

            //add the discharge field to the master fields
            Sql(@"
                INSERT INTO dbo.Fields(FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule]) 
                VALUES
                (2,'Discharge','Volumetric flow rate of water that is transported through a given location. It includes any suspended solids, dissolved chemicals, or biologic material in addition to the water itself',
                    'cfs','[0,5000]','float',null,'Discharge','number',null)
           ");

            //update the watertemp detail view
            Sql(@"
                ALTER view dbo.WaterTemp_Detail_VW
                as
                SELECT        d.Id, d.ReadingDateTime, d.GMTReadingDateTime, d.AirTemperature, d.AirTemperatureF, d.WaterTemperature, d.WaterTemperatureF, d.WaterLevel, d.TempAToD, d.Discharge,
                                         d.BatteryVolts, d.Conductivity, d.RowId, d.RowStatusId, d.ActivityId, d.ByUserId, d.QAStatusId, d.EffDt, d.PSI, d.AbsolutePressure, d.Depth, 
                qa.Name AS RowQAStatus
                FROM            dbo.WaterTemp_Detail AS d INNER JOIN
                                         dbo.QAStatus AS qa ON d.QAStatusId = qa.Id
                WHERE        (d.EffDt =
                                             (SELECT        MAX(EffDt) AS MaxEffDt
                                               FROM            dbo.WaterTemp_Detail AS dd
                                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (d.RowStatusId = 0)
            ");


            //update the main watertemp view
            Sql(@"
                ALTER VIEW dbo.WaterTemp_VW
                AS
                SELECT        h.Id AS WaterTemp_Header_Id, h.Technicians, h.Comments, h.CollectionType, h.FieldActivityType, h.StaticWaterLevel, h.WeatherConditions, h.SamplePeriod, h.SampleTempUnit, h.DeployTime, h.ByUserId, h.EffDt, 
                                         d.Id AS WaterTemp_Detail_Id, d.ReadingDateTime, d.GMTReadingDateTime, d.WaterTemperature, d.WaterTemperatureF, d.AirTemperature, d.AirTemperatureF, d.WaterLevel, d.TempAToD, d.Discharge, d.BatteryVolts, d.Conductivity, 
                                         d.RowId, d.RowStatusId, d.ByUserId AS WaterTemp_Detail_ByUserId, d.QAStatusId AS RowQAStatusId, d.EffDt AS WaterTemp_Detail_EffDt, d.PSI, d.AbsolutePressure, d.Depth, aq.QAStatusId AS ActivityQAStatusId, 
                                         aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId, a.SourceId, a.LocationId, a.UserId AS Activity_UserId, a.ActivityTypeId, a.CreateDate, a.Id AS ActivityId, 
                                         aq.QAStatusName AS ActivityQAStatusName, a.InstrumentId, a.ActivityDate, L.Label, w.Name, L.WaterBodyId, L.LocationTypeId, CASE WHEN d .QAStatusId > 1 THEN '' ELSE d .WaterTemperature END AS CorrectTemp, 
                                         L.OtherAgencyId, qas.Name AS RowQAStatusName
                FROM            dbo.QAStatus AS qas INNER JOIN
                                         dbo.WaterTemp_Detail_VW AS d ON qas.Id = d.QAStatusId RIGHT OUTER JOIN
                                         dbo.Activities AS a INNER JOIN
                                         dbo.WaterTemp_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                                         dbo.Locations AS L ON a.LocationId = L.Id INNER JOIN
                                         dbo.WaterBodies AS w ON L.WaterBodyId = w.Id ON d.ActivityId = h.ActivityId
            ");


        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterTemp_Detail", "Discharge");
        }
    }
}
