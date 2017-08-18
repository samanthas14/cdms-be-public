using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class Drift_Detail : DataDetail
    {
        public string SpeciesGroup { get; set; }
        public string Taxon { get; set; }
        public string LifeStage { get; set; }
        public string SizeClass { get; set; }
        public int? TaxonCount { get; set; }
        public string Qualifier { get; set; }
    }
}