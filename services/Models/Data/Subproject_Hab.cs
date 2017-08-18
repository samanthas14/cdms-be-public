using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace services.Models.Data
{
    public class Subproject_Hab
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectSummary { get; set; }
        public string ProjectDescription { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public string FirstFoods { get; set; }
        public string RiverVisionTouchstone { get; set; }
        public string HabitatObjectives { get; set; }
        public string NoaaEcologicalConcernsSubcategories { get; set; }
        public string NoaaEcologicalConcerns { get; set; }
        public string LimitingFactors { get; set; }
        //public string Funding { get; set; }
        public string Staff { get; set; }
        public string Collaborators { get; set; }
        public string Comments { get; set; }
        public DateTime EffDt { get; set; }
        public int ByUserId { get; set; }
        public int ProjectId { get; set; }
        public int? LocationId { get; set; }
        //public string OtherFundingAgency { get; set; }
        public string OtherCollaborators { get; set; }

        public virtual List<HabitatItem> HabitatItems { get; set; }

        //public virtual List<Funding> Funding { get; set; }
        public string FeatureImage { get; set; }
    }

    public class HabitatItem
    {
        public int Id { get; set; }
        public int SubprojectId { get; set; }
        public string ItemName { get; set; }
        public string ItemFiles { get; set; }
        public string ExternalLinks { get; set; }

        public string ItemType { get; set; }
        public DateTime EffDt { get; set; }
        public int ByUserId { get; set; }

        [JsonIgnore]
        public virtual Subproject_Hab Subproject { get; set; }

    }
}