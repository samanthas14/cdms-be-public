namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreelAddDryAndTagToFieldsTbl : DbMigration
    {
        public override void Up()
        {
            Sql(@"
insert into dbo.Fields
(FieldCategoryId, Name, [Description], DataType, PossibleValues, DbColumnName, ControlType)
values
(15, 'Dry', 'Was the stream dry', 'string', '[""YES"",""NO""]', 'Dry', 'select')
GO

insert into dbo.Fields
(FieldCategoryId, Name, [Description], DataType, PossibleValues, DbColumnName, ControlType)
values
(15, 'Tag', 'Does the fish have a tag of any sort', 'string', '[""WIRE"",""RADIO"",""FLOY"",""PIT"",""VIE"",""OTHER"",""NONE""]', 'Tag', 'multiselect')
GO

insert into dbo.Fields
(FieldCategoryId, Name, [Description], DataType, DbColumnName, ControlType)
values
(15, 'OtherTagId', 'Does the fish have some other tag of any sort', 'string', 'OtherTagId', 'text')
GO
            ");
        }

        public override void Down()
        {
            Sql(@"
delete from dbo.Fields where FieldCategoryId = 15 and Name = 'Dry' and DataType = 'string'
GO

delete from dbo.Fields where FieldCategoryId = 15 and Name = 'Tag' and DataType = 'string'
GO

delete from dbo.Fields where FieldCategoryId = 15 and Name = 'OtherTagId' and DataType = 'string'
GO
            ");
        }
    }
}
