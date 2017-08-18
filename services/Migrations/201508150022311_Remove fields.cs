namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removefields : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SpawningGroundSurvey_Detail", "Easting");
            DropColumn("dbo.SpawningGroundSurvey_Detail", "Northing");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SpawningGroundSurvey_Detail", "Northing", c => c.Single());
            AddColumn("dbo.SpawningGroundSurvey_Detail", "Easting", c => c.Single());

            Sql(@"
                select id into #temp from SpawningGroundSurvey_Header
                delete from SpawningGroundSurvey_Header
                delete from SpawningGroundSurvey_Detail
                delete from activities where id in (select id from #temp)
                drop table #temp
                ");
        }
    }
}
