namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WtAddDataForPressure : DbMigration
    {
        public override void Up()
        {
            Sql(@"
insert into dbo.[Fields]
(FieldCategoryId, Name, [Description], Units, DataType, DbColumnName, ControlType)
values
(2, 'Absolute Pressure', 'Absolute pressure, similar to Well Pressure', 'psi', 'float', 'AbsolutePressure', 'number')

update dbo.[DatasetFields] set OrderIndex = OrderIndex * 10 where DatasetId = 1196 and OrderIndex is not null
update dbo.[DatasetFields] set OrderIndex = 205 where DatasetId = 1196 and [FieldId] = 1072
update dbo.[DatasetFields] set OrderIndex = 207 where DatasetId = 1196 and [FieldId] = 1071

--Verify this record is in place on Prod first.
--update dbo.[DatasetFields] set OrderIndex = 245 where [Id] = 2993
            ");
        }

        public override void Down()
        {
            Sql(@"
delete FROM [CDMS_DEV_prodTrack].[dbo].[Fields]
  where [FieldCategoryId] = 2 and Name = 'Absolute Pressure' and DbColumnName = 'AbsolutePressure' and Units = 'psi'
            ");
        }
    }
}
