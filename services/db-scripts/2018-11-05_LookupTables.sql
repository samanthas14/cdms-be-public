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
CREATE TABLE [dbo].[LookupTableProjects] (
    [LookupTable_Id] [int] NOT NULL,
    [Project_Id] [int] NOT NULL,
    CONSTRAINT [PK_dbo.LookupTableProjects] PRIMARY KEY ([LookupTable_Id], [Project_Id])
)
CREATE INDEX [IX_LookupTable_Id] ON [dbo].[LookupTableProjects]([LookupTable_Id])
CREATE INDEX [IX_Project_Id] ON [dbo].[LookupTableProjects]([Project_Id])
ALTER TABLE [dbo].[LookupTables] ADD CONSTRAINT [FK_dbo.LookupTables_dbo.Datasets_DatasetId] FOREIGN KEY ([DatasetId]) REFERENCES [dbo].[Datasets] ([Id])
ALTER TABLE [dbo].[LookupTableProjects] ADD CONSTRAINT [FK_dbo.LookupTableProjects_dbo.LookupTables_LookupTable_Id] FOREIGN KEY ([LookupTable_Id]) REFERENCES [dbo].[LookupTables] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[LookupTableProjects] ADD CONSTRAINT [FK_dbo.LookupTableProjects_dbo.Projects_Project_Id] FOREIGN KEY ([Project_Id]) REFERENCES [dbo].[Projects] ([Id]) ON DELETE CASCADE


-- todo don't foret to associate projects

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

