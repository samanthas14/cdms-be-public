namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DecdAddNewFieldOptions : DbMigration
    {
        public override void Up()
        {
            Sql(@"
-- This will info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int)    

-- Header Fields
INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT   
        FieldCategoryId = 5, 
        Name = 'Highest and Best Use',
        Description = 'Highest and Best Use of the parcel',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Add on Farm Tract"",""Cropland w/Timber Production"",""Grazing in a Range Unit"",""Grazing Land w/Timber Production"",""Other"",""Pasture"",""Residential"",""Wildlife Habitat""]',
        DbColumnName = 'HighestAndBestUse',
        ControlType = 'select',
        [Rule] = NULL

update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == header

-- Detail Fields
INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo
SELECT
        FieldCategoryId = 5, 
        Name = 'Other Permits or Leases',
        Description = 'Other Permits or Leases',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        --PossibleValues = '[""NO"",""YES""]',
        PossibleValues = NULL,
        DbColumnName = 'OtherPermitLeases',
        ControlType = 'file',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 5, 
        Name = 'Request Number',
        Description = 'BIA Request Number',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'RequestNumber',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 5, 
        Name = 'NWRO Comments',
        Description = 'NWRO Comments',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Acceptable"",""Not Acceptable"",""Approved""]',
        DbColumnName = 'NwroComments',
        ControlType = 'select',
        [Rule] = NULL

update #NewFieldInfo set FieldRoleId = 2 where FieldRoleId is NULL   -- 2 == detail

-- Assign new fields to the datasets -- this will insert a new row for each combination of datasetId and fieldId for the records inserted above
INSERT INTO dbo.DatasetFields(DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, Validation, SourceId, InstrumentId, ControlType, [Rule])
SELECT
    DatasetId      = 1193,
    FieldId        = f.id,
    FieldRoleId    = f.FieldRoleId,
    CreateDateTime = GetDate(),
    Label          = f.fieldName,
    DbColumnName   = f.DbColumnName,
    Validation     = f.Validation,
    SourceId       = 1,
    InstrumentId   = NULL,
    --OrderIndex     = f.OrderIndex * 10,     -- x10 to make it easier to insert intermediate orders
    ControlType    = f.ControlType,
    [Rule]         = f.[Rule]
FROM #NewFieldInfo as f

-- This field is already in the database, but we will update the options.
update dbo.Fields
set PossibleValues = '[""Estate"",""Fair Market Value"",""Fair Market Rental Value"",""Market Study""]'
where FieldCategoryId = 5 and DbColumnName = 'TypeOfTransaction'

insert into dbo.DatasetFields(DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, Validation, SourceId, InstrumentId, OrderIndex, ControlType, [Rule])
values (1193, 1117, 2, GetDate(), 'Transaction Type', 'TypeOfTransaction', null, null, null, 130,'select', null)

insert into dbo.DatasetFields(DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, Validation, SourceId, InstrumentId, OrderIndex, ControlType, [Rule])
values (1193, 1118, 2, GetDate(), 'Parties Involved', 'PartiesInvolved', null, null, null, 140,'text', null)

update dbo.[DatasetFields]
set OrderIndex = OrderIndex * 10
where DatasetId = 1193

update dbo.[DatasetFields] set OrderIndex = 190 where DatasetId = 1193 and DbColumnName = 'HighestAndBestUse'
update dbo.[DatasetFields] set OrderIndex = 100 where DatasetId = 1193 and DbColumnName = 'OtherPermitLeases'
update dbo.[DatasetFields] set OrderIndex = 110 where DatasetId = 1193 and DbColumnName = 'RequestNumber'
update dbo.[DatasetFields] set OrderIndex = 120 where DatasetId = 1193 and DbColumnName = 'NwroComments'

--update dbo.[Appraisal_Header] set [AllotmentStatus] = 'Approved' where CobellAppraisalWave = 'Wave 1'
update dbo.Appraisal_Header
set AllotmentStatus = 'Approved' 
where [Id] in (select Id from dbo.Appraisal_Header_VW where CobellAppraisalWave = 'Wave 1')

--update dbo.[Appraisal_Header] set [AllotmentStatus] = 'Submitted for OAS Review' where CobellAppraisalWave = 'Wave 2'
update dbo.Appraisal_Header
set AllotmentStatus = 'Submitted for OAS Review' 
where [Id] in (select Id from dbo.Appraisal_Header_VW where CobellAppraisalWave = 'Wave 2')

