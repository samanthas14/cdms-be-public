-- Add the tables
CREATE TABLE [dbo].[Tier1_Detail] (
    [Id] [int] NOT NULL IDENTITY,
    [Tier1] [int],
    [Area] [decimal](18, 2),
    [Ct] [int],
    [UnitSpacing] [decimal](18, 2),
    [Freq] [decimal](18, 2),
    [Vol] [decimal](18, 2),
    [Pct] [decimal](18, 2),
    [DpthThlwgMaxAvg] [decimal](18, 2),
    [DpthThlwgExit] [decimal](18, 2),
    [DpthResid] [decimal](18, 2),
    [SubEstBldr] [decimal](18, 2),
    [SubEstCbl] [decimal](18, 2),
    [SubEstGrvl] [decimal](18, 2),
    [SubEstSandFines] [decimal](18, 2),
    [FishCovLW] [decimal](18, 2),
    [FishCovTVeg] [decimal](18, 2),
    [FishCovUcut] [decimal](18, 2),
    [FishCovArt] [decimal](18, 2),
    [FishCovAqVeg] [decimal](18, 2),
    [FishCovNone] [decimal](18, 2),
    [FishCovTotal] [decimal](18, 2),
    [LWVolWetTier1] [decimal](18, 2),
    [LWVolBfTier1] [decimal](18, 2),
    [CountOfLWD] [int],
    [CountOfChinook] [int],
    [CountOfCoho] [int],
    [CountOfSockeye] [int],
    [CountOfChum] [int],
    [CountOfOmykiss] [int],
    [CountOfPink] [int],
    [CountOfCutthroat] [int],
    [CountOfBulltrout] [int],
    [CountOfBrooktrout] [int],
    [CountOfLamprey] [int],
    [CountOfOtherSpecies] [int],
    [DensityOfChinook] [decimal](18, 2),
    [DensityOfCoho] [decimal](18, 2),
    [DensityOfSockeye] [decimal](18, 2),
    [DensityOfChum] [decimal](18, 2),
    [DensityOfOmykiss] [decimal](18, 2),
    [DensityOfPink] [decimal](18, 2),
    [DensityOfCutthroat] [decimal](18, 2),
    [DensityOfBulltrout] [decimal](18, 2),
    [DensityOfBrooktrout] [decimal](18, 2),
    [DensityOfLamprey] [decimal](18, 2),
    [DensityOfOtherSpecies] [decimal](18, 2),
    [RowId] [int] NOT NULL,
    [RowStatusId] [int] NOT NULL,
    [ActivityId] [int] NOT NULL,
    [ByUserId] [int] NOT NULL,
    [QAStatusId] [int] NOT NULL,
    [EffDt] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.Tier1_Detail] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_ActivityId] ON [dbo].[Tier1_Detail]([ActivityId])
CREATE INDEX [IX_ByUserId] ON [dbo].[Tier1_Detail]([ByUserId])
CREATE INDEX [IX_QAStatusId] ON [dbo].[Tier1_Detail]([QAStatusId])

CREATE TABLE [dbo].[Tier1_Header] (
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
    [AEM] [int],
    [Effectiveness] [int],
    [HasFishData] [int],
    [PrimaryVisit] [int],
    [QCVisit] [int],
    [ActivityId] [int] NOT NULL,
    [ByUserId] [int] NOT NULL,
    [EffDt] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.Tier1_Header] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_ActivityId] ON [dbo].[Tier1_Header]([ActivityId])
