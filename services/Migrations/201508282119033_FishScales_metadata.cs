namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FishScales_metadata : DbMigration
    {
        public override void Up()
        {
            Sql(@"

-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'Fish Scales'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = @datasetBaseName


-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 1135    -- Grande Ronde Basin Spring Chinook Salmon Supplementation Monitoring and Evaluation
UNION ALL SELECT id = 1188    -- Umatilla Basin Natural Production Monitoring and Evaluation
UNION ALL SELECT id = 1177    -- Walla Walla Basin Monitoring and Evaluation


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
    Config                    = '{""DataEntryPage"": {""HiddenFields"": [""Instrument""]}}'     -- ,""Location""
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
   Name = 'Run Year',
   Description = 'Year the fish return to the system',
   Units = NULL,
   Validation = NULL,
   DataType = 'int',
   PossibleValues = NULL,
   DbColumnName = 'RunYear',
   ControlType = 'number',
   [Rule] = NULL

UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Technician',
   Description = 'Technician reading the scale',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'Technician',
   ControlType = 'text',
   [Rule] = NULL

update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header

-----
-- Details Fields

INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Scale ID',
   Description = 'Unique Identification number of either the scale envelope or the scale card and card position (i.e. MN-07-01-06 where MN-07-01 is the card number and 06 is the position of the scale on the gum card)',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'FieldScaleID',
   ControlType = 'text',
   [Rule] = NULL

UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Gum Card Scale ID',
   Description = 'Unique Identification number of either the scale envelope or the scale card and card position if remounted to a gum caed (i.e. MN-07-01-06 where MN-07-01 is the card number and 06 is the position of the scale on the gum card)',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'GumCardScaleID',
   ControlType = 'text',
   [Rule] = NULL

UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Scale Collection Date',
   Description = 'Scale Collection Date',
   Units = NULL,
   Validation = NULL,
   DataType = 'date',
   PossibleValues = NULL,
   DbColumnName = 'ScaleCollectionDate',
   ControlType = 'date',
   [Rule] = NULL

UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Species',
   Description = 'Species of fish the scale sample was taken from',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""BUT"", ""CHF"", ""CHS"", ""CO"", ""RBT"", ""STS""]',
   DbColumnName = 'Species',
   ControlType = 'select',
   [Rule] = NULL

UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Life Stage',
   Description = 'Was the scale taken from a Juvenile or an Adult fish',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""Adult"", ""Juvenile""]',
   DbColumnName = 'LifeStage',
   ControlType = 'select',
   [Rule] = '{""DefaultValue"":""Adult""}'

UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Circuli',
   Description = 'Number of Circuli before the first Annulus',
   Units = NULL,
   Validation = NULL,
   DataType = 'int',
   PossibleValues = NULL,
   DbColumnName = 'Circuli',
   ControlType = 'number',
   [Rule] = NULL

UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Juvenile Age',
   Description = 'Age of the juvenile fish at the time the scale sample was collected (i.e. 0+; 1+; 2; 2+; 3; 4)',
   Units = 'years',
   Validation = NULL,
   DataType = 'string',    
   PossibleValues = NULL,
   DbColumnName = 'JuvenileAge',
   ControlType = 'text',
   [Rule] = NULL

UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Freshwater Age',
   Description = 'Number of years the fish spent in freshwater (Steelhead)',
   Units = 'years',
   Validation = NULL,
   DataType = 'int',       
   PossibleValues = NULL,
   DbColumnName = 'FreshwaterAge',
   ControlType = 'number',
   [Rule] = NULL

UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Salt Water Age',
   Description = 'Number of years the fish spent in saltwater (Steelhead)',
   Units = 'years',
   Validation = NULL,
   DataType = 'int',      
   PossibleValues = NULL,
   DbColumnName = 'SaltWaterAge',
   ControlType = 'number',
   [Rule] = NULL

UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Total Adult Age',
   Description = 'European Age of the fish at the time the scale collected (All Adult Fish)',
   Units = 'years',
   Validation = NULL,
   DataType = 'int',
   PossibleValues = NULL,
   DbColumnName = 'TotalAdultAge',
   ControlType = 'number',
   [Rule] = NULL

UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Spawn Check',
   Description = 'Is there a spawn mortality check associated with this fish',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""Yes"", ""No""]',
   DbColumnName = 'SpawnCheck',
   ControlType = 'select',
   [Rule] = '{""DefaultValue"":""No""}'

UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Regeneration',
   Description = 'The scale that was read was a regenerated scale',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""Yes"", ""No""]',
   DbColumnName = 'Regeneration',
   ControlType = 'select',
   [Rule] = '{""DefaultValue"":""No""}'

UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Stock',
   Description = 'Natural extirpated stock versus the Rapid River (RR) era and the Catherine Creek (LGC) era',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '[""Blank"", ""LKG"", ""RR""]',
   DbColumnName = 'Stock',
   ControlType = 'select',
   [Rule] = '{""DefaultValue"":""Blank""}'

UNION ALL SELECT
   FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')),
   Name = 'Fish Comments',
   Description = 'Comments about the individual scales or fish',
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
declare @datasetBaseName as varchar(max) = 'Fish Scales'
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
