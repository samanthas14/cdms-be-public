namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWaterQualityView : DbMigration
    {
        public override void Up()
        {
            // This update changes the view to user the ActivityQAs table, rather than the view.
            // Using the ActivityQAs view, with its MaxEFF stuff, cause the query/reports to time out.
            Sql(@"
create nonclustered index ix_ActivityId_EffDt on [dbo].[ActivityQAs] (ActivityId, EffDt)
go
create nonclustered index ix_ActivityId_EffDt on [dbo].[WaterQuality_Header] (ActivityId, EffDt)
go
create nonclustered index ix_ActivityId_EffDt on [dbo].[WaterQuality_Detail] (ActivityId, EffDt)
go

CREATE VIEW [dbo].[ActivityQAs_VW2]
AS
SELECT        Id, ActivityId, QAStatusId, UserId, EffDt, Comments, QAStatusName, QAStatusDescription
FROM            dbo.ActivityQAs AS a
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS Expr1
                               FROM            dbo.ActivityQAs AS aa
                               WHERE        (ActivityId = a.ActivityId)))
go

drop view WaterQuality_vw
go

CREATE VIEW [dbo].[WaterQuality_vw]
AS
SELECT        h.DataType, h.FieldsheetLink, d.CharacteristicName, d.Result, d.ResultUnits, d.LabDuplicate, d.Comments, d.RowId, d.RowStatusId, d.MdlResults, d.SampleDate, 
                         d.SampleID, d.SampleFraction, d.MethodSpeciation, d.DetectionLimit, d.ContextID, d.MethodID, d.LabName, a.Id AS ActivityId, a.DatasetId, a.InstrumentId, 
                         a.ActivityDate, a.CreateDate, loc.OtherAgencyId, loc.Id AS LocationId, aq.QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId, 
                         aq.QAStatusId AS ActivityQAStatusId
FROM            dbo.WaterQuality_Detail_VW AS d INNER JOIN
                         dbo.WaterQuality_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId INNER JOIN
                         dbo.Locations AS loc ON loc.Id = a.LocationId INNER JOIN
                         dbo.ActivityQAs_VW2 AS aq ON aq.ActivityId = a.Id
go
            ");
        }
        
        public override void Down()
        {
            // This update changes the view to user the ActivityQAs view, rather than the table.
            Sql(@"
drop index ix_ActivityId_EffDt on [dbo].[ActivityQAs]
go
drop index ix_ActivityId_EffDt on [dbo].[WaterQuality_Header]
go
drop index ix_ActivityId_EffDt on [dbo].[WaterQuality_Detail]
go


drop view ActivityQAs_VW2
go

drop view WaterQuality_vw
go

CREATE VIEW [dbo].[WaterQuality_vw]
AS
SELECT        h.DataType, h.FieldsheetLink, d.CharacteristicName, d.Result, d.ResultUnits, d.LabDuplicate, d.Comments, d.RowId, d.RowStatusId, d.MdlResults, d.SampleDate, 
                         d.SampleID, d.SampleFraction, d.MethodSpeciation, d.DetectionLimit, d.ContextID, d.MethodID, d.LabName, a.Id AS ActivityId, a.DatasetId, a.InstrumentId, 
                         a.ActivityDate, a.CreateDate, loc.OtherAgencyId, loc.Id AS LocationId, aq.QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId, 
                         aq.QAStatusId AS ActivityQAStatusId
FROM            dbo.WaterQuality_Detail_VW AS d INNER JOIN
                         dbo.WaterQuality_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId INNER JOIN
                         dbo.Locations AS loc ON loc.Id = a.LocationId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
go
            ");
        }
    }
}
