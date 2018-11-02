-- Create a new datastore + dataset for the CRPP Correspondence dataset (no longer a tab but a bona-fide dataset)

DECLARE @crppprojectid int = 2247;
DECLARE @newdsid int = 0;
DECLARE @newdatasetid int = 0;

insert into Datastores (Name, TablePrefix, OwnerUserId, LocationTypeId, DefaultConfig) values ('CRPPCorrespondence',null,1,null,'{"ActivitiesPage":{"Route":"crppcorrespondence"}}');
select @newdsid = scope_identity();

insert into datasets 
(ProjectId, DefaultRowQAStatusId, DefaultActivityQAStatusId, StatusId, CreateDateTime, Name, Description, DatastoreId, Config) 
values 
(@crppprojectid, 1, 5, 1,getdate(),'CRPP Correspondence','CRPP Correspondence',@newdsid, '{"ActivitiesPage":{"Route":"crppcorrespondence"}}' );

select @newdatasetid = scope_identity();

go


-- now do the same for Habitat Sites.
DECLARE @newhabdsid int = 0;

insert into Datastores (Name, TablePrefix, OwnerUserId, LocationTypeId, DefaultConfig) values ('Habitat Sites',null,1,null,'{"ActivitiesPage":{"Route":"habitatsites"}}');
select @newhabdsid = scope_identity();

--add a dataset for each habitat project that should have one
insert into datasets 
(ProjectId, DefaultRowQAStatusId, DefaultActivityQAStatusId, StatusId, CreateDateTime, Name, Description, DatastoreId, Config) 
select Id,1, 5, 1,getdate(),'Habitat Sites','Habitat Sites',@newhabdsid, '{"ActivitiesPage":{"Route":"habitatsites"}}' 
from projects where Id in (select distinct RelationId from metadatavalues_vw where metadatapropertyid = 24 and [values] = 'Habitat');




