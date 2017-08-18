using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models
{
    public class SubprojectFiles
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int SubprojectId { get; set; }
        public int FileId { get; set; }
        public string FileName { get; set; }
        public int? FeatureImage { get; set; }
    }
}