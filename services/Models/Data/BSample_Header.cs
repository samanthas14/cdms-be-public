using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class BSample_Header: DataHeader
    {
        public int? SampleYear { get; set; }
        public string Technicians { get; set; }
        public string HeaderComments { get; set; }
        public string CollectionType { get; set; }
    }
}