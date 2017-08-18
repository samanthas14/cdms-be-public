namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WqChgResultsType : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.WaterQuality_Detail
set Result = null
where Result = ''
go

update dbo.WaterQuality_Detail
set Result = null
where Result is not null
and ISNUMERIC(Result) = 0
go

alter table dbo.WaterQuality_Detail
add numResults decimal(11,5)
go

update dbo.WaterQuality_Detail
set numResults = CONVERT(decimal(11,5), Result)
where 
Result is not null
and ISNUMERIC(Result) = 1
go

update dbo.WaterQuality_Detail
set Result = null
go
            ");

            AlterColumn("dbo.WaterQuality_Detail", "Result", c => c.Decimal(precision: 11, scale: 5));

            Sql(@"
alter table dbo.WaterQuality_Detail
alter column Result decimal(11,5)
go

update dbo.WaterQuality_Detail
set Result = numResults
go

alter table dbo.WaterQuality_Detail
drop column numResults
go

update dbo.Fields
set ControlType = 'number'
where FieldCategoryId = 7 and [Name] = 'Result'
go
            ");
        }
        
        public override void Down()
        {
            Sql(@"
alter table dbo.WaterQuality_Detail
add strResults nvarchar(max)
go

update dbo.WaterQuality_Detail
set strResults = CONVERT(nvarchar(max), Result)
go

update dbo.WaterQuality_Detail
set Result = null
go
            ");

            AlterColumn("dbo.WaterQuality_Detail", "Result", c => c.String());

            Sql(@"
update dbo.WaterQuality_Detail
set Result = strResults
go

alter table dbo.WaterQuality_Detail
drop column strResults
go

update dbo.Fields
set ControlType = 'text'
where FieldCategoryId = 7 and [Name] = 'Result'
go
            ");
        }
    }
}
