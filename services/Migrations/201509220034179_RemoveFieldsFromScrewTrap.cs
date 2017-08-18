using System.Collections.Generic;

namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFieldsFromScrewTrap : DbMigration
    {
        private static readonly List<Tuple<string, List<string>>> vals = new List<Tuple<string, List<string>>> { 

           // select * from FieldCategories
           //     FieldCategory                         Name                              H/D     dbName                         dbType       CtrlType         Descr                                 Units   Rule                                                                                                                                                                                                                                                                                                                                                                                                                                      Order    PossibleValues               
           new Tuple<string, List<string>>                                                      

        /* 0 */   ("Snorkel Fish", new List<string> { "'Additional Positional Comments'", DET, "'AdditionalPositionalComments'", "'string'", "'text'", "'Additional comments about the position'", "NULL", "NULL", "465", "NULL" } ),
        };

        public override void Up()
        {
            AddColumn("dbo.ScrewTrap_Detail", "AdditionalPositionalComments", c => c.String());
            DropColumn("dbo.ScrewTrap_Header", "TagDateTime");
            DropColumn("dbo.ScrewTrap_Header", "ReleaseDateTime");
            DropColumn("dbo.ScrewTrap_Header", "CaptureMethod");
            DropColumn("dbo.ScrewTrap_Header", "MigratoryYear");
            DropColumn("dbo.ScrewTrap_Header", "TaggingMethod");
            DropColumn("dbo.ScrewTrap_Header", "Organization");
            DropColumn("dbo.ScrewTrap_Header", "CoordinatorID");
            DropColumn("dbo.ScrewTrap_Header", "ReleaseSite");
            DropColumn("dbo.ScrewTrap_Header", "ReleaseRiverKM");

            Sql(@"
-- NO UNDO
delete from datasetfields where fieldid in (select id from fields where name in( 'capture method', 'tagging method', 'Organization', 'Coordinator ID',
                                                                                 'Migratory Year','Tag DateTime','Release DateTime','Release Site','Release River Km')
   and FieldCategoryId = (select id from fieldcategories where name = 'screw trap'))

delete from fields where name in( 'capture method', 'tagging method', 'Organization', 'Coordinator ID','Migratory Year','Tag DateTime','Release DateTime','Release Site','Release River Km')
   and FieldCategoryId = (select id from fieldcategories where name = 'screw trap')

");
            Sql(GetAddFieldSql(0));
        }

        public override void Down()
        {
            AddColumn("dbo.ScrewTrap_Header", "ReleaseRiverKM", c => c.String());
            AddColumn("dbo.ScrewTrap_Header", "ReleaseSite", c => c.String());
            AddColumn("dbo.ScrewTrap_Header", "CoordinatorID", c => c.String());
            AddColumn("dbo.ScrewTrap_Header", "Organization", c => c.String());
            AddColumn("dbo.ScrewTrap_Header", "TaggingMethod", c => c.String());
            AddColumn("dbo.ScrewTrap_Header", "MigratoryYear", c => c.Int());
            AddColumn("dbo.ScrewTrap_Header", "CaptureMethod", c => c.String());
            AddColumn("dbo.ScrewTrap_Header", "ReleaseDateTime", c => c.DateTime());
            AddColumn("dbo.ScrewTrap_Header", "TagDateTime", c => c.DateTime());
            DropColumn("dbo.ScrewTrap_Detail", "AdditionalPositionalComments");

            Sql(GetRemoveFieldSql(0));
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
