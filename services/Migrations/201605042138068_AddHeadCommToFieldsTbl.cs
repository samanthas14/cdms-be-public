namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHeadCommToFieldsTbl : DbMigration
    {
        public override void Up()
        {
            Sql(@"
insert into [dbo].[Fields]
(FieldCategoryId, Name, [Description], DataType, DbColumnName, ControlType)
values
(7, 'Header Comments', 'Header Comments', 'string', 'HeaderComments', 'textarea')
            ");
        }
        
        public override void Down()
        {
            Sql(@"
delete from [dbo].[Fields]
where
FieldCategoryId = 7
and DbColumnName = 'HeaderComments'
            ");
        }
    }
}