CREATE INDEX [IX_ByUserId] ON [dbo].[Tier1_Header]([ByUserId])
ALTER TABLE [dbo].[Tier1_Detail] ADD CONSTRAINT [FK_dbo.Tier1_Detail_dbo.Activities_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activities] ([Id])
ALTER TABLE [dbo].[Tier1_Detail] ADD CONSTRAINT [FK_dbo.Tier1_Detail_dbo.Users_ByUserId] FOREIGN KEY ([ByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[Tier1_Detail] ADD CONSTRAINT [FK_dbo.Tier1_Detail_dbo.QAStatus_QAStatusId] FOREIGN KEY ([QAStatusId]) REFERENCES [dbo].[QAStatus] ([Id])
ALTER TABLE [dbo].[Tier1_Header] ADD CONSTRAINT [FK_dbo.Tier1_Header_dbo.Activities_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activities] ([Id])
ALTER TABLE [dbo].[Tier1_Header] ADD CONSTRAINT [FK_dbo.Tier1_Header_dbo.Users_ByUserId] FOREIGN KEY ([ByUserId]) REFERENCES [dbo].[Users] ([Id])
go

-- Add the views
--drop view Tier1_Detail_VW
--go
create view Tier1_Detail_VW
AS
SELECT        Id, Tier1, Area, Ct, UnitSpacing, Freq, Vol, Pct, DpthThlwgMaxAvg, DpthThlwgExit, DpthResid, SubEstBldr, SubEstCbl, SubEstGrvl, SubEstSandFines, FishCovLW, FishCovTVeg, FishCovUcut, FishCovArt, FishCovAqVeg, 
                         FishCovNone, FishCovTotal, LWVolWetTier1, LWVolBfTier1, CountOfLWD, CountOfChinook, CountOfCoho, CountOfSockeye, CountOfChum, CountOfOmykiss, CountOfPink, CountOfCutthroat, CountOfBulltrout, CountOfBrooktrout, 
                         CountOfLamprey, CountOfOtherSpecies, DensityOfChinook, DensityOfCoho, DensityOfSockeye, DensityOfChum, DensityOfOmykiss, DensityOfPink, DensityOfCutthroat, DensityOfBulltrout, DensityOfBrooktrout, 
                         DensityOfLamprey, DensityOfOtherSpecies, RowStatusId, ActivityId, RowId, ByUserId, QAStatusId, EffDt
FROM            dbo.Tier1_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Tier1_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
go


--drop view Tier1_Header_VW
--go
create view Tier1_Header_VW
AS
SELECT        Id, ProgramSiteID, SiteName, WatershedID, WatershedName, SampleDate, HitchName, CrewName, VisitYear, IterationID, CategoryName, PanelName, VisitID, VisitDate, ProtocolID, ProgramID, AEM, Effectiveness, HasFishData, 
                         PrimaryVisit, QCVisit, ActivityId, ByUserId, EffDt
FROM            dbo.Tier1_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Tier1_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
go


--drop view Tier1_vw
--go
create view Tier1_vw
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.CreateDate, a.ActivityTypeId, a.ActivityDate, h.Id, h.ProgramSiteID, h.SiteName, h.WatershedID, h.WatershedName, h.SampleDate, h.HitchName, h.CrewName, 
                         h.VisitYear, h.IterationID, h.CategoryName, h.PanelName, h.VisitID, h.VisitDate, h.ProtocolID, h.ProgramID, h.AEM, h.Effectiveness, h.HasFishData, h.PrimaryVisit, h.QCVisit, h.ByUserId, h.EffDt, d.Id AS Tier1_Detail_Id, d.Tier1, 
                         d.Area, d.Ct, d.UnitSpacing, d.Freq, d.Vol, d.Pct, d.DpthThlwgMaxAvg, d.DpthThlwgExit, d.DpthResid, d.SubEstBldr, d.SubEstCbl, d.SubEstGrvl, d.SubEstSandFines, d.FishCovLW, d.FishCovTVeg, d.FishCovUcut, d.FishCovAqVeg, 
                         d.FishCovNone, d.FishCovArt, d.FishCovTotal, d.LWVolWetTier1, d.LWVolBfTier1, d.CountOfLWD, d.CountOfChinook, d.CountOfCoho, d.CountOfSockeye, d.CountOfChum, d.CountOfOmykiss, d.CountOfPink, d.CountOfCutthroat, 
                         d.CountOfBulltrout, d.CountOfBrooktrout, d.CountOfLamprey, d.CountOfOtherSpecies, d.DensityOfChinook, d.DensityOfCoho, d.DensityOfSockeye, d.DensityOfChum, d.DensityOfOmykiss, d.DensityOfPink, d.DensityOfCutthroat, 
                         d.DensityOfBulltrout, d.DensityOfBrooktrout, d.DensityOfLamprey, d.DensityOfOtherSpecies, d.ByUserId AS Tier1_Detail_ByUserId, d.QAStatusId, d.EffDt AS Tier1_Detail_EffDt, aq.QAStatusId AS ActivityQAStatusId, 
                         aq.UserId AS ActivityQAUserId, aq.Comments, aq.QAStatusName, l.Label AS LocationLabel
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id INNER JOIN
                         dbo.Tier1_Header_VW AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId LEFT OUTER JOIN
                         dbo.Tier1_Detail_VW AS d ON a.Id = d.ActivityId
go


-- Add the data
declare @datasetBaseName as varchar(max) = 'Tier1'
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
		DataType = 'select',
		PossibleValues = '[Yes,No]',
		[Rule] = NULL,
		DbColumnName = 'AEM',
		ControlType = 'select',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Effectiveness',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'select',
		PossibleValues = '[Yes,No]',
		[Rule] = NULL,
		DbColumnName = 'Effectiveness',
		ControlType = 'select',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Has Fish Data',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'select',
		PossibleValues = '[Yes,No]',
		[Rule] = NULL,
		DbColumnName = 'HasFishData',
		ControlType = 'select',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'Primary Visit',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'select',
		PossibleValues = '[Yes,No]',
		[Rule] = NULL,
		DbColumnName = 'PrimaryVisit',
		ControlType = 'select',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'QC Visit',
		Description = 'Need definition',
		Units = NULL,
		Validation = NULL,
		DataType = 'select',
		PossibleValues = '[Yes,No]',
		[Rule] = NULL,
		DbColumnName = 'QCVisit',
		ControlType = 'select',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))


