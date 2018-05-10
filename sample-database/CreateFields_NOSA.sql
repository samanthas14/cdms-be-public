--Dataset Creator Script (KenB based on George/Sitka's example)
-- Modify the values in the first declarations, then specify the fields you want to create next.

use CDMS_SAMPLE;
begin TRAN T1;

--set these to what you want
DECLARE @datasetname nvarchar(max) = 'StreamNet_NOSA';
DECLARE @datasetdesc nvarchar(max) = 'StreamNet NOSA Dataset';
DECLARE @owneruserid int = 1; 
DECLARE @projectid int = 11041; 
DECLARE @defaultrowqa int = 1;
DECLARE @defaultactivityqa int = 6;
DECLARE @defaultdatasetstatus int = 1;

--these are internally used by the script (no touchy)
DECLARE @fieldcategoryid int = 0;
DECLARE @datastoreid int = 0;
DECLARE @datasetid int = 0;

-- Create the locationtype (if necessary). 
--  comment this out if your project/dataset will instead use an existing locationtype
--  in either case, you'll need to change the config.js to link them (Bug to fix)
--  The LocationTypeID is used when creating new Locations in the project.
INSERT into LocationTypes (Name, Description)
  values (
	@datasetname,
	concat(@datasetname,' Location Type')
 );

-- Create the Field Category
INSERT into FieldCategories (Name, Description) 
  values (
	@datasetname,
	concat(@datasetname, ' related fields')
);

select @fieldcategoryid = scope_identity(); --the id just created

-- Create the Datastore 
INSERT into Datastores (
		Name, 
		[Description], 
		TablePrefix,
		DatastoreDatasetId,
		OwnerUserId,
		FieldCategoryId
	) VALUES (
		@datasetname,
		@datasetdesc,
		@datasetname,
		null,
		@owneruserid,
		@fieldcategoryid
	);

select @datastoreid = scope_identity(); --the id just created

--Create the Dataset
INSERT into Datasets (
	ProjectId,
	DefaultRowQAStatusId,
	StatusId,
	CreateDateTime,
	Name,
	[Description],
	DefaultActivityQAStatusId,
	DatastoreId,
	Config
) VALUES (
	@projectid,
	@defaultrowqa,
	@defaultdatasetstatus,
	GETDATE(),
	@datasetname,
	@datasetdesc,
	@defaultactivityqa,
	@datastoreid,
	NULL
);

select @datasetid = scope_identity(); --the id just created

-- add datasetrowqa and datasetactivityqa options
INSERT into DatasetQAStatus1 (
	Dataset_Id,
	QAStatus_Id
) VALUES (
	@datasetid,
	1 --ok
);

INSERT into DatasetQAStatus (
	Dataset_Id,
	QAStatus_Id
) VALUES (
	@datasetid,
	5 --approved
);

INSERT into DatasetQAStatus (
	Dataset_Id,
	QAStatus_Id
) VALUES (
	@datasetid,
	6 --readyforqa
);

-- Create the Field+DatasetFields

-- This will info about field records inserted below
CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    

-----
-- Insert any new fields we'll need (George says don't reuse fields)
-- Header Fields
INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo



