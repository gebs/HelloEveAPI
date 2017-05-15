using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HelloEveAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v0/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
