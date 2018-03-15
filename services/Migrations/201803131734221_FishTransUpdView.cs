namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FishTransUpdView : DbMigration
    {
        public override void Up()
        {
            Sql(@"
drop view dbo.FishTransport_VW
go
create view dbo.FishTransport_VW
as
SELECT        h.Id AS FishTransport_Header_Id, h.Comments, h.ByUserId, h.EffDt, d.Id AS FishTransport_Detail_Id, d.ReleaseSite, d.TotalFishRepresented, 
                         d.ReleaseSiteComments, d.TransportTankUnit, d.TransportReleaseTemperature, d.TransportReleaseTemperatureF, d.TransportMortality, d.RowId, d.RowStatusId, 
                         d.ByUserId AS FishTransport_Detail_ByUserId, d.QAStatusId, d.EffDt AS FishTransport_Detail_EffDt, aq.QAStatusId AS ActivityQAStatusId, 
                         aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId, a.SourceId, a.LocationId, a.UserId AS Activity_UserId, a.ActivityTypeId, 
                         a.CreateDate, a.ActivityDate, a.Id AS ActivityId, aq.QAStatusName, l.Label AS LocationLabel
FROM            dbo.Activities AS a INNER JOIN
                         dbo.FishTransport_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.FishTransport_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS l ON l.Id = a.LocationId
            ");
        }
        
        public override void Down()
        {
            Sql(@"
drop view dbo.FishTransport_VW
go
create view dbo.FishTransport_VW
as
SELECT        h.Id AS FishTransport_Header_Id, h.Comments, h.ByUserId, h.EffDt, d.Id AS FishTransport_Detail_Id, d.ReleaseSite, d.TotalFishRepresented, 
                         d.ReleaseSiteComments, d.TransportTankUnit, d.TransportReleaseTemperature, d.TransportReleaseTemperatureF, d.TransportMortality, d.RowId, d.RowStatusId, 
                         d.ByUserId AS FishTransport_Detail_ByUserId, d.QAStatusId, d.EffDt AS FishTransport_Detail_EffDt, aq.QAStatusId AS ActivityQAStatusId, 
                         aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId, a.SourceId, a.LocationId, a.UserId AS Activity_UserId, a.ActivityTypeId, 
                         a.CreateDate, a.ActivityDate, a.Id AS ActivityId, aq.QAStatusName
FROM            dbo.Activities AS a INNER JOIN
                         dbo.FishTransport_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.FishTransport_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
            ");
        }
    }
}
