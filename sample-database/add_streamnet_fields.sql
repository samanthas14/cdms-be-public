--creates the new nosa, rpers and sar fields for NPT streamnet
use CDMS_NPT;
begin TRAN T1;


ALTER TABLE [dbo].[StreamNet_NOSA_Detail] ADD [BestValue] [nvarchar](max)
ALTER TABLE [dbo].[StreamNet_NOSA_Detail] ADD [OtherDataSources] [nvarchar](max)
ALTER TABLE [dbo].[StreamNet_SAR_Detail] ADD [BestValue] [nvarchar](max)
ALTER TABLE [dbo].[StreamNet_SAR_Detail] ADD [OtherDataSources] [nvarchar](max)
ALTER TABLE [dbo].[StreamNet_SAR_Detail] ADD [SmoltDef] [nvarchar](max)
ALTER TABLE [dbo].[StreamNet_RperS_Detail] ADD [RecruitDef] [nvarchar](max)
ALTER TABLE [dbo].[StreamNet_RperS_Detail] ADD [BestValue] [nvarchar](max)
ALTER TABLE [dbo].[StreamNet_RperS_Detail] ADD [OtherDataSources] [nvarchar](max)

--update the views
--nosa
ALTER VIEW dbo.StreamNet_NOSA_detail_VW
AS
SELECT        *
FROM            dbo.StreamNet_NOSA_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.StreamNet_NOSA_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

ALTER view dbo.StreamNet_NOSA_vw
as
SELECT        d.Id, d.CommonName, d.Run, d.PopFit, d.WaterBody, d.SpawningYear, d.TRTmethod, d.ContactAgency, d.MethodNumber, d.NOSAIJ, d.NOSAEJ, d.Comment, 
                         d.NullRecord, d.DataStatus, d.ContactPersonFirst, d.ContactPersonLast, d.ContactPhone, d.ContactEmail, d.RowId, d.RowStatusId, d.ByUserId, d.QAStatusId, d.EffDt, 
                         d.ShadowId, d.Age10Prop, d.Age10PropLowerLimit, d.Age10PropUpperLimit, d.Age11PlusProp, d.Age11PlusPropLowerLimit, d.Age11PlusPropUpperLimit, d.Age2Prop,
                          d.Age2PropLowerLimit, d.Age2PropUpperLimit, d.Age3Prop, d.Age3PropLowerLimit, d.Age3PropUpperLimit, d.Age4Prop, d.Age4PropLowerLimit, 
                         d.Age4PropUpperLimit, d.Age5Prop, d.Age5PropLowerLimit, d.Age5PropUpperLimit, d.Age6Prop, d.Age6PropLowerLimit, d.Age6PropUpperLimit, d.Age7Prop, 
                         d.Age7PropLowerLimit, d.Age7PropUpperLimit, d.Age8Prop, d.Age8PropLowerLimit, d.Age8PropUpperLimit, d.Age9Prop, d.Age9PropLowerLimit, 
                         d.Age9PropUpperLimit, d.AgePropAlpha, d.CBFWApopName, d.Comments, d.CommonPopName, d.CompilerRecordID, d.DataEntry, d.DataEntryNotes, d.ESU_DPS, 
                         d.HOSJF, d.IndicatorLocation, d.LastUpdated, d.MajorPopGroup, d.MeasureLocation, d.MetaComments, d.MethodAdjustments, d.MetricLocation, 
                         d.NOBroodStockRemoved, d.NOSAEJAlpha, d.NOSAEJLowerLimit, d.NOSAEJUpperLimit, d.NOSAIJAlpha, d.NOSAIJLowerLimit, d.NOSAIJUpperLimit, d.NOSJF, 
                         d.NOSJFAlpha, d.NOSJFLowerLimit, d.NOSJFUpperLimit, d.PopFitNotes, d.PopID, d.ProtMethDocumentation, d.ProtMethName, d.ProtMethURL, d.Publish, 
                         d.RecoveryDomain, d.RefID, d.SubmitAgency, d.TSAEJ, d.TSAEJAlpha, d.TSAEJLowerLimit, d.TSAEJUpperLimit, d.TSAIJ, d.TSAIJAlpha, d.TSAIJLowerLimit, 
                         d.TSAIJUpperLimit, d.UpdDate, d.pHOSej, d.pHOSejAlpha, d.pHOSejLowerLimit, d.pHOSejUpperLimit, d.pHOSij, d.pHOSijAlpha, d.pHOSijLowerLimit, 
                         d.pHOSijUpperLimit, d.BestValue, d.OtherDataSources,
						 a.Id AS ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate
