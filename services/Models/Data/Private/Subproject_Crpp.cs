using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using services.Models;
using System.Data.Entity;
using services.Models.Data;
using services.ExtensionMethods;

/* 
 * These extension methods make it possible to use linq with ctx.SomeEntity(). See below for example use.
 */
namespace services.ExtensionMethods
{
    public static class Subproject_CrppExtensions
    {
        //Extension method to give ServicesContext this property.
        public static DbSet<Subproject_Crpp> Subproject_Crpp(this ServicesContext ctx)
        {
            return ctx.GetDbSet("Subproject_Crpp").Cast<Subproject_Crpp>();
        }

        public static DbSet<CorrespondenceEvents> CorrespondenceEvents(this ServicesContext ctx)
        {
            return ctx.GetDbSet("CorrespondenceEvents").Cast<CorrespondenceEvents>();
        }
    }
}

namespace services.Models.Data
{
    public class Subproject_Crpp : Subproject
    {
        public string ProjectName { get; set; }
        public string Agency { get; set; }
        public string ProjectProponent { get; set; }
        public string TrackingNumber { get; set; }
        //public string ActionNeeded { get; set; }
        public string YearDate { get; set; }
        public string Closed { get; set; } // Note:  On this Subproject form, this is the "Additional CRPP Action Expected" box.
        public string ProjectLead { get; set; }
        public virtual List<File> Files { get; set; }
        public string County { get; set; }
        public string ProjectDescription { get; set; }
        public string UIR { get; set; }
        public string OffResTribalFee { get; set; }
        public string Comments { get; set; }

        public string OtherAgency { get; set; }
        public string OtherProjectProponent { get; set; }
        public string OtherCounty { get; set; }
        public int ProjectId { get; set; }

        public virtual List<CorrespondenceEvents> CorrespondenceEvents { get; set; }
    }

    public class CorrespondenceEvents : Subproject
    {
        public int SubprojectId { get; set; }
        public DateTime CorrespondenceDate { get; set; }
        public string ResponseType { get; set; }
        public int NumberOfDays { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string StaffMember { get; set; }
        public string EventFiles { get; set; }
        public string EventComments { get; set; }
        public string CorrespondenceType { get; set; }
        //public string ActionNeeded { get; set; }

        [JsonIgnore]
        public virtual Subproject_Crpp Subproject { get; set; }

    }
}