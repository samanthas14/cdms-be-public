using System.Collections.Generic;

namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTonsOfStuffToTheElectroFishingHeaders : DbMigration
    {
        private static readonly List<Tuple<string, List<string>>> vals = new List<Tuple<string, List<string>>> { 

           // select * from FieldCategories
           //     FieldCategory                         Name             H/D     dbName            dbType      CtrlType         Descr          Units   Rule   Order     PossValues                                                                                                                                                                                                                                                                                                                                                                                                                                 Order    PossibleValues               
           new Tuple<string, List<string>>
            /* 0 */   ("Electrofishing", new List<string> { "'Time Begin'", HEAD, "'TimeBegin'", "''", "''", "''", "''", "NULL", "", "''" } ),
            new Tuple<string, List<string>>
            /* 1 */   ("Electrofishing", new List<string> { "'Time End'", HEAD, "'TimeEnd'", "''", "''", "''", "''", "NULL", "", "''" } ),
            new Tuple<string, List<string>>
            /* 2 */   ("Electrofishing", new List<string> { "'Total Seconds EF'", HEAD, "'TotalSecondsEF'", "''", "''", "''", "''", "NULL", "", "''" } ),
            new Tuple<string, List<string>>
            /* 3 */   ("Electrofishing", new List<string> { "'Water Temp Begin'", HEAD, "'WaterTempBegin'", "''", "''", "''", "''", "NULL", "", "''" } ),
            new Tuple<string, List<string>>
            /* 4 */   ("Electrofishing", new List<string> { "'Water Temp Stop'", HEAD, "'WaterTempStop'", "''", "''", "''", "''", "NULL", "", "''" } ),
            new Tuple<string, List<string>>
            /* 5 */   ("Electrofishing", new List<string> { "'Hertz'", HEAD, "'Hertz'", "''", "''", "''", "''", "NULL", "", "''" } ),
            new Tuple<string, List<string>>
            /* 6 */   ("Electrofishing", new List<string> { "'Freq'", HEAD, "'Freq'", "''", "''", "''", "''", "NULL", "", "''" } ),
            new Tuple<string, List<string>>
            /* 7 */   ("Electrofishing", new List<string> { "'Volts'", HEAD, "'Volts'", "''", "''", "''", "''", "NULL", "", "''" } ),

            //// New Fields:

            new Tuple<string, List<string>>
            /* 8 */   ("Electrofishing", new List<string> { "'P1 Time Begin'", HEAD, "'Pass1TimeBegin'", "'DateTime'", "'time'", "'Time started shocking'", "NULL", "NULL", "280", "NULL" } ),
            new Tuple<string, List<string>>
            /* 9 */   ("Electrofishing", new List<string> { "'P1 Time End'", HEAD, "'Pass1TimeEnd'", "'DateTime'", "'time'", "'Time stopped shocking'", "NULL", "NULL", "281", "NULL" } ),
            new Tuple<string, List<string>>
            /* 10 */   ("Electrofishing", new List<string> { "'P1 Total EF'", HEAD, "'Pass1TotalSecondsEF'", "'double'", "'number'", "'Shocking time'", "'sec'", "NULL", "282", "NULL" } ),
            new Tuple<string, List<string>>
            /* 11 */   ("Electrofishing", new List<string> { "'P1 Water Temp Begin'", HEAD, "'Pass1WaterTempBegin'", "'double'", "'number'", "'Water temp at beginning'", "'C'", "NULL", "283", "NULL" } ),
            new Tuple<string, List<string>>
            /* 12 */   ("Electrofishing", new List<string> { "'P1 Water Temp Stop'", HEAD, "'Pass1WaterTempStop'", "'double'", "'number'", "'Water temp at end'", "'C'", "NULL", "284", "NULL" } ),
            new Tuple<string, List<string>>
            /* 13 */   ("Electrofishing", new List<string> { "'P1 Hertz'", HEAD, "'Pass1Hertz'", "'double'", "'number'", "'Total number of Hertz the unit is producing'", "'Hz'", "NULL", "285", "NULL" } ),
            new Tuple<string, List<string>>
            /* 14 */   ("Electrofishing", new List<string> { "'P1 Freq'", HEAD, "'Pass1Freq'", "'double'", "'number'", "'Frequency that that the shocker is using'", "'Hz'", "NULL", "286", "NULL" } ),
            new Tuple<string, List<string>>
            /* 15 */   ("Electrofishing", new List<string> { "'P1 Volts'", HEAD, "'Pass1Volts'", "'double'", "'number'", "'Number of volts the shocker is using'", "'V'", "NULL", "287", "NULL" } ),
            new Tuple<string, List<string>>
            /* 16 */   ("Electrofishing", new List<string> { "'P2 Time Begin'", HEAD, "'Pass2TimeBegin'", "'DateTime'", "'time'", "'Time started shocking'", "NULL", "NULL", "288", "NULL" } ),
            new Tuple<string, List<string>>
            /* 17 */   ("Electrofishing", new List<string> { "'P2 Time End'", HEAD, "'Pass2TimeEnd'", "'DateTime'", "'time'", "'Time stopped shocking'", "NULL", "NULL", "289", "NULL" } ),
            new Tuple<string, List<string>>
            /* 18 */   ("Electrofishing", new List<string> { "'P2 Total EF'", HEAD, "'Pass2TotalSecondsEF'", "'double'", "'number'", "'Shocking time'", "'sec'", "NULL", "290", "NULL" } ),
            new Tuple<string, List<string>>
            /* 19 */   ("Electrofishing", new List<string> { "'P2 Water Temp Begin'", HEAD, "'Pass2WaterTempBegin'", "'double'", "'number'", "'Water temp at beginning'", "'C'", "NULL", "291", "NULL" } ),
            new Tuple<string, List<string>>
            /* 20 */   ("Electrofishing", new List<string> { "'P2 Water Temp Stop'", HEAD, "'Pass2WaterTempStop'", "'double'", "'number'", "'Water temp at end'", "'C'", "NULL", "292", "NULL" } ),
            new Tuple<string, List<string>>
            /* 21 */   ("Electrofishing", new List<string> { "'P2 Hertz'", HEAD, "'Pass2Hertz'", "'double'", "'number'", "'Total number of Hertz the unit is producing'", "'Hz'", "NULL", "293", "NULL" } ),
            new Tuple<string, List<string>>
            /* 22 */   ("Electrofishing", new List<string> { "'P2 Freq'", HEAD, "'Pass2Freq'", "'double'", "'number'", "'Frequency that that the shocker is using'", "'Hz'", "NULL", "294", "NULL" } ),
            new Tuple<string, List<string>>
            /* 23 */   ("Electrofishing", new List<string> { "'P2 Volts'", HEAD, "'Pass2Volts'", "'double'", "'number'", "'Number of volts the shocker is using'", "'V'", "NULL", "295", "NULL" } ),
            new Tuple<string, List<string>>
            /* 24 */   ("Electrofishing", new List<string> { "'P3 Time Begin'", HEAD, "'Pass3TimeBegin'", "'DateTime'", "'time'", "'Time started shocking'", "NULL", "NULL", "296", "NULL" } ),
            new Tuple<string, List<string>>
            /* 25 */   ("Electrofishing", new List<string> { "'P3 Time End'", HEAD, "'Pass3TimeEnd'", "'DateTime'", "'time'", "'Time stopped shocking'", "NULL", "NULL", "297", "NULL" } ),
            new Tuple<string, List<string>>
            /* 26 */   ("Electrofishing", new List<string> { "'P3 Total EF'", HEAD, "'Pass3TotalSecondsEF'", "'double'", "'number'", "'Shocking time'", "'sec'", "NULL", "298", "NULL" } ),
            new Tuple<string, List<string>>
            /* 27 */   ("Electrofishing", new List<string> { "'P3 Water Temp Begin'", HEAD, "'Pass3WaterTempBegin'", "'double'", "'number'", "'Water temp at beginning'", "'C'", "NULL", "299", "NULL" } ),
            new Tuple<string, List<string>>
            /* 28 */   ("Electrofishing", new List<string> { "'P3 Water Temp Stop'", HEAD, "'Pass3WaterTempStop'", "'double'", "'number'", "'Water temp at end'", "'C'", "NULL", "300", "NULL" } ),
            new Tuple<string, List<string>>
            /* 29 */   ("Electrofishing", new List<string> { "'P3 Hertz'", HEAD, "'Pass3Hertz'", "'double'", "'number'", "'Total number of Hertz the unit is producing'", "'Hz'", "NULL", "301", "NULL" } ),
            new Tuple<string, List<string>>
            /* 30 */   ("Electrofishing", new List<string> { "'P3 Freq'", HEAD, "'Pass3Freq'", "'double'", "'number'", "'Frequency that that the shocker is using'", "'Hz'", "NULL", "302", "NULL" } ),
            new Tuple<string, List<string>>
            /* 31 */   ("Electrofishing", new List<string> { "'P3 Volts'", HEAD, "'Pass3Volts'", "'double'", "'number'", "'Number of volts the shocker is using'", "'V'", "NULL", "303", "NULL" } ),
            new Tuple<string, List<string>>
            /* 32 */   ("Electrofishing", new List<string> { "'P4 Time Begin'", HEAD, "'Pass4TimeBegin'", "'DateTime'", "'time'", "'Time started shocking'", "NULL", "NULL", "304", "NULL" } ),
            new Tuple<string, List<string>>
            /* 33 */   ("Electrofishing", new List<string> { "'P4 Time End'", HEAD, "'Pass4TimeEnd'", "'DateTime'", "'time'", "'Time stopped shocking'", "NULL", "NULL", "305", "NULL" } ),
            new Tuple<string, List<string>>
            /* 34 */   ("Electrofishing", new List<string> { "'P4 Total EF'", HEAD, "'Pass4TotalSecondsEF'", "'double'", "'number'", "'Shocking time'", "'sec'", "NULL", "306", "NULL" } ),
            new Tuple<string, List<string>>
            /* 35 */   ("Electrofishing", new List<string> { "'P4 Water Temp Begin'", HEAD, "'Pass4WaterTempBegin'", "'double'", "'number'", "'Water temp at beginning'", "'C'", "NULL", "307", "NULL" } ),
            new Tuple<string, List<string>>
            /* 36 */   ("Electrofishing", new List<string> { "'P4 Water Temp Stop'", HEAD, "'Pass4WaterTempStop'", "'double'", "'number'", "'Water temp at end'", "'C'", "NULL", "308", "NULL" } ),
            new Tuple<string, List<string>>
            /* 37 */   ("Electrofishing", new List<string> { "'P4 Hertz'", HEAD, "'Pass4Hertz'", "'double'", "'number'", "'Total number of Hertz the unit is producing'", "'Hz'", "NULL", "309", "NULL" } ),
            new Tuple<string, List<string>>
            /* 38 */   ("Electrofishing", new List<string> { "'P4 Freq'", HEAD, "'Pass4Freq'", "'double'", "'number'", "'Frequency that that the shocker is using'", "'Hz'", "NULL", "310", "NULL" } ),
            new Tuple<string, List<string>>
            /* 39 */   ("Electrofishing", new List<string> { "'P4 Volts'", HEAD, "'Pass4Volts'", "'double'", "'number'", "'Number of volts the shocker is using'", "'V'", "NULL", "311", "NULL" } ),
            new Tuple<string, List<string>>
            /* 40 */   ("Electrofishing", new List<string> { "'P5 Time Begin'", HEAD, "'Pass5TimeBegin'", "'DateTime'", "'time'", "'Time started shocking'", "NULL", "NULL", "312", "NULL" } ),
            new Tuple<string, List<string>>
            /* 41 */   ("Electrofishing", new List<string> { "'P5 Time End'", HEAD, "'Pass5TimeEnd'", "'DateTime'", "'time'", "'Time stopped shocking'", "NULL", "NULL", "313", "NULL" } ),
            new Tuple<string, List<string>>
            /* 42 */   ("Electrofishing", new List<string> { "'P5 Total EF'", HEAD, "'Pass5TotalSecondsEF'", "'double'", "'number'", "'Shocking time'", "'sec'", "NULL", "314", "NULL" } ),
            new Tuple<string, List<string>>
            /* 43 */   ("Electrofishing", new List<string> { "'P5 Water Temp Begin'", HEAD, "'Pass5WaterTempBegin'", "'double'", "'number'", "'Water temp at beginning'", "'C'", "NULL", "315", "NULL" } ),
            new Tuple<string, List<string>>
            /* 44 */   ("Electrofishing", new List<string> { "'P5 Water Temp Stop'", HEAD, "'Pass5WaterTempStop'", "'double'", "'number'", "'Water temp at end'", "'C'", "NULL", "316", "NULL" } ),
            new Tuple<string, List<string>>
            /* 45 */   ("Electrofishing", new List<string> { "'P5 Hertz'", HEAD, "'Pass5Hertz'", "'double'", "'number'", "'Total number of Hertz the unit is producing'", "'Hz'", "NULL", "317", "NULL" } ),
            new Tuple<string, List<string>>
            /* 46 */   ("Electrofishing", new List<string> { "'P5 Freq'", HEAD, "'Pass5Freq'", "'double'", "'number'", "'Frequency that that the shocker is using'", "'Hz'", "NULL", "318", "NULL" } ),
            new Tuple<string, List<string>>
            /* 47 */   ("Electrofishing", new List<string> { "'P5 Volts'", HEAD, "'Pass5Volts'", "'double'", "'number'", "'Number of volts the shocker is using'", "'V'", "NULL", "319", "NULL" } ),
            new Tuple<string, List<string>>
            /* 48 */   ("Electrofishing", new List<string> { "'P6 Time Begin'", HEAD, "'Pass6TimeBegin'", "'DateTime'", "'time'", "'Time started shocking'", "NULL", "NULL", "320", "NULL" } ),
            new Tuple<string, List<string>>
            /* 49 */   ("Electrofishing", new List<string> { "'P6 Time End'", HEAD, "'Pass6TimeEnd'", "'DateTime'", "'time'", "'Time stopped shocking'", "NULL", "NULL", "321", "NULL" } ),
            new Tuple<string, List<string>>
            /* 50 */   ("Electrofishing", new List<string> { "'P6 Total EF'", HEAD, "'Pass6TotalSecondsEF'", "'double'", "'number'", "'Shocking time'", "'sec'", "NULL", "322", "NULL" } ),
            new Tuple<string, List<string>>
            /* 51 */   ("Electrofishing", new List<string> { "'P6 Water Temp Begin'", HEAD, "'Pass6WaterTempBegin'", "'double'", "'number'", "'Water temp at beginning'", "'C'", "NULL", "323", "NULL" } ),
            new Tuple<string, List<string>>
            /* 52 */   ("Electrofishing", new List<string> { "'P6 Water Temp Stop'", HEAD, "'Pass6WaterTempStop'", "'double'", "'number'", "'Water temp at end'", "'C'", "NULL", "324", "NULL" } ),
            new Tuple<string, List<string>>
            /* 53 */   ("Electrofishing", new List<string> { "'P6 Hertz'", HEAD, "'Pass6Hertz'", "'double'", "'number'", "'Total number of Hertz the unit is producing'", "'Hz'", "NULL", "325", "NULL" } ),
            new Tuple<string, List<string>>
            /* 54 */   ("Electrofishing", new List<string> { "'P6 Freq'", HEAD, "'Pass6Freq'", "'double'", "'number'", "'Frequency that that the shocker is using'", "'Hz'", "NULL", "326", "NULL" } ),
            new Tuple<string, List<string>>
            /* 55 */   ("Electrofishing", new List<string> { "'P6 Volts'", HEAD, "'Pass6Volts'", "'double'", "'number'", "'Number of volts the shocker is using'", "'V'", "NULL", "327", "NULL" } ),

            // Delete only
            new Tuple<string, List<string>>
            /* 56 */   ("Electrofishing", new List<string> { "'Pass Number'", HEAD, "'PassNumber'", "''", "''", "''", "''", "NULL", "", "''" } ),
        };



        public override void Up()
        {
            for(int i = 0; i <= 7; i++)
                Sql(GetRemoveFieldSql(i));      // No undo

            for (int i = 8; i <= 55; i++)
                Sql(GetAddFieldSql(i));

            Sql(GetRemoveFieldSql(56));         // No undo
        }
        

        public override void Down()
        {
            for (int i = 8; i <= 55; i++)
                Sql(GetRemoveFieldSql(i));
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