SELECT   
        FieldCategoryId = @fieldcategoryid, 
        Name = 'ESU_DPS',
        Description = 'Evolutionarily Significant Unit (ESU) or Distinct Population Segment (DPS)',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""ESU"",""DPS""]',
        DbColumnName = 'ESU_DPS',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'CommonName',
        Description = 'Common name of the taxon of fish.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Bull trout"",""Chinook salmon"",""Chum salmon"",""Coho salmon"",""Sockeye salmon"",""Steelhead""]',
        DbColumnName = 'CommonName',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Run',
        Description = 'Run of fish.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Spring"",""Summer"",""Fall"",""Winter"",""Spring/summer"",""N/A""]',
        DbColumnName = 'Run',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'PopFit',
        Description = 'Categorization of how well the geographic extent of the NOSA estimate corresponds to the geographic definition of the population.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Same"",""Portion"",""Multiple""]',
        DbColumnName = 'PopFit',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Publish',
        Description = 'Yes/no value indicating whether this record should be shared freely with all public users via the Exchange Network.  If ""No"" then the record can only be accessed by using the apikey that created it.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Yes"",""No""]',
        DbColumnName = 'Publish',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'TRTmethod',
        Description = 'Flag indicating whether the methods used to generate the values in this record are those defined by the TRT.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Yes"",""No""]',
        DbColumnName = 'TRTmethod',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'NullRecord',
        Description = 'In some years data may not be collected and so a value cannot be calculated.  For example, high muddy water or wildfires can prevent redd counts.  This field is used to indicate that a record does not exist because the data do not exist to calculate it.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Yes"",""No""]',
        DbColumnName = 'NullRecord',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'DataStatus',
        Description = 'Status of the data in the current record.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Draft"",""Reviewed"",""Final""]',
        DbColumnName = 'DataStatus',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'RecoveryDomain',
        Description = 'Name of the ""recovery domain,"" as defined by the NMFS Northwest Region, in which the population falls.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '[""Puget Sound Recovery Domain"", ""Wilamette/Lower Columbia Recovery Domain"", ""Interior Columbia Recovery Domain"",""Oregon Coast Recovery Domain"",""Southern Oregon/Northern California Coast Recovery Domain""]',
        DbColumnName = 'RecoveryDomain',
        ControlType = 'select',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'ContactAgency',
        Description = 'Agency, tribe, or other entity, or person responsible for these data that is the best contact for questions that may arise about this data record.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ContactAgency',
        ControlType = 'text',
        [Rule] = '{""DefaultValue"":""Confederated Tribes of the Umatilla Indian Reservation""}'

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'WaterBody',
        Description = 'Name of the body of water associated with the time series.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'WaterBody',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Comments',
        Description = 'Any issues, problems, questions about this indicator that were not already captured in other places.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'Comments',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'SpawningYear',
        Description = 'The four-digit year in which spawning of this species (and run where appropriate) began.',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'SpawningYear',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'NOSAIJ',
        Description = 'The point estimate for natural origin spawner abundance, including jacks.',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'NOSAIJ',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'NOSAEJ',
        Description = 'The four-digit year in which spawning of this species (and run where appropriate) began.',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'NOSAEJ',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'MethodNumber',
        Description = 'The point estimate for natural origin spawner abundance, excluding jacks.',
        Units = NULL,
        Validation = NULL,
        DataType = 'int',
        PossibleValues = NULL,
        DbColumnName = 'MethodNumber',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'MajorPopGroup',
        Description = 'Name of ""major population group"" (MPG) or ""stratum"" as defined by the NMFS Northwest Region, in which the population falls.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'MajorPopGroup',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
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
        FieldCategoryId = @fieldcategoryid, 
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
        FieldCategoryId = @fieldcategoryid, 
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
        FieldCategoryId = @fieldcategoryid, 
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
        FieldCategoryId = @fieldcategoryid, 
        Name = 'NOSAIJLowerLimit',
        Description = 'The lower limit of the confidence interval for the NOSAIJ field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'NOSAIJLowerLimit',
        ControlType = 'number',
        [Rule] = NULL
		
UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'NOSAIJUpperLimit',
        Description = 'The upper limit of the confidence interval for the NOSAIJ field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'NOSAIJUpperLimit',
        ControlType = 'number',
        [Rule] = NULL
		
UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'NOSAIJAlpha',
        Description = 'The significance level for the NOSAIJ confidence interval, expressed as alpha.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'NOSAIJAlpha',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'NOSAEJLowerLimit',
        Description = 'The lower limit of the confidence interval for the NOSAEJ field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'NOSAEJLowerLimit',
        ControlType = 'number',
        [Rule] = NULL
		
UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'NOSAEJUpperLimit',
        Description = 'The upper limit of the confidence interval for the NOSAEJ field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'NOSAEJUpperLimit',
        ControlType = 'number',
        [Rule] = NULL
		
UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'NOSAEJAlpha',
        Description = 'The significance level for the NOSAEJ confidence interval, expressed as alpha.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'NOSAEJAlpha',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
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
        FieldCategoryId = @fieldcategoryid, 
        Name = 'pHOSij',
        Description = 'Point estimate for the proportion of fish spawning naturally, including jacks, that are hatchery origin fish.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'pHOSij',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'pHOSijLowerLimit',
        Description = 'The lower limit of the confidence interval for the pHOSij field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'pHOSijLowerLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'pHOSijUpperLimit',
        Description = 'The upper limit of the confidence interval for the pHOSij field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'pHOSijUpperLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'pHOSijAlpha',
        Description = 'The significance level for the pHOSij confidence interval, expressed as alpha.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'pHOSijAlpha',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'pHOSej',
        Description = 'Point estimate for the proportion of fish spawning naturally, excluding jacks, that are hatchery origin fish.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'pHOSej',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'pHOSejLowerLimit',
        Description = 'The lower limit of the confidence interval for the pHOSej field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'pHOSejLowerLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'pHOSejUpperLimit',
        Description = 'The upper limit of the confidence interval for the pHOSej field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'pHOSejUpperLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'pHOSejAlpha',
        Description = 'The significance level for the pHOSej confidence interval, expressed as alpha.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'pHOSejAlpha',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'NOSJF',
        Description = 'The point estimate for the natural origin spawners jack fraction.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'NOSJF',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'NOSJFLowerLimit',
        Description = 'The lower limit of the confidence interval for the NOSJF field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'NOSJFLowerLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'NOSJFUpperLimit',
        Description = 'The upper limit of the confidence interval for the NOSJF field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'NOSJFUpperLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'NOSJFAlpha',
        Description = 'The significance level for the NOSJF confidence interval, expressed as alpha.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'NOSJFAlpha',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'HOSJF',
        Description = 'The point estimate for the hatchery origin spawners jack fraction.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'HOSJF',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'TSAIJ',
        Description = 'The point estimate for total spawner abundance, including jacks.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TSAIJ',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'TSAIJLowerLimit',
        Description = 'The lower limit of the confidence interval for the TSAIJ field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TSAIJLowerLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'TSAIJUpperLimit',
        Description = 'The upper limit of the confidence interval for the TSAIJ field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TSAIJUpperLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'TSAIJAlpha',
        Description = 'The significance level for the TSAIJ confidence interval, expressed as alpha.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TSAIJAlpha',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'TSAEJ',
        Description = 'The point estimate for total spawner abundance, excluding jacks.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TSAEJ',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'TSAEJLowerLimit',
        Description = 'The lower limit of the confidence interval for the TSAEJ field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TSAEJLowerLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'TSAEJUpperLimit',
        Description = 'The upper limit of the confidence interval for the TSAEJ field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TSAEJUpperLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'TSAEJAlpha',
        Description = 'The significance level for the TSAEJ confidence interval, expressed as alpha.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'TSAEJAlpha',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age2Prop',
        Description = 'The proportion of natural origin fish that were age 2 (brood year +2).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age2Prop',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age2PropLowerLimit',
        Description = 'The lower limit of the confidence interval for the Age2Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age2PropLowerLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age2PropUpperLimit',
        Description = 'The upper limit of the confidence interval for the Age2Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age2PropUpperLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age3Prop',
        Description = 'The proportion of natural origin fish that were age 3 (brood year +3).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age3Prop',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age3PropLowerLimit',
        Description = 'The lower limit of the confidence interval for the Age3Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age3PropLowerLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age3PropUpperLimit',
        Description = 'The upper limit of the confidence interval for the Age3Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age3PropUpperLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age4Prop',
        Description = 'The proportion of natural origin fish that were age 4 (brood year +4).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age4Prop',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age4PropLowerLimit',
        Description = 'The lower limit of the confidence interval for the Age4Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age4PropLowerLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age4PropUpperLimit',
        Description = 'The upper limit of the confidence interval for the Age4Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age4PropUpperLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age5Prop',
        Description = 'The proportion of natural origin fish that were age 5 (brood year +5).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age5Prop',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age5PropLowerLimit',
        Description = 'The lower limit of the confidence interval for the Age5Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age5PropLowerLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age5PropUpperLimit',
        Description = 'The upper limit of the confidence interval for the Age5Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age5PropUpperLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age6Prop',
        Description = 'The proportion of natural origin fish that were age 6 (brood year +6).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age6Prop',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age6PropLowerLimit',
        Description = 'The lower limit of the confidence interval for the Age6Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age6PropLowerLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age6PropUpperLimit',
        Description = 'The upper limit of the confidence interval for the Age6Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age6PropUpperLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age7Prop',
        Description = 'The proportion of natural origin fish that were age 7 (brood year +7).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age7Prop',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age7PropLowerLimit',
        Description = 'The lower limit of the confidence interval for the Age7Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age7PropLowerLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age7PropUpperLimit',
        Description = 'The upper limit of the confidence interval for the Age7Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age7PropUpperLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age8Prop',
        Description = 'The proportion of natural origin fish that were age 8 (brood year +8).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age8Prop',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age8PropLowerLimit',
        Description = 'The lower limit of the confidence interval for the Age8Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age8PropLowerLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age8PropUpperLimit',
        Description = 'The upper limit of the confidence interval for the Age8Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age8PropUpperLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age9Prop',
        Description = 'The proportion of natural origin fish that were age 9 (brood year +9).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age9Prop',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age9PropLowerLimit',
        Description = 'The lower limit of the confidence interval for the Age9Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age9PropLowerLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age9PropUpperLimit',
        Description = 'The upper limit of the confidence interval for the Age9Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age9PropUpperLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age10Prop',
        Description = 'The proportion of natural origin fish that were age 10 (brood year +10).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age10Prop',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age10PropLowerLimit',
        Description = 'The lower limit of the confidence interval for the Age10Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age10PropLowerLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age10PropUpperLimit',
        Description = 'The upper limit of the confidence interval for the Age10Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age10PropUpperLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age11Prop',
        Description = 'The proportion of natural origin fish that were age 11 (brood year +11).',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age11Prop',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age11PropLowerLimit',
        Description = 'The lower limit of the confidence interval for the Age11Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age11PropLowerLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'Age11PropUpperLimit',
        Description = 'The upper limit of the confidence interval for the Age11Prop field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'Age11PropUpperLimit',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'AgePropAlpha',
        Description = 'The significance level for the Age_x_Prop confidence intervals, expressed as alpha.',
        Units = NULL,
        Validation = NULL,
        DataType = 'float',
        PossibleValues = NULL,
        DbColumnName = 'AgePropAlpha',
        ControlType = 'number',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
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
        FieldCategoryId = @fieldcategoryid, 
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
        FieldCategoryId = @fieldcategoryid, 
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
        FieldCategoryId = @fieldcategoryid, 
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
        FieldCategoryId = @fieldcategoryid, 
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
        FieldCategoryId = @fieldcategoryid, 
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
        FieldCategoryId = @fieldcategoryid, 
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
        FieldCategoryId = @fieldcategoryid, 
        Name = 'ContactPersonFirst',
        Description = 'First name of person who is the best contact for questions that may arise about this data record.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ContactPersonFirst',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'ContactPersonLast',
        Description = 'Last name of person who is the best contact for questions that may arise about this data record.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ContactPersonLast',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'ContactPhone',
        Description = 'Phone number of person who is the best contact for questions that may arise about this data record.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ContactPhone',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'ContactEmail',
        Description = 'Email address of person who is the best contact for questions that may arise about this data record.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'ContactEmail',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
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
        FieldCategoryId = @fieldcategoryid, 
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
        FieldCategoryId = @fieldcategoryid, 
        Name = 'UpdDate',
        Description = 'The date and time that the record was created or updated.  For data obtained in electronic format from another source it can reflect the date and time of data capture or of conversion to Coordinated Assessment/StreamNet standards.',
        Units = NULL,
        Validation = NULL,
        DataType = 'date',
        PossibleValues = NULL,
        DbColumnName = 'UpdDate',
        ControlType = 'datetime',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
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
        FieldCategoryId = @fieldcategoryid, 
        Name = 'DataEntryNotes',
        Description = 'Notes about this record by the compiler identified in the ""DataEntry"" field.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'DataEntryNotes',
        ControlType = 'text',
        [Rule] = NULL

