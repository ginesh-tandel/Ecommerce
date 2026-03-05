using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entitties;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository(OrderContext orderContext) : RepositoryBase<Order>(orderContext), IOrderRepository
    {
        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            var orders = await _orderContext.Orders.AsNoTracking().Where(o => o.UserName == userName).ToListAsync();
            return orders;
        }
    }
}
