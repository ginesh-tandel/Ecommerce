using Dapper;
using Discount.Core.Entities;
using Discount.Core.Entities.Repositories;
using Discount.Infrastrucure.Settings;
using Microsoft.Extensions.Options;

namespace Discount.Infrastrucure.Repositories
{
    public class DiscountRepository(IOptions<DatabaseSettings> options) : IDiscountRepository
    {
        private readonly string _connectionString = options.Value.ConnectionString;
        public async Task<bool> CreateDiscountAsync(Coupon coupon)
        {
            await using var connection = new Npgsql.NpgsqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)", new { coupon.ProductName, coupon.Description, coupon.Amount });
            return affected > 0;
        }

        public async Task<bool> DeleteDiscountAsync(string productName)
        {
            await using var connection = new Npgsql.NpgsqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });
            return affected > 0;
        }

        public async Task<Coupon> GetDiscountAsync(string productName)
        {
            await using var connection = new Npgsql.NpgsqlConnection(_connectionString);
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });
            return coupon ?? new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Available" }; ;
        }

        public async Task<bool> UpdateDiscountAsync(Coupon coupon)
        {
            await using var connection = new Npgsql.NpgsqlConnection(_connectionString);
                var affected = await connection.ExecuteAsync("UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id", new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });
            return affected > 0;
        }
    }
}