UNION ALL SELECT
        FieldCategoryId = @fieldcategoryid, 
        Name = 'CompilerRecordID',
        Description = 'Agency record ID maintained by the data submitter.',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = NULL,
        DbColumnName = 'CompilerRecordID',
        ControlType = 'text',
        [Rule] = NULL

-- Set the Field Role to "details" for all of these fields in our temporary table.
update #NewFieldInfo set FieldRoleId = 2 where FieldRoleId is NULL   -- 2 == details


-- Finally, Bulk add the new fields to the dataset -- this will insert a new row for each combination of datasetId and fieldId for the records inserted above
INSERT INTO dbo.DatasetFields(DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, Validation, SourceId, InstrumentId, OrderIndex, ControlType, [Rule])
SELECT
    DatasetId      = @datasetid,
    FieldId        = f.id,
    FieldRoleId    = f.FieldRoleId,
    CreateDateTime = GETDATE(),
    Label          = f.fieldName,
    DbColumnName   = f.DbColumnName,
    Validation     = f.Validation,
    SourceId       = 1,
    InstrumentId   = NULL,
    OrderIndex     = f.OrderIndex * 10,     -- x10 to make it easier to insert intermediate orders
    ControlType    = f.ControlType,
    [Rule]         = f.[Rule]
FROM #NewFieldInfo as f

drop table #NewFieldInfo

COMMIT TRAN T1;