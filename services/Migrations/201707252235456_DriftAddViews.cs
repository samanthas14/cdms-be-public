namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DriftAddViews : DbMigration
    {
        public override void Up()
        {
            Sql(@"
CREATE VIEW Drift_Detail_VW
AS
SELECT        Id, SpeciesGroup, Taxon, LifeStage, SizeClass, TaxonCount, Qualifier, RowId, RowStatusId, ActivityId, ByUserId, QAStatusId, EffDt
FROM            dbo.Drift_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Drift_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
GO


CREATE VIEW Drift_Header_VW
AS
SELECT        Id, SampleYear, PrePost, VisitId, SampleId, TotalJars, SampleClientId, AquaticTareMass, AquaticTareDryMass, AquaticDryMassFinal, ATTareMass, ATTareDryMass, 
                         ATDryMassFinal, TerrTareMass, TerrTareDryMass, TerrDryMassFinal, FieldComments, ActivityId, ByUserId, EffDt
FROM            dbo.Drift_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Drift_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO


CREATE VIEW Drift_vw
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, h.Id, h.SampleYear, h.PrePost, h.VisitId, h.SampleId, 
                         h.TotalJars, h.SampleClientId, h.AquaticTareMass, h.AquaticTareDryMass, h.AquaticDryMassFinal, h.ATTareMass, h.ATTareDryMass, h.ATDryMassFinal, h.TerrTareMass, 
                         h.TerrTareDryMass, h.TerrDryMassFinal, h.FieldComments, h.ByUserId, h.EffDt, d.Id AS Drift_Detail_Id, d.SpeciesGroup, d.Taxon, d.LifeStage, d.SizeClass, d.TaxonCount, 
                         d.Qualifier, d.RowId, d.RowStatusId, d.ByUserId AS Drift_Detail_ByUserId, d.QAStatusId, d.EffDt AS Drift_Detail_EffDt, aq.QAStatusId AS ActivityQAStatusId, 
                         aq.UserId AS ActivityQAUserId, aq.Comments, aq.QAStatusName, l.Label AS LocationLabel
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Drift_Header_VW AS h ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.Drift_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id
GO
            ");
        }

        public override void Down()
        {
            Sql(@"
DROP VIEW Drift_Detail_VW
DROP VIEW Drift_Header_VW
DROP VIEW Drift_vw
            ");
        }
    }
}
