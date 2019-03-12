--this is the full update script that combines all of other update scripts
-- to update CDMS1 to CDMS2


ALTER TABLE [dbo].[Datastores] ADD [LocationTypeId] [nvarchar](max)
go

DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Datastores')
AND col_name(parent_object_id, parent_column_id) = 'DatastoreDatasetId';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Datastores] DROP CONSTRAINT [' + @var0 + ']')

go
ALTER TABLE [dbo].[Datastores] DROP COLUMN [DatastoreDatasetId]
go
-- map the LocationTypeId as it should be
update Datastores set LocationTypeId = 4 WHERE TablePrefix = 'AdultWeir';
update Datastores set LocationTypeId = 113 WHERE TablePrefix = 'BSample';
update Datastores set LocationTypeId = 6 WHERE TablePrefix = 'WaterTemp';
update Datastores set LocationTypeId = 7 WHERE TablePrefix = 'SpawningGroundSurvey';
update Datastores set LocationTypeId = 109 WHERE TablePrefix = 'CreelSurvey';
update Datastores set LocationTypeId = 101 WHERE TablePrefix = 'Electrofishing';
update Datastores set LocationTypeId = 102 WHERE TablePrefix = 'SnorkelFish';
update Datastores set LocationTypeId = 103 WHERE TablePrefix = 'ScrewTrap';
update Datastores set LocationTypeId = 104 WHERE TablePrefix = 'FishScales';
update Datastores set LocationTypeId = 105 WHERE TablePrefix = 'WaterQuality';
update Datastores set LocationTypeId = 106 WHERE TablePrefix = 'StreamNet_RperS';
update Datastores set LocationTypeId = 108 WHERE TablePrefix = 'StreamNet_SAR';
update Datastores set LocationTypeId = 107 WHERE TablePrefix = 'StreamNet_NOSA';
update Datastores set LocationTypeId = 110 WHERE TablePrefix = 'ArtificialProduction';
update Datastores set LocationTypeId = 111 WHERE TablePrefix = 'CrppContracts';
update Datastores set LocationTypeId = 112 WHERE TablePrefix = 'Metrics';
update Datastores set LocationTypeId = 114 WHERE TablePrefix = 'JvRearing';
update Datastores set LocationTypeId = 115 WHERE TablePrefix = 'Genetic';
update Datastores set LocationTypeId = 116 WHERE TablePrefix = 'Benthic';
update Datastores set LocationTypeId = 117 WHERE TablePrefix = 'Drift';
update Datastores set LocationTypeId = 118 WHERE TablePrefix = 'FishTransport';
update Datastores set LocationTypeId = 8 WHERE TablePrefix = 'Appraisal';
update Datastores set LocationTypeId = 3 WHERE TablePrefix = 'CRPP';
update Datastores set LocationTypeId = 112 WHERE TablePrefix = 'Hab';

go

create view LastUpdatedDatasets_VW as 
select a.DatasetId, a.UserId, u.Fullname, d.ProjectId, d.DatastoreId, a.CreateDate, convert(varchar, a.CreateDate, 100) as LastUpdated, d.Name as DatasetName, p.Name as ProjectName
from activities a
join datasets d on d.id = a.datasetid
join projects p on p.id = d.projectid
join users u on a.userId = u.Id
where a.createdate = (select max(aa.createdate) from activities aa where aa.userid=a.userid and aa.datasetid = a.datasetid)

go

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

ALTER VIEW dbo.Metrics_vw
AS
SELECT        h.Id, h.YearReported, h.ByUserId, h.EffDt, a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, d.Id AS Metrics_Detail_Id, d.WorkElementName, d.Measure, 
                         d.PlannedValue, d.ActualValue, d.Comments, d.RowId, d.RowStatusId, d.ByUserId AS Metrics_Detail_ByUserId, d.QAStatusId, d.EffDt AS Metrics_Detail_EffDt, l.Label AS LocationLabel, aq.QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Metrics_Header_VW AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id LEFT OUTER JOIN
                         dbo.Metrics_Detail_VW AS d ON h.ActivityId = d.ActivityId

go

--add some columns that are long overdue!
ALTER TABLE [dbo].[Datastores] ADD [DefaultConfig] [nvarchar](max)
ALTER TABLE [dbo].[Fields] ADD [DatastoreId] [int] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[Fields] ADD [FieldRoleId] [int] NOT NULL DEFAULT 0
go

-- add a new field category to fix the doubling up of FishTransport and WaterTemp and then set the new DatastoreId field
DECLARE @fishtransportfcid int = 0;
insert into FieldCategories (Name, Description) values ('Fish Transport','Fish Transport');
select @fishtransportfcid = scope_identity();
update fields set FieldCategoryId = @fishtransportfcid where id in (26,32,35,1038,1039,1040,1046,1052);
update datastores set FieldCategoryId = @fishtransportfcid where TablePrefix = 'FishTransport';
update fields set DatastoreId = (select d.Id from Datastores d where d.FieldCategoryId = fields.FieldCategoryId);
go


-- now we can remove the fieldcategory altogether
IF object_id(N'[dbo].[FK_dbo.Fields_dbo.FieldCategories_FieldCategoryId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[Fields] DROP CONSTRAINT [FK_dbo.Fields_dbo.FieldCategories_FieldCategoryId]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_FieldCategoryId' AND object_id = object_id(N'[dbo].[Fields]', N'U'))
    DROP INDEX [IX_FieldCategoryId] ON [dbo].[Fields]
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Datastores')
AND col_name(parent_object_id, parent_column_id) = 'FieldCategoryId';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Datastores] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[Datastores] DROP COLUMN [FieldCategoryId]
DECLARE @var1 nvarchar(128)
SELECT @var1 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Fields')
AND col_name(parent_object_id, parent_column_id) = 'FieldCategoryId';
IF @var1 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Fields] DROP CONSTRAINT [' + @var1 + ']')
ALTER TABLE [dbo].[Fields] DROP COLUMN [FieldCategoryId]
DROP TABLE [dbo].[FieldCategories]
go

-- update the new fieldroleid in the fields table from a row in datasetfields (if exists)
--   note: after this runs, there are still 35 rows that aren't set and need to be address by colette: e.g.: select * from fields where fieldroleid = 0
update fields set fieldroleid = (select top 1 fieldroleid from datasetfields df where df.fieldid = fields.Id) 
where fields.Id in (select distinct fieldid from datasetfields)
go

-- finally update our datastore default config with a config from one of the datasets that have a config for that datastore
update Datastores set DefaultConfig = (select top 1 Config from datasets where Config is not null and Datastores.Id = datasets.datastoreId) 
where Datastores.Id in (select distinct dc.DatastoreId from Datasets dc where dc.Config is not null and dc.DatastoreId is not null)
go

-- create a our activity system fields datastore!

DECLARE @activitydsid int = 0;

insert into Datastores (Name, TablePrefix, OwnerUserId, LocationTypeId, DefaultConfig) values ('ActivitySystemFields',null,1,null,
'{
  "DataEntryPage": {
    "HeaderFields": [
      "Location",
      "ActivityDate",
      "SampleDate",
      "Timezone",
      "Instrument",
      "AccuracyCheck",
      "PostAccuracyCheck"
    ],
    "QAFields": [
      "QAStatus",
      "QAComments"
    ],
    "DetailFields": [
      "RowQAStatus"
    ],
    "HiddenFields": [
      "Instrument",
      "AccuracyCheck",
      "PostAccuracyCheck",
      "Timezone",
      "SampleDate",
      "RowQAStatus"
    ],
    "sort": {
      "field": "ActivityDate",
      "direction": "desc"
    }
  }
}');

select @activitydsid = scope_identity();

insert into Fields (Name, Description, DbColumnName, ControlType, DatastoreId, FieldRoleId)
values 
('Activity Date','Activity date field','ActivityDate','activity-date', @activitydsid, 1),
('QA Status','Activity QA Status','QAStatus','qa-status-select',@activitydsid,1),
('QA Comments','Activity QA Comments','QAComments','qa-status-comment',@activitydsid,1),
('Location','Activity location','LocationId','location-select',@activitydsid,1),
('Instrument','Activity instrument','InstrumentId','instrument-select',@activitydsid,1),
('Accuracy Check','Activity instrument accuracy check','AccuracyCheckId','accuracy-check-select',@activitydsid,1),
('Post Accuracy Check','Activity instrument post-accuracy check','PostAccuracyCheckId','post-accuracy-check-select',@activitydsid,1),
('Reading Timezone','Activity reading timezone','Timezone','timezone-select',@activitydsid,1),
('Row QA Status','Activity row QA status','QAStatusId','select',@activitydsid,1),
('Activity Description','Activity description field','Description','activity-text', @activitydsid, 1);

go

----------------- run this together

DECLARE @sysdsid int = 0;
select @sysdsid=id from DataStores where Name = 'ActivitySystemFields';

-- add activity date to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, (select id from Fields where DatastoreId = @sysdsid AND DbColumnName = 'ActivityDate'), 1, 'Activity Date','ActivityDate',1,1,'activity-date',null
from datasets where datastoreid in (1,2,4,5,7,8,9,10,11,12,13,14,15,18,19,20);

-- add activity date as "sample date" for Benthic and Drift
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, (select id from Fields where DatastoreId = @sysdsid AND DbColumnName = 'ActivityDate'), 1, 'Sample Date','ActivityDate',1,1,'activity-date',null
from datasets where datastoreid in (21,22);

-- add location to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, (select id from Fields where DatastoreId = @sysdsid AND DbColumnName = 'LocationId'), 1, 'Location','LocationId',1,2,'location-select',null
from datasets where datastoreid in (1,2,3,4,5,6,7,8,9,11,12,13,14,15,17,18,19,20,21,22);

-- add qa status to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, (select id from Fields where DatastoreId = @sysdsid AND Name = 'QA Status'), 1, 'QA Status','QAStatusId',1,97,'qa-status-select',null
from datasets where datastoreid in (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,17,18,19,20,21,22);

-- add qa comments to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, (select id from Fields where DatastoreId = @sysdsid AND DbColumnName = 'QAComments'), 1, 'QA Comments','QAComments',1,98,'qa-status-comment',null
from datasets where datastoreid in (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,17,18,19,20,21,22);

-- for crpp contracts

insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, (select id from Fields where DatastoreId = @sysdsid AND DbColumnName = 'ActivityDate'), 1, 'Date Received','ActivityDate',1,1,'activity-date',null
from datasets where datastoreid in (16);

insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, (select id from Fields where DatastoreId = @sysdsid AND DbColumnName = 'LocationId'), 1, 'Location','LocationId',1,2,'hidden',null
from datasets where datastoreid in (16);

insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, (select id from Fields where DatastoreId = @sysdsid AND Name = 'QA Status'), 1, 'QA Status','QAStatusId',1,97,'hidden',null
from datasets where datastoreid in (16);

insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, (select id from Fields where DatastoreId = @sysdsid AND DbColumnName = 'QAComments'), 1, 'QA Comments','QAComments',1,98,'hidden',null
from datasets where datastoreid in (16);


-- add instruments to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, (select id from Fields where DatastoreId = @sysdsid AND DbColumnName = 'InstrumentId'), 1, 'Instrument','InstrumentId',1,4,'instrument-select',null
from datasets where datastoreid in (3,6);

