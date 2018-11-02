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
('Waterbody Id','WaterBodyId','WaterBodyId','number',@newdsid,1,null,'int',null,null),
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

