namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FishScalesViewsUpd : DbMigration
    {
        public override void Up()
        {
            Sql(@"
DROP VIEW [dbo].[FishScales_vw]
GO
CREATE VIEW [dbo].[FishScales_vw]
AS
SELECT        h.RunYear, h.Technician, d.FieldScaleID, d.GumCardScaleID, d.ScaleCollectionDate, d.Species, d.Circuli, d.FreshwaterAge, d.SaltWaterAge, d.TotalAdultAge, 
                         d.SpawnCheck, d.Regeneration, d.RowId, d.RowStatusId, d.ScaleComments, d.BadScale, d.TotalAge, d.QAStatusId, a.Id AS ActivityId, a.DatasetId, a.ActivityDate, 
                         a.CreateDate, aq.QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId, d.Stock, d.LifeStage, a.LocationId, 
                         l.Label AS LocationLabel
FROM            dbo.FishScales_Detail_VW AS d LEFT OUTER JOIN
                         dbo.FishScales_Header_VW AS h ON d.ActivityId = h.ActivityId LEFT OUTER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS l ON l.Id = a.LocationId
            ");
        }
        
        public override void Down()
        {
            Sql(@"
DROP VIEW [dbo].[FishScales_vw]
GO
CREATE VIEW [dbo].[FishScales_vw]
AS
SELECT        h.RunYear, h.Technician, d.FieldScaleID, d.GumCardScaleID, d.ScaleCollectionDate, d.Species, d.Circuli, d.FreshwaterAge, d.SaltWaterAge, d.TotalAdultAge, 
                         d.SpawnCheck, d.Regeneration, d.RowId, d.RowStatusId, d.ScaleComments, d.BadScale, d.TotalAge, d.QAStatusId, a.Id AS ActivityId, a.DatasetId, a.ActivityDate, 
                         a.CreateDate, aq.QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId, d.Stock, d.LifeStage, a.LocationId
FROM            dbo.FishScales_Detail_VW AS d LEFT OUTER JOIN
                         dbo.FishScales_Header_VW AS h ON d.ActivityId = h.ActivityId LEFT OUTER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
            ");
        }
    }
}
