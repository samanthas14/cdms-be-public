namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GeneticAddViews : DbMigration
    {
        public override void Up()
        {
            Sql(@"
create view Genetic_Detail_VW
AS
SELECT        Id, SampleYear, GeneticId, LifeStage, JuvenileAge, ForkLength, P1_Id, P1CollectYear, P1CollectLoc, P1Sex, P1Origin, P2_Id, P2CollectYear, P2CollectLoc, P2Sex, 
                         P2Origin, GeneticComment, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt
FROM            dbo.Genetic_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Genetic_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
GO


create view Genetic_Header_VW
AS
SELECT        Id, HeaderComments, ActivityId, ByUserId, EffDt
FROM            dbo.Genetic_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Genetic_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO


create view Genetic_vw
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, h.HeaderComments, h.ByUserId, h.EffDt, 
                         h.Id AS Genetic_Header_Id, d.Id AS Genetic_Detail_Id, d.SampleYear, d.GeneticId, d.LifeStage, d.JuvenileAge, d.ForkLength, d.P1_Id, d.P1CollectYear, 
                         d.P1CollectLoc, d.P1Sex, d.P1Origin, d.P2_Id, d.P2CollectYear, d.P2CollectLoc, d.P2Sex, d.P2Origin, d.GeneticComment, d.RowId, d.RowStatusId, 
                         d.ByUserId AS Genetic_Detail_ByUserId, d.EffDt AS Genetic_Detail_EffDt, d.QAStatusId, aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, 
                         aq.Comments, aq.QAStatusName, l.Label AS LocationLabel
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Genetic_Header_VW AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.Genetic_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id
            ");
        }
        
        public override void Down()
        {
            Sql(@"
drop view Genetic_Detail_VW
drop view Genetic_Header_VW
drop view Genetic_vw
            ");
        }
    }
}
