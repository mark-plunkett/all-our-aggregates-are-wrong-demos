using JsonUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ServiceComposer.AspNetCore;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Marketing.ViewModelComposition;

namespace Sales.ViewModelComposition
{
    class ProductDetailsGetHandler : ICompositionRequestsHandler
    {
        private readonly MarketingApi api;

        public ProductDetailsGetHandler(MarketingApi api)
        {
            this.api = api;
        }

        [HttpGet("products/details/{id}")]
        public async Task Handle(HttpRequest request)
        {
            var id = (string)request.HttpContext.GetRouteData().Values["id"];

            var response = await this.api.GetAsync($"/api/product-details/product/{id}");

            dynamic details = await response.Content.AsExpando();
            var vm = request.GetComposedResponseModel();
            vm.ProductName = details.Name;
            vm.ProductDescription = details.Description;
        }
    }
}
