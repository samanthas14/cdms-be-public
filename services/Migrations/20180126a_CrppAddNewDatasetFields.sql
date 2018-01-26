-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'CrppContracts'
declare @categoryName as varchar(max) = 'CRPP Contracts'
declare @datastoreName as varchar(max) = @datasetBaseName

-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 2247
		  
CREATE TABLE #NewDatasetIds (id int)        -- This will contain a list of ids of all dataset records inserted below

-- Capture and temporarily store the Dataset Id and Field Category Id
declare @datasetId as numeric;
set @datasetId = (SELECT [Id] FROM dbo.Datasets where Name = 'CRPP Contracts');

declare @fieldCategoryId as numeric;
set @fieldCategoryId = (SELECT [Id] FROM dbo.FieldCategories where Name = 'CRPP Contracts');

INSERT INTO #NewDatasetIds(id) values(@datasetId);

		  
-- Grab the project names
update #ProjectInfo set name = (select name from projects where projects.id = #ProjectInfo.id)

-- This will info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    


-- Insert any new fields we'll need (George says don't reuse fields)
-- Header Fields
INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT   
        FieldCategoryId = @fieldCategoryId, 
        Name = 'Award Amount',
        Description = 'Award Amount',
        Units = '$',
        Validation = null,
        DataType = 'decimal',
        PossibleValues = null,
        DbColumnName = 'AwardAmount',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldCategoryId, 
        Name = 'Date Final Report Submitted',
        Description = 'Date Final Report Submitted',
        Units = NULL,
        Validation = NULL,
        DataType = 'date',
        PossibleValues = NULL,
        DbColumnName = 'FinalReportSubmitted',
        ControlType = 'date',
        [Rule] = NULL
	
update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header

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

-- Update ShawnS
update dbo.Users
set [Description] = 'Archaeologist I/GIS Coord/Asst/ Hanford Coord', DepartmentId = 1015, Fullname = 'Shawn Steinmetz', Roles = '["CRPP"]'
where [Id] = 3116

-- Cleanup
drop table #ProjectInfo
drop table #NewFieldInfo
drop table #NewDatasetIds
--drop table #QaStatusIds
--drop table #NewLocationIds