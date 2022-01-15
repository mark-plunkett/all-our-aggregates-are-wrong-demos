﻿using JsonUtils;
using Marketing.ViewModelComposition.Events;
using ServiceComposer.AspNetCore;
using System;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.ViewModelComposition
{
    class AvailableProductsLoadedSubscriber : ICompositionEventsSubscriber
    {
        private readonly MarketingApi api;

        public AvailableProductsLoadedSubscriber(MarketingApi api)
        {
            this.api = api;
        }

        [HttpGet("/")]
        public void Subscribe(ICompositionEventsPublisher publisher)
        {
            publisher.Subscribe<AvailableProductsLoaded>(async (@event, request) =>
            {
                var ids = String.Join(",", @event.AvailableProductsViewModel.Keys);

                var response = await this.api.GetAsync($"api/product-details/products/{ids}");

                dynamic[] productDetails = await response.Content.AsExpandoArray();

                foreach (dynamic detail in productDetails)
                {
                    @event.AvailableProductsViewModel[(int)detail.Id].ProductName = detail.Name;
                    @event.AvailableProductsViewModel[(int)detail.Id].ProductDescription = detail.Description;
                }
            });
        }
    }
}
