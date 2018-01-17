-- Update Config col of all non-WaterTemp datasets to hide button
-- Update the Description for the first WaterTemp dataset.
update dbo.[Datasets]
set [Description] = 'Water Temperature'
where [Id] = 2

update dbo.[Datasets] set config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange"]}}' where [Id] = 1
update dbo.[Datasets] set config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange"]}}' where [Id] = 3
update dbo.[Datasets] set config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange"]}}' where [Id] = 4
update dbo.[Datasets] set config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange"]}}' where [Id] = 5
update dbo.[Datasets] set config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange"]}}' where [Id] = 6
update dbo.[Datasets] set config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange"]}}' where [Id] = 7
update dbo.[Datasets] set config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange"]}}' where [Id] = 1002
update dbo.[Datasets] set config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange"]}}' where [Id] = 1003
update dbo.[Datasets] set config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange"]}}' where [Id] = 1004
update dbo.[Datasets] set config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange"]}}' where [Id] = 1005
update dbo.[Datasets] set config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange"]}}' where [Id] = 1006
update dbo.[Datasets] set config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange"]}}' where [Id] = 1007
update dbo.[Datasets] set config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange"]}}' where [Id] = 1008
update dbo.[Datasets] set config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange"]}}' where [Id] = 1188
update dbo.[Datasets] set config = '{"DataEntryPage":{"HiddenFields":["BulkQaChange"]}}' where [Id] = 1189
update dbo.[Datasets] set config = '{{"RestrictRoles":["DECD"],"ActivitiesPage":{"Route":"appraisals","ShowFields":["Allotment","AllotmentStatus"]},"DataEntryPage":{"HiddenFields":["Location","ActivityDate","Instrument","QA","BulkQaChange"]}}' where [Id] = 1193
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1207
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1208
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1209
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1210
update dbo.[Datasets] set config = '{"DataEntryPage":{"HiddenFields": ["BulkQaChange"]},"ActivitiesPage":{"ShowFields":["Description","Location.Label","headerdata.DataType","User.Fullname"]}}' where [Id] = 1211
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1212
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1213
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1214
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1215
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1216
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1217
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1218
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1219
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1220
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1221
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1222
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1223
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","Location","BulkQaChange"]}}' where [Id] = 1224
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","Location","BulkQaChange"]}}' where [Id] = 1225
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","Location","BulkQaChange"]}}' where [Id] = 1226
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1227
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1228
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1229
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","addNewRow","BulkQaChange"],"ShowFields": ["Surveyor","addSection","addInterview","addFisherman","addAnotherFish"]}}' where [Id] = 1230
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Timezone","Instrument","BulkQaChange"], "ShowFields": ["ActivityDate"]}}' where [Id] = 1231
update dbo.[Datasets] set config = '{"RestrictRoles":["CRPP"],"ActivitiesPage": {"Route": "crpp"}, "DataEntryPage": {"HiddenFields": ["Location","ActivityDate","QA","Instrument","BulkQaChange"]}}' where [Id] = 1232
update dbo.[Datasets] set config = '{"RestrictRoles":["WRS"],"ActivitiesPage": {"Route": "wrs"}, "DataEntryPage": {"HiddenFields": ["Location","ActivityDate","QA","Instrument","BulkQaChange"]}}' where [Id] = 1235
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","ActivityDate","BulkQaChange"]}}' where [Id] = 1236
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","ActivityDate","BulkQaChange"]}}' where [Id] = 1237
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","ActivityDate","BulkQaChange"]}}' where [Id] = 1238
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","ActivityDate","BulkQaChange"]}}' where [Id] = 1239
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","ActivityDate","BulkQaChange"]}}' where [Id] = 1240
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","ActivityDate","BulkQaChange"]}}' where [Id] = 1241
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1243
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1244
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1245
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1246
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1247
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1248
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1249
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1250
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["Instrument","BulkQaChange"]}}' where [Id] = 1251
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["ActivityDate","Instrument","BulkQaChange"], "ShowFields": ["SampleDate"]}}' where [Id] = 1252
update dbo.[Datasets] set config = '{"DataEntryPage": {"HiddenFields": ["ActivityDate","Instrument","BulkQaChange"], "ShowFields": ["SampleDate"]}}' where [Id] = 1253