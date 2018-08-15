using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class BSample_Detail: DataDetail
    {
        public string Sex { get; set; }
        public string Mark { get; set; }
        public int? ForkLength { get; set; }
        public int? TotalLength { get; set; }
        public int? Weight { get; set; }
        public string ScaleId { get; set; }
        public string GeneticSampleId { get; set; }
        public string SnoutId { get; set; }
        public string LifeStage { get; set; }
        public string Origin { get; set; }
        public string Species { get; set; }
        public string PITTagId { get; set; }
        public string Tag { get; set; }
        public string RadioTagId { get; set; }
        public string FishComments { get; set; }
        public string OtherTagId { get; set; }
        public string KidneyId { get; set; }
        public string PercentRetained { get; set; }
        public string FinClip { get; set; }
        //public int? SampleYear { get; set; }
        public int? TotalCount { get; set; }
        public string RecordNumber { get; set; }
        public int? MEHPLength { get; set; }
        public string SubSample { get; set; }

    }
}