
USE CDMS_STARTUP

--first, delete all data from the dataset tables
DECLARE @NAME VARCHAR(100)
DECLARE @SQL NVARCHAR(300)

DECLARE CUR CURSOR FOR
	select name 
	from (
		select 'AdultWeir' as name
		union select 'Appraisal'
		union select 'ArtificialProduction'
		union select 'Benthic'
		union select 'BSample'
		union select 'CreelSurvey'
		union select 'CrppContracts'
		union select 'Drift'
		union select 'Electrofishing'
		union select 'FishScales'
		union select 'FishTransport'
		union select 'Genetic'
		union select 'JvRearing'
		union select 'Metrics'
		union select 'ScrewTrap'
		union select 'SnorkelFish'
		union select 'SpawningGroundSurvey'
		union select 'StreamNet_NOSA'
		union select 'StreamNet_RperS'
		union select 'StreamNet_SAR'
		union select 'WaterQuality'
		union select 'WaterTemp'
		) 
	as T1
OPEN CUR

FETCH NEXT FROM CUR INTO @NAME

WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @SQL = 'TRUNCATE TABLE '+@NAME+'_Detail;'
		PRINT @SQL
				
		EXEC Sp_executesql
		 @SQL
		 
		SET @SQL = 'TRUNCATE TABLE '+@NAME+'_Header;'
		PRINT @SQL
		
		EXEC Sp_executesql
		 @SQL
				
		FETCH NEXT FROM CUR INTO @NAME
	END

CLOSE CUR
DEALLOCATE CUR

--now clear out all of the other tables
delete from ActivityQAs;
delete from Activities;
delete from ProjectUsers;
delete from Files;
delete from Projects;
delete from UserPreferences;
delete from InstrumentAccuracyChecks;
delete from Instruments;
delete from users where id > 1;
delete from AppraisalFiles;
delete from Collaborators;
delete from CorrespondenceEvents;
delete from departments where Id in (12,13,14,15,1015);
update datastores set OwnerUserId = 1;
truncate table cdms_wtreporttable;
truncate table creelreporttable;

--remove datasetfields/fields for appraisals/correspondence
delete from DatasetFields where Id in (select df.Id from DatasetFields df
join Datasets d on df.DatasetId = d.Id
where d.DatastoreId in 4,16);

delete from fields where fieldcategoryid in(17,5); --appraisals/correspondence
delete from fieldcategories where id in (5,17);--appraisals/correspondence
delete from datasets where datastoreid in (4,16);--appraisals/correspondence
delete from datastores where id in (4,16);--appraisals/correspondence
delete from Fishermen;
delete from Fundings;
delete from HabitatItems;
delete from locations;
delete from locationtypes where Id in (8, 111);
delete from metadatavalues;
delete from seasondates;
delete from subproject_crpp;
delete from subproject_hab;
delete from waterbodies;
delete from datasetfields;
delete from datasets;
delete from DatasetQAStatus;
delete from DatasetQAStatus1;

update metadataproperties set PossibleValues = '["Admin","Water Resources","Fisheries","Wildlife","RAF","GIS"]' where Id = 23; 
