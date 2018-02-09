namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MetricsUpdView : DbMigration
    {
        public override void Up()
        {
            Sql(@"
DROP VIEW [dbo].[Metrics_vw]
GO
CREATE VIEW [dbo].[Metrics_vw]
AS
SELECT        h.Id, h.YearReported, h.ByUserId, h.EffDt, a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, 
                         d.Id AS Metrics_Detail_Id, d.WorkElementName, d.Measure, d.PlannedValue, d.ActualValue, d.Comments, d.RowId, d.RowStatusId, 
                         d.ByUserId AS Metrics_Detail_ByUserId, d.QAStatusId, d.EffDt AS Metrics_Detail_EffDt, l.Label AS LocationLabel
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Metrics_Header_VW AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.Metrics_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id
            ");
        }
        
        public override void Down()
        {
            Sql(@"
DROP VIEW [dbo].[Metrics_vw]
GO
CREATE VIEW [dbo].[Metrics_vw]
AS
SELECT        h.Id, h.YearReported, h.ByUserId, h.EffDt, a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, 
                         d.Id AS Metrics_Detail_Id, d.WorkElementName, d.Measure, d.PlannedValue, d.ActualValue, d.Comments, d.RowId, d.RowStatusId, 
                         d.ByUserId AS Metrics_Detail_ByUserId, d.QAStatusId, d.EffDt AS Metrics_Detail_EffDt
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Metrics_Header_VW AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.Metrics_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId INNER JOIN
                         dbo.Locations ON a.LocationId = dbo.Locations.Id
            ");
        }
    }
}
