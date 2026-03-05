using Ordering.Application.DTOs;
using Ordering.Application.Orders.CreateOrder;
using Ordering.Application.Orders.UpdateOrder;
using Ordering.Core.Entitties;

namespace Ordering.Application.Mappers
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(this Order order)
        {
            return new OrderDto(order.Id, order.UserName, order.TotalPrice, order.FirstName, order.LastName, order.Email, order.AddressLine, order.Country, order.State, order.ZipCode, order.CardName, order.CardNumber, order.Expiration, order.Cvv, order.PaymentMethod);
        }
        public static Order ToEntity(this CreateOrderCommand orderCommand)
        {
            return new Order
            {
                UserName = orderCommand.UserName,
                TotalPrice = orderCommand.TotalPrice,
                FirstName = orderCommand.FirstName,
                LastName = orderCommand.LastName,
                Email = orderCommand.Email,
                AddressLine = orderCommand.AddressLine,
                Country = orderCommand.Country,
                State = orderCommand.State,
                ZipCode = orderCommand.ZipCode,
                CardName = orderCommand.CardName,
                CardNumber = orderCommand.CardNumber,
                Expiration = orderCommand.Expiration,
                Cvv = orderCommand.Cvv,
                PaymentMethod = orderCommand.PaymentMethod
            };
        }
        public static void MapUpdate(this Order order, UpdateOrderCommand orderCommand)
        {
            order.UserName = orderCommand.UserName;
            order.TotalPrice = orderCommand.TotalPrice;
            order.FirstName = orderCommand.FirstName;
            order.LastName = orderCommand.LastName;
            order.Email = orderCommand.Email;
            order.AddressLine = orderCommand.AddressLine;
            order.Country = orderCommand.Country;
            order.State = orderCommand.State;
            order.ZipCode = orderCommand.ZipCode;
            order.CardName = orderCommand.CardName;
            order.CardNumber = orderCommand.CardNumber;
            order.Expiration = orderCommand.Expiration;
            order.Cvv = orderCommand.Cvv;
            order.PaymentMethod = orderCommand.PaymentMethod;
        }
        public static CreateOrderCommand ToCommand(this CreateOrderDto orderDto)
        {
            return new CreateOrderCommand
            {
                UserName = orderDto.UserName,
                TotalPrice = orderDto.TotalPrice,
                FirstName = orderDto.FirstName,
                LastName = orderDto.LastName,
                Email = orderDto.Email,
                AddressLine = orderDto.AddressLine,
                Country = orderDto.Country,
                State = orderDto.State,
                ZipCode = orderDto.ZipCode,
                CardName = orderDto.CardName,
                CardNumber = orderDto.CardNumber,
                Expiration = orderDto.Expiration,
                Cvv = orderDto.Cvv,
                PaymentMethod = orderDto.PaymentMethod
            };
        }

        public static UpdateOrderCommand ToCommand(this OrderDto orderDto)
        {
            return new UpdateOrderCommand
            {
                UserName = orderDto.UserName,
                TotalPrice = orderDto.TotalPrice,
                FirstName = orderDto.FirstName,
                LastName = orderDto.LastName,
                Email = orderDto.Email,
                AddressLine = orderDto.AddressLine,
                Country = orderDto.Country,
                State = orderDto.State,
                ZipCode = orderDto.ZipCode,
                CardName = orderDto.CardName,
                CardNumber = orderDto.CardNumber,
                Expiration = orderDto.Expiration,
                Cvv = orderDto.Cvv,
                PaymentMethod = orderDto.PaymentMethod
            };
        }
    }
}
