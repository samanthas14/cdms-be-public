Drop view dbo.ChannelUnitMetrics_Detail_VW
go
create view dbo.ChannelUnitMetrics_Detail_VW
as
SELECT        Id, ChUnitID, ChUnitNum, Tier1, Tier2, AreaTotal, PolyArea, TotalVol, DpthMax, DpthThlwgExit, DpthResid, CountOfLWD, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt
FROM            dbo.ChannelUnitMetrics_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.ChannelUnitMetrics_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
go


Drop view dbo.ChannelUnitMetrics_Header_VW
go
create view dbo.ChannelUnitMetrics_Header_VW
as
SELECT        Id, ProgramSiteID, SiteName, WatershedID, WatershedName, SampleDate, HitchName, CrewName, VisitYear, IterationID, CategoryName, PanelName, VisitID, VisitDate, ActivityId, ByUserId, EffDt
FROM            dbo.ChannelUnitMetrics_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.ChannelUnitMetrics_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
go


Drop view dbo.ChannelUnitMetrics_vw
go
create view dbo.ChannelUnitMetrics_vw
as
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, h.Id, h.ProgramSiteID, h.SiteName, h.WatershedID, h.WatershedName, h.SampleDate, h.HitchName, h.CrewName, 
                         h.VisitYear, h.IterationID, h.CategoryName, h.PanelName, h.VisitID, h.VisitDate, h.ByUserId, h.EffDt, d.Id AS ChannelUnitMetrics_Detail_Id, d.ChUnitID, d.ChUnitNum, d.Tier1, d.Tier2, d.AreaTotal, d.PolyArea, d.TotalVol, 
                         d.DpthMax, d.DpthThlwgExit, d.DpthResid, d.CountOfLWD, d.RowId, d.ByUserId AS ChannelUnitMetrics_Detail_ByUserId, d.QAStatusId, d.EffDt AS ChannelUnitMetrics_Detail_EffDt, l.Label, l.Status, l.GPSEasting, l.GPSNorthing, 
                         l.Projection, l.UTMZone, l.Latitude, l.Longitude, l.OtherAgencyId, l.ImageLink, l.WettedWidth, l.WettedDepth, l.RiverMile, l.WaterBodyId, l.ProjectId, l.SubprojectId, l.StudyDesign, aq.QAStatusId AS ActivityQAStatusId, 
                         aq.UserId AS ActivityQAUserId, aq.Comments, aq.QAStatusName
FROM            dbo.Activities AS a INNER JOIN
                         dbo.ChannelUnitMetrics_Header AS h ON h.ActivityId = a.Id LEFT OUTER JOIN
                         dbo.ChannelUnitMetrics_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id
go