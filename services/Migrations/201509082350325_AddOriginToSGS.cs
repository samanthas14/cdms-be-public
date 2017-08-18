using System.Collections.Generic;

namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOriginToSGS : DbMigration
    {
        private static readonly List<string> vals = new List<string>() {
            // Name      dbName      dbType      CtrlType    Descr              Rule    Order  PossibleValues
            "'Origin'", "'Origin'", "'string'", "'select'", "'Origin of fish'", "NULL", "345", @"'{""NAT"":""Natural"",""HAT"":""Hatchery"",""UNK"":""Unknown""}'"
        };

        private string datasetName = "Spawning Ground Survey";

        public override void Up()
        {
            AddColumn("dbo.SpawningGroundSurvey_Detail", "Origin", c => c.String());

            Sql(@"
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    
select id into #NewDatasetIds from datasets where name = 'Spawning Ground Survey'


INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT   
        FieldCategoryId = (select id from FieldCategories where name = '" + datasetName + @"'), 
        Name = " + GetFieldName(0) + @",
        Description = " + GetDescr(0) + @",
        Units = NULL,
        Validation = NULL,
        DataType = " + GetDataType(0) + @",
        PossibleValues = " + GetPossibleValues(0) + @",
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
    delete from datasetfields where fieldId in (select id from fields where name in(" + GetFieldNameList() + @") and FieldCategoryId = (select id from FieldCategories where name = '" + datasetName + @"'))
    delete from fields where name in(" + GetFieldNameList() + @") and FieldCategoryId = (select id from FieldCategories where name = '" + datasetName + @"')
");

            DropColumn("dbo.SpawningGroundSurvey_Detail", "Origin");
        }


        private string GetFieldNameList()
        {
            return GetFieldName(0);
        }


        private string GetFieldName(int index)
        {
            return vals[0];
        }

        private string GetDbFieldName(int index)
        {
            return vals[1];
        }

        private string GetDataType(int index)
        {
            return vals[2];
        }

        private string GetControlType(int index)
        {
            return vals[3];
        }

        private string GetDescr(int index)
        {
            return vals[4];
        }

        private string GetRule(int index)
        {
            return vals[5];
        }

        private string GetOrder(int index)
        {
            return vals[6];
        }

        private string GetPossibleValues(int index)
        {
            return vals[7];
        }     
    }
}
