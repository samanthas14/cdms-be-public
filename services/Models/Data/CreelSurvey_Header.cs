using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using services.Models;

namespace services.Models.Data
{
    public class CreelSurvey_Header: DataHeader
    {
        public string SurveySpecies { get; set; }
        public string WorkShift { get; set; }
        public string Surveyor { get; set; }
        public string WeatherConditions { get; set; }
        //public string TimeStart { get; set; }
        public DateTime? TimeStart { get; set; }
        //public string TimeEnd { get; set; }
        public DateTime? TimeEnd { get; set; }
        public int? NumberAnglersObserved { get; set; }
        public int? NumberAnglersInterviewed { get; set; }
        public string SurveyComments { get; set; }
        public string FieldSheetFile { get; set; }
        public string Direction { get; set; }
        public string Dry { get; set; }
        //waterbody is via location
    }
}