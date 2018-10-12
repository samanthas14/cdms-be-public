create view LastUpdatedDatasets_VW as 
select a.DatasetId, a.UserId, d.ProjectId, d.DatastoreId, a.CreateDate, convert(varchar, a.CreateDate, 100) as LastUpdated, d.Name as DatasetName, p.Name as ProjectName
from activities a
join datasets d on d.id = a.datasetid
join projects p on p.id = d.projectid
where a.createdate = (select max(aa.createdate) from activities aa where aa.userid=a.userid and aa.datasetid = a.datasetid)