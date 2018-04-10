namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AwUpdFieldsAndDsFields : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.[Fields]
set [Validation] = 'i4', DataType = 'int', ControlType = 'number'
where FieldCategoryId = 1 and DbColumnName = 'RunYear'

update dbo.DatasetFields
set [Validation] = 'i4', ControlType = 'number'
where Id in 
(
    -- The datasetIds we are working with
    select Id
    from dbo.DatasetFields
    where DatasetId in 
    (
        -- The datastores we are working with
        select Id
        from dbo.[Datasets]
        where DatastoreId = 1
        and Name like '%Weir%'
    )
    and DbColumnName = 'RunYear'
    --order by DatasetId, FieldRoleId, OrderIndex
)
            ");
        }

        public override void Down()
        {
        }
    }
}
