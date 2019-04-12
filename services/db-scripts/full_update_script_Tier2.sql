-- Add the tables
CREATE TABLE [dbo].[Tier2_Detail] (
    [Id] [int] NOT NULL IDENTITY,
    [Tier2] [nvarchar](max),
    [Area] [decimal](18, 2),
    [Ct] [int],
    [UnitSpacing] [decimal](18, 2),
    [Freq] [decimal](18, 2),
    [Vol] [decimal](18, 2),
    [Pct] [decimal](18, 2),
    [DpthThlwgMaxAvg] [decimal](18, 2),
    [DpthThlwgExit] [decimal](18, 2),
    [DpthResid] [decimal](18, 2),
    [RowId] [int] NOT NULL,
    [RowStatusId] [int] NOT NULL,
    [ActivityId] [int] NOT NULL,
    [ByUserId] [int] NOT NULL,
    [QAStatusId] [int] NOT NULL,
    [EffDt] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.Tier2_Detail] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_ActivityId] ON [dbo].[Tier2_Detail]([ActivityId])
CREATE INDEX [IX_ByUserId] ON [dbo].[Tier2_Detail]([ByUserId])
CREATE INDEX [IX_QAStatusId] ON [dbo].[Tier2_Detail]([QAStatusId])
CREATE TABLE [dbo].[Tier2_Header] (
    [Id] [int] NOT NULL IDENTITY,
    [ProgramSiteID] [int],
    [SiteName] [nvarchar](max),
    [WatershedID] [int],
    [WatershedName] [nvarchar](max),
    [SampleDate] [nvarchar](max),
    [HitchName] [nvarchar](max),
    [CrewName] [nvarchar](max),
    [VisitYear] [int],
    [IterationID] [int],
    [CategoryName] [nvarchar](max),
    [PanelName] [nvarchar](max),
    [VisitID] [int],
    [VisitDate] [int],
    [ProtocolID] [int],
    [ProgramID] [int],
    [AEM] [nvarchar](max),
    [Effectiveness] [nvarchar](max),
    [HasFishData] [nvarchar](max),
    [PrimaryVisit] [nvarchar](max),
    [QCVisit] [nvarchar](max),
    [ActivityId] [int] NOT NULL,
    [ByUserId] [int] NOT NULL,
    [EffDt] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.Tier2_Header] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_ActivityId] ON [dbo].[Tier2_Header]([ActivityId])
