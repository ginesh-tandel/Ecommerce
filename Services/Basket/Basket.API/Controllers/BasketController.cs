using Basket.Application.Commands;
using Basket.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetBasketByUserId(string userName)
        {
            var query = new GetBasketByUserNameQuery(userName);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateBasket([FromBody] CreateShoppingCartCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteBasketByUserId(string userName)
        {
            var command = new DeleteBasketByUserNameCommand(userName);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
