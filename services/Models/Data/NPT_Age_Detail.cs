using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class NPT_Age_Detail : DataDetail
    {
        public string SpeciesRun { get; set; }
        public string Origin { get; set; }
        public string TargetFish { get; set; }
        public string Sex { get; set; }
        public int? ForkLength { get; set; }
        public string Lifestage { get; set; }
        public string LifeHistory { get; set; }
        public DateTime CollectionDate { get; set; }
        public string UniqueFishId { get; set; }
        public string SampleNumber { get; set; }
        public string PITCode { get; set; }
        public string CWTCode { get; set; }
        public string OtherId { get; set; }
        public string OtherIdType { get; set; }
        public string CollectionRepository { get; set; }
        public string AgeDetermination { get; set; }
        public string AnalysisId { get; set; }
        public string AgeOrigin { get; set; }
        public int StreamAge { get; set; }
        public string Regenerated { get; set; }
        public int OceanAge { get; set; }
        public string RepeatSpawner { get; set; }
        public int SecondSpawnerAge { get; set; }
        public int TotalAge { get; set; }
        public string EuropeanAge { get; set; }
        public string AgeingComment { get; set; }

    }
}