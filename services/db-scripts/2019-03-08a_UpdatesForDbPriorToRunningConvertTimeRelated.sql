--Updates to fix Prod data, prior to time update
update dbo.ScrewTrap_Header set DepartTime = '10/20/16 14:20:00' where ActivityId = 57305
update dbo.ScrewTrap_Header set DepartTime = '10/17/16 09:10:00' where ActivityId = 57302
update dbo.ScrewTrap_Header set DepartTime = '12/10/16 11:05:00' where ActivityId = 57998
update dbo.ScrewTrap_Header set DepartTime = '01/13/17 11:45:00' where ActivityId = 58827
update dbo.ScrewTrap_Header set HubometerTime = '11/03/16 10:58:00' where ActivityId = 57514
update dbo.ScrewTrap_Header set HubometerTime = '05/31/17 11:58:00' where ActivityId = 65283
update dbo.ScrewTrap_Header set TrapStarted = '08/14/16 14:00:00' where ActivityId = 56985
update dbo.ScrewTrap_Header set FishCollected = '08/31/16 10:39:00' where ActivityId = 56981
update dbo.ScrewTrap_Header set FishCollected = '04/20/18 09:20' where ActivityId = 146877
update dbo.ScrewTrap_Header set FishReleased = '08/31/16 10:39:00' where ActivityId = 56981

--Update to change AdultWeir time control type from text to time.
update dbo.DatasetFields
set ControlType = 'time'
where DatasetId in
(
	SELECT Id
		  FROM dbo.[Datasets]
		  where Name like '%Adult Weir%'
)
and DbColumnName like '%Time%'
--6 records

--Update time fields in other datasets that have ControlType of text.
update dbo.DatasetFields
set ControlType = 'time'
where [Id] in (
	select [Id]--, DatasetId, DbColumnName, ControlType
	from dbo.DatasetFields
	where DbColumnName like '%Time%'
	and ControlType = 'text'
)
--15 records

update dbo.DatasetFields
set ControlType = 'time'
where 
ControlType = 'text'
and 
(DbColumnName = 'TrapStopped'
or DbColumnName = 'TrapStarted'
or DbColumnName = 'FishCollected'
or DbColumnName = 'FishReleased'
)
--4 records

