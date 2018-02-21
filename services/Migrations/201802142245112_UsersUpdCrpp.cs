namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersUpdCrpp : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.Users set Inactive = 1 where  [Id] in (3120, 3121, 3131)
            ");
        }
        
        public override void Down()
        {
        }
    }
}
