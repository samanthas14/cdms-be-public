using Newtonsoft.Json.Linq;
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
    }
}
