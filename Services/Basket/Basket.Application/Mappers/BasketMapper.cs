using Basket.Application.Commands;
using Basket.Application.Responses;
using Basket.Core.Entities;

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
    }
}
