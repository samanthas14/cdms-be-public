use PALUUT2_DEV
go

--This script will convert the header start time from HH:mm to utc.
declare @theMonth as nvarchar(max);
declare @theDay as nvarchar(max);
declare @theStartHour as nvarchar(max);
declare @theStartMinute as nvarchar(max);
declare @theStopHour as nvarchar(max);
declare @theStopMinute as nvarchar(max);

--Good
-- AdultWeir TimeStart, ISK-AdultWeir
raiserror(N'Updating AdultWeir TimeStart...', 0, 1) with nowait
update dbo.AdultWeir_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.hTimeStart, 0, CHARINDEX(':', r.hTimeStart))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.hTimeStart, CHARINDEX(':', r.hTimeStart)+1, LEN(r.hTimeStart))), 2),
TimeStart = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TimeStart as hTimeStart, h.TimeEnd as hTimeEnd
from dbo.Activities a
inner join dbo.AdultWeir_Header h on h.ActivityId = a.Id
--where h.TimeStart is not null and a.DatasetId = 1004 --and a.Id = 146924
where h.TimeStart is not null
) as r
where ActivityId = r.aId and TimeStart is not null;
--686 records expected

--Good
-- AdultWeir TimeEnd, ISK-AdultWeir
raiserror(N'Updating AdultWeir TimeEnd...', 0, 1) with nowait
update dbo.AdultWeir_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStopHour = right(concat('00', SUBSTRING(r.hTimeEnd, 0, CHARINDEX(':', r.hTimeEnd))), 2),
@theStopMinute = right(concat('00', SUBSTRING(r.hTimeEnd, CHARINDEX(':', r.hTimeEnd)+1, LEN(r.hTimeEnd))), 2),
TimeEnd = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStopHour, ':', @theStopMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TimeStart as hTimeStart, h.TimeEnd as hTimeEnd
from dbo.Activities a
inner join dbo.AdultWeir_Header h on h.ActivityId = a.Id
--where h.TimeStart is not null and a.DatasetId = 1004 --and a.Id = 146924
where h.TimeEnd is not null
) as r
where ActivityId = r.aId and TimeEnd is not null
--0 records expected

--Good
--ScrewTrap, UMME, ArrivalTime, have date and time, but not UTC (has a T in it)
-- in mm/dd/yy HH:mm
-- in mm/dd/yy HH:mm:ss
-- in mmm dd yyyy h:mmAM
raiserror(N'Updating ScrewTrap ArrivalTime, have date and time, but not UTC (has a T in it)...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', datepart(hour, r.ArrivalTime)), 2),
@theStartMinute = right(concat('00', datepart(minute, r.ArrivalTime)), 2),
ArrivalTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.ArrivalTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.ArrivalTime is not null and a.DatasetId = 1215 and len(h.ArrivalTime) > 5 and len(h.ArrivalTime) != 8
where h.ArrivalTime is not null and len(h.ArrivalTime) > 5 and len(h.ArrivalTime) != 8
and charIndex('T', h.ArrivalTime) = 0
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.ArrivalTime is not null and len(dbo.ScrewTrap_Header.ArrivalTime) > 5 and len(dbo.ScrewTrap_Header.ArrivalTime) != 8
and charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) = 0
--3228 records expected

--Good
--ScrewTrap, UMME, DepartTime, have date and time, but not UTC (has a T in it)
-- in mm/dd/yy HH:mm
-- in mm/dd/yy HH:mm:ss
-- in mmm dd yyyy h:mmAM
raiserror(N'Updating ScrewTrap DepartTime, have date and time, but not UTC (has a T in it)...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', datepart(hour, r.DepartTime)), 2),
@theStartMinute = right(concat('00', datepart(minute, r.DepartTime)), 2),
DepartTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.DepartTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.DepartTime is not null and a.DatasetId = 1215 and len(h.DepartTime) > 5 and len(h.DepartTime) != 8
where h.DepartTime is not null and len(h.DepartTime) > 5 and len(h.DepartTime) != 8
and charIndex('T', h.DepartTime) = 0
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.DepartTime is not null and len(dbo.ScrewTrap_Header.DepartTime) > 5 and len(dbo.ScrewTrap_Header.DepartTime) != 8
and charIndex('T', dbo.ScrewTrap_Header.DepartTime) = 0
--1491 records expected

--Good
--ScrewTrap, UMME, HubometerTime, have date and time, but not UTC (has a T in it)
-- in mm/dd/yy HH:mm
-- in mm/dd/yy HH:mm:ss
-- in mmm dd yyyy h:mmAM
raiserror(N'Updating ScrewTrap HubometerTime, have date and time, but not UTC (has a T in it)...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', datepart(hour, r.HubometerTime)), 2),
@theStartMinute = right(concat('00', datepart(minute, r.HubometerTime)), 2),
HubometerTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.HubometerTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.HubometerTime is not null and a.DatasetId = 1215 and len(h.HubometerTime) > 5 and len(h.HubometerTime) != 8
where h.HubometerTime is not null and len(h.HubometerTime) > 5 and len(h.HubometerTime) != 8
and charIndex('T', h.HubometerTime) = 0
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.HubometerTime is not null and len(dbo.ScrewTrap_Header.HubometerTime) > 5 and len(dbo.ScrewTrap_Header.HubometerTime) != 8
and charIndex('T', dbo.ScrewTrap_Header.HubometerTime) = 0
--1425 records expected

