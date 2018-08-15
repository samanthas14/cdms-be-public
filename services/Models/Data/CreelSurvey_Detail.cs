using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Newtonsoft.Json;
using services.Models;

namespace services.Models.Data
{
    //has all fields used by both Phone and Field interviews...
    public class CreelSurvey_Detail: DataDetail
    {
        //public string InterviewTime { get; set; }
        public DateTime? InterviewTime { get; set; }
        public int? FishermanId { get; set; }
        public int? TotalTimeFished { get; set; }
        public int? FishCount { get; set; }

        public string InterviewComments { get; set; }

        //note: all decimal precision is set in the ServicesContext onmodelbuilding for EFF 5
        public decimal? GPSEasting { get; set; }
        public decimal? GPSNorthing { get; set; }

        //this gets its own specific location -- built auto-magically from teh GPS-Easting/GPS-Northing (on the UI side)
        public int? DetailLocationId { get; set; }
        public virtual Location DetailLocation { get; set; }

        // These fields are from the Carcass table
        public string Species { get; set; }
        public string MethodCaught { get; set; }
        public string Disposition { get; set; }
        public string Sex { get; set; }
        public string Origin { get; set; }
        public string FinClip { get; set; }
        public string Marks { get; set; }
        public int? ForkLength { get; set; }
        public int? MeHPLength { get; set; }
        public string SnoutId { get; set; }
        public string ScaleId { get; set; }
        public string CarcassComments { get; set; }
        public string Tag { get; set; }
        public string OtherTagId { get; set; }

        //public string WaterBody { get; set; } //this is a lookup, so we just store the matching label.
        //public string SectionNumber { get; set; }

    }
}