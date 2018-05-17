--update the view
--rpers (has to be run separately, btw)
                               
ALTER view dbo.StreamNet_RperS_vw
as
SELECT   
d.[Id]
      ,d.[CommonName]
      ,d.[Run]
      ,d.[PopFit]
      ,d.[SpawnerLocation]
      ,d.[RecruitLocation]
      ,d.[BroodYear]
      ,d.[RperStype]
      ,d.[TRTmethod]
      ,d.[ContactAgency]
      ,d.[MethodNumber]
      ,d.[RperS]
      ,d.[Comments]
      ,d.[NullRecord]
      ,d.[DataStatus]
      ,d.[ContactPersonFirst]
      ,d.[ContactPersonLast]
      ,d.[ContactPhone]
      ,d.[ContactEmail]
      ,d.[Age10Adults]
      ,d.[Age11PlusAdults]
      ,d.[Age1Juvs]
      ,d.[Age2Adults]
      ,d.[Age2Juvs]
      ,d.[Age3Adults]
      ,d.[Age3Juvs]
      ,d.[Age4Adults]
      ,d.[Age4PlusJuvs]
      ,d.[Age5Adults]
      ,d.[Age6Adults]
      ,d.[Age7Adults]
      ,d.[Age8Adults]
      ,d.[Age9Adults]
      ,d.[CBFWApopName]
      ,d.[CommonPopName]
      ,d.[CompilerRecordID]
      ,d.[DataEntry]
      ,d.[DataEntryNotes]
      ,d.[ESU_DPS]
      ,d.[HarvestAdj]
      ,d.[HatcherySpawners]
      ,d.[HatcherySpawnersAlpha]
      ,d.[HatcherySpawnersLowerLimit]
      ,d.[HatcherySpawnersUpperLimit]
      ,d.[IndicatorLocation]
      ,d.[LastUpdated]
      ,d.[MainstemHarvest]
      ,d.[MajorPopGroup]
      ,d.[MeasureLocation]
      ,d.[MetaComments]
      ,d.[MethodAdjustments]
      ,d.[MetricLocation]
      ,d.[NOBroodStockRemoved]
      ,d.[OceanHarvest]
      ,d.[PopFitNotes]
      ,d.[PopID]
      ,d.[ProtMethDocumentation]
      ,d.[ProtMethName]
      ,d.[ProtMethURL]
      ,d.[Publish]
      ,d.[RecoveryDomain]
      ,d.[Recruits]
      ,d.[RecruitsAlpha]
      ,d.[RecruitsLowerLimit]
      ,d.[RecruitsMissing]
      ,d.[RecruitsMissingExplanation]
      ,d.[RecruitsUpperLimit]
      ,d.[RefID]
      ,d.[RperSAlpha]
      ,d.[RperSLowerLimit]
      ,d.[RperSUpperLimit]
      ,d.[SubmitAgency]
      ,d.[TotalSpawners]
      ,d.[TotalSpawnersAlpha]
      ,d.[TotalSpawnersLowerLimit]
      ,d.[TotalSpawnersUpperLimit]
      ,d.[TribHarvest]
      ,d.[UpdDate]
      ,d.[YOY]
      ,d.[ShadowId]
      ,d.[RecruitDef]
      ,d.[BestValue]
      ,d.[OtherDataSources]
, a.Id AS ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate
FROM            dbo.Activities AS a INNER JOIN
                         dbo.StreamNet_RperS_Header_VW AS h ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.StreamNet_RperS_detail_VW AS d ON a.Id = d.ActivityId



--creates the new nosa, rpers and sar fields for NPT streamnet
use CDMS_NPT;
begin TRAN T1;

-- This will info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    

INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo
--nosa fields
SELECT   
        FieldCategoryId = 12,  
        Name = 'Age1Juvs',
        Description = 'Total number of juvenile recruits (parr or smolts) at age 1 (brood year +1).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age1Juvs',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12,  
        Name = 'Age2Juvs',
        Description = 'Total number of juvenile recruits at age 2 (brood year +2).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age2Juvs',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12,  
        Name = 'Age3Juvs',
        Description = 'Total number of juvenile recruits at age 3 (brood year +3).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age3Juvs',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12,  
        Name = 'Age4PlusJuvs',
        Description = 'Total number of juvenile recruits at age 4 (brood year +4) or older.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age4PlusJuvs',
        ControlType = 'number',
        [Rule] = NULL
        