-- add accuracy check to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, (select id from Fields where DatastoreId = @sysdsid AND DbColumnName = 'AccuracyCheckId'), 1, 'Accuracy Check','AccuracyCheckId',1,5,'accuracy-check-select',null
from datasets where datastoreid in (3,6);

-- add post accuracy check to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, (select id from Fields where DatastoreId = @sysdsid AND DbColumnName = 'PostAccuracyCheckId'), 1, 'Post-accuracy Check','PostAccuracyCheckId',1,6,'post-accuracy-check-select',null
from datasets where datastoreid in (3,6);

-- add timezone to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, (select id from Fields where DatastoreId = @sysdsid AND DbColumnName = 'Timezone'), 1, 'Timezone','Timezone',1,7,'timezone-select',null
from datasets where datastoreid in (3,6);

-- add rowqa to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, (select id from Fields where DatastoreId = @sysdsid AND Name = 'Row QA Status'), 2, 'QA','QAStatusId',1,99,'select',null
from datasets where datastoreid in (3);

-- add description to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, (select id from Fields where DatastoreId = @sysdsid AND DbColumnName = 'Description'), 1, 'Description','Description',1,8,'activity-text',null
from datasets where datastoreid in (3,6);


go

DECLARE @newdsid int = 0;
DECLARE @newprojectypeid int = 0;
DECLARE @newprojectid int = 0;
DECLARE @newdatasetid int = 0;

insert into ProjectTypes (Name, Description) values ('System','CDMS System')
select @newprojectypeid = scope_identity();

insert into Projects 
	(ProjectTypeId, OrganizationId, Name, Description, CreateDateTime) 
	values 
	(@newprojectypeid, 1,'System','CDMS System',getdate());
select @newprojectid = scope_identity();

insert into Datastores (Name, TablePrefix, OwnerUserId, LocationTypeId, DefaultConfig) values ('LocationSystemFields',null,1,null,'{}');
select @newdsid = scope_identity();

insert into Fields (Name, Description, DbColumnName, ControlType, DatastoreId, FieldRoleId, DataSource, DataType,PossibleValues,Validation) 	
values 
('Location Type','LocationTypeId','LocationTypeId','select-number',@newdsid,1,'select Id, Name as Label from locationtypes','int',null,null),
('Label','Label','Label','text',@newdsid,1,null,'string',null,null),
('Description','Description','Description','textarea',@newdsid,1,null,'string',null,null),
('Other Agency Id','OtherAgencyId','OtherAgencyId','text',@newdsid,1,null,'string',null,null),
('Elevation','Elevation','Elevation','number',@newdsid,1,null,'int',null,null),
('GPS Easting','GPSEasting','GPSEasting','easting',@newdsid,1,null,'double',null,'required;[100000,999999]'),
('GPS Northing','GPSNorthing','GPSNorthing','northing',@newdsid,1,null,'double',null,'required;[1000000,9999999]'),
('UTM Zone','UTMZone','UTMZone','text',@newdsid,1,null,'string',null,null),
('Projection','Projection','Projection','text',@newdsid,1,null,'string',null,null),
('Latitude','Latitude','Latitude','number',@newdsid,1,null,'int',null,null),
('Longitude','Longitude','Longitude','number',@newdsid,1,null,'int',null,null),
('Status','Status','Status','select-number',@newdsid,1,null,'int','{"0":"Active","1":"Inactive"}',null),
('Name','Name','Name','text',@newdsid,1,null,'string',null,null),
('River Mile','RiverMile','RiverMile','number',@newdsid,1,null,'int',null,null),
('Study Design','StudyDesign','StudyDesign','text',@newdsid,1,null,'string',null,null),
('Wetted Depth','WettedDepth','WettedDepth','number',@newdsid,1,null,'int',null,null),
('Wetted Width','WettedWidth','WettedWidth','number',@newdsid,1,null,'int',null,null),
('Waterbody','WaterBodyId','WaterBodyId','select-number',@newdsid,1,'SELECT Id, Name as Label from Waterbodies','int',null,null),
('Create DateTime','CreateDateTime','CreateDateTime','datetime',@newdsid,1,null, 'datetime',null,null),
('Image Link','ImageLink','ImageLink','text',@newdsid,1,null,'string',null,null),
('Subproject Id','SubprojectId','SubprojectId','number',@newdsid,1,null,'int',null,null),
('SdeObject Id','SdeObjectId','SdeObjectId','number',@newdsid,1,null,'int',null,null);


insert into datasets 
(ProjectId, DefaultRowQAStatusId, DefaultActivityQAStatusId, StatusId, CreateDateTime, Name, Description, DatastoreId) 
values 
(@newprojectid, 1, 5, 1,getdate(),'Location','CDMS Location Form',@newdsid );

select @newdatasetid = scope_identity();


insert into DatasetFields 
(DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, ControlType,InstrumentId,SourceId) 
select
@newdatasetid, Id, FieldRoleId, getDate(), Name, DbColumnName, ControlType, null ,1
FROM Fields where DatastoreId = @newdsid;

go

-- Create a new datastore + dataset for the CRPP Correspondence dataset (no longer a tab but a bona-fide dataset)

DECLARE @crppprojectid int = 2247;
DECLARE @newdsid int = 0;
DECLARE @newdatasetid int = 0;

insert into Datastores (Name, TablePrefix, OwnerUserId, LocationTypeId, DefaultConfig) values ('CRPPCorrespondence',null,1,null,'{"ActivitiesPage":{"Route":"crppcorrespondence"}}');
select @newdsid = scope_identity();

insert into datasets 
(ProjectId, DefaultRowQAStatusId, DefaultActivityQAStatusId, StatusId, CreateDateTime, Name, Description, DatastoreId, Config) 
values 
(@crppprojectid, 1, 5, 1,getdate(),'CRPP Correspondence','CRPP Correspondence',@newdsid, '{"ActivitiesPage":{"Route":"crppcorrespondence"}}' );

select @newdatasetid = scope_identity();

go


-- now do the same for Habitat Sites.
DECLARE @newhabdsid int = 0;

insert into Datastores (Name, TablePrefix, OwnerUserId, LocationTypeId, DefaultConfig) values ('Habitat Sites',null,1,null,'{"ActivitiesPage":{"Route":"habitatsites"}}');
select @newhabdsid = scope_identity();

--add a dataset for each habitat project that should have one
insert into datasets 
(ProjectId, DefaultRowQAStatusId, DefaultActivityQAStatusId, StatusId, CreateDateTime, Name, Description, DatastoreId, Config) 
select Id,1, 5, 1,getdate(),'Habitat Sites','Habitat Sites',@newhabdsid, '{"ActivitiesPage":{"Route":"habitatsites"}}' 
from projects where Id in (select distinct RelationId from metadatavalues_vw where metadatapropertyid = 24 and [values] = 'Habitat');

go

update metadataproperties set controltype = 'hidden' where id in (11,13,25,26);
update metadataproperties set controltype = 'text' where id in (22);
update metadataproperties set controltype = 'textarea' where id in (1,3,4,5,9,14,15,20);

DECLARE @entityid int = 0;
insert into metadataentities (name, description) values ('Correspondence','CRPP Correspondence lookup fields');
select @entityid = scope_identity();

insert into metadataproperties (MetadataEntityId, Name, Description, DataType, PossibleValues, ControlType) values 
(@entityid, 'Agency','Agency','string','[]','select'),
(@entityid, 'County','County','string','[]','select'),
(@entityid, 'CorrespondenceType','Correspondence Type','string','[]','select'),
(@entityid, 'ResponseType','Response Type','string','[]','select'),
(@entityid, 'NumberOfDays','Number Of Days','number','[]','select'),
(@entityid, 'StaffMember','Staff Member - These names must match the user''s name in Outlook, and the full name in table Users.','string','[]','select'),
(@entityid, 'ProjectLead','Project Lead - These names must match the user''s name in Outlook, and the full name in table Users.','string','[]','select');

update metadataproperties set PossibleValues = 
'[
"ACHP",
"Anderson Perry",
"Army",
"Baker County",
"Benton County",
"BIA",
"BLM",
"Blue Mountain Ranger District",
"BNSF",
"BOR",
"BPA",
"Camas",
"CenturyLink",
"Clark County",
"College Place",
"Columbia County",
"Corps Portland District",
"Corps Regulatory",
"Corps Walla Walla District",
"CRGNSA",
"CTUIR",
"DAHP",
"DECD",
"Department of Ecology",
"DEQ",
"DOE",
"DOGAMI",
"DSL",
"EPA",
"FAA",
"FCC",
"Federal Transit Authority",
"FEMA",
"FERC",
"FHWA",
"Fisheries",
"Fort Vancouver (NPS)",
"Franklin County",
"FSA",
"Hells Canyon NRA",
"Heppner Ranger District",
"Hermiston",
"Hood River County",
"HUD",
"Idaho Power",
"Irrigon",
"John Day Fossil Beds (NPS)",
"Kennewick",
"Klickitat County",
"La Grande Ranger District",
"Landowner",
"Malheur National Forest",
"Morrow County",
"Navy",
"Nez Perce National Historical Park (NPS)",
"North Fork John Day Ranger District",
"Northwest Pipeline",
"NPS",
"NRCS",
"ODEQ",
"ODOE",
"ODOT",
"OPRD",
"Oregon City",
"Oregon Military Department/Oregon Army National Guard",
"Other",
"OWRD",
"PacifiCorp",
"Pasco",
"PGE",
"Planning Dept",
"Pomeroy Ranger District",
"Port of Benton",
"Port of Clarkston",
"Port of Columbia",
"Port of Kennewick",
"Port of Morrow",
"Port of Umatilla",
"Port of Walla Walla",
"Public Works",
"RAF",
"Recreation and Conservation Office",
"Richland",
"Rural Development",
"RUS",
"SHPO Oregon",
"Skamania County",
"Skamania County PUD",
"Umatilla County",
"Umatilla National Forest",
"UPRR",
"USACE",
"USFWS",
"VA",
"Vancouver",
"Walla Walla City",
"Walla Walla County",
"Walla Walla Ranger District",
"Wallowa County",
"Wallowa Valley Ranger District",
"Wallowa-Whitman National Forest",
"Wasco County",
"Washington Department of Commerce",
"Washington Department of Health",
"Washington Department of Natural Resources",
"Washington State Parks",
"Water Resources",
"WDFW",
"Western Federal Lands Highway Division",
"Whitman Mission (NPS)",
"Whitman Unit",
"Wildhorse Resort and Casino (WRC)",
"Wildlife",
"WSDOT",
"Yellowstone National Park"
]' where name = 'Agency';

update metadataproperties set PossibleValues = 
'[
"Asotin",
"Baker",
"Benton",
"Clark",
"Columbia",
"Franklin",
"Garfield",
"Gilliam",
"Grant, WA",
"Grant, OR",
"Hood River",
"Klickitat",
"Malheur",
"Morrow",
"Multnomah",
"Other",
"Sherman",
"Skamania",
"Umatilla",
"Union",
"Walla Walla",
"Wallowa",
"Wasco",
"Wheeler",
"Whitman"]' where name = 'County';

