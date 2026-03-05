using Microsoft.Extensions.Logging;
using Ordering.Application.Abstactions;
using Ordering.Application.Exceptions;
using Ordering.Core.Entitties;
using Ordering.Core.Repositories;

namespace Ordering.Application.Orders.DeleteOrder
{
    public class DeleteOrderHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommand> logger) : ICommandHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ILogger<DeleteOrderCommand> _logger = logger;
        public async Task Handle(DeleteOrderCommand command)
        {
            var orderToDelete = await _orderRepository.GetByIdAsync(command.Id) ?? throw new OrderNotFoundException(nameof(Order), command.Id);
            await _orderRepository.DeleteAsync(orderToDelete);
            _logger.LogInformation("Order with id {Id} is successfully deleted.", command.Id);
        }
    }
}
