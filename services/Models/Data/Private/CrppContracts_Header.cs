using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class CrppContracts_Header: DataHeader
    {
        public string CrppPersonnel { get; set; }
        public int? ActivityTypeId { get; set; }
        public string Agency { get; set; }
        public string ProjectProponent { get; set; }
        public string PermitNumber { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateOfAction { get; set; }
        public string ActionTaken { get; set; }
        public string ActivityNotes { get; set; }
        public string AttachedDocument { get; set; }
    }
}