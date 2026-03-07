using EventBus.Messages.Events;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.Extensions.Logging;
using Ordering.Application.Abstactions;
using Ordering.Application.Mappers;
using Ordering.Application.Orders.CreateOrder;

namespace Ordering.Application.EventBusConsumer
{
    public class BasketOrderingConsumer(ICommandHandler<CreateOrderCommand, int> commandHandler, ILogger<BasketOrderingConsumer> logger) : IConsumer<BasketCheckoutEvent>
    {
        private readonly ICommandHandler<CreateOrderCommand, int> _commandHandler = commandHandler;
        private readonly ILogger<BasketOrderingConsumer> _logger = logger;
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            using var scope = _logger.BeginScope("Consuming BasketCheckoutEvent for User: {UserName}", context.Message.UserName);
            var command = context.Message.ToCheckoutOrderCommand();
            var orderId = await _commandHandler.Handle(command);
            _logger.LogInformation("Received BasketCheckoutEvent for OrderId: {OrderId} User: {UserName}, TotalPrice: {TotalPrice}", context.Message.UserName, context.Message.TotalPrice, orderId);
        }
    }
}
