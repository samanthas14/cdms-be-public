namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BSampleAddData : DbMigration
    {
        public override void Up()
        {
            Sql(@"
-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'BSample'
declare @categoryName as varchar(max) = 'Biological Sample'
declare @datastoreName as varchar(max) = 'Biological Sample'


-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 1135 -- Grande Ronde Basin Monitoring and Evaluation
UNION ALL SELECT id = 1188 -- Umatilla Basin Natural Production Monitoring and Evaluation


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
UPDATE dbo.Datasets SET Name = 'UMME-'+ @datasetBaseName WHERE Name = 'BSample' and Description like 'BSample: Umatilla Basin%'
UPDATE dbo.Datasets SET Name = 'GRME-'+ @datasetBaseName WHERE Name = 'BSample' and Description like 'BSample: Grande Ronde Basin%'
-------------------------

-- This will info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    

-----
-- Insert any new fields we'll need (George says don't reuse fields)
-- Header Fields
INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Sample Year',
        Description = 'Year the Fish was sampled',
        Units = NULL,
        Validation = 'yyyy',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'SampleYear',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT   
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Collection Type',
        Description = 'The method used to enumerate the fish: Artificial production spawning (ART), Spawning Ground Survey (SGS), Salvage operations (SALV), Carcass Only survey (CARC), Supplemental Survey (SUPPLE), Miscellaneous finds of a carcass (MISC)',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""ART"", ""SGS"", ""SALV"", ""CARCASS"", ""SUPPLE"", ""MISC""]',
        DbColumnName = 'CollectionType',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Technicians',
        Description = 'The person/persons who are conducting the  activity at the time',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'Technicians',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Header Comments',
        Description = 'Any comments about the activity being conducted',
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
        Name = 'Sex',
        Description = 'The sex of the fish: Male (M), Female (F), Unknown (UNK)',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""M"", ""F"", ""UNK""]',
        DbColumnName = 'Sex',
        ControlType = 'select',
        [Rule] = NULL
         
UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Life Stage',
        Description = 'The approximate age of the fish: Adult (A), Jack (J), Mini/SubJack (SJ), STS-One Salt Year (S1), STS-Two Salt Year (S2), Kelt (K); Unknown (UNK);',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""A"", ""J"", ""SJ"", ""S1"", ""S2"", ""K"", ""UNK""]',
        DbColumnName = 'LifeStage',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Origin',
        Description = 'Refers to whether or not the fish appears to be a Natural (NAT); Hatchery (HAT); Unknown Origin (UNK)',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""NAT"", ""HAT"", ""UNK""]',
        DbColumnName = 'Origin',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Fork Length',
        Description = 'The fork length of the fish.  Measured from the tip of the snout to the end of the middle caudal fin rays and is used in fishes in which it is difficult to tell where the vertebral column ends (mm).',
        Units = 'mm',
        Validation = '[1,1300]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'ForkLength',
        ControlType = 'number',
        [Rule] = NULL


UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Total Length',
        Description = 'The total length of the fish.  Measured from the tip of the snout to the tip of the longer lobe of the caudal fin, usually measured with the lobes compressed along the midline. It is a straight-line measure, not measured over the curve of the body (mm).',
        Units = 'mm',
        Validation = '[1,1500]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'TotalLength',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Weight',
        Description = 'The total weight of the fish at the time of handling (g)',
        Units = 'g',
        Validation = '[1,9999]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'Weight',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Fin Clip',
        Description = 'The physical removal of a fin from the fish: None;  Adipose fin (AD); Adipose fin left ventricle (ADLV); Adipose fin Right ventricle (ADRV); Left ventricle (LV); Right ventricle (RV); Not Available (NA);',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""NONE"", ""NA"", ""AD"", ""ADLV"", ""ADRV"", ""LV"", ""RV""]',
        DbColumnName = 'FinClip',
        ControlType = 'multiselect',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Mark',
        Description = 'Any man made mark: None;  Not Available (NA); 1 Right Opercule Punch (1ROP); 1 Left Opercule Punch (1LOP); 2 Right Opercule Punches (2ROP); 2 Left Opercule Punches (2LOP); 3 Right Opercule Punches (3ROP); 3 Left Opercule Punches (3LOP); 1 Upper Caudal Punch  (1CAU);  2 Upper Caudal Punches (2CAU); 3 Upper Caudal Punches (3CAU)',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""NONE"", ""NA"", ""1ROP"", ""1LOP"", ""2ROP"", ""2LOP"", ""3ROP"", ""3LOP"", ""1CAU"", ""2CAU"", ""3CAU""]',
        DbColumnName = 'Mark',
        ControlType = 'multiselect',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Total Count',
        Description = 'The total number of fish of a specific category',
        Units = NULL,
        Validation = '[1,2000]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'TotalCount',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Percent Retained',
        Description = 'Some fish who present at the weir may have already spawned.  Used to determine pre-spawn mortality. This is the amount the fish has spawned out.',
        Units = '%',
        Validation = '[0,100]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'PercentRetained',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Tag',
        Description = 'Does the fish have a Tag of any sort These could be Wire Tag (Wire); Radio; Floy; PIT; Visible Implant Elastomer (VIE); Other; None',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""WIRE"", ""RADIO"", ""FLOY"", ""PIT"", ""VIE"", ""OTHER"", ""NONE""]',
        DbColumnName = 'Tag',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'PIT Tag Id',
        Description = 'The number of the PIT tag if it is read. This could be a half or full duplex number.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'PITTagId',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Radio Tag Id',
        Description = 'The number on the radio tag if the fish has one (i.e. 151.36.032)',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'RadioTagId',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Genetic Sample Id',
        Description = 'The number of the genetic envelope or bottle in which a genetic sample is placed for future analysis.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'GeneticSampleId',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Scale Id',
        Description = 'The number on the scale card in which the scale sample is placed for future analysis.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ScaleId',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'SnoutId',
        Description = 'The number on the snout tag card which is placed in the bag to be transported with the snout for determination of the Wire Tag (CWT)',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'SnoutId',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'KidneyId',
        Description = 'The number of the kidney sample collected',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'KidneyId',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Other Tag Id',
        Description = 'If some other tag, what is the number or color (i.e VIE color or spaghetti tag number)',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'OtherTagId',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Fish Comments',
        Description = 'Comments about the specific fish that is viewed',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'FishComments',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'RecordNumber',
        Description = 'Record Number',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'RecordNumber',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MEHP Length',
        Description = 'MEHP Length',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MEHPLength',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'SubSample',
        Description = 'SubSample',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = '[""YES"", ""NO""]',
        DbColumnName = 'SubSample',
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
CREATE TABLE #QaStatusIds (id int)
INSERT INTO #QaStatusIds (id) 
          SELECT id = 5     -- Approved
UNION ALL SELECT id = 6     -- Ready for QA


INSERT INTO dbo.DatasetQAStatus(Dataset_Id, QAStatus_id)
SELECT
    Dataset_Id  = d.id,
    QAStatus_id = q.id
FROM #NewDatasetIds as d, #QaStatusIds as q

-- Biological Sample will only have Activity QA
--INSERT INTO dbo.DatasetQAStatus1(Dataset_Id, QAStatus_id)
--SELECT
--    Dataset_Id  = d.id,
--    QAStatus_id = q.id
--FROM #NewDatasetIds as d, #QaStatusIds as q

INSERT INTO dbo.LocationTypes (Name, [Description]) VALUES ('Biological Sample', 'Biological Sample')

update dbo.Fields set ControlType = 'multiselect' where DbColumnName = 'FinClip' and ControlType = 'select'
update dbo.Fields set ControlType = 'multiselect' where DbColumnName = 'Mark' and ControlType = 'select'

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
