using JsonUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ServiceComposer.AspNetCore;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse.ViewModelComposition
{
    class ProductDetailsGetHandler : ICompositionRequestsHandler
    {
        private readonly WarehouseApi api;

        public ProductDetailsGetHandler(WarehouseApi api)
        {
            this.api = api;
        }

        [HttpGet("products/details/{id}")]
        public async Task Handle(HttpRequest request)
        {
            var id = (string)request.HttpContext.GetRouteData().Values["id"];

            var response = await this.api.GetAsync($"/api/inventory/product/{id}");

            dynamic stockItem = await response.Content.AsExpando();
            var vm = request.GetComposedResponseModel();
            vm.ProductInventory = stockItem.Inventory;
            vm.ProductOutOfStock = stockItem.Inventory == 0;
        }
    }
}
