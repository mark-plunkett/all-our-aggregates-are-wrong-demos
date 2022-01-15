﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.Data;
using System;
using System.Linq;

namespace Sales.Api.Controllers
{
    [Route("api/shopping-cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly SalesContext db;

        public ShoppingCartController(SalesContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("{id}")]
        public dynamic GetCart(Guid id)
        {
            var cartItems = db.ShoppingCarts
                .Include(c => c.Items)
                .Where(o => o.Id == id)
                .SelectMany(cart => cart.Items)
                .ToArray()
                .GroupBy(cartItem => cartItem.ProductId)
                .Select(group => new
                {
                    ProductId = group.Key,
                    Quantity = group.Sum(cartItem => cartItem.Quantity),
                    CurrentPrice = group.FirstOrDefault()?.CurrentPrice,
                    LastPrice = group.FirstOrDefault()?.LastPrice,
                })
                .ToArray();

            return new
            {
                CartId = id,
                Items = cartItems
            };
        }
    }
}