update metadataproperties set PossibleValues = 
'["Project Notification",
"Notice of Application",
"Seeking Concurrence",
"Document Review",
"Permit Review",
"Sending materials for our records",
"Other"]' where name = 'CorrespondenceType';

update metadataproperties set PossibleValues = 
'["APE letter",
"Asked to be consulting party",
"Defer to other tribe(s)",
"Determination of Eligibility",
"Did not review",
"Emailed Comments",
"Finding of Effect",
"Issued survey/excavation permit",
"Let it go",
"Missed opportunity to review",
"NAGPRA FR Notice",
"NAGPRA inventory/summary",
"none--ok",
"Notice of Application",
"Other",
"Out of area",
"Permit Application",
"Report for Review",
"Requested a monitor",
"Requested a survey",
"Requested additional information",
"Requested report",
"Requested testing",
"Response to comments",
"Reviewed report",
"Same as previously reviewed project",
"Sent letter",
"Signed off on",
"Simple Notification"]' where name = 'ResponseType';

update metadataproperties set PossibleValues = 
'["Bambi Rodriguez",
"Shawn Steinmetz",
"Catherine Dickson",
"Carey Miller",
"Jennifer Karson Engum",
"Shari Sheoships",
"Arthur Van Pelt",
"Teara Farrow Ferman",
"Dara Williams-Worden",
"Julius Patrick",
"Nathan May",
"Kristen Tiede"]' where name = 'StaffMember';

update metadataproperties set PossibleValues = 
'["Bambi Rodriguez",
"Carey Miller",
"Catherine Dickson",
"Kristen Tiede",
"Shawn Steinmetz",
"Arthur Van Pelt",
"Dara Williams-Worden",
"Jennifer Karson Engum",
"Julius Patrick",
"Shari Sheoships",
"Nathan May",
"Teara Farrow Ferman"]' where name = 'ProjectLead';

go

--creates lookuptables entities

CREATE TABLE [dbo].[LookupTables] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max),
    [Label] [nvarchar](max),
    [Description] [nvarchar](max),
    [DatasetId] [int],
    CONSTRAINT [PK_dbo.LookupTables] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_DatasetId] ON [dbo].[LookupTables]([DatasetId])
ALTER TABLE [dbo].[LookupTables] ADD CONSTRAINT [FK_dbo.LookupTables_dbo.Datasets_DatasetId] FOREIGN KEY ([DatasetId]) REFERENCES [dbo].[Datasets] ([Id])

ALTER TABLE [dbo].[Projects] ADD [Config] [nvarchar](max)

go

-- setup our location table fields: fishermen
DECLARE @newdsid int = 0;
DECLARE @newdatasetid int = 0;
DECLARE @cdmssystemprojectid int = 0;

set @cdmssystemprojectid =(select Id from projects where [name] = 'System');

insert into Datastores (Name, TablePrefix, OwnerUserId, LocationTypeId, DefaultConfig) values ('Fisherman Lookup Fields','Fishermen',1,null,'{}');
select @newdsid = scope_identity();

insert into Fields (Name, Description, DbColumnName, ControlType, DatastoreId, FieldRoleId, DataSource, DataType,PossibleValues,Validation) 	
values 
('First Name','FirstName','FirstName','text',@newdsid,1,null,'string',null,null),
('Aka','Aka','Aka','text',@newdsid,1,null,'string',null,null),
('Last Name','LastName','LastName','text',@newdsid,1,null,'string',null,null),
('Phone Number','PhoneNumber','PhoneNumber','text',@newdsid,1,null,'string',null,null),
('Date Added','DateAdded','DateAdded','date',@newdsid,1,null,'datetime',null,null),
('Date Inactive','DateInactive','DateInactive','date',@newdsid,1,null,'datetime',null,null),
('Fullname','FullName','FullName','text',@newdsid,1,null,'string',null,null),
('Comments','FishermanComments','FishermanComments','text',@newdsid,1,null,'string',null,null),
('Status','StatusId','StatusId','select-number',@newdsid,1,null,'int','{"0":"Active","1":"Inactive"}',null),
('Ok To Call','OkToCallId','OkToCallId','select-number',@newdsid,1,null,'int','{"0":"Yes","1":"No"}',null);


insert into datasets 
(ProjectId, DefaultRowQAStatusId, DefaultActivityQAStatusId, StatusId, CreateDateTime, Name, Description, DatastoreId) 
values 
(@cdmssystemprojectid, 1, 5, 1,getdate(),'Fishermen','CDMS Fishermen Lookup Table',@newdsid );

select @newdatasetid = scope_identity();

insert into DatasetFields 
(DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, ControlType,InstrumentId,SourceId) 
select
@newdatasetid, Id, FieldRoleId, getDate(), Name, DbColumnName, ControlType, null ,1
FROM Fields where DatastoreId = @newdsid;

insert into LookupTables
(Name, Label, Description, DatasetId)
values
('Fishermen','Fishermen','Fishermen Lookup Table',@newdatasetid)

go

-- setup our location table fields: seasons
DECLARE @newdsid int = 0;
DECLARE @newdatasetid int = 0;
DECLARE @cdmssystemprojectid int = 0;

set @cdmssystemprojectid =(select Id from projects where [name] = 'System');

insert into Datastores (Name, TablePrefix, OwnerUserId, LocationTypeId, DefaultConfig) values ('Seasons Lookup Fields','Seasons',1,null,'{}');
select @newdsid = scope_identity();

insert into Fields (Name, Description, DbColumnName, ControlType, DatastoreId, FieldRoleId, DataSource, DataType,PossibleValues,Validation) 	
values 
('Species','Species','Species','text',@newdsid,1,null,'string',null,null),
('Season','Season','Season','number',@newdsid,1,null,'int',null,null),
('Total Days','TotalDays','TotalDays','number',@newdsid,1,null,'int',null,null),
('Open Date','OpenDate','OpenDate','date',@newdsid,1,null,'datetime',null,null),
('Close Date','CloseDate','CloseDate','date',@newdsid,1,null,'datetime',null,null),
('Dataset','DatasetId','DatasetId','number',@newdsid,1,null,'int',null,null);


insert into datasets 
(ProjectId, DefaultRowQAStatusId, DefaultActivityQAStatusId, StatusId, CreateDateTime, Name, Description, DatastoreId) 
values 
(@cdmssystemprojectid, 1, 5, 1,getdate(),'Seasons','CDMS Seasons Lookup Table',@newdsid );

select @newdatasetid = scope_identity();

insert into DatasetFields 
(DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, ControlType,InstrumentId,SourceId) 
select
@newdatasetid, Id, FieldRoleId, getDate(), Name, DbColumnName, ControlType, null ,1
FROM Fields where DatastoreId = @newdsid;

insert into LookupTables
(Name, Label, Description, DatasetId)
values
('Seasons','Seasons','Seasons Lookup Table',@newdatasetid)

go

insert into LookupTables
(Name, Label, Description, DatasetId)
values
('Instruments','Instruments','Instruments Lookup Table',null)

go

--creates a new location type for "Habitat Sites" and modifies existing locations that are habitat sites to use it.

DECLARE @newloctypeid int = 0;

INSERT INTO LocationTypes (Name, Description) values ('Habitat Sites','Habitat Site Locations');

select @newloctypeid = scope_identity();

UPDATE Locations set LocationTypeId = @newloctypeid where subprojectid is not null and LocationTypeId = 112;

UPDATE Datastores set LocationTypeId = @newloctypeid where [Name] = 'Habitat Sites';

go

DECLARE @entityid int = 0;
insert into metadataentities (name, description) values ('Habitat Sites','Habitat Sites lookup fields');
select @entityid = scope_identity();

insert into metadataproperties (MetadataEntityId, Name, Description, DataType, PossibleValues, ControlType) values 
(@entityid, 'Collaborator','Collaborator','string','[]','select'),
(@entityid, 'Funder','Funder','string','[]','select');

update metadataproperties set PossibleValues = 
'[
"Blue Mountain Habitat Restoration Council",
"Bureau of Reclamation",
"Bonneville Power Authority",
"Columbia Conservation District",
"CTUIR",
"Eco Trust",
"Grande Ronde Model Watershed",
"Landowners",
"Nez Perce Tribe",
"NF John Day Watershed Council",
"Natural Resource Conservation Service",
"Oregon Department of Fish and Wildlife",
"Oregon Department of Transportation",
"Oregon Watershed Enhancement Board",
"Other",
"Pacific Coastal Salmon Recovery Fund",
"Pomeroy Conservation District",
"Salmon Recovery Funding Board",
"Snake River Salmon Recovery Board",
"Umatilla County Soil and Water Conservation District",
"Umatilla National Forest",
"US Forest Service",
"Wallowa Whitman National Forest",
"Washington Department of Fish and Wildlife"
]' where name = 'Collaborator';

update metadataproperties set PossibleValues = 
'[
"Blue Mountain Habitat Restoration Council",
"Bonneville Power Authority",
"Bureau of Reclamation",
"CTUIR",
"Eco Trust",
"Grande Ronde Model Watershed",
"NF John Day Watershed Council",
"Oregon Department of Fish and Wildlife",
"Oregon Watershed Enhancement Board",
"Other",
"Pacific Coastal Salmon Recovery Fund",
"Snake River Salmon Recovery Board",
"Umatilla County Soil and Water Conservation District",
"Umatilla National Forest",
"US Forest Service",
"Wallowa Whitman National Forest",
"Washington Department of Fish and Wildlife"
]' where name = 'Funder';

go

create table __Analytics (
	Id INT NOT NULL IDENTITY(1,1),
	RequestTimestamp datetime default CURRENT_TIMESTAMP,
	Username varchar(255) NOT NULL,
	Route varchar(255) NOT NULL,
	Action varchar(255) NOT NULL,
	Target INT,
	PRIMARY KEY (Id)
)
go


--- tweaks and fixes for CTUIR

--set creel time fields to be time types
update fields set ControlType = 'time' where id in (select id from fields where dbcolumnname in ('TimeStart', 'TimeEnd', 'InterviewTime') and DatastoreId = 14);
update datasetfields set ControlType = 'time' where id in (select id from datasetfields where dbcolumnname in ('TimeStart', 'TimeEnd', 'InterviewTime') and datasetid = 1230);
go

--set creel dropdown
update fields set possiblevalues = '{ "0": "00:00",
  "15": "00:15",
  "30": "00:30",
  "45": "00:45",
  "60": "01:00",
  "75": "01:15",
  "90": "01:30",
  "105": "01:45",
  "120": "02:00",
  "135": "02:15",
  "150": "02:30",
  "165": "02:45",
  "180": "03:00",
  "195": "03:15",
  "210": "03:30",
  "225": "03:45",
  "240": "04:00",
  "255": "04:15",
  "270": "04:30",
  "285": "04:45",
  "300": "05:00",
  "315": "05:15",
  "330": "05:30",
  "345": "05:45",
  "360": "06:00",
  "375": "06:15",
  "390": "06:30",
  "405": "06:45",
  "420": "07:00",
  "435": "07:15",
  "450": "07:30",
  "465": "07:45",
  "480": "08:00",
  "495": "08:15",
  "510": "08:30",
  "525": "08:45",
  "540": "09:00",
  "555": "09:15",
  "570": "09:30",
  "585": "09:45",
  "600": "10:00",
  "615": "10:15",
  "630": "10:30",
  "645": "10:45",
  "660": "11:00",
  "675": "11:15",
  "690": "11:30",
  "705": "11:45",
  "720": "12:00",
  "735": "12:15",
  "750": "12:30",
  "765": "12:45",
  "780": "13:00",
  "795": "13:15",
  "810": "13:30",
  "825": "13:45",
  "840": "14:00",
  "855": "14:15",
  "870": "14:30",
  "885": "14:45",
  "900": "15:00",
  "915": "15:15",
  "930": "15:30",
  "945": "15:45",
  "960": "16:00",
  "975": "16:15",
  "990": "16:30",
  "1005": "16:45",
  "1020": "17:00",
  "1035": "17:15",
  "1050": "17:30",
  "1065": "17:45",
  "1080": "18:00" }'
  where id = (select id from fields where name = 'Total Time Fished');
