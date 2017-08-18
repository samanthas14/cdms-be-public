namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HabAddViews : DbMigration
    {
        public override void Up()
        {
            Sql(@"
if exists (select * from sys.views where name = 'Metrics_Detail_VW' and schema_id = SCHEMA_ID('dbo')) drop view [dbo].[Metrics_Detail_VW]
GO
CREATE VIEW [dbo].[Metrics_Detail_VW]
AS
SELECT        Id, WorkElementName, Measure, PlannedValue, ActualValue, Comments, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt
FROM            dbo.Metrics_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Metrics_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
GO

if exists (select * from sys.views where name = 'Metrics_Header_VW' and schema_id = SCHEMA_ID('dbo')) drop view [dbo].[Metrics_Header_VW]
GO
CREATE VIEW [dbo].[Metrics_Header_VW]
AS
SELECT        Id, YearReported, ActivityId, ByUserId, EffDt
FROM            dbo.Metrics_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Metrics_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO

if exists (select * from sys.views where name = 'Metrics_vw' and schema_id = SCHEMA_ID('dbo')) drop view [dbo].[Metrics_vw]
GO
CREATE VIEW [dbo].[Metrics_vw]
AS
SELECT        h.Id, h.YearReported, h.ByUserId, h.EffDt, a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, d.Id AS Metrics_Detail_Id, 
                         d.WorkElementName, d.Measure, d.PlannedValue, d.ActualValue, d.Comments, d.RowId, d.RowStatusId, d.ByUserId AS Metrics_Detail_ByUserId, d.QAStatusId, 
                         d.EffDt AS Metrics_Detail_EffDt
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Metrics_Header_VW AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.Metrics_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId INNER JOIN
                         dbo.Locations ON a.LocationId = dbo.Locations.Id
GO
            ");
        }
        
        public override void Down()
        {
            Sql(@"
drop view [dbo].[Metrics_Detail_VW]
GO
drop view [dbo].[Metrics_Header_VW]
GO
drop view [dbo].[Metrics_vw]
GO
            ");
        }
    }
}
