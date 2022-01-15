using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Data;

namespace Warehouse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly WarehouseContext db;
        
        public InventoryController(WarehouseContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("product/{id}")]
        public dynamic Get(int id)
        {
            var item = db.StockItems
                .Where(o => o.ProductId == id)
                .SingleOrDefault();

            return item;
        }

        [HttpGet]
        [Route("products/{ids}")]
        public IEnumerable<dynamic> Get(string ids)
        {
            var productIds = ids.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
            var items = db.StockItems
                .Where(status => productIds.Any(id => id == status.ProductId))
                .ToArray();

            return items;
        }
    }
}