UNION ALL SELECT
        FieldCategoryId = 12,  
        Name = 'Age2Adults',
        Description = 'Total number of adult recruits that recruited at age 2 (brood year +2).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age2Adults',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12,  
        Name = 'Age3Adults',
        Description = 'Total number of adult recruits that recruited at age 3 (brood year +3).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age3Adults',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12,  
        Name = 'Age4Adults',
        Description = 'Total number of adult recruits that recruited at age 4 (brood year +4).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age4Adults',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12,  
        Name = 'Age5Adults',
        Description = 'Total number of adult recruits that recruited at age 5 (brood year +5).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age5Adults',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12,  
        Name = 'Age6Adults',
        Description = 'Total number of adult recruits that recruited at age 6 (brood year +6).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age6Adults',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12,  
        Name = 'Age7Adults',
        Description = 'Total number of adult recruits that recruited at age 7 (brood year +7).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age7Adults',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12,  
        Name = 'Age8Adults',
        Description = 'Total number of adult recruits that recruited at age 8 (brood year +8).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age8Adults',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12,  
        Name = 'Age9Adults',
        Description = 'Total number of adult recruits that recruited at age 9 (brood year +9).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age9Adults',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12,  
        Name = 'Age10Adults',
        Description = 'Total number of adult recruits that recruited at age 10 (brood year +10).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age10Adults',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12,  
        Name = 'Age11PlusAdults',
        Description = 'Total number of adult recruits that recruited at age 11 (brood year +11) or older.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age11PlusAdults',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'CBFWApopName',
        Description = 'CBFWApopName	Population name as defined by CBFWA for subbasin planning purposes, from subbasin plans and agencies.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'CBFWApopName',
        ControlType = 'text',
        [Rule] = NULL
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'CommonPopName',
        Description = 'Population name used by local biologists.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'CommonPopName',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'PopFitNotes',
        Description = 'Text description of how well the natural origin spawner abundance value corresponds to the defined population.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'PopFitNotes',
        ControlType = 'text',
        [Rule] = NULL
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'CompilerRecordID',
        Description = 'Agency record ID maintained by the data submitter.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'CompilerRecordID',
        ControlType = 'text',
        [Rule] = NULL
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'DataEntry',
        Description = 'Compiler''s name.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'DataEntry',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'DataEntryNotes',
        Description = 'Notes about this record by the compiler identified in the "DataEntry" field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'DataEntryNotes',
        ControlType = 'text',
        [Rule] = NULL      
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'ESU_DPS',
        Description = 'Evolutionarily Significant Unit (ESU) or Distinct Population Segment (DPS)',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '["ESU","DPS"]',
        DbColumnName = 'ESU_DPS',
        ControlType = 'select',
        [Rule] = NULL  
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'HarvestAdj',
        Description = 'For adult returns, how was the return adjusted to account for harvest?',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'HarvestAdj',
        ControlType = 'text',
        [Rule] = NULL         
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'HatcherySpawners',
        Description = 'Point estimate for the number of parent spawners of hatchery origin that contributed to the brood year this record reflects.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'HatcherySpawners',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'HatcherySpawnersAlpha',
        Description = 'The significance level for the HatcherySpawners confidence interval, expressed as alpha.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'HatcherySpawnersAlpha',
        ControlType = 'number',
        [Rule] = NULL        
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'HatcherySpawnersLowerLimit',
        Description = 'The lower limit of the confidence interval for the HatcherySpawners field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'HatcherySpawnersLowerLimit',
        ControlType = 'number',
        [Rule] = NULL
		
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'HatcherySpawnersUpperLimit',
        Description = 'The upper limit of the confidence interval for the HatcherySpawners field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'HatcherySpawnersUpperLimit',
        ControlType = 'number',
        [Rule] = NULL        
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'IndicatorLocation',
        Description = 'Where this indicator is maintained at the source.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'IndicatorLocation',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'MainstemHarvest',
        Description = 'The estimated number of adults and jacks from the recruit group indicated in the Recruits field that were harvested in the mainstem (including the estuary).',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'MainstemHarvest',
        ControlType = 'text',
        [Rule] = NULL
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'MeasureLocation',
        Description = 'Where the measurements are maintained that were used for these calculations.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'MeasureLocation',
        ControlType = 'text',
        [Rule] = NULL

 UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'MajorPopGroup',
        Description = 'Name of "major population group" (MPG) or "stratum" as defined by the NMFS Northwest Region, in which the population falls.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'MajorPopGroup',
        ControlType = 'text',
        [Rule] = NULL
       
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'MetaComments',
        Description = 'Comments regarding the supporting information.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'MetaComments',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'MethodAdjustments',
        Description = 'Minor adjustments to a method in a given year that are not described in the method citations above but are important.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'MethodAdjustments',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'MetricLocation',
        Description = 'Where the supporting metrics are maintained at the source.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'MetricLocation',
        ControlType = 'text',
        [Rule] = NULL
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'NOBroodStockRemoved',
        Description = 'The number of additional natural origin fish (adults plus jacks) that would have spawned, had there not been removal of natural origin fish for use as hatchery broodstock.',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'NOBroodStockRemoved',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'OceanHarvest',
        Description = 'The estimated number of adults and jacks from the recruit group indicated in the Recruits field that were harvested in the ocean.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'OceanHarvest',
        ControlType = 'text',
        [Rule] = NULL        
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'PopID',
        Description = 'Code for the TRT population of fish represented by this record.',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'PopID',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'ProtMethName',
        Description = 'The name(s) of all protocols and associated data collection and data analysis methods used to calculate the indicator estimate.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ProtMethName',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'ProtMethURL',
        Description = 'URL(s) for published protocols and methods describing the methodology and documenting the derivation of the indicator.  If published in MonitoringMethods.org, this link will provide access to study design information and all methods associated with the protocol.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ProtMethURL',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'ProtMethDocumentation',
        Description = 'Citation or documentation that describes the protocol and/or method(s) listed in the ProtMethName field.  Include references not documented in MonitoringMethods.org, such as reports, journal articles or other publications that describe the survey design, field methodology and analytical approach used to derive the indicator estimate.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ProtMethDocumentation',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'RecoveryDomain',
        Description = 'Name of the "recovery domain," as defined by the NMFS Northwest Region, in which the population falls.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '["Puget Sound Recovery Domain", "Wilamette/Lower Columbia Recovery Domain", "Interior Columbia Recovery Domain","Oregon Coast Recovery Domain","Southern Oregon/Northern California Coast Recovery Domain"]',
        DbColumnName = 'RecoveryDomain',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'Recruits',
        Description = 'Point estimate for the number of parent spawners of hatchery origin that contributed to the brood year this record reflects.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Recruits',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'RecruitsAlpha',
        Description = 'The significance level for the Recruits confidence interval, expressed as alpha.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'RecruitsAlpha',
        ControlType = 'number',
        [Rule] = NULL        
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'RecruitsLowerLimit',
        Description = 'The lower limit of the confidence interval for the Recruits field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'RecruitsLowerLimit',
        ControlType = 'number',
        [Rule] = NULL
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'RecruitsMissing',
        Description = 'This field indicates whether any recruits for this brood year were missing.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'RecruitsMissing',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'RecruitsMissingExplanation',
        Description = 'If some recruits data are not accounted for in the RperS estimate, explain the gap.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'RecruitsMissingExplanation',
        ControlType = 'number',
        [Rule] = NULL
						
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'RecruitsUpperLimit',
        Description = 'The upper limit of the confidence interval for the Recruits field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'RecruitsUpperLimit',
        ControlType = 'number',
        [Rule] = NULL         

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'RefID',
        Description = 'The unique StreamNet reference ID number that identifies the source document or database from which the record was obtained.',
        Units = NULL,
        Validation = NULL,
        DataType = 'long',
        PossibleValues = NULL,
        DbColumnName = 'RefID',
        ControlType = 'number',
        [Rule] = NULL
        
        
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'RperSAlpha',
        Description = 'The significance level for the RperS confidence interval, expressed as alpha.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'RperSAlpha',
        ControlType = 'number',
        [Rule] = NULL        
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'RperSLowerLimit',
        Description = 'The lower limit of the confidence interval for the RperS field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'RperSLowerLimit',
        ControlType = 'number',
        [Rule] = NULL
						
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'RperSUpperLimit',
        Description = 'The upper limit of the confidence interval for the RperS field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'RperSUpperLimit',
        ControlType = 'number',
        [Rule] = NULL         

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'TotalSpawners',
        Description = 'Point estimate for the number of parent spawners of hatchery origin that contributed to the brood year this record reflects.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TotalSpawners',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'TotalSpawnersAlpha',
        Description = 'The significance level for the TotalSpawners confidence interval, expressed as alpha.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TotalSpawnersAlpha',
        ControlType = 'number',
        [Rule] = NULL        
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'TotalSpawnersLowerLimit',
        Description = 'The lower limit of the confidence interval for the TotalSpawners field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TotalSpawnersLowerLimit',
        ControlType = 'number',
        [Rule] = NULL
		
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'TotalSpawnersUpperLimit',
        Description = 'The upper limit of the confidence interval for the TotalSpawners field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TotalSpawnersUpperLimit',
        ControlType = 'number',
        [Rule] = NULL        

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'TribHarvest',
        Description = 'The estimated number of adults and jacks from the recruit group indicated in the Recruits field that were harvested in tributaries.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'TribHarvest',
        ControlType = 'text',
        [Rule] = NULL
        
UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'YOY',
        Description = 'Total number of juvenile recruits (parr or smolts) at age 0 (brood year +0).',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'YOY',
        ControlType = 'text',
        [Rule] = NULL        

UNION ALL SELECT
        FieldCategoryId = 12, 
        Name = 'UpdDate',
        Description = 'The date and time that the record was created or updated.  For data obtained in electronic format from another source it can reflect the date and time of data capture or of conversion to Coordinated Assessment/StreamNet standards.',
        Units = NULL,
        Validation = NULL,
        DataType = 'date',
        PossibleValues = NULL,
        DbColumnName = 'UpdDate',
        ControlType = 'datetime',
        [Rule] = NULL


        
drop table #NewFieldInfo

COMMIT TRAN T1;
	

    