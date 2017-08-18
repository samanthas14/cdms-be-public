using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace services.Models.Data
{
    public class Subproject_Crpp
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Agency { get; set; }
        public string ProjectProponent { get; set; }
        public string TrackingNumber { get; set; }
        //public string ActionNeeded { get; set; }
        public string YearDate { get; set; }
        public string Closed { get; set; } // Note:  On this Subproject form, this is the "Additional CRPP Action Expected" box.
        public string ProjectLead { get; set; }
        public DateTime EffDt { get; set; }
        public int ByUserId { get; set; }
        public virtual List<File> Files { get; set; }
        public string County { get; set; }
        public string ProjectDescription { get; set; }
        public string UIR { get; set; }
        public string OffResTribalFee { get; set; }
        public string Comments { get; set; }

        public string OtherAgency { get; set; }
        public string OtherProjectProponent { get; set; }
        public string OtherCounty { get; set; }

        public virtual List<CorrespondenceEvents> CorrespondenceEvents { get; set; }
    }

    public class CorrespondenceEvents
    {
        public int Id { get; set; }
        public int SubprojectId { get; set; }
        public DateTime CorrespondenceDate { get; set; }
        public string ResponseType { get; set; }
        public int NumberOfDays { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string StaffMember { get; set; }
        public string EventFiles { get; set; }
        public string EventComments { get; set; }
        public DateTime EffDt { get; set; }
        public int ByUserId { get; set; }
        public string CorrespondenceType { get; set; }
        //public string ActionNeeded { get; set; }

        [JsonIgnore]
        public virtual Subproject_Crpp Subproject { get; set; }

    }
}