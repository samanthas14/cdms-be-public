namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DecdAvUpdUpdateViews : DbMigration
    {
        public override void Up()
        {
            Sql(@"
drop view dbo.Appraisal_Detail_VW
GO
create view dbo.Appraisal_Detail_VW
as
SELECT        Id, AppraisalYear, AppraisalFiles, AppraisalPhotos, AppraisalComments, AppraisalStatus, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, AppraisalType,
                          AppraisalLogNumber, AppraisalValue, AppraisalValuationDate, Appraiser, TypeOfTransaction, PartiesInvolved, AppraisalProjectType, RequestNumber, 
                          NwroComments, RegionalOfficeReviewFiles, HighestAndBestUse, LastAppraisalRequestDate
FROM            dbo.Appraisal_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Appraisal_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
GO

drop view dbo.Appraisal_Header_VW
GO
create view dbo.Appraisal_Header_VW
as
SELECT        Id, Allotment, AllotmentPhotoFiles, FarmingLeaseFiles, UpdatedTSRFile, GrazingLeaseFiles, MapFiles, TimberAppraisalFiles, TSRFiles, 
                         AllotmentStatus, AllotmentName, AllotmentDescription, AllotmentComments, CobellAppraisalWave, LeaseTypes, HasTimber, IsMappable, 
                         Acres, PriorityType, LegalDescription, ActivityId, ByUserId, EffDt, OtherPermitLeases
FROM            dbo.Appraisal_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Appraisal_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO

drop view dbo.Appraisal_VW
GO
create view dbo.Appraisal_VW
as
SELECT        h.Id AS Appraisal_Header_Id, h.Allotment, h.AllotmentStatus, h.AllotmentName, h.UpdatedTSRFile, h.AllotmentDescription, h.AllotmentComments, 
                         h.HasTimber, h.IsMappable, h.Acres, h.PriorityType, h.LegalDescription, h.AllotmentPhotoFiles, h.FarmingLeaseFiles, h.OtherPermitLeases,
                         h.GrazingLeaseFiles, h.MapFiles, h.TimberAppraisalFiles, h.TSRFiles, h.CobellAppraisalWave, h.LeaseTypes, h.ByUserId, h.EffDt, 
                         d.Id AS Appraisal_Detail_Id, d.AppraisalYear, d.AppraisalFiles, d.AppraisalPhotos, d.AppraisalComments, d.AppraisalStatus, d.AppraisalType, 
                         d.AppraisalLogNumber, d.AppraisalValue, d.AppraisalValuationDate, d.Appraiser, d.TypeOfTransaction, d.PartiesInvolved, d.AppraisalProjectType, d.RowId, 
                         d.RowStatusId, d.ByUserId AS Appraisal_Detail_ByUserId, d.QAStatusId, d.EffDt AS Appraisal_Detail_EffDt, d.RequestNumber, 
                         d.NwroComments, d.RegionalOfficeReviewFiles, d.HighestAndBestUse, d.LastAppraisalRequestDate,
                         aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId, a.SourceId, a.LocationId, 
                         a.UserId AS Activity_UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, a.Id AS ActivityId, aq.QAStatusName
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Appraisal_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.Appraisal_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
GO
            ");
        }

        public override void Down()
        {
            Sql(@"
drop view dbo.Appraisal_Detail_VW
GO
create view dbo.Appraisal_Detail_VW
as
SELECT        Id, AppraisalYear, AppraisalFiles, AppraisalPhotos, AppraisalComments, AppraisalStatus, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, AppraisalType,
                          AppraisalLogNumber, AppraisalValue, AppraisalValuationDate, Appraiser, TypeOfTransaction, PartiesInvolved, AppraisalProjectType, RequestNumber, 
                         OtherPermitLeases, NwroComments
FROM            dbo.Appraisal_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Appraisal_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
GO

drop view dbo.Appraisal_Header_VW
GO
create view dbo.Appraisal_Header_VW
as
SELECT        Id, Allotment, AllotmentPhotoFiles, FarmingLeaseFiles, UpdatedTSRFile, GrazingLeaseFiles, MapFiles, RegionalOfficeReviewFiles, TimberAppraisalFiles, TSRFiles, 
                         LastAppraisalRequestDate, AllotmentStatus, AllotmentName, AllotmentDescription, AllotmentComments, CobellAppraisalWave, LeaseTypes, HasTimber, IsMappable, 
                         Acres, PriorityType, LegalDescription, ActivityId, ByUserId, EffDt, HighestAndBestUse
FROM            dbo.Appraisal_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Appraisal_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO

drop view dbo.Appraisal_VW
GO
create view dbo.Appraisal_VW
as
SELECT        h.Id AS Appraisal_Header_Id, h.Allotment, h.AllotmentStatus, h.AllotmentName, h.UpdatedTSRFile, h.AllotmentDescription, h.AllotmentComments, 
                         h.LastAppraisalRequestDate, h.HasTimber, h.IsMappable, h.Acres, h.PriorityType, h.LegalDescription, h.AllotmentPhotoFiles, h.FarmingLeaseFiles, 
                         h.GrazingLeaseFiles, h.MapFiles, h.RegionalOfficeReviewFiles, h.TimberAppraisalFiles, h.TSRFiles, h.CobellAppraisalWave, h.LeaseTypes, h.ByUserId, h.EffDt, 
                         h.HighestAndBestUse, d.Id AS Appraisal_Detail_Id, d.AppraisalYear, d.AppraisalFiles, d.AppraisalPhotos, d.AppraisalComments, d.AppraisalStatus, d.AppraisalType, 
                         d.AppraisalLogNumber, d.AppraisalValue, d.AppraisalValuationDate, d.Appraiser, d.TypeOfTransaction, d.PartiesInvolved, d.AppraisalProjectType, d.RowId, 
                         d.RowStatusId, d.ByUserId AS Appraisal_Detail_ByUserId, d.QAStatusId, d.EffDt AS Appraisal_Detail_EffDt, d.RequestNumber, d.OtherPermitLeases, 
                         d.NwroComments, aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId, a.SourceId, a.LocationId, 
                         a.UserId AS Activity_UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, a.Id AS ActivityId, aq.QAStatusName
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Appraisal_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.Appraisal_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
GO
            ");
        }
    }
}