--Good
--ScrewTrap, UMME, TrapStopped, have date and time, but not UTC (has a T in it)
-- in mm/dd/yy HH:mm
-- in mm/dd/yy HH:mm:ss
-- in mmm dd yyyy h:mmAM
raiserror(N'Updating ScrewTrap TrapStopped, have date and time, but not UTC (has a T in it)...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', datepart(hour, r.TrapStopped)), 2),
@theStartMinute = right(concat('00', datepart(minute, r.TrapStopped)), 2),
TrapStopped = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStopped
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.TrapStopped is not null and a.DatasetId = 1215 and len(h.TrapStopped) > 5 and len(h.TrapStopped) != 8
where h.TrapStopped is not null and len(h.TrapStopped) > 5 and len(h.TrapStopped) != 8
and charIndex('T', h.TrapStopped) = 0
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStopped is not null and len(dbo.ScrewTrap_Header.TrapStopped) > 5 and len(dbo.ScrewTrap_Header.TrapStopped) != 8
and charIndex('T', dbo.ScrewTrap_Header.TrapStopped) = 0
--165 records expected

--Good
--ScrewTrap, UMME, TrapStarted, have date and time, but not UTC (has a T in it)
-- in mm/dd/yy HH:mm
-- in mm/dd/yy HH:mm:ss
-- in mmm dd yyyy h:mmAM
raiserror(N'Updating ScrewTrap TrapStarted, have date and time, but not UTC (has a T in it)...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', datepart(hour, r.TrapStarted)), 2),
@theStartMinute = right(concat('00', datepart(minute, r.TrapStarted)), 2),
TrapStarted = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStarted
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.TrapStarted is not null and a.DatasetId = 1215 and len(h.TrapStarted) > 5 and len(h.TrapStarted) != 8
where h.TrapStarted is not null and len(h.TrapStarted) > 5 and len(h.TrapStarted) != 8
and charIndex('T', h.TrapStarted) = 0
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStarted is not null and len(dbo.ScrewTrap_Header.TrapStarted) > 5 and len(dbo.ScrewTrap_Header.TrapStarted) != 8
and charIndex('T', dbo.ScrewTrap_Header.TrapStarted) = 0
--182 records expected

-- Good
--ScrewTrap, UMME, FishCollected, have date and time, but not UTC (has a T in it)
-- in mm/dd/yy HH:mm
-- in mm/dd/yy HH:mm:ss
-- in mmm dd yyyy h:mmAM
raiserror(N'Updating ScrewTrap FishCollected, have date and time, but not UTC (has a T in it)...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', datepart(hour, r.FishCollected)), 2),
@theStartMinute = right(concat('00', datepart(minute, r.FishCollected)), 2),
FishCollected = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.FishCollected
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.FishCollected is not null and a.DatasetId = 1215 and len(h.FishCollected) > 5 and len(h.FishCollected) != 8
where h.FishCollected is not null and len(h.FishCollected) > 5 and len(h.FishCollected) != 8
and charIndex('T', h.FishCollected) = 0
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.FishCollected is not null and len(dbo.ScrewTrap_Header.FishCollected) > 5 and len(dbo.ScrewTrap_Header.FishCollected) != 8
and charIndex('T', dbo.ScrewTrap_Header.FishCollected) = 0
--1286 records expected

--Good
--ScrewTrap, UMME, FishReleased, have date and time, but not UTC (has a T in it)
-- in mm/dd/yy HH:mm
-- in mm/dd/yy HH:mm:ss
-- in mmm dd yyyy h:mmAM
raiserror(N'Updating ScrewTrap FishReleased, have date and time, but not UTC (has a T in it)...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', datepart(hour, r.FishReleased)), 2),
@theStartMinute = right(concat('00', datepart(minute, r.FishReleased)), 2),
FishReleased = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.FishReleased
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.FishReleased is not null and a.DatasetId = 1215 and len(h.FishReleased) > 5 and len(h.FishReleased) != 8
where h.FishReleased is not null and len(h.FishReleased) > 5 and len(h.FishReleased) != 8
and charIndex('T', h.FishReleased) = 0
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.FishReleased is not null and len(dbo.ScrewTrap_Header.FishReleased) > 5 and len(dbo.ScrewTrap_Header.FishReleased) != 8
and charIndex('T', dbo.ScrewTrap_Header.FishReleased) = 0
--1341 records expected

