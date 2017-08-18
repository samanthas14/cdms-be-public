namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCrppContractsViews : DbMigration
    {
        public override void Up()
        {
            Sql(@"
CREATE VIEW [dbo].[CrppContracts_Detail_VW]
AS
SELECT        Id, AcresSurveyed, Testing, NewSites, MonitoredSites, SitesEvaluated, UpdatedSites, NewIsolates, Evaluation, HprcsitsRecorded, Monitoring, Notes, 
                         ShpoReportNumber, ShpoCaseNumber, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt
FROM            dbo.CrppContracts_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.CrppContracts_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
go


CREATE VIEW [dbo].[CrppContracts_Header_VW]
AS
SELECT        Id, CrppPersonnel, ActivityTypeId, Agency, ProjectProponent, PermitNumber, DateReceived, DateOfAction, ActionTaken, ActivityNotes, AttachedDocument, ActivityId, 
                         ByUserId, EffDt
FROM            dbo.CrppContracts_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.CrppContracts_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
go


CREATE VIEW [dbo].[CrppContracts_VW]
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, h.Id, h.CrppPersonnel, h.Agency, h.ProjectProponent, 
                         h.PermitNumber, h.DateReceived, h.DateOfAction, h.ActionTaken, h.ActivityNotes, h.AttachedDocument, h.ByUserId, h.EffDt, d.Id AS CrppContracts_Detail_Id, 
                         d.AcresSurveyed, d.Testing, d.NewSites, d.MonitoredSites, d.SitesEvaluated, d.UpdatedSites, d.NewIsolates, d.Evaluation, d.HprcsitsRecorded, d.Monitoring, 
                         d.Notes, d.ShpoReportNumber, d.ShpoCaseNumber, d.RowId, d.RowStatusId, d.ByUserId AS CrppContracts_Detail_ByUserId, d.QAStatusId, 
                         d.EffDt AS CrppContracts_Detail_EffDt, aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments, aq.QAStatusName
FROM            dbo.Activities AS a INNER JOIN
                         dbo.CrppContracts_Header AS h ON a.Id = h.ActivityId INNER JOIN
                         dbo.CrppContracts_Detail AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs AS aq ON a.Id = aq.ActivityId
go
            ");
        }

        public override void Down()
        {
            Sql(@"
drop view [dbo].[CrppContracts_Detail_VW]
go
drop view [dbo].[CrppContracts_Header_VW]
go
drop view [dbo].[CrppContracts_VW]
go
            ");
        }
    }
}
