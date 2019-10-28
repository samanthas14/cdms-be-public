using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class Release_Data_Detail : DataDetail
    {

        public string TagCode_ReleaseID { get; set; }
        public string TagType { get; set; }
        public int FirstSequentialNumber { get; set; }
        public int LastSequentialNumber { get; set; }
        public string RelatedGroupType { get; set; }
        public int? BroodYear { get; set; }
        public string HatcheryLocation { get; set; }
        public string StockLocation { get; set; }
        public string ReleaseStage { get; set; }
        public string RearingType { get; set; }
        public string StudyType { get; set; }
        public string ReleaseStrategy { get; set; }
        public string StudyIntegrity { get; set; }
        public string CWT_Mark1 { get; set; }
        public int CWT_Count1 { get; set; }
        public string CWT_Mark2 { get; set; }
        public int CWT_Count2 { get; set; }
        public string NonCWT_Mark1 { get; set; }
        public int NonCWT_Count1 { get; set; }
        public string NonCWT_Mark2 { get; set; }
        public int NonCWT_Count2 { get; set; }
        public string CountingMethod { get; set; }
        public string Comments { get; set; }

    }
}