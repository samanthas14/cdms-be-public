namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DecdAvUpdMoveFields2 : DbMigration
    {
        public override void Up()
        {
            Sql(@"
-- Create tmp views for the job
CREATE VIEW [dbo].[tmpAppraisal_Detail_VW]
AS
SELECT        Id, ActivityId, EffDt, OtherPermitLeases, RegionalOfficeReviewFiles, HighestAndBestUse, LastAppraisalRequestDate
FROM            dbo.Appraisal_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Appraisal_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
GO


CREATE VIEW [dbo].[tmpAppraisal_Header_VW]
AS
SELECT        Id, ActivityId, Allotment, RegionalOfficeReviewFiles, HighestAndBestUse, LastAppraisalRequestDate, OtherPermitLeases, EffDt
FROM            dbo.Appraisal_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Appraisal_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO


CREATE VIEW [dbo].[tmpAppraisal_VW]
AS
SELECT			a.Id AS ActivityId,
				h.Id AS hId, h.Allotment, 
				h.RegionalOfficeReviewFiles as hRegionalOfficeReviewFiles, 
				h.HighestAndBestUse as hHighestAndBestUse, 
				h.LastAppraisalRequestDate as hLastAppraisalRequestDate,  
				h.OtherPermitLeases as hOtherPermitLeases, 
				h.EffDt as hEffDt, 
                d.Id AS dId, d.EffDt AS dEffDt, 
				d.RegionalOfficeReviewFiles as dRegionalOfficeReviewFiles, 
				d.HighestAndBestUse as dHighestAndBestUse,
				d.LastAppraisalRequestDate as dLastAppraisalRequestDate, 
				d.OtherPermitLeases as dOtherPermitLeases
                
FROM            dbo.Activities AS a INNER JOIN
                         dbo.tmpAppraisal_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.tmpAppraisal_Detail_VW AS d ON d.ActivityId = a.Id --INNER JOIN
                         --dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
GO


-- Copy detail data to header -- Unnecesary because OtherPermitLeases is new and null.

-- Copy Header data to detail
update dbo.Appraisal_Detail
set RegionalOfficeReviewFiles = r.hRegionalOfficeReviewFiles,
HighestAndBestUse = r.hHighestAndBestUse,
LastAppraisalRequestDate = r.hLastAppraisalRequestDate
from
(
select ActivityId, Allotment,
hId, dId,
hRegionalOfficeReviewFiles, dRegionalOfficeReviewFiles,
hHighestAndBestUse, dHighestAndBestUse,
hLastAppraisalRequestDate, dLastAppraisalRequestDate,
hOtherPermitLeases, dOtherPermitLeases,
hEffDt, dEffDt
from tmpAppraisal_VW
) as r
where dbo.Appraisal_Detail.ActivityId = r.ActivityId
and dbo.Appraisal_Detail.Id = r.dId

            ");
        }

        public override void Down()
        {
            Sql(@"

            ");
        }
    }
}
