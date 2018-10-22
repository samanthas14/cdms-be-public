-- create a our activity system fields datastore!

DECLARE @activitydsid int = 0;

insert into Datastores (Name, TablePrefix, OwnerUserId, LocationTypeId, DefaultConfig) values ('ActivitySystemFields',null,1,null,
'{
  "DataEntryPage": {
    "HeaderFields": [
      "Location",
      "ActivityDate",
      "SampleDate",
      "Timezone",
      "Instrument",
      "AccuracyCheck",
      "PostAccuracyCheck"
    ],
    "QAFields": [
      "QAStatus",
      "QAComments"
    ],
    "DetailFields": [
      "RowQAStatus"
    ],
    "HiddenFields": [
      "Instrument",
      "AccuracyCheck",
      "PostAccuracyCheck",
      "Timezone",
      "SampleDate",
      "RowQAStatus"
    ],
    "sort": {
      "field": "ActivityDate",
      "direction": "desc"
    }
  }
}');

select @activitydsid = scope_identity();

insert into Fields (Name, Description, DbColumnName, ControlType, DatastoreId, FieldRoleId)
values 
('Activity Date','Activity date field','ActivityDate','activity-date', @activitydsid, 1),
('QA Status','Activity QA Status','QAStatus','qa-status-select',@activitydsid,1),
('QA Comments','Activity QA Comments','QAComments','qa-status-comment',@activitydsid,1),
('Location','Activity location','LocationId','location-select',@activitydsid,1),
('Instrument','Activity instrument','InstrumentId','instrument-select',@activitydsid,1),
('Accuracy Check','Activity instrument accuracy check','AccuracyCheckId','accuracy-check-select',@activitydsid,1),
('Post Accuracy Check','Activity instrument post-accuracy check','PostAccuracyCheckId','post-accuracy-check-select',@activitydsid,1),
('Reading Timezone','Activity reading timezone','Timezone','timezone-select',@activitydsid,1),
('Row QA Status','Activity row QA status','QAStatusId','select',@activitydsid,1);

