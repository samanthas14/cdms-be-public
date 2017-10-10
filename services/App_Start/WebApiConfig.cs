using System.Web.Http;
using services.Resources;

namespace services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            /* Note:  The services.js file in the front in has lots of calls that begin with mod.factory.
             * Those calls are the ones that send/request information from this (services) service.
             * The "folder" paths specified in the calls cannot be found anywhere else, and sometimes
             * may not make sense.  However, the definitions (rationale) for those paths are list below. ~GC
            */
            config.Routes.MapHttpRoute(
                name: "Api",
                routeTemplate: "api/v1/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            /*
            config.Routes.MapHttpRoute(
                name: "ControllerActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new {
                    id = RouteParameter.Optional,
                    action = "Get"
                }
            );

            //these will go away...
            config.Routes.MapHttpRoute(
                name: "RpcAction",
                routeTemplate: "action/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, controller = "Action", action="Get" }
            );
            config.Routes.MapHttpRoute(
                name: "RpcDataAction",
                routeTemplate: "data/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, controller = "DataAction", action = "Get" }
            );
            config.Routes.MapHttpRoute(
                name: "LoginAction",
                routeTemplate: "account/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, controller = "Account", action = "Get" }
            );
            */
           
            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            //config.EnableSystemDiagnosticsTracing();

            config.Filters.Add(new UnhandledExceptionFilter());

            //uncomment to allow json to be returned directly to browsers
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
        }
    }
}
