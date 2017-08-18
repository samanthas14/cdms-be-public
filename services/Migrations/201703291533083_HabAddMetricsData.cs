namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HabAddMetricsData : DbMigration
    {
        public override void Up()
        {
            Sql(@"
-- Make backup tables for Colette
select * into dbo.FieldsBu from dbo.Fields
go
select * into dbo.DatasetFieldsBu from dbo.DatasetFields
go

-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'Metrics'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = @datasetBaseName


-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 1223 -- Umatilla Subbasin Fish HabitatRestoration
UNION ALL SELECT id = 1202 -- Walla Walla ...
UNION ALL SELECT id = 2228 -- Grande Ronde ....
UNION ALL SELECT id = 2223 -- NF John Day Habitat Improvement Project
UNION ALL SELECT id = 2226 -- Rainwater Wildlife Area
UNION ALL SELECT id = 2229 -- Tucannon River Fish Habitat Restoration


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
    Config                    = '{""DataEntryPage"": {""HiddenFields"": [""Instrument"",""ActivityDate""]}}'
FROM #ProjectInfo as p

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
        Name = 'Year Reported',
        Description = 'Year the Metric is reported',
        Units = NULL,
        Validation = 'y',
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'YearReported',
        ControlType = 'number',
        [Rule] = NULL

update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header

-- Detail Fields
INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo
SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Work Element Name',
        Description = 'Name of the work element in which the subproject is being conducted',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Salmon Habitat Protection and Restoration"",
            ""Increase Instream Habitat Complexity and Stabilization"",
            ""Install Fish Screen"",
            ""Install Fence"",
            ""Remove/Breach Fish Passage Barrier"",
            ""Plant Vegetation"",
            ""Remove Vegetation"",
            ""Practice No-till and Conservation Tillage Systems"",
            ""Enhance Floodplain/Remove, Modify, Breach Dike"",
            ""Remove/Install Diversion"",
            ""Maintain/Remove Vegetation"",
            ""Realign, Connect, and/or Create Channel"",
            ""Remove Mine Tailings"",
            ""Install Fish Passage Structure"",
            ""Decommission Road/Relocate Road"",
            ""Trap and Haul"",
            ""Install Sprinkler"",
            ""Develop Alternative Water Source"",
            ""Develop Terrestrial Habitat Features"",
            ""Acquire Water Instream"",
            ""Lease Land""
        ]',
        DbColumnName = 'WorkElementName',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Measure',
        Description = 'Unit of measure',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""acres improved in riparian areas"",
            ""acres treated instream"",
            ""miles treated"",
            ""miles of stream with improved complexity"",
            ""structures installed"",
            ""acre-feet/year of water protected"",
            ""cubic-feet per second (cfs) of water flow protected"",
            ""miles of primary stream reach improved"",
            ""miles of total stream reach improved"",
            ""miles of fence installed"",
            ""miles of stream improved in riparian areas"",
            ""miles of habitat accessed"",
            ""acres treated"",
            ""miles of stream with improved channel form"",
            ""barriers improved"",
            ""miles of dike removed or modified"",
            ""acres protected by fencing"",
            ""acres protected in riparian areas"",
            ""miles of stream fenced"",
            ""miles of stream protected in riparian areas"",
            ""barriers removed"",
            ""acre-feet/year of water conserved"",
            ""cubic-feet per second (cfs) of water flow conserved"",
            ""fish transported"",
            ""erosion control structures or water sources installed"",
            ""screens addressed"",
            ""screens installed or addressed"",
            ""miles of road or trail improved or decommissioned"",
            ""miles of road or trail relocated"",
            ""features developed""
        ]',
        DbColumnName = 'Measure',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Planned Value',
        Description = 'Planned result of the work element',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'PlannedValue',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Actual Value',
        Description = 'Actual result of the work element',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'ActualValue',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Comments',
        Description = 'Comments',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'Comments',
        ControlType = 'text',
        [Rule] = NULL

update #NewFieldInfo set FieldRoleId = 2 where FieldRoleId is NULL   -- 2 == detail

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
    
-- Add a record to the LocationTypes table
INSERT INTO dbo.LocationTypes(Name)
values (@datastoreName)

INSERT INTO dbo.MetadataProperties(MetadataEntityId, Name, [Description], DataType, PossibleValues, ControlType)
values (2, 'Collaborators', 'Collaborators', 'string', 
'[""Blue Mountain Habitat Restoration Council"",
""Bureau of Reclamation"",
""Bonneville Power Authority"",
""Columbia Conservation District"",
""CTUIR"",
""Eco Trust"",
""Grande Ronde Model Watershed"",
""Landowners"",
""Nez Perce Tribe"",
""NF John Day Watershed Council"",
""Natural Resource Conservation Service"",
""Oregon Department of Fish and Wildlife"",
""Oregon Department of Transportation"",
""Oregon Watershed Enhancement Board"",
""Other"",
""Pacific Coastal Salmon Recovery Fund"",
""Pomeroy Conservation District"",
""Salmon Recovery Funding Board"",
""Snake River Salmon Recovery Board"",
""Umatilla County Soil and Water Conservation District"",
""Umatilla National Forest"",
""US Forest Service"",
""Wallowa Whitman National Forest"",
""Washington Department of Fish and Wildlife""
]',
'multiselect')

