namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JvRearAddViews : DbMigration
    {
        public override void Up()
        {
            Sql(@"
create view JvRearing_Detail_VW
AS
SELECT        Id, Action, Species, PointData, Result, ResultUnit, ActionComments, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt
FROM            dbo.JvRearing_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.JvRearing_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
GO


create view JvRearing_Header_VW
AS
SELECT        Id, AcclimationYear, HeaderComments, ActivityId, ByUserId, EffDt
FROM            dbo.JvRearing_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.JvRearing_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO


create view JvRearing_vw
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, h.Id AS JvRearing_Header_Id, h.AcclimationYear, 
                         h.HeaderComments, h.ByUserId, h.EffDt, d.Id AS JvRearing_Detail_Id, d.Action, d.Species, d.PointData, d.Result, d.ResultUnit, d.ActionComments, d.RowId, 
                         d.RowStatusId, d.ByUserId AS JvRearing_Detail_ByUserId, d.QAStatusId, d.EffDt AS JvRearing_Detail_EffDt, q.QAStatusId AS ActivityQAStatusId, 
                         q.UserId AS ActivityQAs_UserId, q.Comments, q.QAStatusName, l.Label AS LocationLabel
FROM            dbo.Activities AS a INNER JOIN
                         dbo.JvRearing_Header_VW AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.JvRearing_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS q ON a.Id = q.ActivityId INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id
GO
            ");
        }

        public override void Down()
        {
            Sql(@"
drop view JvRearing_Detail_VW
drop view JvRearing_Header_VW
drop view JvRearing_vw
            ");
        }
    }
}
