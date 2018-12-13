using Newtonsoft.Json.Linq;
using services.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace services.Controllers
{
    public class SyslogController : CDMSController
    {
        [HttpPost]
        public string SystemLog(JObject jsonData)
        {
            dynamic json = jsonData;

            if (jsonData.GetValue("Type").ToString() == "AUDIT")
            {
                logger.Info(jsonData.GetValue("Message").ToString().Substring(0, 255));
            }
            else
            {
                logger.Error(jsonData.GetValue("Message").ToString().Substring(0, 255));
            }

            return "{Message: 'Success'}";
        }


        [HttpGet]
        public dynamic GetAnalytics()
        {
            var db = ServicesContext.Current;

            var sql = "SELECT * FROM Analytics_VW";

            DataTable syslogs = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                //using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(syslogs);
                }
            }

            return syslogs;
        }

    }
}
