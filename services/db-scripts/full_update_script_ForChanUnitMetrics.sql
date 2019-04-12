--Add the tables
CREATE TABLE [dbo].[ChannelUnitMetrics_Detail] (
    [Id] [int] NOT NULL IDENTITY,
    [ChUnitID] [int],
    [ChUnitNum] [int],
    [Tier1] [int],
    [Tier2] [int],
    [AreaTotal] [decimal](18, 2),
    [PolyArea] [decimal](18, 2),
    [TotalVol] [decimal](18, 2),
    [DpthMax] [decimal](18, 2),
    [DpthThlwgExit] [decimal](18, 2),
    [DpthResid] [decimal](18, 2),
    [CountOfLWD] [int],
    [RowId] [int] NOT NULL,
    [RowStatusId] [int] NOT NULL,
    [ActivityId] [int] NOT NULL,
    [ByUserId] [int] NOT NULL,
    [QAStatusId] [int] NOT NULL,
    [EffDt] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.ChannelUnitMetrics_Detail] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_ActivityId] ON [dbo].[ChannelUnitMetrics_Detail]([ActivityId])
CREATE INDEX [IX_ByUserId] ON [dbo].[ChannelUnitMetrics_Detail]([ByUserId])
CREATE INDEX [IX_QAStatusId] ON [dbo].[ChannelUnitMetrics_Detail]([QAStatusId])
CREATE TABLE [dbo].[ChannelUnitMetrics_Header] (
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
    [ActivityId] [int] NOT NULL,
    [ByUserId] [int] NOT NULL,
    [EffDt] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.ChannelUnitMetrics_Header] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_ActivityId] ON [dbo].[ChannelUnitMetrics_Header]([ActivityId])
CREATE INDEX [IX_ByUserId] ON [dbo].[ChannelUnitMetrics_Header]([ByUserId])
ALTER TABLE [dbo].[ChannelUnitMetrics_Detail] ADD CONSTRAINT [FK_dbo.ChannelUnitMetrics_Detail_dbo.Activities_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activities] ([Id])
ALTER TABLE [dbo].[ChannelUnitMetrics_Detail] ADD CONSTRAINT [FK_dbo.ChannelUnitMetrics_Detail_dbo.Users_ByUserId] FOREIGN KEY ([ByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[ChannelUnitMetrics_Detail] ADD CONSTRAINT [FK_dbo.ChannelUnitMetrics_Detail_dbo.QAStatus_QAStatusId] FOREIGN KEY ([QAStatusId]) REFERENCES [dbo].[QAStatus] ([Id])
ALTER TABLE [dbo].[ChannelUnitMetrics_Header] ADD CONSTRAINT [FK_dbo.ChannelUnitMetrics_Header_dbo.Activities_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activities] ([Id])
ALTER TABLE [dbo].[ChannelUnitMetrics_Header] ADD CONSTRAINT [FK_dbo.ChannelUnitMetrics_Header_dbo.Users_ByUserId] FOREIGN KEY ([ByUserId]) REFERENCES [dbo].[Users] ([Id])
go

--Add the views
Drop view dbo.ChannelUnitMetrics_Detail_VW
go
create view dbo.ChannelUnitMetrics_Detail_VW
as
SELECT        Id, ChUnitID, ChUnitNum, Tier1, Tier2, AreaTotal, PolyArea, TotalVol, DpthMax, DpthThlwgExit, DpthResid, CountOfLWD, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt
FROM            dbo.ChannelUnitMetrics_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.ChannelUnitMetrics_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
go


Drop view dbo.ChannelUnitMetrics_Header_VW
go
create view dbo.ChannelUnitMetrics_Header_VW
as
SELECT        Id, ProgramSiteID, SiteName, WatershedID, WatershedName, SampleDate, HitchName, CrewName, VisitYear, IterationID, CategoryName, PanelName, VisitID, VisitDate, ActivityId, ByUserId, EffDt
FROM            dbo.ChannelUnitMetrics_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.ChannelUnitMetrics_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
go


Drop view dbo.ChannelUnitMetrics_vw
go
create view dbo.ChannelUnitMetrics_vw
as
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, h.Id, h.ProgramSiteID, h.SiteName, h.WatershedID, h.WatershedName, h.SampleDate, h.HitchName, h.CrewName, 
                         h.VisitYear, h.IterationID, h.CategoryName, h.PanelName, h.VisitID, h.VisitDate, h.ByUserId, h.EffDt, d.Id AS ChannelUnitMetrics_Detail_Id, d.ChUnitID, d.ChUnitNum, d.Tier1, d.Tier2, d.AreaTotal, d.PolyArea, d.TotalVol, 
                         d.DpthMax, d.DpthThlwgExit, d.DpthResid, d.CountOfLWD, d.RowId, d.ByUserId AS ChannelUnitMetrics_Detail_ByUserId, d.QAStatusId, d.EffDt AS ChannelUnitMetrics_Detail_EffDt, l.Label, l.Status, l.GPSEasting, l.GPSNorthing, 
                         l.Projection, l.UTMZone, l.Latitude, l.Longitude, l.OtherAgencyId, l.ImageLink, l.WettedWidth, l.WettedDepth, l.RiverMile, l.WaterBodyId, l.ProjectId, l.SubprojectId, l.StudyDesign, aq.QAStatusId AS ActivityQAStatusId, 
                         aq.UserId AS ActivityQAUserId, aq.Comments, aq.QAStatusName
FROM            dbo.Activities AS a INNER JOIN
                         dbo.ChannelUnitMetrics_Header AS h ON h.ActivityId = a.Id LEFT OUTER JOIN
                         dbo.ChannelUnitMetrics_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id
go


--Add the data
declare @datasetBaseName as varchar(max) = 'Channel Unit Metrics'
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


update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header


--Details Fields

INSERT INTO dbo.Fields(Name, [Description], Units, Validation, DataType, PossibleValues, [Rule], DbColumnName, ControlType, DatastoreId)
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType, INSERTED.[Rule], NULL INTO #NewFieldInfo
SELECT
		Name = 'ChUnitID',
		Description = 'Unique identifier of a Channel Unit.',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'ChUnitID',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'ChUnitNum',
		Description = 'Channel Unit Number within a site as identified during field data collection.',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'ChUnitNum',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Tier1',
		Description = 'Tier 1 channel unit type.',
		Units = NULL,
		Validation = NULL,
		DataType = 'select',
		PossibleValues = '[Fast-NonTurbulent/Glide, Fast-Turbulent, Slow/Pool, Small Side Channel]',
		[Rule] = NULL,
		DbColumnName = 'Tier1',
		ControlType = 'select',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Tier2',
		Description = 'Tier 2 channel unit type.',
		Units = NULL,
		Validation = NULL,
		DataType = 'select',
		PossibleValues = '[Dam Pool, Fast-NonTurbulent/Glide, Off Channel, Plunge Pool, Rapid, Riffle, Scour Pool, Small Side Channel]',
		[Rule] = NULL,
		DbColumnName = 'Tier2',
		ControlType = 'select',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'AreaTotal',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'AreaTotal',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'PolyArea',
		Description = 'Area of the channel unit after clipped to the wetted extent.',
		Units = 'm2',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'PolyArea',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'TotalVol',
		Description = 'Total volume of channel unit type.',
		Units = 'm3',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'TotalVol',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Dpth_Max',
		Description = 'Maximum depth of the wetted channel unit.',
		Units = 'm',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DpthMax',
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

UNION ALL SELECT
		Name = 'CountOfLWD',
		Description = 'Number of Large woody debry in side of the channel unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfLWD',
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