go

--set appraisal route and activity list fields
/*update datasets set Config = '{"RestrictRoles":["DECD"],"ActivitiesPage":{"ShowFields":["Allotment","AllotmentStatus"]},"DataEntryPage":{"HiddenFields":["Location","ActivityDate","Instrument","QA","BulkQaChange"]},"SpecifyActivityListFields":true,"ActivityListFields":["ActivityDate","AllotmentStatus","LeaseTypes","LocationId","Allotment","CobellAppraisalWave"]}' 
where id = 1193;
go
*/


--add project configs for lookups
update projects set config = '{"Lookups":[{"Id":"3","Label":"Instruments"}]}' where id = 1199;
update projects set config = '{"Lookups":[{"Id":"1","Label":"Fishermen","DatasetId":1275},{"Id":"2","Label":"Seasons","DatasetId":1276}]}' where id = 1217;
update projects set config = '{"Lookups":[{"Id":"7","Label":"Correspondence","Type":"Metafields"}]}' where id = 2247;
update projects set config = '{
  "Lookups": [
    {
      "Id": "8",
      "Label": "Habitat",
      "Type": "Metafields"
    }
  ],
  "ShowHabitatSitesForDatasets": [
    "Biom-Benthic",
    "Biom-Drift",
    "BioM-Electrofishing",
    "BioM-Snorkel"
  ]
}' where id = 2249;

go

--appraisal locations linked to project
--delete from locationprojects where Project_Id = 2246;
--insert into locationprojects (Location_Id, Project_Id) 
--select id, 2246 from locations where locationtypeid=8;
--go

-- add SharingLevel to files table, set default to 1 (private), update all existing rows
ALTER TABLE Files
ADD SharingLevel int NOT NULL
CONSTRAINT DefaultSharingLevelPrivate DEFAULT (1)
WITH VALUES;
go

ALTER TABLE HabitatItems
ADD SharingLevel int NOT NULL
CONSTRAINT DefaultHabItemSharingLevelPrivate DEFAULT (1)
WITH VALUES;
go

--delete these unused records
delete from fields where FieldRoleId = 0;
go

-- set for crppcontracts ProjectLead field
--update fields set DataSource = 'select PossibleValues from metadataproperties where name = ''ProjectLead''' where Name = 'Project Lead' and DatastoreId = 16;

--update water quality to 9,3 decimalprecision
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.WaterQuality_Detail')
AND col_name(parent_object_id, parent_column_id) = 'Result';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[WaterQuality_Detail] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[WaterQuality_Detail] ALTER COLUMN [Result] [decimal](9, 3) NULL

go

--update dataset configs per colette
update datasets set Config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange"]},"AllowSaveWithErrors":true,"EnableDuplicateChecking":true,"DuplicateCheckFields":["ActivityDate","CollectionType","LocationId"],"SpecifyActivityListFields":true,"ActivityListFields":["ActivityDate","CollectionType","LocationId","QAStatusId"]}' where DatastoreId = 1;
update datasets set Config = '{"DataEntryPage":{"HiddenFields":["ActivityDate"],"ShowFields":["Timezone","Instrument"]},"ActivitiesPage":{"ShowFields":["Location.Label","headerdata.FieldActivityType","Description","User.Fullname"]},"SpecifyActivityListFields":true,"ActivityListFields":["Description","FieldActivityType","InstrumentId","LocationId","QAStatusId"],"AllowSaveWithErrors":true,"EnableDuplicateChecking":true,"DuplicateCheckFields":["Description","FieldActivityType","InstrumentId","LocationId"]}' where DatastoreId = 3;
update datasets set Config = '{"DataEntryPage":{"HiddenFields":["Instrument","BulkQaChange"]},"AllowSaveWithErrors":true,"EnableDuplicateChecking":true,"DuplicateCheckFields":["ActivityDate","LocationId"],"SpecifyActivityListFields":true,"ActivityListFields":["ActivityDate","LocationId","QAStatusId"]}' where DatastoreId = 5;
update datasets set Config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange","ActivityDate"]},"ActivitiesPage":{"ShowFields":["Description","Location.Label","headerdata.DataType","User.Fullname"]},"AllowSaveWithErrors":true,"EnableDuplicateChecking":true,"DuplicateCheckFields":["DataType","LocationId"],"SpecifyActivityListFields":true,"ActivityListFields":["DataType","LocationId","QAStatusId"]}' where DatastoreId = 6;
update datasets set Config = '{"DataEntryPage":{"HiddenFields":["Instrument","BulkQaChange"]},"AllowSaveWithErrors":true,"EnableDuplicateChecking":true,"DuplicateCheckFields":["ActivityDate","LocationId"],"SpecifyActivityListFields":true,"ActivityListFields":["ActivityDate","LocationId","QAStatusId"]}' where DatastoreId in (7);
update datasets set Config = '{"DataEntryPage":{"HiddenFields":["Instrument","BulkQaChange"]},"AllowSaveWithErrors":true,"EnableDuplicateChecking":true,"DuplicateCheckFields":["ActivityDate","ArrivalTime","LocationId"],"SpecifyActivityListFields":true,"ActivityListFields":["ActivityDate","ArrivalTime","LocationId","QAStatusId"]}' where DatastoreId in (8);
update datasets set Config = '{"DataEntryPage":{"HiddenFields":["Instrument","BulkQaChange"]},"AllowSaveWithErrors":true,"EnableDuplicateChecking":true,"DuplicateCheckFields":["ActivityDate","LocationId"],"SpecifyActivityListFields":true,"ActivityListFields":["ActivityDate","LocationId","QAStatusId"]}' where DatastoreId in (9);
update datasets set Config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where DatastoreId in (18);
update datasets set Config = '{"DataEntryPage":{"HiddenFields":["Instrument","Location","BulkQaChange"]},"ActivitiesPage":{"ShowFields":["headerdata.RunYear","Location.Label","User.Fullname"],"HasDatasetLocations":"Yes","AllowMultipleLocations":"No"},"SpecifyActivityListFields":true,"ActivityListFields":["QAStatusId","RunYear","Technician"],"AllowSaveWithErrors":true,"EnableDuplicateChecking":false}' where DatastoreId = 10;
update datasets set Config = '{"DataEntryPage":{"HiddenFields":["Instrument","ActivityDate","BulkQaChange"]},"ActivitiesPage":{"ShowFields":["headerdata.YearReported","Location.Label"]},"AllowSaveWithErrors":true,"EnableDuplicateChecking":true,"DuplicateCheckFields":["LocationId","YearReported"],"SpecifyActivityListFields":true,"ActivityListFields":["LocationId","QAStatusId","YearReported"]}' where DatastoreId = 17;
update datasets set Config = '{"DataEntryPage":{"HiddenFields":["Instrument","BulkQaChange"]},"ActivitiesPage":{"ShowFields":["Description"]},"AllowSaveWithErrors":true,"EnableDuplicateChecking":false,"SpecifyActivityListFields":false}' where DatastoreId = 20;

go

--add metrics RVTouchstone field
ALTER TABLE [dbo].[Metrics_Detail] ADD [RVTouchstone] [nvarchar](max)

DECLARE @newrvtfield int = 0;

insert into Fields (Name, Description, DbColumnName, ControlType, DatastoreId, FieldRoleId, PossibleValues)
values ('RV Touchstone','Related River Vision Touchstone for this metric','RVTouchstone','text', 17, 2, '["Hydrology","Connectivity","Geomorphology","Aquatic Biota","Riparian Vegetation"]');

select @newrvtfield = scope_identity();

   -- add RVTouchstone field to all datasets that are metrics
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, @newrvtfield, 2, 'RV Touchstone','RVTouchstone',1,70,'select',null
from datasets where datastoreid in (17);

go

-- update rule for adult weir origin column (the function is in common-functions.js)
update datasetfields set [Rule] = '{"OnValidate": "validateOriginFinClip(row, row_errors)"}' where id in (1078, 1178, 1186)

go

-- update project configs per colette
update projects set config = '{"Lookups":[{"Id":"3","Label":"Instruments"}]}' where id in (1135,1140,1161,1188,1199,1200,1206,2232,2238,2248,2321,10018,11039, 2226);
update projects set config = '{"Lookups":[{"Id":"1","Label":"Fishermen","DatasetId":1275},{"Id":"2","Label":"Seasons","DatasetId":1276}]}' where id = 1217;
update projects set config = '{"Lookups":[{"Id":"8","Label":"Habitat","Type":"Metafields"},{"Id":"3","Label":"Instruments"}],"ShowHabitatSitesForDatasets":["UmHab-Metrics"]}' where id = 1223;
update projects set config = '{"Lookups":[{"Id":"8","Label":"Habitat","Type":"Metafields"},{"Id":"3","Label":"Instruments"}],"ShowHabitatSitesForDatasets":["NFJDHab-Metrics"]}' where id = 2223;
update projects set config = '{"Lookups":[{"Id":8,"Label":"Habitat Sites","Type":"Metafields"},{"Label":"Instruments"}],"ShowHabitatSitesForDatasets":["GrHab-Metrics"]}' where id = 2228;
update projects set config = '{"Lookups":[{"Id":"8","Label":"Habitat","Type":"Metafields"},{"Id":"3","Label":"Instruments"}],"ShowHabitatSitesForDatasets":["TcHab-Metrics"]}' where id = 2229;
update projects set config = '{"Lookups":[{"Id":"8","Label":"Habitat","Type":"Metafields"},{"Id":"3","Label":"Instruments"}],"ShowHabitatSitesForDatasets":["Biom-Benthic","Biom-Drift","BioM-Electrofishing","BioM-Snorkel"]}' where id = 2249;
update projects set config = '{  "Lookups": [    {      "Id": "8",      "Label": "Habitat",      "Type": "Metafields"    },    {      "Id": "3",      "Label": "Instruments"    }  ]}' where id in (11029, 11037);

go

update datasetfields set FieldRoleId = 1 where datasetid = 1005 and fieldid = 24; --fix technician dataset fieldroleid
go