update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header


--Details Fields

INSERT INTO dbo.Fields(Name, [Description], Units, Validation, DataType, PossibleValues, [Rule], DbColumnName, ControlType, DatastoreId)
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType, INSERTED.[Rule], NULL INTO #NewFieldInfo
SELECT
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
		Name = 'Area',
		Description = 'Area of this type of unit',
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
		Description = 'Count of this type of unit',
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
		Units = NULL,
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
		Units = NULL,
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

UNION ALL SELECT
		Name = 'SubEstBldr',
		Description = 'Percent of boulders (256-4000 mm) within the wetted site area.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SubEstBldr',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'SubEstCbl',
		Description = 'Percent of cobbles (64-256 mm) within the wetted site area.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SubEstCbl',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'SubEstGrvl',
		Description = 'Percent of coarse and fine gravel (2-64 mm) within the wetted site area.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SubEstGrvl',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'SubEstSandFines',
		Description = 'Percent of sand and fine sediment (0.01-2 mm) within the wetted site area.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'SubEstSandFines',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'FishCovLW',
		Description = 'Percent of wetted area that has woody debris as fish cover.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FishCovLW',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'FishCovTVeg',
		Description = 'Percent of wetted area that has live terrestrial vegetation as fish cover.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FishCovTVeg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'FishCovUcut',
		Description = 'Retired metric. Percent of wetted area that has Undercut as fish cover.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FishCovUcut',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'FishCovArt',
		Description = 'Percent of wetted area that has artificial structure as fish cover.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FishCovArt',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'FishCovAqVeg',
		Description = 'Percent of wetted area that has aquatic vegetation as fish cover.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FishCovAqVeg',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'FishCovNone',
		Description = 'Percent of channel unit with no fish cover.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FishCovNone',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'FishCovTotal',
		Description = 'Percent of wetted area with the following types of cover: aquatic vegetation, artificial, woody debris, and terrestrial vegetation.',
		Units = '%',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'FishCovTotal',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'LWVol_WetTier1',
		Description = 'Total volume of  large wood pieces  within the wetted channel and Slow Water Pool channel units.',
		Units = 'm3',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'LWVolWetTier1',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'LWVol_BfTier1',
		Description = 'Total volume of  large wood pieces  within the wetted channel.',
		Units = 'm3',
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'LWVolBfTier1',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'CountOfLWD',
		Description = 'Count of Large Wood Debry in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfLWD',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'CountOfChinook',
		Description = 'Count of Chinook in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfChinook',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'CountOfCoho',
		Description = 'Count of Coho in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfCoho',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'CountOfSockeye',
		Description = 'Count of Sockeye in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfSockeye',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'CountOfChum',
		Description = 'Count of Chum in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfChum',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'CountOfOmykiss',
		Description = 'Count of Omykiss in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfOmykiss',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'CountOfPink',
		Description = 'Count of Pink in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfPink',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'CountOfCutthroat',
		Description = 'Count of Cutthroat in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfCutthroat',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'CountOfBulltrout',
		Description = 'Count of Bulltrout in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfBulltrout',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'CountOfBrooktrout',
		Description = 'Count of Brooktrout in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfBrooktrout',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'CountOfLamprey',
		Description = 'Count of Lamprey in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfLamprey',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'CountOfOtherSpecies',
		Description = 'Count of Other Species in Unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'int',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'CountOfOtherSpecies',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'DensityOfChinook',
		Description = 'Density of Chinook in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfChinook',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'DensityOfCoho',
		Description = 'Density of Coho in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfCoho',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'DensityOfSockeye',
		Description = 'Density of Sockeye in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfSockeye',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'DensityOfChum',
		Description = 'Density of Chum in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfChum',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'DensityOfOmykiss',
		Description = 'Density of Omykiss in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfOmykiss',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'DensityOfPink',
		Description = 'Density of Pink in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfPink',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'DensityOfCutthroat',
		Description = 'Density of Cutthroat in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfCutthroat',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'DensityOfBulltrout',
		Description = 'Density of Bulltrout in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfBulltrout',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'DensityOfBrooktrout',
		Description = 'Density of Brooktrout in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfBrooktrout',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'DensityOfLamprey',
		Description = 'Density of Lamprey in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfLamprey',
		ControlType = 'number',
		DatastoreId = (SELECT IDENT_CURRENT('dbo.Datastores'))

