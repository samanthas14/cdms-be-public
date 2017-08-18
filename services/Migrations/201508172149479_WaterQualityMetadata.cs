namespace services.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class WaterQualityMetadata : DbMigration
    {
        public override void Up()
        {
            Sql(@"

-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'Water Quality'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = @datasetBaseName


-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 1199  -- WRP Surface Water Monitoring


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
    Config                    = '{""DataEntryPage"": {""ShowFields"": [""Laboratory""]}}'
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
        Name = 'Data Type',
        Description = 'Type of data collected',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Lab"", ""ISCO"", ""Handheld""]',
        DbColumnName = 'DataType',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT   
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Laboratory',
        Description = 'The name of the Lab',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'Laboratory',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT   
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Sample ID',
        Description = 'Identifcation number from the lab on the sample',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'SampleID',
        ControlType = 'text',
        [Rule] = NULL        

UNION ALL SELECT   
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Sample Date',
        Description = 'Date of sample',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'SampleDate',
        ControlType = 'date',
        [Rule] = NULL

update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header

-----
-- Details Fields

INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT   
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Result',
        Description = 'Results of the substance as reported by the instrument',
        Units = NULL,
        Validation = NULL,
        DataType = 'number',
        PossibleValues = NULL,
        DbColumnName = 'Result',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT   
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Result Units',
        Description = 'Unit of measure for the results',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ResultUnits',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT   
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Lab Duplicate?',
        Description = 'Duplicate sample ran',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Yes"", ""No""]',
        DbColumnName = 'LabDuplicate',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT   
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Comments',
        Description = 'Comments or Notes about the sample results',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'Comments',
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
declare @datasetBaseName as varchar(max) = 'Water Quality'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = @datasetBaseName

--delete from dbo.Locations where id in (select location_id from dbo.LocationProjects where project_id in (select ProjectId from datasets where name = @datasetBaseName))
--delete from dbo.LocationProjects where project_id in (select ProjectId from datasets where name = @datasetBaseName)
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
