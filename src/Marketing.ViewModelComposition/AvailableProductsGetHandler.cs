using JsonUtils;
using Marketing.ViewModelComposition.Events;
using Microsoft.AspNetCore.Http;
using ServiceComposer.AspNetCore;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.ViewModelComposition
{
    class AvailableProductsGetHandler : ICompositionRequestsHandler
    {
        private readonly MarketingApi api;

        public AvailableProductsGetHandler(MarketingApi api)
        {
            this.api = api;
        }

        [HttpGet("/")]
        public async Task Handle(HttpRequest request)
        {
            var response = await this.api.GetAsync("api/available/products");

            var availableProducts = await response.Content.As<int[]>();
            var availableProductsViewModel = MapToDictionary(availableProducts);
            var vm = request.GetComposedResponseModel();
            await vm.RaiseEvent(new AvailableProductsLoaded()
            {
                AvailableProductsViewModel = availableProductsViewModel
            });

            vm.AvailableProducts = availableProductsViewModel.Values.ToList();
        }

        IDictionary<int, dynamic> MapToDictionary(IEnumerable<int> availableProducts)
        {
            var availableProductsViewModel = new Dictionary<int, dynamic>();

            foreach (var id in availableProducts)
            {
                dynamic vm = new ExpandoObject();
                vm.Id = id;

                availableProductsViewModel[id] = vm;
            }

            return availableProductsViewModel;
        }
    }
}
