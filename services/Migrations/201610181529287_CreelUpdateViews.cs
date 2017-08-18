namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreelUpdateViews : DbMigration
    {
        public override void Up()
        {
                        Sql(@"
drop view [dbo].[CreelSurvey_Header_VW]
GO
CREATE VIEW [dbo].[CreelSurvey_Header_VW]
AS
SELECT        Id, Surveyor, NumberAnglersObserved, NumberAnglersInterviewed, FieldSheetFile, ByUserId, EffDt, SurveySpecies, WorkShift, WeatherConditions, TimeStart, 
                         TimeEnd, SurveyComments, Direction, ActivityId, Dry
FROM            dbo.CreelSurvey_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.CreelSurvey_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO

drop view [dbo].[CreelSurvey_Detail_VW]
GO
CREATE VIEW [dbo].[CreelSurvey_Detail_VW]
AS
SELECT        d.InterviewTime, d.InterviewComments, d.GPSEasting, d.GPSNorthing, d.RowId, d.RowStatusId, d.ActivityId, d.ByUserId, d.QAStatusId, d.EffDt, d.Id, d.FishermanId, 
                         d.TotalTimeFished, d.FishCount, d.Species, d.MethodCaught, d.Disposition, d.Sex, d.Origin, d.FinClip, d.Tag, d.OtherTagId, d.Marks, d.ForkLength, d.MeHPLength, d.SnoutId, 
                         d.ScaleId, d.CarcassComments, dbo.Fishermen.FirstName, dbo.Fishermen.Aka, dbo.Fishermen.LastName, dbo.Fishermen.PhoneNumber, dbo.Fishermen.DateAdded, 
                         dbo.Fishermen.DateInactive, dbo.Fishermen.FullName, dbo.Fishermen.FishermanComments, dbo.Fishermen.StatusId, dbo.Fishermen.Id AS FishermenId
FROM            dbo.CreelSurvey_Detail AS d INNER JOIN
                         dbo.Fishermen ON d.FishermanId = dbo.Fishermen.Id
WHERE        (d.EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.CreelSurvey_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (d.RowStatusId = 0)
GO

drop view [dbo].[CreelSurvey_VW]
GO
CREATE VIEW [dbo].[CreelSurvey_vw]
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, h.Id AS CreelSurvey_Header_ID, h.Surveyor, 
                         h.NumberAnglersObserved, h.NumberAnglersInterviewed, h.FieldSheetFile, h.ByUserId, h.EffDt, h.SurveySpecies, h.WorkShift, h.WeatherConditions, h.TimeStart, 
                         h.TimeEnd, h.SurveyComments, h.Direction, h.Dry, d.Id AS CreelSurvey_Detail_ID, d.InterviewTime, d.InterviewComments, d.GPSEasting, d.GPSNorthing, d.RowId, 
                         d.RowStatusId, d.ByUserId AS CreelSurvey_Detail_ByUserId, aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, 
                         aq.Comments AS ActivityQAComments, aq.QAStatusName, d.EffDt AS CreelSurvey_Detail_EffDt, d.TotalTimeFished, d.FishCount, d.Species, d.MethodCaught, 
                         d.Disposition, d.Sex, d.Origin, d.FinClip, d.Tag, d.OtherTagId, d.Marks, d.ForkLength, d.MeHPLength, d.SnoutId, d.ScaleId, d.CarcassComments, d.QAStatusId, d.FirstName, d.Aka, 
                         d.LastName, d.PhoneNumber, d.DateAdded, d.DateInactive, d.FullName, d.FishermanComments, d.StatusId, l.Label AS LocationLabel, w.Name AS StreamName, 
                         d.FishermanId
FROM            dbo.Activities AS a LEFT OUTER JOIN
                         dbo.CreelSurvey_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id INNER JOIN
                         dbo.WaterBodies AS w ON l.WaterBodyId = w.Id FULL OUTER JOIN
                         dbo.CreelSurvey_Header_vw AS h ON a.Id = h.ActivityId
GO
            ");
        }
        
        public override void Down()
        {
            Sql(@"
drop view [dbo].[CreelSurvey_Header_VW]
GO
CREATE VIEW [dbo].[CreelSurvey_Header_VW]
AS
SELECT        Id, Surveyor, NumberAnglersObserved, NumberAnglersInterviewed, FieldSheetFile, ByUserId, EffDt, SurveySpecies, WorkShift, WeatherConditions, TimeStart, 
                         TimeEnd, SurveyComments, Direction, ActivityId
FROM            dbo.CreelSurvey_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.CreelSurvey_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO

drop view [dbo].[CreelSurvey_Detail_VW]
GO
CREATE VIEW [dbo].[CreelSurvey_Detail_VW]
AS
SELECT        d.InterviewTime, d.InterviewComments, d.GPSEasting, d.GPSNorthing, d.RowId, d.RowStatusId, d.ActivityId, d.ByUserId, d.QAStatusId, d.EffDt, d.Id, d.FishermanId, 
                         d.TotalTimeFished, d.FishCount, d.Species, d.MethodCaught, d.Disposition, d.Sex, d.Origin, d.FinClip, d.Marks, d.ForkLength, d.MeHPLength, d.SnoutId, 
                         d.ScaleId, d.CarcassComments, dbo.Fishermen.FirstName, dbo.Fishermen.Aka, dbo.Fishermen.LastName, dbo.Fishermen.PhoneNumber, dbo.Fishermen.DateAdded, 
                         dbo.Fishermen.DateInactive, dbo.Fishermen.FullName, dbo.Fishermen.FishermanComments, dbo.Fishermen.StatusId, dbo.Fishermen.Id AS FishermenId
FROM            dbo.CreelSurvey_Detail AS d INNER JOIN
                         dbo.Fishermen ON d.FishermanId = dbo.Fishermen.Id
WHERE        (d.EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.CreelSurvey_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (d.RowStatusId = 0)
GO

drop view [dbo].[CreelSurvey_vw]
GO
CREATE VIEW [dbo].[CreelSurvey_vw]
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, h.Id AS CreelSurvey_Header_ID, h.Surveyor, 
                         h.NumberAnglersObserved, h.NumberAnglersInterviewed, h.FieldSheetFile, h.ByUserId, h.EffDt, h.SurveySpecies, h.WorkShift, h.WeatherConditions, h.TimeStart, 
                         h.TimeEnd, h.SurveyComments, h.Direction, d.Id AS CreelSurvey_Detail_ID, d.InterviewTime, d.InterviewComments, d.GPSEasting, d.GPSNorthing, d.RowId, 
                         d.RowStatusId, d.ByUserId AS CreelSurvey_Detail_ByUserId, aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, 
                         aq.Comments AS ActivityQAComments, aq.QAStatusName, d.EffDt AS CreelSurvey_Detail_EffDt, d.TotalTimeFished, d.FishCount, d.Species, d.MethodCaught, 
                         d.Disposition, d.Sex, d.Origin, d.FinClip, d.Marks, d.ForkLength, d.MeHPLength, d.SnoutId, d.ScaleId, d.CarcassComments, d.QAStatusId, d.FirstName, d.Aka, 
                         d.LastName, d.PhoneNumber, d.DateAdded, d.DateInactive, d.FullName, d.FishermanComments, d.StatusId, l.Label AS LocationLabel, w.Name AS StreamName, 
                         d.FishermanId
FROM            dbo.Activities AS a INNER JOIN
                         dbo.CreelSurvey_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id INNER JOIN
                         dbo.WaterBodies AS w ON l.WaterBodyId = w.Id FULL OUTER JOIN
                         dbo.CreelSurvey_Header_VW AS h ON a.Id = h.ActivityId
            ");
        }
    }
}
