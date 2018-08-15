using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class JvRearing_Header : DataHeader
    {
        public int? AcclimationYear { get; set; }
        public string HeaderComments { get; set; }
    }
}