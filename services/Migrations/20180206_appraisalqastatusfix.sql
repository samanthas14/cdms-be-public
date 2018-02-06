-- Insert the missing datasetqastatus (5 = approved; appraisals are automatically approved / no QA workflow)
insert into DatasetQAStatus (dataset_id, qastatus_id) values (1193,5);

-- Insert any missing ActivityQAs
insert into ActivityQAs (ActivityId, QAStatusId, UserId, EffDt) 
	select a.Id, 5, 1, a.ActivityDate from Activities a where a.Id not in (select aqa.ActivityId from ActivityQAs aqa);