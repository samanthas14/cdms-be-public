namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WbReviseDepthData : DbMigration
    {
        public override void Up()
        {
            Sql(@"
Update dbo.Fields
set Name = 'Depth',
Units = 'meters',
DbColumnName = 'Depth'
where Id in
(
SELECT Id
  FROM dbo.[Fields]
  where FieldCategoryId = 2
  and DbColumnName = 'DepthToWater'
)

update dbo.DatasetFields
set FieldRoleId = 2,
Label = 'Depth',
DbColumnName = 'Depth'
where Id in 
(
select Id
from dbo.DatasetFields
where DatasetId in 
(
SELECT Id
  FROM dbo.[Datasets]
  where DatastoreId = 3
)
and DbColumnName = 'DepthToWater'
)
            ");
        }
        
        public override void Down()
        {
            Sql(@"
Update dbo.Fields
set Name = 'Depth to Water',
Units = 'ft',
DbColumnName = 'DepthToWater'
where Id in
(
SELECT Id
  FROM dbo.[Fields]
  where FieldCategoryId = 2
  and DbColumnName = 'Depth'
)

update dbo.DatasetFields
set Label = 'Depth to water',
DbColumnName = 'DepthToWater'
where Id in 
(
select Id
from dbo.DatasetFields
where DatasetId in 
(
SELECT Id
  FROM dbo.[Datasets]
  where DatastoreId = 3
)
and DbColumnName = 'Depth'
)
            ");
        }
    }
}
