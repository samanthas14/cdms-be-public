using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class SGS_Redd_Detail : DataDetail
    {
        //Redd data

        //public string ReddType { get; set; } // delete?
        //public string ReddComments { get; set; } //delete?
        
        //WPT data - coalesced from CarcassWPT and ReddWPT tables
        public string WPTName { get; set; }
        public string WPTType {get ; set; } //New Redd, Previous Redd, Other Location
        public string ReddSpecies { get; set; } // spp list or NA if Other Location?
        public int? Count { get; set; }
        public string WPTComments { get; set;}
        public string Latitude { get; set; } // string??
        public string Longitude { get; set; } // string??
        //User Defined Fields
        public string UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string UDF3 { get; set; }
        public string UDF4 { get; set; }
        public string UDF5 { get; set; }
    }
}