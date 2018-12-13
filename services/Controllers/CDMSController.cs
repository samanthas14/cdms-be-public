using NLog;
using services.Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace services.Controllers
{
    /*
     * All other CDMS Controllers subclass CDMSController, so you can add other properties or methods for them 
     *  to inherit into this class. You can also override ApiController behavior here.
     */
    public class CDMSController : ApiController
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public static string ServerEnvironment = System.Configuration.ConfigurationManager.AppSettings["ServerEnvironment"]; //in web.config
        public static string DEV_ENVIRONMENT = "dev";
        public static string TEST_ENVIRONMENT = "test";
        public static string PRODUCTION_ENVIRONMENT = "prod";


        public bool isProduction()
        {
            return (ServerEnvironment == PRODUCTION_ENVIRONMENT) ? true : false;
        }

        public bool isTest()
        {
            return (ServerEnvironment == TEST_ENVIRONMENT) ? true : false;
        }

        public bool isDev()
        {
            return (ServerEnvironment == DEV_ENVIRONMENT) ? true : false;

        }

        //log request to our analytics table
        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            return base
                .ExecuteAsync(controllerContext, cancellationToken)
                .ContinueWith(t =>
                {
                    // the controller action has finished executing, 
                    string controller = controllerContext.RequestContext.RouteData.Values["controller"].ToString();
                    string action = controllerContext.RequestContext.RouteData.Values["action"].ToString();
                    var queryparms = controllerContext.Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
                    int id = queryparms.ContainsKey("id") ? Convert.ToInt32(queryparms["id"]) : 0;
                    //var guid = controllerContext.Request.GetCorrelationId();
                    string username = controllerContext.RequestContext.Principal.Identity.Name;

                    //CDMSController.logger.Debug("Hit --- " + username + " - " + controller + " - " + action + " - " + id + " - " + guid);

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
                    {
                        con.Open();
                        var query = "insert into __Analytics (Username, Route, Action, Target) VALUES (@username, @route, @action, @target)";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@route", controller);
                            cmd.Parameters.AddWithValue("@action", action);
                            cmd.Parameters.AddWithValue("@target", id);

                            //logger.Debug(query);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    return t.Result;
                });
        }

        public HttpResponseMessage error(string message)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = new StringContent(message, System.Text.Encoding.UTF8, "text/plain");
            return response;
        }
    }
}