update dbo.Appraisal_Detail
set AppraisalValuationDate = '2014-09-15 00:00:00.000' 
where [Id] in (SELECT Appraisal_Detail_Id FROM [CDMS_DEV].[dbo].[Appraisal_VW] where CobellAppraisalWave = 'Wave 2')

delete from dbo.[Users] where [Id] = 3138 -- Avary has a duplicate entry.
update dbo.Users set Fullname = 'Avary McKay' where UserName = 'avarym' -- Update Avary's full name.
insert into dbo.[ProjectUsers](Project_Id, User_Id) values(2246, 3118) -- Add Avary to the Project Editors.
insert into dbo.[ProjectUsers](Project_Id, User_Id) values(2246, 10)   -- Nicole Novak
insert into dbo.[ProjectUsers](Project_Id, User_Id) values(2246, 1060) -- Rachel Matamoros
insert into dbo.[ProjectUsers](Project_Id, User_Id) values(2246, 1061) -- Appraisal Contractor
insert into dbo.[ProjectUsers](Project_Id, User_Id) values(2246, 1063) -- Kelly George
insert into dbo.[ProjectUsers](Project_Id, User_Id) values(2246, 1064) -- Leslie LeComu
insert into dbo.[ProjectUsers](Project_Id, User_Id) values(2246, 1065) -- Stephanie Quaempts
insert into dbo.[ProjectUsers](Project_Id, User_Id) values(2246, 1066) -- Daisy Minthorn
insert into dbo.[ProjectUsers](Project_Id, User_Id) values(2246, 1067) -- Candice Cowapoo
insert into dbo.[ProjectUsers](Project_Id, User_Id) values(2246, 1068) -- Michael Jackson
insert into dbo.[ProjectUsers](Project_Id, User_Id) values(2246, 1069) -- Andrea Kadish
insert into dbo.[ProjectUsers](Project_Id, User_Id) values(2246, 1070) -- Koko Hufford
--insert into dbo.[ProjectUsers](Project_Id, User_Id) values(2246, 1073) -- Andrea Hall
            ");

        }

        public override void Down()
        {
            Sql(@"
delete from dbo.DatasetFields where DatasetId = 1193 and DbColumnName = 'OtherPermitLeases'
delete from dbo.DatasetFields where DatasetId = 1193 and DbColumnName = 'RequestNumber'
delete from dbo.DatasetFields where DatasetId = 1193 and DbColumnName = 'NwroComments'

delete from dbo.Fields where FieldCategoryId = 5 and DbColumnName = 'OtherPermitLeases'
delete from dbo.Fields where FieldCategoryId = 5 and DbColumnName = 'RequestNumber'
delete from dbo.Fields where FieldCategoryId = 5 and DbColumnName = 'NwroComments'

update dbo.Appraisal_Header
set AllotmentStatus = r.AllotmentStatus
from (select Id, AllotmentStatus from dbo.Appraisal_Header_BefMassUpdates) as r
where dbo.Appraisal_Header.Id = r.Id

update dbo.Appraisal_Detail
set AppraisalValuationDate = r.AppraisalValuationDate
from (select Id, AppraisalValuationDate from dbo.Appraisal_Detail_BefMassUpdates) as r
where dbo.Appraisal_Detail.Id = r.Id

delete from dbo.[ProjectUsers] where Project_Id = 2246 and User_Id = 10     -- Nicole Novak
delete from dbo.[ProjectUsers] where Project_Id = 2246 and User_Id = 1060   -- Rachel Matamoros
delete from dbo.[ProjectUsers] where Project_Id = 2246 and User_Id = 1061   -- Appraisal Contractor
delete from dbo.[ProjectUsers] where Project_Id = 2246 and User_Id = 1063   -- Kelly George
delete from dbo.[ProjectUsers] where Project_Id = 2246 and User_Id = 1064   -- Leslie LeComu
delete from dbo.[ProjectUsers] where Project_Id = 2246 and User_Id = 1065   -- Stephanie Quaempts
delete from dbo.[ProjectUsers] where Project_Id = 2246 and User_Id = 1066   -- Daisy Minthorn
delete from dbo.[ProjectUsers] where Project_Id = 2246 and User_Id = 1067   -- Candice Cowapoo
delete from dbo.[ProjectUsers] where Project_Id = 2246 and User_Id = 1068   -- Michael Jackson
delete from dbo.[ProjectUsers] where Project_Id = 2246 and User_Id = 1069   -- Andrea Kadish
delete from dbo.[ProjectUsers] where Project_Id = 2246 and User_Id = 1070   -- Koko Hufford
            ");
        }
    }
}
