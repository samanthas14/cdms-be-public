namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreViewsForSGS : DbMigration
    {
        public override void Up()
        {
            Sql(@"
CREATE VIEW [dbo].[SpawningGroundSurvey_Detail_VW]
AS
SELECT        *
FROM            dbo.SpawningGroundSurvey_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SpawningGroundSurvey_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId)))

go

CREATE VIEW [dbo].[SpawningGroundSurvey_Header_VW]
AS
SELECT        *
FROM            dbo.SpawningGroundSurvey_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SpawningGroundSurvey_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)));
go

");
        }
        
        public override void Down()
        {
            Sql(@"
drop view SpawningGroundSurvey_Detail_VW
go
drop view SpawningGroundSurvey_Header_VW
go

");
        }
    }
}