FROM            dbo.Activities AS a INNER JOIN
                         dbo.StreamNet_NOSA_Header_VW AS h ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.StreamNet_NOSA_detail_VW AS d ON a.Id = d.ActivityId

                         
--rpers
ALTER VIEW dbo.StreamNet_RperS_detail_VW
AS
SELECT        *
FROM            dbo.StreamNet_RperS_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.StreamNet_RperS_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
                               
ALTER view dbo.StreamNet_RperS_vw
as
SELECT        d.Id, d.CommonName, d.Run, d.PopFit, d.SpawnerLocation, d.RecruitLocation, d.BroodYear, d.RperStype, d.TRTmethod, d.ContactAgency, d.MethodNumber, d.RperS, 
                         d.Comments, d.NullRecord, d.DataStatus, d.ContactPersonFirst, d.ContactPersonLast, d.ContactPhone, d.ContactEmail, d.Age10Adults, d.Age11PlusAdults, 
                         d.Age1Juvs, d.Age2Adults, d.Age2Juvs, d.Age3Adults, d.Age3Juvs, d.Age4Adults, d.Age4PlusJuvs, d.Age5Adults, d.Age6Adults, d.Age7Adults, d.Age8Adults, 
                         d.Age9Adults, d.CBFWApopName, d.CommonPopName, d.CompilerRecordID, d.DataEntry, d.DataEntryNotes, d.ESU_DPS, d.HarvestAdj, d.HatcherySpawners, 
                         d.HatcherySpawnersAlpha, d.HatcherySpawnersLowerLimit, d.HatcherySpawnersUpperLimit, d.IndicatorLocation, d.LastUpdated, d.MainstemHarvest, 
                         d.MajorPopGroup, d.MeasureLocation, d.MetaComments, d.MethodAdjustments, d.MetricLocation, d.NOBroodStockRemoved, d.OceanHarvest, d.PopFitNotes, 
                         d.PopID, d.ProtMethDocumentation, d.ProtMethName, d.ProtMethURL, d.Publish, d.RecoveryDomain, d.Recruits, d.RecruitsAlpha, d.RecruitsLowerLimit, 
                         d.RecruitsMissing, d.RecruitsMissingExplanation, d.RecruitsUpperLimit, d.RefID, d.RperSAlpha, d.RperSLowerLimit, d.RperSUpperLimit, d.SubmitAgency, 
                         d.TotalSpawners, d.TotalSpawnersAlpha, d.TotalSpawnersLowerLimit, d.TotalSpawnersUpperLimit, d.TribHarvest, d.UpdDate, d.YOY, d.ShadowId, 
						 d.RecruitDef, d.BestValue, d.OtherDataSources,
						 a.Id AS ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate
FROM            dbo.Activities AS a INNER JOIN
                         dbo.StreamNet_RperS_Header_VW AS h ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.StreamNet_RperS_detail_VW AS d ON a.Id = d.ActivityId


--SAR
ALTER VIEW dbo.StreamNet_SAR_detail_VW
AS
SELECT        *
FROM            dbo.StreamNet_SAR_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.StreamNet_SAR_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)

