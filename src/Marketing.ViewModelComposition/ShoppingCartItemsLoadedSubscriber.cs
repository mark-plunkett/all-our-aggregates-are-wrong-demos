﻿using JsonUtils;
using Sales.ViewModelComposition.Events;
using ServiceComposer.AspNetCore;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.ViewModelComposition
{
    public class ShoppingCartItemsLoadedSubscriber : ICompositionEventsSubscriber
    {
        private readonly MarketingApi api;

        public ShoppingCartItemsLoadedSubscriber(MarketingApi api)
        {
            this.api = api;
        }

        [HttpGet("/ShoppingCart")]
        public void Subscribe(ICompositionEventsPublisher publisher)
        {
            publisher.Subscribe<ShoppingCartItemsLoaded>(async (@event, request) =>
            {
                var ids = String.Join(",", @event.CartItemsViewModel.Keys);

                var response = await this.api.GetAsync($"api/product-details/products/{ids}").ConfigureAwait(false);

                dynamic[] productDetails = await response.Content.AsExpandoArray().ConfigureAwait(false);

                foreach (dynamic detail in productDetails)
                {
                    @event.CartItemsViewModel[detail.Id].ProductName = detail.Name;
                    @event.CartItemsViewModel[detail.Id].ProductDescription = detail.Description;
                }
            });
        }
    }
}
