using ODataTutorial.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.OData;
using System.Web.Http.OData.Extensions;
using System.Web.Http.OData.Query;
using System.Web.Http.Routing;
using System.Web.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ODataTutorial.Controllers
{
    public class PageResult<T>
    {
        public long? TotalCount { get; set; }
        public T Items { get; set; }
    }

    public class PageResultAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            if (actionExecutedContext.Response != null)
            {
                dynamic responseContent = null;
                if (actionExecutedContext.Response.Content != null)
                    responseContent = actionExecutedContext.Response.Content.ReadAsAsync<dynamic>().Result;
                var count = actionExecutedContext.Response.RequestMessage.ODataProperties().TotalCount;
                var response = new PageResult<dynamic>() {TotalCount = count, Items = responseContent};

                HttpResponseMessage message = new HttpResponseMessage
                {
                    StatusCode = actionExecutedContext.Response.StatusCode,
                    Content = new StringContent(
                        JsonConvert.SerializeObject(response),
                        Encoding.UTF8, 
                        "application/json")
                };

                actionExecutedContext.Response = message;
            }
        }
    }

    public class ProductsController : ApiController
    {
        [PageResult, EnableQuery, HttpGet, Route("api/foo")]
        public IQueryable<Product> Get()
        {
            var q = new ProductsContext().Set<Product>();
            //.AsQueryable<Product>();
            //q = q.Include("Category");
            //q = q.Include("Box");
            return q;
        }
    }
}