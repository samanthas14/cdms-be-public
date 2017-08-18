namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCrppContractsDatasetData : DbMigration
    {
        public override void Up()
        {
            Sql(@"
-- Correct the name of CRPP in the Projects table
update dbo.[Projects]
set [Name] = 'Cultural Resources Protection Program'
where [Id] = 2247

-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'CRPP Contracts'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = 'CrppContracts'


-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 2247 
--UNION ALL SELECT id = 1135 
--UNION ALL SELECT id = 1188
--UNION ALL SELECT id = 1177


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
    Config                    = '{""RestrictRoles"":[""CRPP""],""ActivitiesPage"": {""Route"": ""crpp""}, ""DataEntryPage"": {""HiddenFields"": [""Location"",""ActivityDate"",""QA"",""Instrument""]}}'
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
        Name = 'CRPP Personnel',
        Description = 'CRPP Personnel',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Shawn"", ""Catherine"", ""Amy"", ""Teara"", ""Bambie"", ""Carey"", ""Arthur"", ""Jennifer"", ""Sari""]',
        DbColumnName = 'CrppPersonnel',
        ControlType = 'muliselect',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Activity Type ID',
        Description = 'Activity Type ID',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = (select Name from dbo.ActivityTypes),
        DbColumnName = 'ActivityTypeId',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Agency',
        Description = 'Agency',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'Agency',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Project Proponent',
        Description = 'Project Proponent',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ProjectProponent',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Permit Number',
        Description = 'Permit Number',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'PermitNumber',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Date Received',
        Description = 'Date Received',
        Units = 'mm/dd/yyyy',
        Validation = NULL,
        DataType = 'date',
        PossibleValues = NULL,
        DbColumnName = 'DateReceived',
        ControlType = 'date',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Date of Action',
        Description = 'Date of Action',
        Units = 'mm/dd/yyyy',
        Validation = NULL,
        DataType = 'date',
        PossibleValues = NULL,
        DbColumnName = 'DateOfAction',
        ControlType = 'date',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Action Taken',
        Description = 'Action Taken',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""No Response"", ""Report Generated""]',
        DbColumnName = 'ActionTaken',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Activity Notes',
        Description = 'Activity Notes',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ActivityNotes',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Attached Document',
        Description = 'Attached Document',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'AttachedDocument',
        ControlType = 'text',
        [Rule] = NULL

update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header

-----
-- Details Fields

INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo
SELECT FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Acres Surveyed',
        Description = 'Acres Surveyed',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'AcresSurveyed',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Testing',
        Description = 'Testing',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Yes"", ""No""]',
        DbColumnName = 'Testing',
        ControlType = 'select',
        [Rule] = NULL


UNION ALL SELECT FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'New Sites',
        Description = 'New Sites',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'NewSites',
        ControlType = 'number',
        [Rule] = NULL
         
UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Monitored Sites',
        Description = 'Monitored Sites',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MonitoredSites',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Sites Evaluated',
        Description = 'Sites Evaluated',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'SitesEvaluated',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Updated Sites',
        Description = 'Updated Sites',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'UpdatedSites',
        ControlType = 'number',
        [Rule] = NULL


UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'New Isolates',
        Description = 'New Isolates',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'NewIsolates',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Evaluation',
        Description = 'Evaluation',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Yes"", ""No"", ""Not Eligible"", ""Recommended""]',
        DbColumnName = 'Evaluation',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'HPRCSITS Recorded',
        Description = 'HPRCSITS Recorded',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Yes"", ""No""]',
        DbColumnName = 'HprcsitsRecorded',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Monitoring',
        Description = 'Monitoring',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Yes"", ""No"", ""Recommended""]',
        DbColumnName = 'Monitoring',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Notes',
        Description = 'Notes',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'Notes',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'Shpo Report Number',
        Description = 'Shpo Report Number',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ShpoReportNumber',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = (SELECT IDENT_CURRENT('dbo.FieldCategories')), 
        Name = 'ShpoCaseNumber',
        Description = 'ShpoCaseNumber',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ShpoCaseNumber',
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


-- Create a location type
INSERT INTO dbo.LocationTypes (Name, Description) 
SELECT Name        = @categoryName,
       Description = @datasetBaseName + ' related location'

-- Add CRPP to the Departments table
insert into dbo.[Departments](OrganizationId, Name, [Description])
values (1, 'CRPP', 'Cultural Resources Protection Program')

