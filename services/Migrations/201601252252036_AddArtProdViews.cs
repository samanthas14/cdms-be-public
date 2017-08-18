namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArtProdViews : DbMigration
    {
        public override void Up()
        {
            Sql(@"
CREATE VIEW [dbo].[ArtificialProduction_Detail_VW]
AS
SELECT        Id, RunYear, Species, Origin, Sex, Disposition, TotalFishRepresented, LifeStage, FinClip, Tag, NumberEggsTaken, ReleaseSite, FishComments, RowId, RowStatusId, 
                         ActivityId, ByUserId, QAStatusId, EffDt, ProgramGroup, EyedEggs
FROM            dbo.ArtificialProduction_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.ArtificialProduction_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
go


CREATE VIEW [dbo].[ArtificialProduction_Header_VW]
AS
SELECT        Id, Comments, FieldSheetFile, ActivityId, ByUserId, EffDt
FROM            dbo.ArtificialProduction_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.ArtificialProduction_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
go


CREATE VIEW [dbo].[ArtificialProduction_VW]
AS
SELECT        h.Id AS ArtificialProduction_Header_Id, h.Comments, h.FieldSheetFile, h.ByUserId, h.EffDt, d.Id AS ArtificialProduction_Detail_Id, d.RunYear, d.Species, d.Origin, 
                         d.Sex, d.Disposition, d.TotalFishRepresented, d.LifeStage, d.FinClip, d.Tag, d.NumberEggsTaken, d.ReleaseSite, d.FishComments, d.RowId, d.RowStatusId, 
                         d.ByUserId AS ArtificialProduction_Detail_ByUserId, d.ProgramGroup, d.QAStatusId, d.EffDt AS ArtificialProduction_Detail_EffDt, aq.QAStatusId AS ActivityQAStatusId, 
                         aq.UserId AS ActivityQAUserId, aq.Comments AS ActivityQAComments, a.DatasetId, a.SourceId, a.LocationId, a.UserId AS Activity_UserId, a.ActivityTypeId, 
                         a.CreateDate, a.ActivityDate, a.Id AS ActivityId, aq.QAStatusName
FROM            dbo.Activities AS a INNER JOIN
                         dbo.ArtificialProduction_Header_VW AS h ON h.ActivityId = a.Id INNER JOIN
                         dbo.ArtificialProduction_Detail_VW AS d ON d.ActivityId = a.Id INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
go
            ");
        }
        
        public override void Down()
        {
            Sql(@"
drop view [dbo].[ArtificialProduction_Detail_VW]
go
drop view [dbo].[ArtificialProduction_Header_VW]
go
drop view [dbo].[ArtificialProduction_VW]
go
            ");
        }
    }
}
