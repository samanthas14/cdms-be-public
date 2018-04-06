namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AwFillRunYearCol : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.AdultWeir_Detail
set RunYear = r.bRunYear
from
(
select awd.[Id], awd.RunYear, awdBu.Id AS bId, awdBu.RunYear AS bRunYear
from dbo.AdultWeir_Detail as awd
inner join dbo.AdultWeir_DetailBu as awdBu on awdBu.Id = awd.Id
where ISNUMERIC(awdBu.RunYear) = 1
) as r
where dbo.AdultWeir_Detail.Id = r.Id

update dbo.AdultWeir_Detail
set RunYear = 2018 where RunYear = 20187
            ");
        }

        public override void Down()
        {
        }
    }
}
