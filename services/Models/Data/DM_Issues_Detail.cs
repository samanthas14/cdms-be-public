using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class DM_Issues_Detail : DataDetail
    {
        public DateTime CommentDate { get; set; }
        public string Commenter { get; set; }
        public string Comment { get; set; }
        public string PossibleOption { get; set; }
        public string Feasibility { get; set; }

    }
}