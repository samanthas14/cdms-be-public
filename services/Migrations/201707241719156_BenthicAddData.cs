namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BenthicAddData : DbMigration
    {
        public override void Up()
        {
            Sql(@"
-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'Benthic'
declare @categoryName as varchar(max) = 'Benthic'
declare @datastoreName as varchar(max) = 'Benthic'


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
UPDATE dbo.Datasets SET Name = 'Biom-'+ @datasetBaseName WHERE Name = 'Benthic' and Description like 'Benthic: Biomonitoring of Fish%'

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
        Description = 'Identification Id used for CHaMP/AEM',
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
        Name = 'Tare Mass',
        Description = 'Weight of the sample bottle',
        Units = 'g',
        Validation = '4d',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TareMass',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Dry Mass',
        Description = 'Weight of the sample including the collection bottle after the water has been removed',
        Units = 'g',
        Validation = '4d',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'DryMass',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Dry Mass Final',
        Description = 'Weight of the sample without the collection bottle and water',
        Units = 'g',
        Validation = '4d',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'DryMassFinal',
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
        Name = 'Metric Taxa Richness',
        Description = 'Measures the overall variety of the macroinvertebrate assemblage',
        Units = NULL,
        Validation = '2d',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'MetricTaxaRichness',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT 
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Metric Hilsenhoff Biotic Index',
        Description = 'Uses tolerance values to weight abundance in an estimate of overall pollution. Originally designed to evaluate organic pollution',
        Units = NULL,
        Validation = '2d',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'MetricHilsenhoffBioticIndex',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Metric Chironomidae',
        Description = 'Percent of midge larvae',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MetricChironomidae',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Metric Coleoptera',
        Description = 'Percent Coleoptera in the sample divided by the number of individuals in the sample',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MetricColeoptera',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Metric Diptera',
        Description = 'Percent of all ""true"" fly larvae',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MetricDiptera',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Metric Ephemeroptera',
        Description = 'Percent of mayfly nymphs',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MetricEphemeroptera',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Metric Lepidoptera',
        Description = 'Percent Lepidoptera in the sample divided by the number of individuals in the sample',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MetricLepidoptera',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Metric Megaloptera',
        Description = 'Percent Megaloptera in the sample divided by the number of individuals in the sample',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MetricMegaloptera',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Metric Odonata',
        Description = 'Percent Odonata in the sample divided by the number of individuals in the sample',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MetricOdonata',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Metric Oligochaeta',
        Description = 'Percent of aquatic worms',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MetricOligochaeta',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Metric Other NonInsect',
        Description = 'Percent Other Non-Insects in the sample divided by the number of individuals in the sample',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MetricOtherNonInsect',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Metric Plecoptera',
        Description = 'Percent of stonefly nymphs',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MetricPlecoptera',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Metric Trichoptera',
        Description = 'Percent of caddisfly larvae',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MetricTrichoptera',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MV Taxa Richness',
        Description = 'Measures the overall variety of the macroinvertebrate assemblage (Metric Value)',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MvTaxaRichness',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MV E Richness',
        Description = 'Number of mayfly taxa (usually genus or species level) (Metric Value)',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MvERichness',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MV P Richness',
        Description = 'Number of stonefly taxa (usually genus of species level) (Metric Value)',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MvPRichness',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MV T Richness',
        Description = 'Number of caddisfly taxa (usually genus or species level) (Metric Value)',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MvTRichness',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MV Pollution Sensitive Richness',
        Description = 'Number of taxa that are sensitive to pollution (Metric Value)',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MvPollutionSensitiveRichness',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MV Clinger Richness',
        Description = 'Number of insects having fixed retreats or adaptations for attachment to surfaces in flowing water. (Metric Value)',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MvClingerRichness',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MV Semivoltine Richness',
        Description = 'The presence or absence of a long-lived stonefly genus (2-3 year life cycle) (Metric Value)',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MvSemivoltineRichness',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MV Pollution Tolerant Percent',
        Description = 'Percent of pollution tolerant taxon (Metric Value)',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MvPollutionTolerantPercent',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MV Predator Percent',
        Description = 'Percent of the predator functional feeding group. Can be made restrictive to exclude omnivores (Metric Value)',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MvPredatorPercent',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MV Dominant Taxa3 Percent',
        Description = 'Measures the dominance of the single most abundant taxon. (Metric Value)',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MvDominantTaxa3Percent',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MS Taxa Richness',
        Description = 'Measures the dominance of the single most abundant taxon. (Metric Value)',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MsTaxaRichness',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MS E Richness',
        Description = 'Number of mayfly taxa (usually genus or species level) (Metric Scores)',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MsERichness',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MS P Richness',
        Description = 'Number of stonefly taxa (usually genus of species level) (Metric Scores)',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MsPRichness',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MS T Richness',
        Description = 'Number of caddisfly taxa (usually genus or species level) (Metric Scores)',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MsTRichness',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MS Pollution Sensitive Richness',
        Description = 'Number of taxa that are sensitive to pollution (Metric Scores)',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MsPollutionSensitiveRichness',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MS Clinger Richness',
        Description = 'Number of insects having fixed retreats or adaptations for attachment to surfaces in flowing water. (Metric Scores)',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MsClingerRichness',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MS Semivoltine Richness',
        Description = 'The presence or absence of a long-lived stonefly genus (2-3 year life cycle) (Metric Scores)',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MsSemivoltineRichness',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MS Pollution Tolerant Percent',
        Description = 'Percent of pollution tolerant taxon (Metric Scores)',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MsPollutionTolerantPercent',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MS Predator Percent',
        Description = 'Percent of the predator functional feeding group. Can be made restrictive to exclude omnivores (Metric Scores)',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MsPredatorPercent',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MS Dominant Taxa3 Percent',
        Description = 'Measures the dominance of the single most abundant taxon. (Metric Scores)',
        Units = '%',
        Validation = 'p2d',
        DataType = 'decimal',
        PossibleValues = NULL,
        DbColumnName = 'MsDominantTaxa3Percent',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'B IBI Score',
        Description = 'Benthic Index of Biological Integrity (B-IBI) An Index of Biological Integrity monitoring approach provides the following four types of stream condition descriptors of the condition of a stream as reflected by the biota',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'BIbiScore',
        ControlType = 'number',
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

INSERT INTO dbo.LocationTypes (Name, [Description]) VALUES ('Benthic', 'Benthic')

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
