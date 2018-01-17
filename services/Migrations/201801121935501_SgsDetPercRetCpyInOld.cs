namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SgsDetPercRetCpyInOld : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.SpawningGroundSurvey_Detail
set PercentRetained = r.PercentRetained
from 
(select bu.Id, bu.PercentRetained from dbo.SpawningGroundSurvey_Detail_bu bu)
as r
where dbo.SpawningGroundSurvey_Detail.Id = r.Id
            ");
        }
        
        public override void Down()
        {
        }
    }
}
