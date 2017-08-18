using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;

namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SnorkelRefinement : DbMigration
    {
        private static readonly List<Tuple<string, List<string>>> vals = new List<Tuple<string, List<string>>> { 

           // select * from FieldCategories
           //     FieldCategory                         Name              H/D     dbName            dbType      CtrlType         Descr                                                                                             Units   Rule   Order     PossValues                                                                                                                                                                                                                                                                                                                                                                                                                                 Order    PossibleValues               
                                                                                                                                                                                                                                  
        // Add new columns                                                                                                                                                                                                        
           new Tuple<string, List<string>>                                                                                                                                                                                        
        /* 0 */   ("Snorkel Fish", new List<string> { "'Dominant Species'", HEAD, "'DominantSpecies'", "'string'", "'select'", "'For the overall survey what was the dominant species (>50%)'",                                     "NULL", "NULL", "186", @"'{""(STS)"":""Steelhead/rainbow trout"", ""(CH)"":""Chinook salmon"", ""(BT)"":""Bull trout"", ""(RS)"":""Redside shiner"", ""(NPM)"":""Northern pikeminnow"", ""(MWF)"":""Mountain whitefish"", ""(BS)"":""Bridgelip sucker"", ""(MS)"":""Mountain sucker"", ""(LS)"":""Largescale sucker"", ""(LD)"":""Longnose dace"", ""(SD)"":""Speckled dace"", ""(CR)"":""Carp"", ""(PL)"":""Lamprey"", ""(CM)"":""Chiselmouth"", ""(CT)"":""Cottidae (scuplins)"", ""(IC)"":""Ichthiluridae (catfishes)"", ""(CN)"":""And Centrarchids (sunfishes)""}'" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                        
        /* 1 */   ("Snorkel Fish", new List<string> { "'Common Species'",   HEAD, "'CommonSpecies'",   "'string'", "'select'", "'For the overall survey what was the common species (10-50%)'",                                     "NULL", "NULL", "187", @"'{""(STS)"":""Steelhead/rainbow trout"", ""(CH)"":""Chinook salmon"", ""(BT)"":""Bull trout"", ""(RS)"":""Redside shiner"", ""(NPM)"":""Northern pikeminnow"", ""(MWF)"":""Mountain whitefish"", ""(BS)"":""Bridgelip sucker"", ""(MS)"":""Mountain sucker"", ""(LS)"":""Largescale sucker"", ""(LD)"":""Longnose dace"", ""(SD)"":""Speckled dace"", ""(CR)"":""Carp"", ""(PL)"":""Lamprey"", ""(CM)"":""Chiselmouth"", ""(CT)"":""Cottidae (scuplins)"", ""(IC)"":""Ichthiluridae (catfishes)"", ""(CN)"":""And Centrarchids (sunfishes)""}'" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                        
        /* 2 */   ("Snorkel Fish", new List<string> { "'Rare Species'",     HEAD, "'RareSpecies'",     "'string'", "'select'", "'For the overall survey what was the most rare species (<10%)'",                                    "NULL", "NULL", "188", @"'{""(STS)"":""Steelhead/rainbow trout"", ""(CH)"":""Chinook salmon"", ""(BT)"":""Bull trout"", ""(RS)"":""Redside shiner"", ""(NPM)"":""Northern pikeminnow"", ""(MWF)"":""Mountain whitefish"", ""(BS)"":""Bridgelip sucker"", ""(MS)"":""Mountain sucker"", ""(LS)"":""Largescale sucker"", ""(LD)"":""Longnose dace"", ""(SD)"":""Speckled dace"", ""(CR)"":""Carp"", ""(PL)"":""Lamprey"", ""(CM)"":""Chiselmouth"", ""(CT)"":""Cottidae (scuplins)"", ""(IC)"":""Ichthiluridae (catfishes)"", ""(CN)"":""And Centrarchids (sunfishes)""}'" } ),


        // Rename Size Class to AEMLength
           new Tuple<string, List<string>>                                                                                                                                                      
        /* 3 */   ("Snorkel Fish", new List<string> { "'SizeClass'",        HEAD, "'SizeClass'",        "'string'", "'text'", "'Average size of the group of fish species observed in the current unit (deliniated every 10 cm)'",  "NULL", "NULL", "130", @"NULL" } ),
           new Tuple<string, List<string>>                                                                                                                                                            
        /* 4 */   ("Snorkel Fish", new List<string> { "'AEM Length'",       HEAD, "'AEMLength'",        "'string'", "'text'", "'Average size of the group of fish species observed in the current unit (deliniated every 10 cm)'",  "NULL", "NULL", "130", @"NULL" } ),
                          
                                                                           
        // Remove these
           new Tuple<string, List<string>>                                                                                                                                                      
        /* 5 */   ("Snorkel Fish", new List<string> { "'ChannelUnit'",      DET,  "'ChannelUnit'",     "'string'", "'select'", "'Daily fin clips'",                                                                                  "NULL", "NULL", "185", @"'[""UC"", ""LC"", ""BC"", ""A"", ""AU"", ""AL"", ""AB"", ""D""]'" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                         
        /* 6 */   ("Snorkel Fish", new List<string> { "'HabitatType'",      DET,  "'HabitatType'",     "'string'", "'select'", "'Daily fin clips'",                                                                                  "NULL", "NULL", "185", @"'[""UC"", ""LC"", ""BC"", ""A"", ""AU"", ""AL"", ""AB"", ""D""]'" } ),
                        
                                                                                                                                                                                                           
        // Rename length to size class                                                                                                                                                                                             
           new Tuple<string, List<string>>                                                                                                                                                                                         
        /* 7 */   ("Snorkel Fish", new List<string> { "'Length'",           DET,  "'Length'",          "'string'", "'select'", "'Size classes for non AEM snorkel events Salmonids'",                                                "NULL", "NULL", "140", @"'[""BUT/STS <80"", ""BUT/STS 80-129"", ""BUT/STS 130-199"", ""BUT/STS 200+"", ""CH <100"", ""CH 100+"", ""Adult""]'" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                         

        /* 8 */   ("Snorkel Fish", new List<string> { "'Size Class'",       DET,  "'SizeClass'",       "'string'", "'select'", "'Size classes for non AEM snorkel events Salmonids'",                                                "NULL", "NULL", "140", @"'[""BUT/STS <80"", ""BUT/STS 80-129"", ""BUT/STS 130-199"", ""BUT/STS 200+"", ""CH <100"", ""CH 100+"", ""Adult""]'" } ),

        // Fix this one
           new Tuple<string, List<string>>                                                                                                                                                                                         
        /* 9 */   ("Snorkel Fish", new List<string> { "'AEM Habitat Type'",      DET,  "'AEMHabitatType'",  "'string'", "'select'",      "'Habitat Notes: Fast = fish is actively swimming to maintain position; Slow = fish is easily maintaining position without much effort; Edge = BFW > 25m, velocity < 0.15 m/s, depth < 0.61m; Backwater = A naturally or artificially formed arm or area of standing or slow moving water partially isolated from the flow of the main channel of a stream.'", "NULL", "NULL",                                                                                                           "105", @"'{""FAST"":""Fast"", ""SLOW"":""Slow"", ""EDGE"":""Edge"", ""BACKWATER"":""Backwater""}'" } ),
        };                                                                     




        public override void Up()
        {
            DropColumn("dbo.SnorkelFish_Detail", "ChannelUnit");
            DropColumn("dbo.SnorkelFish_Detail", "HabitatType");
            DropColumn("dbo.SnorkelFish_Detail", "Length");
            Sql(GetAddFieldSql(0));
            Sql(GetAddFieldSql(1));
            Sql(GetAddFieldSql(2));

            Sql(GetRemoveFieldSql(3));
            Sql(GetAddFieldSql(4));

            Sql(GetRemoveFieldSql(5));
            Sql(GetRemoveFieldSql(6));

            Sql(GetRemoveFieldSql(7));
            Sql(GetAddFieldSql(8));

            Sql(GetRemoveFieldSql(9));
            Sql(GetAddFieldSql(9));
        }
        
        public override void Down()
        {
            AddColumn("dbo.SnorkelFish_Detail", "Length", c => c.String());
            AddColumn("dbo.SnorkelFish_Detail", "HabitatType", c => c.String());
            AddColumn("dbo.SnorkelFish_Detail", "ChannelUnit", c => c.String());


            Sql(GetRemoveFieldSql(0));
            Sql(GetRemoveFieldSql(1));
            Sql(GetRemoveFieldSql(2));

            Sql(GetAddFieldSql(3));
            Sql(GetRemoveFieldSql(4));

            Sql(GetRemoveFieldSql(5));
            Sql(GetRemoveFieldSql(6));

            Sql(GetAddFieldSql(7));
            Sql(GetRemoveFieldSql(8));
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

