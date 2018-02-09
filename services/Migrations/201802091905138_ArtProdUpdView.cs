namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtProdUpdView : DbMigration
    {
        public override void Up()
        {
            Sql(@"
DROP VIEW [dbo].[ArtificialProduction_VW]
GO
CREATE VIEW [dbo].[ArtificialProduction_VW]
AS
SELECT        h.Id AS ArtificialProduction_Header_Id, h.Comments, h.FieldSheetFile, h.ByUserId, h.EffDt, d.Id AS ArtificialProduction_Detail_Id, d.RunYear, d.Species, d.Origin, 
                         d.Sex, d.Disposition, d.TotalFishRepresented, d.LifeStage, REPLACE(REPLACE(d.FinClip, '""]', ''), '[""', '') AS FinClip, REPLACE(REPLACE(d.Tag, '""]', ''), '[""', '') AS Tag, 
                         d.NumberEggsTaken, d.ReleaseSite, d.FishComments, d.RowId, d.RowStatusId, d.ByUserId AS ArtificialProduction_Detail_ByUserId, d.ProgramGroup, d.QAStatusId,
                         d.EffDt AS ArtificialProduction_Detail_EffDt, aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId,
                         a.SourceId, a.LocationId, a.UserId AS Activity_UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, a.Id AS ActivityId, aq.QAStatusName, l.Label AS LocationLabel,
                         d.EyedEggs
FROM            dbo.Activities AS a INNER JOIN
                         dbo.ArtificialProduction_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.ArtificialProduction_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id
            ");
        }

        public override void Down()
        {
            Sql(@"
DROP VIEW [dbo].[ArtificialProduction_VW]
GO
CREATE VIEW [dbo].[ArtificialProduction_VW]
AS
SELECT        h.Id AS ArtificialProduction_Header_Id, h.Comments, h.FieldSheetFile, h.ByUserId, h.EffDt, d.Id AS ArtificialProduction_Detail_Id, d.RunYear, d.Species, d.Origin, 
                         d.Sex, d.Disposition, d.TotalFishRepresented, d.LifeStage, REPLACE(REPLACE(d.FinClip, '""]', ''), '[""', '') AS FinClip, REPLACE(REPLACE(d.Tag, '""]', ''), '[""', '') AS Tag, 
                         d.NumberEggsTaken, d.ReleaseSite, d.FishComments, d.RowId, d.RowStatusId, d.ByUserId AS ArtificialProduction_Detail_ByUserId, d.ProgramGroup, d.QAStatusId,
                         d.EffDt AS ArtificialProduction_Detail_EffDt, aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId,
                         a.SourceId, a.LocationId, a.UserId AS Activity_UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, a.Id AS ActivityId, aq.QAStatusName, dbo.Locations.Label,
                         d.EyedEggs
FROM            dbo.Activities AS a INNER JOIN
                         dbo.ArtificialProduction_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.ArtificialProduction_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id INNER JOIN
                         dbo.Locations ON a.LocationId = dbo.Locations.Id
            ");
        }
    }
}
