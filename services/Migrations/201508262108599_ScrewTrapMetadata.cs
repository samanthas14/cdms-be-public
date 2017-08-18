namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScrewTrapMetadata : DbMigration
    {
        public override void Up()
        {
            Sql(@"

-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'Screw Trap'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = @datasetBaseName


-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 1135   -- Grande Ronde Basin Spring Chinook Salmon Supplementation Monitoring and Evaluation
UNION ALL SELECT id = 1188   -- Umatilla Basin Natural Production Monitoring and Evaluation
UNION ALL SELECT id = 1177   -- Walla Walla Basin Monitoring and Evaluation
UNION ALL SELECT id = 1161   -- Pacific Lamprey Research and Restoration


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
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), 
                            [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    

-----
-- Insert any new fields we'll need (George says don't reuse fields)
-- Header Fields
INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'File Title',
   Description = 'File name of the associated P3 file',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'FileTitle',
   ControlType = 'file',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Clip Files',
   Description = 'File number used for the clips taken from the fish',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'ClipFiles',
   ControlType = 'text',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Tag DateTime',
   Description = 'P3-Date the tagging activity took place',
   Units = NULL,
   Validation = NULL,
   DataType = 'date',
   PossibleValues = NULL,
   DbColumnName = 'TagDateTime',
   ControlType = 'date',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Release DateTime',
   Description = 'P3-Date time the fish were released',
   Units = NULL,
   Validation = NULL,
   DataType = 'date',
   PossibleValues = NULL,
   DbColumnName = 'ReleaseDateTime',
   ControlType = 'date',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Tagger',
   Description = 'P3-Persons performing the trap check',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'Tagger',
   ControlType = 'text',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Capture Method',
   Description = 'P3-Method used to capture the fish',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'CaptureMethod',
   ControlType = 'text',
   [Rule] = '{""DefaultValue"":""SCREWT""}'
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Migratory Year',
   Description = 'P3-Year the fish migrate',
   Units = NULL,
   Validation = NULL,
   DataType = 'int',
   PossibleValues = NULL,
   DbColumnName = 'MigratoryYear',
   ControlType = 'number',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Livewell Temp',
   Description = 'Temperature of the water at the start of the survey',
   Units = 'C',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'LivewellTemp',
   ControlType = 'number',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Tagging Temp',
   Description = 'P3-Temperature of the water during the tagging operation',
   Units = 'C',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'TaggingTemp',
   ControlType = 'number',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Post Tagging Temp',
   Description = 'P3-Temperature of the water at the end of the tagging activity',
   Units = 'C',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'PostTaggingTemp',
   ControlType = 'number',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Release Temp',
   Description = 'P3-Temperature of the water at the time the fish were released',
   Units = 'C',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'ReleaseTemp',
   ControlType = 'number',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'TaggingMethod',
   Description = 'P3-Method used to tage the fish',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'TaggingMethod',
   ControlType = 'text',
   [Rule] = '{""DefaultValue"":""HAND""}'
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Organization',
   Description = 'P3-Organization doing the tagging',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'Organization',
   ControlType = 'text',
   [Rule] = '{""DefaultValue"":""CTUIR""}'
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Coordinator ID',
   Description = 'P3-Acronym for the tagging coordniator',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'CoordinatorID',
   ControlType = 'text',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Arrival Time',
   Description = 'Time the crew arrived at the site',
   Units = '00:00',
   Validation = NULL,
   DataType = 'time',
   PossibleValues = NULL,
   DbColumnName = 'ArrivalTime',
   ControlType = 'time',  
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Depart Time',
   Description = 'Time the crew left the site',
   Units = '00:00',
   Validation = NULL,
   DataType = 'time',
   PossibleValues = NULL,
   DbColumnName = 'DepartTime',
   ControlType = 'time',  
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Arrival RPMs',
   Description = 'Speed of the cone at the time of arrival',
   Units = 'rpm',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'ArrivalRPMs',
   ControlType = 'number',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Departure RPMs',
   Description = 'Speed of the cone at the time of departure',
   Units = 'rpm',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'DepartureRPMs',
   ControlType = 'number',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Hubometer',
   Description = 'Number of the hub on the trap',
   Units = NULL,
   Validation = NULL,
   DataType = 'int',
   PossibleValues = NULL,
   DbColumnName = 'Hubometer',
   ControlType = 'number',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Hubometer Time',
   Description = 'Time the hubometer was read',
   Units = '00:00',
   Validation = NULL,
   DataType = 'time',
   PossibleValues = NULL,
   DbColumnName = 'HubometerTime',
   ControlType = 'time',  
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Trap Stopped',
   Description = 'Time the Trap was stopped for sampling',
   Units = '00:00',
   Validation = NULL,
   DataType = 'time',
   PossibleValues = NULL,
   DbColumnName = 'TrapStopped',
   ControlType = 'time',  
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Trap Started',
   Description = 'Time the Trap was started after sampling',
   Units = '00:00',
   Validation = NULL,
   DataType = 'time',
   PossibleValues = NULL,
   DbColumnName = 'TrapStarted',
   ControlType = 'time',  
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Fish Collected',
   Description = 'Time the fish were collected from the live well',
   Units = '00:00',
   Validation = NULL,
   DataType = 'time',
   PossibleValues = NULL,
   DbColumnName = 'FishCollected',
   ControlType = 'time',  
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Fish Released',
   Description = 'Time of the fish were released after the activity',
   Units = '00:00',
   Validation = NULL,
   DataType = 'time',
   PossibleValues = NULL,
   DbColumnName = 'FishReleased',
   ControlType = 'time',  
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Flow',
   Description = 'Condition of the stream at the time of the Spawning survey.  Dry- no flow; Low-stream covers <50% of active channel width; Moderate-stream covers 50%-75% of active channel width; High-steam covers>75% of active channel width; Flooding-stream is out of its banks',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""Dry"", ""Low"", ""Moderate"", ""High"", ""Flooding""]',
   DbColumnName = 'Flow',
   ControlType = 'select',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Turbitity',
   Description = 'Estimation of the amount of sediment in the stream',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""Clear"", ""Olive"", ""Lt. Brown"", ""Dk. Brown""]',
   DbColumnName = 'Turbitity',
   ControlType = 'select',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Trap Debris',
   Description = 'Amount of debris in the trap',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""None"", ""Light"", ""Moderate"", ""Heavy"", ""Very Heavy"", ""Full Livewell"", ""Full Cone"", ""Stopped""]',
   DbColumnName = 'TrapDebris',
   ControlType = 'select',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'River Debris',
   Description = 'Amount of debris in the river',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""None"", ""Low"", ""Moderate"", ""Moderate Heavy"", ""Heavy""]',
   DbColumnName = 'RiverDebris',
   ControlType = 'select',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Task',
   Description = 'Tasks performed at the trap',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""Start Trap"", ""Remove Frish from Trap"", ""Stop Trap"", ""C Trap"", ""Pit-tag Fish""]',
   DbColumnName = 'Task',
   ControlType = 'multiselect',
   [Rule] = NULL

UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Activity Comments',
   Description = 'Any comments about the activity',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'ActivityComments',
   ControlType = 'text',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Release Site',
   Description = 'P3-Location the fish were released',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'ReleaseSite',
   ControlType = 'text',
   [Rule] = NULL
  
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Release River KM',
   Description = 'P3-Kilometer where the fish were released',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'ReleaseRiverKM',
   ControlType = 'text',
   [Rule] = NULL



update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header

-----
-- Details Fields

INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Sequence',
   Description = 'P3-Sequence the fish was labeled in the system',
   Units = NULL,
   Validation = NULL,
   DataType = 'int',
   PossibleValues = NULL,
   DbColumnName = 'Sequence',
   ControlType = 'number',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Pit Tag Code',
   Description = 'P3-Code of the pit tag inserted into the fish',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'PitTagCode',
   ControlType = 'text',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Species Run Rearing',
   Description = 'P3 code used for the fish (i.e. 11W, 32W',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '{""00U"":""Unknown (fish not observed)"", ""11H"":""Hat. Spring Chinook"", ""11U"":""Spring Chinook (unknown r/t)"", ""11W"":""Wild Spring Chinook"", ""13H"":""Hat. Fall Chinook"", ""13W"":""Wild Fall Chinook"", ""25H"":""Hat. Coho"", ""25W"":""Wild Coho"", ""32H"":""Hat. Summer Steelhead"", ""32W"":""Wild Summer Steelhead"", ""3RH"":""Hat. Rainbow Trout"", ""3RW"":""Wild Rainbow Trout"", ""7RW"":""Bull Trout"", ""90U"":""Other"", ""A0W"":""Lamprey"", ""D0W"":""Northern Pikeminnow"", ""ERU"":""Brook Trout"", ""G0W"":""Mountain Whitefish""}',
   DbColumnName = 'SpeciesRunRearing',
   ControlType = 'select',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Fork Length',
   Description = 'P3-Fork Length of the fish (mm)',
   Units = 'mm',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'ForkLength',
   ControlType = 'number',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Weight',
   Description = 'p3-Weight of the fish',
   Units = 'g',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'Weight',
   ControlType = 'number',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Other Species',
   Description = 'Other Species of fish caught used for manual data entry',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""Dace"", ""Sculpin"", ""Sucker"", ""Shiner"", ""NP Minnow"", ""Crayfish"", ""Bullhead Catfish"", ""White Fish"", ""Hatchery CHS"", ""Hatchery STS"", ""Chinook Fry"", ""Steelhead Fry"", ""Lamprey-Ammo"", ""Lamprey-Macro"", ""Other""]',
   DbColumnName = 'OtherSpecies',
   ControlType = 'select',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Fish Count',
   Description = 'Number of fish of a specific species caught',
   Units = NULL,
   Validation = NULL,
   DataType = 'int',
   PossibleValues = NULL,
   DbColumnName = 'FishCount',
   ControlType = 'number',
   [Rule] = '{""DefaultValue"":""1""}'
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Conditional Comment',
   Description = 'P3-Flag codes for P3 (ex RE, BT, M, KL, JA)',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'ConditionalComment',
   ControlType = 'text',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Textual Comments',
   Description = 'P3-Ad hoc annotations unique to individual tag',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'TextualComments',
   ControlType = 'text',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Note',
   Description = 'P3-Any notes that are ad hoc. Global comments pertaining to the tagging session',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'Note',
   ControlType = 'text',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Release Location',
   Description = 'The spot where the fish were released after the activity',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""Upstream"", ""Downstream"", ""Livewell""]',
   DbColumnName = 'ReleaseLocation',
   ControlType = 'select',  
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Tag',
   Description = 'Type of Tag',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""New"", ""Recap"", ""None""]',
   DbColumnName = 'Tag',
   ControlType = 'select',   
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Clip',
   Description = 'Type of fin clip on the fish',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""New"", ""Recap"",""None""]',
   DbColumnName = 'Clip',
   ControlType = 'select',   
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Fish Comments',
   Description = 'Comments about the individual fish',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'FishComments',
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



-- Cleanup
drop table #ProjectInfo
drop table #NewFieldInfo
drop table #NewDatasetIds
drop table #QaStatusIds
");

        }

        public override void Down()
        {
            Sql(@"

-- Note: These MUST match values declared in Up()
declare @datasetBaseName as varchar(max) = 'Screw Trap'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = @datasetBaseName

--delete from dbo.Locations where id in (select location_id from dbo.LocationProjects where project_id in (select ProjectId from datasets where name = 'Snorkel Fish'))
--delete from dbo.LocationProjects where project_id in (select ProjectId from datasets where name = 'Snorkel Fish')
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
