namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFishScalesViews : DbMigration
    {
        public override void Up()
        {
            Sql(@"
insert into [dbo].[Fields]
(FieldCategoryId, Name, [Description], DataType, DbColumnName, ControlType)
values
(11, 'Total Age', 'Total Age of the fish', 'int', 'TotalAge', 'number')
go

drop view [dbo].[FishScales_Detail_VW]
go
CREATE VIEW [dbo].[FishScales_Detail_VW]
AS
SELECT        Id, FieldScaleID, GumCardScaleID, ScaleCollectionDate, Species, LifeStage, Circuli, FreshwaterAge, SaltWaterAge, TotalAdultAge, SpawnCheck, Regeneration, Stock, RowId, RowStatusId, ActivityId, ByUserId, 
                         QAStatusId, EffDt, ScaleComments, BadScale, TotalAge
FROM            dbo.FishScales_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.FishScales_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId)))
GO

drop view [dbo].[FishScales_vw]
GO
CREATE VIEW [dbo].[FishScales_vw]
AS
SELECT        h.RunYear, h.Technician, d.FieldScaleID, d.GumCardScaleID, d.ScaleCollectionDate, d.Species, d.Circuli, d.FreshwaterAge, d.SaltWaterAge, d.TotalAdultAge, d.SpawnCheck, d.Regeneration, d.Stock, d.RowId, 
                         d.RowStatusId, d.ScaleComments, d.BadScale, d.TotalAge, d.QAStatusId, ef.Unit, COALESCE (st.HatNat, sgs.Origin, aw.Origin) AS HatNat, COALESCE (st.ForkLength, ef.ForkLength, sgs.ForkLength, aw.ForkLength) 
                         AS ForkLength, aw.LifeStage, COALESCE (st.Weight, ef.Weight, aw.Weight) AS Weight, COALESCE (st.AdditionalPositionalComments, ef.AdditionalPositionalComment, sgs.FinClips, aw.FinClip) AS FinClips, 
                         COALESCE (sgs.Marks, aw.Mark) AS Marks, sgs.MeHPLength, COALESCE (sgs.SpawningStatus, aw.PercentSpawned) AS PercentRetained, COALESCE (sgs.Sex, aw.Sex) AS Sex, a.Id AS ActivityId, a.DatasetId, 
                         a.InstrumentId, a.LaboratoryId, a.ActivityDate, a.CreateDate, aq.QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId, COALESCE (st.LocationLabel, ef.LocationLabel, 
                         sgs.LocationLabel) AS LocationLabel, COALESCE (st.LocationId, ef.LocationId, sgs.LocationId) AS LocationId
FROM            dbo.FishScales_Detail_VW AS d LEFT OUTER JOIN
                         dbo.FishScales_Header_VW AS h ON d.ActivityId = h.ActivityId LEFT OUTER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.Screwtrap_vw AS st ON st.TextualComments = d.FieldScaleID LEFT OUTER JOIN
                         dbo.Electrofishing_vw AS ef ON ef.TextualComments = d.FieldScaleID LEFT OUTER JOIN
                         dbo.SpawningGroundSurvey_vw AS sgs ON sgs.ScaleID = d.FieldScaleID LEFT OUTER JOIN
                         dbo.AdultWeir_vw AS aw ON aw.ScaleId = d.FieldScaleID LEFT OUTER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
GO
");
        }
        
        public override void Down()
        {
            Sql(@"
delete from [dbo].[Fields]
where FieldCategoryId = 11
and Name = 'Total Age'
and DbColumnName = 'TotalAge'
go

drop view [dbo].[FishScales_Detail_VW]
go
CREATE VIEW [dbo].[FishScales_Detail_VW]
AS
SELECT        Id, FieldScaleID, GumCardScaleID, ScaleCollectionDate, Species, LifeStage, Circuli, FreshwaterAge, SaltWaterAge, TotalAdultAge, SpawnCheck, Regeneration, Stock, RowId, RowStatusId, ActivityId, ByUserId, 
                         QAStatusId, EffDt, ScaleComments, BadScale
FROM            dbo.FishScales_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.FishScales_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId)))
GO

drop view [dbo].[FishScales_vw]
GO
CREATE VIEW [dbo].[FishScales_vw]
AS
SELECT        h.RunYear, h.Technician, d.FieldScaleID, d.GumCardScaleID, d.ScaleCollectionDate, d.Species, d.Circuli, d.FreshwaterAge, d.SaltWaterAge, d.TotalAdultAge, d.SpawnCheck, d.Regeneration, d.Stock, d.RowId, 
                         d.RowStatusId, d.ScaleComments, d.BadScale, d.QAStatusId, ef.Unit, COALESCE (st.HatNat, sgs.Origin, aw.Origin) AS HatNat, COALESCE (st.ForkLength, ef.ForkLength, sgs.ForkLength, aw.ForkLength) 
                         AS ForkLength, aw.LifeStage, COALESCE (st.Weight, ef.Weight, aw.Weight) AS Weight, COALESCE (st.AdditionalPositionalComments, ef.AdditionalPositionalComment, sgs.FinClips, aw.FinClip) AS FinClips, 
                         COALESCE (sgs.Marks, aw.Mark) AS Marks, sgs.MeHPLength, COALESCE (sgs.SpawningStatus, aw.PercentSpawned) AS PercentRetained, COALESCE (sgs.Sex, aw.Sex) AS Sex, a.Id AS ActivityId, a.DatasetId, 
                         a.InstrumentId, a.LaboratoryId, a.ActivityDate, a.CreateDate, aq.QAStatusName, aq.Comments AS ActivityQAComments, aq.QAStatusId AS ActivityQAStatusId, COALESCE (st.LocationLabel, ef.LocationLabel, 
                         sgs.LocationLabel) AS LocationLabel, COALESCE (st.LocationId, ef.LocationId, sgs.LocationId) AS LocationId
FROM            dbo.FishScales_Detail_VW AS d LEFT OUTER JOIN
                         dbo.FishScales_Header_VW AS h ON d.ActivityId = h.ActivityId LEFT OUTER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.Screwtrap_vw AS st ON st.TextualComments = d.FieldScaleID LEFT OUTER JOIN
                         dbo.Electrofishing_vw AS ef ON ef.TextualComments = d.FieldScaleID LEFT OUTER JOIN
                         dbo.SpawningGroundSurvey_vw AS sgs ON sgs.ScaleID = d.FieldScaleID LEFT OUTER JOIN
                         dbo.AdultWeir_vw AS aw ON aw.ScaleId = d.FieldScaleID LEFT OUTER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
GO
");
        }
    }
}
