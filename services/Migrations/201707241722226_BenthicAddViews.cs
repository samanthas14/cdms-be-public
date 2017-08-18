namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BenthicAddViews : DbMigration
    {
        public override void Up()
        {
            Sql(@"
CREATE VIEW Benthic_Detail_VW
AS
SELECT        Id, MetricTaxaRichness, MetricHilsenhoffBioticIndex, MetricChironomidae, MetricColeoptera, MetricDiptera, MetricEphemeroptera, MetricLepidoptera, 
                         MetricMegaloptera, MetricOdonata, MetricOligochaeta, MetricOtherNonInsect, MetricPlecoptera, MetricTrichoptera, MvTaxaRichness, MvERichness, MvPRichness, 
                         MvTRichness, MvPollutionSensitiveRichness, MvClingerRichness, MvSemivoltineRichness, MvPollutionTolerantPercent, MvPredatorPercent, 
                         MvDominantTaxa3Percent, MsTaxaRichness, MsERichness, MsPRichness, MsTRichness, MsPollutionSensitiveRichness, MsClingerRichness, 
                         MsSemivoltineRichness, MsPollutionTolerantPercent, MsPredatorPercent, MsDominantTaxa3Percent, BIbiScore, RowId, RowStatusId, ActivityId, ByUserId, 
                         QAStatusId, EffDt
FROM            dbo.Benthic_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Benthic_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId))) AND (RowStatusId = 0)
GO


CREATE VIEW Benthic_Header_VW
AS
SELECT        Id, SampleYear, PrePost, VisitId, SampleId, SampleClientId, TareMass, DryMass, DryMassFinal, FieldComments, ActivityId, ByUserId, EffDt
FROM            dbo.Benthic_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.Benthic_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)))
GO


CREATE VIEW Benthic_vw
AS
SELECT        a.Id AS ActivityId, a.DatasetId, a.SourceId, a.LocationId, a.UserId, a.ActivityTypeId, a.CreateDate, a.ActivityDate, h.Id, h.SampleYear, h.PrePost, h.VisitId, h.SampleId, 
                         h.SampleClientId, h.TareMass, h.DryMass, h.DryMassFinal, h.FieldComments, h.ByUserId, h.EffDt, d.Id AS Benthic_Detail_Id, d.MetricTaxaRichness, 
                         d.MetricHilsenhoffBioticIndex, d.MetricChironomidae, d.MetricColeoptera, d.MetricDiptera, d.MetricEphemeroptera, d.MetricLepidoptera, d.MetricMegaloptera, 
                         d.MetricOdonata, d.MetricOligochaeta, d.MetricOtherNonInsect, d.MetricPlecoptera, d.MetricTrichoptera, d.MvTaxaRichness, d.MvERichness, d.MvPRichness, 
                         d.MvTRichness, d.MvPollutionSensitiveRichness, d.MvClingerRichness, d.MvSemivoltineRichness, d.MvPollutionTolerantPercent, d.MvPredatorPercent, 
                         d.MvDominantTaxa3Percent, d.MsTaxaRichness, d.MsERichness, d.MsTRichness, d.MsPollutionSensitiveRichness, d.MsPRichness, d.MsClingerRichness, 
                         d.MsSemivoltineRichness, d.MsPollutionTolerantPercent, d.MsPredatorPercent, d.MsDominantTaxa3Percent, d.BIbiScore, d.RowId, 
                         d.ByUserId AS Benthic_Detail_ByUserId, d.QAStatusId, d.EffDt AS Benthic_Detail_EffDt, aq.QAStatusId AS ActivityQAStatusId, aq.UserId AS ActivityQAUserId, aq.Comments, aq.QAStatusName, 
                         l.Label AS LocationLabel
FROM            dbo.Activities AS a INNER JOIN
                         dbo.Benthic_Header_VW AS h ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.Benthic_Detail_VW AS d ON a.Id = d.ActivityId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON a.Id = aq.ActivityId INNER JOIN
                         dbo.Locations AS l ON a.LocationId = l.Id
GO
            ");
        }

        public override void Down()
        {
            Sql(@"
DROP VIEW Benthic_Detail_VW
DROP VIEW Benthic_Header_VW
DROP VIEW Benthic_vw
            ");
        }
    }
}
