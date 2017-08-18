namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddP4DataToEf : DbMigration
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
          SELECT id = 1135   -- Grande Ronde Basin Spring Chinook Salmon Supplementation Monitoring and Evaluation
UNION ALL SELECT id = 1188   -- Umatilla Basin Natural Production Monitoring and Evaluation
UNION ALL SELECT id = 1177   -- Walla Walla Basin Monitoring and Evaluation
UNION ALL SELECT id = 1161   -- Pacific Lamprey Research and Restoration
UNION ALL SELECT id = 1140   -- Steelhead Supplementation Evaluation
UNION ALL SELECT id = 2232   -- Fish Passage Operations in the Umatilla River
UNION ALL SELECT id = 2249   -- Biomonitoring of Fish Habitat Enhancement

-- Grab the project names
update #ProjectInfo set name = (select name from projects where projects.id = #ProjectInfo.id)


-- Create a field category
/*INSERT INTO dbo.FieldCategories (Name, Description) 
SELECT Name        = @categoryName,
       Description = @datasetBaseName + ' related fields'
*/

-- Add records to the Datastores
/*INSERT INTO dbo.Datastores (Name, Description, TablePrefix, DatastoreDatasetId, OwnerUserId, FieldCategoryId) 
SELECT
	Name               = @datastoreName, 
	Description        = NULL, 
	TablePrefix        = REPLACE(@datastoreName, ' ', ''), -- Strip spaces
	DatastoreDatasetId = NULL, 
	OwnerUserId        = 1081,      -- George
	FieldCategoryId    = (SELECT IDENT_CURRENT('dbo.FieldCategories'))
*/

CREATE TABLE #NewDatasetIds (id int)        -- This will contain a list of ids of all dataset records inserted below
insert into #NewDatasetIds(id) values (1218), (1219), (1220), (1221), (1222), (1223), (1234)

-- Add record to the Datasets --> will create one record per project
/*INSERT INTO	Datasets (ProjectId, DefaultRowQAStatusId, StatusId, CreateDateTime, Name, Description, DefaultActivityQAStatusId, DatastoreId, Config)
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
*/
-------------------------

-- This will info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), 
                            [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    

-----
-- Insert any new fields we'll need (George says don't reuse fields)
-- Header Fields
--INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
--OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo


-----
-- Details Fields

INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT
   FieldCategoryId = 9,
   Name = 'Event Type',
   Description = 'P4-Type of event in our system',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '{""DEPLETION"":""Depletion"", ""MARK"":""Mark"", ""RECAP"":""Recapture"", ""SALVAGE"":""Salvage"", ""SINGLE-PASS"":""Single-pass"", ""RECOVER"":""Recover"", ""TALLY"":""Tally"", ""OTHER"":""Other""}',
   DbColumnName = 'EventTypeD',
   ControlType = 'select',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = 9,
   Name = 'SecondPitTag',
   Description = 'P4-Code of the second pit tag inserted into the fish',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'SecondPitTag',
   ControlType = 'text',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = 9,
   Name = 'Raceway/Transect/Tank',
   Description = 'P4-Raceway/Transect/Tank',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'RacewayTransectTank',
   ControlType = 'text',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = 9,
   Name = 'Life Stage',
   Description = 'P3-Life Stage',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '{""ADULT"":""Adult"",""JUVENILE"":""Juvenile"",""UNKNOWN"":""Unknown""}',
   DbColumnName = 'LifeStage',
   ControlType = 'select',
   [Rule] = NULL
   
