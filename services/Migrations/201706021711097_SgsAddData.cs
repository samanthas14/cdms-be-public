namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SgsAddData : DbMigration
    {
        public override void Up()
        {
            Sql(@"
-- Convert field SpawningStatus to PercentRetained.
update dbo.DatasetFields set DbColumnName = 'PercentRetained' where DatasetId = 1209 and DbColumnName = 'SpawningStatus'
update dbo.Fields set DbColumnName = 'PercentRetained' where FieldCategoryId = 6 and DbColumnName = 'SpawningStatus'


-- This will info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    

-----
-- Insert any new fields we'll need (George says don't reuse fields)
-- Header Fields
INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT   
        FieldCategoryId = 6, 
        Name = 'Number of Eggs Retained',
        Description = 'Number of Eggs Retained',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'NumberEggsRetained',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 6, 
        Name = 'Mortality Type',
        Description = 'Mortality Type',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""ANGLER"", ""SPAWNING GROUND"", ""TRAP/HAUL""]',
        DbColumnName = 'MortalityType',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 6, 
        Name = 'Redd Measurements',
        Description = 'Redd Measurements',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ReddMeasurements',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 6, 
        Name = 'Spawning Status',
        Description = 'Spawning Status',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""PRESPAWN"", ""SPAWNED OUT"", ""PARTIAL SPAWN"", ""NA""]',
        DbColumnName = 'SpawningStatus',
        ControlType = 'select',
        [Rule] = NULL

update #NewFieldInfo set FieldRoleId = 2 where FieldRoleId is NULL   -- 2 == details


-- Assign new fields to the datasets -- this will insert a new row for each combination of datasetId and fieldId for the records inserted above
INSERT INTO dbo.DatasetFields(DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, Validation, SourceId, InstrumentId, OrderIndex, ControlType, [Rule])
SELECT
    DatasetId      = 1209,
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
FROM #NewFieldInfo as f

update dbo.DatasetFields set OrderIndex = 430 where DatasetId = 1209 and DbColumnName = 'NumberEggsRetained'
update dbo.DatasetFields set OrderIndex = 440 where DatasetId = 1209 and DbColumnName = 'MortalityType'
update dbo.DatasetFields set OrderIndex = 450 where DatasetId = 1209 and DbColumnName = 'ReddMeasurements'
update dbo.DatasetFields set OrderIndex = 460 where DatasetId = 1209 and DbColumnName = 'SpawningStatus'


-- Cleanup
drop table #NewFieldInfo

            ");
        }

        public override void Down()
        {
            Sql(@"
delete from dbo.DatasetFields where DatasetId = 1209 and DbColumnName = 'NumberEggsRetained'
delete from dbo.DatasetFields where DatasetId = 1209 and DbColumnName = 'MortalityType'
delete from dbo.DatasetFields where DatasetId = 1209 and DbColumnName = 'ReddMeasurements'
delete from dbo.DatasetFields where DatasetId = 1209 and DbColumnName = 'SpawningStatus'

delete from dbo.Fields where FieldCategoryId = 6 and DbColumnName = 'NumberEggsRetained'
delete from dbo.Fields where FieldCategoryId = 6 and DbColumnName = 'MortalityType'
delete from dbo.Fields where FieldCategoryId = 6 and DbColumnName = 'ReddMeasurements'
delete from dbo.Fields where FieldCategoryId = 6 and DbColumnName = 'SpawningStatus'

-- Convert field PercentRetained to SpawningStatus.
update dbo.DatasetFields set DbColumnName = 'SpawningStatus' where DatasetId = 1209 and DbColumnName = 'PercentRetained'
update dbo.Fields set DbColumnName = 'SpawningStatus' where FieldCategoryId = 6 and DbColumnName = 'PercentRetained'
            ");
        }
    }
}
