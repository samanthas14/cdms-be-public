namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHabSubpRefRecord : DbMigration
    {
        public override void Up()
        {
            Sql(@"
insert into dbo.Subproject_Hab(ProjectName, EffDt, ByUserId, ProjectId)
values('Reference Record', convert(varchar(19), getdate(), 120),1081, 1223)
go
            ");
        }

        public override void Down()
        {
            Sql(@"
delete from dbo.Subproject_Hab
go
            ");
        }
    }
}
