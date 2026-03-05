using Ordering.Application.Abstactions;
using Ordering.Application.DTOs;

namespace Ordering.Application.Orders.GetOrders
{
    public record GetOrderListQuery(string? UserName) : IQuery<List<OrderDto>>;
}
