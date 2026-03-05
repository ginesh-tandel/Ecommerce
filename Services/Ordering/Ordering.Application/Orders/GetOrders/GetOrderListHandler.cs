using Ordering.Application.Abstactions;
using Ordering.Application.DTOs;
using Ordering.Application.Mappers;
using Ordering.Core.Repositories;

namespace Ordering.Application.Orders.GetOrders
{
    public class GetOrderListHandler(IOrderRepository orderRepository) : IQueryHandler<GetOrderListQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<List<OrderDto>> Handle(GetOrderListQuery query, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetOrdersByUserName(query.UserName);
            return [.. orders.Select(o => o.ToDto())];
        }
    }
}