--INSERT INTO dbo.MetadataProperties(MetadataEntityId, Name, [Description], DataType, PossibleValues, ControlType)
--values (2, 'Funding', 'Funding', 'string', 
--'[""Blue Mountain Habitat Restoration Council"",
--""Bureau of Reclamation"",
--""Bonneville Power Authority"",
--""Bureau of Reclamation"",
--""CTUIR"",
--""Eco Trust"",
--""Grande Ronde Model Watershed"",
--""NF John Day Watershed Council"",
--""Oregon Department of Fish and Wildlife"",
--""Oregon Watershed Enhancement Board"",
--""Other"",
--""Pacific Coastal Salmon Recovery Fund"",
--""Snake River Salmon Recovery Board"",
--""Umatilla County Soil and Water Conservation District"",
--""Umatilla National Forest"",
--""US Forest Service"",
--""Wallowa Whitman National Forest"",
--""Washington Department of Fish and Wildlife""
--]',
--'multiselect')


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
-- Note: These MUST match values declared in Up()
declare @datasetBaseName as varchar(max) = 'Metrics'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = @datasetBaseName

-- We did not add these, so we do not need to delete them.
--alter index IX_LocationTypeId on dbo.Locations disable
--alter index IX_SdeFeatureClassId on dbo.Locations disable
--alter index IX_WaterBodyId on dbo.Locations disable
--delete from dbo.Locations where id in (select location_id from dbo.LocationProjects where project_id in (select ProjectId from datasets where name = @datasetBaseName))

--alter index IX_Location_Id on dbo.LocationProjects disable
--alter index IX_Project_Id on dbo.LocationProjects disable
--delete from dbo.LocationProjects where project_id in (select ProjectId from datasets where name = @datasetBaseName)

delete from dbo.LocationTypes where Name = @datasetBaseName

alter index IX_Dataset_Id on dbo.DatasetQAStatus disable
alter index IX_QAStatus_Id on dbo.DatasetQAStatus disable
delete from dbo.DatasetQAStatus  where Dataset_Id in (select id from dbo.Datasets where name = @datasetBaseName)

alter index IX_Dataset_Id on dbo.DatasetQAStatus1 disable
alter index IX_QAStatus_Id on dbo.DatasetQAStatus1 disable
delete from dbo.DatasetQAStatus1 where Dataset_Id in (select id from dbo.Datasets where name = @datasetBaseName)

alter index IX_DatasetId on dbo.DatasetFields disable
alter index IX_FieldId on dbo.DatasetFields disable
alter index IX_FieldRoleId on dbo.DatasetFields disable
alter index IX_InstrumentId on dbo.DatasetFields disable
alter index IX_SourceId on dbo.DatasetFields disable
delete from dbo.DatasetFields where DatasetId in (select id from dbo.Datasets where name = @datasetBaseName)

alter index IX_FieldCategoryId on dbo.Fields disable
delete from dbo.Fields where FieldCategoryId in (select id from dbo.FieldCategories where name = @categoryName)

alter index IX_DatastoreId on dbo.Datasets disable
alter index IX_DefaultRowQAStatusId on dbo.Datasets disable
delete from dbo.Datasets where name = @datasetBaseName

delete from dbo.Datastores where name = @datastoreName
delete from dbo.FieldCategories where name = @categoryName
--delete from dbo.Projects where name = @

-- Rebuild the indexes, after deleting all stuff.
--alter index IX_LocationTypeId on dbo.Locations rebuild
--alter index IX_SdeFeatureClassId on dbo.Locations rebuild
--alter index IX_WaterBodyId on dbo.Locations rebuild

--alter index IX_Location_Id on dbo.LocationProjects rebuild
--alter index IX_Project_Id on dbo.LocationProjects rebuild

alter index IX_Dataset_Id on dbo.DatasetQAStatus rebuild
alter index IX_QAStatus_Id on dbo.DatasetQAStatus rebuild

alter index IX_Dataset_Id on dbo.DatasetQAStatus1 rebuild
alter index IX_QAStatus_Id on dbo.DatasetQAStatus1 rebuild

alter index IX_DatasetId on dbo.DatasetFields rebuild
alter index IX_FieldId on dbo.DatasetFields rebuild
alter index IX_FieldRoleId on dbo.DatasetFields rebuild
alter index IX_InstrumentId on dbo.DatasetFields rebuild
alter index IX_SourceId on dbo.DatasetFields rebuild

alter index IX_FieldCategoryId on dbo.Fields rebuild

alter index IX_DatastoreId on dbo.Datasets rebuild
alter index IX_DefaultRowQAStatusId on dbo.Datasets rebuild
            ");
        }
    }
}
