using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class NPT_Sturgeon_Detail : DataDetail
    {
        public string PITCode { get; set; }
        public string RKM_stream { get; set; }
        public string RKM_columbia { get; set; }
        public int? FloyTag { get; set; }
        public string ForkLength { get; set; }
        public string TotalLength { get; set; }
        public string Girth { get; set; }
        public string Weight { get; set; }
        public string EventType { get; set; }
        public string CaptureMethod { get; set; }
        public string HookType { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Comments { get; set; }
    }
}