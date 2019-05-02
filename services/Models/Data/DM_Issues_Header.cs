using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class DM_Issues_Header : DataHeader
    {
        public string SubmittedBy { get; set; }
        public string Project { get; set; }
        public string Application { get; set; }
        public string IssueType { get; set; }
        public string Keywords { get; set; }
        public string IssueDetails { get; set; }
        public string DMTTPriority { get; set; }
        public DateTime ExpectedCompletion { get; set; }
        public string CompletedBy { get; set; }
        public DateTime DateCompleted { get; set; }
    }
}