-- Good
--ScrewTrap, ArrivalTime, have just time
raiserror(N'Updating ScrewTrap ArrivalTime...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.ArrivalTime, 0, CHARINDEX(':', r.ArrivalTime))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.ArrivalTime, CHARINDEX(':', r.ArrivalTime)+1, LEN(r.ArrivalTime))), 2),
ArrivalTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.ArrivalTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.ArrivalTime is not null and a.DatasetId = 1215 and len(h.ArrivalTime < 6) --and a.Id = 146924
where h.ArrivalTime is not null and len(h.ArrivalTime) < 6 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.ArrivalTime is not null and len(dbo.ScrewTrap_Header.ArrivalTime) < 6
--3045 records expected

-- Good
--ScrewTrap, DepartTime, have just time
raiserror(N'Updating ScrewTrap DepartTime, those records with just a time...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.DepartTime, 0, CHARINDEX(':', r.DepartTime))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.DepartTime, CHARINDEX(':', r.DepartTime)+1, LEN(r.DepartTime))), 2),
DepartTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.DepartTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.DepartTime is not null and a.DatasetId = 1215 and len(h.DepartTime < 6) --and a.Id = 146924
where h.DepartTime is not null and len(h.DepartTime) < 6 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.DepartTime is not null and len(dbo.ScrewTrap_Header.DepartTime) < 6
--2773 records expected

-- Good
--ScrewTrap, UMME, HubometerTime, have just time
raiserror(N'Updating ScrewTrap HubometerTime, those records with just a time...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.HubometerTime, 0, CHARINDEX(':', r.HubometerTime))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.HubometerTime, CHARINDEX(':', r.HubometerTime)+1, LEN(r.HubometerTime))), 2),
HubometerTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.HubometerTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.HubometerTime is not null and a.DatasetId = 1215 and len(h.HubometerTime < 6) --and a.Id = 146924
where h.HubometerTime is not null and len(h.HubometerTime) < 6 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.HubometerTime is not null and len(dbo.ScrewTrap_Header.HubometerTime) < 6
--2441 records expected

--Good
--ScrewTrap, UMME, FishCollected, have just time
raiserror(N'Updating ScrewTrap FishCollected, those records with just a time...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.FishCollected, 0, CHARINDEX(':', r.FishCollected))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.FishCollected, CHARINDEX(':', r.FishCollected)+1, LEN(r.FishCollected))), 2),
FishCollected = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.FishCollected
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.FishCollected is not null and a.DatasetId = 1215 and len(h.FishCollected < 6) --and a.Id = 146924
where h.FishCollected is not null and len(h.FishCollected) < 6 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.FishCollected is not null and len(dbo.ScrewTrap_Header.FishCollected) < 6
--2368 records expected

--Good
--ScrewTrap, UMME, FishReleased, have just time
raiserror(N'Updating ScrewTrap FishReleased, those records with just a time...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.FishReleased, 0, CHARINDEX(':', r.FishReleased))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.FishReleased, CHARINDEX(':', r.FishReleased)+1, LEN(r.FishReleased))), 2),
FishReleased = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.FishReleased
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.FishReleased is not null and a.DatasetId = 1215 and len(h.FishReleased < 6) --and a.Id = 146924
where h.FishReleased is not null and len(h.FishReleased) < 6 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.FishReleased is not null and len(dbo.ScrewTrap_Header.FishReleased) < 6
--2807 records expected

--Good
--ScrewTrap, UMME, TrapStarted, have just time
raiserror(N'Updating ScrewTrap TrapStarted, those records with just a time...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.TrapStarted, 0, CHARINDEX(':', r.TrapStarted))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.TrapStarted, CHARINDEX(':', r.TrapStarted)+1, LEN(r.TrapStarted))), 2),
TrapStarted = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStarted
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.TrapStarted is not null and a.DatasetId = 1215 and len(h.TrapStarted < 6) --and a.Id = 146924
where h.TrapStarted is not null and len(h.TrapStarted) < 6 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStarted is not null and  len(dbo.ScrewTrap_Header.TrapStarted) < 6
--1472 records expected

--Good
--ScrewTrap, UMME, ArrivalTime, have HH.mm.ss format (8-chars)
raiserror(N'Updating ScrewTrap ArrivalTime, those records with HH.mm.ss format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.aActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.aActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.aArrivalTime, 0, CHARINDEX(':', r.aArrivalTime))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.aArrivalTime, CHARINDEX(':', r.aArrivalTime)+1, 2)), 2),
ArrivalTime = concat(YEAR(r.aActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate as aActivityDate, h.ArrivalTime as aArrivalTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.[ArrivalTime is not null and a.DatasetId = 1215 and len(h.ArrivalTime) = 8 --and a.Id = 146924
where h.ArrivalTime is not null and len(h.ArrivalTime) = 8 --and a.Id = 146924
) as r
where ActivityId = r.aId and ArrivalTime is not null and len(ArrivalTime) = 8
--7 records expected

--Good
--ScrewTrap, UMME, DepartTime, have HH.mm.ss format (8-chars)
raiserror(N'Updating ScrewTrap DepartTime, those records with HH.mm.ss format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.aActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.aActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.DepartTime, 0, CHARINDEX(':', r.DepartTime))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.DepartTime, CHARINDEX(':', r.DepartTime)+1, 2)), 2),
DepartTime = concat(YEAR(r.aActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate as aActivityDate, h.DepartTime as DepartTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.DepartTime is not null and a.DatasetId = 1215 and len(h.DepartTime) = 8 --and a.Id = 146924
where h.DepartTime is not null and len(h.DepartTime) = 8 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.DepartTime is not null and len(dbo.ScrewTrap_Header.DepartTime) = 8
--6 records expected

--Good
--ScrewTrap, UMME, HubometerTime, have HH.mm.ss format (8-chars)
raiserror(N'Updating ScrewTrap HubometerTime, those records with HH.mm.ss format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.aActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.aActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.HubometerTime, 0, CHARINDEX(':', r.HubometerTime))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.HubometerTime, CHARINDEX(':', r.HubometerTime)+1, 2)), 2),
HubometerTime = concat(YEAR(r.aActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate as aActivityDate, h.HubometerTime as HubometerTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.HubometerTime is not null and a.DatasetId = 1215 and len(h.HubometerTime) = 8 --and a.Id = 146924
where h.HubometerTime is not null and len(h.HubometerTime) = 8 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.HubometerTime is not null and len(dbo.ScrewTrap_Header.HubometerTime) = 8
--6 records expected

--Good
--ScrewTrap, UMME, TrapStarted, have HH.mm.ss format (8-chars)
raiserror(N'Updating ScrewTrap TrapStarted, those records with HH.mm.ss format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.aActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.aActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.TrapStarted, 0, CHARINDEX(':', r.TrapStarted))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.TrapStarted, CHARINDEX(':', r.TrapStarted)+1, 2)), 2),
TrapStarted = concat(YEAR(r.aActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate as aActivityDate, h.TrapStarted as TrapStarted
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.TrapStarted is not null and a.DatasetId = 1215 and len(h.TrapStarted) = 8 --and a.Id = 146924
where h.TrapStarted is not null and len(h.TrapStarted) = 8 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStarted is not null and len(dbo.ScrewTrap_Header.TrapStarted) = 8
--1 record expected

--Good
--ScrewTrap, UMME, FishCollected, have HH.mm.ss format (8-chars)
raiserror(N'Updating ScrewTrap FishCollected, those records with HH.mm.ss format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.aActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.aActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.FishCollected, 0, CHARINDEX(':', r.FishCollected))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.FishCollected, CHARINDEX(':', r.FishCollected)+1, 2)), 2),
FishCollected = concat(YEAR(r.aActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate as aActivityDate, h.FishCollected as FishCollected
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.FishCollected is not null and a.DatasetId = 1215 and len(h.FishCollected) = 8 --and a.Id = 146924
where h.FishCollected is not null and len(h.FishCollected) = 8 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.FishCollected is not null and len(dbo.ScrewTrap_Header.FishCollected) = 8
--7 records expected

--Good
--ScrewTrap, UMME, FishReleased, have HH.mm.ss format (8-chars)
raiserror(N'Updating ScrewTrap FishReleased, those records with HH.mm.ss format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.aActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.aActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.FishReleased, 0, CHARINDEX(':', r.FishReleased))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.FishReleased, CHARINDEX(':', r.FishReleased)+1, 2)), 2),
FishReleased = concat(YEAR(r.aActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate as aActivityDate, h.FishReleased as FishReleased
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.FishReleased is not null and a.DatasetId = 1215 and len(h.FishReleased) = 8 --and a.Id = 146924
where h.FishReleased is not null and len(h.FishReleased) = 8 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.FishReleased is not null and len(dbo.ScrewTrap_Header.FishReleased) = 8
--9 records

--Good
--ScrewTrap, UMME, TrapStopped, have HH.mm.ss format (8-chars)
raiserror(N'Updating ScrewTrap TrapStopped, those records with HH.mm.ss format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.aActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.aActivityDate))), 2),
@theStartHour = right(concat('00', SUBSTRING(r.TrapStopped, 0, CHARINDEX(':', r.TrapStopped))), 2),
@theStartMinute = right(concat('00', SUBSTRING(r.TrapStopped, CHARINDEX(':', r.TrapStopped)+1, 2)), 2),
TrapStopped = concat(YEAR(r.aActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate as aActivityDate, h.TrapStopped as TrapStopped
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.TrapStopped is not null and a.DatasetId = 1215 and len(h.TrapStopped) = 8 --and a.Id = 146924
where h.TrapStopped is not null and len(h.TrapStopped) = 8 --and a.Id = 146924
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStopped is not null and len(dbo.ScrewTrap_Header.TrapStopped) = 8
--0 records expected

--Good
--ScrewTrap, UMME, ArrivalTime, have YYYY-MM-DD HH:mm, or THH:mm formats
raiserror(N'Updating ScrewTrap ArrivalTime, have YYYY-MM-DD HH:mm, or THH:mm formats...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.ArrivalTime, 
		charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) + 1, -- as PosOfHour,
		--charIndex(':', dbo.ScrewTrap_Header.ArrivalTime) as PosOfColon,-- - charIndex('T', h.ArrivalTime + 1)),
		((charIndex(':', dbo.ScrewTrap_Header.ArrivalTime)) - (charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.ArrivalTime, 
		charIndex(':', dbo.ScrewTrap_Header.ArrivalTime) + 1,
		2)), 
	2),
ArrivalTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.ArrivalTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.ArrivalTime is not null 
and len(h.ArrivalTime) > 6
and charIndex('T', h.ArrivalTime) > 0
and h.ArrivalTime not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.ArrivalTime is not null 
and len(dbo.ScrewTrap_Header.ArrivalTime) > 6
and charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) > 0
and dbo.ScrewTrap_Header.ArrivalTime not like '%null-%'
--3233 records

--Good
--ScrewTrap, UMME, DepartTime, have YYYY-MM-DD HH:mm, or THH:mm formats
raiserror(N'Updating ScrewTrap DepartTime, have YYYY-MM-DD HH:mm, or THH:mm formats...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.DepartTime, 
		charIndex('T', dbo.ScrewTrap_Header.DepartTime) + 1, -- as PosOfHour,
		--charIndex(':', h.DepartTime) as PosOfColon,-- - charIndex('T', h.DepartTime + 1)),
		((charIndex(':', dbo.ScrewTrap_Header.DepartTime)) - (charIndex('T', dbo.ScrewTrap_Header.DepartTime) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.DepartTime, 
		charIndex(':', dbo.ScrewTrap_Header.DepartTime) + 1,
		2)), 
	2),
DepartTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.DepartTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.DepartTime is not null 
and len(h.DepartTime) > 6
and charIndex('T', h.DepartTime) > 0
and h.DepartTime not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.DepartTime is not null 
and len(dbo.ScrewTrap_Header.DepartTime) > 6
and charIndex('T', dbo.ScrewTrap_Header.DepartTime) > 0
and dbo.ScrewTrap_Header.DepartTime not like '%null-%'
--1488

--Good
--ScrewTrap, UMME, HubometerTime, have YYYY-MM-DD HH:mm, or THH:mm formats
raiserror(N'Updating ScrewTrap HubometerTime, have YYYY-MM-DD HH:mm, or THH:mm formats...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.HubometerTime, 
		charIndex('T', dbo.ScrewTrap_Header.HubometerTime) + 1, -- as PosOfHour,
		--charIndex(':', dbo.ScrewTrap_Header.HubometerTime) as PosOfColon,-- - charIndex('T', dbo.ScrewTrap_Header.HubometerTime + 1)),
		((charIndex(':', dbo.ScrewTrap_Header.HubometerTime)) - (charIndex('T', dbo.ScrewTrap_Header.HubometerTime) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.HubometerTime, 
		charIndex(':', dbo.ScrewTrap_Header.HubometerTime) + 1,
		2)), 
	2),
HubometerTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.HubometerTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.HubometerTime is not null and a.DatasetId = 1215 and len(h.HubometerTime) > 5 and len(h.HubometerTime) != 8
where h.HubometerTime is not null and len(h.HubometerTime) > 5 and len(h.HubometerTime) != 8
and charIndex('T', h.HubometerTime) = 0
and h.HubometerTime not like '%null-%'
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.HubometerTime is not null and len(dbo.ScrewTrap_Header.HubometerTime) > 5 and len(dbo.ScrewTrap_Header.HubometerTime) != 8
and charIndex('T', dbo.ScrewTrap_Header.HubometerTime) = 0
and dbo.ScrewTrap_Header.HubometerTime not like '%null-%'
--1425 records expected

--Good
--ScrewTrap, UMME, TrapStopped, have YYYY-MM-DD HH:mm, or THH:mm formats
raiserror(N'Updating ScrewTrap TrapStopped, have YYYY-MM-DD HH:mm, or THH:mm formats...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		charIndex('T', dbo.ScrewTrap_Header.TrapStopped) + 1, -- as PosOfHour,
		--charIndex(':', h.TrapStopped) as PosOfColon,-- - charIndex('T', h.TrapStopped + 1)),
		((charIndex(':', dbo.ScrewTrap_Header.TrapStopped)) - (charIndex('T', dbo.ScrewTrap_Header.TrapStopped) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStopped) + 1,
		2)), 
	2),
TrapStopped = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStopped
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.TrapStopped is not null and a.DatasetId = 1215 and len(h.TrapStopped) > 5 and len(h.TrapStopped) != 8
where h.TrapStopped is not null and len(h.TrapStopped) > 5 and len(h.TrapStopped) != 8
and charIndex('T', h.TrapStopped) = 0
and h.TrapStopped not like '%null-%'
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStopped is not null and len(dbo.ScrewTrap_Header.TrapStopped) > 5 and len(dbo.ScrewTrap_Header.TrapStopped) != 8
and charIndex('T', dbo.ScrewTrap_Header.TrapStopped) = 0
and dbo.ScrewTrap_Header.TrapStopped not like '%null-%'
--165 records expected

--Good
--ScrewTrap, UMME, TrapStarted, have YYYY-MM-DD HH:mm, or THH:mm formats
raiserror(N'Updating ScrewTrap TrapStarted, have YYYY-MM-DD HH:mm, or THH:mm formats...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', + 
	substring(
		dbo.ScrewTrap_Header.TrapStarted, 
		charIndex('T', dbo.ScrewTrap_Header.TrapStarted) + 1, -- as PosOfHour,
		--charIndex(':', h.TrapStarted) as PosOfColon,-- - charIndex('T', h.TrapStarted + 1)),
		((charIndex(':', dbo.ScrewTrap_Header.TrapStarted)) - (charIndex('T', dbo.ScrewTrap_Header.TrapStarted) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStarted, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStarted) + 1,
		2)), 
	2),
TrapStarted = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStarted
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.TrapStarted is not null and a.DatasetId = 1215 and len(h.TrapStarted) > 5 and len(h.TrapStarted) != 8
where h.TrapStarted is not null and len(h.TrapStarted) > 5 and len(h.TrapStarted) != 8
and charIndex('T', h.TrapStarted) = 0
and h.TrapStarted not like '%null-%'
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStarted is not null and len(dbo.ScrewTrap_Header.TrapStarted) > 5 and len(dbo.ScrewTrap_Header.TrapStarted) != 8
and charIndex('T', dbo.ScrewTrap_Header.TrapStarted) = 0
and dbo.ScrewTrap_Header.TrapStarted not like '%null-%'
--182 records expected

--Good
--ScrewTrap, UMME, FishCollected, have YYYY-MM-DD HH:mm, or THH:mm formats
raiserror(N'Updating ScrewTrap FishCollected, have YYYY-MM-DD HH:mm, or THH:mm formats...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.FishCollected, 
		charIndex('T', dbo.ScrewTrap_Header.FishCollected) + 1, -- as PosOfHour,
		--charIndex(':', h.FishCollected) as PosOfColon,-- - charIndex('T', h.FishCollected + 1)),
		((charIndex(':', dbo.ScrewTrap_Header.FishCollected)) - (charIndex('T', dbo.ScrewTrap_Header.FishCollected) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.FishCollected, 
		charIndex(':', dbo.ScrewTrap_Header.FishCollected) + 1,
		2)), 
	2),
FishCollected = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.FishCollected
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.FishCollected is not null and a.DatasetId = 1215 and len(h.FishCollected) > 5 and len(h.FishCollected) != 8
where h.FishCollected is not null and len(h.FishCollected) > 5 and len(h.FishCollected) != 8
and charIndex('T', h.FishCollected) = 0
and h.FishCollected not like '%null-%'
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.FishCollected is not null and len(dbo.ScrewTrap_Header.FishCollected) > 5 and len(dbo.ScrewTrap_Header.FishCollected) != 8
and charIndex('T', dbo.ScrewTrap_Header.FishCollected) = 0
and dbo.ScrewTrap_Header.FishCollected not like '%null-%'
--1286 records

--Good
--ScrewTrap, UMME, FishReleased, have YYYY-MM-DD HH:mm, or THH:mm formats
raiserror(N'Updating ScrewTrap FishReleased, have YYYY-MM-DD HH:mm, or THH:mm formats...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.FishReleased, 
		charIndex('T', dbo.ScrewTrap_Header.FishReleased) + 1, -- as PosOfHour,
		--charIndex(':', h.FishReleased) as PosOfColon,-- - charIndex('T', h.FishReleased + 1)),
		((charIndex(':', dbo.ScrewTrap_Header.FishReleased)) - (charIndex('T', dbo.ScrewTrap_Header.FishReleased) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.FishReleased, 
		charIndex(':', dbo.ScrewTrap_Header.FishReleased) + 1,
		2)), 
	2),
FishReleased = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.FishReleased
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
--where h.FishReleased is not null and a.DatasetId = 1215 and len(h.FishReleased) > 5 and len(h.FishReleased) != 8
where h.FishReleased is not null and len(h.FishReleased) > 5 and len(h.FishReleased) != 8
and charIndex('T', h.FishReleased) = 0
and h.FishReleased not like '%null-%'
) as r
where ActivityId = r.aId and dbo.ScrewTrap_Header.FishReleased is not null and len(dbo.ScrewTrap_Header.FishReleased) > 5 and len(dbo.ScrewTrap_Header.FishReleased) != 8
and charIndex('T', dbo.ScrewTrap_Header.FishReleased) = 0
and dbo.ScrewTrap_Header.FishReleased not like '%null-%'
--1341 records expected


raiserror(N'Updating ScrewTrap ArrivalTime, have YYYY-MM-DDTHH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.ArrivalTime, 
		charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) + 1, -- as PosOfHour,
		((charIndex(':', dbo.ScrewTrap_Header.ArrivalTime)) - (charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.ArrivalTime, 
		charIndex(':', dbo.ScrewTrap_Header.ArrivalTime) + 1,
		2)), 
	2),
ArrivalTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.ArrivalTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.ArrivalTime is not null 
and len(h.ArrivalTime) > 6
and charIndex('T', h.ArrivalTime) > 0
and h.ArrivalTime not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.ArrivalTime is not null 
and len(dbo.ScrewTrap_Header.ArrivalTime) > 6
and charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) > 0
and dbo.ScrewTrap_Header.ArrivalTime not like '%null-%'


raiserror(N'Updating ScrewTrap DepartTime, have YYYY-MM-DDTHH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.DepartTime, 
		charIndex('T', dbo.ScrewTrap_Header.DepartTime) + 1, -- as PosOfHour,
		--charIndex(':', h.DepartTime) as PosOfColon,-- - charIndex('T', h.DepartTime + 1)),
		((charIndex(':', dbo.ScrewTrap_Header.DepartTime)) - (charIndex('T', dbo.ScrewTrap_Header.DepartTime) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.DepartTime, 
		charIndex(':', dbo.ScrewTrap_Header.DepartTime) + 1,
		2)), 
	2),
DepartTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.DepartTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.DepartTime is not null 
and len(h.DepartTime) > 6
and charIndex('T', h.DepartTime) > 0
and h.DepartTime not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.DepartTime is not null 
and len(dbo.ScrewTrap_Header.DepartTime) > 6
and charIndex('T', dbo.ScrewTrap_Header.DepartTime) > 0
and dbo.ScrewTrap_Header.DepartTime not like '%null-%'


raiserror(N'Updating ScrewTrap HubometerTime, have YYYY-MM-DDTHH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.HubometerTime, 
		charIndex('T', dbo.ScrewTrap_Header.HubometerTime) + 1, -- as PosOfHour,
		((charIndex(':', dbo.ScrewTrap_Header.HubometerTime)) - (charIndex('T', dbo.ScrewTrap_Header.HubometerTime) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.HubometerTime, 
		charIndex(':', dbo.ScrewTrap_Header.HubometerTime) + 1,
		2)), 
	2),
HubometerTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.HubometerTime
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.HubometerTime is not null 
and len(h.HubometerTime) > 6
and charIndex('T', h.HubometerTime) > 0
and h.HubometerTime not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.HubometerTime is not null 
and len(dbo.ScrewTrap_Header.HubometerTime) > 6
and charIndex('T', dbo.ScrewTrap_Header.HubometerTime) > 0
and dbo.ScrewTrap_Header.HubometerTime not like '%null-%'


raiserror(N'Updating ScrewTrap TrapStopped, have YYYY-MM-DDTHH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		charIndex('T', dbo.ScrewTrap_Header.TrapStopped) + 1, -- as PosOfHour,
		((charIndex(':', dbo.ScrewTrap_Header.TrapStopped)) - (charIndex('T', dbo.ScrewTrap_Header.TrapStopped) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStopped) + 1,
		2)), 
	2),
TrapStopped = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStopped
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.TrapStopped is not null 
and len(h.TrapStopped) > 6
and charIndex('T', h.TrapStopped) > 0
and h.TrapStopped not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.TrapStopped is not null 
and len(dbo.ScrewTrap_Header.TrapStopped) > 6
and charIndex('T', dbo.ScrewTrap_Header.TrapStopped) > 0
and dbo.ScrewTrap_Header.TrapStopped not like '%null-%'


raiserror(N'Updating ScrewTrap TrapStarted, have YYYY-MM-DDTHH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.TrapStarted, 
		charIndex('T', dbo.ScrewTrap_Header.TrapStarted) + 1, -- as PosOfHour,
		((charIndex(':', dbo.ScrewTrap_Header.TrapStarted)) - (charIndex('T', dbo.ScrewTrap_Header.TrapStarted) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStarted, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStarted) + 1,
		2)), 
	2),
TrapStarted = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStarted
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.TrapStarted is not null 
and len(h.TrapStarted) > 6
and charIndex('T', h.TrapStarted) > 0
and h.TrapStarted not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.TrapStarted is not null 
and len(dbo.ScrewTrap_Header.TrapStarted) > 6
and charIndex('T', dbo.ScrewTrap_Header.TrapStarted) > 0
and dbo.ScrewTrap_Header.TrapStarted not like '%null-%'


raiserror(N'Updating ScrewTrap FishCollected, have YYYY-MM-DDTHH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.FishCollected, 
		charIndex('T', dbo.ScrewTrap_Header.FishCollected) + 1, -- as PosOfHour,
		((charIndex(':', dbo.ScrewTrap_Header.FishCollected)) - (charIndex('T', dbo.ScrewTrap_Header.FishCollected) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.FishCollected, 
		charIndex(':', dbo.ScrewTrap_Header.FishCollected) + 1,
		2)), 
	2),
FishCollected = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.FishCollected
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.FishCollected is not null 
and len(h.FishCollected) > 6
and charIndex('T', h.FishCollected) > 0
and h.FishCollected not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.FishCollected is not null 
and len(dbo.ScrewTrap_Header.FishCollected) > 6
and charIndex('T', dbo.ScrewTrap_Header.FishCollected) > 0
and dbo.ScrewTrap_Header.FishCollected not like '%null-%'


raiserror(N'Updating ScrewTrap FishReleased, have YYYY-MM-DDTHH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.FishReleased, 
		charIndex('T', dbo.ScrewTrap_Header.FishReleased) + 1, -- as PosOfHour,
		((charIndex(':', dbo.ScrewTrap_Header.FishReleased)) - (charIndex('T', dbo.ScrewTrap_Header.FishReleased) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.FishReleased, 
		charIndex(':', dbo.ScrewTrap_Header.FishReleased) + 1,
		2)), 
	2),
FishReleased = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.FishReleased
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.FishReleased is not null 
and len(h.FishReleased) > 6
and charIndex('T', h.FishReleased) > 0
and h.FishReleased not like '%null-%'
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.FishReleased is not null 
and len(dbo.ScrewTrap_Header.FishReleased) > 6
and charIndex('T', dbo.ScrewTrap_Header.FishReleased) > 0
and dbo.ScrewTrap_Header.FishReleased not like '%null-%'


raiserror(N'Updating ScrewTrap ArrivalTime, has THH:mm...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.ArrivalTime, 
		charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) + 1, -- as PosOfHour,
		((charIndex(':', dbo.ScrewTrap_Header.ArrivalTime)) - (charIndex('T', dbo.ScrewTrap_Header.ArrivalTime) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.ArrivalTime, 
		charIndex(':', dbo.ScrewTrap_Header.ArrivalTime) + 1,
		2)), 
	2),
ArrivalTime = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, a.DatasetId, h.ArrivalTime, h.DepartTime, h.HubometerTime, h.TrapStopped, h.TrapStarted, h.FishCollected, h.FishReleased
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.ArrivalTime is not null
and len(h.ArrivalTime) <= 7
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.ArrivalTime is not null 
and len(dbo.ScrewTrap_Header.ArrivalTime) <= 7


raiserror(N'Updating ScrewTrap TrapStopped, has HH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00',  
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		charIndex('T', dbo.ScrewTrap_Header.TrapStopped) + 1, -- as PosOfHour,
		((charIndex(':', dbo.ScrewTrap_Header.TrapStopped)) - (charIndex('T', dbo.ScrewTrap_Header.TrapStopped) + 1)) --as Diff,
		)), --2), 
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStopped) + 1,
		2)), 
	2),
TrapStopped = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, a.DatasetId, h.ArrivalTime, h.DepartTime, h.HubometerTime, h.TrapStopped, h.TrapStarted, h.FishCollected, h.FishReleased
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.TrapStopped is not null
and h.TrapStopped != 'NA'
and len(h.TrapStopped) < 23
and len(h.TrapStopped) > 4
) as r
where ActivityId = r.aId
and dbo.ScrewTrap_Header.TrapStopped is not null 
and dbo.ScrewTrap_Header.TrapStopped != 'NA'
and len(dbo.ScrewTrap_Header.TrapStopped) < 23
and len(dbo.ScrewTrap_Header.TrapStopped) > 4

raiserror(N'Updating ScrewTrap TrapStopped, H:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		1, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStopped) - 1
		)),
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStopped) + 1,
		2)), 
	2),
