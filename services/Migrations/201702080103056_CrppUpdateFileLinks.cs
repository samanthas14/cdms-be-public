namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CrppUpdateFileLinks : DbMigration
    {
        public override void Up()
        {
            Sql(@"
--Shorten link
update dbo.[Files]
set Link = replace(Link, 'ProjectDocuments\', '')
where Subproject_CrppId is not null

update dbo.[Files]
set Link = replace(Link, 'uploads\subprojects\', '')
where Subproject_CrppId is not null

update dbo.[Files]
set Link = replace(Link, 'uploads\', '')
where Subproject_CrppId is not null
            ");
        }

        public override void Down()
        {
            Sql(@"
-- Put it back the way it was.
update dbo.[Files]
set Link = replace(Link, 'CDMS\', 'CDMS\ProjectDocuments\')
where Subproject_CrppId is not null

update dbo.[Files]
set Link = replace(Link, 'Prod\', 'Prod\uploads\subprojects\')
where Subproject_CrppId is not null
            ");
        }
    }
}
