namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdViewsForLocationLabel : DbMigration
    {
        public override void Up()
        {
            Sql(@"
--Metrics
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
GO
						 
--Artificial Production
DROP VIEW [dbo].[ArtificialProduction_VW]
GO
CREATE VIEW [dbo].[ArtificialProduction_VW]
AS
SELECT        h.Id AS ArtificialProduction_Header_Id, h.Comments, h.FieldSheetFile, h.ByUserId, h.EffDt, d.Id AS ArtificialProduction_Detail_Id, d.RunYear, d.Species, d.Origin, 
                         d.Sex, d.Disposition, d.TotalFishRepresented, d.LifeStage, REPLACE(REPLACE(d.FinClip, '""]', ''), '[""', '') AS FinClip, REPLACE(REPLACE(d.Tag, '""]', ''), '[""', '') AS Tag, 
                         d.NumberEggsTaken, d.ReleaseSite, d.FishComments, d.RowId, d.RowStatusId, d.ByUserId AS ArtificialProduction_Detail_ByUserId, d.ProgramGroup, d.QAStatusId,
                         d.EffDt AS ArtificialProduction_Detail_EffDt, aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId,
                         a.SourceId, a.LocationId, a.UserId AS Activity_UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, a.Id AS ActivityId, aq.QAStatusName, l.Label AS LocationLabel,
                         d.EyedEggs
FROM            dbo.Activities AS a INNER JOIN
                         dbo.ArtificialProduction_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.ArtificialProduction_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id
GO
						 
--WaterQuality
DROP VIEW [dbo].[WaterQuality_vw]
GO
CREATE VIEW [dbo].[WaterQuality_vw]
AS
SELECT        TOP (100) PERCENT loc.OtherAgencyId, d.SampleDate, d.CharacteristicName, d.Result, 
                         CASE d .ResultUnits WHEN 'FT' THEN 'ft' WHEN 'G/L' THEN 'g/L' WHEN 'INHG' THEN 'inHg' WHEN 'M' THEN 'm' WHEN 'MG/L' THEN 'mg/L' WHEN 'MG/M3' THEN 'mg/m3'
                          WHEN 'MMHG' THEN 'mmHg' WHEN 'NG/KG' THEN 'ng/kg' WHEN 'UG/KG' THEN 'ug/kg' WHEN 'UG/L' THEN 'ug/L' WHEN 'UMHO/CM' THEN 'umho/cm' WHEN 'CFU/100ML'
                          THEN 'cfu/100mL' WHEN 'NG/L' THEN 'ng/L' WHEN 'US/CM' THEN 'uS/cm' ELSE ResultUnits END AS ResultUnits, d.MdlResults, d.SampleFraction, 
                         d.MethodSpeciation, d.ContextID, d.MethodID, d.DetectionLimit, CASE WHEN (Result IS NULL) AND DataType = 'ISCO' THEN 'Ignore' WHEN (Result IS NULL) AND 
                         DataType = 'Handheld' THEN 'Not Reported' WHEN (Result IS NULL) AND MdlResults LIKE ('%>%') THEN 'Present Above Quantification Limit' WHEN (Result IS NULL)
                          AND MdlResults LIKE ('%<%') THEN 'Present Below Quantification Limit' WHEN (Result IS NULL) AND MdlResults LIKE ('%Present%') 
                         THEN 'Present Below Quantification Limit' WHEN (Result IS NULL) THEN 'Not Detected' ELSE '' END AS ResultDetectionCondition, d.LabDuplicate, d.LabName, 
                         d.SampleID, d.Comments, h.DataType, h.FieldsheetLink, h.HeaderComments, a.Id AS ActivityId, a.DatasetId, a.InstrumentId, a.ActivityDate, a.CreateDate, 
                         loc.Id AS LocationId, loc.Label AS LocationLabel, aq.QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId, aq.QAStatusId AS ActivityQAStatusId, d.RowId, d.RowStatusId, 
                         CASE DataType WHEN 'ISCO' THEN 'Field Msr/Obs-Portable Data Logger' WHEN 'Handheld' THEN 'Field Msr/Obs-Portable Data Logger' WHEN 'Lab' THEN 'Sample-Routine'
                          ELSE '' END AS ActivityType, 
                         CASE CharacteristicName WHEN 'Conductivity-TDS' THEN 'Calculated' WHEN 'Conductivity-Spec Cond' THEN 'Calculated' ELSE 'Actual' END AS ResultValueType, 
                         CASE d .ResultUnits WHEN 'FT' THEN 'ft' WHEN 'G/L' THEN 'g/L' WHEN 'INHG' THEN 'inHg' WHEN 'M' THEN 'm' WHEN 'MG/L' THEN 'mg/L' WHEN 'MG/M3' THEN 'mg/m3'
                          WHEN 'MMHG' THEN 'mmHg' WHEN 'NG/KG' THEN 'ng/kg' WHEN 'UG/KG' THEN 'ug/kg' WHEN 'UG/L' THEN 'ug/L' WHEN 'UMHO/CM' THEN 'umho/cm' WHEN 'CFU/100ML'
                          THEN 'cfu/100mL' WHEN 'NG/L' THEN 'ng/L' WHEN 'US/CM' THEN 'uS/cm' ELSE d .ResultUnits END AS ResultDetectionUnit
FROM            dbo.WaterQuality_Detail_VW AS d INNER JOIN
                         dbo.WaterQuality_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId INNER JOIN
                         dbo.Locations AS loc ON loc.Id = a.LocationId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
GO						 
						 
--Appraisals
DROP VIEW [dbo].[Appraisal_VW]
GO
CREATE VIEW [dbo].[Appraisal_VW]
AS
SELECT        h.Id AS Appraisal_Header_Id, h.Allotment, h.AllotmentStatus, h.AllotmentName, h.UpdatedTSRFile, h.AllotmentDescription, h.AllotmentComments, h.HasTimber, 
                         h.IsMappable, h.Acres, h.PriorityType, h.LegalDescription, h.AllotmentPhotoFiles, h.FarmingLeaseFiles, h.OtherPermitLeases, h.GrazingLeaseFiles, h.MapFiles, 
                         h.TimberAppraisalFiles, h.TSRFiles, h.CobellAppraisalWave, h.LeaseTypes, h.ByUserId, h.EffDt, d.Id AS Appraisal_Detail_Id, d.AppraisalYear, d.AppraisalFiles, 
                         d.AppraisalPhotos, d.AppraisalComments, d.AppraisalStatus, d.AppraisalType, d.AppraisalLogNumber, d.AppraisalValue, d.AppraisalValuationDate, d.Appraiser, 
                         d.TypeOfTransaction, d.PartiesInvolved, d.AppraisalProjectType, d.RowId, d.RowStatusId, d.ByUserId AS Appraisal_Detail_ByUserId, d.QAStatusId, 
                         d.EffDt AS Appraisal_Detail_EffDt, d.RequestNumber, d.NwroComments, d.RegionalOfficeReviewFiles, d.HighestAndBestUse, d.LastAppraisalRequestDate, 
                         aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId, a.SourceId, a.LocationId, l.Label AS LocationLabel,
                         a.UserId AS Activity_UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, a.Id AS ActivityId, aq.QAStatusName
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Appraisal_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.Appraisal_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
						 dbo.Locations AS l ON a.LocationId = l.Id
GO
						 
--CRPP Contracts
DROP VIEW [dbo].[CrppContracts_VW]
GO
CREATE VIEW [dbo].[CrppContracts_VW]
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, h.Id, h.ProjectProponent, h.ByUserId, h.EffDt, 
                         h.ProjectLead, h.CostCenter, h.ProjectName, h.Client, h.AgreeNumb, h.DateExec, h.DraftDue, h.FinalDue, h.ContractEnd, h.ModExtDate, h.DocumentLink, 
                         h.ActivityComments, h.AwardAmount, h.FinalReportSubmitted, d.Id AS CrppContracts_Detail_Id, d.Testing, d.NewSites, d.MonitoredSites, d.SitesEvaluated, 
                         d.UpdatedSites, d.NewIsolates, d.Evaluation, d.Monitoring, d.Notes, d.RowId, d.RowStatusId, d.ByUserId AS CrppContracts_Detail_ByUserId, d.QAStatusId, 
                         d.EffDt AS CrppContracts_Detail_EffDt, d.SurveyAcres, d.TestSites, d.SHRENum, d.SHCSNum, d.HPRCSIT, aq.QAStatusId AS ActivityQAStatusId, 
                         aq.UserId AS ActivityQAUserId, aq.Comments, aq.QAStatusName, l.Label AS LocationLabel
FROM            dbo.Activities AS a INNER JOIN
                         dbo.CrppContracts_Header_VW AS h ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.CrppContracts_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId INNER JOIN
						 dbo.Locations AS l ON a.LocationId = l.Id
GO
            ");
        }
        
        public override void Down()
        {
            Sql(@"
--Metrics
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
						 

--Artificial Production
DROP VIEW [dbo].[ArtificialProduction_VW]
GO
CREATE VIEW [dbo].[ArtificialProduction_VW]
AS
SELECT        h.Id AS ArtificialProduction_Header_Id, h.Comments, h.FieldSheetFile, h.ByUserId, h.EffDt, d.Id AS ArtificialProduction_Detail_Id, d.RunYear, d.Species, d.Origin, 
                         d.Sex, d.Disposition, d.TotalFishRepresented, d.LifeStage, REPLACE(REPLACE(d.FinClip, '""]', ''), '[""', '') AS FinClip, REPLACE(REPLACE(d.Tag, '""]', ''), '[""', '') AS Tag, 
                         d.NumberEggsTaken, d.ReleaseSite, d.FishComments, d.RowId, d.RowStatusId, d.ByUserId AS ArtificialProduction_Detail_ByUserId, d.ProgramGroup, d.QAStatusId,
                         d.EffDt AS ArtificialProduction_Detail_EffDt, aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId,
                         a.SourceId, a.LocationId, a.UserId AS Activity_UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, a.Id AS ActivityId, aq.QAStatusName, dbo.Locations.Label,
                         d.EyedEggs
FROM            dbo.Activities AS a INNER JOIN
                         dbo.ArtificialProduction_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.ArtificialProduction_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations ON a.LocationId = dbo.Locations.Id
						 
--WaterQuality
DROP VIEW [dbo].[WaterQuality_vw]
GO
CREATE VIEW [dbo].[WaterQuality_vw]
AS
SELECT        TOP (100) PERCENT loc.OtherAgencyId, d.SampleDate, d.CharacteristicName, d.Result, 
                         CASE d .ResultUnits WHEN 'FT' THEN 'ft' WHEN 'G/L' THEN 'g/L' WHEN 'INHG' THEN 'inHg' WHEN 'M' THEN 'm' WHEN 'MG/L' THEN 'mg/L' WHEN 'MG/M3' THEN 'mg/m3'
                          WHEN 'MMHG' THEN 'mmHg' WHEN 'NG/KG' THEN 'ng/kg' WHEN 'UG/KG' THEN 'ug/kg' WHEN 'UG/L' THEN 'ug/L' WHEN 'UMHO/CM' THEN 'umho/cm' WHEN 'CFU/100ML'
                          THEN 'cfu/100mL' WHEN 'NG/L' THEN 'ng/L' WHEN 'US/CM' THEN 'uS/cm' ELSE ResultUnits END AS ResultUnits, d.MdlResults, d.SampleFraction, 
                         d.MethodSpeciation, d.ContextID, d.MethodID, d.DetectionLimit, CASE WHEN (Result IS NULL) AND DataType = 'ISCO' THEN 'Ignore' WHEN (Result IS NULL) AND 
                         DataType = 'Handheld' THEN 'Not Reported' WHEN (Result IS NULL) AND MdlResults LIKE ('%>%') THEN 'Present Above Quantification Limit' WHEN (Result IS NULL)
                          AND MdlResults LIKE ('%<%') THEN 'Present Below Quantification Limit' WHEN (Result IS NULL) AND MdlResults LIKE ('%Present%') 
                         THEN 'Present Below Quantification Limit' WHEN (Result IS NULL) THEN 'Not Detected' ELSE '' END AS ResultDetectionCondition, d.LabDuplicate, d.LabName, 
                         d.SampleID, d.Comments, h.DataType, h.FieldsheetLink, h.HeaderComments, a.Id AS ActivityId, a.DatasetId, a.InstrumentId, a.ActivityDate, a.CreateDate, 
                         loc.Id AS LocationId, aq.QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId, aq.QAStatusId AS ActivityQAStatusId, d.RowId, d.RowStatusId, 
                         CASE DataType WHEN 'ISCO' THEN 'Field Msr/Obs-Portable Data Logger' WHEN 'Handheld' THEN 'Field Msr/Obs-Portable Data Logger' WHEN 'Lab' THEN 'Sample-Routine'
                          ELSE '' END AS ActivityType, 
                         CASE CharacteristicName WHEN 'Conductivity-TDS' THEN 'Calculated' WHEN 'Conductivity-Spec Cond' THEN 'Calculated' ELSE 'Actual' END AS ResultValueType, 
                         CASE d .ResultUnits WHEN 'FT' THEN 'ft' WHEN 'G/L' THEN 'g/L' WHEN 'INHG' THEN 'inHg' WHEN 'M' THEN 'm' WHEN 'MG/L' THEN 'mg/L' WHEN 'MG/M3' THEN 'mg/m3'
                          WHEN 'MMHG' THEN 'mmHg' WHEN 'NG/KG' THEN 'ng/kg' WHEN 'UG/KG' THEN 'ug/kg' WHEN 'UG/L' THEN 'ug/L' WHEN 'UMHO/CM' THEN 'umho/cm' WHEN 'CFU/100ML'
                          THEN 'cfu/100mL' WHEN 'NG/L' THEN 'ng/L' WHEN 'US/CM' THEN 'uS/cm' ELSE d .ResultUnits END AS ResultDetectionUnit
FROM            dbo.WaterQuality_Detail_VW AS d INNER JOIN
                         dbo.WaterQuality_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId INNER JOIN
                         dbo.Locations AS loc ON loc.Id = a.LocationId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
GO
						 
--Appraisals
DROP VIEW [dbo].[Appraisal_VW]
GO
CREATE VIEW [dbo].[Appraisal_VW]
AS
SELECT        h.Id AS Appraisal_Header_Id, h.Allotment, h.AllotmentStatus, h.AllotmentName, h.UpdatedTSRFile, h.AllotmentDescription, h.AllotmentComments, h.HasTimber, 
                         h.IsMappable, h.Acres, h.PriorityType, h.LegalDescription, h.AllotmentPhotoFiles, h.FarmingLeaseFiles, h.OtherPermitLeases, h.GrazingLeaseFiles, h.MapFiles, 
                         h.TimberAppraisalFiles, h.TSRFiles, h.CobellAppraisalWave, h.LeaseTypes, h.ByUserId, h.EffDt, d.Id AS Appraisal_Detail_Id, d.AppraisalYear, d.AppraisalFiles, 
                         d.AppraisalPhotos, d.AppraisalComments, d.AppraisalStatus, d.AppraisalType, d.AppraisalLogNumber, d.AppraisalValue, d.AppraisalValuationDate, d.Appraiser, 
                         d.TypeOfTransaction, d.PartiesInvolved, d.AppraisalProjectType, d.RowId, d.RowStatusId, d.ByUserId AS Appraisal_Detail_ByUserId, d.QAStatusId, 
                         d.EffDt AS Appraisal_Detail_EffDt, d.RequestNumber, d.NwroComments, d.RegionalOfficeReviewFiles, d.HighestAndBestUse, d.LastAppraisalRequestDate, 
                         aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId, a.SourceId, a.LocationId, 
                         a.UserId AS Activity_UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, a.Id AS ActivityId, aq.QAStatusName
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Appraisal_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.Appraisal_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
GO
						 
--CRPP Contracts
DROP VIEW [dbo].[CrppContracts_VW]
GO
CREATE VIEW [dbo].[CrppContracts_VW]
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, h.Id, h.ProjectProponent, h.ByUserId, h.EffDt, 
                         h.ProjectLead, h.CostCenter, h.ProjectName, h.Client, h.AgreeNumb, h.DateExec, h.DraftDue, h.FinalDue, h.ContractEnd, h.ModExtDate, h.DocumentLink, 
                         h.ActivityComments, h.AwardAmount, h.FinalReportSubmitted, d.Id AS CrppContracts_Detail_Id, d.Testing, d.NewSites, d.MonitoredSites, d.SitesEvaluated, 
                         d.UpdatedSites, d.NewIsolates, d.Evaluation, d.Monitoring, d.Notes, d.RowId, d.RowStatusId, d.ByUserId AS CrppContracts_Detail_ByUserId, d.QAStatusId, 
                         d.EffDt AS CrppContracts_Detail_EffDt, d.SurveyAcres, d.TestSites, d.SHRENum, d.SHCSNum, d.HPRCSIT, aq.QAStatusId AS ActivityQAStatusId, 
                         aq.UserId AS ActivityQAUserId, aq.Comments, aq.QAStatusName
FROM            dbo.Activities AS a INNER JOIN
                         dbo.CrppContracts_Header_VW AS h ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.CrppContracts_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId
GO
            ");
        }
    }
}
