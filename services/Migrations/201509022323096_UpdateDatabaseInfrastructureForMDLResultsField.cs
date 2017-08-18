namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabaseInfrastructureForMDLResultsField : DbMigration
    {
        public override void Up()
        {
            Sql(@"
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    
select id into #NewDatasetIds from datasets where name = 'Water Quality with Labs'


INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT   
        FieldCategoryId = (select id from FieldCategories where name = 'Water Quality with Labs'), 
        Name = " + GetFieldName(0) + @",
        Description = " + GetDescr(0) + @",
        Units = NULL,
        Validation = NULL,
        DataType = " + GetDataType(0) + @",
        PossibleValues = NULL,
        DbColumnName = " + GetDbFieldName(0) + @",
        ControlType = " + GetControlType(0) + @",
        [Rule] = " + GetRule(0) + @"

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
    OrderIndex     =  " + GetOrder(0) + @", 
    ControlType    = f.ControlType,
    [Rule]         = f.[Rule]
FROM #NewDatasetIds as d, #NewFieldInfo as f



drop table #NewFieldInfo
drop table #NewDatasetIds


");
        }

        public override void Down()
        {
            Sql(@"
    delete from datasetfields where label in (" + GetFieldNames() + @")
    delete from fields where name in(" + GetFieldNames() + @")
");
        }


        private static readonly Tuple<string, string, string, string, string, string, string> vals = new Tuple<string, string, string, string, string, string, string>(
                // Name             dbName         dbType      CtrlType    Descr                       Rule   Order
                "'MDL Results'", "'MdlResults'", "'string'", "'text'", "'Non-numeric results field'", "NULL", "55"
            );

        private string GetFieldNames()
        {
            return GetFieldName(0);
        }
    
        
        private string GetFieldName(int index)
        {
            return vals.Item1;
        }           
        
        private string GetDbFieldName(int index)
        {
            return vals.Item2;
        }

        private string GetDataType(int index)
        {
            return vals.Item3;
        }

        private string GetControlType(int index)
        {
            return vals.Item4;
        }

        private string GetDescr(int index)
        {
            return vals.Item5;
        }

        private string GetRule(int index)
        {
            return vals.Item6;
        }

        private string GetOrder(int index)
        {
            return vals.Item7;
        } 
    }
}