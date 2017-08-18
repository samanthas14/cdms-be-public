using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using services.Models;

namespace services
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            SecurityConfig.ConfigureGlobal(GlobalConfiguration.Configuration);

            Database.SetInitializer<ServicesContext>(null);

            // Remove the XML serializer so that we get JSON back when testing web requests from the browser
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new System.Web.Mvc.AuthorizeAttribute());
        }

        protected virtual void Application_BeginRequest()
        {
            HttpContext.Current.Items["_EntityContext"] = new ServicesContext();
        }

        protected virtual void Application_EndRequest()
        {
            var entityContext = HttpContext.Current.Items["_EntityContext"] as ServicesContext;
            if (entityContext != null)
                entityContext.Dispose();
        }
    }
}