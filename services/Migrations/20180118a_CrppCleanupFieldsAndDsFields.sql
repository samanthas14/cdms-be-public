declare @DatasetId as numeric;
set @DatasetId = (select Id from dbo.Datasets where Name = 'CRPP Contracts') --2247

delete from dbo.DatasetFields
where DatasetId = @DatasetId
and DbColumnName in ('CrppPersonnel', 'ActivityTypeId', 
'Agency', 'PermitNumber', 'DateReceived', 'DateOfAction', 'ActionTaken', 'ActivityNotes', 
'AttachedDocument', 'AcresSurveyed', 'HprcsitsRecorded', 'ShpoReportNumber', 'ShpoCaseNumber')

declare @FieldCategoryId as numeric;
set @FieldCategoryId = (select Id from dbo.Datasets where Name = 'CRPP Contracts') --17

delete from dbo.Fields
where FieldCategoryId = @FieldCategoryId
and DbColumnName in ('CrppPersonnel', 'ActivityTypeId', 
'Agency', 'PermitNumber', 'DateReceived', 'DateOfAction', 'ActionTaken', 'ActivityNotes', 
'AttachedDocument', 'AcresSurveyed', 'HprcsitsRecorded', 'ShpoReportNumber', 'ShpoCaseNumber')