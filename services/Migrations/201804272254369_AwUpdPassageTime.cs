namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AwUpdPassageTime : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.Fields
set ControlType = 'time'
where Id in (SELECT Id
  FROM [Fields]
  where DbColumnName = 'PassageTime'
  )

update dbo.DatasetFields
set ControlType = 'time'
where Id in 
(
select Id
From dbo.[DatasetFields]
where DatasetId in (select Id from Datasets where DatastoreId = 1 and Name like '%Weir%') and DbColumnName = 'PassageTime'
)

            ");
        }
        
        public override void Down()
        {
            Sql(@"
update dbo.Fields
set ControlType = 'text'
where Id in (SELECT Id
  FROM [CDMS_DEV462].[dbo].[Fields]
  where DbColumnName = 'PassageTime'
  )

update dbo.DatasetFields
set ControlType = 'text'
where Id in 
(
select Id
From dbo.[DatasetFields]
where DatasetId in (select Id from CDMS_DEV462.dbo.Datasets where DatastoreId = 1 and Name like '%Weir%') and DbColumnName = 'PassageTime'
)

update dbo.AdultWeir_vw
set PassageTime = DateAdd(day, DATEDIFF(day, '18991231', ActivityDate), CAST(PassageTime as datetime2(7)))
where AdultWeir_Detail_Id in 
(
select AdultWeir_Detail_Id --, ActivityDate, PassageTime--, DateAdd(day, DATEDIFF(day, '18991231', ActivityDate), CAST(PassageTime as datetime2(7))) AS pTime
from dbo.AdultWeir_vw
where PassageTime is not null-- and Year(PassageTime) < 2000
)
            ");
        }
    }
}
