using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class Release_Data_Header : DataHeader
    {
        public string RecordCode { get; set; }
        public string FormatVersion { get; set; }
        public string SubmissionDate { get; set; }
        public string ReportingAgency { get; set; }
        public string ReleaseAgency { get; set; }
        public string Coordinator { get; set; }
    }
}