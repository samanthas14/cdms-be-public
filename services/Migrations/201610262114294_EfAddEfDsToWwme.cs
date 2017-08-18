namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EfAddEfDsToWwme : DbMigration
    {
        public override void Up()
        {
            Sql(@"
declare @datasetBaseName as varchar(max) = 'WWME-Electrofishing'

-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 1177 

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
    [Description]             = @datasetBaseName + ': Walla Walla Basin Monitoring and Evaluation',	
    DefaultActivityQAStatusId = 6,
    DatastoreId               = 9,
    Config                    = '{""DataEntryPage"": {""HiddenFields"": [""Instrument""]}}'
FROM #ProjectInfo as p
print 'Added to Datasets'

--select * from #NewDatasetIds

-- This will contain info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))
print 'Created #NewFieldInfo'

insert into #NewFieldInfo (id, fieldName, DbColumnName, [Validation], ControlType, [Rule])
select f.Id, f.Name, f.DbColumnName, f.Validation, f.ControlType, f.[Rule]
from dbo.Fields f where f.FieldCategoryId = 10
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
CREATE TABLE #QaStatusIds (id int)
INSERT INTO #QaStatusIds (id) 
          SELECT id = 5     -- Approved
UNION ALL SELECT id = 6     -- Ready for QA
print 'Created #QaStatusIds'

INSERT INTO dbo.DatasetQAStatus(Dataset_Id, QAStatus_id)
SELECT
    Dataset_Id  = d.id,
    QAStatus_id = q.id
FROM #NewDatasetIds as d, #QaStatusIds as q
print 'Added to DatasetQAStatus'

--select * from dbo.DatasetQAStatus dqs inner join #NewDatasetIds on dqs.Dataset_Id = #NewDatasetIds.id


INSERT INTO dbo.DatasetQAStatus1(Dataset_Id, QAStatus_id)
SELECT
    Dataset_Id  = d.id,
    QAStatus_id = q.id
FROM #NewDatasetIds as d, #QaStatusIds as q
print 'Added to DatasetQAStatus1'

--select * from dbo.DatasetQAStatus dqs1 inner join #NewDatasetIds on dqs1.Dataset_Id = #NewDatasetIds.id

declare @theDatasetId as int
select @theDatasetId = id from #NewDatasetIds
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'Sequence'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'PitTagCode'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'ChannelUnitType'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'SpeciesRunRearing'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'ForkLength'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'TotalLength'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'OtherSpecies'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'FishCount'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'SizeCategory'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'ConditionalComment'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'TextualComments'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'Note'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'TagStatus'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'ClipStatus'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'OtolithID'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'GeneticID'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'OtherID'
update dbo.DatasetFields set FieldRoleId = 2 where DatasetId = @theDatasetId and DbColumnName = 'AdditionalPositionalComment'

drop table #ProjectInfo
drop table #NewDatasetIds
drop table #NewFieldInfo
drop table #QaStatusIds
            ");
        }
        
        public override void Down()
        {
            Sql(@"
declare @datasetBaseName as varchar(max) = 'WWME-Electrofishing'

declare @theDatasetId as int
select @theDatasetId = id from dbo.Datasets where Name = @datasetBaseName

delete from dbo.DatasetQAStatus1 where Dataset_Id = @theDatasetId
delete from dbo.DatasetQAStatus where Dataset_Id = @theDatasetId
delete from dbo.DatasetFields where DatasetId = @theDatasetId
delete from dbo.Datasets where Id = @theDatasetId
            ");
        }
    }
}
