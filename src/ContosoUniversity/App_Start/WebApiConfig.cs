using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace ContosoUniversity
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            //default config is XMl format
            GlobalConfiguration.Configuration.Formatters.Clear();
            
            //change to json format
            GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter());
            
            //in case to retrun xml
            //GlobalConfiguration.Configuration.Formatters.Add(new XmlMediaTypeFormatter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
