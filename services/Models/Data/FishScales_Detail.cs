using System;

namespace services.Models.Data
{
    public class FishScales_Detail : DataDetail
    {
        public string FieldScaleID { get; set; }
        public string GumCardScaleID { get; set; }
        public DateTime? ScaleCollectionDate { get; set; }
        public string Species { get; set; }
        public string LifeStage { get; set; }
        public double? Circuli { get; set; }
        public double? FreshwaterAge { get; set; }
        public double? SaltWaterAge { get; set; }
        public double? TotalAdultAge { get; set; }
        public string SpawnCheck { get; set; }
        public string Regeneration { get; set; }
        public string Stock { get; set; }
        public string ScaleComments { get; set; }
        public string BadScale { get; set; }
        public int? TotalAge { get; set; }
    }
}