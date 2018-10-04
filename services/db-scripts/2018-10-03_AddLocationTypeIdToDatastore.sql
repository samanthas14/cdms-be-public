
ALTER TABLE [dbo].[Datastores] ADD [LocationTypeId] [nvarchar](max)
go
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Datastores')
AND col_name(parent_object_id, parent_column_id) = 'DatastoreDatasetId';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Datastores] DROP CONSTRAINT [' + @var0 + ']')

go
ALTER TABLE [dbo].[Datastores] DROP COLUMN [DatastoreDatasetId]
go
-- map the LocationTypeId as it should be
update Datastores set LocationTypeId = 4 WHERE TablePrefix = 'AdultWeir';
update Datastores set LocationTypeId = 113 WHERE TablePrefix = 'BSample';
update Datastores set LocationTypeId = 6 WHERE TablePrefix = 'WaterTemp';
update Datastores set LocationTypeId = 7 WHERE TablePrefix = 'SpawningGroundSurvey';
update Datastores set LocationTypeId = 109 WHERE TablePrefix = 'CreelSurvey';
update Datastores set LocationTypeId = 101 WHERE TablePrefix = 'Electrofishing';
update Datastores set LocationTypeId = 102 WHERE TablePrefix = 'SnorkelFish';
update Datastores set LocationTypeId = 103 WHERE TablePrefix = 'ScrewTrap';
update Datastores set LocationTypeId = 104 WHERE TablePrefix = 'FishScales';
update Datastores set LocationTypeId = 105 WHERE TablePrefix = 'WaterQuality';
update Datastores set LocationTypeId = 106 WHERE TablePrefix = 'StreamNet_RperS';
update Datastores set LocationTypeId = 108 WHERE TablePrefix = 'StreamNet_SAR';
update Datastores set LocationTypeId = 107 WHERE TablePrefix = 'StreamNet_NOSA';
update Datastores set LocationTypeId = 110 WHERE TablePrefix = 'ArtificialProduction';
update Datastores set LocationTypeId = 112 WHERE TablePrefix = 'Metrics';
update Datastores set LocationTypeId = 114 WHERE TablePrefix = 'JvRearing';
update Datastores set LocationTypeId = 115 WHERE TablePrefix = 'Genetic';
update Datastores set LocationTypeId = 116 WHERE TablePrefix = 'Benthic';
update Datastores set LocationTypeId = 117 WHERE TablePrefix = 'Drift';
update Datastores set LocationTypeId = 118 WHERE TablePrefix = 'FishTransport';
update Datastores set LocationTypeId = 8 WHERE TablePrefix = 'Appraisal';
update Datastores set LocationTypeId = 3 WHERE TablePrefix = 'CRPP';
update Datastores set LocationTypeId = 112 WHERE TablePrefix = 'Hab';

