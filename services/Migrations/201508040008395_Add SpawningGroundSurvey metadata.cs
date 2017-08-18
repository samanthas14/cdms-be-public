namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddSpawningGroundSurveymetadata : DbMigration
    {
        public override void Up()
        {
            Sql(@"

-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'Spawning Ground Survey'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = @datasetBaseName


-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 2249 
UNION ALL SELECT id = 1135 
UNION ALL SELECT id = 1188
UNION ALL SELECT id = 1177


-- Grab the project names
update #ProjectInfo set name = (select name from projects where projects.id = #ProjectInfo.id)


-- Create a field category
INSERT INTO dbo.FieldCategories (Name, Description) 
SELECT Name        = @categoryName,
       Description = @datasetBaseName + ' related fields'


-- Add records to the Datastores
INSERT INTO dbo.Datastores (Name, Description, TablePrefix, DatastoreDatasetId, OwnerUserId, FieldCategoryId) 
SELECT
	Name               = @datastoreName, 
	Description        = NULL, 
	TablePrefix        = REPLACE(@datastoreName, ' ', ''), -- Strip spaces
	DatastoreDatasetId = NULL, 
	OwnerUserId        = 1081,      -- George
	FieldCategoryId    = (SELECT IDENT_CURRENT('dbo.FieldCategories'))


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
    Config                    = '{""DataEntryPage"": {""HiddenFields"": [""Instrument""]}}'
FROM #ProjectInfo as p

-------------------------

-- This will info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    

-----
-- Insert any new fields we'll need (George says don't reuse fields)
-- Header Fields
INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT   
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Species',
        Description = 'Species the survey is targeting',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""CHF"", ""CO"", ""STS"", ""CHS"", ""PL""]',
        DbColumnName = 'Species',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Technicians',
        Description = 'Name of the technician who visited the site and performed the activity',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'Technicians',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Start Time',
        Description = 'Time the technician started the section survey',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'StartTime',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'End Time',
        Description = 'Time the technician ended the section survey',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'EndTime',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Start Temperature',
        Description = 'Temperature of the water at the time the survey began',
        Units = 'C',
        Validation = '[-18,49]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'StartTemperature',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'End Temperature',
        Description = 'Temperature of the water at the time the survey ended',
        Units = 'C',
        Validation = '[-18,49]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'EndTemperature',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Waypoints File',
        Description = 'Upload a copy of your waypoints file',
        Units = NULL,
        Validation = NULL,
        DataType = NULL,
        PossibleValues = NULL,
        DbColumnName = '',
        ControlType = 'temp-waypoint-file',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Start Easting',
        Description = 'Easting location of the Start location of the Spawning Survey',
        Units = NULL,
        Validation = '[100000,999999]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'StartEasting',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Start Northing',
        Description = 'Northing location of the Start Location of the Spawning Survey',
        Units = NULL,
        Validation = '[1000000,9999999]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'StartNorthing',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'End Easting',
        Description = 'Easting Location of the End Location of the Spawning Survey',
        Units = NULL,
        Validation = '[100000,999999]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'EndEasting',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'End Northing',
        Description = 'Northing Location of the End Location of the Spawning survey',
        Units = NULL,
        Validation = '[1000000,9999999]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'EndNorthing',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Flow',
        Description = 'Condition of the stream at the time of the Spawning survey',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Dry"", ""Low"", ""Moderate"", ""High"", ""Flooding""]',
        DbColumnName = 'Flow',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Water Visibility',
        Description = 'Can see bottom of Riffles and Pools (1), Can see bottom of riffles (2), Cannot see bottom of riffles or pools (3)',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Riffles and Pools"",""Riffles"",""Neither Riffles or Pools""]',
        DbColumnName = 'WaterVisibility',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Weather',
        Description = 'Describes the current weather conditions:',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues =  '[""Clear (C)"", ""Overcast (O)"", ""Rain (R)"", ""Snow (S)"", ""Foggy (F)"", ""Partly Cloudy (P)""]',
        DbColumnName = 'Weather',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Flagged Redds',
        Description = 'Number of old Redds that were already flagged ',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'FlaggedRedds',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'New Redds',
        Description = 'Number of new Redds found during the Spawning Survey',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'NewRedds',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Survey Comments',
        Description = 'Comments about the site or any other observations',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'HeaderComments',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Fieldsheet Link',
        Description = 'Paper scan of the fieldsheet',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'FieldsheetLink',
        ControlType = 'file',
        [Rule] = NULL

update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header

-----
-- Details Fields

INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo
SELECT FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Feature Number',
        Description = 'Number assigned',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'FeatureID',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Feature Type',
        Description = 'Type of observation',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Redd"", ""Live"", ""Carcass"", ""Observation""]',
        DbColumnName = 'FeatureType',
        ControlType = 'select',
        [Rule] = NULL


