namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImportDecdData : DbMigration
    {
        public override void Up()
        {
            /* Note:  CDMS_DECD started from a copy of CDMS_PROD in 2016 (5/20/2016).  Along the way, we deleted the 
            *  Appraisal contents out of CDMS_PROD but left the structure.  Since then the users have added
            *  new appraisals (locations, activities, etc.) to CDMS_DECD.  Therefore, we must do the following.
            *  Capture the old data and copy it back into CDMS where it was.
            *  Capture the new data, update it as necessary (Activities.LocationId), and add it to CDMS_PROD.
            */
            Sql(@"
-- Capture the data from CDMS_DECD and copy.
-- Copy the project in, reusing the primary key.
set identity_insert dbo.[Projects] on;
go

insert into dbo.[Projects]
([Id],[ProjectTypeId],[OrganizationId],[Name],[Description],[CreateDateTime],[StartDate],[EndDate],[OwnerId],[ShowLaboratories])
select [Id],[ProjectTypeId],[OrganizationId],[Name],[Description],[CreateDateTime],[StartDate],[EndDate],[OwnerId],[ShowLaboratories]
FROM [CDMS_DECD].[dbo].[Projects]
where Name like '%DECD%'
go

set identity_insert dbo.[Projects] off;
go


-- Add DECD Users to the Database
set identity_insert dbo.[Users] on;
go

insert into dbo.Users
(
[Id],[OrganizationId],[GUID],[Username],[Description],[LastLogin],[DepartmentId],[Fullname],[Roles],[ProfileImageUrl]
)
select [Id],[OrganizationId],[GUID],[Username],[Description],[LastLogin],[DepartmentId],[Fullname],[Roles],[ProfileImageUrl]
from CDMS_DECD.dbo.Users decdU
where decdU.Username = 'avarym'

set identity_insert dbo.[Users] off;
go


-- Add Appraisal FieldCategory back in
set identity_insert dbo.FieldCategories on;
go

insert into dbo.FieldCategories
(
[Id],[Name],[Description]
)
select [Id],[Name],[Description]
from CDMS_DECD.dbo.FieldCategories
where Name = 'Appraisal'

set identity_insert dbo.FieldCategories off;
go


-- Add Fields back in
set identity_insert dbo.Fields on;
go

insert into dbo.Fields
(
[Id],[FieldCategoryId],[TechnicalName],[Name],[Description],[Units],[Validation],[DataType],[PossibleValues]
      ,[Rule],[DbColumnName],[ControlType],[DataSource]
)
select [Id],[FieldCategoryId],[TechnicalName],[Name],[Description],[Units],[Validation],[DataType],[PossibleValues]
      ,[Rule],[DbColumnName],[ControlType],[DataSource]
from CDMS_DECD.dbo.Fields
where FieldCategoryId = 5

set identity_insert dbo.Fields off;
go


-- Add Appraisals datastore back in
set identity_insert dbo.[Datastores] on;
go

insert into dbo.Datastores
(
[Id],[Name],[Description],[TablePrefix],[DatastoreDatasetId],[OwnerUserId],[FieldCategoryId]
)
select [Id],[Name],[Description],[TablePrefix],[DatastoreDatasetId],[OwnerUserId],[FieldCategoryId]
from CDMS_DECD.dbo.Datastores
where TablePrefix = 'Appraisal'

set identity_insert dbo.[Datastores] off;
go


-- Add DECD Dataset back in
set identity_insert dbo.[Datasets] on;
go

insert into dbo.Datasets
(
[Id],[ProjectId],[DefaultRowQAStatusId],[StatusId],[CreateDateTime],[Name],[Description],[DefaultActivityQAStatusId],[DatastoreId],[Config]
)
select [Id],[ProjectId],[DefaultRowQAStatusId],[StatusId],[CreateDateTime],[Name],[Description],[DefaultActivityQAStatusId],[DatastoreId],[Config]
from CDMS_DECD.dbo.Datasets
where Name = 'DECD Appraisals'

set identity_insert dbo.[Datasets] off;
go


-- Add DECD DatasetFields back in
set identity_insert dbo.DatasetFields on;
go

insert into dbo.DatasetFields
(
[Id],[DatasetId],[FieldId],[FieldRoleId],[CreateDateTime],[Label],[DbColumnName],[Validation],[SourceId],[InstrumentId]
      ,[OrderIndex],[ControlType],[Rule]
)
select [Id],[DatasetId],[FieldId],[FieldRoleId],[CreateDateTime],[Label],[DbColumnName],[Validation],[SourceId],[InstrumentId]
      ,[OrderIndex],[ControlType],[Rule]
from CDMS_DECD.dbo.DatasetFields
where DatasetId = 1193

set identity_insert dbo.DatasetFields off;
go




-- Add the old locations back in
set identity_insert dbo.[Locations] on;
go

insert into dbo.[Locations]
([Id],[LocationTypeId],[SdeFeatureClassId],[SdeObjectId],[Label],[Name],[Description],[CreateDateTime],[UserId],[Elevation]
,[Status],[GPSEasting],[GPSNorthing],[Projection],[UTMZone],[Latitude],[Longitude],[OtherAgencyId],[ImageLink],[WettedWidth]
,[WettedDepth],[RiverMile],[WaterBodyId],[ProjectId]
)
select [Id],[LocationTypeId],[SdeFeatureClassId],[SdeObjectId],[Label],[Name],[Description],[CreateDateTime],[UserId],[Elevation]
,[Status],[GPSEasting],[GPSNorthing],[Projection],[UTMZone],[Latitude],[Longitude],[OtherAgencyId],[ImageLink],[WettedWidth]
,[WettedDepth],[RiverMile],[WaterBodyId],2246
from CDMS_DECD.dbo.Locations dl
where dl.LocationTypeId = 8
and dl.Id < 3758 
-- Note:  There are at least 22 new locations in the DECD db, where the location Ids also exist in the CDMS_PROD db for other projects.

set identity_insert dbo.[Locations] off;
go


-- Add the old Activities back in, retaining/reusing the same Activity Id.
set identity_insert dbo.[Activities] on;
go

insert into dbo.Activities
(
[Id],[Name],[Description],[DatasetId],[SourceId],[LocationId],[UserId],[ActivityTypeId],[CreateDate],[ActivityDate]
      ,[InstrumentId],[AccuracyCheckId],[PostAccuracyCheckId],[Timezone],[LaboratoryId]
)
select [Id],[Name],[Description],[DatasetId],[SourceId],[LocationId],[UserId],[ActivityTypeId],[CreateDate],[ActivityDate]
      ,[InstrumentId],[AccuracyCheckId],[PostAccuracyCheckId],[Timezone],[LaboratoryId]
from CDMS_DECD.dbo.Activities decdA
--where DatasetId = 1193 and LocationId < 3758
where DatasetId = 1193 and [Id] < 49303

set identity_insert dbo.[Activities] off;
go


-- ***** New Locations start *****
-- Capture the new DECD locations
SELECT *
into dbo.LocationsNewDecd 
FROM [CDMS_DECD].[dbo].[Locations]
where LocationTypeId = 8
and [Id] >= 3758

-- Add ProjectId to the new table (LocationsNewDecd)
alter table dbo.LocationsNewDecd
add ProjectId integer
go

-- Set the ProjectId for the DECD locations
update dbo.LocationsNewDecd
set ProjectId = 2246

-- Insert the new locations into the Locations table
insert into dbo.Locations
([LocationTypeId],[SdeFeatureClassId],[SdeObjectId],[Label],[Name],[Description],[CreateDateTime],[UserId],[Elevation],[Status]
      ,[GPSEasting],[GPSNorthing],[Projection],[UTMZone],[Latitude],[Longitude],[OtherAgencyId],[ImageLink],[WettedWidth]
      ,[WettedDepth],[RiverMile],[WaterBodyId],[ProjectId]
)
select [LocationTypeId],[SdeFeatureClassId],[SdeObjectId],[Label],[Name],[Description],[CreateDateTime],[UserId],[Elevation],[Status]
      ,[GPSEasting],[GPSNorthing],[Projection],[UTMZone],[Latitude],[Longitude],[OtherAgencyId],[ImageLink],[WettedWidth]
      ,[WettedDepth],[RiverMile],[WaterBodyId],[ProjectId]
from dbo.LocationsNewDecd
-- ***** New Locations end *****


-- ***** New Activities start *****
-- Capture the new DECD Activities
select *
into dbo.ActivitiesNewDecd
  FROM [CDMS_DECD].[dbo].[Activities]
  --where DatasetId = 1193 and LocationId >= 3758
  where DatasetId = 1193 and Id >= 49303

-- Update the LocationId in the new Activities table to match what is on the new records in table Locations
update dbo.ActivitiesNewDecd
set LocationId = r.Id
from
(
select actNewDecd.Id DAid, actNewDecd.LocationId DLid, locNewDecd.Id nLLid, locNewDecd.SdeObjectId nLSdeId, l.Id, l.SdeObjectId
from dbo.ActivitiesNewDecd actNewDecd
inner join dbo.LocationsNewDecd locNewDecd on locNewDecd.Id = actNewDecd.LocationId
inner join dbo.Locations l on l.SdeObjectId = locNewDecd.SdeObjectId
) as r
where dbo.ActivitiesNewDecd.Id = r.DAid

-- Add the updated records from ActivitiesNewDecd to dbo.Activities.
insert into dbo.Activities
([Name],[Description],[DatasetId],[SourceId],[LocationId],[UserId],[ActivityTypeId],[CreateDate],[ActivityDate]
      ,[InstrumentId],[AccuracyCheckId],[PostAccuracyCheckId],[Timezone],[LaboratoryId]
)
select [Name],[Description],[DatasetId],[SourceId],[LocationId],[UserId],[ActivityTypeId],[CreateDate],[ActivityDate]
      ,[InstrumentId],[AccuracyCheckId],[PostAccuracyCheckId],[Timezone],[LaboratoryId]
FROM dbo.ActivitiesNewDecd
-- ***** New Activities end *****


-- ***** Headers start *****
-- Capture the new Activities, for easy reference when working with Headers/Details.
SELECT TOP 5 *
into dbo.ActivitiesTmp
  FROM dbo.[Activities]
  where DatasetId = 1193
  order by [Id] desc

set identity_insert dbo.[Appraisal_Header] on;
go

-- Copy in all the old header data
insert into dbo.Appraisal_Header
(
	[Id],[Allotment],[AllotmentStatus],[AllotmentName],[AllotmentDescription],[AllotmentComments],[ActivityId],[ByUserId]
      ,[EffDt],[CobellAppraisalWave],[LeaseTypes],[MapFiles],[TSRFiles],[FarmingLeaseFiles],[TimberAppraisalFiles]
      ,[GrazingLeaseFiles],[AllotmentPhotoFiles],[RegionalOfficeReviewFiles],[LastAppraisalRequestDate],[UpdatedTSRFile]
      ,[HasTimber],[IsMappable],[Acres],[PriorityType],[LegalDescription]
)
select [Id],[Allotment],[AllotmentStatus],[AllotmentName],[AllotmentDescription],[AllotmentComments],[ActivityId],[ByUserId]
      ,[EffDt],[CobellAppraisalWave],[LeaseTypes],[MapFiles],[TSRFiles],[FarmingLeaseFiles],[TimberAppraisalFiles]
      ,[GrazingLeaseFiles],[AllotmentPhotoFiles],[RegionalOfficeReviewFiles],[LastAppraisalRequestDate],[UpdatedTSRFile]
      ,[HasTimber],[IsMappable],[Acres],[PriorityType],[LegalDescription]
from CDMS_DECD.dbo.Appraisal_Header ah
where ah.[ActivityId] not in (select [Id] from dbo.ActivitiesNewDecd)

set identity_insert dbo.[Appraisal_Header] off;
go


-- Capture all the new header records
select * 
into dbo.Appraisal_HeaderNew
from CDMS_DECD.dbo.Appraisal_Header
where [ActivityId] in (select [Id] from dbo.ActivitiesNewDecd)


-- Change Old ActivityId to the new one in dbo.ActivitiesTmp
update dbo.Appraisal_HeaderNew 
set ActivityId = r.TId
from
(
select n.Id AId, n.[LocationId] ALocId, t.Id TId, h.Id HId, h.ActivityId HActId
from dbo.ActivitiesNewDecd n
inner join dbo.ActivitiesTmp t on t.LocationId = n.LocationId
inner join CDMS_DECD.dbo.Appraisal_Header h on h.ActivityId = n.Id
) as r
where dbo.Appraisal_HeaderNew.ActivityId = r.AId


-- Add the new Header records into the Appraisal_Header table.
insert into dbo.Appraisal_Header
(
[Allotment],[AllotmentStatus],[AllotmentName],[AllotmentDescription],[AllotmentComments],[ActivityId],[ByUserId]
      ,[EffDt],[CobellAppraisalWave],[LeaseTypes],[MapFiles],[TSRFiles],[FarmingLeaseFiles],[TimberAppraisalFiles]
      ,[GrazingLeaseFiles],[AllotmentPhotoFiles],[RegionalOfficeReviewFiles],[LastAppraisalRequestDate],[UpdatedTSRFile]
      ,[HasTimber],[IsMappable],[Acres],[PriorityType],[LegalDescription]
)
select [Allotment],[AllotmentStatus],[AllotmentName],[AllotmentDescription],[AllotmentComments],[ActivityId],[ByUserId]
      ,[EffDt],[CobellAppraisalWave],[LeaseTypes],[MapFiles],[TSRFiles],[FarmingLeaseFiles],[TimberAppraisalFiles]
      ,[GrazingLeaseFiles],[AllotmentPhotoFiles],[RegionalOfficeReviewFiles],[LastAppraisalRequestDate],[UpdatedTSRFile]
      ,[HasTimber],[IsMappable],[Acres],[PriorityType],[LegalDescription]
from dbo.Appraisal_HeaderNew 

update dbo.[Appraisal_Header]
set MapFiles = replace(MapFiles, '//gis.ctuir.org/DECD/Appraisals/maps/', '\\\\gis-data01\\CDMS-Share\\Prod\\P\\2246\\D\\1193\\')

-- ***** Headers end *****


-- ***** Details start *****
-- Copy in all the old detail data
set identity_insert dbo.[Appraisal_Detail] on;
go

insert into dbo.Appraisal_Detail
(
[Id],[AppraisalYear],[AppraisalFiles],[AppraisalPhotos],[AppraisalComments],[AppraisalStatus],[RowId],[RowStatusId],[ActivityId]
      ,[ByUserId],[QAStatusId],[EffDt],[AppraisalType],[AppraisalLogNumber],[AppraisalValue],[AppraisalValuationDate],[Appraiser]
      ,[TypeOfTransaction],[PartiesInvolved],[AppraisalProjectType]
)
select [Id],[AppraisalYear],[AppraisalFiles],[AppraisalPhotos],[AppraisalComments],[AppraisalStatus],[RowId],[RowStatusId],[ActivityId]
      ,[ByUserId],[QAStatusId],[EffDt],[AppraisalType],[AppraisalLogNumber],[AppraisalValue],[AppraisalValuationDate],[Appraiser]
      ,[TypeOfTransaction],[PartiesInvolved],[AppraisalProjectType]
from CDMS_DECD.dbo.Appraisal_Detail ad
where ad.ActivityId not in (select [Id] from dbo.ActivitiesNewDecd)

set identity_insert dbo.[Appraisal_Detail] off;
go


-- Capture all the new detail records
select * 
into dbo.Appraisal_DetailNew
from CDMS_DECD.dbo.Appraisal_Detail
where [ActivityId] in (select [Id] from dbo.ActivitiesNewDecd)


-- Change Old ActivityId to the new one in dbo.ActivitiesTmp
update dbo.Appraisal_DetailNew 
set ActivityId = r.TId
from
(
select n.Id AId, n.[LocationId] ALocId, t.Id TId, d.Id DId, d.ActivityId DActId
from dbo.ActivitiesNewDecd n
inner join dbo.ActivitiesTmp t on t.LocationId = n.LocationId
inner join CDMS_DECD.dbo.Appraisal_Detail d on d.ActivityId = n.Id
) as r
where dbo.Appraisal_DetailNew.ActivityId = r.AId


-- Add the new Detail records into the Appraisal_Detail table.
insert into dbo.Appraisal_Detail
(
[AppraisalYear],[AppraisalFiles],[AppraisalPhotos],[AppraisalComments],[AppraisalStatus],[RowId],[RowStatusId],[ActivityId]
      ,[ByUserId],[QAStatusId],[EffDt],[AppraisalType],[AppraisalLogNumber],[AppraisalValue],[AppraisalValuationDate],[Appraiser]
      ,[TypeOfTransaction],[PartiesInvolved],[AppraisalProjectType]
)
select [AppraisalYear],[AppraisalFiles],[AppraisalPhotos],[AppraisalComments],[AppraisalStatus],[RowId],[RowStatusId],[ActivityId]
      ,[ByUserId],[QAStatusId],[EffDt],[AppraisalType],[AppraisalLogNumber],[AppraisalValue],[AppraisalValuationDate],[Appraiser]
      ,[TypeOfTransaction],[PartiesInvolved],[AppraisalProjectType]
from dbo.Appraisal_DetailNew

-- ***** Details end *****


-- ***** Files start *****
-- Copy the old file data in
set identity_insert dbo.[Files] on;
go

insert into dbo.[Files]
(
[Id],[ProjectId],[UserId],[Name],[Title],[Description],[UploadDate],[Size],[Link],[FileTypeId],[DatasetId]
)
select [Id],[ProjectId],[UserId],[Name],[Title],[Description],[UploadDate],[Size],[Link],[FileTypeId],1193
from CDMS_DECD.dbo.[Files]
where ProjectId = 2246 and [Id] < 4371

set identity_insert dbo.[Files] off;
go

update dbo.Files set DatasetId = null where Name = '2013-10-30_1059.png'

-- Capture all the new file records
select *
into dbo.FilesNewDecd
from CDMS_DECD.dbo.Files
where ProjectId = 2246 and [Id] >= 4371

-- Add new columns to FilesNewDecd
alter table dbo.FilesNewDecd add Subproject_CrppId integer
alter table dbo.FilesNewDecd add FeatureImage integer
alter table dbo.FilesNewDecd add DatasetId integer

-- Set the DatasetId for the new records.
update dbo.FilesNewDecd set DatasetId = 1193
update dbo.FilesNewDecd set DatasetId = null where Name = 'owl.jpg'

-- Add the new records to the Files table (there are 50)
insert into dbo.Files
(
[ProjectId],[UserId],[Name],[Title],[Description],[UploadDate],[Size],[Link],[FileTypeId]
      ,[Subproject_CrppId],[FeatureImage],[DatasetId]
)
select [ProjectId],[UserId],[Name],[Title],[Description],[UploadDate],[Size],[Link],[FileTypeId]
      ,[Subproject_CrppId],[FeatureImage],[DatasetId]
from dbo.FilesNewDecd

SELECT [Id],[AppraisalFiles]
into dbo.AppraisalFiles
  FROM [CDMS_DECD].[dbo].[Appraisal_Detail]
  where AppraisalFiles is not null


update dbo.[Files]
set Link = replace(Link, 'https://cdms.ctuir.org/services/uploads', '\\gis-data01\CDMS-Share\Prod\P')
where ProjectId = 2246

update dbo.[Files]
set Link = replace(Link, '/', '\')
where ProjectId = 2246

update dbo.[MetadataValues]
set [Values] = replace([Values], 'http://cdms.ctuir.org/services/uploads/', '\\gis-data01\CDMS-Share\Prod\P\')
where RelationId = 2246 and MetadataPropertyId in (13, 25)

update dbo.[MetadataValues]
set [Values] = replace([Values], 'https://cdms.ctuir.org/services/uploads/', '\\gis-data01\CDMS-Share\Prod\P\')
where RelationId = 2246 and MetadataPropertyId in (13, 25)

update dbo.[MetadataValues]
set [Values] = replace([Values], '2246/2013', '2246\2013')
where RelationId = 2246 and MetadataPropertyId in (13, 25)

update dbo.[MetadataValues]
set [Values] = replace([Values], '2246/thCA4Q', '2246\thCA4Q')
where RelationId = 2246 and MetadataPropertyId = 13

update dbo.[Fields]
set ControlType = 'file'
  where FieldCategoryId = 5 and DbColumnName = 'AppraisalFiles'

--insert into dbo.[FileTypes] (Name, [Description])
--values ('Map', 'Map in a pdf')

-- ***** Files end *****
            ");
        }
        
        public override void Down()
        {
        }
    }
}
