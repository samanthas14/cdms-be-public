namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWatQualViews : DbMigration
    {
        public override void Up()
        {
            Sql(@"
drop view [dbo].[WaterQuality_Detail_VW]
go

CREATE VIEW [dbo].[WaterQuality_Detail_VW]
AS
SELECT        Id, CharacteristicName, Result, ResultUnits, LabDuplicate, Comments, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, MdlResults, SampleDate, 
                         SampleID, SampleFraction, MethodSpeciation, DetectionLimit, ContextID, MethodID, LabName
FROM            dbo.WaterQuality_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.WaterQuality_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
go

drop view [dbo].[WaterQuality_Header_VW]
go

create view [dbo].[WaterQuality_Header_VW]
as
SELECT        Id, DataType, ActivityId, ByUserId, EffDt, FieldsheetLink, HeaderComments
FROM            dbo.WaterQuality_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.WaterQuality_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
go

drop view [dbo].[WaterQuality_vw]
go

create view [dbo].[WaterQuality_vw]
as
SELECT        TOP (100) PERCENT loc.OtherAgencyId, d.SampleDate, d.CharacteristicName, d.Result, 
                         CASE d .ResultUnits WHEN 'FT' THEN 'ft' WHEN 'G/L' THEN 'g/L' WHEN 'INHG' THEN 'inHg' WHEN 'M' THEN 'm' WHEN 'MG/L' THEN 'mg/L' WHEN 'MG/M3' THEN 'mg/m3'
                          WHEN 'MMHG' THEN 'mmHg' WHEN 'NG/KG' THEN 'ng/kg' WHEN 'UG/KG' THEN 'ug/kg' WHEN 'UG/L' THEN 'ug/L' WHEN 'UMHO/CM' THEN 'umho/cm (micro mho/cm)'
                          WHEN 'CFU/100ML' THEN 'cfu/100mL' WHEN 'NG/L' THEN 'ng/L' WHEN 'US/CM' THEN 'uS/cm' ELSE ResultUnits END AS ResultUnits, d.MdlResults, 
                         d.SampleFraction, d.MethodSpeciation, d.ContextID, d.MethodID, d.DetectionLimit, CASE WHEN (Result IS NULL) AND 
                         DataType = 'ISCO' THEN 'Ignore' WHEN (Result IS NULL) AND DataType = 'Handheld' THEN 'Not Reported' WHEN (Result IS NULL) AND MdlResults LIKE ('%>%') 
                         THEN 'Present Above Quantification Limit' WHEN (Result IS NULL) AND MdlResults LIKE ('%<%') THEN 'Present Below Quantification Limit' WHEN (Result IS NULL) 
                         AND MdlResults LIKE ('%Present%') THEN 'Present Below Quantification Limit' WHEN (Result IS NULL) 
                         THEN 'Not Detected' ELSE '' END AS ResultDetectionCondition, d.LabDuplicate, d.LabName, d.SampleID, d.Comments, h.DataType, h.FieldsheetLink, 
                         h.HeaderComments, a.Id AS ActivityId, a.DatasetId, a.InstrumentId, a.ActivityDate, a.CreateDate, loc.Id AS LocationId, aq.QAStatusName, 
						 aq.Comments AS ActivityQAComments, aq.QAStatusId, aq.QAStatusId AS ActivityQAStatusId, d.RowId, d.RowStatusId, 
                         CASE DataType WHEN 'ISCO' THEN 'Field Msr/Obs-Portable Data Logger' WHEN 'Handheld' THEN 'Field Msr/Obs-Portable Data Logger' WHEN 'Lab' THEN 'Sample-Routine'
                          ELSE '' END AS ActivityType, 
                         CASE CharacteristicName WHEN 'Conductivity-TDS' THEN 'Calculated' WHEN 'Conductivity-Spec Cond' THEN 'Calculated' ELSE 'Actual' END AS ResultValueType, 
                         CASE d .ResultUnits WHEN 'FT' THEN 'ft' WHEN 'G/L' THEN 'g/L' WHEN 'INHG' THEN 'inHg' WHEN 'M' THEN 'm' WHEN 'MG/L' THEN 'mg/L' WHEN 'MG/M3' THEN 'mg/m3'
                          WHEN 'MMHG' THEN 'mmHg' WHEN 'NG/KG' THEN 'ng/kg' WHEN 'UG/KG' THEN 'ug/kg' WHEN 'UG/L' THEN 'ug/L' WHEN 'UMHO/CM' THEN 'umho/cm (micro mho/cm)'
                          WHEN 'CFU/100ML' THEN 'cfu/100mL' WHEN 'NG/L' THEN 'ng/L' WHEN 'US/CM' THEN 'uS/cm' ELSE d .ResultUnits END AS ResultDetectionUnit
FROM            dbo.WaterQuality_Detail_VW AS d INNER JOIN
                         dbo.WaterQuality_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId INNER JOIN
                         dbo.Locations AS loc ON loc.Id = a.LocationId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
go
            ");
        }
        
        public override void Down()
        {
            Sql(@"
drop view [dbo].[WaterQuality_Detail_VW]
go

CREATE VIEW [dbo].[WaterQuality_Detail_VW]
AS
SELECT        Id, CharacteristicName, Result, ResultUnits, LabDuplicate, Comments, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt, MdlResults, SampleDate, 
                         SampleID, SampleFraction, MethodSpeciation, DetectionLimit, ContextID, MethodID, LabName
FROM            dbo.WaterQuality_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.WaterQuality_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
go

drop view [dbo].[WaterQuality_Header_VW]
go

create view [dbo].[WaterQuality_Header_VW]
as
SELECT        Id, DataType, ActivityId, ByUserId, EffDt, FieldsheetLink
FROM            dbo.WaterQuality_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.WaterQuality_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
go

drop view [dbo].[WaterQuality_vw]
go

create view [dbo].[WaterQuality_vw]
as
SELECT        TOP (100) PERCENT loc.OtherAgencyId, d.SampleDate, d.CharacteristicName, d.Result, 
                         CASE d .ResultUnits WHEN 'FT' THEN 'ft' WHEN 'G/L' THEN 'g/L' WHEN 'INHG' THEN 'inHg' WHEN 'M' THEN 'm' WHEN 'MG/L' THEN 'mg/L' WHEN 'MG/M3' THEN 'mg/m3'
                          WHEN 'MMHG' THEN 'mmHg' WHEN 'NG/KG' THEN 'ng/kg' WHEN 'UG/KG' THEN 'ug/kg' WHEN 'UG/L' THEN 'ug/L' WHEN 'UMHO/CM' THEN 'umho/cm (micro mho/cm)'
                          WHEN 'CFU/100ML' THEN 'cfu/100mL' WHEN 'NG/L' THEN 'ng/L' WHEN 'US/CM' THEN 'uS/cm' ELSE ResultUnits END AS ResultUnits, d.MdlResults, 
                         d.SampleFraction, d.MethodSpeciation, d.ContextID, d.MethodID, d.DetectionLimit, CASE WHEN (Result IS NULL) AND 
                         DataType = 'ISCO' THEN 'Ignore' WHEN (Result IS NULL) AND DataType = 'Handheld' THEN 'Not Reported' WHEN (Result IS NULL) AND MdlResults LIKE ('%>%') 
                         THEN 'Present Above Quantification Limit' WHEN (Result IS NULL) AND MdlResults LIKE ('%<%') THEN 'Present Below Quantification Limit' WHEN (Result IS NULL) 
                         AND MdlResults LIKE ('%Present%') THEN 'Present Below Quantification Limit' WHEN (Result IS NULL) 
                         THEN 'Not Detected' ELSE '' END AS ResultDetectionCondition, d.LabDuplicate, d.LabName, d.SampleID, d.Comments, h.DataType, h.FieldsheetLink, 
                         a.Id AS ActivityId, a.DatasetId, a.InstrumentId, a.ActivityDate, a.CreateDate, loc.Id AS LocationId, aq.QAStatusName, aq.Comments AS ActivityQAComments, 
                         aq.QAStatusId, aq.QAStatusId AS ActivityQAStatusId, d.RowId, d.RowStatusId, 
                         CASE DataType WHEN 'ISCO' THEN 'Field Msr/Obs-Portable Data Logger' WHEN 'Handheld' THEN 'Field Msr/Obs-Portable Data Logger' WHEN 'Lab' THEN 'Sample-Routine'
                          ELSE '' END AS ActivityType, 
                         CASE CharacteristicName WHEN 'Conductivity-TDS' THEN 'Calculated' WHEN 'Conductivity-Spec Cond' THEN 'Calculated' ELSE 'Actual' END AS ResultValueType, 
                         CASE d .ResultUnits WHEN 'FT' THEN 'ft' WHEN 'G/L' THEN 'g/L' WHEN 'INHG' THEN 'inHg' WHEN 'M' THEN 'm' WHEN 'MG/L' THEN 'mg/L' WHEN 'MG/M3' THEN 'mg/m3'
                          WHEN 'MMHG' THEN 'mmHg' WHEN 'NG/KG' THEN 'ng/kg' WHEN 'UG/KG' THEN 'ug/kg' WHEN 'UG/L' THEN 'ug/L' WHEN 'UMHO/CM' THEN 'umho/cm (micro mho/cm)'
                          WHEN 'CFU/100ML' THEN 'cfu/100mL' WHEN 'NG/L' THEN 'ng/L' WHEN 'US/CM' THEN 'uS/cm' ELSE d .ResultUnits END AS ResultDetectionUnit
FROM            dbo.WaterQuality_Detail_VW AS d INNER JOIN
                         dbo.WaterQuality_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId INNER JOIN
                         dbo.Locations AS loc ON loc.Id = a.LocationId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
go
            ");
        }
    }
}
