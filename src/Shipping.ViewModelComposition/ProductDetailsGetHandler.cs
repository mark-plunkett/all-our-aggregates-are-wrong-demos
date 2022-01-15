﻿using JsonUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ServiceComposer.AspNetCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Shipping.ViewModelComposition
{
    class ProductDetailsGetHandler : ICompositionRequestsHandler
    {
        private readonly ShippingApi api;

        public ProductDetailsGetHandler(ShippingApi api)
        {
            this.api = api;
        }

        [HttpGet("products/details/{id}")]
        public async Task Handle(HttpRequest request)
        {
            var id = (string)request.HttpContext.GetRouteData().Values["id"];

            var response = await this.api.GetAsync($"/api/shipping-options/product/{id}");

            dynamic productShippingOptions = await response.Content.AsExpando();

            var options = ((IEnumerable<dynamic>)productShippingOptions.Options)
                .Select(o => o.Option)
                .ToArray();

            var vm = request.GetComposedResponseModel();
            vm.ProductShippingOptions = string.Join(", ", options);
        }
    }
}
