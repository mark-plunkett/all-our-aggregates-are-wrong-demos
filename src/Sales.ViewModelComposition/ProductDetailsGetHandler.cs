using JsonUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ServiceComposer.AspNetCore;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sales.ViewModelComposition
{
    class ProductDetailsGetHandler : ICompositionRequestsHandler
    {
        private readonly SalesApi api;

        public ProductDetailsGetHandler(SalesApi api)
        {
            this.api = api;
        }

        [HttpGet("products/details/{id}")]
        public async Task Handle(HttpRequest request)
        {
            var id = (string)request.HttpContext.GetRouteData().Values["id"];

            var response = await this.api.GetAsync($"/api/prices/product/{id}");

            dynamic productPrice = await response.Content.AsExpando();
            var vm = request.GetComposedResponseModel();
            vm.ProductPrice = productPrice.Price;
        }
    }
}
