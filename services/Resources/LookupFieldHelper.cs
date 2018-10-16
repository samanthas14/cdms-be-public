using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using services.Models;
using Newtonsoft.Json;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace services.Resources
{
    public class LookupFieldHelper
    {
        //executes the query and returns the "Label" as an array
        //  this is intended for use in dynamically returning lookup or multilookup controltype fields.
        public static string getPossibleValues(string raw_query)
        {
            if (raw_query == null)
                return null;

            string pv_value = raw_query.ToUpper();
            string query = pv_value.Replace("DROP", "").Replace("DELETE", "").Replace(";","").Replace("UPDATE","").Replace("CREATE","");

            DataTable possibleValues = new DataTable();

            //open a raw database connection...
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(possibleValues);
                }
            }

            return JsonConvert.SerializeObject(possibleValues); 
        }

        //returns the first item in a list no matter what kind of thing it is
        public static dynamic getFirstItem(dynamic list)
        {
            dynamic first = null;
            foreach (var item in list)
            {
                first = item;
                break;
            }
            return first;
        }

        
    }
}