using System.Collections.Generic;

namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreDatasetChangesForColette : DbMigration
    {
        private static readonly List<Tuple<string, List<string>>> vals = new List<Tuple<string, List<string>>> { 
                             
           //     Dataset                             Name                  H/D   dbName                dbType     CtrlType    Descr                                                                          Rule                     Order          PossibleValues               
           new Tuple<string, List<string>>                                                           
/* 0 */        ("Fish Scales", new List<string>     { "'Bad Scale'",        DET,  "'BadScale'",       "'string'", "'select'", "'Is scale bad?'",                                                       @"'{""DefaultValue"":""NO""}'", "155", @"'{""NO"":""No"",""YES"":""Yes""}'" } ),
           new Tuple<string, List<string>>                                                                                                                                                             
/* 1 */        ("Screw Trap",  new List<string>     { "'Unit'",             HEAD, "'Unit'",           "'string'", "'text'",   "'Number or name of the unit being sampled within the location'",              "NULL",                    "54",             "NULL" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                                            
/* 2 */        ("Electrofishing",  new List<string> { "'Visit ID'",         HEAD, "'VisitID'",        "'string'", "'text'",   "'Champ visit id associated with the sample'",                                  "NULL",                   "53",             "NULL" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                                            
/* 3 */        ("Electrofishing",  new List<string> { "'Unit'",             HEAD, "'Unit'",           "'string'", "'text'",   "'Number or name of the unit being sampled within the location'",               "NULL",                   "56",             "NULL" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                              
/* 4 */        ("Electrofishing",  new List<string> { "'Disposition'" }), // Delete only!
           new Tuple<string, List<string>>                                                                                                                                                                                              
/* 5 */        ("Electrofishing",  new List<string> { "'Release Location'", HEAD,"'ReleaseLocation'","'string'",   "'text'",   "'The spot where the fish were released after the activity'",                  "NULL",                   "51",             "NULL" } ),
           new Tuple<string, List<string>>                                                                                                                                                                                              
/* 6 */        ("Screw Trap",  new List<string>     { "'Task'" }),        // Delete only!
           new Tuple<string, List<string>>                                                                                                                                                                                                            
/* 7 */        ("Snorkel Fish", new List<string>    { "'Unit'",             HEAD, "'Unit'",           "'string'", "'text'",   "'Number or name of the unit being sampled within the location'",               "NULL",                   "45",             "NULL" } ),

        };


        public override void Up()
        {
            Sql(@"
-- Fish Scales: Change the Spawn check from a yes or no to a number
    update DatasetFields set controltype = 'number', [rule] = NULL                        where label = 'Spawn Check'
    update fields        set controltype = 'number', [rule] = NULL, PossibleValues = NULL where name  = 'Spawn Check'

-- Fish Scales: Add BadScale Attribute:  Yes or No default No 
" +
GetAddFieldSql(0) +

@"

-- Fish Scales: Change Fish Comment to Scale Comment
    update DatasetFields set label = 'Scale Comment', DbColumnName = 'ScaleComments' where fieldId in (select id from fields  where name = 'Fish Comments' and fieldcategoryid = (select id from fieldcategories where name = 'Fish Scales'))
    update fields        set name  = 'Scale Comment', DbColumnName = 'ScaleComments' where name = 'Fish Comments' and fieldcategoryid = (select id from fieldcategories where name = 'Fish Scales')


-- Spawning Ground: Tag needs to be multiselect
    update DatasetFields set controlType = 'multiselect' where fieldId in (select id from fields  where name = 'Tag' and fieldcategoryid = (select id from fieldcategories where name = 'Spawning Ground Survey'))
    update fields        set controlType = 'multiselect' where name = 'Tag' and fieldcategoryid = (select id from fieldcategories where name = 'Spawning Ground Survey')

-- Screw Trap and Electrofishing Header Add Unit: text field    " + 

GetAddFieldSql(1) +

@"

-- Screw Trap and Electrofishing Details: Change Tag to Tag Status
    update DatasetFields set label = 'Tag Status', DbColumnName = 'TagStatus' where fieldId in (select id from fields  where name = 'Tag' and fieldcategoryid in (select id from fieldcategories where name in('Electrofishing', 'Screw Trap')))
    update fields        set name  = 'Tag Status', DbColumnName = 'TagStatus' where name = 'Tag' and fieldcategoryid in (select id from fieldcategories where name in('Electrofishing', 'Screw Trap'))


-- Screw Trap and Electrofishing Details: Change Clip to Clip Status
    update DatasetFields set label = 'Clip Status', DbColumnName = 'ClipStatus' where fieldId in (select id from fields  where name = 'Clip' and fieldcategoryid in (select id from fieldcategories where name in('Electrofishing', 'Screw Trap')))
    update fields        set name  = 'Clip Status', DbColumnName = 'ClipStatus' where name = 'Clip' and fieldcategoryid in (select id from fieldcategories where name in('Electrofishing', 'Screw Trap'))


-- Snorkel: Add Unknown to species, remove Steelhead/Rainbow trout (this is the same as O. Mykiss)
    update fields set PossibleValues = '{""O. MYKISS"":""O. mykiss"",""CHINOOK SALMON"":""Chinook salmon"",""BULL TROUT"":""Bull trout"",""REDSIDE SHINER"":""Redside shiner"",""NORTHERN PIKEMINNOW"":""Northern pikeminnow"",""MOUNTAIN WHITEFISH"":""Mountain whitefish"",""BRIDGELIP SUCKER"":""Bridgelip sucker"",""MOUNTAIN SUCKER"":""Mountain sucker"",""LARGESCALE SUCKER"":""Largescale sucker"",""LONGNOSE DACE"":""Longnose dace"",""SPECKLED DACE"":""Speckled dace"",""CARP"":""Carp"",""LAMPREY"":""Lamprey"",""CHISELMOUTH"":""Chiselmouth"",""COTTIDAE (SCUPLINS)"":""Cottidae (scuplins)"",""ICHTHILURIDAE (CATFISHES)"":""Ichthiluridae (catfishes)"",""CENTRARCHIDS (SUNFISHES)"":""Centrarchids (sunfishes)"" ,""UNKNOWN"":""Unknown""}'
            where name = 'Species' and fieldcategoryid in (select id from fieldcategories where name in('Snorkel Fish'))



-- Fish Scales: Remove Juvenile Age (they have agreed that Freshwater and Juvenile age are the same)
-- NOTE: No Down method for this... once it's gone, it's gone!
    delete from datasetfields where label = 'Juvenile Age'
    delete from fields where name = 'Juvenile Age'


-- Electro fishing: Add Single pass to event type
    update fields set PossibleValues = '[""Depletion"",""Mark"",""Recapture"",""Salvage"",""Single-pass"",""Other""]' where name = 'Event Type' 

-- Electro fishing: Add Visit ID to the header (Text field-Champ visit id associated with the sample (same as in Snorkeling dataset))
-- Electro fishing: Add Unit to header (text field-Number or name of the united being sampled within the location)
" +

GetAddFieldSql(2) +
GetAddFieldSql(3) +

@"

-- Electro fishing: Remove Disposition from details (it will be done in Conditional comments as they are for p3)
" +

GetRemoveFieldSql(4) +     // NO UNDO!!!

@"

-- Electro fishing: Release location  in details needs to be text not select and moved to header.  (we had this with the p3 header detail and then we deleted it)
" +
GetRemoveFieldSql(5) +     // NO UNDO!!!
GetAddFieldSql(5) +        // NO UNDO!!!

@"
                
-- Rotary Screw Trap: Remove Tasks from header
" +

GetRemoveFieldSql(6) +     // NO UNDO!!!


@"                
-- Snorkel: Add Unit to header (text field-Number or name of the united being sampled within the location)
" +

GetAddFieldSql(7) +

@"
--Done



");

        }
        
        public override void Down()
        {
            Sql(@"
    update fields        set controltype = 'select', [rule] = '{""DefaultValue"":""No""}', PossibleValues = '[""Yes"", ""No""]' where name  = 'Spawn Check'
    update DatasetFields set controltype = 'select', [rule] = '{""DefaultValue"":""No""}'                                       where label = 'Spawn Check'
" +
    
           GetRemoveFieldSql(0)

  + @"
    update DatasetFields set label = 'Fish Comments', DbColumnName = 'FishComments' where fieldId in (select id from fields  where name = 'Scale Comment' and fieldcategoryid = (select id from fieldcategories where name = 'Fish Scales'))
    update fields        set name  = 'Fish Comments', DbColumnName = 'FishComments' where name = 'Scale Comment' and fieldcategoryid = (select id from fieldcategories where name = 'Fish Scales')

    update DatasetFields set controlType = 'select' where fieldId in (select id from fields  where name = 'Tag' and fieldcategoryid = (select id from fieldcategories where name = 'Spawning Ground Survey'))
    update fields        set controlType = 'select' where name = 'Tag' and fieldcategoryid = (select id from fieldcategories where name = 'Spawning Ground Survey')
" +

           GetRemoveFieldSql(1)

  + @"
    update DatasetFields set label = 'Tag', DbColumnName = 'Tag' where fieldId in (select id from fields  where name = 'Tag Status' and fieldcategoryid in (select id from fieldcategories where name in('Electrofishing', 'Screw Trap')))
    update fields        set name  = 'Tag', DbColumnName = 'Tag' where name = 'Tag Status' and fieldcategoryid in (select id from fieldcategories where name in('Electrofishing', 'Screw Trap'))

    update DatasetFields set label = 'Clip', DbColumnName = 'Clip' where fieldId in (select id from fields  where name = 'Clip Status' and fieldcategoryid in (select id from fieldcategories where name in('Electrofishing', 'Screw Trap')))
    update fields        set name  = 'Clip', DbColumnName = 'Clip' where name = 'Clip Status' and fieldcategoryid in (select id from fieldcategories where name in('Electrofishing', 'Screw Trap'))

    update fields set PossibleValues = '{""O. MYKISS"":""O. mykiss"",""STEELHEAD/RAINBOW TROUT"":""Steelhead/rainbow trout"",""CHINOOK SALMON"":""Chinook salmon"",""BULL TROUT"":""Bull trout"",""REDSIDE SHINER"":""Redside shiner"",""NORTHERN PIKEMINNOW"":""Northern pikeminnow"",""MOUNTAIN WHITEFISH"":""Mountain whitefish"",""BRIDGELIP SUCKER"":""Bridgelip sucker"",""MOUNTAIN SUCKER"":""Mountain sucker"",""LARGESCALE SUCKER"":""Largescale sucker"",""LONGNOSE DACE"":""Longnose dace"",""SPECKLED DACE"":""Speckled dace"",""CARP"":""Carp"",""LAMPREY"":""Lamprey"",""CHISELMOUTH"":""Chiselmouth"",""COTTIDAE (SCUPLINS)"":""Cottidae (scuplins)"",""ICHTHILURIDAE (CATFISHES)"":""Ichthiluridae (catfishes)"",""CENTRARCHIDS (SUNFISHES)"":""Centrarchids (sunfishes)""}' 
            where name = 'Species' and fieldcategoryid in (select id from fieldcategories where name in('Snorkel Fish'))

    update fields set PossibleValues = '[""Depletion"",""Mark"",""Recapture"",""Salvage"",""Other""]' where name = 'Event Type' 

" +

           GetRemoveFieldSql(2) +
           GetRemoveFieldSql(3) +


           GetRemoveFieldSql(7) +

@"



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
