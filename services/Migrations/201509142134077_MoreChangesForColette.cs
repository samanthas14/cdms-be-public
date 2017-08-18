using System.Collections.Generic;

namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreChangesForColette : DbMigration
    {
        private static readonly List<Tuple<string, List<string>>> vals = new List<Tuple<string, List<string>>> { 
                             
           //     Dataset                             Name            H/D   dbName                dbType     CtrlType    Descr         Rule       Order    PossibleValues               
           new Tuple<string, List<string>>                                                     
/* 0 */        ("Electrofishing", new List<string>  { "'End Time'",   HEAD,  "'EndTime'",       "'time'",   "'time'",  "'End time'",   "NULL",     "45",       "NULL" } ),


           new Tuple<string, List<string>>                                                                                                                                                                                              
/* 1 */        ("Electrofishing",  new List<string> { "'Tagger'" }),         // Delete only!
           new Tuple<string, List<string>>                                                                                                                                                                                              
/* 2 */        ("Electrofishing",  new List<string> { "'Capture Method'" }), // Delete only!
           new Tuple<string, List<string>>                                                                                                                                                                                              
/* 3 */        ("Electrofishing",  new List<string> { "'Migratory Year'" }), // Delete only!
           new Tuple<string, List<string>>                                                                                                                                                                                              
/* 4 */        ("Electrofishing",  new List<string> { "'Tagging Method'" }), // Delete only!
           new Tuple<string, List<string>>                                                                                                                                                                                              
/* 5 */        ("Electrofishing",  new List<string> { "'Organization'" }),   // Delete only!
           new Tuple<string, List<string>>                                                                                                                                                                                              
/* 6 */        ("Electrofishing",  new List<string> { "'Coordinator ID'" }), // Delete only!

        };


        public override void Up()
        {
            Sql(@"
-- Change Tag Date Time to Start Time (Don’t need the date part)
    update DatasetFields set label = 'Start Time', dbcolumnName = 'StartTime', controltype = 'time' where fieldid in(
                                                                              select id from fields where name = 'Tag DateTime' and fieldcategoryid = (select id from fieldcategories where name = 'Electrofishing'))
    update fields        set name  = 'Start Time', dbcolumnName = 'StartTime', controltype = 'time' where name = 'Tag DateTime' and fieldcategoryid = (select id from fieldcategories where name = 'Electrofishing')

-- Add End Time" +
GetAddFieldSql(0) +
@"

-- Change Release Date Time to Release Time (Don’t Need the date part)
    update DatasetFields set label = 'Release Time', dbcolumnName = 'ReleaseDateTime', controltype = 'time' where fieldid in(
                                                                select id from fields where name = 'Release DateTime' and fieldcategoryid = (select id from fieldcategories where name = 'Electrofishing'))
    update fields        set name  = 'Release Time', dbcolumnName = 'ReleaseDateTime', controltype = 'time' where name = 'Release DateTime' and fieldcategoryid = (select id from fieldcategories where name = 'Electrofishing')



-- Change Tagging Temp to Start Temp (C)
    update DatasetFields set label = 'Start Temp', dbcolumnName = 'StartTemp' where fieldid in(
                                                        select id from fields where name = 'Tagging Temp' and fieldcategoryid = (select id from fieldcategories where name = 'Electrofishing'))
    update fields        set name  = 'Start Temp', dbcolumnName = 'StartTemp' where name = 'Tagging Temp' and fieldcategoryid = (select id from fieldcategories where name = 'Electrofishing')



-- Remove these P3 categories they decided they don’t need them
--     Tagger
--     Capture Method
--     Migratory Year
--     Tagging Method
--     Organization
--     Coordinator ID

-- NO UNDO!!!!!

" +

GetRemoveFieldSql(1) +
GetRemoveFieldSql(2) +
GetRemoveFieldSql(3) +
GetRemoveFieldSql(4) +
GetRemoveFieldSql(5) +
GetRemoveFieldSql(6) 


);

        }

        public override void Down()
        {
            Sql(@"
    update DatasetFields set label = 'Tag DateTime', dbcolumnName = 'TagDateTime', controltype = 'date' where fieldid in(
                                                         select id from fields    where name = 'Start Time' and fieldcategoryid = (select id from fieldcategories where name = 'Electrofishing'))
    update fields        set name  = 'Tag DateTime', dbcolumnName = 'TagDateTime', controltype = 'date'  where name = 'Start Time' and fieldcategoryid = (select id from fieldcategories where name = 'Electrofishing')
" +
GetRemoveFieldSql(0) +
@"

    update DatasetFields set label = 'Release DateTime', dbcolumnName = 'ReleaseDateTime', controltype = 'date' where fieldid in(
                                                             select id from fields        where name = 'Release Time' and fieldcategoryid = (select id from fieldcategories where name = 'Electrofishing'))
    update fields        set name  = 'Release DateTime', dbcolumnName = 'ReleaseDateTime', controltype = 'date' where name = 'Release Time' and fieldcategoryid = (select id from fieldcategories where name = 'Electrofishing')



    update DatasetFields set label = 'Tagging Temp', dbcolumnName = 'TaggingTemp' where fieldid in(
                                                     select id from fields        where name = 'Start Temp' and fieldcategoryid = (select id from fieldcategories where name = 'Electrofishing'))
    update fields        set name  = 'Tagging Temp', dbcolumnName = 'TaggingTemp' where name = 'Start Temp' and fieldcategoryid = (select id from fieldcategories where name = 'Electrofishing')


");
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
            Units = NULL,
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

        private string GetRule(int index)
        {
            return vals[index].Item2[6];
        }

        private string GetOrder(int index)
        {
            return vals[index].Item2[7];
        }

        private string GetPossibleValues(int index)
        {
            return vals[index].Item2[8];
        }

    }
}
