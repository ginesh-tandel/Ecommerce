using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Basket.Application.Handlers
{
    public class BasketCheckoutHandler(IMediator mediator, IPublishEndpoint publishEndpoint, ILogger<BasketCheckoutHandler> logger) : IRequestHandler<BasketCheckoutCommand, Unit>
    {
        readonly IMediator _mediator = mediator;
        readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        readonly ILogger<BasketCheckoutHandler> _logger = logger;
        public async Task<Unit> Handle(BasketCheckoutCommand request, CancellationToken cancellationToken)
        {
            var basketDto = request.BasketCheckoutDto;
            var basketResponse = await _mediator.Send(new GetBasketByUserNameQuery(basketDto.UserName), cancellationToken);
            if (basketResponse == null || basketResponse.Items.Count == 0)
            {
                throw new InvalidOperationException($"Basket not found for user {basketDto.UserName}");
            }
            var basket = basketResponse.ToEntity();
            var evnt = basketDto.ToBasketCheckoutEvent(basket);
            _logger.LogInformation("Publishing BasketCheckoutEvent for user {UserName}", basket.UserName);
            await _publishEndpoint.Publish(evnt, cancellationToken);

            await _mediator.Send(new DeleteBasketByUserNameCommand(basketDto.UserName), cancellationToken);
            return Unit.Value;
        }
    }
}
