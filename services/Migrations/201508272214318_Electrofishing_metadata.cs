namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Electrofishing_metadata : DbMigration
    {
        public override void Up()
        {
            Sql(@"

-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'Electrofishing'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = @datasetBaseName


-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 2249       -- Biomonitoring of Fish Habitat Enhancement 
UNION ALL SELECT id = 2232       -- Fish Passage Operations in the Umatilla
UNION ALL SELECT id = 1135       -- Grande Ronde Basin Spring Chinook Salmon Supplementation Monitoring and Evaluation 
UNION ALL SELECT id = 1161       -- Pacific Lamprey Research and Restoration 
UNION ALL SELECT id = 1140       -- Steelhead Supplementation Evaluation 
UNION ALL SELECT id = 1188       -- Umatilla Basin Natural Production Monitoring and Evaluation 


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
   Name = 'Event Type',
   Description = 'What type of even is this Mark; Recapture; Salvage; Other',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""Depletion"",""Mark"",""Recapture"",""Salvage"",""Other""]',
   DbColumnName = 'EventType',
   ControlType = 'select',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'File Title',
   Description = 'File name of the associated P3 file',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'FileTitle',
   ControlType = 'upload',
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
   DataType = 'DateTime',
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
   DataType = 'DateTime',
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
   Name = 'Crew',
   Description = 'Initials of the entire shocking crew',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'Crew',
   ControlType = 'text',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Capture Method',
   Description = 'P3-Method used to capture the fish',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '{""SHOCK"":""Shock"",""SEIN"":""Sein""}',
   DbColumnName = 'CaptureMethod',
   ControlType = 'select',
   [Rule] = '{""DefaultValue"":""SHOCK""}'
   
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
   Name = 'Tagging Method',
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
   Name = 'Conductivity',
   Description = 'Electrical conductivity of the water (micromhos per centimeter )',
   Units = 'µmhos/cm',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'Conductivity',
   ControlType = 'number',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'EF Model',
   Description = 'Model of the electroshocker',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'EFModel',
   ControlType = 'text',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Site Length',
   Description = 'Length of the site',
   Units = 'm',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'SiteLength',
   ControlType = 'number',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Site Width',
   Description = 'Width of the site in meters',
   Units = 'm',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'SiteWidth',
   ControlType = 'number',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Site Depth',
   Description = 'Mean depth of the site in meters',
   Units = 'm',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'SiteDepth',
   ControlType = 'number',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Site Area',
   Description = 'Area of the site in meters squared',
   Units = 'm2',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'SiteArea',
   ControlType = 'number',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Habitat Type',
   Description = 'Stream habitat type associated with the unit',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'HabitatType',
   ControlType = 'text',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Visibility',
   Description = 'Estimation of the amount of sediment in the stream',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""1-Can see bottom of Riffles and Pools"",""2-Can see bottom of riffles"",""3-Cannot see bottom of riffles or pools""]',
   DbColumnName = 'Visibility',
   ControlType = 'select',
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
   Name = 'Weather',
   Description = 'Current weather conditions',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""Sunny"",Partly ""Cloudy"",""Cloudy"",""Rain""]',
   DbColumnName = 'Weather',
   ControlType = 'select',
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
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'PassNumber',
   Description = 'Number of the electrofishing pass',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""1"",""2"",""3"",""4"",""5"",""6""]',
   DbColumnName = 'PassNumber',
   ControlType = 'select',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Time Begin',
   Description = 'Time Started shocking',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'TimeBegin',
   ControlType = 'time',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Time End',
   Description = 'Time Stopped shocking',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'TimeEnd',
   ControlType = 'time',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Total Seconds EF',
   Description = 'Total number of seconds shocked according to the electroshocker',
   Units = NULL,
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'TotalSecondsEF',
   ControlType = 'number',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Water Temp Begin',
   Description = 'Water Temperature at the start of the shocking pass',
   Units = 'C',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'WaterTempBegin',
   ControlType = 'number',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Water Temp Stop',
   Description = 'Water Temperature at the stop of the shocking pass',
   Units = 'C',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'WaterTempStop',
   ControlType = 'number',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Hertz',
   Description = 'Total number of Hertz the unit is producing',
   Units = 'Hz',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'Hertz',
   ControlType = 'number',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Freq',
   Description = 'Frequency that that the shocker is using',
   Units = 'Hz',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'Freq',
   ControlType = 'number',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Volts',
   Description = 'Number of volts the shocker is using',
   Units = 'V',
   Validation = NULL,
   DataType = 'double',
   PossibleValues = NULL,
   DbColumnName = 'Volts',
   ControlType = 'number',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Total Fish Captured',
   Description = 'Total number of fish captured on each pass',
   Units = NULL,
   Validation = NULL,
   DataType = 'int',
   PossibleValues = NULL,
   DbColumnName = 'TotalFishCaptured',
   ControlType = 'number',
   [Rule] = NULL



