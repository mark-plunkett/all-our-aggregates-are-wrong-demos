using JsonUtils;
using Sales.ViewModelComposition.Events;
using ServiceComposer.AspNetCore;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse.ViewModelComposition
{
    public class ShoppingCartItemsLoadedSubscriber : ICompositionEventsSubscriber
    {
        private readonly WarehouseApi api;

        public ShoppingCartItemsLoadedSubscriber(WarehouseApi api)
        {
            this.api = api;
        }

        [HttpGet("/ShoppingCart")]
        public void Subscribe(ICompositionEventsPublisher publisher)
        {
            publisher.Subscribe<ShoppingCartItemsLoaded>(async (@event, request) =>
            {
                var ids = String.Join(",", @event.CartItemsViewModel.Keys);

                var response = await this.api.GetAsync($"/api/shopping-cart/products/{ids}");

                dynamic[] inventoryDetails = await response.Content.AsExpandoArray();
                if (inventoryDetails == null || inventoryDetails.Length == 0)
                {
                    //eventual consitency is making fun of us
                    foreach (var item in @event.CartItemsViewModel.Values)
                    {
                        item.Inventory = "evaluation in progress";
                    }
                }
                else
                {
                    foreach (dynamic detail in inventoryDetails)
                    {
                        @event.CartItemsViewModel[detail.ProductId].Inventory = $"{detail.Inventory} item(s) left in stock";
                    }
                }
            });
        }
    }
}
