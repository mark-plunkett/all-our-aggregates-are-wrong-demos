using JsonUtils;
using Marketing.ViewModelComposition.Events;
using ServiceComposer.AspNetCore;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sales.ViewModelComposition
{
    class AvailableProductsLoadedSubscriber : ICompositionEventsSubscriber
    {
        private readonly SalesApi api;

        public AvailableProductsLoadedSubscriber(SalesApi api)
        {
            this.api = api;
        }

        [HttpGet("/")]
        public void Subscribe(ICompositionEventsPublisher publisher)
        {
            publisher.Subscribe<AvailableProductsLoaded>(async (@event, request) =>
            {
                var ids = String.Join(",", @event.AvailableProductsViewModel.Keys);

                var response = await this.api.GetAsync($"/api/prices/products/{ids}");

                dynamic[] productPrices = await response.Content.AsExpandoArray();

                foreach (dynamic productPrice in productPrices)
                {
                    @event.AvailableProductsViewModel[(int)productPrice.Id].ProductPrice = productPrice.Price;
                }
            });
        }
    }
}
