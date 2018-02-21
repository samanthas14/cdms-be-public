declare @DatasetId as numeric;
set @DatasetId = (select Id from dbo.Datasets where Name = 'CRPP Contracts') --1232, ProjectId = 2247

update dbo.Datasets
set Config = '{"RestrictRoles":["CRPP"], "DataEntryPage": {"HiddenFields": ["Location","ActivityDate","QA","Instrument"], "ShowFields": ["DateReceived","ProjectLead"]}}'
where [Id] = @DatasetId 

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

update dbo.Fields set [Validation] = 'i' where FieldCategoryId = 17 and DataType = 'int'