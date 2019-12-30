using System;

namespace services.Models.Data
{
    public class StreamNet_JuvOutmigrationDetail_Detail : DataDetail
    {
        //public StreamNet_JuvOutmigrationDetail_Detail()
        //{
        //    ShadowId = Guid.NewGuid().ToString();
        //}

        public string JuvenileOutmigrantsID { get; set; }
        public string Location { get; set; }
        public string LocPTcode { get; set; }
        public string LifeStage { get; set; }
        public int TotalNatural { get; set; }
        public int TotalNaturalLowerLimit { get; set; }
        public int TotalNaturalUpperLimit { get; set; }
        public float TotalNaturalAlpha { get; set; }
        public float SurvivalRate { get; set; }
        public float SurvivalRateLowerLimit { get; set; }
        public float SurvivalRateUpperLimit { get; set; }
        public float SurvivalRateAlpha { get; set; }
        public string ContactAgency { get; set; }
        public string Comments { get; set; }
        public string JMXID { get; set; }
        public string NullRecord { get; set; }
        public string LastUpdated { get; set; }
        public string MetricLocation { get; set; }
        public string MeasureLocation { get; set; }
        // the following fields are needed for all submitted cax datasets
        public string SubmitAgency { get; set; }
        public string RefID { get; set; }
        public string UpdDate { get; set; }
        public string DataEntry { get; set; }
        public string DataEntryNotes { get; set; }
        public string CompilerRecordID { get; set; }
        public string Publish { get; set; }

        public String ShadowId { get; set; }
    }
}