UNION ALL SELECT FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Species',
        Description = 'Species of the carcass found',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""CHF"", ""CO"", ""STS"", ""CHS"", ""PL""]',
        DbColumnName = 'Species',
        ControlType = 'select',
        [Rule] = NULL
         
UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Time',
        Description = 'Time the feature was discovered',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'Time',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Temp',
        Description = 'Water temperature at the feature',
        Units = 'C',
        Validation = '[-18,49]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'Temp',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Waypoint Number',
        Description = 'Waypoint Number as assigned by the GPS device',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'WaypointNumber',
        ControlType = 'number',
        [Rule] = '{""OnChange"": ""if(scope.waypoints){var w=scope.waypoints[value]; if(w){row[''NorthingUTM'']=w.y;row[''EastingUTM'']=w.x;}}""}'


UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Easting (UTM)',
        Description = 'NAD 83 Zone 11N UTM Easting (X or Longitude) coordinates for the site',
        Units = NULL,
        Validation = '[100000,999999]',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'EastingUTM',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Northing (UTM)',
        Description = 'NAD 83 Zone 11N UTM Northing (Y or Latitude) coordinates for the site',
        Units = NULL,
        Validation = '[1000000,9999999]',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'NorthingUTM',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Channel',
        Description = 'Channel type the Redd is located in',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Braided (B)"", ""Single (S)""]',
        DbColumnName = 'Channel',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Redd Location',
        Description = 'Location of the Redd',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Middle (M)"", ""Side (S)""]',
        DbColumnName = 'ReddLocation',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Redd Habitat',
        Description = 'Habitat the Redd is located in',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Pool (P)"", ""Pool Tail Out (TO)"", ""Riffle (RI)"", ""Glide (GL)""]',
        DbColumnName = 'ReddHabitat',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Fish Count',
        Description = 'Number of carcasses or live fish on the Redd',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'FishCount',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Fish Location',
        Description = 'Location of the fish to the Redd',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Near Redd (NR)"", ""Off Redd (OR)""]',
        DbColumnName = 'FishLocation',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Sex',
        Description = 'Sex',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Male (M)"", ""Female (F)"", ""Unknown (UNK)""]',
        DbColumnName = 'Sex',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Fin Clips',
        Description = 'Type of Fin Clips on the live/carcass',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = (select PossibleValues from dbo.Fields where name = 'Fin Clip'),
        DbColumnName = 'FinClips',
        ControlType = 'multiselect',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Marks',
        Description = 'Any man made mark',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""NONE"",""NA"",""1ROP"",""1LOP"",""2ROP"",""2LOP"",""3ROP"",""3LOP"",""1TAIL"",""2TAIL"",""1CAU"",""2CAU"",""3CAU"",""4CAU""]',
        DbColumnName = 'Marks',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Spawning Status',
        Description = 'Percent of Eggs or Sperm the carcass has RETAINED (i.e. 100 would indicate a pre-spawn mortality)',
        Units = '%',
        Validation = '[0,100]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'SpawningStatus',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Fork Length',
        Description = 'Fork Length of the Carcass',
        Units = 'mm',
        Validation = '[1,1300]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'ForkLength',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MeHP Length',
        Description = 'Mid Eye to Hypural Plate Length',
        Units = 'mm',
        Validation = '[1,1300]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MeHPLength',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Snout ID',
        Description = 'Id from the snout card',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'SnoutID',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Scale ID',
        Description = 'Id from the scale card',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ScaleID',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Tag',
        Description = 'Does the fish have a tag of any sort?',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""WIRE"",""RADIO"",""FLOY"",""PIT"",""VIE"",""OTHER"",""NONE""]',
        DbColumnName = 'Tag',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Tag ID',
        Description = 'Identification number on the tag if available',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'TagID',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Comments',
        Description = 'Any comments about the live/carcass or the Redd',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'Comments',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Ident',
        Description = 'Identification number attributed to the point (Waypoint number)',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'Ident',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Waypoint Date/Time',
        Description = 'Date and Time the waypoint was taken',
        Units = NULL,
        Validation = NULL,
        DataType = 'date',
        PossibleValues = NULL,
        DbColumnName = 'DateTime',
        ControlType = 'text',
        [Rule] = NULL

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



CREATE TABLE #NewLocationIds (id int)        

INSERT INTO dbo.Locations(LocationTypeId, SdeFeatureClassId, SdeObjectId, Label, CreateDateTime)
OUTPUT INSERTED.id into #NewLocationIds

