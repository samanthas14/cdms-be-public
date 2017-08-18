namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ElecAddPassNumData : DbMigration
    {
        public override void Up()
        {
            Sql(@"
insert into dbo.[Fields]
(FieldCategoryId, Name, [Description], DataType, PossibleValues, DbColumnName, ControlType)
values
(10, 'Pass Number', 'Pass number', 'int', '[""1"", ""2"", ""3"", ""4"", ""5"", ""6""]', 'PassNumber', 'select')
            ");
        }
        
        public override void Down()
        {
            Sql(@"
delete FROM [dbo].[Fields]
  where [FieldCategoryId] = 10 and Name = 'Pass Number' and DbColumnName = 'PassNumber' and DataType = 'int' and ControlType = 'select'
            ");
        }
    }
}
