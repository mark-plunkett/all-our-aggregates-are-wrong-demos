using Microsoft.EntityFrameworkCore;
using NServiceBus;
using Shipping.Data;
using Shipping.Data.Models;
using Shipping.Messages.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace Shipping.Service.Handlers
{
    class AddItemToCartHandler : IHandleMessages<AddItemToCart>
    {
        private readonly ShippingContext db;

        public AddItemToCartHandler(ShippingContext db)
        {
            this.db = db;
        }           

        public async Task Handle(AddItemToCart message, IMessageHandlerContext context)
        {
            var requestAlreadyHandled = await db.ShoppingCartItems
                .Where(o => o.RequestId == message.RequestId)
                .SingleOrDefaultAsync() != null;

            if (!requestAlreadyHandled)
            {
                var shippingOptions = db.ProductShippingOptions
                    .Include(so => so.Options)
                    .Where(o => o.ProductId == message.ProductId)
                    .Single();

                var shortest = shippingOptions.Options.Min(o => o.EstimatedMinDeliveryDays);
                var longest = shippingOptions.Options.Max(o => o.EstimatedMaxDeliveryDays);
                var estimate = "";
                if (shortest == int.MaxValue && longest == int.MaxValue)
                {
                    estimate = "ah ah ah ah ah ah";
                }
                else
                {
                    estimate = $"between {shortest} and {longest} days";
                }

                db.ShoppingCartItems.Add(new ShoppingCartItem()
                {
                    CartId = message.CartId,
                    RequestId = message.RequestId,
                    ProductId = message.ProductId,
                    DeliveryEstimate = estimate,
                    Quantity = message.Quantity
                });

                await db.SaveChangesAsync();
            }
        }
    }
}
