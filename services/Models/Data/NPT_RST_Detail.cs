using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class NPT_RST_Detail : DataDetail
    {
        public string Lifestage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Period { get; set; }
        public int? Unmarked { get; set; }
        public int? Marked { get; set; }
        public int? Recapture { get; set; }
        public int? Abundance { get; set; }
        public decimal? StdError { get; set; }
        public int? Lower95 { get; set; }
        public int? Upper95 { get; set; }
        public string Comments { get; set; }
    }
}