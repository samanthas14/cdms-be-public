using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class Lamprey_Data_Detail : DataDetail
    {
        public float Length { get; set; }
        public float Weight { get; set; }
        public string LifeStage { get; set; }
        public string Sample { get; set; }
        public string SampleNumber { get; set; }
        public string SampleComment { get; set; }

    }
}