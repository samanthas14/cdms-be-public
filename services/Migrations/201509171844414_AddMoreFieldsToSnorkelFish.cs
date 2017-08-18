using System.Collections.Generic;

namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreFieldsToSnorkelFish : DbMigration
    {
        private static readonly List<Tuple<string, List<string>>> vals = new List<Tuple<string, List<string>>> { 
                             
           // select * from FieldCategories
           //     FieldCategory                         Name                  H/D     dbName                  dbType       CtrlType         Descr                                 Units   Rule                                                                                                                                                                                                                                                                                                                                                                                                                                      Order    PossibleValues               
           new Tuple<string, List<string>>                                                      
/* 0 */        ("Snorkel Fish", new List<string> { "'Note Taker'",            HEAD, "'NoteTaker'",            "'string'", "'text'",        "'Person filling out the field sheet'", "NULL", "NULL",                                                                                                                                                                                                                                                                                                                                                                                                                                  "35", "NULL" } ),
           new Tuple<string, List<string>>                  
/* 1 */        ("Snorkel Fish", new List<string> { "'Start Time'",            HEAD, "'StartTime'",            "'string'", "'time'",        "'Time the survey started'", "NULL", "NULL",                                                                                                                                                                                                                                                                                                                                                                                                                                             "25", "NULL" } ),
           new Tuple<string, List<string>>                  
/* 2 */        ("Snorkel Fish", new List<string> { "'End Time'",              HEAD, "'EndTime'",              "'string'", "'time'",        "'Time the survey ended'", "NULL", "NULL",                                                                                                                                                                                                                                                                                                                                                                                                                                               "26", "NULL" } ),
           new Tuple<string, List<string>>                  
/* 3 */        ("Snorkel Fish", new List<string> { "'Weather Conditions'",    HEAD, "'WeatherConditions'",    "'string'", "'text'",        "'What was the weather like at the time of the survey'", "NULL", "NULL",                                                                                                                                                                                                                                                                                                                                                                                                                 "45", "NULL" } ),
           new Tuple<string, List<string>>                  
/* 4 */        ("Snorkel Fish", new List<string> { "'No. Snorklers'",         DET,  "'NoSnorklers'",          "'int'",    "'number'",      "'Number of snorkelers in the water for the survey of the channel unit'", "NULL", "NULL",                                                                                                                                                                                                                                                                                                                                                                                                "95 ", "NULL" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
/* 5 */        ("Snorkel Fish", new List<string> { "'Channel Unit Type'",     DET,  "'ChannelUnitType'",      "'string'", "'select'",      "'Channel unit types: Fastwater non-turbulent (FNT); Riffle (RI); Cascade (CA); Rapid (RA); Falls and Steps (FA); Scour pool (SP); Plunge Pool (PP); Dam Pool (DP); Beaver Pool (B)'", "NULL", "NULL",                                                                                                                                                                                                                                                                                   "85 ", @"'[""FNT"", ""RI"", ""CA"", ""RA"", ""FA"", ""SP"", ""PP""]'" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
/* 6 */        ("Snorkel Fish", new List<string> { "'AEM Habitat Type'",      DET,  "'AEMHabitatType'",       "'string'", "'select'",      "'Habitat Notes: Fast = fish is actively swimming to maintain position; Slow = fish is easily maintaining position without much effort; Edge = BFW > 25m, velocity < 0.15 m/s, depth < 0.61m; Backwater = A naturally or artificially formed arm or area of standing or slow moving water partially isolated from the flow of the main channel of a stream.'", "NULL", "NULL",                                                                                                           "105", @"'{""FAST"":""Fast"" ""SLOW"":""Slow"" ""EDGE"":""Edge"" ""BACKWATER"":""Backwater""}'" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
/* 7 */        ("Snorkel Fish", new List<string> { "'Channel Average Depth'", DET,  "'ChannelAverageDepth'",  "'double'", "'number'",      "'Average Depth of the unit'", "'m'", "NULL",                                                                                                                                                                                                                                                                                                                                                                                                                                            "144", "NULL" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
/* 8 */        ("Snorkel Fish", new List<string> { "'Channel Length'",        DET,  "'ChannelLength'",        "'double'", "'number'",      "'Length of the Channel Unit'", "'m'", "NULL",                                                                                                                                                                                                                                                                                                                                                                                                                                           "145", "NULL" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
/* 9 */       ("Snorkel Fish", new List<string>  { "'Channel Width'",         DET,  "'ChannelWidth'",         "'double'", "'number'",      "'Width of the Channel Unit'", "'m'", "NULL",                                                                                                                                                                                                                                                                                                                                                                                                                                            "146", "NULL" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
/* 10 */       ("Snorkel Fish", new List<string> { "'Channel Max Depth'",     DET,  "'ChannelMaxDepth'",      "'double'", "'number'",      "'Maximum depth of the channel unit'", "'m'", "NULL",                                                                                                                                                                                                                                                                                                                                                                                                                                    "147", "NULL" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
/* 11 */       ("Snorkel Fish", new List<string> { "'AEM Length'",            DET,  "'AEMLength'",             "'int'",   "'number'",      "'AEM average size of the group of fish species observed in the current unit (deliniated every 10 cm)'", "NULL", "NULL",                                                                                                                                                                                                                                                                                                                                                                 "93 ", "NULL" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
/* 12 */       ("Snorkel Fish", new List<string> { "'Unidentified Salmonid'", DET,  "'UnidentifiedSalmonid'", "'string'", "'select'",      "'Where there any unidentified salmonids young of the year (YOY) in the channel'", "NULL", "NULL",                                                                                                                                                                                                                                                                                                                                                                                      "133", @"'{""YES"":""Yes"",""NO"":""No""}'" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
/* 13 */       ("Snorkel Fish", new List<string> { "'Other Species Pres'",    DET,  "'OtherSpeciesPres'",     "'string'", "'multiselect'", "'Other fish species that may be present in the snorkel channel.  steelhead/rainbow trout (STS), Chinook salmon (CH), bull trout (BT), redside shiner (RS), northern pikeminnow (NPM), mountain whitefish (MWF), bridgelip sucker (BS), mountain sucker (MS), largescale sucker (LS), longnose dace (LD), speckled dace (SD), carp (CR), lamprey (PL), chiselmouth (CM), Cottidae (scuplins) (CT), Ichthiluridae (catfishes) (IC), and Centrarchids (sunfishes) (CN).'", "NULL", "NULL", "136", @"'[""STS"", ""CH"", ""BT"", ""RS"", ""NPM"", ""MWF"", ""BS"", ""MS"", ""LS"", ""LD"", ""SD"", ""CR"", ""PL"", ""CM"", ""Scuplins"", ""CT"", ""Catfishes"", ""IC"", ""Sunfishes"", ""CN""]'" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
/* 14 */       ("Snorkel Fish", new List<string> { "'Ambient Temp'",          DET,  "'AmbientTemp'",          "'double'", "'number'",      "'Ambient temperature at the time'", "'C'", "NULL",                                                                                                                                                                                                                                                                                                                                                                                                                                      "148", "NULL" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
/* 15 */       ("Snorkel Fish", new List<string> { "'Minimum Temp'",          DET,  "'MinimumTemp'",          "'double'", "'number'",      "'Minimum Temperature of the unit'", "NULL", "NULL",                                                                                                                                                                                                                                                                                                                                                                                                                                     "149", "NULL" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
/* 16 */       ("Snorkel Fish", new List<string> { "'Is AEM Site'",           HEAD,  "'IsAEM'",                "'string'", "'select'",      "'Is this an AEM site?'", "NULL", "NULL",                                                                                                                                                                                                                                                                                                                                                                                                                                               "48",  @"'{""YES"":""Yes"",""NO"":""No""}'" } ),

        };


        public override void Up()
        {
            for(int i = 0; i < 17; i++)
                Sql(GetAddFieldSql(i));
        }


        public override void Down()
        {
            for(int i = 0; i < 17; i++)
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
