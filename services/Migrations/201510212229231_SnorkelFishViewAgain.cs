using System.Collections.Generic;

namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SnorkelFishViewAgain : DbMigration
    {

        private static readonly List<Tuple<string, List<string>>> vals = new List<Tuple<string, List<string>>> { 

           // select * from FieldCategories
           //     FieldCategory                         Name                              H/D     dbName                         dbType       CtrlType         Descr                                 Units   Rule                                                                                                                                                                                                                                                                                                                                                                                                                                      Order    PossibleValues               
           new Tuple<string, List<string>>                                                      
        /* 0 */   ("Snorkel Fish", new List<string> { "'Additional Positional Comments'", DET, "'AdditionalPositionalComments'", "'string'", "'text'", "'Additional comments about the position'", "NULL", "NULL", "465", "NULL" } ),
            new Tuple<string, List<string>>                                                      
       /* 1 */   ("Screw Trap", new List<string> { "'Additional Positional Comments'", DET, "'AdditionalPositionalComments'", "'string'", "'text'", "'Additional comments about the position'", "NULL", "NULL", "465", "NULL" } ),
           new Tuple<string, List<string>>                                                      
        /* 2 */   ("Snorkel Fish", new List<string> { "'AEM Length'",       HEAD, "'AEMLength'",        "'string'", "'text'", "'Average size of the group of fish species observed in the current unit (deliniated every 10 cm)'",  "NULL", "NULL", "130", @"NULL" } ),
            new Tuple<string, List<string>>                                                      
       /* 3 */    ("Snorkel Fish", new List<string> { "'AEM Length'",       DET, "'AEMLength'",        "'string'", "'text'", "'Average size of the group of fish species observed in the current unit (deliniated every 10 cm)'",  "NULL", "NULL", "130", @"NULL" } ),
        };


        
        public override void Up()
        {
            // Move field from Snorkel to ScrewTrap
            Sql(GetRemoveFieldSql(0)); 
            Sql(GetAddFieldSql(1));

            // Move field from header to detail
            Sql(GetRemoveFieldSql(2));
            Sql(GetAddFieldSql(3));


            Sql(@"


drop view SnorkelFish_vw
go
drop view SnorkelFish_Detail_VW
go
drop view SnorkelFish_Header_VW
go

CREATE VIEW [dbo].[SnorkelFish_Detail_VW]
AS
SELECT        *
FROM            dbo.SnorkelFish_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SnorkelFish_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId)))
go


CREATE VIEW [dbo].[SnorkelFish_Header_VW]
AS
SELECT        *
FROM            dbo.SnorkelFish_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SnorkelFish_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)));
go



create view SnorkelFish_vw as
select
    h.Team, h.NoteTaker, h.StartTime, h.EndTime, h.WaterTemperature, h.Visibility, h.WeatherConditions, h.VisitId, h.Comments, h.CollectionType, 
    h.DominantSpecies, h.CommonSpecies, h.RareSpecies, h.Unit, iif(h.IsAEM='YES',AEMHabitatType,ChannelUnitType) as UnitType, h.IsAEM,

    d.NoSnorklers, d.FishID, d.ChannelUnitNumber, d.Lane, d.Type, d.ChannelAverageDepth, d.ChannelLength, 
    d.ChannelWidth, d.ChannelMaxDepth, d.ChannelLength *  d.ChannelWidth as ChannelArea, d.FishCount, d.Species, d.SizeClass, 
    d.UnidentifiedSalmonID, d.OtherSpeciesPres, d.NaturalWoodUsed, 
    d.PlacedWoodUsed, d.NaturalBoulderUsed, d.PlacedBoulderUsed, d.NaturalOffChannelUsed, d.CreatedOffChannelUsed, d.NewSideChannelUsed, 
    d.NoStructureUsed, d.AmbientTemp, d.MinimumTemp, d.FieldNotes, d.ChannelUnitType, d.AEMHabitatType, d.AEMLength,
    d.RowId, d.RowStatusId, d.QAStatusId, 

    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate,  a.CreateDate,

    w.id as WaterbodyId, w.name as WaterbodyName, 

    l.id as LocationId, l.name as LocationLabel,

    aq.QAStatusName as QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId  -- required


from SnorkelFish_Detail_vw d 
join SnorkelFish_Header_vw h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId
join locations l on l.id = a.locationid
join waterbodies w on w.id = l.waterbodyid
join ActivityQAs_VW AS aq ON aq.ActivityId = a.Id


go

");
        }
        
        public override void Down()
        {
            Sql(GetRemoveFieldSql(1));
            Sql(GetAddFieldSql(0));

            // Undo move field from header to detail
            Sql(GetRemoveFieldSql(3));
            Sql(GetAddFieldSql(2));


            Sql(@"
drop view SnorkelFish_vw
go
drop view SnorkelFish_Detail_VW
go

CREATE VIEW [dbo].[SnorkelFish_Detail_VW]
AS
SELECT        *
FROM            dbo.SnorkelFish_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SnorkelFish_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId)))
go


create view SnorkelFish_vw as
select
    h.Team, h.NoteTaker, h.StartTime, h.EndTime, h.WaterTemperature, h.Visibility, h.WeatherConditions, h.VisitId, h.Comments, h.CollectionType, 
    h.DominantSpecies, h.CommonSpecies, h.RareSpecies, h.Unit, iif(h.IsAEM='YES',AEMHabitatType,ChannelUnitType) as UnitType,

    d.NoSnorklers, d.FishID, d.ChannelUnitNumber, d.Lane, d.Type, d.ChannelAverageDepth, d.ChannelLength, 
    d.ChannelWidth, d.ChannelMaxDepth, d.ChannelLength *  d.ChannelWidth as ChannelArea, d.FishCount, d.Species, d.SizeClass, 
    d.UnidentifiedSalmonID, d.OtherSpeciesPres, d.NaturalWoodUsed, 
    d.PlacedWoodUsed, d.NaturalBoulderUsed, d.PlacedBoulderUsed, d.NaturalOffChannelUsed, d.CreatedOffChannelUsed, d.NewSideChannelUsed, 
    d.NoStructureUsed, d.AmbientTemp, d.MinimumTemp, d.FieldNotes, d.ChannelUnitType, d.AEMHabitatType, d.AEMLength,

    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, w.id as WaterbodyId, w.name as WaterbodyName, l.id as LocationId, 

    l.name as LocationLabel

from SnorkelFish_Detail_vw d 
join SnorkelFish_Header_vw h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId
join locations l on l.id = a.locationid
join waterbodies w on w.id = l.waterbodyid

go

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