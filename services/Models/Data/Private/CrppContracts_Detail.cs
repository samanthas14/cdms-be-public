using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class CrppContracts_Detail: DataDetail
    {
        public double? AcresSurveyed { get; set; }
        public string Testing { get; set; }
        public int? NewSites { get; set; }
        public int? MonitoredSites { get; set; }
        public int? SitesEvaluated { get; set; }
        public int? UpdatedSites { get; set; }
        public int? NewIsolates { get; set; }
        public string Evaluation { get; set; }
        public string HprcsitsRecorded { get; set; }
        public string Monitoring { get; set; }
        public string Notes { get; set; }
        public string ShpoReportNumber { get; set; }
        public string ShpoCaseNumber { get; set; }
    }
}