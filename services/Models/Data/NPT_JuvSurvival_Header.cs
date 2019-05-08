using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class NPT_JuvSurvival_Header : DataHeader
    {
        public string SpeciesRun { get; set; }
        public string Origin { get; set; }
        public string Hatchery { get; set; }
        public int? BroodYear { get; set; }
        public int MigratoryYear { get; set; }
        public string SampleType { get; set; }
    }
}