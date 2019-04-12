-- First set the FieldRoleId to Detail for Row QA Status
update dbo.Fields
set FieldRoleId = 2
where [Name] = 'Row QA Status' and  [Description] = 'Activity row QA status'

-- Next, add a record to DatasetFields for Row QA
declare @DatasetId as int
declare @FieldId as int

set @DatasetId = (select Id from dbo.Datasets where [Name] = 'WRP-Water Chemistry')
set @FieldId = (select Id from dbo.Fields where [Name] = 'Row QA Status' and DbColumnName = 'QAStatusId')

insert into dbo.DatasetFields(DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, Validation, SourceId, InstrumentId, OrderIndex, ControlType, [Rule])
values(@DatasetId, @FieldId, 2, (select convert(varchar, GETDATE(), 121)), 'Row QA', 'QAStatusId', null, 1, null, 99, 'select', null)

--Next, remove the existing records in Row QA Status table (DatasetQAStatus1), because they are incorrect; they are for Activity-level
delete from dbo.DatasetQAStatus1 where Dataset_Id = @DatasetId
--2 records

--Next, add the Row QA Status options into the Row QA Status table
insert into dbo.DatasetQAStatus1(Dataset_Id, QAStatus_Id)
values
(@DatasetId, 1),
(@DatasetId, 12),
(@DatasetId, 13),
(@DatasetId, 14),
(@DatasetId, 15),
(@DatasetId, 16),
(@DatasetId, 18)


--Now, update the WaterQuality_vw
--up
drop view WaterQuality_vw
go
create view WaterQuality_vw
AS
SELECT        TOP (100) PERCENT loc.OtherAgencyId, d.SampleDate, CASE d .CharacteristicName WHEN 'Water Temperature' THEN 'Temperature, Water' ELSE d .CharacteristicName END AS CharacteristicName, d.Result, 
                         CASE d .ResultUnits WHEN 'FT' THEN 'ft' WHEN 'G/L' THEN 'g/L' WHEN 'INHG' THEN 'inHg' WHEN 'M' THEN 'm' WHEN 'MG/L' THEN 'mg/L' WHEN 'MG/M3' THEN 'mg/m3' WHEN 'MMHG' THEN 'mmHg' WHEN 'NG/KG' THEN 'ng/kg'
                          WHEN 'UG/KG' THEN 'ug/kg' WHEN 'UG/L' THEN 'ug/L' WHEN 'UMHO/CM' THEN 'umho/cm' WHEN 'CFU/100ML' THEN 'cfu/100mL' WHEN 'NG/L' THEN 'ng/L' WHEN 'US/CM' THEN 'uS/cm' ELSE ResultUnits END AS ResultUnits,
                          d.MdlResults, d.SampleFraction, d.MethodSpeciation, d.ContextID, d.MethodID, d.DetectionLimit, CASE WHEN (Result IS NULL) AND DataType = 'ISCO' THEN 'Ignore' WHEN (Result IS NULL) AND 
                         DataType = 'Handheld' THEN 'Not Reported' WHEN (Result IS NULL) AND MdlResults LIKE ('%>%') THEN 'Present Above Quantification Limit' WHEN (Result IS NULL) AND MdlResults LIKE ('%<%') 
                         THEN 'Present Below Quantification Limit' WHEN (Result IS NULL) AND MdlResults LIKE ('%Present%') THEN 'Present Below Quantification Limit' WHEN (Result IS NULL) 
                         THEN 'Not Detected' ELSE '' END AS ResultDetectionCondition, d.LabDuplicate, d.LabName, d.SampleID, d.Comments, h.DataType, h.FieldsheetLink, h.HeaderComments, a.Id AS ActivityId, a.DatasetId, a.InstrumentId, 
                         a.ActivityDate, a.CreateDate, loc.Id AS LocationId, loc.Label AS LocationLabel, aq.QAStatusName, aq.Comments AS ActivityQAComments, d.QAStatusId AS RowQAStatusId, aq.QAStatusId AS ActivityQAStatusId, d.RowId, d.RowStatusId, 
                         CASE DataType WHEN 'ISCO' THEN 'Field Msr/Obs-Portable Data Logger' WHEN 'Handheld' THEN 'Field Msr/Obs-Portable Data Logger' WHEN 'Lab' THEN 'Sample-Routine' ELSE '' END AS ActivityType, 
                         CASE CharacteristicName WHEN 'Conductivity-TDS' THEN 'Calculated' WHEN 'Conductivity-Spec Cond' THEN 'Calculated' ELSE 'Actual' END AS ResultValueType, 
                         CASE d .ResultUnits WHEN 'FT' THEN 'ft' WHEN 'G/L' THEN 'g/L' WHEN 'INHG' THEN 'inHg' WHEN 'M' THEN 'm' WHEN 'MG/L' THEN 'mg/L' WHEN 'MG/M3' THEN 'mg/m3' WHEN 'MMHG' THEN 'mmHg' WHEN 'NG/KG' THEN 'ng/kg'
                          WHEN 'UG/KG' THEN 'ug/kg' WHEN 'UG/L' THEN 'ug/L' WHEN 'UMHO/CM' THEN 'umho/cm' WHEN 'CFU/100ML' THEN 'cfu/100mL' WHEN 'NG/L' THEN 'ng/L' WHEN 'US/CM' THEN 'uS/cm' ELSE d .ResultUnits END AS ResultDetectionUnit
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterQuality_Header_VW AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.Locations AS loc ON loc.Id = a.LocationId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id LEFT OUTER JOIN
                         dbo.WaterQuality_Detail_VW AS d ON h.ActivityId = d.ActivityId