CREATE INDEX [IX_ByUserId] ON [dbo].[Tier2_Header]([ByUserId])
ALTER TABLE [dbo].[Tier2_Detail] ADD CONSTRAINT [FK_dbo.Tier2_Detail_dbo.Activities_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activities] ([Id])
ALTER TABLE [dbo].[Tier2_Detail] ADD CONSTRAINT [FK_dbo.Tier2_Detail_dbo.Users_ByUserId] FOREIGN KEY ([ByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[Tier2_Detail] ADD CONSTRAINT [FK_dbo.Tier2_Detail_dbo.QAStatus_QAStatusId] FOREIGN KEY ([QAStatusId]) REFERENCES [dbo].[QAStatus] ([Id])
ALTER TABLE [dbo].[Tier2_Header] ADD CONSTRAINT [FK_dbo.Tier2_Header_dbo.Activities_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activities] ([Id])
ALTER TABLE [dbo].[Tier2_Header] ADD CONSTRAINT [FK_dbo.Tier2_Header_dbo.Users_ByUserId] FOREIGN KEY ([ByUserId]) REFERENCES [dbo].[Users] ([Id])
go


-- Add the views
drop view Tier2_Detail_VW
go
create view Tier2_Detail_VW
AS
SELECT        Id, Tier2, Area, Ct, UnitSpacing, Freq, Vol, Pct, DpthThlwgMaxAvg, DpthThlwgExit, DpthResid, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt
FROM            dbo.Tier2_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Tier2_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
go


drop view Tier2_Header_VW
go
create view Tier2_Header_VW
AS
SELECT        Id, ProgramSiteID, SiteName, WatershedID, WatershedName, SampleDate, HitchName, CrewName, VisitYear, IterationID, CategoryName, PanelName, VisitID, VisitDate, ProtocolID, ProgramID, AEM, HasFishData, Effectiveness, 
                         PrimaryVisit, QCVisit, ActivityId, ByUserId, EffDt
FROM            dbo.Tier2_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Tier2_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
go


drop view Tier2_vw
go
create view Tier2_vw
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, h.Id, h.ProgramSiteID, h.SiteName, h.WatershedID, h.WatershedName, h.SampleDate, h.HitchName, h.CrewName, 
                         h.VisitYear, h.IterationID, h.CategoryName, h.PanelName, h.VisitID, h.VisitDate, h.ProtocolID, h.ProgramID, h.AEM, h.HasFishData, h.Effectiveness, h.PrimaryVisit, h.QCVisit, h.ByUserId, h.EffDt, d.Id AS Tier2_Detail_Id, d.Tier2, 
                         d.Area, d.Ct, d.UnitSpacing, d.Freq, d.Vol, d.Pct, d.DpthThlwgMaxAvg, d.DpthThlwgExit, d.DpthResid, d.RowId, d.ByUserId AS Tier2_Detail_ByUserId, d.QAStatusId, d.EffDt AS Tier2_Detail_EffDt, 
                         aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments, aq.QAStatusName, l.Label AS LocationLabel
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Tier2_Header_VW AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.Tier2_Detail_VW AS d ON a.Id = d.ActivityId LEFT OUTER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId
go


-- Add the data
declare @datasetBaseName as varchar(max) = 'Tier2'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = @datasetBaseName

-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 2249 

-- Grab the project names
update #ProjectInfo set name = (select name from projects where projects.id = #ProjectInfo.id)

create table #NewLocationTypeIds (id int)

-- Add record to LocationTypes
INSERT into dbo.LocationTypes([Name], [Description])
OUTPUT INSERTED.id into #NewLocationTypeIds
values (@datasetBaseName, @datasetBaseName)

-- Add records to the Datastores
INSERT INTO dbo.Datastores (Name, Description, TablePrefix, OwnerUserId, LocationTypeId, DefaultConfig) 
SELECT
	Name               = @datastoreName, 
	Description        = NULL, 
	TablePrefix        = REPLACE(@datastoreName, ' ', ''), -- Strip spaces 
	OwnerUserId        = 1081,      -- George
	LocationTypeId     = lt.id,
	DefaultConfig	   = '{}'
from #NewLocationTypeIds as lt


CREATE TABLE #NewDatasetIds (id int)        -- This will contain a list of ids of all dataset records inserted below

-- Add record to the Datasets --> will create one record per project
INSERT INTO	Datasets (ProjectId, DefaultRowQAStatusId, StatusId, CreateDateTime, Name, Description, DefaultActivityQAStatusId, DatastoreId, Config)
OUTPUT INSERTED.id into #NewDatasetIds
SELECT 
    ProjectId                 = p.id,
    DefaultRowQAStatusId      = 1,
    StatusId                  = 1,
    CreateDateTime            = GetDate(),
    Name                      = @datasetBaseName,
    Description               = @datasetBaseName + ': ' + p.name,
    DefaultActivityQAStatusId = 6,
    DatastoreId               = (SELECT IDENT_CURRENT('dbo.Datastores')),
    Config                    = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}'
FROM #ProjectInfo as p


-- This will info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    

-----
-- Insert any new fields we'll need (George says don't reuse fields)
-- Header Fields
INSERT INTO dbo.Fields (Name, [Description], Units, Validation, DataType, PossibleValues, [Rule], DbColumnName, ControlType, DatastoreId)
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType, INSERTED.[Rule], NULL INTO #NewFieldInfo
SELECT
		Name = 'ProgramSiteID',
		Description = 'AEM Champ Number Assigned by Program',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'ProgramSiteID',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'SiteName',
		Description = 'AEM Champ Number plus Ctuir site name',
		Units = NULL,
		Validation = NULL,
		DataType = 'string',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SiteName',
		ControlType = 'text',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'WatershedID',
		Description = 'AEM Champ Number assigned to watershed',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WatershedID',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'WatershedName',
		Description = 'Watershed Name',
		Units = NULL,
		Validation = NULL,
		DataType = 'string',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'WatershedName',
		ControlType = 'text',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'SampleDate',
		Description = 'Date of Activity',
		Units = NULL,
		Validation = NULL,
		DataType = 'datetime',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SampleDate',
		ControlType = 'datetime',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'HitchName',
		Description = 'Name created for Hitch',
		Units = NULL,
		Validation = NULL,
		DataType = 'string',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'HitchName',
		ControlType = 'text',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'CrewName',
		Description = 'Name of the technician who collect the data',
		Units = NULL,
		Validation = NULL,
		DataType = 'string',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CrewName',
		ControlType = 'text',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'VisitYear',
		Description = 'Year of the activity',
		Units = 'yyyy',
		Validation = 'i4',
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'VisitYear',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'IterationID',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'IterationID',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'CategoryName',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'string',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CategoryName',
		ControlType = 'text',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'PanelName',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'select',
		PossibleValues = '[All, Annual, CTUIR Annual, Default, Rotating Panel 1, Rotating Panel 2, Rotating Panel 3]',
		[Rule] = NULL,
		DbColumnName = 'PanelName',
		ControlType = 'select',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'VisitID',
		Description = 'Number assigned to the activity',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'VisitID',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'VisitDate',
		Description = 'Date of Activity',
		Units = NULL,
		Validation = NULL,
		DataType = 'datetime',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'VisitDate',
		ControlType = 'datetime',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'ProtocolID',
		Description = 'Number of the protocol used for that visit',
		Units = NULL,
		Validation = NULL,
		DataType = 'select',
		PossibleValues = '[413, 806, 1870, 1875, 1877, 1955, 1966, 2020, 2030, 2038, 2044, 9999, 10001]',
		[Rule] = NULL,
		DbColumnName = 'ProtocolID',
		ControlType = 'select',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'ProgramID',
		Description = 'AEM Champ Number Assigned by Program',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'ProgramID',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'AEM',
		Description = 'is this an AEM Site',
		Units = NULL,
		Validation = NULL,
		DataType = 'string',
		PossibleValues = '[Yes,No]',
		[Rule] = NULL,
		DbColumnName = 'AEM',
		ControlType = 'text',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Effectiveness',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'string',
		PossibleValues = '[Yes,No]',
		[Rule] = NULL,
		DbColumnName = 'Effectiveness',
		ControlType = 'text',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Has Fish Data',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'string',
		PossibleValues = '[Yes,No]',
		[Rule] = NULL,
		DbColumnName = 'HasFishData',
		ControlType = 'text',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Primary Visit',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'string',
		PossibleValues = '[Yes,No]',
		[Rule] = NULL,
		DbColumnName = 'PrimaryVisit',
		ControlType = 'text',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'QC Visit',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'string',
		PossibleValues = '[Yes,No]',
		[Rule] = NULL,
		DbColumnName = 'QCVisit',
		ControlType = 'text',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))


