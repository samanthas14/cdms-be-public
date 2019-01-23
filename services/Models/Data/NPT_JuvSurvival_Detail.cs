using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using services.Resources.Attributes;

namespace services.Models.Data
{
    public class NPT_JuvSurvival_Detail : DataDetail
    {
        public string TagSite { get; set; }
        public DateTime? TagDate { get; set; }
        public string RearingVessel { get; set; }
        public string ReleaseType { get; set; }
        public string ReleaseGroup { get; set; }
        public DateTime? ReleaseStartDate { get; set; }
        public DateTime? ReleaseEndDate { get; set; }
        public string AdClipped { get; set; }
        public string Lifestage { get; set; }
        public string SurvivalTo { get; set; }
        [DecimalPrecision(5, 4)]
        public decimal? Survival { get; set; }
        [DecimalPrecision(5, 4)]
        public decimal? StdError { get; set; }
        [DecimalPrecision(5, 4)]
        public decimal? Lower95 { get; set; }
        [DecimalPrecision(5, 4)]
        public decimal? Upper95 { get; set; }
        public string Comments { get; set; }
    }
}