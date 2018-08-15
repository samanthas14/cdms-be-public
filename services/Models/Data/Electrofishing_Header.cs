
using System;

namespace services.Models.Data
{
    public class Electrofishing_Header : DataHeader
    {
        public string FishNumber { get; set; }
        public string EventType { get; set; }
        public string FileTitle { get; set; }
        public string ClipFiles { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? ReleaseTime { get; set; }
        public string ReleaseLocation { get; set; }
        public string VisitID { get; set; }
        public string Unit { get; set; }
        public string Crew { get; set; }
        public double? StartTemp { get; set; }
        public double? ReleaseTemp { get; set; }
        public double? Conductivity { get; set; }
        public string EFModel { get; set; }
        public double? SiteLength { get; set; }
        public double? SiteWidth { get; set; }
        public double? SiteDepth { get; set; }
        public double? SiteArea { get; set; }
        public string HabitatType { get; set; }
        public string Visibility { get; set; }
        public string ActivityComments { get; set; }
        public string ReleaseSite { get; set; }
        public string Weather { get; set; }
        public string ReleaseRiverKM { get; set; }
        public DateTime? Pass1TimeBegin { get; set; }
        public DateTime? Pass1TimeEnd { get; set; }
        public double? Pass1TotalSecondsEF { get; set; }
        public double? Pass1WaterTempBegin { get; set; }
        public double? Pass1WaterTempStop { get; set; }
        public double? Pass1Hertz { get; set; }
        public double? Pass1Freq { get; set; }
        public double? Pass1Volts { get; set; }
        public DateTime? Pass2TimeBegin { get; set; }
        public DateTime? Pass2TimeEnd { get; set; }
        public double? Pass2TotalSecondsEF { get; set; }
        public double? Pass2WaterTempBegin { get; set; }
        public double? Pass2WaterTempStop { get; set; }
        public double? Pass2Hertz { get; set; }
        public double? Pass2Freq { get; set; }
        public double? Pass2Volts { get; set; }
        public DateTime? Pass3TimeBegin { get; set; }
        public DateTime? Pass3TimeEnd { get; set; }
        public double? Pass3TotalSecondsEF { get; set; }
        public double? Pass3WaterTempBegin { get; set; }
        public double? Pass3WaterTempStop { get; set; }
        public double? Pass3Hertz { get; set; }
        public double? Pass3Freq { get; set; }
        public double? Pass3Volts { get; set; }
        public DateTime? Pass4TimeBegin { get; set; }
        public DateTime? Pass4TimeEnd { get; set; }
        public double? Pass4TotalSecondsEF { get; set; }
        public double? Pass4WaterTempBegin { get; set; }
        public double? Pass4WaterTempStop { get; set; }
        public double? Pass4Hertz { get; set; }
        public double? Pass4Freq { get; set; }
        public double? Pass4Volts { get; set; }
        public DateTime? Pass5TimeBegin { get; set; }
        public DateTime? Pass5TimeEnd { get; set; }
        public double? Pass5TotalSecondsEF { get; set; }
        public double? Pass5WaterTempBegin { get; set; }
        public double? Pass5WaterTempStop { get; set; }
        public double? Pass5Hertz { get; set; }
        public double? Pass5Freq { get; set; }
        public double? Pass5Volts { get; set; }
        public DateTime? Pass6TimeBegin { get; set; }
        public DateTime? Pass6TimeEnd { get; set; }
        public double? Pass6TotalSecondsEF { get; set; }
        public double? Pass6WaterTempBegin { get; set; }
        public double? Pass6WaterTempStop { get; set; }
        public double? Pass6Hertz { get; set; }
        public double? Pass6Freq { get; set; }
        public double? Pass6Volts { get; set; }

        public int? TotalFishCaptured { get; set; }
        public string FieldsheetLink { get; set; }
    }
}