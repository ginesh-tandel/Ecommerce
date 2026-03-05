using Microsoft.Extensions.Logging;
using Ordering.Core.Entitties;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seeded {context} with sample data.", nameof(OrderContext));
            }
        }

        private static IEnumerable<Order> GetOrders()
        {
            return
            [
                new() {
                    UserName = "ginesh",
                    FirstName = "Ginesh",
                    LastName = "Sahay",
                    Email = "ginesh29@gmail.com",
                    AddressLine = "Bilimora",
                    State = "GJ",
                    Country = "India",
                    ZipCode = "396321",
                    CardName = "Visa",
                    CardNumber = "4111111111111111",
                    CreatedBy = "Ginesh",
                    Expiration = "12/27",
                    Cvv = "123",
                    PaymentMethod = 1,
                    LastModifiedBy = "Ginesh",
                    LastModifiedDate = DateTime.UtcNow
                }
            ];
        }
    }
}
