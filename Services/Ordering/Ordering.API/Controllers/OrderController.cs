using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Abstactions;
using Ordering.Application.DTOs;
using Ordering.Application.Mappers;
using Ordering.Application.Orders.CreateOrder;
using Ordering.Application.Orders.DeleteOrder;
using Ordering.Application.Orders.GetOrders;
using Ordering.Application.Orders.UpdateOrder;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController(
      ICommandHandler<CreateOrderCommand, int> createOrderHandler,
      ICommandHandler<UpdateOrderCommand> updateOrderHandler,
      ICommandHandler<DeleteOrderCommand> deleteOrderHandler,
      IQueryHandler<GetOrderListQuery, List<OrderDto>> getOrderListHandler,
      ILogger<OrderController> logger) : ControllerBase
    {
        private readonly ICommandHandler<CreateOrderCommand, int> _createOrderHandler = createOrderHandler;
        private readonly ICommandHandler<UpdateOrderCommand> _updateOrderHandler = updateOrderHandler;
        private readonly ICommandHandler<DeleteOrderCommand> _deleteOrderHandler = deleteOrderHandler;
        private readonly IQueryHandler<GetOrderListQuery, List<OrderDto>> _getOrderListHandler = getOrderListHandler;
        private readonly ILogger<OrderController> _logger = logger;

        [HttpGet("{userName}", Name = "GetOrdersByUserName")]
        public async Task<ActionResult<List<OrderDto>>> GetOrdersByUserName(string userName, CancellationToken cancellationToken)
        {
            var query = new GetOrderListQuery(userName);

            var orders = await _getOrderListHandler.Handle(query, cancellationToken);

            _logger.LogInformation("Orders fetched for user {@UserName}", userName);
            return Ok(orders);
        }

        // Testing purpose
        [HttpPost(Name = "CheckoutOrder")]
        public async Task<IActionResult> CheckoutOrder([FromBody] CreateOrderDto dto)
        {
            var command = dto.ToCommand();

            var orderId = await _createOrderHandler.Handle(command);

            _logger.LogInformation("Order created with Id {OrderId}", orderId);
            return Ok(orderId);
        }

        [HttpPut(Name = "UpdateOrder")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderDto dto)
        {
            var command = dto.ToCommand();

            await _updateOrderHandler.Handle(command);

            _logger.LogInformation("Order updated with Id {OrderId}", dto.Id);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(
            int id)
        {
            var command = new DeleteOrderCommand(id);

            await _deleteOrderHandler.Handle(command);

            _logger.LogInformation("Order deleted with Id {OrderId}", id);
            return NoContent();
        }
    }
}