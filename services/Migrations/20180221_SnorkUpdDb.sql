-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'SnorkelFish'
declare @categoryName as varchar(max) = 'Snorkel Fish'
declare @datastoreName as varchar(max) = 'Snorkel Fish'

-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
--print N'Created #ProjectInfo'
INSERT INTO #ProjectInfo (id) 
          SELECT id = 1135  -- Grande Ronde Basin Spring Chinook Salmon Supplementation Monitoring and Evaluation (Project Id=1135)
UNION ALL SELECT id = 2249  -- Biomonitoring of Fish Habitat Enhancement (Project Id=2249)
--print N'Filled #ProjectInfo'
		  
CREATE TABLE #NewDatasetIds (id int)        -- This will contain a list of ids of all dataset records inserted below
--print N'Created #NewDatasetIds'

-- Capture and temporarily store the Dataset Id and Field Category Id
insert into #NewDatasetIds SELECT [Id] FROM dbo.Datasets where Name like '%snorkel%'
--print N'Filled #NewDatasetIds'

declare @fieldCategoryId as numeric;
set @fieldCategoryId = (SELECT [Id] FROM dbo.FieldCategories where Name like '%snorkel%');
--print N'Set @fieldCategoryId'
		  
-- Grab the project names
update #ProjectInfo set name = (select name from projects where projects.id = #ProjectInfo.id)
--print N'Updated #ProjectInfo'

-- This will info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    
--print N'Created table #NewFieldInfo'

-- Insert any new fields we'll need (George says don't reuse fields)
-- Header Fields
INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT   
        FieldCategoryId = @fieldCategoryId, 
        Name = 'Protocol',
        Description = 'Protocol',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '["White et al. 2011", "Crawford-10mm", "Crawford-Binned"]',
        DbColumnName = 'Protocol',
        ControlType = 'select',
        [Rule] = NULL
	
update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header
--print N'Filled #NewFieldInfo'

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
--print N'Filled DatasetFields'

declare @datasetId as numeric;

set @datasetId = (select Id from dbo.Datasets where Name = 'GRME-Snorkel')
update dbo.DatasetFields set OrderIndex = 189 where DatasetId = @datasetId and DbColumnName = 'Protocol'
set @datasetId = (select Id from dbo.Datasets where Name = 'BioM-Snorkel')
update dbo.DatasetFields set OrderIndex = 55 where DatasetId = @datasetId and DbColumnName = 'Protocol'

-- Cleanup
drop table #ProjectInfo
drop table #NewFieldInfo
drop table #NewDatasetIds
--drop table #QaStatusIds
--drop table #NewLocationIds
--print N'Cleaned-up'