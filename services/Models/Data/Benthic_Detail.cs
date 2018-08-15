using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class Benthic_Detail : DataDetail
    {
        public decimal? MetricTaxaRichness { get; set; }
        public decimal? MetricHilsenhoffBioticIndex { get; set; }
        public decimal? MetricChironomidae { get; set; }
        public decimal? MetricColeoptera { get; set; }
        public decimal? MetricDiptera { get; set; }
        public decimal? MetricEphemeroptera { get; set; }
        public decimal? MetricLepidoptera { get; set; }
        public decimal? MetricMegaloptera { get; set; }
        public decimal? MetricOdonata { get; set; }
        public decimal? MetricOligochaeta { get; set; }
        public decimal? MetricOtherNonInsect { get; set; }
        public decimal? MetricPlecoptera { get; set; }
        public decimal? MetricTrichoptera { get; set; }
        public int? MvTaxaRichness { get; set; }
        public int? MvERichness { get; set; }
        public int? MvPRichness { get; set; }
        public int? MvTRichness { get; set; }
        public int? MvPollutionSensitiveRichness { get; set; }
        public int? MvClingerRichness { get; set; }
        public int? MvSemivoltineRichness { get; set; }
        public decimal? MvPollutionTolerantPercent { get; set; }
        public decimal? MvPredatorPercent { get; set; }
        public decimal? MvDominantTaxa3Percent { get; set; }
        public int? MsTaxaRichness { get; set; }
        public int? MsERichness { get; set; }
        public int? MsPRichness { get; set; }
        public int? MsTRichness { get; set; }
        public int? MsPollutionSensitiveRichness { get; set; }
        public int? MsClingerRichness { get; set; }
        public int? MsSemivoltineRichness { get; set; }
        //public decimal? MsPollutionTolerantPercent { get; set; }
        public decimal? MsPollutionTolerant { get; set; }
        //public decimal? MsPredatorPercent { get; set; }
        public decimal? MsPredator { get; set; }
        //public decimal? MsDominantTaxa3Percent { get; set; }
        public decimal? MsDominantTaxa3 { get; set; }
        public int? BIbiScore { get; set; }
    }
}