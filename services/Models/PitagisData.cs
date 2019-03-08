using services.Resources.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models
{
    public class PitagisData
    {
        public int Id { get; set; }
        public int MarkSubbasinCode { get; set; }
        public string MarkSubbasinSite { get; set; }
        public int MarkYear { get; set; }
        public DateTime MarkDate { get; set; }
        public string MarkMethodCode { get; set; }
        public int ReleaseSubbasin { get; set; }
        public string ReleaseSubbasinSite { get; set; }
        public string ReleaseSiteName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int MigrationYear { get; set; }
        public string PittagCode { get; set; }
        public string SRRCode { get; set; }
        public string SRRName { get; set; }
        public int? MarkLength { get; set; }

        [DecimalPrecision(9, 2)]
        public decimal? MarkWeight { get; set; }
        
        public string PassageSiteCode { get; set; }
        public string PassageSiteName { get; set; }
        public DateTime FirstTimeValue { get; set; }
        public DateTime LastTimeValue { get; set; }
        public int Count { get; set; }
        public int SpeciesCode { get; set; }
        public string SpeciesName { get; set; }
        public int? RunCode { get; set; }
        public string RunName { get; set; }
        public string RearTypeCode { get; set; }
        public string RearTypeName { get; set; }
        
    }
}