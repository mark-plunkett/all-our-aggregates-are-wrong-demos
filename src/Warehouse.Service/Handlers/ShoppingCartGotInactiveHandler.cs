using Microsoft.EntityFrameworkCore;
using NServiceBus;
using Sales.Messages.Events;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Data;
using Console = Colorful.Console;

namespace Warehouse.Service.Handlers
{
    class ShoppingCartGotInactiveHandler : IHandleMessages<ShoppingCartGotInactive>
    {
        private readonly WarehouseContext db;

        public ShoppingCartGotInactiveHandler(WarehouseContext db)
        {
            this.db = db;
        }

        public async Task Handle(ShoppingCartGotInactive message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Ready to wipe cart {message.CartId}.", Color.Yellow);

            var cartItems = await db.ShoppingCartItems
                .Where(o => o.CartId == message.CartId)
                .ToListAsync();

            db.ShoppingCartItems.RemoveRange(cartItems);
            await db.SaveChangesAsync();

            Console.WriteLine($"Cart {message.CartId} wiped.", Color.Green);
        }
    }
}