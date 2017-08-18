namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TurnOffInactiveUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.[Users] set Inactive = 1 
where Username in ('JayneB', 'AdrienE', 'SteveBoe', 'jonathanT', 'brianm', 'akadish',
'TanyaH', 'OlinA', 'anitian-user', 'JoelleO', 'OutsideTemp', 'ReyW', 'AndreaH',
'Sang-SeonY', 'innovate', 'victorz', 'cayusetech')

print 'Updated Inactive Users'

--CREATE TABLE #DatasetIds (id int)        -- This will contain a list of the ids of all AdultWeir datasets, where DatastoreId != null

-- This will contain a list of the ids of all AdultWeir datasets, where DatastoreId != null
select Id into #DatasetIds from dbo.Datasets where Name like '%Adult Weir%' and DatastoreId is not null

print 'Collected affected DatasetIds'

insert into dbo.[Fields] (FieldCategoryId, Name, [Description], DataType, DbColumnName, ControlType)
values(1, 'FieldSheetFile', 'Copy of the field sheet', 'string', 'FieldSheetFile', 'file')

-- This will contain info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    

-----
-- Insert any new fields we'll need (George says don't reuse fields)
-- Header Fields
INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT   
        FieldCategoryId = 1, 
        Name = 'FieldSheet File',
        Description = 'Copy of the field sheet',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'FieldSheetFile',
        ControlType = 'file',
        [Rule] = NULL

update #NewFieldInfo set FieldRoleId = 1 where FieldRoleId is NULL   -- 1 == Header

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
FROM #DatasetIds as d, #NewFieldInfo as f

print 'Added new field to tables Fields and DatasetFields'

update dbo.DatasetFields set OrderIndex = 10 where DatasetId = 1002 and DbColumnName = 'FieldSheetFile'
update dbo.DatasetFields set OrderIndex = 10 where DatasetId = 1003 and DbColumnName = 'FieldSheetFile'
update dbo.DatasetFields set OrderIndex = 20 where DatasetId = 1004 and DbColumnName = 'FieldSheetFile'
update dbo.DatasetFields set OrderIndex = 10 where DatasetId = 1005 and DbColumnName = 'FieldSheetFile'
update dbo.DatasetFields set OrderIndex = 30 where DatasetId = 1006 and DbColumnName = 'FieldSheetFile'

print 'Updated the OrderIndex in tables'

-- Cleanup
drop table #NewFieldInfo
drop table #DatasetIds

print 'Completed cleanup'
            ");
        }
        
        public override void Down()
        {
            Sql(@"
-- Set inactive users to Inactive
update dbo.[Users] set Inactive = null
where Username in ('JayneB', 'AdrienE', 'SteveBoe', 'jonathanT', 'brianm', 'akadish',
'TanyaH', 'OlinA', 'anitian-user', 'JoelleO', 'OutsideTemp', 'ReyW', 'AndreaH',
'Sang-SeonY', 'innovate', 'victorz', 'cayusetech')

delete from dbo.DatasetFields where DbColumnName = 'FieldSheetFile' and DatasetId in (select Id FROM dbo.[Datasets] where Name like '%Adult Weir%' and DatastoreId is not null)
delete from dbo.[Fields] where FieldCategoryId = 1 and DbColumnName = 'FieldSheetFile'

            ");
        }
    }
}
