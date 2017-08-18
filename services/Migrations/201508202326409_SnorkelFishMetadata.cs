namespace services.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SnorkelFishMetadata : DbMigration
    {
        public override void Up()
        {
            Sql(@"

-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'Snorkel Fish'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = @datasetBaseName


-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 1135  -- Grande Ronde Basin Spring Chinook Salmon Supplementation Monitoring and Evaluation (Project Id=1135)
UNION ALL SELECT id = 2249  -- Biomonitoring of Fish Habitat Enhancement (Project Id=2249)

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
    Config                    = ''
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
    Name = 'Crew',
    Description = 'Team/Program that Conducted the Survey',
    Units = NULL,
    Validation = NULL,
    DataType = 'string',
    PossibleValues = NULL,
    DbColumnName = 'Team',
    ControlType = 'text',
    [Rule] = NULL

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'Water Temperature',
    Description = 'Temperature of the water at the start of the survey',
    Units = 'C',
    Validation = NULL,
    DataType = 'double',
    PossibleValues = NULL,
    DbColumnName = 'WaterTemperature',
    ControlType = 'number',
    [Rule] = NULL

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'Visibility',
    Description = 'How clear is the water at the time of the survey',
    Units = NULL,
    Validation = NULL,
    DataType = 'number',
    PossibleValues = '{""0"":""Not snorkelable due to high turbidity or cover"",""1"":""High amount of cover and/or poor water clarity"",""2"":""Moderate cover and/or moderate water clarity"",""3"":""Little hiding cover and good water clarity""}',
    DbColumnName = 'Visibility',
    ControlType = 'select',
    [Rule] = NULL

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'VisitId',
    Description = 'CHaMP assigned ID',
    Units = NULL,
    Validation = NULL,
    DataType = 'int',
    PossibleValues = NULL,
    DbColumnName = 'VisitId',
    ControlType = 'number',
    [Rule] = NULL

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'Comments',
    Description = 'Comments about the survey as a whole',
    Units = NULL,
    Validation = NULL,
    DataType = 'string',
    PossibleValues = NULL,
    DbColumnName = 'Comments',
    ControlType = 'text',
    [Rule] = NULL

update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header

-----
-- Details Fields

INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'FishID',
    Description = 'Line number of the data for the fish entry',
    Units = NULL,
    Validation = NULL,
    DataType = 'int',
    PossibleValues = NULL,
    DbColumnName = 'FishID',
    ControlType = 'number',
    [Rule] = NULL

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'ChannelUnit',
    Description = 'Number or Text delineating the Channel',
    Units = NULL,
    Validation = NULL,
    DataType = 'string',
    PossibleValues = NULL,
    DbColumnName = 'ChannelUnit',
    ControlType = 'text',
    [Rule] = NULL

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'ChannelUnitNumber',
    Description = 'Unit number of the current survey',
    Units = NULL,
    Validation = NULL,
    DataType = 'int',
    PossibleValues = NULL,
    DbColumnName = 'ChannelUnitNumber',
    ControlType = 'number',
    [Rule] = NULL

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'Lane',
    Description = 'Designated survey line for the snorkler',
    Units = NULL,
    Validation = NULL,
    DataType = 'int',
    PossibleValues = '[""1"",""2"",""3""]',
    DbColumnName = 'Lane',
    ControlType = 'select',
    [Rule] = NULL

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'HabitatType',
    Description = 'Habitat Notes: Fast = fish is actively swimming to maintain position; Slow = fish is easily maintaining position without much effort; Edge = BFW > 25m, velocity < 0.15 m/s, depth < 0.61m; Backwater = A naturally or artificially formed arm or area of standing or slow moving water partially isolated from the flow of the main channel of a stream.',
    Units = NULL,
    Validation = NULL,
    DataType = 'string',
    PossibleValues = '{""FAST"":""Fast"",""SLOW"":""Slow"",""EDGE"":""Edge"",""BACKWATER"":""Backwater""}',
    DbColumnName = 'HabitatType',
    ControlType = 'select',
    [Rule] = NULL

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'FishCount',
    Description = 'Total number of a single species of fish observed in the unit during the survey',
    Units = NULL,
    Validation = NULL,
    DataType = 'int',
    PossibleValues = NULL,
    DbColumnName = 'FishCount',
    ControlType = 'number',
    [Rule] = NULL

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'Species',
    Description = 'Species of fish observed in the current unit during the survery',
    Units = NULL,
    Validation = NULL,
    DataType = 'string',
    PossibleValues = '{""O. MYKISS"":""O. mykiss"",""STEELHEAD/RAINBOW TROUT"":""Steelhead/rainbow trout"",""CHINOOK SALMON"":""Chinook salmon"",""BULL TROUT"":""Bull trout"",""REDSIDE SHINER"":""Redside shiner"",""NORTHERN PIKEMINNOW"":""Northern pikeminnow"",""MOUNTAIN WHITEFISH"":""Mountain whitefish"",""BRIDGELIP SUCKER"":""Bridgelip sucker"",""MOUNTAIN SUCKER"":""Mountain sucker"",""LARGESCALE SUCKER"":""Largescale sucker"",""LONGNOSE DACE"":""Longnose dace"",""SPECKLED DACE"":""Speckled dace"",""CARP"":""Carp"",""LAMPREY"":""Lamprey"",""CHISELMOUTH"":""Chiselmouth"",""COTTIDAE (SCUPLINS)"":""Cottidae (scuplins)"",""ICHTHILURIDAE (CATFISHES)"":""Ichthiluridae (catfishes)"",""CENTRARCHIDS (SUNFISHES)"":""Centrarchids (sunfishes)""}',
    DbColumnName = 'Species',
    ControlType = 'select',
    [Rule] = NULL
    --[Rule] =  '{""OnChange"": ""debugger;if(row.Species==\""BULL TROUT\""||row.Species==\""STEELHEAD/RAINBOW TROUT\"") scope.CellOptions.LengthOptions={\""<80\"":\""<80\"",\""80-129\"":\""80-129\"",\""130-199\"":\""130-199\"",\""200+\"":\""200+\"",\""ADULT\"":\""Adult\""};  else if(row.Species==\""CHINOOK SALMON\"") scope.CellOptions.LengthOptions={\""<100\"":\""<100\"",\""100+\"":\""100+\"",\""ADULT\"":\""Adult\""}; else scope.CellOptions.LengthOptions={\""NO OPTIONS\"":\""No Options\""}; ""}'

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'SizeClass',
    Description = 'Average size of the group of fish species observed in the current unit (deliniated every 10 cm)',
    Units = NULL,
    Validation = NULL,
    DataType = 'string',
    PossibleValues = NULL,
    DbColumnName = 'SizeClass',
    ControlType = 'text',
    [Rule] = NULL

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'Length',
    Description = 'Size classes for non AEM snorkel events Salmonids',
    Units = NULL,
    Validation = NULL,
    DataType = 'string',
    PossibleValues = '[""BUT/STS <80"", ""BUT/STS 80-129"", ""BUT/STS 130-199"", ""BUT/STS 200+"", ""CH <100"", ""CH 100+"", ""Adult""]',
    DbColumnName = 'Length',
    ControlType = 'select',
    [Rule] = NULL

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'NaturalWoodUsed',
    Description = '',
    Units = NULL,
    Validation = NULL,
    DataType = 'string',
    PossibleValues = '{""YES"":""Yes"",""NO"":""No""}',
    DbColumnName = 'NaturalWoodUsed',
    ControlType = 'select',
    [Rule] = '{""DefaultValue"":""NO""}'

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'PlacedWoodUsed',
    Description = '',
    Units = NULL,
    Validation = NULL,
    DataType = 'string',
    PossibleValues = '{""YES"":""Yes"",""NO"":""No""}',
    DbColumnName = 'PlacedWoodUsed',
    ControlType = 'select',
    [Rule] = '{""DefaultValue"":""NO""}'

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'NaturalBoulderUsed',
    Description = '',
    Units = NULL,
    Validation = NULL,
    DataType = 'string',
    PossibleValues = '{""YES"":""Yes"",""NO"":""No""}',
    DbColumnName = 'NaturalBoulderUsed',
    ControlType = 'select',
    [Rule] = '{""DefaultValue"":""NO""}'

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'PlacedBoulderUsed',
    Description = '',
    Units = NULL,
    Validation = NULL,
    DataType = 'string',
    PossibleValues = '{""YES"":""Yes"",""NO"":""No""}',
    DbColumnName = 'PlacedBoulderUsed',
    ControlType = 'select',
    [Rule] = '{""DefaultValue"":""NO"" }'

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'NaturalOffChannelUsed',
    Description = '',
    Units = NULL,
    Validation = NULL,
    DataType = 'string',
    PossibleValues = '{""YES"":""Yes"",""NO"":""No""}',
    DbColumnName = 'NaturalOffChannelUsed',
    ControlType = 'select',
    [Rule] = '{""DefaultValue"":""NO""}'

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'CreatedOffChannelUsed',
    Description = '',
    Units = NULL,
    Validation = NULL,
    DataType = 'string',
    PossibleValues = '{""YES"":""Yes"",""NO"":""No""}',
    DbColumnName = 'CreatedOffChannelUsed',
    ControlType = 'select',
    [Rule] = '{""DefaultValue"":""NO""}'

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'NewSideChannelUsed',
    Description = '',
    Units = NULL,
    Validation = NULL,
    DataType = 'string',
    PossibleValues = '{""YES"":""Yes"",""NO"":""No""}',
    DbColumnName = 'NewSideChannelUsed',
    ControlType = 'select',
    [Rule] = '{""DefaultValue"":""NO""}'

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'NoStructureUsed',
    Description = '',
    Units = NULL,
    Validation = NULL,
    DataType = 'string',
    PossibleValues = '{""YES"":""Yes"",""NO"":""No""}',
    DbColumnName = 'NoStructureUsed',
    ControlType = 'select',
    [Rule] = '{""DefaultValue"":""NO""}'

UNION ALL SELECT   
    FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
    Name = 'FieldNotes',
    Description = 'General comments about the unit or fish',
    Units = NULL,
    Validation = NULL,
    DataType = 'string',
    PossibleValues = NULL,
    DbColumnName = 'FieldNotes',
    ControlType = 'text',
    [Rule] = NULL



update #NewFieldInfo set FieldRoleId = 2 where FieldRoleId is NULL   -- 2 == details


update datasets set config = '{""DataEntryPage"": {""HiddenFields"": [""Instrument""]}}' where id in (select id from #NewDatasetIds)


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
declare @datasetBaseName as varchar(max) = 'Snorkel Fish'
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
