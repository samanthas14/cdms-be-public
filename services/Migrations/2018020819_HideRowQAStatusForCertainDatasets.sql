-- hide bulkqachange field for datasets where it isn't hidden yet.
-- rule: only watertemp should have bulkqachange not hidden...

--electrofishing
update datasets set Config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' 
where id in (select d.Id from Datasets d
 join DataStores ds on d.DatastoreId = ds.Id
where ds.TablePrefix = 'Electrofishing' );

--crppcontracts
update datasets set Config = '{"RestrictRoles":["CRPP"], "DataEntryPage": {"HiddenFields": ["Location","ActivityDate","QA","Instrument","BulkQaChange"], "ShowFields": ["DateReceived","ProjectLead"]}}' 
where id in (select d.Id from Datasets d
 join DataStores ds on d.DatastoreId = ds.Id
where ds.TablePrefix = 'CrppContracts' );

--waterquality - update ShowFields to HiddenFields
update datasets set Config = '{"DataEntryPage":{"HiddenFields": ["BulkQaChange","ActivityDate"]},"ActivitiesPage":{"ShowFields":["Description","Location.Label","headerdata.DataType","User.Fullname"]}}' 
where id in (select d.Id from Datasets d
 join DataStores ds on d.DatastoreId = ds.Id
where ds.TablePrefix = 'WaterQuality' );



