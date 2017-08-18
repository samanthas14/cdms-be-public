namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateScrewTrapFieldData : DbMigration
    {
        public override void Up()
        {
            Sql(@"
insert into dbo.[Fields]([FieldCategoryId],[Name],[Description],[DataType],[PossibleValues],[DbColumnName],[ControlType])
values
(
9,
'Crew', 
'The technicians that were tending to the trap during the activity', 
'string', 
null,
'Crew',
'text'
)
go


insert into dbo.[Fields]([FieldCategoryId],[Name],[Description],[DataType],[PossibleValues],[DbColumnName],[ControlType])
values
(
9,
'Trap Status', 
'Type of activity associated with the trap', 
'string', 
'{""STOPPED ON ARRIVAL"":""Stopped on Arrival"", ""PULLED"":""Pulled"", ""START"":""Start"", ""IN"":""In"", ""OUT"":""Out""}',
'TrapStatus',
'select'
)
go


update dbo.[DatasetFields]
set [Label] = 'Turbidity', [DbColumnName] = 'Turbidity'
where [DatasetId] = 1215
and [DbColumnName] = 'Turbitity'
go 

insert into dbo.[DatasetFields]
([DatasetId],[FieldId],[FieldRoleId],[CreateDateTime],[Label],[DbColumnName],[SourceId],[InstrumentId],[OrderIndex],[ControlType] )
values
(1215, 1509, 1, CONVERT(varchar(30),getDate(),121), 'Trap Status', 'TrapStatus', 1, null, 55, 'select')
go


delete FROM dbo.[DatasetFields]
  where [DatasetId] = 1215
  and [DbColumnName] = 'Unit'
go
            ");
        }
        
        public override void Down()
        {
            Sql(@"
delete from dbo.[DatasetFields]
where 
    [DatasetId] = 1215
    and [FieldId] = 1509
    and [DbColumnName] = 'TrapStatus'
go

delete from dbo.[Fields]
where
    [FieldCategoryId] = 9
    and [DbColumnName] = 'TrapStatus'
go

delete from dbo.[Fields]
where 
    [FieldCategoryId] = 9
    and [DbColumnName] = 'Crew'   
go

update dbo.[DatasetFields]
set [Label] = 'Turbitity', [DbColumnName] = 'Turbitity'
where [DatasetId] = 1215
and [DbColumnName] = 'Turbidity'
go
            ");
        }
    }
}