/*UNION ALL SELECT
   FieldCategoryId = 9,
   Name = 'Genetic ID',
   Description = 'P4-Genitic ID of the fish',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'GeneticId',
   ControlType = 'text',
   [Rule] = NULL
*/   
UNION ALL SELECT
   FieldCategoryId = 9,
   Name = 'Coded Wire Tag',
   Description = 'P4-Coded wire tag',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '{""YES"":""Yes"",""NO"":""No""}',
   DbColumnName = 'CodedWireTag',
   ControlType = 'select',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = 9,
   Name = 'Brood Year',
   Description = 'P4-Brood year of the fish',
   Units = NULL,
   Validation = NULL,
   DataType = 'int',
   PossibleValues = NULL,
   DbColumnName = 'BroodYear',
   ControlType = 'number',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = 9,
   Name = 'Migration Year',
   Description = 'P4-Year the fish migrated',
   Units = NULL,
   Validation = NULL,
   DataType = 'int',
   PossibleValues = NULL,
   DbColumnName = 'MigrationYear',
   ControlType = 'number',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = 9,
   Name = 'Size of Count',
   Description = 'P4-Size of the fish',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '{""SMALL"":""Small"",""MEDIUM"":""Medium"",""LARGE"":""Large""}',
   DbColumnName = 'SizeOfCount',
   ControlType = 'select',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = 9,
   Name = 'Scale Id',
   Description = 'P4-The fish scale ID number',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = NULL,
   DbColumnName = 'ScaleId',
   ControlType = 'text',
   [Rule] = NULL
   
UNION ALL SELECT
   FieldCategoryId = 9,
   Name = 'Containment',
   Description = 'P4-Was the fish in containment',
   Units = NULL,
   Validation = NULL,
   DataType = 'string',
   PossibleValues = '{""YES"":""Yes"", ""NO"":""No""}',
   DbColumnName = 'Containment',
   ControlType = 'select',  
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
/*CREATE TABLE #QaStatusIds (id int)
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
*/

-- Update the Possible Values for Weather in Electrofishing.
update dbo.[Fields]
set PossibleValues = '{"""":"""",""SUNNY"":""Sunny"",""PARTLY CLOUDY"":""Partly Cloudy"",""CLOUDY"":""Cloudy"",""RAIN"":""Rain""}'
where FieldCategoryId = 10 and DbColumnName = 'Weather'


-- Cleanup
drop table #ProjectInfo
drop table #NewFieldInfo
drop table #NewDatasetIds
--drop table #QaStatusIds

--Make other necessary updates
update dbo.[Fields]
set PossibleValues = '{""SUNNY"":""Sunny"",""PARTLY CLOUDY"":""Partly Cloudy"",""CLOUDY"":""Cloudy"",""RAIN"":""Rain""}'
where PossibleValues = '[""Sunny"",Partly ""Cloudy"",""Cloudy"",""Rain""]'
            ");
        }

        public override void Down()
        {
            Sql(@"
-- Note: These MUST match values declared in Up()
--declare @datasetBaseName as varchar(max) = 'ScrewTrap'
--declare @categoryName as varchar(max) = @datasetBaseName
--declare @datastoreName as varchar(max) = @datasetBaseName

--delete from dbo.Locations where id in (select location_id from dbo.LocationProjects where project_id in (select ProjectId from datasets where name = 'Snorkel Fish'))
--delete from dbo.LocationProjects where project_id in (select ProjectId from datasets where name = 'Snorkel Fish')
--delete from dbo.DatasetQAStatus  where Dataset_Id in (select id from dbo.Datasets where name = @datasetBaseName)
--delete from dbo.DatasetQAStatus1 where Dataset_Id in (select id from dbo.Datasets where name = @datasetBaseName)

delete from dbo.DatasetFields where DatasetId in (1218), (1219), (1220), (1221), (1222), (1223), (1234)
  and DbColumnName in ('EventType', 'SecondPitTag', 'RacewayTransectTank', 'LifeStage', 
  'CodedWireTag', 'BroodYear', 'MigrationYear', 'SizeOfCount', 'ScaleId', 'Containment')
delete from dbo.Fields where FieldCategoryId = 9 and DbColumnName in (
  'EventType', 'SecondPitTag', 'RacewayTransectTank', 'LifeStage', 'CodedWireTag',
  'BroodYear', 'MigrationYear', 'SizeOfCount', 'ScaleId', 'Containment'
  )

--delete from dbo.Datasets where name = @datasetBaseName
--delete from dbo.FieldCategories where name = @categoryName
--delete from dbo.Datastores where name = @datastoreName
            ");
        }
    }
}