ALTER view dbo.StreamNet_SAR_vw
as
SELECT        d.Id, d.CommonName, d.Run, d.PopFit, d.PopFitNotes, d.PopAggregation, d.SmoltLocation, d.AdultLocation, d.SARtype, d.OutmigrationYear, d.TRTmethod, 
                         d.ContactAgency, d.MethodNumber, d.SAR, d.RearingType, d.Comments, d.NullRecord, d.DataStatus, d.ContactPersonFirst, d.ContactPersonLast, d.ContactPhone, 
                         d.ContactEmail, d.RowId, d.RowStatusId, d.ByUserId, d.QAStatusId, d.EffDt, d.ShadowId, d.BroodStockRemoved, d.CBFWApopName, d.CommonPopName, 
                         d.CompilerRecordID, d.DataEntry, d.DataEntryNotes, d.ESU_DPS, d.HarvestAdj, d.IndicatorLocation, d.LastUpdated, d.MainstemHarvest, d.MajorPopGroup, 
                         d.MeasureLocation, d.MetaComments, d.MethodAdjustments, d.MetricLocation, d.OceanHarvest, d.PopID, d.ProtMethDocumentation, d.ProtMethName, 
                         d.ProtMethURL, d.Publish, d.RecoveryDomain, d.RefID, d.ReturnDef, d.ReturnsMissing, d.ReturnsMissingExplanation, d.SARAlpha, d.SARLowerLimit, 
                         d.SARUpperLimit, d.ScopeOfInference, d.SmoltLocPTcode, d.SubmitAgency, d.TAR, d.TARAlpha, d.TARLowerLimit, d.TARUpperLimit, d.TSO, d.TSOAlpha, 
                         d.TSOLowerLimit, d.TSOUpperLimit, d.TribHarvest, d.UpdDate, 
						 d.BestValue, d.OtherDataSources, d.SmoltDef,
						 a.Id AS ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate
FROM            dbo.Activities AS a INNER JOIN
                         dbo.StreamNet_SAR_Header_VW AS h ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.StreamNet_SAR_detail_VW AS d ON a.Id = d.ActivityId                               

-- This will info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    

INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo
--nosa fields
SELECT   
        FieldCategoryId = 24,  
        Name = 'BestValue',
        Description = 'A declaration of whether the ContactAgency considers this record to be their approved best estimate  for this combination of PopID and SpawningYear..',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '["Yes","No","Not specified"]',
        DbColumnName = 'BestValue',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 24, 
        Name = 'OtherDataSources',
        Description = 'This "OtherDataSources" field identifies additional organizations that provided data or expertise to calculate the indicator(s), metric(s), or age distribution for this record.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'OtherDataSources',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 24, 
        Name = 'SubmitAgency',
        Description = 'Initials or acronym for the agency, tribe, or other entity, or name of person that sent this record of data to the exchange network node at StreamNet.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '["NPT"]',
        DbColumnName = 'SubmitAgency',
        ControlType = 'select',
        [Rule] = '{"DefaultValue":"NPT"}'

--        
UNION ALL SELECT
        FieldCategoryId = 12,  
        Name = 'BestValue',
        Description = 'A declaration of whether the ContactAgency considers this record to be their approved best estimate  for this combination of PopID and SpawningYear..',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '["Yes","No","Not specified"]',
        DbColumnName = 'BestValue',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'OtherDataSources',
        Description = 'This "OtherDataSources" field identifies additional organizations that provided data or expertise to calculate the indicator(s), metric(s), or age distribution for this record.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'OtherDataSources',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'RecruitDef',
        Description = 'How "recruit" is defined for this R/S estimate.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'RecruitDef',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 14, 
        Name = 'BestValue',
        Description = 'A declaration of whether the ContactAgency considers this record to be their approved best estimate  for this combination of PopID and SpawningYear..',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '["Yes","No","Not specified"]',
        DbColumnName = 'BestValue',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 14, 
        Name = 'OtherDataSources',
        Description = 'This "OtherDataSources" field identifies additional organizations that provided data or expertise to calculate the indicator(s), metric(s), or age distribution for this record.  ',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'OtherDataSources',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 14, 
        Name = 'SmoltDef',
        Description = 'How the number of smolts is defined.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'SmoltDef',
        ControlType = 'text',
        [Rule] = NULL
        
drop table #NewFieldInfo

COMMIT TRAN T1;