using System.Collections.Generic;

namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TryNewStreamNetStructure : DbMigration
    {
        private static readonly List<Tuple<string, List<string>>> vals = new List<Tuple<string, List<string>>> { 

           // select * from FieldCategories
           //     FieldCategory                         Name             H/D     dbName            dbType      CtrlType         Descr          Units   Rule   Order     PossValues                                                                                                                                                                                                                                                                                                                                                                                                                                 Order    PossibleValues               
           new Tuple<string, List<string>>
        /* 0 */   ("StreamNet_RperS", new List<string> { "'CommonName'", DET, "'CommonName'", "'string'", "'text'", "'CommonName'", "NULL", "NULL", "10", "NULL" } ),
            new Tuple<string, List<string>>
        /* 1 */   ("StreamNet_RperS", new List<string> { "'Run'", DET, "'Run'", "'string'", "'text'", "'Run'", "NULL", "NULL", "20", "NULL" } ),
            new Tuple<string, List<string>>
        /* 2 */   ("StreamNet_RperS", new List<string> { "'PopFit'", DET, "'PopFit'", "'string'", "'text'", "'PopFit'", "NULL", "NULL", "30", "NULL" } ),
            new Tuple<string, List<string>>
        /* 3 */   ("StreamNet_RperS", new List<string> { "'SpawnerLocation'", DET, "'SpawnerLocation'", "'string'", "'text'", "'SpawnerLocation'", "NULL", "NULL", "40", "NULL" } ),
            new Tuple<string, List<string>>
        /* 4 */   ("StreamNet_RperS", new List<string> { "'RecruitLocation'", DET, "'RecruitLocation'", "'string'", "'text'", "'RecruitLocation'", "NULL", "NULL", "50", "NULL" } ),
            new Tuple<string, List<string>>
        /* 5 */   ("StreamNet_RperS", new List<string> { "'BroodYear'", DET, "'BroodYear'", "'string'", "'text'", "'BroodYear'", "NULL", "NULL", "60", "NULL" } ),
            new Tuple<string, List<string>>
        /* 6 */   ("StreamNet_RperS", new List<string> { "'RperStype'", DET, "'RperStype'", "'string'", "'text'", "'RperStype'", "NULL", "NULL", "70", "NULL" } ),
            new Tuple<string, List<string>>
        /* 7 */   ("StreamNet_RperS", new List<string> { "'TRTmethod'", DET, "'TRTmethod'", "'string'", "'text'", "'TRTmethod'", "NULL", "NULL", "80", "NULL" } ),
            new Tuple<string, List<string>>
        /* 8 */   ("StreamNet_RperS", new List<string> { "'ContactAgency'", DET, "'ContactAgency'", "'string'", "'text'", "'ContactAgency'", "NULL", "NULL", "90", "NULL" } ),
            new Tuple<string, List<string>>
        /* 9 */   ("StreamNet_RperS", new List<string> { "'MethodNumber'", DET, "'MethodNumber'", "'string'", "'text'", "'MethodNumber'", "NULL", "NULL", "100", "NULL" } ),
            new Tuple<string, List<string>>
        /* 10 */   ("StreamNet_RperS", new List<string> { "'RperS'", DET, "'RperS'", "'string'", "'text'", "'RperS'", "NULL", "NULL", "110", "NULL" } ),
            new Tuple<string, List<string>>
        /* 11 */   ("StreamNet_RperS", new List<string> { "'Comments'", DET, "'Comments'", "'string'", "'text'", "'Comments'", "NULL", "NULL", "120", "NULL" } ),
            new Tuple<string, List<string>>
        /* 12 */   ("StreamNet_RperS", new List<string> { "'NullRecord'", DET, "'NullRecord'", "'string'", "'text'", "'NullRecord'", "NULL", "NULL", "130", "NULL" } ),
            new Tuple<string, List<string>>
        /* 13 */   ("StreamNet_RperS", new List<string> { "'DataStatus'", DET, "'DataStatus'", "'string'", "'text'", "'DataStatus'", "NULL", "NULL", "140", "NULL" } ),
            new Tuple<string, List<string>>
        /* 14 */   ("StreamNet_RperS", new List<string> { "'ContactPersonFirst'", DET, "'ContactPersonFirst'", "'string'", "'text'", "'ContactPersonFirst'", "NULL", "NULL", "150", "NULL" } ),
            new Tuple<string, List<string>>
        /* 15 */   ("StreamNet_RperS", new List<string> { "'ContactPersonLast'", DET, "'ContactPersonLast'", "'string'", "'text'", "'ContactPersonLast'", "NULL", "NULL", "160", "NULL" } ),
            new Tuple<string, List<string>>
        /* 16 */   ("StreamNet_RperS", new List<string> { "'ContactPhone'", DET, "'ContactPhone'", "'string'", "'text'", "'ContactPhone'", "NULL", "NULL", "170", "NULL" } ),
            new Tuple<string, List<string>>
        /* 17 */   ("StreamNet_RperS", new List<string> { "'ContactEmail'", DET, "'ContactEmail'", "'string'", "'text'", "'ContactEmail'", "NULL", "NULL", "180", "NULL" } ),
            new Tuple<string, List<string>>
        /* 18 */   ("StreamNet_RperS", new List<string> { "'LastModifiedOn'", DET, "'LastModifiedOn'", "'string'", "'text'", "'LastModifiedOn'", "NULL", "NULL", "190", "NULL" } ),
             };



        public override void Up()
        {
            AddColumn("dbo.StreamNet_Detail", "CommonName", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "Run", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "PopFit", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "SpawnerLocation", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "RecruitLocation", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "BroodYear", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "RperStype", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "TRTmethod", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "ContactAgency", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "MethodNumber", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "RperS", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "Comments", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "NullRecord", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "DataStatus", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "ContactPersonFirst", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "ContactPersonLast", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "ContactPhone", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "ContactEmail", c => c.String());
            AlterColumn("dbo.StreamNet_Detail", "LastModifiedOn", c => c.String());
            DropColumn("dbo.StreamNet_Detail", "Species");
            DropColumn("dbo.StreamNet_Detail", "Location");
            DropColumn("dbo.StreamNet_Detail", "Spawning");
            DropColumn("dbo.StreamNet_Detail", "Origin");
            DropColumn("dbo.StreamNet_Detail", "Age");
            DropColumn("dbo.StreamNet_Detail", "EstimatedNumber");
            DropColumn("dbo.StreamNet_Detail", "BroodStok");
            DropColumn("dbo.StreamNet_Detail", "Reference");
            DropColumn("dbo.StreamNet_Detail", "CIPercent");
            DropColumn("dbo.StreamNet_Detail", "Lower");
            DropColumn("dbo.StreamNet_Detail", "Upper");
            DropColumn("dbo.StreamNet_Detail", "Comment");

            Sql(@"
SET IDENTITY_INSERT  projects on
go

insert into projects (id, ProjectTypeId, OrganizationId, name, Description, CreateDateTime, StartDate, EndDate, OwnerId)
            values   (9999, 15, 1, 'StreamNet', '', getDate(), null, null, 6)
go
 SET IDENTITY_INSERT  projects off
go

-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'StreamNet_RperS'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = @datasetBaseName


-- These are predefined project IDs -- the records for them should already exist
CREATE TABLE #ProjectInfo (id int, name varchar(max))
INSERT INTO #ProjectInfo (id) 
          SELECT id = 9999  -- 


-- Grab the project names
update #ProjectInfo set name = (select name from projects where projects.id = #ProjectInfo.id)


-- Create a field category
INSERT INTO dbo.FieldCategories (Name, Description) 
SELECT Name        = @categoryName,
       Description = @datasetBaseName + ' related fields'


-- Add records to the Datastores
INSERT INTO dbo.Datastores (Name, Description, TablePrefix, DatastoreDatasetId, OwnerUserId, FieldCategoryId) 
SELECT
	Name               = @datastoreName, 
	Description        = NULL, 
	TablePrefix        = REPLACE(@datastoreName, ' ', ''), -- Strip spaces
	DatastoreDatasetId = NULL, 
	OwnerUserId        = 1081,      -- George
	FieldCategoryId    = (SELECT IDENT_CURRENT('dbo.FieldCategories'))


CREATE TABLE #NewDatasetIdsX (id int)        -- This will contain a list of ids of all dataset records inserted below

-- Add record to the Datasets --> will create one record per project
INSERT INTO	Datasets (ProjectId, DefaultRowQAStatusId, StatusId, CreateDateTime, Name, Description, DefaultActivityQAStatusId, DatastoreId, Config)
OUTPUT INSERTED.id into #NewDatasetIdsX
SELECT 
    ProjectId                 = p.id,
    DefaultRowQAStatusId      = 1,
    StatusId                  = 1,
    CreateDateTime            = GetDate(),
    Name                      = @datasetBaseName,
    Description               = @datasetBaseName + ': ' + p.name,
    DefaultActivityQAStatusId = 6,
    DatastoreId               = (SELECT IDENT_CURRENT('dbo.Datastores')),
    Config                    = NULL
FROM #ProjectInfo as p


");

            for (int i = 0; i <= 18; i++)
                Sql(GetAddFieldSql(i));


            Sql(@"
-- Add some new DatasetQAStatus records for our new datasets
CREATE TABLE #QaStatusIds (id int)
INSERT INTO #QaStatusIds (id) 
          SELECT id = 5     -- Approved
UNION ALL SELECT id = 6     -- Ready for QA


INSERT INTO dbo.DatasetQAStatus(Dataset_Id, QAStatus_id)
SELECT
    Dataset_Id  = d.id,
    QAStatus_id = q.id
FROM #NewDatasetIdsX as d, #QaStatusIds as q


INSERT INTO dbo.DatasetQAStatus1(Dataset_Id, QAStatus_id)
SELECT
    Dataset_Id  = d.id,
    QAStatus_id = q.id
FROM #NewDatasetIdsX as d, #QaStatusIds as q



-- Cleanup
drop table #ProjectInfo
drop table #NewDatasetIdsX
drop table #QaStatusIds
");
        }
        

        public override void Down()
        {
            AddColumn("dbo.StreamNet_Detail", "Comment", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "Upper", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "Lower", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "CIPercent", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "Reference", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "BroodStok", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "EstimatedNumber", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "Age", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "Origin", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "Spawning", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "Location", c => c.String());
            AddColumn("dbo.StreamNet_Detail", "Species", c => c.String());
            AlterColumn("dbo.StreamNet_Detail", "LastModifiedOn", c => c.DateTime());
            DropColumn("dbo.StreamNet_Detail", "ContactEmail");
            DropColumn("dbo.StreamNet_Detail", "ContactPhone");
            DropColumn("dbo.StreamNet_Detail", "ContactPersonLast");
            DropColumn("dbo.StreamNet_Detail", "ContactPersonFirst");
            DropColumn("dbo.StreamNet_Detail", "DataStatus");
            DropColumn("dbo.StreamNet_Detail", "NullRecord");
            DropColumn("dbo.StreamNet_Detail", "Comments");
            DropColumn("dbo.StreamNet_Detail", "RperS");
            DropColumn("dbo.StreamNet_Detail", "MethodNumber");
            DropColumn("dbo.StreamNet_Detail", "ContactAgency");
            DropColumn("dbo.StreamNet_Detail", "TRTmethod");
            DropColumn("dbo.StreamNet_Detail", "RperStype");
            DropColumn("dbo.StreamNet_Detail", "BroodYear");
            DropColumn("dbo.StreamNet_Detail", "RecruitLocation");
            DropColumn("dbo.StreamNet_Detail", "SpawnerLocation");
            DropColumn("dbo.StreamNet_Detail", "PopFit");
            DropColumn("dbo.StreamNet_Detail", "Run");
            DropColumn("dbo.StreamNet_Detail", "CommonName");

            for(int i = 0; i <= 18; i++)
                Sql(GetRemoveFieldSql(i)); 
 
            Sql(@"
    -- Note: These MUST match values declared in Up()
declare @datasetBaseName as varchar(max) = 'StreamNet_RperS'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = @datasetBaseName

delete from dbo.DatasetQAStatus  where Dataset_Id in (select id from dbo.Datasets where name = @datasetBaseName)
delete from dbo.DatasetQAStatus1 where Dataset_Id in (select id from dbo.Datasets where name = @datasetBaseName)
delete from dbo.DatasetFields where DatasetId in (select id from dbo.Datasets where name = @datasetBaseName)
delete from dbo.Fields where FieldCategoryId in (select id from dbo.FieldCategories where name = @categoryName)
delete from dbo.Datasets where name = @datasetBaseName
delete from dbo.FieldCategories where name = @categoryName
delete from dbo.Datastores where name = @datastoreName
delete from Projects where id = 9999
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