update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header


--Details Fields

INSERT INTO dbo.Fields(Name, [Description], Units, Validation, DataType, PossibleValues, [Rule], DbColumnName, ControlType, DatastoreId)
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType, INSERTED.[Rule], NULL INTO #NewFieldInfo
SELECT
		Name = 'Tier2',
		Description = 'Teir 2 channel unit type',
		Units = NULL,
		Validation = NULL,
		DataType = 'select',
		PossibleValues = '[Dam Pool, Fast-NonTurbulent-Glide, Off Channel, Plunge Pool, Rapid, Riffle, Scour Pool, Small Side Channel]',
		[Rule] = NULL,
		DbColumnName = 'Tier2',
		ControlType = 'select',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Area',
		Description = 'Area of this type of unit through out site',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'Area',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Ct',
		Description = 'Count of this type of unit through out site',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'Ct',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'UnitSpacing',
		Description = 'Need definition',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'UnitSpacing',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Freq',
		Description = 'Frequency of channel unit ',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'Freq',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Vol',
		Description = 'Need definition',
		Units = 'm3',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'Vol',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Pct',
		Description = 'Need definition',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'Pct',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'DpthThlwgMax_Avg',
		Description = 'Maximum depth of the thalweg within the channel unit.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DpthThlwgMaxAvg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'DpthThlwgExit',
		Description = 'Depth of the thalweg at the downstream edge of the channel unit.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DpthThlwgExit',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'DpthResid',
		Description = 'Average residual depth of the channel unit.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DpthResid',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))


