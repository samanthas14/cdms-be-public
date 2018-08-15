
using System;

namespace services.Models.Data
{
    public class FishScales_Header : DataHeader
    {
        public int? RunYear { get; set; }
        public string Technician { get; set; }
    }
}