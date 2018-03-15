ALTER view BSample_Detail_VW
AS
SELECT        Id, Sex, 
REPLACE(REPLACE(REPLACE(Mark, '["', ''), '"]', ''), '","', ',') AS Mark,
ForkLength, TotalLength, Weight, 
GeneticSampleId, ScaleId, SnoutId, LifeStage, Origin, Species, PITTagId, 
Tag, RadioTagId, FishComments, 
OtherTagId, KidneyId, PercentRetained, 
REPLACE(REPLACE(REPLACE(FinClip, '["', ''), '"]', ''), '","', ',') AS FinClip,
TotalCount, RecordNumber, 
MEHPLength, SubSample, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, 
EffDt
FROM            dbo.BSample_Detail AS d
WHERE        (EffDt =
(SELECT        MAX(EffDt) AS MaxEffDt
FROM            dbo.BSample_Detail AS dd
WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0);

