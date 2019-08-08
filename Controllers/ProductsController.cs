using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using ODataTutorial.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ODataTutorial.Controllers
{
    public class ProductsController : ODataController
    {
        ProductsContext db = new ProductsContext();

        private bool ProductExists(int key)
        {
            return db.Products.Any(p => p.Id == key);
        } 

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // https://localhost:44387/Products?$count=true&pageSize=2&$filter=Category eq 'stuff'
        public PageResult<Product> Get(ODataQueryOptions<Product> options, int pageSize = 5)
        {
            var results = options.ApplyTo(db.Products.AsQueryable(), new ODataQuerySettings()
            {
                PageSize = pageSize
            });

            return new PageResult<Product>(
                results as IEnumerable<Product>,
                Request.GetNextPageLink(pageSize),
                Request.ODataProperties().TotalCount);
        }

        [EnableQuery]
        public SingleResult<Product> Get([FromODataUri] int key)
        {
            IQueryable<Product> result = db.Products.Where(p => p.Id == key);
            return SingleResult.Create(result);
        }

        public async Task<IHttpActionResult> Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return Created(product);
        }
    }
}