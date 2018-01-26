namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CrppUpdViews2 : DbMigration
    {
        public override void Up()
        {
            Sql(@"
DROP VIEW [dbo].[CrppContracts_Header_VW]
GO
CREATE VIEW [dbo].[CrppContracts_Header_VW]
AS
SELECT        Id, ProjectProponent, ByUserId, EffDt, ProjectLead, CostCenter, ProjectName, Client, AgreeNumb, DateExec, 
DraftDue, FinalDue, ContractEnd, ModExtDate, DocumentLink, ActivityComments, ActivityId, AwardAmount, FinalReportSubmitted
FROM            dbo.CrppContracts_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.CrppContracts_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO

DROP VIEW [dbo].[CrppContracts_VW]
GO
CREATE VIEW [dbo].[CrppContracts_VW]
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, 
h.Id, h.ProjectProponent, h.ByUserId, h.EffDt, h.ProjectLead, h.CostCenter, h.ProjectName, h.Client, h.AgreeNumb, h.DateExec, 
h.DraftDue, h.FinalDue, h.ContractEnd, h.ModExtDate, h.DocumentLink, h.ActivityComments, h.AwardAmount, h.FinalReportSubmitted,
d.Id AS CrppContracts_Detail_Id, d.Testing, d.NewSites, d.MonitoredSites, d.SitesEvaluated, d.UpdatedSites, d.NewIsolates, 
d.Evaluation, d.Monitoring, d.Notes, d.RowId, d.RowStatusId, d.ByUserId AS CrppContracts_Detail_ByUserId, d.QAStatusId, 
d.EffDt AS CrppContracts_Detail_EffDt, d.SurveyAcres, d.TestSites, d.SHRENum, d.SHCSNum, d.HPRCSIT,
aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments, aq.QAStatusName
FROM            dbo.Activities AS a INNER JOIN
                         dbo.CrppContracts_Header_VW AS h ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.CrppContracts_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId
GO
            ");
        }
        
        public override void Down()
        {
            Sql(@"
DROP VIEW [dbo].[CrppContracts_Detail_VW]
GO
CREATE VIEW [dbo].[CrppContracts_Detail_VW]
AS
SELECT        Id, Testing, NewSites, MonitoredSites, SitesEvaluated, UpdatedSites, NewIsolates, Evaluation, Monitoring, 
Notes, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, SurveyAcres, TestSites, SHRENum, SHCSNum, HPRCSIT
FROM            dbo.CrppContracts_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.CrppContracts_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
GO

DROP VIEW [dbo].[CrppContracts_Header_VW]
GO
CREATE VIEW [dbo].[CrppContracts_Header_VW]
AS
SELECT        Id, ProjectProponent, ByUserId, EffDt, ProjectLead, CostCenter, ProjectName, Client, AgreeNumb, DateExec, 
DraftDue, FinalDue, ContractEnd, ModExtDate, DocumentLink, ActivityComments, ActivityId
FROM            dbo.CrppContracts_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.CrppContracts_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO

DROP VIEW [dbo].[CrppContracts_VW]
GO
CREATE VIEW [dbo].[CrppContracts_VW]
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, 
h.Id, h.ProjectProponent, h.ByUserId, h.EffDt, h.ProjectLead, h.CostCenter, h.ProjectName, h.Client, h.AgreeNumb, h.DateExec, 
h.DraftDue, h.FinalDue, h.ContractEnd, h.ModExtDate, h.DocumentLink, h.ActivityComments,
d.Id AS CrppContracts_Detail_Id, d.Testing, d.NewSites, d.MonitoredSites, d.SitesEvaluated, d.UpdatedSites, d.NewIsolates, 
d.Evaluation, d.Monitoring, d.Notes, d.RowId, d.RowStatusId, d.ByUserId AS CrppContracts_Detail_ByUserId, d.QAStatusId, 
d.EffDt AS CrppContracts_Detail_EffDt, d.SurveyAcres, d.TestSites, d.SHRENum, d.SHCSNum, d.HPRCSIT,
aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments, aq.QAStatusName
FROM            dbo.Activities AS a INNER JOIN
                         dbo.CrppContracts_Header_VW AS h ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.CrppContracts_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId
GO
            ");
        }
    }
}
