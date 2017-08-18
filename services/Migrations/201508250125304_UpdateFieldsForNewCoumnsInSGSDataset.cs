namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFieldsForNewCoumnsInSGSDataset : DbMigration
    {
        public override void Up()
        {
            Sql(@"

CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    
select id into #NewDatasetIds from datasets where name = 'Spawning Ground Survey'


INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT   
        FieldCategoryId = (select id from FieldCategories where name = 'Spawning Ground Survey'), 
        Name = 'GeneticID',
        Description = 'Genetic Id',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'GeneticID',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT   
        FieldCategoryId = (select id from FieldCategories where name = 'Spawning Ground Survey'), 
        Name = 'KidneyID',
        Description = 'Kidney Id',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'KidneyID',
        ControlType = 'text',
        [Rule] = NULL

update #NewFieldInfo set FieldRoleId = 2    -- 2 == data


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
    OrderIndex     = 415, 
    ControlType    = f.ControlType,
    [Rule]         = f.[Rule]
FROM #NewDatasetIds as d, #NewFieldInfo as f

update datasetfields set OrderIndex = 417 where label = 'KidneyID'



drop table #NewFieldInfo
drop table #NewDatasetIds


");
        }
        
        public override void Down()
        {
            Sql(@"
    delete from fields where name in('KidneyID','GeneticID')
    delete from datasetfields where label in ('KidneyID','GeneticID')
");
        }
    }
}
