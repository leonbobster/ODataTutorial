﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ODataTutorial.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;

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

            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products");
            builder.EntityType<Product>().Filter("Category").OrderBy("Name");
            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: null,
                model: builder.GetEdmModel());
        }
    }
}
