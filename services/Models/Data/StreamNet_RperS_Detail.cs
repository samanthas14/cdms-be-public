using System;

namespace services.Models.Data
{
    public class StreamNet_RperS_Detail : DataDetail
    {
        public StreamNet_RperS_Detail()
        {
            ShadowId = Guid.NewGuid().ToString();
        }

        public string CommonName { get; set; }
        public string Run { get; set; }
        public string PopFit { get; set; }
        public string SpawnerLocation { get; set; }
        public string RecruitLocation { get; set; }
        public string BroodYear { get; set; }
        public string RperStype { get; set; }
        public string TRTmethod { get; set; }
        public string ContactAgency { get; set; }
        public string MethodNumber { get; set; }
        public string RperS { get; set; }
        public string Comments { get; set; }
        public string NullRecord { get; set; }
        public string DataStatus { get; set; }
        public string ContactPersonFirst { get; set; }
        public string ContactPersonLast { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }

        // Extra fields not exposed in the UI, but required for StreamNet. 

        public string Age10Adults { get; set; }
        public string Age11PlusAdults { get; set; }
        public string Age1Juvs { get; set; }
        public string Age2Adults { get; set; }
        public string Age2Juvs { get; set; }
        public string Age3Adults { get; set; }
        public string Age3Juvs { get; set; }
        public string Age4Adults { get; set; }
        public string Age4PlusJuvs { get; set; }
        public string Age5Adults { get; set; }
        public string Age6Adults { get; set; }
        public string Age7Adults { get; set; }
        public string Age8Adults { get; set; }
        public string Age9Adults { get; set; }
        public string CBFWApopName { get; set; }
        public string CommonPopName { get; set; }
        public string CompilerRecordID { get; set; }
        public string DataEntry { get; set; }
        public string DataEntryNotes { get; set; }
        public string ESU_DPS { get; set; }
        public string HarvestAdj { get; set; }
        public string HatcherySpawners { get; set; }
        public string HatcherySpawnersAlpha { get; set; }
        public string HatcherySpawnersLowerLimit { get; set; }
        public string HatcherySpawnersUpperLimit { get; set; }
        public string IndicatorLocation { get; set; }
        public string LastUpdated { get; set; }
        public string MainstemHarvest { get; set; }
        public string MajorPopGroup { get; set; }
        public string MeasureLocation { get; set; }
        public string MetaComments { get; set; }
        public string MethodAdjustments { get; set; }
        public string MetricLocation { get; set; }
        public string NOBroodStockRemoved { get; set; }
        public string OceanHarvest { get; set; }
        public string PopFitNotes { get; set; }
        public string PopID { get; set; }
        public string ProtMethDocumentation { get; set; }
        public string ProtMethName { get; set; }
        public string ProtMethURL { get; set; }
        public string Publish { get; set; }
        public string RecoveryDomain { get; set; }
        public string Recruits { get; set; } 
        public string RecruitsAlpha { get; set; }
        public string RecruitsLowerLimit { get; set; }
        public string RecruitsMissing { get; set; }
        public string RecruitsMissingExplanation { get; set; }
        public string RecruitsUpperLimit { get; set; }
        public string RefID { get; set; }
        public string RperSAlpha { get; set; }
        public string RperSLowerLimit { get; set; }
        public string RperSUpperLimit { get; set; }
        public string SubmitAgency { get; set; }
        public string TotalSpawners { get; set; }
        public string TotalSpawnersAlpha { get; set; }
        public string TotalSpawnersLowerLimit { get; set; }
        public string TotalSpawnersUpperLimit { get; set; }
        public string TribHarvest { get; set; }
        public string UpdDate { get; set; }
        public string YOY { get; set; }

        public string ShadowId { get; set; }
    }
}