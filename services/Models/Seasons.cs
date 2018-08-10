using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models
{
    public class Seasons
    {
        public int Id { get; set; }
        public string Species { get; set; }
        public int Season { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public int TotalDays { get; set; }
    }
}