go

-- On WaterTemp, Change the Label, to reduce confusion.
update dbo.DatasetFields
set Label = 'Row QA'
where DatasetId in (select Id from dbo.Datasets where [Name] like '%Water Temp%')
and Label = 'QA' and DbColumnName = 'QAStatusId'
--19 Records

--Down					 
/*drop view WaterQuality_vw
go
create view WaterQuality_vw
AS
SELECT        TOP (100) PERCENT loc.OtherAgencyId, d.SampleDate, CASE d .CharacteristicName WHEN 'Water Temperature' THEN 'Temperature, Water' ELSE d .CharacteristicName END AS CharacteristicName, d.Result, 
                         CASE d .ResultUnits WHEN 'FT' THEN 'ft' WHEN 'G/L' THEN 'g/L' WHEN 'INHG' THEN 'inHg' WHEN 'M' THEN 'm' WHEN 'MG/L' THEN 'mg/L' WHEN 'MG/M3' THEN 'mg/m3' WHEN 'MMHG' THEN 'mmHg' WHEN 'NG/KG' THEN 'ng/kg'
                          WHEN 'UG/KG' THEN 'ug/kg' WHEN 'UG/L' THEN 'ug/L' WHEN 'UMHO/CM' THEN 'umho/cm' WHEN 'CFU/100ML' THEN 'cfu/100mL' WHEN 'NG/L' THEN 'ng/L' WHEN 'US/CM' THEN 'uS/cm' ELSE ResultUnits END AS ResultUnits,
                          d.MdlResults, d.SampleFraction, d.MethodSpeciation, d.ContextID, d.MethodID, d.DetectionLimit, CASE WHEN (Result IS NULL) AND DataType = 'ISCO' THEN 'Ignore' WHEN (Result IS NULL) AND 
                         DataType = 'Handheld' THEN 'Not Reported' WHEN (Result IS NULL) AND MdlResults LIKE ('%>%') THEN 'Present Above Quantification Limit' WHEN (Result IS NULL) AND MdlResults LIKE ('%<%') 
                         THEN 'Present Below Quantification Limit' WHEN (Result IS NULL) AND MdlResults LIKE ('%Present%') THEN 'Present Below Quantification Limit' WHEN (Result IS NULL) 
                         THEN 'Not Detected' ELSE '' END AS ResultDetectionCondition, d.LabDuplicate, d.LabName, d.SampleID, d.Comments, h.DataType, h.FieldsheetLink, h.HeaderComments, a.Id AS ActivityId, a.DatasetId, a.InstrumentId, 
                         a.ActivityDate, a.CreateDate, loc.Id AS LocationId, loc.Label AS LocationLabel, aq.QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId, aq.QAStatusId AS ActivityQAStatusId, d.RowId, d.RowStatusId, 
                         CASE DataType WHEN 'ISCO' THEN 'Field Msr/Obs-Portable Data Logger' WHEN 'Handheld' THEN 'Field Msr/Obs-Portable Data Logger' WHEN 'Lab' THEN 'Sample-Routine' ELSE '' END AS ActivityType, 
                         CASE CharacteristicName WHEN 'Conductivity-TDS' THEN 'Calculated' WHEN 'Conductivity-Spec Cond' THEN 'Calculated' ELSE 'Actual' END AS ResultValueType, 
                         CASE d .ResultUnits WHEN 'FT' THEN 'ft' WHEN 'G/L' THEN 'g/L' WHEN 'INHG' THEN 'inHg' WHEN 'M' THEN 'm' WHEN 'MG/L' THEN 'mg/L' WHEN 'MG/M3' THEN 'mg/m3' WHEN 'MMHG' THEN 'mmHg' WHEN 'NG/KG' THEN 'ng/kg'
                          WHEN 'UG/KG' THEN 'ug/kg' WHEN 'UG/L' THEN 'ug/L' WHEN 'UMHO/CM' THEN 'umho/cm' WHEN 'CFU/100ML' THEN 'cfu/100mL' WHEN 'NG/L' THEN 'ng/L' WHEN 'US/CM' THEN 'uS/cm' ELSE d .ResultUnits END AS ResultDetectionUnit
FROM            dbo.Activities AS a INNER JOIN
                         dbo.WaterQuality_Header_VW AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.Locations AS loc ON loc.Id = a.LocationId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id LEFT OUTER JOIN
                         dbo.WaterQuality_Detail_VW AS d ON h.ActivityId = d.ActivityId
go
*/
