create table __Analytics (
	Id INT NOT NULL IDENTITY(1,1),
	RequestTimestamp datetime default CURRENT_TIMESTAMP,
	Username varchar(255) NOT NULL,
	Route varchar(255) NOT NULL,
	Action varchar(255) NOT NULL,
	Target INT,
	PRIMARY KEY (Id)
)
go

select 
count(*) as hits, 
convert(varchar(10),RequestTimeStamp, 120) as OnDate,
username
from __analytics 
where datediff(m,RequestTimeStamp,GETDATE()) = 0
group by 
convert(varchar(10),RequestTimeStamp, 120),
username
go

