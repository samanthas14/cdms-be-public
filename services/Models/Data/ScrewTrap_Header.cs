
using System;

namespace services.Models.Data
{
    public class ScrewTrap_Header : DataHeader
    {
        public string FileTitle { get; set; }
        public string ClipFiles { get; set; }
        public string Tagger { get; set; }
        public double? LivewellTemp { get; set; }
        public double? TaggingTemp { get; set; }
        public double? PostTaggingTemp { get; set; }
        public double? ReleaseTemp { get; set; }
        public string ArrivalTime { get; set; }
        public string DepartTime { get; set; }
        public double? ArrivalRPMs { get; set; }
        public double? DepartureRPMs { get; set; }
        //public int? Hubometer { get; set; }
        public double? Hubometer { get; set; }
        public string HubometerTime { get; set; }
        public string TrapStopped { get; set; }
        public string TrapStarted { get; set; }
        public string FishCollected { get; set; }
        public string FishReleased { get; set; }
        public string DailyFinClips { get; set; }
        public string Flow { get; set; }
        public string Turbidity { get; set; }
        public string TrapDebris { get; set; }
        public string RiverDebris { get; set; }
        public string ActivityComments { get; set; }
        public string Unit { get; set; }
        public string Crew { get; set; }
        public string TrapStatus { get; set; }
        public string Weather { get; set; }
    }
}