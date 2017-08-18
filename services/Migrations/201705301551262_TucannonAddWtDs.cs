namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TucannonAddWtDs : DbMigration
    {
        public override void Up()
        {
            Sql(@"
-- Cleanout old backup tables.
drop table dbo.FieldsBu
go
drop table dbo.DatasetFieldsBu
go

-- Make new backup tables for Colette
select * into dbo.FieldsBu from dbo.Fields
go
select * into dbo.DatasetFieldsBu from dbo.DatasetFields
go


declare @datasetBaseName as varchar(max) = 'TucHab-Water Temp'

-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 2229

print 'Created #ProjectInfo'

CREATE TABLE #NewDatasetIds (id int)        -- This will contain a list of ids of all dataset records inserted below
print 'Created #NewDatasetIds'

-- Add record to the Datasets --> will create one record per project
INSERT INTO	Datasets (ProjectId, DefaultRowQAStatusId, StatusId, CreateDateTime, Name, [Description], DefaultActivityQAStatusId, DatastoreId, Config)
OUTPUT INSERTED.id into #NewDatasetIds
SELECT 
    ProjectId                 = p.id,
    DefaultRowQAStatusId      = 1,
    StatusId                  = 1,
    CreateDateTime            = GetDate(),
    Name                      = @datasetBaseName,
    --[Description]             = @datasetBaseName + ': ' + p.name,
    [Description]             = @datasetBaseName + ': Surface Water Temperature',	
    DefaultActivityQAStatusId = 6,
    DatastoreId               = 3,
    Config                    = '{""DataEntryPage"": {""HiddenFields"": [""ActivityDate""], ""ShowFields"": [""Timezone""]}}'
FROM #ProjectInfo as p
print 'Added to Datasets'

--select * from #NewDatasetIds

-- This will contain info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))
print 'Created #NewFieldInfo'

insert into #NewFieldInfo (id, fieldName, DbColumnName, [Validation], ControlType, [Rule])
select f.Id, f.Name, f.DbColumnName, f.Validation, f.ControlType, f.[Rule]
from dbo.Fields f where f.FieldCategoryId = 2
print 'Added to #NewFieldInfo'

update #NewFieldInfo set FieldRoleId = 1  -- Header
print 'Updated #NewFieldInfo'

insert into dbo.DatasetFields(DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, [Validation], SourceId, InstrumentId, OrderIndex, ControlType, [Rule])
select 
    DatasetId      = d.id,
    FieldId        = f.id,
    FieldRoleId    = f.FieldRoleId,
    CreateDateTime = GetDate(),
    Label          = f.fieldName,
    DbColumnName   = f.DbColumnName,
    [Validation]   = f.Validation,
    SourceId       = 1,
    InstrumentId   = NULL,
    OrderIndex     = f.OrderIndex * 10,     -- x10 to make it easier to insert intermediate orders
    ControlType    = f.ControlType,
    [Rule]         = f.[Rule]
FROM #NewDatasetIds as d, #NewFieldInfo as f
--FROM #NewDatasetIds d cross join #NewFieldInfo f
print 'Added to DatasetFields'

--select * from dbo.DatasetFields df inner join #NewDatasetIds on df.Id = #NewDatasetIds.id

-- Add some new DatasetQAStatus records for our new datasets
-- The following are WaterTemp QA Statuses
CREATE TABLE #QaStatusIds (id int)
INSERT INTO #QaStatusIds (id) 
          SELECT id = 1     -- OK
UNION ALL SELECT id = 2     -- Bad
UNION ALL SELECT id = 3     -- Invalid
UNION ALL SELECT id = 4     -- Out of Range
UNION ALL SELECT id = 5     -- Approved
UNION ALL SELECT id = 6     -- Ready for QA
UNION ALL SELECT id = 12    -- Pre-deployment
UNION ALL SELECT id = 13    -- Post-deployment
UNION ALL SELECT id = 14    -- Sediment
UNION ALL SELECT id = 15    -- Out of water
UNION ALL SELECT id = 16    -- Malfunction
UNION ALL SELECT id = 17    -- Logging-Do Not Use
--print 'Created #QaStatusIds'

-- This table has the Activity-level QA Statuses
INSERT INTO dbo.DatasetQAStatus(Dataset_Id, QAStatus_id)
SELECT
    Dataset_Id  = d.id,
    QAStatus_id = q.id
FROM #NewDatasetIds as d, #QaStatusIds as q
where q.id = 5 or q.id = 6
--print 'Added to DatasetQAStatus'

--select * from dbo.DatasetQAStatus dqs inner join #NewDatasetIds on dqs.Dataset_Id = #NewDatasetIds.id

-- This table has the Row-level (Detail) QA Statuses
INSERT INTO dbo.DatasetQAStatus1(Dataset_Id, QAStatus_id)
SELECT
    Dataset_Id  = d.id,
    QAStatus_id = q.id
FROM #NewDatasetIds as d, #QaStatusIds as q
where q.id != 5 and q.id != 6
--print 'Added to DatasetQAStatus1'

--select * from dbo.DatasetQAStatus dqs1 inner join #NewDatasetIds on dqs1.Dataset_Id = #NewDatasetIds.id

declare @theDatasetId as int
select @theDatasetId = id from #NewDatasetIds
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'BatteryVolts'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'ReadingDateTime'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'WaterTemperature'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'WaterTemperatureF'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'AirTemperature'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'AirTemperatureF'
update dbo.DatasetFields set FieldRoleId = 5 where DatasetId = @theDatasetId and DbColumnName = 'ReadingDate'
update dbo.DatasetFields set FieldRoleId = 5 where DatasetId = @theDatasetId and DbColumnName = 'ReadingTime'

drop table #ProjectInfo
drop table #NewDatasetIds
drop table #NewFieldInfo
drop table #QaStatusIds
            ");
        }

        public override void Down()
        {
        }
    }
}
