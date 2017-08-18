namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCrppSubpRefRecord : DbMigration
    {
        public override void Up()
        {
            Sql(@"
insert into dbo.Subproject_Crpp(ProjectName, EffDt, ByUserId)
values('Reference Record', convert(varchar(19), getdate(), 120),1081)
go
            ");
        }

        public override void Down()
        {
            Sql(@"
delete from dbo.Subproject_Crpp
go
            ");
        }
    }
}
