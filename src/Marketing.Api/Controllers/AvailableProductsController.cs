using Marketing.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Marketing.Api.Controllers
{
    [Route("api/available")]
    [ApiController]
    public class AvailableProductsController : ControllerBase
    {
        private readonly MarketingContext db;

        public AvailableProductsController(MarketingContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("products")]
        public IEnumerable<int> Get()
        {
            var all = db.ProductsDetails
                .Select(p => p.Id)
                .ToArray();

            return all;
        }
    }
}