CREATE TABLE [dbo].[PitagisDatas] (
    [Id] [int] NOT NULL IDENTITY,
    [MarkSubbasinCode] [int] NOT NULL,
    [MarkSubbasinSite] [nvarchar](max),
    [MarkYear] [int] NOT NULL,
    [MarkDate] [datetime] NOT NULL,
    [MarkMethodCode] [nvarchar](max),
    [ReleaseSubbasin] [int] NOT NULL,
    [ReleaseSubbasinSite] [nvarchar](max),
    [ReleaseSiteName] [nvarchar](max),
    [ReleaseDate] [datetime] NOT NULL,
    [MigrationYear] [int] NOT NULL,
    [PittagCode] [nvarchar](max),
    [SRRCode] [nvarchar](max),
    [SRRName] [nvarchar](max),
    [MarkLength] [int],
    [MarkWeight] [decimal](9, 2),
    [PassageSiteCode] [nvarchar](max),
    [PassageSiteName] [nvarchar](max),
    [FirstTimeValue] [datetime] NOT NULL,
    [LastTimeValue] [datetime] NOT NULL,
    [Count] [int] NOT NULL,
    [SpeciesCode] [int] NOT NULL,
    [SpeciesName] [nvarchar](max),
    [RunCode] [int],
    [RunName] [nvarchar](max),
    [RearTypeCode] [nvarchar](max),
    [RearTypeName] [nvarchar](max),
    CONSTRAINT [PK_dbo.PitagisDatas] PRIMARY KEY ([Id])
)

--Updates to fix Prod data, prior to time update
update dbo.ScrewTrap_Header set DepartTime = '10/20/16 14:20:00' where ActivityId = 57305
update dbo.ScrewTrap_Header set DepartTime = '10/17/16 09:10:00' where ActivityId = 57302
update dbo.ScrewTrap_Header set DepartTime = '12/10/16 11:05:00' where ActivityId = 57998
update dbo.ScrewTrap_Header set DepartTime = '01/13/17 11:45:00' where ActivityId = 58827
update dbo.ScrewTrap_Header set HubometerTime = '11/03/16 10:58:00' where ActivityId = 57514
update dbo.ScrewTrap_Header set HubometerTime = '05/31/17 11:58:00' where ActivityId = 65283
update dbo.ScrewTrap_Header set TrapStarted = '08/14/16 14:00:00' where ActivityId = 56985
update dbo.ScrewTrap_Header set FishCollected = '08/31/16 10:39:00' where ActivityId = 56981
update dbo.ScrewTrap_Header set FishCollected = '04/20/18 09:20' where ActivityId = 146877
update dbo.ScrewTrap_Header set FishReleased = '08/31/16 10:39:00' where ActivityId = 56981

--Update to change AdultWeir time control type from text to time.
update dbo.DatasetFields
set ControlType = 'time'
where DatasetId in
(
	SELECT Id
		  FROM dbo.[Datasets]
		  where Name like '%Adult Weir%'
)
and DbColumnName like '%Time%'
--6 records

--Update time fields in other datasets that have ControlType of text.
update dbo.DatasetFields
set ControlType = 'time'
where [Id] in (
	select [Id]--, DatasetId, DbColumnName, ControlType
	from dbo.DatasetFields
	where DbColumnName like '%Time%'
	and ControlType = 'text'
)
--15 records

update dbo.DatasetFields
set ControlType = 'time'
where 
ControlType = 'text'
and 
(DbColumnName = 'TrapStopped'
or DbColumnName = 'TrapStarted'
or DbColumnName = 'FishCollected'
or DbColumnName = 'FishReleased'
)
--4 records

update dbo.SpawningGroundSurvey_Header set StartTime = '10:23' where ActivityId = 49337
update SpawningGroundSurvey_Detail set Time = '13:23' where Id = 308
update dbo.ScrewTrap_Header set TrapStopped = null where TrapStopped like '%NA%' --16 Records
update dbo.ScrewTrap_Header set TrapStarted = null where TrapStarted like '%NA%' --16 Records


--The following scripts will convert the time fields from whatever they are to utc.
declare @theMonth as nvarchar(max);
declare @theDay as nvarchar(max);
declare @theStartHour as nvarchar(max);
declare @theStartMinute as nvarchar(max);
declare @theStopHour as nvarchar(max);
declare @theStopMinute as nvarchar(max);

--Good
-- AdultWeir TimeStart, ISK-AdultWeir
raiserror(N'Updating AdultWeir TimeStart...', 0, 1) with nowait
update dbo.AdultWeir_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.hTimeStart, 0, CHARINDEX(':', r.hTimeStart))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.hTimeStart, CHARINDEX(':', r.hTimeStart)+1, LEN(r.hTimeStart))), 2),
TimeStart = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TimeStart as hTimeStart, h.TimeEnd as hTimeEnd
from dbo.Activities a
inner join dbo.AdultWeir_Header h on h.ActivityId = a.Id
--where h.TimeStart is not null and a.DatasetId = 1004 --and a.Id = 146924
where h.TimeStart is not null
) as r
where ActivityId = r.aId and TimeStart is not null;
--686 records expected

--Good
-- AdultWeir TimeEnd, ISK-AdultWeir
raiserror(N'Updating AdultWeir TimeEnd...', 0, 1) with nowait
update dbo.AdultWeir_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStopHour = right(concat('00', SUBSTRING(r.hTimeEnd, 0, CHARINDEX(':', r.hTimeEnd))), 2),
@theStopMinute = right(concat('00', SUBSTRING(r.hTimeEnd, CHARINDEX(':', r.hTimeEnd)+1, LEN(r.hTimeEnd))), 2),
TimeEnd = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStopHour, ':', @theStopMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TimeStart as hTimeStart, h.TimeEnd as hTimeEnd
from dbo.Activities a
inner join dbo.AdultWeir_Header h on h.ActivityId = a.Id
--where h.TimeStart is not null and a.DatasetId = 1004 --and a.Id = 146924
where h.TimeEnd is not null
) as r
where ActivityId = r.aId and TimeEnd is not null
--0 records expected

--Good
--ScrewTrap, UMME, ArrivalTime, have date and time, but not UTC (has a T in it)
-- in mm/dd/yy HH:mm
-- in mm/dd/yy HH:mm:ss
-- in mmm dd yyyy h:mmAM
raiserror(N'Updating ScrewTrap ArrivalTime, have date and time, but not UTC (has a T in it)...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', datepart(hour, r.ArrivalTime)), 2),
@theStartMinute = right(concat('00', datepart(minute, r.ArrivalTime)), 2),
ArrivalTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.ArrivalTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.ArrivalTime is not null and a.DatasetId = 1215 and len(h.ArrivalTime) > 5 and len(h.ArrivalTime) != 8
where h.ArrivalTime is not null and len(h.ArrivalTime) > 5 and len(h.ArrivalTime) != 8
and charIndex('T', h.ArrivalTime) = 0
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.ArrivalTime is not null and len(dbo.ScrewTrap_Header.ArrivalTime) > 5 and len(dbo.ScrewTrap_Header.ArrivalTime) != 8
and charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) = 0
--3228 records expected

--Good
--ScrewTrap, UMME, DepartTime, have date and time, but not UTC (has a T in it)
-- in mm/dd/yy HH:mm
-- in mm/dd/yy HH:mm:ss
-- in mmm dd yyyy h:mmAM
raiserror(N'Updating ScrewTrap DepartTime, have date and time, but not UTC (has a T in it)...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', datepart(hour, r.DepartTime)), 2),
@theStartMinute = right(concat('00', datepart(minute, r.DepartTime)), 2),
DepartTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.DepartTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.DepartTime is not null and a.DatasetId = 1215 and len(h.DepartTime) > 5 and len(h.DepartTime) != 8
where h.DepartTime is not null and len(h.DepartTime) > 5 and len(h.DepartTime) != 8
and charIndex('T', h.DepartTime) = 0
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.DepartTime is not null and len(dbo.ScrewTrap_Header.DepartTime) > 5 and len(dbo.ScrewTrap_Header.DepartTime) != 8
and charIndex('T', dbo.ScrewTrap_Header.DepartTime) = 0
--1491 records expected

--Good
--ScrewTrap, UMME, HubometerTime, have date and time, but not UTC (has a T in it)
-- in mm/dd/yy HH:mm
-- in mm/dd/yy HH:mm:ss
-- in mmm dd yyyy h:mmAM
raiserror(N'Updating ScrewTrap HubometerTime, have date and time, but not UTC (has a T in it)...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', datepart(hour, r.HubometerTime)), 2),
@theStartMinute = right(concat('00', datepart(minute, r.HubometerTime)), 2),
HubometerTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.HubometerTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.HubometerTime is not null and a.DatasetId = 1215 and len(h.HubometerTime) > 5 and len(h.HubometerTime) != 8
where h.HubometerTime is not null and len(h.HubometerTime) > 5 and len(h.HubometerTime) != 8
and charIndex('T', h.HubometerTime) = 0
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.HubometerTime is not null and len(dbo.ScrewTrap_Header.HubometerTime) > 5 and len(dbo.ScrewTrap_Header.HubometerTime) != 8
and charIndex('T', dbo.ScrewTrap_Header.HubometerTime) = 0
--1425 records expected

--Good
--ScrewTrap, UMME, TrapStopped, have date and time, but not UTC (has a T in it)
-- in mm/dd/yy HH:mm
-- in mm/dd/yy HH:mm:ss
-- in mmm dd yyyy h:mmAM
raiserror(N'Updating ScrewTrap TrapStopped, have date and time, but not UTC (has a T in it)...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', datepart(hour, r.TrapStopped)), 2),
@theStartMinute = right(concat('00', datepart(minute, r.TrapStopped)), 2),
TrapStopped = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStopped
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.TrapStopped is not null and a.DatasetId = 1215 and len(h.TrapStopped) > 5 and len(h.TrapStopped) != 8
where h.TrapStopped is not null and len(h.TrapStopped) > 5 and len(h.TrapStopped) != 8
and charIndex('T', h.TrapStopped) = 0
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStopped is not null and len(dbo.ScrewTrap_Header.TrapStopped) > 5 and len(dbo.ScrewTrap_Header.TrapStopped) != 8
and charIndex('T', dbo.ScrewTrap_Header.TrapStopped) = 0
--165 records expected

--Good
--ScrewTrap, UMME, TrapStarted, have date and time, but not UTC (has a T in it)
-- in mm/dd/yy HH:mm
-- in mm/dd/yy HH:mm:ss
-- in mmm dd yyyy h:mmAM
raiserror(N'Updating ScrewTrap TrapStarted, have date and time, but not UTC (has a T in it)...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', datepart(hour, r.TrapStarted)), 2),
@theStartMinute = right(concat('00', datepart(minute, r.TrapStarted)), 2),
TrapStarted = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStarted
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.TrapStarted is not null and a.DatasetId = 1215 and len(h.TrapStarted) > 5 and len(h.TrapStarted) != 8
where h.TrapStarted is not null and len(h.TrapStarted) > 5 and len(h.TrapStarted) != 8
and charIndex('T', h.TrapStarted) = 0
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStarted is not null and len(dbo.ScrewTrap_Header.TrapStarted) > 5 and len(dbo.ScrewTrap_Header.TrapStarted) != 8
and charIndex('T', dbo.ScrewTrap_Header.TrapStarted) = 0
--182 records expected

