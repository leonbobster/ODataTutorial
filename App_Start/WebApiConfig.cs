using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ODataTutorial.Models;
using System.Net.Http.Formatting;
using System.Web.Http.OData.Builder;
using Newtonsoft.Json.Serialization;

namespace ODataTutorial
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
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