UNION ALL SELECT
		Name = 'DensityOfOtherSpecies',
		Description = 'Density of Other Species in unit',
		Units = NULL,
		Validation = NULL,
		DataType = 'float',
		PossibleValues = NULL,
		[Rule] = NULL,
		DbColumnName = 'DensityOfOtherSpecies',
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

set @DatasetId = (select Id from dbo.Datasets where [Name] like '%Tier1%');

set @FieldId = (select Id from dbo.Fields where DatastoreId in (select Id from dbo.Datastores where [Name] like '%ActivitySystem%') and [DbColumnName] = 'ActivityDate');

insert into DatasetFields (DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, [Validation], SourceId, InstrumentId, OrderIndex, ControlType)
values (@DatasetId, @FieldId, 1, CONVERT(VARCHAR(23), GETDATE(), 121), 'Activity Date', 'ActivityDate', null, 1, null, 3, 'activity-date');

set @FieldId = (select Id from dbo.Fields where DatastoreId in (select Id from dbo.Datastores where [Name] like '%ActivitySystem%') and DbColumnName = 'LocationId');

insert into DatasetFields (DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, [Validation], SourceId, InstrumentId, OrderIndex, ControlType)
values (@DatasetId, @FieldId, 1, CONVERT(VARCHAR(23), GETDATE(), 121), 'Location', 'LocationId', null, 1, null, 5, 'location-select');