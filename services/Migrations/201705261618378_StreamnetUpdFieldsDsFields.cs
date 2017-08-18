namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StreamnetUpdFieldsDsFields : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.[Fields] set DbColumnName = 'LastUpdated' where FieldCategoryId = 12 and DbColumnName = 'LastModifiedOn'
update dbo.[DatasetFields] set DbColumnName = 'LastUpdated' where DatasetId = 1227 and DbColumnName = 'LastModifiedOn'
            ");
        }
        
        public override void Down()
        {
            Sql(@"
update dbo.[Fields] set DbColumnName = 'LastModifiedOn' where FieldCategoryId = 12 and DbColumnName = 'LastUpdated'
update dbo.[DatasetFields] set DbColumnName = 'LastModifiedOn' where DatasetId = 1227 and DbColumnName = 'LastUpdated'
            ");
        }
    }
}
