
-- add activity date to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, 1744, 1, 'Activity Date','ActivityDate',1,1,'activity-date',null
from datasets where datastoreid in (1,2,4,5,7,8,9,10,11,12,13,14,15,18,19,20);

-- add activity date as "sample date" for Benthic and Drift
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, 1744, 1, 'Sample Date','ActivityDate',1,1,'activity-date',null
from datasets where datastoreid in (21,22);

-- add location to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, 1747, 1, 'Location','LocationId',1,0,'location-select',null
from datasets where datastoreid in (1,2,3,4,5,6,7,8,9,11,12,13,14,15,17,18,19,20,21,22);

-- add qa status to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, 1745, 1, 'QA Status','QAStatusId',1,2,'qa-status-select',null
from datasets where datastoreid in (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,17,18,19,20,21,22);

-- add qa comments to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, 1746, 1, 'QA Comments','QAComments',1,3,'qa-status-comment',null
from datasets where datastoreid in (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,17,18,19,20,21,22);

-- add instruments to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, 1748, 1, 'Instrument','InstrumentId',1,4,'instrument-select',null
from datasets where datastoreid in (3,6);

-- add accuracy check to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, 1749, 1, 'Accuracy Check','AccuracyCheckId',1,5,'accuracy-check-select',null
from datasets where datastoreid in (3,6);

-- add post accuracy check to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, 1750, 1, 'Post-accuracy Check','PostAccuracyCheckId',1,6,'post-accuracy-check-select',null
from datasets where datastoreid in (3,6);

-- add timezone to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, 1751, 1, 'Timezone','Timezone',1,7,'timezone-select',null
from datasets where datastoreid in (3,6);

-- add rowqa to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, 1753, 2, 'QA','QAStatusId',1,71,'select',null
from datasets where datastoreid in (3);

-- add description to all datasets that need it
insert into DatasetFields (CreateDateTime, DatasetId, FieldId, FieldRoleId, Label, DbColumnName, SourceId, OrderIndex, ControlType, InstrumentId)
select getdate(), id, 1763, 1, 'Description','Description',1,8,'activity-text',null
from datasets where datastoreid in (3);
