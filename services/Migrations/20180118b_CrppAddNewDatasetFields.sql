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
        Name = 'Project Lead',
        Description = 'CRPP personnel',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = null,
        DbColumnName = 'ProjectLead',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldCategoryId, 
        Name = 'Cost Center',
        Description = 'Cost Center the contract is billed to',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'CostCenter',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldCategoryId, 
        Name = 'Project Name',
        Description = 'Project Name the contract is attributed to',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ProjectName',
        ControlType = 'text',
        [Rule] = NULL
		
UNION ALL SELECT
        FieldCategoryId = @fieldCategoryId, 
        Name = 'Client',
        Description = 'The person to whom the contract is with',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'Client',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldCategoryId, 
        Name = 'Client Agreement Number',
        Description = 'Clients number on the agreement/contract',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'AgreeNumb',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldCategoryId, 
        Name = 'Date of Execution',
        Description = 'Date the agreement/contract was executed',
        Units = 'mm/dd/yyyy',
        Validation = NULL,
        DataType = 'date',
        PossibleValues = NULL,
        DbColumnName = 'DateExec',
        ControlType = 'date',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldCategoryId, 
        Name = 'Draft Due',
        Description = 'Date the draft report is due',
        Units = 'mm/dd/yyyy',
        Validation = NULL,
        DataType = 'date',
        PossibleValues = NULL,
        DbColumnName = 'DraftDue',
        ControlType = 'date',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldCategoryId, 
        Name = 'Final Due',
        Description = 'Date the final report is due',
        Units = 'mm/dd/yyyy',
        Validation = NULL,
        DataType = 'date',
        PossibleValues = NULL,
        DbColumnName = 'FinalDue',
        ControlType = 'date',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldCategoryId, 
        Name = 'Contract End',
        Description = 'Date the Contract ends',
        Units = 'mm/dd/yyyy',
        Validation = NULL,
        DataType = 'date',
        PossibleValues = NULL,
        DbColumnName = 'ContractEnd',
        ControlType = 'date',
        [Rule] = NULL
		
UNION ALL SELECT
        FieldCategoryId = @fieldCategoryId, 
        Name = 'Mod Ext End Date',
        Description = 'Modification Extension End Date. New date the contract/agreement is set to end.  Different from Contract End Date',
        Units = 'mm/dd/yyyy',
        Validation = NULL,
        DataType = 'date',
        PossibleValues = NULL,
        DbColumnName = 'ModExtDate',
        ControlType = 'date',
        [Rule] = NULL
		
UNION ALL SELECT
        FieldCategoryId = @fieldCategoryId, 
        Name = 'Document Link',
        Description = 'Add and remove an attached document',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'DocumentLink',
        ControlType = 'file',
        [Rule] = NULL
	
UNION ALL SELECT
        FieldCategoryId = @fieldCategoryId, 
        Name = 'Activity Comments',
        Description = 'Comments about the agreement',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ActivityComments',
        ControlType = 'text',
        [Rule] = NULL
	
update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header

SELECT   
        FieldCategoryId = @fieldCategoryId, 
        Name = '	Survey Acres',
        Description = 'Number of Acres Surveyed',
        Units = 'Acres',
        Validation = NULL,
        DataType = 'float',
        PossibleValues = null,
        DbColumnName = '	SurveyAcres',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldCategoryId, 
        Name = 'Test Sites',
        Description = 'Number of sites tested',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'TestSites',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldCategoryId, 
        Name = 'HPRCSIT',
        Description = 'HISTORIC PROPERTIES OF RELIGIOUS AND CULTURAL SIGNIFICANCE. TO INDIAN TRIBES',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '["YES","NO"]',
        DbColumnName = 'HPRCSIT',
        ControlType = 'select',
        [Rule] = NULL
		
UNION ALL SELECT
        FieldCategoryId = @fieldCategoryId, 
        Name = 'SHPO Report Num',
        Description = 'State Historic properties number',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'SHRENum',
        ControlType = 'text',
        [Rule] = NULL
		
UNION ALL SELECT
        FieldCategoryId = @fieldCategoryId, 
        Name = 'SHPO Case Num',
        Description = 'State Historic Cultural Significance number',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'SHCSNum',
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