-- Good
--ScrewTrap, UMME, FishCollected, have date and time, but not UTC (has a T in it)
-- in mm/dd/yy HH:mm
-- in mm/dd/yy HH:mm:ss
-- in mmm dd yyyy h:mmAM
raiserror(N'Updating ScrewTrap FishCollected, have date and time, but not UTC (has a T in it)...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', datepart(hour, r.FishCollected)), 2),
@theStartMinute = right(concat('00', datepart(minute, r.FishCollected)), 2),
FishCollected = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.FishCollected
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.FishCollected is not null and a.DatasetId = 1215 and len(h.FishCollected) > 5 and len(h.FishCollected) != 8
where h.FishCollected is not null and len(h.FishCollected) > 5 and len(h.FishCollected) != 8
and charIndex('T', h.FishCollected) = 0
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.FishCollected is not null and len(dbo.ScrewTrap_Header.FishCollected) > 5 and len(dbo.ScrewTrap_Header.FishCollected) != 8
and charIndex('T', dbo.ScrewTrap_Header.FishCollected) = 0
--1286 records expected

--Good
--ScrewTrap, UMME, FishReleased, have date and time, but not UTC (has a T in it)
-- in mm/dd/yy HH:mm
-- in mm/dd/yy HH:mm:ss
-- in mmm dd yyyy h:mmAM
raiserror(N'Updating ScrewTrap FishReleased, have date and time, but not UTC (has a T in it)...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', datepart(hour, r.FishReleased)), 2),
@theStartMinute = right(concat('00', datepart(minute, r.FishReleased)), 2),
FishReleased = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.FishReleased
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.FishReleased is not null and a.DatasetId = 1215 and len(h.FishReleased) > 5 and len(h.FishReleased) != 8
where h.FishReleased is not null and len(h.FishReleased) > 5 and len(h.FishReleased) != 8
and charIndex('T', h.FishReleased) = 0
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.FishReleased is not null and len(dbo.ScrewTrap_Header.FishReleased) > 5 and len(dbo.ScrewTrap_Header.FishReleased) != 8
and charIndex('T', dbo.ScrewTrap_Header.FishReleased) = 0
--1341 records expected