-- Add CRPP to the roles for the DBAs
update dbo.[Users] set [Roles] = '[""Admin"",""DECD"",""CRPP""]' where [Id] = 1081   -- George
update dbo.[Users] set [Roles] = '[""Admin"",""DECD"",""CRPP""]' where [Id] = 2      -- Colette
update dbo.[Users] set [Roles] = '[""Admin"",""DECD"",""CRPP""]' where [Id] = 9      -- Stacy

-- Add users from the CRPP office
insert into [dbo].[Users]
(
OrganizationId, Username, [Description], LastLogin, DepartmentId, Fullname, Roles
)
values
(1, 'BambiH', 'Assistant Program Manager', '2016-03-15', 1015, 'Bambi Rodriquez', '[""CRPP""]'),
--(1, 'ShawnS', 'Archaeologist I/GIS Coord/Asst/ Hanford Coord', '2016-03-15', 1015, 'Shawn Steinmetz', '[""CRPP""]'),
--(1, 'CatherineD', 'Principal Investigator', '2016-03-15', 1015, 'Catherine Dickson', '[""CRPP""]'),
(1, 'CareyM', 'Archaeologist I/THPO', '2016-03-15', 1015, 'Carey Miller', '[""CRPP""]'),
(1, 'JenniferK', 'Cultural Anthropologist', '2016-03-15', 1015, 'Jennifer Karson Engum', '[""CRPP""]'),
(1, 'ShariS', 'Cultural Resources Technician I/Monitor', '2016-03-15', 1015, 'Shari Sheoships', '[""CRPP""]'),
(1, 'ArthurVP', 'CRPP Technician', '2016-03-15', 1015, 'Arthur Van Pelt', '[""CRPP""]')
--(1, 'AmyS', 'Archeologist I', '2016-03-15', 1015, 'Amy Senn', '[""CRPP""]' ) Already exists
--(1, 'HollyB', 'Senior Archaeologist', '2016-07-18', 1015, 'Holly Shea Barrick', '[""CRPP""]'),
--(1, 'WilburB', 'Field Archaeologist', '2016-07-18', 1015, 'Wilbur Barrick', '[""CRPP""]')

-- Update ShawnS
update dbo.Users
set [Description] = 'Archaeologist I/GIS Coord/Asst/ Hanford Coord', DepartmentId = 1015, Fullname = 'Shawn Steinmetz', Roles = '[""CRPP""]'
where [Id] = 3116

-- Update CatherineD
update dbo.Users
set [Description] = 'Principal Investigator', DepartmentId = 1015, Fullname = 'Catherine Dickson', Roles = '[""CRPP""]'
where [Id] = 3119

-- Update AmyS
update dbo.Users
set [Description] = 'Archeologist I', DepartmentId = 1015, Fullname = 'Amy Senn', Roles = '[""CRPP""]'
where [Id] = 1054

-- Update HollyB
update dbo.Users
set [Description] = 'Senior Archaeologist', DepartmentId = 1015, Fullname = 'Holly Shea Barrick', Roles = '[""CRPP""]'
where [Id] = 3120

-- Update WilburB
update dbo.Users
set [Description] = 'Field Archaeologist', DepartmentId = 1015, Fullname = 'Wilbur Barrick', Roles = '[""CRPP""]'
where [Id] = 3121


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
declare @datasetBaseName as varchar(max) = 'CRPP Contracts'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = 'CrppContracts'

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

delete from dbo.FieldCategories where name = @categoryName
delete from dbo.Datastores where name = @datastoreName

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

-- Remove CRPP from the roles for the DBAs
update dbo.[Users] set [Roles] = '[""Admin"",""DECD""]' where [Id] = 1081   -- George
update dbo.[Users] set [Roles] = '[""Admin"",""DECD""]' where [Id] = 2      -- Colette
update dbo.[Users] set [Roles] = '[""Admin"",""DECD""]' where [Id] = 9      -- Stacy

-- Remove users from the CRPP office
delete [dbo].[Users]where Username = 'BambiH'
delete [dbo].[Users]where Username = 'ShawnS'
delete [dbo].[Users]where Username = 'CatherineD'
delete [dbo].[Users]where Username = 'CareyM'
delete [dbo].[Users]where Username = 'JenniferK'
delete [dbo].[Users]where Username = 'ShariS'
delete [dbo].[Users]where Username = 'ArthurVP'
--delete [dbo].[Users]where Username = 'AmyS'
delete [dbo].[Users]where Username = 'HollyB'
delete [dbo].[Users]where Username = 'WilburB'
            ");
        }
    }
}
