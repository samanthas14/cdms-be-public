namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SfAddFieldsData : DbMigration
    {
        public override void Up()
        {
            Sql(@"
insert into dbo.[Fields]
(FieldCategoryId, Name, [Description], Units, DataType, PossibleValues, DbColumnName, ControlType)
values
(8, 'Habitat Visit Id', 'Habitat Visit Id', null, 'int', null, 'HabitatVisitId', 'number'),
(8, 'End Water Temp', 'End Water Temp', 'C', 'double', null, 'EndWaterTemp', 'number')

update dbo.[Fields]
set Name = 'Start Water Temp', DbColumnName = 'StartWaterTemp'
where FieldCategoryId = 8 and Name = 'Water Temperature' and DbColumnName = 'WaterTemperature'
            ");
        }

        public override void Down()
        {
            Sql(@"
delete FROM [dbo].[Fields]
  where [FieldCategoryId] = 8 and Name = 'Habitat Visit Id' and DbColumnName = 'HabitatVisitId' and DataType = 'int' and ControlType = 'number'
delete FROM [dbo].[Fields]
  where [FieldCategoryId] = 8 and Name = 'End Water Temp' and DbColumnName = 'EndWaterTemp' and DataType = 'double' and ControlType = 'number'

update dbo.[Fields]
set Name = 'Water Temperature', DbColumnName = 'WaterTemperature'
where FieldCategoryId = 8 and Name = 'Start Water Temp' and DbColumnName = 'StartWaterTemp'
            ");
        }
    }
}