-- Good
--ScrewTrap, ArrivalTime, have just time
raiserror(N'Updating ScrewTrap ArrivalTime...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.ArrivalTime, 0, CHARINDEX(':', r.ArrivalTime))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.ArrivalTime, CHARINDEX(':', r.ArrivalTime)+1, LEN(r.ArrivalTime))), 2),
ArrivalTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.ArrivalTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.ArrivalTime is not null and a.DatasetId = 1215 and len(h.ArrivalTime < 6) --and a.Id = 146924
where h.ArrivalTime is not null and len(h.ArrivalTime) < 6 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.ArrivalTime is not null and len(dbo.ScrewTrap_Header.ArrivalTime) < 6
--3045 records expected

-- Good
--ScrewTrap, DepartTime, have just time
raiserror(N'Updating ScrewTrap DepartTime, those records with just a time...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.DepartTime, 0, CHARINDEX(':', r.DepartTime))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.DepartTime, CHARINDEX(':', r.DepartTime)+1, LEN(r.DepartTime))), 2),
DepartTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.DepartTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.DepartTime is not null and a.DatasetId = 1215 and len(h.DepartTime < 6) --and a.Id = 146924
where h.DepartTime is not null and len(h.DepartTime) < 6 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.DepartTime is not null and len(dbo.ScrewTrap_Header.DepartTime) < 6
--2773 records expected

-- Good
--ScrewTrap, UMME, HubometerTime, have just time
raiserror(N'Updating ScrewTrap HubometerTime, those records with just a time...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.HubometerTime, 0, CHARINDEX(':', r.HubometerTime))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.HubometerTime, CHARINDEX(':', r.HubometerTime)+1, LEN(r.HubometerTime))), 2),
HubometerTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.HubometerTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.HubometerTime is not null and a.DatasetId = 1215 and len(h.HubometerTime < 6) --and a.Id = 146924
where h.HubometerTime is not null and len(h.HubometerTime) < 6 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.HubometerTime is not null and len(dbo.ScrewTrap_Header.HubometerTime) < 6
--2441 records expected

--Good
--ScrewTrap, UMME, FishCollected, have just time
raiserror(N'Updating ScrewTrap FishCollected, those records with just a time...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.FishCollected, 0, CHARINDEX(':', r.FishCollected))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.FishCollected, CHARINDEX(':', r.FishCollected)+1, LEN(r.FishCollected))), 2),
FishCollected = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.FishCollected
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.FishCollected is not null and a.DatasetId = 1215 and len(h.FishCollected < 6) --and a.Id = 146924
where h.FishCollected is not null and len(h.FishCollected) < 6 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.FishCollected is not null and len(dbo.ScrewTrap_Header.FishCollected) < 6
--2368 records expected

--Good
--ScrewTrap, UMME, FishReleased, have just time
raiserror(N'Updating ScrewTrap FishReleased, those records with just a time...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.FishReleased, 0, CHARINDEX(':', r.FishReleased))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.FishReleased, CHARINDEX(':', r.FishReleased)+1, LEN(r.FishReleased))), 2),
FishReleased = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.FishReleased
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.FishReleased is not null and a.DatasetId = 1215 and len(h.FishReleased < 6) --and a.Id = 146924
where h.FishReleased is not null and len(h.FishReleased) < 6 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.FishReleased is not null and len(dbo.ScrewTrap_Header.FishReleased) < 6
--2807 records expected

--Good
--ScrewTrap, UMME, TrapStarted, have just time
raiserror(N'Updating ScrewTrap TrapStarted, those records with just a time...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.TrapStarted, 0, CHARINDEX(':', r.TrapStarted))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.TrapStarted, CHARINDEX(':', r.TrapStarted)+1, LEN(r.TrapStarted))), 2),
TrapStarted = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStarted
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.TrapStarted is not null and a.DatasetId = 1215 and len(h.TrapStarted < 6) --and a.Id = 146924
where h.TrapStarted is not null and len(h.TrapStarted) < 6 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStarted is not null and  len(dbo.ScrewTrap_Header.TrapStarted) < 6
--1472 records expected

--Good
--ScrewTrap, UMME, ArrivalTime, have HH.mm.ss format (8-chars)
raiserror(N'Updating ScrewTrap ArrivalTime, those records with HH.mm.ss format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.aActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.aActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.aArrivalTime, 0, CHARINDEX(':', r.aArrivalTime))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.aArrivalTime, CHARINDEX(':', r.aArrivalTime)+1, 2)), 2),
ArrivalTime = concat(YEAR(r.aActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate as aActivityDate, h.ArrivalTime as aArrivalTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.[ArrivalTime is not null and a.DatasetId = 1215 and len(h.ArrivalTime) = 8 --and a.Id = 146924
where h.ArrivalTime is not null and len(h.ArrivalTime) = 8 --and a.Id = 146924
) as r
where ActivityId = r.aId and ArrivalTime is not null and len(ArrivalTime) = 8
--7 records expected

--Good
--ScrewTrap, UMME, DepartTime, have HH.mm.ss format (8-chars)
raiserror(N'Updating ScrewTrap DepartTime, those records with HH.mm.ss format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.aActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.aActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.DepartTime, 0, CHARINDEX(':', r.DepartTime))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.DepartTime, CHARINDEX(':', r.DepartTime)+1, 2)), 2),
DepartTime = concat(YEAR(r.aActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate as aActivityDate, h.DepartTime as DepartTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.DepartTime is not null and a.DatasetId = 1215 and len(h.DepartTime) = 8 --and a.Id = 146924
where h.DepartTime is not null and len(h.DepartTime) = 8 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.DepartTime is not null and len(dbo.ScrewTrap_Header.DepartTime) = 8
--6 records expected

--Good
--ScrewTrap, UMME, HubometerTime, have HH.mm.ss format (8-chars)
raiserror(N'Updating ScrewTrap HubometerTime, those records with HH.mm.ss format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.aActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.aActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.HubometerTime, 0, CHARINDEX(':', r.HubometerTime))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.HubometerTime, CHARINDEX(':', r.HubometerTime)+1, 2)), 2),
HubometerTime = concat(YEAR(r.aActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate as aActivityDate, h.HubometerTime as HubometerTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.HubometerTime is not null and a.DatasetId = 1215 and len(h.HubometerTime) = 8 --and a.Id = 146924
where h.HubometerTime is not null and len(h.HubometerTime) = 8 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.HubometerTime is not null and len(dbo.ScrewTrap_Header.HubometerTime) = 8
--6 records expected

--Good
--ScrewTrap, UMME, TrapStarted, have HH.mm.ss format (8-chars)
raiserror(N'Updating ScrewTrap TrapStarted, those records with HH.mm.ss format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.aActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.aActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.TrapStarted, 0, CHARINDEX(':', r.TrapStarted))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.TrapStarted, CHARINDEX(':', r.TrapStarted)+1, 2)), 2),
TrapStarted = concat(YEAR(r.aActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate as aActivityDate, h.TrapStarted as TrapStarted
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.TrapStarted is not null and a.DatasetId = 1215 and len(h.TrapStarted) = 8 --and a.Id = 146924
where h.TrapStarted is not null and len(h.TrapStarted) = 8 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStarted is not null and len(dbo.ScrewTrap_Header.TrapStarted) = 8
--1 record expected

--Good
--ScrewTrap, UMME, FishCollected, have HH.mm.ss format (8-chars)
raiserror(N'Updating ScrewTrap FishCollected, those records with HH.mm.ss format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.aActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.aActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.FishCollected, 0, CHARINDEX(':', r.FishCollected))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.FishCollected, CHARINDEX(':', r.FishCollected)+1, 2)), 2),
FishCollected = concat(YEAR(r.aActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate as aActivityDate, h.FishCollected as FishCollected
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.FishCollected is not null and a.DatasetId = 1215 and len(h.FishCollected) = 8 --and a.Id = 146924
where h.FishCollected is not null and len(h.FishCollected) = 8 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.FishCollected is not null and len(dbo.ScrewTrap_Header.FishCollected) = 8
--7 records expected

--Good
--ScrewTrap, UMME, FishReleased, have HH.mm.ss format (8-chars)
raiserror(N'Updating ScrewTrap FishReleased, those records with HH.mm.ss format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.aActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.aActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.FishReleased, 0, CHARINDEX(':', r.FishReleased))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.FishReleased, CHARINDEX(':', r.FishReleased)+1, 2)), 2),
FishReleased = concat(YEAR(r.aActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate as aActivityDate, h.FishReleased as FishReleased
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.FishReleased is not null and a.DatasetId = 1215 and len(h.FishReleased) = 8 --and a.Id = 146924
where h.FishReleased is not null and len(h.FishReleased) = 8 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.FishReleased is not null and len(dbo.ScrewTrap_Header.FishReleased) = 8
--9 records

--Good
--ScrewTrap, UMME, TrapStopped, have HH.mm.ss format (8-chars)
raiserror(N'Updating ScrewTrap TrapStopped, those records with HH.mm.ss format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.aActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.aActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.TrapStopped, 0, CHARINDEX(':', r.TrapStopped))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.TrapStopped, CHARINDEX(':', r.TrapStopped)+1, 2)), 2),
TrapStopped = concat(YEAR(r.aActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate as aActivityDate, h.TrapStopped as TrapStopped
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.TrapStopped is not null and a.DatasetId = 1215 and len(h.TrapStopped) = 8 --and a.Id = 146924
where h.TrapStopped is not null and len(h.TrapStopped) = 8 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStopped is not null and len(dbo.ScrewTrap_Header.TrapStopped) = 8
--0 records expected

--Good
--ScrewTrap, UMME, ArrivalTime, have YYYY-MM-DD HH:mm, or THH:mm formats
raiserror(N'Updating ScrewTrap ArrivalTime, have YYYY-MM-DD HH:mm, or THH:mm formats...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.ArrivalTime, 
		charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) + 1, -- as PosOfHour,
		--charIndex(':', dbo.ScrewTrap_Header.ArrivalTime) as PosOfColon,-- - charIndex('T', h.ArrivalTime + 1)),
		((charIndex(':', dbo.ScrewTrap_Header.ArrivalTime)) - (charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.ArrivalTime, 
		charIndex(':', dbo.ScrewTrap_Header.ArrivalTime) + 1,
		2)), 
	2),
ArrivalTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.ArrivalTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.ArrivalTime is not null 
and len(h.ArrivalTime) > 6
and charIndex('T', h.ArrivalTime) > 0
and h.ArrivalTime not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.ArrivalTime is not null 
and len(dbo.ScrewTrap_Header.ArrivalTime) > 6
and charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) > 0
and dbo.ScrewTrap_Header.ArrivalTime not like '%null-%'
--3233 records

--Good
--ScrewTrap, UMME, DepartTime, have YYYY-MM-DD HH:mm, or THH:mm formats
raiserror(N'Updating ScrewTrap DepartTime, have YYYY-MM-DD HH:mm, or THH:mm formats...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.DepartTime, 
		charIndex('T', dbo.ScrewTrap_Header.DepartTime) + 1, -- as PosOfHour,
		--charIndex(':', h.DepartTime) as PosOfColon,-- - charIndex('T', h.DepartTime + 1)),
		((charIndex(':', dbo.ScrewTrap_Header.DepartTime)) - (charIndex('T', dbo.ScrewTrap_Header.DepartTime) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.DepartTime, 
		charIndex(':', dbo.ScrewTrap_Header.DepartTime) + 1,
		2)), 
	2),
DepartTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.DepartTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.DepartTime is not null 
and len(h.DepartTime) > 6
and charIndex('T', h.DepartTime) > 0
and h.DepartTime not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.DepartTime is not null 
and len(dbo.ScrewTrap_Header.DepartTime) > 6
and charIndex('T', dbo.ScrewTrap_Header.DepartTime) > 0
and dbo.ScrewTrap_Header.DepartTime not like '%null-%'
--1488

--Good
--ScrewTrap, UMME, HubometerTime, have YYYY-MM-DD HH:mm, or THH:mm formats
raiserror(N'Updating ScrewTrap HubometerTime, have YYYY-MM-DD HH:mm, or THH:mm formats...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.HubometerTime, 
		charIndex('T', dbo.ScrewTrap_Header.HubometerTime) + 1, -- as PosOfHour,
		--charIndex(':', dbo.ScrewTrap_Header.HubometerTime) as PosOfColon,-- - charIndex('T', dbo.ScrewTrap_Header.HubometerTime + 1)),
		((charIndex(':', dbo.ScrewTrap_Header.HubometerTime)) - (charIndex('T', dbo.ScrewTrap_Header.HubometerTime) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.HubometerTime, 
		charIndex(':', dbo.ScrewTrap_Header.HubometerTime) + 1,
		2)), 
	2),
HubometerTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.HubometerTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.HubometerTime is not null and a.DatasetId = 1215 and len(h.HubometerTime) > 5 and len(h.HubometerTime) != 8
where h.HubometerTime is not null and len(h.HubometerTime) > 5 and len(h.HubometerTime) != 8
and charIndex('T', h.HubometerTime) = 0
and h.HubometerTime not like '%null-%'
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.HubometerTime is not null and len(dbo.ScrewTrap_Header.HubometerTime) > 5 and len(dbo.ScrewTrap_Header.HubometerTime) != 8
and charIndex('T', dbo.ScrewTrap_Header.HubometerTime) = 0
and dbo.ScrewTrap_Header.HubometerTime not like '%null-%'
--1425 records expected

--Good
--ScrewTrap, UMME, TrapStopped, have YYYY-MM-DD HH:mm, or THH:mm formats
raiserror(N'Updating ScrewTrap TrapStopped, have YYYY-MM-DD HH:mm, or THH:mm formats...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		charIndex('T', dbo.ScrewTrap_Header.TrapStopped) + 1, -- as PosOfHour,
		--charIndex(':', h.TrapStopped) as PosOfColon,-- - charIndex('T', h.TrapStopped + 1)),
		((charIndex(':', dbo.ScrewTrap_Header.TrapStopped)) - (charIndex('T', dbo.ScrewTrap_Header.TrapStopped) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStopped) + 1,
		2)), 
	2),
TrapStopped = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStopped
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.TrapStopped is not null and a.DatasetId = 1215 and len(h.TrapStopped) > 5 and len(h.TrapStopped) != 8
where h.TrapStopped is not null and len(h.TrapStopped) > 5 and len(h.TrapStopped) != 8
and charIndex('T', h.TrapStopped) = 0
and h.TrapStopped not like '%null-%'
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStopped is not null and len(dbo.ScrewTrap_Header.TrapStopped) > 5 and len(dbo.ScrewTrap_Header.TrapStopped) != 8
and charIndex('T', dbo.ScrewTrap_Header.TrapStopped) = 0
and dbo.ScrewTrap_Header.TrapStopped not like '%null-%'
--165 records expected

--Good
--ScrewTrap, UMME, TrapStarted, have YYYY-MM-DD HH:mm, or THH:mm formats
raiserror(N'Updating ScrewTrap TrapStarted, have YYYY-MM-DD HH:mm, or THH:mm formats...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', + 
	substring(
		dbo.ScrewTrap_Header.TrapStarted, 
		charIndex('T', dbo.ScrewTrap_Header.TrapStarted) + 1, -- as PosOfHour,
		--charIndex(':', h.TrapStarted) as PosOfColon,-- - charIndex('T', h.TrapStarted + 1)),
		((charIndex(':', dbo.ScrewTrap_Header.TrapStarted)) - (charIndex('T', dbo.ScrewTrap_Header.TrapStarted) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStarted, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStarted) + 1,
		2)), 
	2),
TrapStarted = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStarted
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.TrapStarted is not null and a.DatasetId = 1215 and len(h.TrapStarted) > 5 and len(h.TrapStarted) != 8
where h.TrapStarted is not null and len(h.TrapStarted) > 5 and len(h.TrapStarted) != 8
and charIndex('T', h.TrapStarted) = 0
and h.TrapStarted not like '%null-%'
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStarted is not null and len(dbo.ScrewTrap_Header.TrapStarted) > 5 and len(dbo.ScrewTrap_Header.TrapStarted) != 8
and charIndex('T', dbo.ScrewTrap_Header.TrapStarted) = 0
and dbo.ScrewTrap_Header.TrapStarted not like '%null-%'
--182 records expected

--Good
--ScrewTrap, UMME, FishCollected, have YYYY-MM-DD HH:mm, or THH:mm formats
raiserror(N'Updating ScrewTrap FishCollected, have YYYY-MM-DD HH:mm, or THH:mm formats...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.FishCollected, 
		charIndex('T', dbo.ScrewTrap_Header.FishCollected) + 1, -- as PosOfHour,
		--charIndex(':', h.FishCollected) as PosOfColon,-- - charIndex('T', h.FishCollected + 1)),
		((charIndex(':', dbo.ScrewTrap_Header.FishCollected)) - (charIndex('T', dbo.ScrewTrap_Header.FishCollected) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.FishCollected, 
		charIndex(':', dbo.ScrewTrap_Header.FishCollected) + 1,
		2)), 
	2),
FishCollected = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.FishCollected
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.FishCollected is not null and a.DatasetId = 1215 and len(h.FishCollected) > 5 and len(h.FishCollected) != 8
where h.FishCollected is not null and len(h.FishCollected) > 5 and len(h.FishCollected) != 8
and charIndex('T', h.FishCollected) = 0
and h.FishCollected not like '%null-%'
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.FishCollected is not null and len(dbo.ScrewTrap_Header.FishCollected) > 5 and len(dbo.ScrewTrap_Header.FishCollected) != 8
and charIndex('T', dbo.ScrewTrap_Header.FishCollected) = 0
and dbo.ScrewTrap_Header.FishCollected not like '%null-%'
--1286 records

--Good
--ScrewTrap, UMME, FishReleased, have YYYY-MM-DD HH:mm, or THH:mm formats
raiserror(N'Updating ScrewTrap FishReleased, have YYYY-MM-DD HH:mm, or THH:mm formats...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.FishReleased, 
		charIndex('T', dbo.ScrewTrap_Header.FishReleased) + 1, -- as PosOfHour,
		--charIndex(':', h.FishReleased) as PosOfColon,-- - charIndex('T', h.FishReleased + 1)),
		((charIndex(':', dbo.ScrewTrap_Header.FishReleased)) - (charIndex('T', dbo.ScrewTrap_Header.FishReleased) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.FishReleased, 
		charIndex(':', dbo.ScrewTrap_Header.FishReleased) + 1,
		2)), 
	2),
FishReleased = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.FishReleased
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.FishReleased is not null and a.DatasetId = 1215 and len(h.FishReleased) > 5 and len(h.FishReleased) != 8
where h.FishReleased is not null and len(h.FishReleased) > 5 and len(h.FishReleased) != 8
and charIndex('T', h.FishReleased) = 0
and h.FishReleased not like '%null-%'
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.FishReleased is not null and len(dbo.ScrewTrap_Header.FishReleased) > 5 and len(dbo.ScrewTrap_Header.FishReleased) != 8
and charIndex('T', dbo.ScrewTrap_Header.FishReleased) = 0
and dbo.ScrewTrap_Header.FishReleased not like '%null-%'
--1341 records expected


raiserror(N'Updating ScrewTrap ArrivalTime, have YYYY-MM-DDTHH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.ArrivalTime, 
		charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) + 1, -- as PosOfHour,
		((charIndex(':', dbo.ScrewTrap_Header.ArrivalTime)) - (charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.ArrivalTime, 
		charIndex(':', dbo.ScrewTrap_Header.ArrivalTime) + 1,
		2)), 
	2),
ArrivalTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.ArrivalTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.ArrivalTime is not null 
and len(h.ArrivalTime) > 6
and charIndex('T', h.ArrivalTime) > 0
and h.ArrivalTime not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.ArrivalTime is not null 
and len(dbo.ScrewTrap_Header.ArrivalTime) > 6
and charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) > 0
and dbo.ScrewTrap_Header.ArrivalTime not like '%null-%'


raiserror(N'Updating ScrewTrap DepartTime, have YYYY-MM-DDTHH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.DepartTime, 
		charIndex('T', dbo.ScrewTrap_Header.DepartTime) + 1, -- as PosOfHour,
		--charIndex(':', h.DepartTime) as PosOfColon,-- - charIndex('T', h.DepartTime + 1)),
		((charIndex(':', dbo.ScrewTrap_Header.DepartTime)) - (charIndex('T', dbo.ScrewTrap_Header.DepartTime) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.DepartTime, 
		charIndex(':', dbo.ScrewTrap_Header.DepartTime) + 1,
		2)), 
	2),
DepartTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.DepartTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.DepartTime is not null 
and len(h.DepartTime) > 6
and charIndex('T', h.DepartTime) > 0
and h.DepartTime not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.DepartTime is not null 
and len(dbo.ScrewTrap_Header.DepartTime) > 6
and charIndex('T', dbo.ScrewTrap_Header.DepartTime) > 0
and dbo.ScrewTrap_Header.DepartTime not like '%null-%'


raiserror(N'Updating ScrewTrap HubometerTime, have YYYY-MM-DDTHH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.HubometerTime, 
		charIndex('T', dbo.ScrewTrap_Header.HubometerTime) + 1, -- as PosOfHour,
		((charIndex(':', dbo.ScrewTrap_Header.HubometerTime)) - (charIndex('T', dbo.ScrewTrap_Header.HubometerTime) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.HubometerTime, 
		charIndex(':', dbo.ScrewTrap_Header.HubometerTime) + 1,
		2)), 
	2),
HubometerTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.HubometerTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.HubometerTime is not null 
and len(h.HubometerTime) > 6
and charIndex('T', h.HubometerTime) > 0
and h.HubometerTime not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.HubometerTime is not null 
and len(dbo.ScrewTrap_Header.HubometerTime) > 6
and charIndex('T', dbo.ScrewTrap_Header.HubometerTime) > 0
and dbo.ScrewTrap_Header.HubometerTime not like '%null-%'


raiserror(N'Updating ScrewTrap TrapStopped, have YYYY-MM-DDTHH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		charIndex('T', dbo.ScrewTrap_Header.TrapStopped) + 1, -- as PosOfHour,
		((charIndex(':', dbo.ScrewTrap_Header.TrapStopped)) - (charIndex('T', dbo.ScrewTrap_Header.TrapStopped) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStopped) + 1,
		2)), 
	2),
TrapStopped = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStopped
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.TrapStopped is not null 
and len(h.TrapStopped) > 6
and charIndex('T', h.TrapStopped) > 0
and h.TrapStopped not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.TrapStopped is not null 
and len(dbo.ScrewTrap_Header.TrapStopped) > 6
and charIndex('T', dbo.ScrewTrap_Header.TrapStopped) > 0
and dbo.ScrewTrap_Header.TrapStopped not like '%null-%'


raiserror(N'Updating ScrewTrap TrapStarted, have YYYY-MM-DDTHH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.TrapStarted, 
		charIndex('T', dbo.ScrewTrap_Header.TrapStarted) + 1, -- as PosOfHour,
		((charIndex(':', dbo.ScrewTrap_Header.TrapStarted)) - (charIndex('T', dbo.ScrewTrap_Header.TrapStarted) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStarted, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStarted) + 1,
		2)), 
	2),
TrapStarted = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStarted
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.TrapStarted is not null 
and len(h.TrapStarted) > 6
and charIndex('T', h.TrapStarted) > 0
and h.TrapStarted not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.TrapStarted is not null 
and len(dbo.ScrewTrap_Header.TrapStarted) > 6
and charIndex('T', dbo.ScrewTrap_Header.TrapStarted) > 0
and dbo.ScrewTrap_Header.TrapStarted not like '%null-%'


raiserror(N'Updating ScrewTrap FishCollected, have YYYY-MM-DDTHH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.FishCollected, 
		charIndex('T', dbo.ScrewTrap_Header.FishCollected) + 1, -- as PosOfHour,
		((charIndex(':', dbo.ScrewTrap_Header.FishCollected)) - (charIndex('T', dbo.ScrewTrap_Header.FishCollected) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.FishCollected, 
		charIndex(':', dbo.ScrewTrap_Header.FishCollected) + 1,
		2)), 
	2),
FishCollected = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.FishCollected
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.FishCollected is not null 
and len(h.FishCollected) > 6
and charIndex('T', h.FishCollected) > 0
and h.FishCollected not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.FishCollected is not null 
and len(dbo.ScrewTrap_Header.FishCollected) > 6
and charIndex('T', dbo.ScrewTrap_Header.FishCollected) > 0
and dbo.ScrewTrap_Header.FishCollected not like '%null-%'


raiserror(N'Updating ScrewTrap FishReleased, have YYYY-MM-DDTHH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.FishReleased, 
		charIndex('T', dbo.ScrewTrap_Header.FishReleased) + 1, -- as PosOfHour,
		((charIndex(':', dbo.ScrewTrap_Header.FishReleased)) - (charIndex('T', dbo.ScrewTrap_Header.FishReleased) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.FishReleased, 
		charIndex(':', dbo.ScrewTrap_Header.FishReleased) + 1,
		2)), 
	2),
FishReleased = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.FishReleased
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.FishReleased is not null 
and len(h.FishReleased) > 6
and charIndex('T', h.FishReleased) > 0
and h.FishReleased not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.FishReleased is not null 
and len(dbo.ScrewTrap_Header.FishReleased) > 6
and charIndex('T', dbo.ScrewTrap_Header.FishReleased) > 0
and dbo.ScrewTrap_Header.FishReleased not like '%null-%'


raiserror(N'Updating ScrewTrap ArrivalTime, has THH:mm...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.ArrivalTime, 
		charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) + 1, -- as PosOfHour,
		((charIndex(':', dbo.ScrewTrap_Header.ArrivalTime)) - (charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.ArrivalTime, 
		charIndex(':', dbo.ScrewTrap_Header.ArrivalTime) + 1,
		2)), 
	2),
ArrivalTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, a.DatasetId, h.ArrivalTime, h.DepartTime, h.HubometerTime, h.TrapStopped, h.TrapStarted, h.FishCollected, h.FishReleased
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.ArrivalTime is not null
and len(h.ArrivalTime) <= 7
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.ArrivalTime is not null 
and len(dbo.ScrewTrap_Header.ArrivalTime) <= 7


raiserror(N'Updating ScrewTrap TrapStopped, has HH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		charIndex('T', dbo.ScrewTrap_Header.TrapStopped) + 1, -- as PosOfHour,
		((charIndex(':', dbo.ScrewTrap_Header.TrapStopped)) - (charIndex('T', dbo.ScrewTrap_Header.TrapStopped) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStopped) + 1,
		2)), 
	2),
TrapStopped = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, a.DatasetId, h.ArrivalTime, h.DepartTime, h.HubometerTime, h.TrapStopped, h.TrapStarted, h.FishCollected, h.FishReleased
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.TrapStopped is not null
and h.TrapStopped != 'NA'
and len(h.TrapStopped) < 23
and len(h.TrapStopped) > 4
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.TrapStopped is not null 
and dbo.ScrewTrap_Header.TrapStopped != 'NA'
and len(dbo.ScrewTrap_Header.TrapStopped) < 23
and len(dbo.ScrewTrap_Header.TrapStopped) > 4

raiserror(N'Updating ScrewTrap TrapStopped, H:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		1, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStopped) - 1
		)),
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStopped) + 1,
		2)), 
	2),
TrapStopped = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStopped
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.TrapStopped is not null and len(h.TrapStopped) < 5
and h.TrapStopped != 'NA'
and len(h.TrapStopped) > 0
) as r
where dbo.ScrewTrap_Header.ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStopped is not null and len(dbo.ScrewTrap_Header.TrapStopped) < 5
and dbo.ScrewTrap_Header.TrapStopped != 'NA'
and len(dbo.ScrewTrap_Header.TrapStopped) > 0


raiserror(N'Updating ScrewTrap TrapStopped, HH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		1, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStopped) - 1
		)),
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStopped) + 1,
		2)), 
	2),
