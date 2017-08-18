using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace services.Models
{
    public class Fisherman
    {
        public const int ACTIVE_STATUSID = 0;  // Active
        public const int INACTIVE_STATUSID = 1;  // Inactive
        
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Aka { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateInactive { get; set; }
        public string FullName { get; set; }
        public string FishermanComments { get; set; }
        public int StatusId { get; set; }
        //public int UserId { get; set; }
        public int? OkToCallId { get; set; }

        //the following indicate to EntityFramework to make a many to many
        [JsonIgnore]
        [InverseProperty("Fishermen")]
        public virtual List<Project> ProjectFishermen { get; set; } // Project Fisherman options

        public Fisherman()
        {
            StatusId = ACTIVE_STATUSID;
        }

    }
}