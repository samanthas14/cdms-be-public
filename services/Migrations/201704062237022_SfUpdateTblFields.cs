namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SfUpdateTblFields : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                update dbo.[Fields] set [Validation] = 'i4' where DbColumnName = 'HabitatVisitId'
            ");
        }
        
        public override void Down()
        {
            Sql(@"
                update dbo.[Fields] set [Validation] = null where DbColumnName = 'HabitatVisitId'
            ");
        }
    }
}
