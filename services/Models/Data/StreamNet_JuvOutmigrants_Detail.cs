using System;

namespace services.Models.Data
{
    public class StreamNet_JuvOutmigrants_Detail : DataDetail
    {
        public StreamNet_JuvOutmigrants_Detail()
        {
            ShadowId = Guid.NewGuid().ToString();
        }

        public string CommonName { get; set; }
        public string Run { get; set; }
        public string RecoveryDomain { get; set; }
        public string ESU_DPS { get; set; }
        public string MajorPopGroup { get; set; }
        public string PopID { get; set; }
        public string CBFWApopName { get; set; }
        public string CommonPopName { get; set; }
        public string PopFit { get; set; }
        public string PopFitNotes { get; set; }
        public string SmoltEqLocation { get; set; }
        public string SmoltEqLocPTcode { get; set; }
        public string OutmigrationYear { get; set; }
        public string ContactAgency { get; set; }
        public string MethodNumber { get; set; }
        public string BestValue { get; set; }
        public string TotalNatural { get; set; }
        public string TotalNaturalLowerLimit { get; set; }
        public string TotalNaturalUpperLimit { get; set; }
        public string TotalNaturalAlpha { get; set; }
        public string Age0Prop { get; set; }
        public string Age0PropLowerLimit { get; set; }
        public string Age0PropUpperLimit { get; set; }
        public string Age1Prop { get; set; }
        public string Age1PropLowerLimit { get; set; }
        public string Age1PropUpperLimit { get; set; }
        public string Age2Prop { get; set; }
        public string Age2PropLowerLimit { get; set; }
        public string Age2PropUpperLimit { get; set; }
        public string Age3Prop { get; set; }
        public string Age3PropLowerLimit { get; set; }
        public string Age3PropUpperLimit { get; set; }
        public string Age4PlusProp { get; set; }
        public string Age4PlusPropLowerLimit { get; set; }
        public string Age4PlusPropUpperLimit { get; set; }
        public string AgePropAlpha { get; set; }
        public string ProtMethName { get; set; }
        public string ProtMethURL { get; set; }
        public string ProtMethDocumentation { get; set; }
        public string MethodAdjustments { get; set; }
        public string OtherDataSources { get; set; }
        public string Comments { get; set; }
        public string NullRecord { get; set; }
        public string DataStatus { get; set; }
        public string LastUpdated { get; set; }
        public string IndicatorLocation { get; set; }
        public string ContactPersonFirst { get; set; }
        public string ContactPersonLast { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string MetaComments { get; set; }
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