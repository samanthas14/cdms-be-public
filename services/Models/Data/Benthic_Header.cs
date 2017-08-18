using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class Benthic_Header : DataHeader
    {
        public int? SampleYear { get; set; }
        //public DateTime? SampleDate { get; set; } // This is actually the ActivityDate
        public string PrePost { get; set; }
        public int? VisitId { get; set; }
        public string SampleId { get; set; }
        public string SampleClientId { get; set; }
        public decimal? TareMass { get; set; }
        public decimal? DryMass { get; set; }
        public decimal? DryMassFinal { get; set; }
        public string FieldComments { get; set; }
    }
}