namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreelData : DbMigration
    {
        public override void Up()
        {
            Sql(@"

-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'Creel Survey'
declare @categoryName as varchar(max) = 'Harvest'
declare @datastoreName as varchar(max) = @datasetBaseName


-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 1217

-- Grab the project names
update #ProjectInfo set name = (select name from projects where projects.id = #ProjectInfo.id)


-- Create a field category
INSERT INTO dbo.[FieldCategories] ([Name], [Description]) 
SELECT Name        = @categoryName,
       Description = @categoryName + ' related fields'


-- Add records to the Datastores
INSERT INTO dbo.Datastores (Name, [Description], TablePrefix, DatastoreDatasetId, OwnerUserId, FieldCategoryId) 
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
    Name                      = @categoryName + ' - ' + p.name,
    Description               = @categoryName,
    DefaultActivityQAStatusId = 6,
    DatastoreId               = (SELECT IDENT_CURRENT('dbo.Datastores')),
    Config                    = '{""DataEntryPage"": {""HiddenFields"": [""Instrument"",""addNewRow""],""ShowFields"": [""Surveyor"",""addSection"",""addInterview"",""addFisherman"",""addAnotherFish""]}}'
FROM #ProjectInfo as p

-- This will info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    

-----
-- Insert any new fields we'll need (George says don't reuse fields)
INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

-- Header Fields
SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Direction',
        Description = 'Direction the surveyor was going (upstream or downstream)',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '["""",""Downstream"",""Upstream""]',
        DbColumnName = 'Direction',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Shift',
        Description = 'What shift the surveyor worked',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""All day"",""Midday"",""Morning"",""Evening""]',
        DbColumnName = 'WorkShift',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Survey Species',
        Description = 'Species of fish the creel is being performed for',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""CHS"",""STS""]',
        DbColumnName = 'SurveySpecies',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Surveyor',
        Description = 'Name of the Person Conducting the Survey',
        Units = NULL,
        Validation = 'nb',
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'Surveyor',
        ControlType = 'text',
        [Rule] = '{""DefaultValue"":""Malvin Jamison""}'

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Weather Conditions',
        Description = 'Comments about the weather conditions at the time of survey',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'WeatherConditions',
        ControlType = 'textarea',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Time Start',
        Description = 'Time the surveyor started the section survey',
        Units = 'HH:MM',
        Validation = 't',
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'TimeStart',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Time End',
        Description = 'Time the surveyor ended the section survey',
        Units = 'HH:MM',
        Validation = 't',
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'TimeEnd',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Number Anglers Observed',
        Description = 'Number of anglers observed in the survey',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'NumberAnglersObserved',
        ControlType = 'number',
        [Rule] = '{""DefaultValue"":""0""}'

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Number Anglers Interviewed',
        Description = 'Number of anglers interviewed in the survey',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'NumberAnglersInterviewed',
        ControlType = 'number',
        [Rule] = '{""DefaultValue"":""0""}'

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Survey Comments',
        Description = 'Comments regarding the Survey',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'SurveyComments',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Field Sheet Link',
        Description = 'Link to the scanned paper field sheet',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'FieldSheetFile',
        ControlType = 'file',
        [Rule] = NULL


update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header

-----
-- Details Fields

INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Interview Time',
        Description = 'Time the interview took place',
        Units = 'HH:MM',
        Validation = 't',
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'InterviewTime',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'GPS Easting',
        Description = 'NAD 83 Zone 11N UTM Easting (X or Longitude) coordinates for the site',
        Units = NULL,
        Validation = '6',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'GPSEasting',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'GPS Northing',
        Description = 'NAD 83 Zone 11N UTM Northing (Y or Latitude) coordinates for the site',
        Units = NULL,
        Validation = '7',
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'GPSNorthing',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Interview Comments',
        Description = 'Comments regarding the header',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'InterviewComments',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Total Time Fished',
        Description = 'The total time fished, calculated from hours and minutes to total minutes',
        Units = 'HH:MM',
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'TotalTimeFished',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Fish Count',
        Description = 'Total number of fish of this type',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'FishCount',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT   
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Species',
        Description = 'Species of the carcass found',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""BUT"",""CHF"",""CHS"",""CO"",""MWF"",""NPM"",""PL"",""RBT"",""STS""]',
        DbColumnName = 'Species',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Disposition',
        Description = 'Was the fish kept or released',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Kept"",""Released""]',
        DbColumnName = 'Disposition',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Sex',
        Description = 'Sex of the fish, Male or Female or Unknown',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""M"",""F"",""UNK""]',
        DbColumnName = 'Sex',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Fin Clip',
        Description = 'Type of Fin Clips on the carcass',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""NONE"",""NA"",""AD"",""LV"",""ADLV"",""ADRV"",""RV""]',
        DbColumnName = 'FinClip',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Origin',
        Description = 'Origin of the Fish. Hatchery (HAT); Natural (NAT); Unknown (UNK)',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Kept"",""Released""]',
        DbColumnName = 'Origin',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Mark',
        Description = 'Type of Marks on the carcass',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""NONE"",""NA"",""1ROP"",""1LOP"",""2ROP"",""2LOP"",""3ROP"",""3LOP"",""1TAIL"",""2TAIL"",""1CAU"",""2CAU"",""3CAU"",""4CAU""]',
        DbColumnName = 'Marks',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Fork Length',
        Description = 'Fork Length of the Carcass',
        Units = 'mm',
        Validation = '[1,1300]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'ForkLength',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'MeHP Length',
        Description = 'Mid Eye to Hypural Plate Length',
        Units = 'mm',
        Validation = '[1,1300]',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MeHPLength',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Snout Id',
        Description = 'Id from the snout card',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'SnoutId',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Scale Id',
        Description = 'Id from the scale card',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ScaleId',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Carcass Comments',
        Description = 'Comments regarding the carcass',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'CarcassComments',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Fisherman',
        Description = 'The fisherman interviewed',
        Units = NULL,
        Validation = 'i',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'FishermanId',
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

-- Delete the Fisherman record from the DatasetFields table; that field goes in the Fields table, but not this one.
DELETE FROM dbo.DatasetFields
WHERE (Label = 'Fisherman' 
and DbColumnName = 'FishermanId')

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

-- Add record(s) to the LocationTypes table
Insert into [dbo].[LocationTypes]([Name],[Description])values('Harvest/Stream Survey Section','Harvest/Stream Survey Section')

");

        }
        
        public override void Down()
        {
            Sql(@"

-- Note: These MUST match values declared in Up()
declare @datasetBaseName as varchar(max) = 'Creel Survey'
declare @categoryName as varchar(max) = 'Harvest'
declare @datastoreName as varchar(max) = @datasetBaseName

-- Turn the constraints for the following tables off.  Otherwise, the delete actions will throw errors about the constraints.
alter table dbo.DatasetFields nocheck constraint all
alter table dbo.Fields nocheck constraint all
alter table dbo.Datasets nocheck constraint all
alter table dbo.Datastores nocheck constraint all

delete from dbo.LocationProjects where project_id in (select ProjectId from Datasets where Name = @datasetBaseName)
delete from dbo.Locations where id in (select Location_Id from dbo.LocationProjects where Project_Id in (select ProjectId from Datasets where Name = @datasetBaseName))
delete from dbo.DatasetQAStatus  where Dataset_Id in (select Id from dbo.Datasets where Name = @datasetBaseName)
delete from dbo.DatasetQAStatus1 where Dataset_Id in (select Id from dbo.Datasets where Name = @datasetBaseName)

-- The next delete action throws errors due to foreign key constraints.  Added following lines to adjust the constraints before/after to allow the action to work.
delete from dbo.DatasetFields where DatasetId in (select Id from dbo.Datasets where Name = @datasetBaseName)

delete from dbo.Fields where FieldCategoryId in (select Id from dbo.FieldCategories where Name = @categoryName)

delete from dbo.Datasets where Name = @datasetBaseName

-- The FieldCategoryId is still turned off, so we are OK.
delete from dbo.Datastores where Name = @datastoreName

-- Now turn the constraints back on.
alter table dbo.DatasetFields check constraint all
alter table dbo.Fields check constraint all
alter table dbo.Datasets check constraint all
alter table dbo.Datastores check constraint all

delete from dbo.FieldCategories where Name = @categoryName

");
        }
    }
}
