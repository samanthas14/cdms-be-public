--creates a new location type for "Habitat Sites" and modifies existing locations that are habitat sites to use it.

DECLARE @newloctypeid int = 0;

INSERT INTO LocationTypes (Name, Description) values ('Habitat Sites','Habitat Site Locations');

select @newloctypeid = scope_identity();

UPDATE Locations set LocationTypeId = @newloctypeid where subprojectid is not null and LocationTypeId = 112;

go