TrapStopped = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStopped
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.TrapStopped is not null and len(h.TrapStopped) < 23
and h.TrapStopped != 'NA'
and len(h.TrapStopped) > 0
) as r
where dbo.ScrewTrap_Header.ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStopped is not null and len(dbo.ScrewTrap_Header.TrapStopped) < 23
and dbo.ScrewTrap_Header.TrapStopped != 'NA'
and len(dbo.ScrewTrap_Header.TrapStopped) > 0


raiserror(N'Updating SpawningGroundSurvey_Header StartTime, HH:mm format...', 0, 1) with nowait
update dbo.SpawningGroundSurvey_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', 
	substring(
		dbo.SpawningGroundSurvey_Header.StartTime, 
		1, 
		charIndex(':', dbo.SpawningGroundSurvey_Header.StartTime) - 1
		)),
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.SpawningGroundSurvey_Header.StartTime, 
		charIndex(':', dbo.SpawningGroundSurvey_Header.StartTime) + 1,
		2)), 
	2),
StartTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.StartTime 
from dbo.Activities a
inner join dbo.SpawningGroundSurvey_Header h on h.ActivityId = a.Id
where StartTime is not null
and len(StartTime) < 23
and len(StartTime) > 0
) as r
where dbo.SpawningGroundSurvey_Header.ActivityId = r.aId and dbo.SpawningGroundSurvey_Header.StartTime is not null and len(dbo.SpawningGroundSurvey_Header.StartTime) < 23
and len(dbo.SpawningGroundSurvey_Header.StartTime) > 0
--280 records

raiserror(N'Updating SpawningGroundSurvey_Header EndTime, HH:mm format...', 0, 1) with nowait
update dbo.SpawningGroundSurvey_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', 
	substring(
		dbo.SpawningGroundSurvey_Header.EndTime, 
		1, 
		charIndex(':', dbo.SpawningGroundSurvey_Header.EndTime) - 1
		)),
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.SpawningGroundSurvey_Header.EndTime, 
		charIndex(':', dbo.SpawningGroundSurvey_Header.EndTime) + 1,
		2)), 
	2),
EndTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.EndTime 
from dbo.Activities a
inner join dbo.SpawningGroundSurvey_Header h on h.ActivityId = a.Id
where EndTime is not null
and len(EndTime) < 23
and len(EndTime) > 0
) as r
where dbo.SpawningGroundSurvey_Header.ActivityId = r.aId and dbo.SpawningGroundSurvey_Header.EndTime is not null and len(dbo.SpawningGroundSurvey_Header.EndTime) < 23
and len(dbo.SpawningGroundSurvey_Header.EndTime) > 0
--234 records


raiserror(N'Updating SpawningGroundSurvey_Detail Time, HH:mm format...', 0, 1) with nowait
update dbo.SpawningGroundSurvey_Detail
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', 
	substring(
		dbo.SpawningGroundSurvey_Detail.[Time], 
		1, 
		charIndex(':', dbo.SpawningGroundSurvey_Detail.[Time]) - 1
		)),
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.SpawningGroundSurvey_Detail.[Time], 
		charIndex(':', dbo.SpawningGroundSurvey_Detail.[Time]) + 1,
		2)), 
	2),
[Time] = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select d.Id as dId, a.Id as aId, a.ActivityDate, d.[Time] as dTime
from dbo.Activities a
inner join dbo.SpawningGroundSurvey_Detail d on d.ActivityId = a.Id
where d.[Time] is not null
and len(d.[Time]) > 0
) as r
where dbo.SpawningGroundSurvey_Detail.ActivityId = r.aId and dbo.SpawningGroundSurvey_Detail.[Time] is not null
and len(dbo.SpawningGroundSurvey_Detail.[Time]) > 0
--23594 records


raiserror(N'Finished!', 0, 1) with nowait