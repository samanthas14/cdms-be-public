using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class SGS_Redd_Header : DataHeader
    {
        public string TargetSpecies { get; set; }
        public int Pass { get; set; }
        public string StartSurvey { get; set; }
        public string EndSurvey { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Observers { get; set; }
        public string SurveyType { get; set; }
        public string SurveyMethod { get; set; }
        public string GPSUnit { get; set; }
        public string Weather { get; set; }
        public string Visibility { get; set; }
        public string SurveyComments { get; set; }
    }
}