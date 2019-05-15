using Arquitetura.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Helpers;
using System.Web.SessionState;

namespace Smart.PortalAcessoSmart
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public void Application_Start(object sender, EventArgs e)
        {
            if (Arquitetura.Classes.Util.memoryManagementThread)
            {
                AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
                GlobalConfiguration.Configure(x =>
                {
                    x.MapHttpAttributeRoutes();
                    x.Filters.Add(new ValidateApiAntiForgeryRulesAttribute());
                    x.Filters.Add(new ValidateApiModelAttribute());
                    x.Filters.Add(new HandleExceptionAttribute());
                    x.Filters.Add(new DefaultAuthorizeFilterApiAttribute());
                });
                GlobalFilters.Filters.Add(new DefaultAuthorizeFilterMvcAttribute());
                GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
                AreaRegistration.RegisterAllAreas();
                RouteTable.Routes.MapRoute("Paginas", "Paginas/{controller}/{action}");
                ControllerBuilder.Current.SetControllerFactory(new PrimeTeamControllerFactory());
                ViewEngines.Engines.Clear();
                ViewEngines.Engines.Add(new ViewEngineFactory());

                //Rotinas.rotinasManagementTask()
            }
            // Code that runs on application startup
        }

        public void Application_PostAuthorizeRequest()
        {
            if (HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith("~/api/"))
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        public void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
        }

        public void Application_End(object sender, EventArgs e)
        {
            // Code that runs on application shutdown
        }

        public void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
        }

        public void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
        }

        public void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends.
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer
            // or SQLServer, the event is not raised.
        }
    }
}
