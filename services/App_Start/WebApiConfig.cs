using System.Web.Http;
using services.Resources;

namespace services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            /* 
             * This file defines the route patterns for the incoming API calls.
             * You can find the url endpoints on the front-end mainly in services.js
            */

            /*
             * NOTE: we've decided to only use RPC style api calls (and not REST style)
             * this means that calling /api/projects by itself won't work. You'll need to call
             * something like /api/project/getprojects - providing both the controller and the action you are targeting.
             * this is for consistency sake.
             */

            //api/project/getprojects or /api/project/getproject/171
            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/v1/{controller}/{action}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional
                }
            );


            /* REST style that we don't use
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1/{controller}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional
                }
            );
            */

            config.Filters.Add(new UnhandledExceptionFilter());

        }
    }
}
