using Basket.Application.Commands;
using Basket.Application.DTOs;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;

namespace Basket.Application.Mappers
{
    public static class BasketMapper
    {
        public static ShoppingCartResponse ToResponse(this ShoppingCart shoppingCart)
        {
            if (shoppingCart == null) return new ShoppingCartResponse();
            return new ShoppingCartResponse
            {
                UserName = shoppingCart.UserName,
                Items = [.. shoppingCart.Items.Select(item => new ShoppingCartItemResponse
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ImageFile = item.ImageFile
                })]
            };
        }
        public static readonly Func<ShoppingCart, ShoppingCartResponse> ToResponseFunc = shoppingCart => new ShoppingCartResponse
        {
            UserName = shoppingCart.UserName,
            Items = [.. shoppingCart.Items.Select(item => new ShoppingCartItemResponse
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ImageFile = item.ImageFile
                })]
        };
        public static ShoppingCartResponse ToResponseUsingDelegate(this ShoppingCart shoppingCart)
        {
            if (shoppingCart == null) return new ShoppingCartResponse();
            return ToResponseFunc(shoppingCart);
        }
        public static ShoppingCart ToEntity(this CreateShoppingCartCommand request)
        {
            return new ShoppingCart
            {
                UserName = request.UserName,
                Items = [.. request.Items.Select(item => new ShoppingCartItem
               {
                   ProductId = item.ProductId,
                   ProductName = item.ProductName,
                   Price = item.Price,
                   Quantity = item.Quantity,
                   ImageFile = item.ImageFile
               })]
            };
        }
        public static ShoppingCart ToEntity(this ShoppingCartResponse shoppingCartResponse)
        {
            return new ShoppingCart(shoppingCartResponse.UserName)
            {
                Items = [.. shoppingCartResponse.Items.Select(s => new ShoppingCartItem
                {
                    ProductId = s.ProductId,
                    ProductName = s.ProductName,
                    Price = s.Price,
                    Quantity = s.Quantity,
                    ImageFile = s.ImageFile
                })],
            };
        }
        public static BasketCheckoutEvent ToBasketCheckoutEvent(this BasketCheckoutDto dto, ShoppingCart basket)
        {
            return new BasketCheckoutEvent
            {
                UserName = dto.UserName,
                TotalPrice = basket.Items.Sum(item => item.Price * item.Quantity),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                AddressLine = dto.AddressLine,
                Country = dto.Country,
                State = dto.State,
                ZipCode = dto.ZipCode,
                CardName = dto.CardName,
                CardNumber = dto.CardNumber,
                Expiration = dto.Expiration,
                Cvv = dto.Cvv,
                PaymentMethod = dto.PaymentMethod
            };
        }
    }
}