SELECT
  LocationTypeId        = 1,
  SdeFeatureClassId     = 2,
  SdeObjectId           = 33, 
  Label                 = 'Buckaroo 1',
  CreateDateTime        = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 34, 
    Label               = 'Buckaroo 2',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 42, 
    Label               = 'East Meacham 1',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 31, 
    Label               = 'Isqúulktpe 1',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 32, 
    Label               = 'Isqúulktpe 2',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 55, 
    Label               = 'Little Lookingglass Creek - Unit 4',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 51, 
    Label               = 'Lookingglass Creek - Unit 1',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 52, 
    Label               = 'Lookingglass Creek - Unit 2',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 53, 
    Label               = 'Lookingglass Creek - Unit 3L',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 54, 
    Label               = 'Lookingglass Creek - Unit 3U',
    CreateDateTime      = GetDate()


UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 36, 
    Label               = 'Meacham 1',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 37, 
    Label               = 'Meacham 2',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 38, 
    Label               = 'Meacham 3',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 39, 
    Label               = 'Meacham 4',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 40, 
    Label               = 'Meacham 5',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 41, 
    Label               = 'Meacham 6',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 43, 
    Label               = 'Meacham 7',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 44, 
    Label               = 'Meacham 8',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 35, 
    Label               = 'Moonshine 1',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 29, 
    Label               = 'NF Umatilla 1',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 30, 
    Label               = 'NF Umatilla 2',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 49, 
    Label               = 'Pearson 1',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 50, 
    Label               = 'Pearson 2',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 27, 
    Label               = 'SF Umatilla 1',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 28, 
    Label               = 'Thomas Creek 1',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 1,  
    Label               = 'Umatilla 1',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 2,  
    Label               = 'Umatilla 2',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 3,  
    Label               = 'Umatilla 3',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 4,  
    Label               = 'Umatilla 4',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 5,  
    Label               = 'Umatilla 5',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 6,  
    Label               = 'Umatilla 6',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 7,  
    Label               = 'Umatilla 7',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 8,  
    Label               = 'Umatilla 8',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 9,  
    Label               = 'Umatilla 9',
    CreateDateTime      = GetDate()


UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 10, 
    Label               = 'Umatilla 10',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 11, 
    Label               = 'Umatilla 11',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 12, 
    Label               = 'Umatilla 12',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 13, 
    Label               = 'Umatilla 13',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 14, 
    Label               = 'Umatilla 14',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 15, 
    Label               = 'Umatilla 15',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 16, 
    Label               = 'Umatilla 16',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 17, 
    Label               = 'Umatilla 17',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 18, 
    Label               = 'Umatilla 18',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 19, 
    Label               = 'Umatilla 19',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 20, 
    Label               = 'Umatilla 20',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 21, 
    Label               = 'Umatilla 21',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 22, 
    Label               = 'Umatilla 22',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 23, 
    Label               = 'Umatilla 23',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 24, 
    Label               = 'Umatilla 24',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 25, 
    Label               = 'Umatilla 25',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 26, 
    Label               = 'Umatilla 26',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 45, 
    Label               = 'Willdhorse 1',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 46, 
    Label               = 'Wildhorse 2',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 47, 
    Label               = 'Wildhorse 3',
    CreateDateTime      = GetDate()

UNION ALL SELECT
    LocationTypeId      = 7,
    SdeFeatureClassId   = 2,
    SdeObjectId         = 48, 
    Label               = 'Wildhorse 4',
    CreateDateTime      = GetDate()



-- Associate the locations above with our new projects
INSERT INTO dbo.LocationProjects(Location_Id, Project_Id)
SELECT
    Location_Id  = l.id,
    Project_Id = p.id
FROM #NewLocationIds as l, #ProjectInfo as p
 


-- Cleanup
drop table #ProjectInfo
drop table #NewFieldInfo
drop table #NewDatasetIds
drop table #QaStatusIds
drop table #NewLocationIds
");

        }

        public override void Down()
        {
            Sql(@"

-- Note: These MUST match values declared in Up()
declare @datasetBaseName as varchar(max) = 'Spawning Ground Survey'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = @datasetBaseName

delete from dbo.Locations where id in (select location_id from dbo.LocationProjects where project_id in (select ProjectId from datasets where name = @datasetBaseName))
delete from dbo.LocationProjects where project_id in (select ProjectId from datasets where name = @datasetBaseName)
delete from dbo.DatasetQAStatus  where Dataset_Id in (select id from dbo.Datasets where name = @datasetBaseName)
delete from dbo.DatasetQAStatus1 where Dataset_Id in (select id from dbo.Datasets where name = @datasetBaseName)
delete from dbo.DatasetFields where DatasetId in (select id from dbo.Datasets where name = @datasetBaseName)
delete from dbo.Fields where FieldCategoryId in (select id from dbo.FieldCategories where name = @categoryName)
delete from dbo.Datasets where name = @datasetBaseName
delete from dbo.FieldCategories where name = @categoryName
delete from dbo.Datastores where name = @datastoreName

");
        }
    }
}
