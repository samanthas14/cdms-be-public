using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class Drift_Header : DataHeader
    {
        public int? SampleYear { get; set; }
        public string PrePost { get; set; }
        public int? VisitId { get; set; }
        public string SampleId { get; set; }
        public string SampleClientId { get; set; }
        public int? TotalJars { get; set; }
        public float? AquaticTareMass { get; set; }
        public float? AquaticTareDryMass { get; set; }
        public float? AquaticDryMassFinal { get; set; }
        public float? ATTareMass { get; set; }
        public float? ATTareDryMass { get; set; }
        public float? ATDryMassFinal { get; set; }
        public float? TerrTareMass { get; set; }
        public float? TerrTareDryMass { get; set; }
        public float? TerrDryMassFinal { get; set; }
        public string FieldComments { get; set; }
    }
}