--declare the vars we will need
declare @pubdetaileffdt datetime = null;
declare @pubheadereffdt datetime = null;
declare @pubactivityqaeffdt datetime = null;

--get the date of the last changes to the headers/details
select @pubheadereffdt = max(detaileffdt) from CDMS_Rpt.dbo.Public_WaterTemp;
select @pubheadereffdt = max(headereffdt) from CDMS_Rpt.dbo.Public_WaterTemp;
select @pubactivityqaeffdt = max(ActivityQAStatusEffDt) from CDMS_Rpt.dbo.Public_WaterTemp;

SELECT * FROM CDMS_Rpt.dbo.Public_WaterTemp
	WHERE 
	HeaderId in 
		(SELECT h.Id FROM CDMS_PROD.dbo.WaterTemp_Header_VW as h where h.EffDt > @pubdetaileffdt)
	OR
	DetailId in 
		(SELECT d.Id FROM CDMS_PROD.dbo.WaterTemp_Detail_VW as d where d.EffDt > @pubdetaileffdt)
	OR
	ActivityId in
		(Select a.Id FROM CDMS_PROD.dbo.ActivityQAs_VW as a where a.EffDt > @pubactivityqaeffdt);

SELECT
a.DatasetId, 
a.SourceId, 
a.LocationId, 
a.UserId, 
a.ActivityTypeId, 
a.CreateDate, 
a.Id AS ActivityId,
a.InstrumentId, 
a.ActivityDate,
a.AccuracyCheckId,
a.PostAccuracyCheckId,
a.Timezone,
a.Description,

aq.QAStatusId as ActivityQAStatusId,
aq.UserId as ActivityQAStatusUserId,
aq.EffDt as ActivityQAStatusEffDt,
aq.Comments as ActivityQAStatusComments,
aq.QAStatusName as ActivityQAStatusName,

h.Id AS HeaderId, 
h.Technicians, 
h.Comments, 
h.CollectionType, 
h.DepthToWater, 
h.FieldActivityType, 
h.StaticWaterLevel, 
h.WeatherConditions, 
h.SamplePeriod, 
h.SampleTempUnit, 
h.DeployTime, 
h.ByUserId as HeaderByUserId,
h.EffDt as HeaderEffDt,

d.Id AS DetailId, 
d.ReadingDateTime, 
d.GMTReadingDateTime, 
d.WaterTemperature, 
d.WaterTemperatureF, 
d.AirTemperature, 
d.AirTemperatureF, 
d.WaterLevel, 
d.TempAToD, 
d.BatteryVolts, 
d.Conductivity, 
d.RowId, 
d.RowStatusId, 
d.ByUserId AS DetailByUserId, 
d.QAStatusId, 
d.EffDt AS DetailEffDt, 
d.PSI, 
d.AbsolutePressure,
year(d.ReadingDateTime) as WaterYearCalculated
                         
						 
FROM          			 CDMS_TEST.dbo.Activities AS a INNER JOIN
						 CDMS_TEST.dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         CDMS_TEST.dbo.WaterTemp_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         CDMS_TEST.dbo.WaterTemp_Detail_VW AS d ON d.ActivityId = a.Id

WHERE (h.EffDt > @pubheadereffdt OR d.EffDt > @pubdetaileffdt OR aq.EffDt > @pubactivityqaeffdt)
      AND
	  aq.QAStatusId = 5; --approved activities only.

