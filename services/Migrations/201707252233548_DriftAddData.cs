namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DriftAddData : DbMigration
    {
        public override void Up()
        {
            Sql(@"


-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'Drift'
declare @categoryName as varchar(max) = 'Drift'
declare @datastoreName as varchar(max) = 'Drift'


-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 2249 -- Biomonitoring of Fish Habitat Enhancement


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
    Config                    = '{""DataEntryPage"": {""HiddenFields"": [""ActivityDate"",""Instrument""], ""ShowFields"": [""SampleDate""]}}'
FROM #ProjectInfo as p

-- Add the project indicator to the front of the dataset name.
UPDATE dbo.Datasets SET Name = 'Biom-'+ @datasetBaseName WHERE Name = 'Drift' and Description like 'Drift: Biomonitoring of Fish%'

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
        Name = 'Sample Year',
        Description = 'Year the Fish was sampled',
        Units = NULL,
        Validation = 'i4',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'SampleYear',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'PrePost',
        Description = 'Pre or post sampling',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""PRE"",""POST"",""PREPOST""]',
        DbColumnName = 'PrePost',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Visit Id',
        Description = 'Identification Id used for CHaMP and AEM',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'VisitId',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Sample Id',
        Description = 'Identification used in the field by the collection crew',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'SampleId',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Sample Client Id',
        Description = 'Identification number used by the client to identify the sample',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'SampleClientId',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Total Jars',
        Description = 'Total Number of jars collected',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'TotalJars',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Aquatic Tare Mass',
        Description = 'Weight of the sample bottle (Aquatic)',
        Units = 'g',
        Validation = '4d',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'AquaticTareMass',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Aquatic Tare Dry Mass',
        Description = 'Weight of the sample including the collection bottle after the water has been removed (Aquatic)',
        Units = 'g',
        Validation = '4d',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'AquaticTareDryMass',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Aquatic Dry Mass Final',
        Description = 'Weight of the sample without the collection bottle and water (Aquatic)',
        Units = 'g',
        Validation = '4d',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'AquaticDryMassFinal',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'AT Tare Mass',
        Description = 'Weight of the sample bottle (Aquatic Terrestrial)',
        Units = 'g',
        Validation = '4d',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'ATTareMass',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'AT Tare Dry Mass',
        Description = 'Weight of the sample including the collection bottle after the water has been removed (Aquatic Terrestrial)',
        Units = 'g',
        Validation = '4d',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'ATTareDryMass',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'AT Dry Mass Final',
        Description = 'Weight of the sample without the collection bottle and water (Aquatic Terrestrial)',
        Units = 'g',
        Validation = '4d',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'ATDryMassFinal',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Terr Tare Mass',
        Description = 'Weight of the sample bottle (Terrestrial)',
        Units = 'g',
        Validation = '4d',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TerrTareMass',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Terr Tare Dry Mass',
        Description = 'Weight of the sample including the collection bottle after the water has been removed (Terrestrial)',
        Units = 'g',
        Validation = '4d',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TerrTareDryMass',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Terr Dry Mass Final',
        Description = 'Weight of the sample without the collection bottle and water (Terrestrial)',
        Units = 'g',
        Validation = '4d',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TerrDryMassFinal',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Field Comments',
        Description = 'Any comments about the activity being conducted',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'FieldComments',
        ControlType = 'textarea',
        [Rule] = NULL

update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header

-----
-- Details Fields

INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT 
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Group',
        Description = 'Group the macroinvertebrates species belongs too',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""AQUATIC"",""AQUATICTERRESTRIAL"",""TERRESTRIAL""]',
        DbColumnName = 'SpeciesGroup',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT 
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Taxon',
        Description = 'Taxonomic names of the macroinvertebrate species',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'Taxon',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Life Stage',
        Description = 'Life stage of the macroinvertebrate at the time of sampling',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""ADULT"",""LARVA"",""PUPA"",""UNKNOWN""]',
        DbColumnName = 'LifeStage',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Size Class',
        Description = 'Size of the macroinvertebrate at the time of sampling in millimeters (mm)',
        Units = 'mm',
        Validation = 'i',
        DataType = 'int',
        PossibleValues = '[""00-03"",""03-06"",""06-09"",""09-12"",""12-15"",""15-18"",""18-21"",""21-24"",""24-27"",""27-30"",""30-33""]',
        DbColumnName = 'SizeClass',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Count',
        Description = 'Total number of the taxon on collected in the sample',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'TaxonCount',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Qualifier',
        Description = 'Notes about the sample',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'Qualifier',
        ControlType = 'textarea',
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

-- Benthic will only have Activity QA
--INSERT INTO dbo.DatasetQAStatus1(Dataset_Id, QAStatus_id)
--SELECT
--    Dataset_Id  = d.id,
--    QAStatus_id = q.id
--FROM #NewDatasetIds as d, #QaStatusIds as q

INSERT INTO dbo.LocationTypes (Name, [Description]) VALUES ('Drift', 'Drift')

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
            Sql(@"

            ");
        }
    }
}