update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header

-----
-- Details Fields

INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Pass Number',
   Description = 'Electro fishing pass in which the fish was captured',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""1"",""2"",""3"",""4"",""5"",""6""]',
   DbColumnName = 'PassNumber',
   ControlType = 'select',
   [Rule] = NULL
   
UNION ALL SELECT
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
   Description = 'P3 code used for the fish (i.e. 11W, 32W)',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '{""00U"":""Unknown (fish not observed)"", ""11H"":""Hat. Spring Chinook"", ""11U"":""Spring Chinook (unknown r/t)"", ""11W"":""Wild Spring Chinook"", ""13H"":""Hat. Fall Chinook"", ""13W"":""Wild Fall Chinook"", ""25H"":""Hat. Coho"", ""25W"":""Wild Coho"", ""32H"":""Hat. Summer Steelhead"", ""32W"":""Wild Summer Steelhead"", ""3RH"":""Hat. Rainbow Trout"", ""3RW"":""Wild Rainbow Trout"", ""7RW"":""Bull Trout"", ""90U"":""Other"", ""A0W"":""Lamprey"", ""D0W"":""Northern Pikeminnow"", ""ERU"":""Brook Trout"", ""G0W"":""Mountain Whitefish""}',
   DbColumnName = 'SpeciesRunRearing',
   ControlType = 'text',
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
   PossibleValues = '[""Dace"",""Sculpin"",""Sucker"",""Shiner"",""NP Minnow"",""Crayfish"",""White Fish"",""Hatchery CHS"",""Hatchery STS"",""Chinook"",""Coho"",""Steelhead"",""Lamprey-Ammo"",""Lamprey-Macro"",""Other""]',
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
   Name = 'Fish Size',
   Description = 'Size range of Salmonids captured and hand counted. For manual entry',
   Units = 'mm',
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""CH <50"",""CH 50-80"",""BUT/STS <70"",""BUT/STS 70-129"",""BUT/STS 130-2""]',
   DbColumnName = 'FishSize',
   ControlType = 'select',
   [Rule] = NULL
   
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
   PossibleValues = '[""Upstream"",""Downstream"",""Livewell""]',
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
   PossibleValues = '[""New"",""Recap"",""None""]',
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
   PossibleValues = '[""New"",""Recap"",""None""]',
   DbColumnName = 'Clip',
   ControlType = 'select',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Otolith ID',
   Description = 'Otolith sample identification number assigned to the fish',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'OtolithID',
   ControlType = 'text',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Genetic ID',
   Description = 'Genetic sample identifcation number assigned to the fish',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'GeneticID',
   ControlType = 'text',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Other ID',
   Description = 'Other sample identifcation number assigned to the fish (pit, radio)',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'OtherID',
   ControlType = 'text',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Disposition',
   Description = 'Was the fish released or mortality',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""Release"",""Mortality""]',
   DbColumnName = 'Disposition',
   ControlType = 'select',
   [Rule] = '{""DefaultValue"":""Release""}'
   
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
declare @datasetBaseName as varchar(max) = 'Electrofishing'
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
