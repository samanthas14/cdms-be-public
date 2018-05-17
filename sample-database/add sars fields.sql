--update the view
--SAR (has to be run separately, btw)
                               


--creates the new nosa, SAR and sar fields for NPT streamnet
use CDMS_NPT;
begin TRAN T1;

-- This will info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    

INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo
--sar fields

SELECT
        FieldCategoryId = 14, 
        Name = 'BroodStockRemoved',
        Description = 'The number of additional fish that would have returned from the smolt group indicated in the TSO field, had there not been removal of fish for use as broodstock in a hatchery.  The value of the SARtype field determines whether females, males, and jacks are included here.',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'BroodStockRemoved',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
        Name = 'SARAlpha',
        Description = 'The significance level for the SAR confidence interval, expressed as alpha.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'SARAlpha',
        ControlType = 'number',
        [Rule] = NULL        
        
UNION ALL SELECT
        FieldCategoryId = 14, 
        Name = 'SARLowerLimit',
        Description = 'The lower limit of the confidence interval for the SAR field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'SARLowerLimit',
        ControlType = 'number',
        [Rule] = NULL
		
UNION ALL SELECT
        FieldCategoryId = 14, 
        Name = 'SARUpperLimit',
        Description = 'The upper limit of the confidence interval for the SAR field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'SARUpperLimit',
        ControlType = 'number',
        [Rule] = NULL        
        
UNION ALL SELECT
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
        Name = 'TAR',
        Description = 'Total adult return.  Point estimate of the number of adults returning for the first time from the indicated outmigration year, or the group of marked smolts (as appropriate), to match the outmigrants in the TSO field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TAR',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 14, 
        Name = 'TARAlpha',
        Description = 'The significance level for the TAR confidence interval, expressed as alpha.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TARAlpha',
        ControlType = 'number',
        [Rule] = NULL        
        
UNION ALL SELECT
        FieldCategoryId = 14, 
        Name = 'TARLowerLimit',
        Description = 'The lower limit of the confidence interval for the TAR field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TARLowerLimit',
        ControlType = 'number',
        [Rule] = NULL
						
UNION ALL SELECT
        FieldCategoryId = 14, 
        Name = 'TARUpperLimit',
        Description = 'The upper limit of the confidence interval for the TAR field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TARUpperLimit',
        ControlType = 'number',
        [Rule] = NULL      
        
UNION ALL SELECT
        FieldCategoryId = 14, 
        Name = 'ReturnsMissing',
        Description = 'This field indicates whether any Returns for this brood year were missing.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'ReturnsMissing',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = 14, 
        Name = 'ReturnsMissingExplanation',
        Description = 'If some Returns data are not accounted for in the SAR estimate, explain the gap.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'ReturnsMissingExplanation',
        ControlType = 'number',
        [Rule] = NULL
      

UNION ALL SELECT
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
        Name = 'ScopeOfInference',
        Description = 'Description of what this SAR represents:  the specific population(s); specific ESU/DPS(s); specific MPG(s); etc. represented.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ScopeOfInference',
        ControlType = 'text',
        [Rule] = NULL        

UNION ALL SELECT
        FieldCategoryId = 14, 
        Name = 'SmoltLocPTcode',
        Description = 'PTAGIS code for the location where smolts were enumerated.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'SmoltLocPTcode',
        ControlType = 'text',
        [Rule] = NULL    

UNION ALL SELECT
        FieldCategoryId = 14, 
        Name = 'TSO',
        Description = 'Total smolt outmigration.  Point estimate of the number of smolts for this outmigration year, or the number of marked smolts used to calculate the SAR.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TSO',
        ControlType = 'number',
        [Rule] = NULL        
        
        
UNION ALL SELECT
        FieldCategoryId = 14, 
        Name = 'TSOAlpha',
        Description = 'The significance level for the TSO confidence interval, expressed as alpha.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TSOAlpha',
        ControlType = 'number',
        [Rule] = NULL        
        
UNION ALL SELECT
        FieldCategoryId = 14, 
        Name = 'TSOLowerLimit',
        Description = 'The lower limit of the confidence interval for the TSO field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TSOLowerLimit',
        ControlType = 'number',
        [Rule] = NULL
						
UNION ALL SELECT
        FieldCategoryId = 14, 
        Name = 'TSOUpperLimit',
        Description = 'The upper limit of the confidence interval for the TSO field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TSOUpperLimit',
        ControlType = 'number',
        [Rule] = NULL         

UNION ALL SELECT
        FieldCategoryId = 14, 
        Name = 'ReturnDef',
        Description = 'How "return" is defined for this SAR estimate.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ReturnDef',
        ControlType = 'text',
        [Rule] = NULL        

UNION ALL SELECT
        FieldCategoryId = 14, 
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
        FieldCategoryId = 14, 
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
	

    