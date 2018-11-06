using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models
{
    public class LookupTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public System.Nullable<int> DatasetId { get; set; } //dataset with the fields for the lookup table

        [JsonIgnore]
        public virtual List<Project> Projects { get; set; }

        public virtual Dataset Dataset { get; set; }

    }
}