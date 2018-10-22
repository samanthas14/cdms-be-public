--add some columns that are long overdue!
ALTER TABLE [dbo].[Datastores] ADD [DefaultConfig] [nvarchar](max)
ALTER TABLE [dbo].[Fields] ADD [DatastoreId] [int] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[Fields] ADD [FieldRoleId] [int] NOT NULL DEFAULT 0
go

-- add a new field category to fix the doubling up of FishTransport and WaterTemp and then set the new DatastoreId field
DECLARE @fishtransportfcid int = 0;
insert into FieldCategories (Name, Description) values ('Fish Transport','Fish Transport');
select @fishtransportfcid = scope_identity();
update fields set FieldCategoryId = @fishtransportfcid where id in (26,32,35,1038,1039,1040,1046,1052);
update datastores set FieldCategoryId = @fishtransportfcid where TablePrefix = 'FishTransport';
update fields set DatastoreId = (select d.Id from Datastores d where d.FieldCategoryId = fields.FieldCategoryId);
go


-- now we can remove the fieldcategory altogether
IF object_id(N'[dbo].[FK_dbo.Fields_dbo.FieldCategories_FieldCategoryId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[Fields] DROP CONSTRAINT [FK_dbo.Fields_dbo.FieldCategories_FieldCategoryId]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_FieldCategoryId' AND object_id = object_id(N'[dbo].[Fields]', N'U'))
    DROP INDEX [IX_FieldCategoryId] ON [dbo].[Fields]
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Datastores')
AND col_name(parent_object_id, parent_column_id) = 'FieldCategoryId';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Datastores] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[Datastores] DROP COLUMN [FieldCategoryId]
DECLARE @var1 nvarchar(128)
SELECT @var1 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Fields')
AND col_name(parent_object_id, parent_column_id) = 'FieldCategoryId';
IF @var1 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Fields] DROP CONSTRAINT [' + @var1 + ']')
ALTER TABLE [dbo].[Fields] DROP COLUMN [FieldCategoryId]
DROP TABLE [dbo].[FieldCategories]
go

-- update the new fieldroleid in the fields table from a row in datasetfields (if exists)
--   note: after this runs, there are still 35 rows that aren't set and need to be address by colette: e.g.: select * from fields where fieldroleid = 0
update fields set fieldroleid = (select top 1 fieldroleid from datasetfields df where df.fieldid = fields.Id) 
where fields.Id in (select distinct fieldid from datasetfields)
go

-- finally update our datastore default config with a config from one of the datasets that have a config for that datastore
update Datastores set DefaultConfig = (select top 1 Config from datasets where Config is not null and Datastores.Id = datasets.datastoreId) 
where Datastores.Id in (select distinct dc.DatastoreId from Datasets dc where dc.Config is not null and dc.DatastoreId is not null)
go

