namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GeneticAddData : DbMigration
    {
        public override void Up()
        {
            Sql(@"
-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'Genetic'
declare @categoryName as varchar(max) = 'Genetic'
declare @datastoreName as varchar(max) = 'Genetic'


-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 1135 -- Grande Ronde Basin Monitoring and Evaluation
UNION ALL SELECT id = 2242 -- Grande Ronde Satellite Facilities O&M
UNION ALL SELECT id = 1161 -- Pacific Lamprey Research and Restoration
UNION ALL SELECT id = 1140 -- Steelhead Supplementation Evaluation
UNION ALL SELECT id = 1188 -- Umatilla Basin Natural Production Monitoring and Evaluation 
UNION ALL SELECT id = 1177 -- Walla Walla Basin Monitoring and Evaluation


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
	TablePrefix        = @datasetBaseName,
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

-- Add the project indicator to the front of the dataset name.
UPDATE dbo.Datasets SET Name = 'GRME-'+ @datasetBaseName WHERE Name = 'Genetic' and Description like 'Genetic: Grande Ronde Basin%'
UPDATE dbo.Datasets SET Name = 'GROM-'+ @datasetBaseName WHERE Name = 'Genetic' and Description like 'Genetic: Grande Ronde Satellite%'
UPDATE dbo.Datasets SET Name = 'PL-'+ @datasetBaseName WHERE Name = 'Genetic' and Description like 'Genetic: Pacific%'
UPDATE dbo.Datasets SET Name = 'STS-'+ @datasetBaseName WHERE Name = 'Genetic' and Description like 'Genetic: Steelhead%'
UPDATE dbo.Datasets SET Name = 'UMME-'+ @datasetBaseName WHERE Name = 'Genetic' and Description like 'Genetic: Umatilla Basin%'
UPDATE dbo.Datasets SET Name = 'WMME-'+ @datasetBaseName WHERE Name = 'Genetic' and Description like 'Genetic: Walla Walla Basin%'

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
        Name = 'Header Comments',
        Description = 'Any comments returned from the lab about the samples',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'HeaderComments',
        ControlType = 'text',
        [Rule] = NULL

update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header

-----
-- Details Fields

INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT 
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Sample Year',
        Description = 'Year the Genetic Sample was taken',
        Units = NULL,
        Validation = 'yyyy',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'SampleYear',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT 
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Genetic Id',
        Description = 'Unique Identification number of genetic sample',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'GeneticId',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Life Stage',
        Description = 'Was the scale taken from a Juvenile or an Adult',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""ADULT"", ""JUVENILE""]',
        DbColumnName = 'LifeStage',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Juvenile Age',
        Description = 'Age of the fish at the time the sample was collected (i.e. 0+; 1+; 2; 2+; 3; 4)',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'JuvenileAge',
        ControlType = 'text',
        [Rule] = NULL
         
UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Fork Length',
        Description = 'Fork length of the fish sampled',
        Units = 'mm',
        Validation = '[1,1300]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'ForkLength',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'P1 Id',
        Description = 'Year the first parent genetic sample was collected',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'P1_Id',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'P1 Collect Year',
        Description = 'Year the first parent genetic sample was collected',
        Units = NULL,
        Validation = 'yyyy',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'P1CollectYear',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'P1 Collect Loc',
        Description = 'Place the first parent genetic sample was collected',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'P1CollectLoc',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'P1 Sex',
        Description = 'Sex of the first parent; F: Female; M: Male; UNK: Unknown',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""F"", ""M"", ""UNK""]',
        DbColumnName = 'P1Sex',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'P1 Origin',
        Description = 'Origin of the first parent NAT: Natural; HAT: Hatchery; UNK: Unknown',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""NAT"", ""HAT"", ""UNK""]',
        DbColumnName = 'P1Origin',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'P2 Id',
        Description = 'Year the second parent genetic sample was collected',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'P2_Id',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'P2 Collect Year',
        Description = 'Year the second parent genetic sample was collected',
        Units = NULL,
        Validation = 'yyyy',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'P2CollectYear',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'P2 Collect Loc',
        Description = 'Place the second parent genetic sample was collected',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'P2CollectLoc',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'P2 Sex',
        Description = 'Sex of the second parent; F: Female; M: Male; UNK: Unknown',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""F"", ""M"", ""UNK""]',
        DbColumnName = 'P2Sex',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'P2 Origin',
        Description = 'Origin of the second parent NAT: Natural; HAT: Hatchery; UNK: Unknown',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""NAT"", ""HAT"", ""UNK""]',
        DbColumnName = 'P2Origin',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Genetic Comment',
        Description = 'Comments about the condition of the genetic sample',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'GeneticComment',
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

-- Juvenile Rearing will only have Activity QA
--INSERT INTO dbo.DatasetQAStatus1(Dataset_Id, QAStatus_id)
--SELECT
--    Dataset_Id  = d.id,
--    QAStatus_id = q.id
--FROM #NewDatasetIds as d, #QaStatusIds as q

INSERT INTO dbo.LocationTypes (Name, [Description]) VALUES ('Genetic', 'Genetic')

-- Cleanup
drop table #ProjectInfo
drop table #NewFieldInfo
drop table #NewDatasetIds
drop table #QaStatusIds
--drop table #NewLocationIds
");
        }
        
        public override void Down()
        {
        }
    }
}