TrapStopped = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStopped
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.TrapStopped is not null and len(h.TrapStopped) < 5
and h.TrapStopped != 'NA'
and len(h.TrapStopped) > 0
) as r
where dbo.ScrewTrap_Header.ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStopped is not null and len(dbo.ScrewTrap_Header.TrapStopped) < 5
and dbo.ScrewTrap_Header.TrapStopped != 'NA'
and len(dbo.ScrewTrap_Header.TrapStopped) > 0


raiserror(N'Updating ScrewTrap TrapStopped, HH:mm format...', 0, 1) with nowait
update dbo.ScrewTrap_Header
set @theMonth = right(concat('00', convert(nvarchar(max), Datepart(MONTH, r.ActivityDate))), 2), 
@theDay = right(concat('00', convert(nvarchar(max), Datepart(Day, r.ActivityDate))), 2),
@theStartHour = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		1, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStopped) - 1
		)),
	2),
@theStartMinute = right(concat('00', 
	substring(
		dbo.ScrewTrap_Header.TrapStopped, 
		charIndex(':', dbo.ScrewTrap_Header.TrapStopped) + 1,
		2)), 
	2),
TrapStopped = concat(YEAR(r.ActivityDate), '-', @theMonth, '-', @theDay, ' ', @theStartHour, ':', @theStartMinute, ':00.000')

from
(select a.[Id] as aId, a.ActivityDate, h.TrapStopped
from dbo.Activities a
inner join dbo.ScrewTrap_Header h on h.ActivityId = a.Id
where h.TrapStopped is not null and len(h.TrapStopped) < 23
and h.TrapStopped != 'NA'
and len(h.TrapStopped) > 0
) as r
where dbo.ScrewTrap_Header.ActivityId = r.aId and dbo.ScrewTrap_Header.TrapStopped is not null and len(dbo.ScrewTrap_Header.TrapStopped) < 23
and dbo.ScrewTrap_Header.TrapStopped != 'NA'
and len(dbo.ScrewTrap_Header.TrapStopped) > 0

raiserror(N'Finished!', 0, 1) with nowait