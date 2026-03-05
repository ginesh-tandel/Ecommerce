using Ordering.Application.Abstactions;
using Ordering.Application.Mappers;
using Ordering.Core.Repositories;

namespace Ordering.Application.Orders.CreateOrder
{
    public class CreateOrderHandler(IOrderRepository orderRepository) : ICommandHandler<CreateOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<int> Handle(CreateOrderCommand command)
        {
            var order = command.ToEntity();
            var createdOrder = await _orderRepository.AddAsync(order);
            return createdOrder.Id;
        }
    }
}
