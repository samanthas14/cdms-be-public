using System;

namespace services.Models.Data
{
    public class SpawningGroundSurvey_Detail : DataDetail
    {
        public int? FeatureID { get; set; }
        public string FeatureType { get; set; }
        public string Species { get; set; }
        public string Time { get; set; }
        public float? Temp { get; set; }
        public string Channel { get; set; }
        public string ReddLocation { get; set; }
        public string ReddHabitat { get; set; }
        public int? WaypointNumber { get; set; }
        public int? FishCount { get; set; }
        public string FishLocation { get; set; }
        public string Sex { get; set; }
        public string FinClips { get; set; }
        public string Marks { get; set; }
        //public int? SpawningStatus { get; set; }
        public int? ForkLength { get; set; }
        public int? MeHPLength { get; set; }
        public string SnoutID { get; set; }
        public string ScaleID { get; set; }
        public string Tag { get; set; }
        public string TagID { get; set; }
        public string GeneticID { get; set; }
        public string KidneyID { get; set; }
        public string Origin { get; set; }
        public string Comments { get; set; }
        public int? Ident { get; set; }
        public double? EastingUTM { get; set; }
        public double? NorthingUTM { get; set; }
        public string EstimatedLocation { get; set; }
        public string DateTime { get; set; }

        public int? NumberEggsRetained { get; set; }
        public string MortalityType { get; set; }
        public string ReddMeasurements { get; set; }
        public string SpawningStatus { get; set; }
    }
}