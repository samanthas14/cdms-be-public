--Harvest/Creel
update datasets set Config = 
'{"DataEntryPage":{"HiddenFields":["Instrument","addNewRow","BulkQaChange"],"ShowFields":["Surveyor","addSection","addInterview","addFisherman","addAnotherFish"]},"ActivitiesPage":{"ShowFields":["ActivityDate","headerdata.TimeStart","Location.Label"]}}'
where id in (select d.Id from Datasets d
 join DataStores ds on d.DatastoreId = ds.Id
where ds.TablePrefix = 'CreelSurvey' );

--FishScales 10
update datasets set Config = '{"DataEntryPage":{"HiddenFields":["Instrument","Location","BulkQaChange"]},"ActivitiesPage":{"ShowFields":["headerdata.RunYear","Location.Label","User.Fullname"]}}' 
where id in (select d.Id from Datasets d
 join DataStores ds on d.DatastoreId = ds.Id
where ds.TablePrefix = 'FishScales' );

--Metrics 17
update datasets set Config = '{"DataEntryPage":{"HiddenFields":["Instrument","ActivityDate","BulkQaChange"]},"ActivitiesPage":{"ShowFields":["headerdata.YearReported","Location.Label"]}}' 
where id in (select d.Id from Datasets d
 join DataStores ds on d.DatastoreId = ds.Id
where ds.TablePrefix = 'Metrics' );