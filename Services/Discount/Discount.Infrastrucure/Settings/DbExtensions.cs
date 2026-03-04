using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Discount.Infrastrucure.Settings
{
    public static class DbExtensions
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("DbExtensions");
            var databaseSettings = services.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            try
            {
                logger.LogInformation("Discount db migration started");
                ApplyMigration(databaseSettings.ConnectionString);
                logger.LogInformation("Discount db migration completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database.");
                throw;
            }
            return host;
        }
        private static void ApplyMigration(string connectionString)
        {
            var retry = 5; while (retry > 0)
            {
                try
                {
                    using var connection = new Npgsql.NpgsqlConnection(connectionString);
                    connection.Open();
                    var command = new Npgsql.NpgsqlCommand
                    {
                        Connection = connection,
                        CommandText = @"DROP TABLE IF EXISTS Coupon"
                    };
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE IF NOT EXISTS Coupon(
                                            Id SERIAL PRIMARY KEY,
                                            ProductName VARCHAR(24) NOT NULL,
                                            Description TEXT,
                                            Amount INT
                                            )";
                    command.ExecuteNonQuery();

                    command.CommandText = @"INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150)";
                    command.ExecuteNonQuery();

                    command.CommandText = @"INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100)";
                    command.ExecuteNonQuery();

                    break;
                }
                catch (Npgsql.NpgsqlException)
                {
                    retry--;
                    if (retry == 0)
                        throw;
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
