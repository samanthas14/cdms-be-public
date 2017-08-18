using System;

namespace services.Models.Data
{
    public class StreamNet_SAR_Detail : DataDetail
    {
        public StreamNet_SAR_Detail()
        {
            ShadowId = Guid.NewGuid().ToString();
        }

        public string CommonName { get; set; }
        public string Run { get; set; }
        public string PopFit { get; set; }
        public string PopFitNotes { get; set; }
        public string PopAggregation { get; set; }
        public string SmoltLocation { get; set; }
        public string AdultLocation { get; set; }
        public string SARtype { get; set; }
        public string OutmigrationYear { get; set; }
        public string TRTmethod { get; set; }
        public string ContactAgency { get; set; }
        public string MethodNumber { get; set; }
        public string SAR { get; set; }
        public string RearingType { get; set; }
        public string Comments { get; set; }
        public string NullRecord { get; set; }
        public string DataStatus { get; set; }
        public string ContactPersonFirst { get; set; }
        public string ContactPersonLast { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }

        // Extra fields not exposed in the UI, but required for StreamNet. 

        public string BroodStockRemoved { get; set; }
        public string CBFWApopName { get; set; }
        public string CommonPopName { get; set; }
        public string CompilerRecordID { get; set; }
        public string DataEntry { get; set; }
        public string DataEntryNotes { get; set; }
        public string ESU_DPS { get; set; }
        public string HarvestAdj { get; set; }
        public string IndicatorLocation { get; set; }
        public string LastUpdated { get; set; }
        public string MainstemHarvest { get; set; }
        public string MajorPopGroup { get; set; }
        public string MeasureLocation { get; set; }
        public string MetaComments { get; set; }
        public string MethodAdjustments { get; set; }
        public string MetricLocation { get; set; }
        public string OceanHarvest { get; set; }
        public string PopID { get; set; }
        public string ProtMethDocumentation { get; set; }
        public string ProtMethName { get; set; }
        public string ProtMethURL { get; set; }
        public string Publish { get; set; }
        public string RecoveryDomain { get; set; }
        public string RefID { get; set; }
        public string ReturnDef { get; set; }
        public string ReturnsMissing { get; set; }
        public string ReturnsMissingExplanation { get; set; }
        public string SARAlpha { get; set; }
        public string SARLowerLimit { get; set; }
        public string SARUpperLimit { get; set; }
        public string ScopeOfInference { get; set; }
        public string SmoltLocPTcode { get; set; }
        public string SubmitAgency { get; set; }
        public string TAR { get; set; }
        public string TARAlpha { get; set; }
        public string TARLowerLimit { get; set; }
        public string TARUpperLimit { get; set; }
        public string TSO { get; set; }
        public string TSOAlpha { get; set; }
        public string TSOLowerLimit { get; set; }
        public string TSOUpperLimit { get; set; }
        public string TribHarvest { get; set; }
        public string UpdDate { get; set; }

        public String ShadowId { get; set; }
    }
}