-- this is just for CTUIR CDMS - our water temp rules were broken
update fields set [Rule] = '{"OnChange":"activities.errors = undefined; removeRowErrorsBeforeRecheck(); checkForDuplicates();"}' where id = 1065;
update datasetfields set [Rule] = '{"OnChange":"activities.errors = undefined; removeRowErrorsBeforeRecheck(); checkForDuplicates();"}'
where id in (1241, 4468, 4518, 4541, 4615);
go
update datasetfields set [Rule] = null where [rule] like '%activities.errors%' and DbColumnName = 'ReadingDateTime';
update fields set [Rule] = null where [rule] like '%activities.errors%' and DbColumnName = 'ReadingDateTime';
go

-- this adds a field to our creel dataset for fisherman to work with the datasource lookup
insert into datasetfields (DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, SourceId, OrderIndex, ControlType,InstrumentId)
values (1230, 1507,2,getdate(),'Fisherman','FishermanId',1,5,'select',null);

-- update the datasource column of our fishermanid field - this will now provide the possibleValues list
update fields set DataSource = 'select Id, fullname as Label from Fishermen' where id = 1507;

go

ALTER VIEW dbo.WaterTemp_VW
    AS
    SELECT        h.Id AS WaterTemp_Header_Id, h.Technicians, h.Comments, h.CollectionType, h.FieldActivityType, h.StaticWaterLevel, h.WeatherConditions, h.SamplePeriod, h.SampleTempUnit, h.DeployTime, h.ByUserId, h.EffDt, 
                                d.Id AS WaterTemp_Detail_Id, d.ReadingDateTime, d.GMTReadingDateTime, d.WaterTemperature, d.WaterTemperatureF, d.AirTemperature, d.AirTemperatureF, d.WaterLevel, d.TempAToD, d.Discharge, d.BatteryVolts, d.Conductivity, 
                                d.RowId, d.RowStatusId, d.ByUserId AS WaterTemp_Detail_ByUserId, d.QAStatusId AS RowQAStatusId, d.EffDt AS WaterTemp_Detail_EffDt, d.PSI, d.AbsolutePressure, d.Depth, aq.QAStatusId AS ActivityQAStatusId, 
                                aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.Description, a.DatasetId, a.SourceId, a.LocationId, a.UserId AS Activity_UserId, a.ActivityTypeId, a.CreateDate, a.Id AS ActivityId, 
                                aq.QAStatusName AS ActivityQAStatusName, a.InstrumentId, a.ActivityDate, L.Label, w.Name, L.WaterBodyId, L.LocationTypeId, CASE WHEN d .QAStatusId > 1 THEN '' ELSE d .WaterTemperature END AS CorrectTemp, 
                                L.OtherAgencyId, qas.Name AS RowQAStatusName
    FROM            dbo.QAStatus AS qas INNER JOIN
                                dbo.WaterTemp_Detail_VW AS d ON qas.Id = d.QAStatusId RIGHT OUTER JOIN
                                dbo.Activities AS a INNER JOIN
                                dbo.WaterTemp_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                                dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                                dbo.Locations AS L ON a.LocationId = L.Id INNER JOIN
                                dbo.WaterBodies AS w ON L.WaterBodyId = w.Id ON d.ActivityId = h.ActivityId

go