update #NewFieldInfo set FieldRoleId = 2 where FieldRoleId is NULL   -- 2 == details


-- Assign new fields to the datasets -- this will insert a new row for each combination of datasetId and fieldId for the records inserted above
INSERT INTO dbo.DatasetFields(DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, Validation, SourceId, InstrumentId, OrderIndex, ControlType, [Rule])
SELECT
    DatasetId      = d.id,
    FieldId        = f.id,
    FieldRoleId    = f.FieldRoleId,
    CreateDateTime = GetDate(),
    Label          = f.fieldName,
    DbColumnName   = f.DbColumnName,
    Validation     = f.Validation,
    SourceId       = 1,
    InstrumentId   = NULL,
    OrderIndex     = f.OrderIndex * 10,     -- x10 to make it easier to insert intermediate orders
    ControlType    = f.ControlType,
    [Rule]         = f.[Rule]
FROM #NewDatasetIds as d, #NewFieldInfo as f

-- Add some new DatasetQAStatus records for our new datasets
CREATE TABLE #QaStatusIds (id int)
INSERT INTO #QaStatusIds (id) 
          SELECT id = 5     -- Approved
UNION ALL SELECT id = 6     -- Ready for QA


INSERT INTO dbo.DatasetQAStatus(Dataset_Id, QAStatus_id)
SELECT
    Dataset_Id  = d.id,
    QAStatus_id = q.id
FROM #NewDatasetIds as d, #QaStatusIds as q


INSERT INTO dbo.DatasetQAStatus1(Dataset_Id, QAStatus_id)
SELECT
    Dataset_Id  = d.id,
    QAStatus_id = q.id
FROM #NewDatasetIds as d, #QaStatusIds as q

-- Cleanup
drop table #ProjectInfo
drop table #NewLocationTypeIds
drop table #NewFieldInfo
drop table #QaStatusIds
drop table #NewDatasetIds


-- Add system fields
declare @DatasetId as int;
declare @FieldId as int;

set @DatasetId = (select Id from dbo.Datasets where [Name] like '%Tier2%');

set @FieldId = (select Id from dbo.Fields where DatastoreId in (select Id from dbo.Datastores where [Name] like '%ActivitySystem%') and [DbColumnName] = 'ActivityDate');

insert into DatasetFields (DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, [Validation], SourceId, InstrumentId, OrderIndex, ControlType)
values (@DatasetId, @FieldId, 1, CONVERT(VARCHAR(23), GETDATE(), 121), 'Activity Date', 'ActivityDate', null, 1, null, 3, 'activity-date');

set @FieldId = (select Id from dbo.Fields where DatastoreId in (select Id from dbo.Datastores where [Name] like '%ActivitySystem%') and DbColumnName = 'LocationId');

insert into DatasetFields (DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, [Validation], SourceId, InstrumentId, OrderIndex, ControlType)
values (@DatasetId, @FieldId, 1, CONVERT(VARCHAR(23), GETDATE(), 121), 'Location', 'LocationId', null, 1, null, 5, 'location-select');