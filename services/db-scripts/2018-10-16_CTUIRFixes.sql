-- this is just for CTUIR CDMS - our water temp rules were broken
update fields set [Rule] = '{"OnChange":"activities.errors = undefined; removeRowErrorsBeforeRecheck(); checkForDuplicates();"}' where id = 1065;
update datasetfields set [Rule] = '{"OnChange":"activities.errors = undefined; removeRowErrorsBeforeRecheck(); checkForDuplicates();"}'
where id in (1241, 4468, 4518, 4541, 4615);

go

-- this adds a field to our creel dataset for fisherman to work with the datasource lookup
insert into datasetfields (DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, SourceId, OrderIndex, ControlType,InstrumentId)
values (1230, 1507,2,getdate(),'Fisherman','FishermanId',1,5,'select',null);

-- update the datasource column of our fishermanid field - this will now provide the possibleValues list
update fields set DataSource = 'select Id, fullname as Label from Fishermen' where id = 1507;
