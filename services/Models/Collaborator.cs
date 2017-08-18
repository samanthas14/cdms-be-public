using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models
{
    public class Collaborator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SubprojectId { get; set; }
        public int ProjectId { get; set; }
    }
}