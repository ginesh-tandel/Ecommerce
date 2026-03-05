using Microsoft.Extensions.Logging;
using Ordering.Application.Abstactions;
using Ordering.Application.Exceptions;
using Ordering.Application.Mappers;
using Ordering.Core.Entitties;
using Ordering.Core.Repositories;

namespace Ordering.Application.Orders.UpdateOrder
{
    public class UpdateOrderHandler(IOrderRepository orderRepository, ILogger<UpdateOrderCommand> logger) : ICommandHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ILogger<UpdateOrderCommand> _logger = logger;
        public async Task Handle(UpdateOrderCommand command)
        {
            var orderToUpdate = await _orderRepository.GetByIdAsync(command.Id) ?? throw new OrderNotFoundException(nameof(Order), command.Id);
            orderToUpdate.MapUpdate(command);
            await _orderRepository.UpdateAsync(orderToUpdate);
            _logger.LogInformation("Order with id {Id} is successfully updated.", command.Id);
        }
    }
}
