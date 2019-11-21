using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class Arrays_Efficiency_Detail : DataDetail
    {
        public string SiteId { get; set; }
        public string Node { get; set; }
        public string N_tags { get; set; }
        public string Estimate { get; set; }
        public string StDev { get; set; }
        public string LowerCI { get; set; }
        public string UpperCI { get; set; }
    }
}