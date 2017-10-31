namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StreamNetAddPopId : DbMigration
    {
        public override void Up()
        {
            Sql(@"
declare @FieldCategoryId int;
declare @DatasetId int;
declare @FieldId nvarchar(max);

select @FieldCategoryId = (SELECT [Id] FROM dbo.[FieldCategories] where Name = 'StreamNet_RperS' );
insert into dbo.[Fields](FieldCategoryId, Name, [Description], DataType, DbColumnName, ControlType)
values(@FieldCategoryId, 'PopID', 'Population Id', 'string', 'PopID', 'text');

select @FieldId = (select Id from dbo.[Fields] where FieldCategoryId = @FieldCategoryId and DbColumnName = 'PopID');
select @DatasetId = (select Id from dbo.Datasets where Name = 'StreamNet_RperS');
insert into dbo.[DatasetFields](DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, InstrumentId, OrderIndex, SourceId, ControlType)
values(@DatasetId, @FieldId, 2, GETDATE(), 'PopID', 'PopID', null, 520, 1, 'text');

select @FieldCategoryId = (SELECT [Id] FROM dbo.[FieldCategories] where Name = 'StreamNet_NOSA' );
insert into dbo.[Fields](FieldCategoryId, Name, [Description], DataType, DbColumnName, ControlType)
values(@FieldCategoryId, 'PopID', 'Population Id', 'string', 'PopID', 'text');

select @FieldId = (select Id from dbo.[Fields] where FieldCategoryId = @FieldCategoryId and DbColumnName = 'PopID');
select @DatasetId = (select Id from dbo.Datasets where Name = 'StreamNet_NOSA');
insert into dbo.[DatasetFields](DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, InstrumentId, OrderIndex, SourceId, ControlType)
values(@DatasetId, @FieldId, 2, GETDATE(), 'PopID', 'PopID', null, 520, 1, 'text');


select @FieldCategoryId = (SELECT [Id] FROM dbo.[FieldCategories] where Name = 'StreamNet_SAR' );
insert into dbo.[Fields](FieldCategoryId, Name, [Description], DataType, DbColumnName, ControlType)
values(@FieldCategoryId, 'PopID', 'Population Id', 'string', 'PopID', 'text');

select @FieldId = (select Id from dbo.[Fields] where FieldCategoryId = @FieldCategoryId and DbColumnName = 'PopID');
select @DatasetId = (select Id from dbo.Datasets where Name = 'StreamNet_SAR');
insert into dbo.[DatasetFields](DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, InstrumentId, OrderIndex, SourceId, ControlType)
values(@DatasetId, @FieldId, 2, GETDATE(), 'PopID', 'PopID', null, 520, 1, 'text');

            ");
        }
        
        public override void Down()
        {
            Sql(@"
declare @FieldCategoryId int;
declare @DatasetId int;
declare @FieldId nvarchar(max);

select @FieldCategoryId = (SELECT [Id] FROM dbo.[FieldCategories] where Name = 'StreamNet_RperS' );
select @FieldId = (select Id from dbo.[Fields] where FieldCategoryId = @FieldCategoryId and DbColumnName = 'PopID');
select @DatasetId = (select Id from dbo.Datasets where Name = 'StreamNet_RperS');
delete from dbo.[DatasetFields] where DatasetId = @DatasetId and FieldId = @FieldId and DbColumnName = 'PopID';
delete from dbo.[Fields] where FieldCategoryId = @FieldCategoryId and DbColumnName = 'PopID';

select @FieldCategoryId = (SELECT [Id] FROM dbo.[FieldCategories] where Name = 'StreamNet_NOSA' );
select @FieldId = (select Id from dbo.[Fields] where FieldCategoryId = @FieldCategoryId and DbColumnName = 'PopID');
select @DatasetId = (select Id from dbo.Datasets where Name = 'StreamNet_NOSA');
delete from dbo.[DatasetFields] where DatasetId = @DatasetId and FieldId = @FieldId and DbColumnName = 'PopID';
delete from dbo.[Fields] where FieldCategoryId = @FieldCategoryId and DbColumnName = 'PopID';

select @FieldCategoryId = (SELECT [Id] FROM dbo.[FieldCategories] where Name = 'StreamNet_SAR' );
select @FieldId = (select Id from dbo.[Fields] where FieldCategoryId = @FieldCategoryId and DbColumnName = 'PopID');
select @DatasetId = (select Id from dbo.Datasets where Name = 'StreamNet_SAR');
delete from dbo.[DatasetFields] where DatasetId = @DatasetId and FieldId = @FieldId and DbColumnName = 'PopID';
delete from dbo.[Fields] where FieldCategoryId = @FieldCategoryId and DbColumnName = 'PopID';
            ");
        }
    }
}
