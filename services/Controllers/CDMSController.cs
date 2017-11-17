using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace services.Controllers
{
    /*
     * All other CDMS Controllers subclass CDMSController, so you can add other properties or methods for them 
     *  to inherit into this class. You can also override ApiController behavior here.
     */
    public class CDMSController : ApiController
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public static string config_server_environment = System.Configuration.ConfigurationManager.AppSettings["server_environment"]; //in web.config
        public static string PRODUCTION_ENVIRONMENT = "prod";


        public bool isProduction()
        {
            return (config_server_environment == PRODUCTION_ENVIRONMENT) ? true : false;
        }


        public HttpResponseMessage error(string message)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = new StringContent(message, System.Text.Encoding.UTF8, "text/plain");
            return response;
        }
    }
}
