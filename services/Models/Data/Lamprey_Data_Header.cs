using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class Lamprey_Data_Header : DataHeader
    {

        public string SpeciesRun { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public float WaterTemperature { get; set; }
        public float PlotLength { get; set; }
        public float PlotWidth { get; set; }
        public string PredominantHabitatType { get; set; }
        public int FirstPassTime { get; set; }
        public int FirstPassCount { get; set; }
        public int SecondPassTime { get; set; }
        public int SecondPassCount { get; set; }
        public int ThirdPassTime { get; set; }
        public int ThirdPassCount { get; set; }
        public string FishReleasedNoData { get; set; }
        public string Collector { get; set; }
        public string SurveyComment { get; set; }
    }
}