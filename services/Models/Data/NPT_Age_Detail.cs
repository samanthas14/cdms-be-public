using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class NPT_Age_Detail : DataDetail
    {
        public string SampleNumber { get; set; }
        public string HistoricSampleNumber { get; set; }
        public DateTime CollectionDate { get; set; }
        public string Species { get; set; }
        public string Origin { get; set; }
        public string TargetFish { get; set; }
        public string Sex { get; set; }
        public int? ForkLength { get; set; }
        public string Lifestage { get; set; }
        public string LifeHistory { get; set; }
        public string PITCode { get; set; }
        public string CWTCode { get; set; }
        public string OtherId { get; set; }
        public string OtherId_Type { get; set; }
       // public int RepositoryId { get; set; }
        public string CollectionRepository { get; set; }
        public string AgeDetermination { get; set; }
        public string AnalysisId { get; set; }
        public string AgeOrigin { get; set; }
        public int StreamAge { get; set; }
        public int OceanAge { get; set; }
        public string RepeatSpawner { get; set; }
        public int SecondOcean { get; set; }
        public int Age { get; set; }
        public string AgeingComment { get; set; }

    }
}