using System;
using System.Collections.Generic;


namespace services.Models.Data
{
    public class JvRearing_Detail : DataDetail
    {
        public string Action { get; set; }
        public string Species { get; set; }
        public string PointData { get; set; }
        public float? Result { get; set; }
        public string ResultUnit { get; set; }
        public string ActionComments { get; set; }
    }
}