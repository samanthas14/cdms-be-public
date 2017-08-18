using System;

namespace services.Models.Data
{
    public class ScrewTrap_Detail : DataDetail
    {
        public int? Sequence { get; set; }
        public string PitTagCode { get; set; }
        public string SpeciesRunRearing { get; set; }
        public double? ForkLength { get; set; }
        public double? Weight { get; set; }
        public string OtherSpecies { get; set; }
        public int? FishCount { get; set; }
        public string ConditionalComment { get; set; }
        public string TextualComments { get; set; }
        public string Note { get; set; }
        public string ReleaseLocation { get; set; }
        public string TagStatus { get; set; }
        public string ClipStatus { get; set; }
        public string FishComments { get; set; }
        public string AdditionalPositionalComments { get; set; }

        public string EventType { get; set; }
        public string SecondPitTag { get; set; }
        public string RacewayTransectTank { get; set; }
        public string LifeStage { get; set; }
        public string GeneticId { get; set; }
        public string CodedWireTag { get; set; }
        public int? BroodYear { get; set; }
        public int? MigrationYear { get; set; }
        public string SizeOfCount { get; set; }
        public string ScaleId { get; set; }
        public string Containment { get; set; }
    }
}