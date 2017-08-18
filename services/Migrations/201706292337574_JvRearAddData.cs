namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JvRearAddData : DbMigration
    {
        public override void Up()
        {
            Sql(@"
-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'JvRearing'
declare @categoryName as varchar(max) = 'Juvenile Rearing'
declare @datastoreName as varchar(max) = 'Juvenile Rearing'


-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 2239 -- Umatilla Hatchery Satellite Facilities O & M



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
UPDATE dbo.Datasets SET Name = 'UHSFOM-'+ @datasetBaseName WHERE Name = 'JvRearing' and Description like 'JvRearing: Umatilla Hatchery Satellite%'

-------------------------

-- This will info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    

-----
-- Insert any new fields we'll need (George says don't reuse fields)
-- Header Fields
INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Acclimation Year',
        Description = 'Year the acclimation is taking place.  This may or may not be same as calendar year depending on Species',
        Units = NULL,
        Validation = 'i4',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'AcclimationYear',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Header Comments',
        Description = 'Comments about the activity',
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

SELECT FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Action',
        Description = 'Feeding Fish (F), Purchase Feed (P), Pounds Fish Received (R), Water Temperature (T), Dissolved Oxygen (O), Count Fish Received (C), Mortality (M)',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""C"", ""F"", ""M"", ""O"", ""P"", ""R"", ""T""]',
        DbColumnName = 'Action',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Species',
        Description = 'The species of the fish: Fall Chinook (CHF); Spring Chinook (CHS); Coho (CO); Summer Steelhead (STS); Bull Trout (BUT); Rainbow Trout (RBT); Pacific Lamprey (PL);',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""CHF"", ""CHS"", ""CO"", ""STS"", ""BUT"", ""RBT"", ""PL""]',
        DbColumnName = 'Species',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'PointData',
        Description = 'Specific location within a facility where the activity is occurring.  There could be multiple points for a single activity date: All facilities Pond 1, 2, 3, 4, Gauge (G), Meter (M), Inlet (I), Minthorn Springs (Upper (U), Lower (L))',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""1"", ""2"", ""3"", ""4"", ""G"", ""I"", ""L"", ""M"", ""U""]',
        DbColumnName = 'PointData',
        ControlType = 'select',
        [Rule] = NULL
         
UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Result',
        Description = 'The quantity of the action taking place',
        Units = NULL,
        Validation = 'i',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Result',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'ResultUnit',
        Description = 'Unit of measure for the result of the action: If Action is T then unit is Celsius (C); if Feeding Fish, Purchase Feed, Pounds Received then the unit is Pounds (lbs); If Dissolved Oxygen then milligram per liter; If Count then unit is Count',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""C"", ""COUNT"", ""LBS"", ""MG/L""]',
        DbColumnName = 'ResultUnit',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Action Comments',
        Description = 'Comments about the action occurring',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ActionComments',
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

INSERT INTO dbo.LocationTypes (Name, [Description]) VALUES ('Juvenile Rearing', 'Juvenile Rearing')

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
