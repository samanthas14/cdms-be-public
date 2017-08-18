using System.Collections.Generic;

namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WaterQualityUpdatesFromOct1 : DbMigration
    {
        private static readonly List<Tuple<string, List<string>>> vals = new List<Tuple<string, List<string>>> { 

           // select * from FieldCategories
           //     FieldCategory                         Name                H/D     dbName            dbType      CtrlType         Descr              Units           Rule                   Order     PossValues                                                                                                                                                                                                                                                                                                                                                                                                                                 Order    PossibleValues               
                                                                                                                                                                                                                                  
        // Add new columns                                                                                                                                                                                                        
           new Tuple<string, List<string>>                                                                                                                                                                                             
    /* 0 */   ("Water Quality with Labs", new List<string> { "'Sample Fraction'", DET, "'SampleFraction'", "'string'", "'select'", "'Sample Fraction'", "NULL", "NULL", "100", @"'{""DISSOLVED"":""Dissolved"", ""TOTAL"":""Total""}'" } ),
            new Tuple<string, List<string>>
    /* 1 */   ("Water Quality with Labs", new List<string> { "'Method Speciation'", DET, "'MethodSpeciation'", "'string'", "'select'", "'Method Speciation'", "NULL", "NULL", "110", @"'{ ""AS CACO3"":""as CaCO3"", ""AS N"":""as N"", ""AS P"":""as P"" }'" } ),
            new Tuple<string, List<string>>
    /* 2 */   ("Water Quality with Labs", new List<string> { "'Detection Limit'", DET, "'DetectionLimit'", "'string'", "'text'", "'Detection Limit'", "NULL", "NULL", "120", "NULL" } ),
            new Tuple<string, List<string>>
    /* 3 */   ("Water Quality with Labs", new List<string> { "'Context ID'", DET, "'ContextID'", "'string'", "'select'", "'Context ID'", "NULL", "NULL", "130", @"'[""USEPA"", ""APHA"", ""USGS""]'" } ),
            new Tuple<string, List<string>>
    /* 4 */   ("Water Quality with Labs", new List<string> { "'Method ID'", DET, "'MethodID'", "'string'", "'text'", "'Method ID'", "NULL", "NULL", "140", "NULL" } ),
            new Tuple<string, List<string>>
    /* 5 */   ("Water Quality with Labs", new List<string> { "'Lab Name'", DET, "'LabName'", "'string'", "'select'", "'Lab Name'", "NULL", "NULL", "150", @"'{""BOR-BOISE, ID"":""BOR-Boise, ID"", ""COFFEY LABS"":""Coffey Labs"", ""DEQ-PORTLAND"":""DEQ-Portland"", ""PACE ANALYTICAL"":""PACE Analytical"", ""PENDLETON LAB"":""Pendleton Lab"", ""TABLE ROCK ANALYTICAL"":""Table Rock Analytical"", ""TEST AMERICA"":""Test America""}'" } ),

        };



        public override void Up()
        {
            Sql(GetAddFieldSql(0));
            Sql(GetAddFieldSql(1));
            Sql(GetAddFieldSql(2));
            Sql(GetAddFieldSql(3));
            Sql(GetAddFieldSql(4));
            Sql(GetAddFieldSql(5));
        }

        public override void Down()
        {
            Sql(GetRemoveFieldSql(0));
            Sql(GetRemoveFieldSql(1));
            Sql(GetRemoveFieldSql(2));
            Sql(GetRemoveFieldSql(4));
            Sql(GetRemoveFieldSql(5));
            Sql(GetRemoveFieldSql(6));
        }


        private string GetAddFieldSql(int index)
        {
            string datasetName = vals[index].Item1;

            return @"
                CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    
    select id into #NewDatasetIds from datasets where name = '" + datasetName + @"'


    INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
    OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

    SELECT   
            FieldCategoryId = (select id from FieldCategories where name = '" + datasetName + @"'), 
            Name = " + GetFieldName(index) + @",
            Description = " + GetDescr(index) + @",
            Units =  " + GetUnits(index) + @",
            Validation = NULL,
            DataType = " + GetDataType(index) + @",
            PossibleValues = " + GetPossibleValues(index) + @",
            DbColumnName = " + GetDbFieldName(index) + @",
            ControlType = " + GetControlType(index) + @",
            [Rule] = " + GetRule(index) + @"

    update #NewFieldInfo set FieldRoleId = " + GetFieldType(index) + @"


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
        OrderIndex     =  " + GetOrder(index) + @", 
        ControlType    = f.ControlType,
        [Rule]         = f.[Rule]
    FROM #NewDatasetIds as d, #NewFieldInfo as f

    drop table #NewFieldInfo
    drop table #NewDatasetIds
    go
    ";
        }


        private string GetRemoveFieldSql(int index)
        {
            string datasetName = vals[index].Item1;

            return @"    
    delete from datasetfields where fieldId in (select id from fields where name in(" + GetFieldNameList(index) + @") and FieldCategoryId = (select id from FieldCategories where name = '" + datasetName + @"'))
    delete                                                from fields where name in(" + GetFieldNameList(index) + @") and FieldCategoryId = (select id from FieldCategories where name = '" + datasetName + @"')
    go
";
        }

        private const string HEAD = "1";
        private const string DET = "2";


        private string GetFieldNameList(int index)
        {
            return GetFieldName(index);
        }


        private string GetFieldName(int index)
        {
            return vals[index].Item2[0];
        }

        private string GetFieldType(int index)
        {
            return vals[index].Item2[1];
        }

        private string GetDbFieldName(int index)
        {
            return vals[index].Item2[2];
        }

        private string GetDataType(int index)
        {
            return vals[index].Item2[3];
        }

        private string GetControlType(int index)
        {
            return vals[index].Item2[4];
        }

        private string GetDescr(int index)
        {
            return vals[index].Item2[5];
        }

        private string GetUnits(int index)
        {
            return vals[index].Item2[6];
        }

        private string GetRule(int index)
        {
            return vals[index].Item2[7];
        }

        private string GetOrder(int index)
        {
            return vals[index].Item2[8];
        }

        private string GetPossibleValues(int index)
        {
            return vals[index].Item2[9];
        }
    }
}
