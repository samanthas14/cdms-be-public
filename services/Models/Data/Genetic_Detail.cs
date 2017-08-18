using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class Genetic_Detail : DataDetail
    {
        public int? SampleYear { get; set; }
        public string GeneticId { get; set; }
        public string LifeStage { get; set; }
        public string JuvenileAge { get; set; }
        public int? ForkLength { get; set; }

        public string P1_Id { get; set; }
        public int? P1CollectYear { get; set; }
        public string P1CollectLoc { get; set; }
        public string P1Sex { get; set; }
        public string P1Origin { get; set; }

        public string P2_Id { get; set; }
        public int? P2CollectYear { get; set; }
        public string P2CollectLoc { get; set; }
        public string P2Sex { get; set; }
        public string P2Origin { get; set; }

        public string GeneticComment { get; set; }
    }
}