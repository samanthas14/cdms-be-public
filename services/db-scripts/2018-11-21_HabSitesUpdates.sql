--creates a new location type for "Habitat Sites" and modifies existing locations that are habitat sites to use it.

DECLARE @newloctypeid int = 0;

INSERT INTO LocationTypes (Name, Description) values ('Habitat Sites','Habitat Site Locations');

select @newloctypeid = scope_identity();

UPDATE Locations set LocationTypeId = @newloctypeid where subprojectid is not null and LocationTypeId = 112;

go

DECLARE @entityid int = 0;
insert into metadataentities (name, description) values ('Habitat Sites','Habitat Sites lookup fields');
select @entityid = scope_identity();

insert into metadataproperties (MetadataEntityId, Name, Description, DataType, PossibleValues, ControlType) values 
(@entityid, 'Collaborator','Collaborator','string','[]','select'),
(@entityid, 'Funder','Funder','string','[]','select');

update metadataproperties set PossibleValues = 
'[
"Blue Mountain Habitat Restoration Council",
"Bureau of Reclamation",
"Bonneville Power Authority",
"Columbia Conservation District",
"CTUIR",
"Eco Trust",
"Grande Ronde Model Watershed",
"Landowners",
"Nez Perce Tribe",
"NF John Day Watershed Council",
"Natural Resource Conservation Service",
"Oregon Department of Fish and Wildlife",
"Oregon Department of Transportation",
"Oregon Watershed Enhancement Board",
"Other",
"Pacific Coastal Salmon Recovery Fund",
"Pomeroy Conservation District",
"Salmon Recovery Funding Board",
"Snake River Salmon Recovery Board",
"Umatilla County Soil and Water Conservation District",
"Umatilla National Forest",
"US Forest Service",
"Wallowa Whitman National Forest",
"Washington Department of Fish and Wildlife"
]' where name = 'Collaborator';

update metadataproperties set PossibleValues = 
'[
"Blue Mountain Habitat Restoration Council",
"Bonneville Power Authority",
"Bureau of Reclamation",
"CTUIR",
"Eco Trust",
"Grande Ronde Model Watershed",
"NF John Day Watershed Council",
"Oregon Department of Fish and Wildlife",
"Oregon Watershed Enhancement Board",
"Other",
"Pacific Coastal Salmon Recovery Fund",
"Snake River Salmon Recovery Board",
"Umatilla County Soil and Water Conservation District",
"Umatilla National Forest",
"US Forest Service",
"Wallowa Whitman National Forest",
"Washington Department of Fish and Wildlife"
]' where name = 'Funder';

go