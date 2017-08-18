using System.Collections.Generic;

namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StreamNetDefsForSAR : DbMigration
    {
        private static readonly List<Tuple<string, List<string>>> vals = new List<Tuple<string, List<string>>> { 

           // select * from FieldCategories
           //     FieldCategory                         Name             H/D     dbName            dbType      CtrlType         Descr          Units   Rule   Order     PossValues                                                                                                                                                                                                                                                                                                                                                                                                                                 Order    PossibleValues               
          		new Tuple<string, List<string>>
/* 0 */   ("StreamNet_SAR", new List<string> { "'CommonName'", DET, "'CommonName'", "'string'", "'text'", "'CommonName'", "NULL", "NULL", "10", "NULL" } ),
		new Tuple<string, List<string>>
/* 1 */   ("StreamNet_SAR", new List<string> { "'Run'", DET, "'Run'", "'string'", "'text'", "'Run'", "NULL", "NULL", "20", "NULL" } ),
		new Tuple<string, List<string>>
/* 2 */   ("StreamNet_SAR", new List<string> { "'PopFit'", DET, "'PopFit'", "'string'", "'text'", "'PopFit'", "NULL", "NULL", "30", "NULL" } ),
		new Tuple<string, List<string>>
/* 3 */   ("StreamNet_SAR", new List<string> { "'PopFitNotes'", DET, "'PopFitNotes'", "'string'", "'text'", "'PopFitNotes'", "NULL", "NULL", "40", "NULL" } ),
		new Tuple<string, List<string>>
/* 4 */   ("StreamNet_SAR", new List<string> { "'PopAggregation'", DET, "'PopAggregation'", "'string'", "'text'", "'PopAggregation'", "NULL", "NULL", "50", "NULL" } ),
		new Tuple<string, List<string>>
/* 5 */   ("StreamNet_SAR", new List<string> { "'SmoltLocation'", DET, "'SmoltLocation'", "'string'", "'text'", "'SmoltLocation'", "NULL", "NULL", "60", "NULL" } ),
		new Tuple<string, List<string>>
/* 6 */   ("StreamNet_SAR", new List<string> { "'AdultLocation'", DET, "'AdultLocation'", "'string'", "'text'", "'AdultLocation'", "NULL", "NULL", "70", "NULL" } ),
		new Tuple<string, List<string>>
/* 7 */   ("StreamNet_SAR", new List<string> { "'SARtype'", DET, "'SARtype'", "'string'", "'text'", "'SARtype'", "NULL", "NULL", "80", "NULL" } ),
		new Tuple<string, List<string>>
/* 8 */   ("StreamNet_SAR", new List<string> { "'OutmigrationYear'", DET, "'OutmigrationYear'", "'string'", "'text'", "'OutmigrationYear'", "NULL", "NULL", "90", "NULL" } ),
		new Tuple<string, List<string>>
/* 9 */   ("StreamNet_SAR", new List<string> { "'TRTmethod'", DET, "'TRTmethod'", "'string'", "'text'", "'TRTmethod'", "NULL", "NULL", "100", "NULL" } ),
		new Tuple<string, List<string>>
/* 11 */   ("StreamNet_SAR", new List<string> { "'MethodNumber'", DET, "'MethodNumber'", "'string'", "'text'", "'MethodNumber'", "NULL", "NULL", "120", "NULL" } ),
		new Tuple<string, List<string>>
/* 12 */   ("StreamNet_SAR", new List<string> { "'SAR'", DET, "'SAR'", "'string'", "'text'", "'SAR'", "NULL", "NULL", "130", "NULL" } ),
		new Tuple<string, List<string>>
/* 13 */   ("StreamNet_SAR", new List<string> { "'RearingType'", DET, "'RearingType'", "'string'", "'text'", "'RearingType'", "NULL", "NULL", "140", "NULL" } ),
		new Tuple<string, List<string>>
/* 14 */   ("StreamNet_SAR", new List<string> { "'Comments'", DET, "'Comments'", "'string'", "'text'", "'Comments'", "NULL", "NULL", "150", "NULL" } ),
		new Tuple<string, List<string>>
/* 15 */   ("StreamNet_SAR", new List<string> { "'NullRecord'", DET, "'NullRecord'", "'string'", "'text'", "'NullRecord'", "NULL", "NULL", "160", "NULL" } ),
		new Tuple<string, List<string>>
/* 16 */   ("StreamNet_SAR", new List<string> { "'DataStatus'", DET, "'DataStatus'", "'string'", "'text'", "'DataStatus'", "NULL", "NULL", "170", "NULL" } ),
		new Tuple<string, List<string>>
/* 17 */   ("StreamNet_SAR", new List<string> { "'ContactPersonFirst'", DET, "'ContactPersonFirst'", "'string'", "'text'", "'ContactPersonFirst'", "NULL", "NULL", "180", "NULL" } ),
		new Tuple<string, List<string>>
/* 18 */   ("StreamNet_SAR", new List<string> { "'ContactPersonLast'", DET, "'ContactPersonLast'", "'string'", "'text'", "'ContactPersonLast'", "NULL", "NULL", "190", "NULL" } ),
		new Tuple<string, List<string>>
/* 19 */   ("StreamNet_SAR", new List<string> { "'ContactPhone'", DET, "'ContactPhone'", "'string'", "'text'", "'ContactPhone'", "NULL", "NULL", "200", "NULL" } ),
	    new Tuple<string, List<string>>
        /* 8 */   ("StreamNet_SAR", new List<string> { "'ContactAgency'", DET, "'ContactAgency'", "'string'", "'text'", "'ContactAgency'", "NULL", "NULL", "90", "NULL" } ),
            new Tuple<string, List<string>>
/* 20 */   ("StreamNet_SAR", new List<string> { "'ContactEmail'", DET, "'ContactEmail'", "'string'", "'text'", "'ContactEmail'", "NULL", "NULL", "210", "NULL" } ),


             };

        public override void Up()
        {
            Sql(@"

-- Note: These MUST match values declared in Down()
declare @datasetBaseName as varchar(max) = 'StreamNet_SAR'
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

            for (int i = 0; i < vals.Count; i++)
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
            for (int i = 0; i < vals.Count; i++)
                Sql(GetRemoveFieldSql(i));

            Sql(@"
    -- Note: These MUST match values declared in Up()
declare @datasetBaseName as varchar(max) = 'StreamNet_SAR'
declare @categoryName as varchar(max) = @datasetBaseName
declare @datastoreName as varchar(max) = @datasetBaseName

delete from dbo.DatasetQAStatus  where Dataset_Id in (select id from dbo.Datasets where name = @datasetBaseName)
delete from dbo.DatasetQAStatus1 where Dataset_Id in (select id from dbo.Datasets where name = @datasetBaseName)
delete from dbo.DatasetFields where DatasetId in (select id from dbo.Datasets where name = @datasetBaseName)
delete from dbo.Fields where FieldCategoryId in (select id from dbo.FieldCategories where name = @categoryName)
delete from dbo.Datasets where name = @datasetBaseName
delete from dbo.FieldCategories where name = @categoryName
delete from dbo.Datastores where name = @datastoreName


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