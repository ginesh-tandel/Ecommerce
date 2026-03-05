using Ordering.Application.Abstactions;

namespace Ordering.Application.Orders.DeleteOrder
{
    public record DeleteOrderCommand(int Id) : ICommand;
}
