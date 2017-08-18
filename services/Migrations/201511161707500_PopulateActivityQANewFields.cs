namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateActivityQANewFields : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update aq
set aq.QAStatusName = q.Name
from dbo.ActivityQAs as aq
inner join dbo.QAStatus as q on aq.QAStatusId = q.[Id]
go

update aq
set aq.QAStatusDescription = q.Description
from dbo.ActivityQAs as aq
inner join dbo.QAStatus as q on aq.QAStatusId = q.[Id]
go
            ");
        }
        
        public override void Down()
        {
            Sql(@"
update dbo.[ActivityQAs] set QAStatusName = null
update dbo.[ActivityQAs] set QAStatusDescription = null
            ");
        }
    }
}
