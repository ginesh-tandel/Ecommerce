using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Abstactions;
using Ordering.Application.Behaviours;
using Ordering.Application.Validators;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;

namespace Ordering.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOrderingServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("OrderingConnectionString"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                        sqlOptions.MigrationsAssembly("Ordering.Infrastructure");
                    });

            });
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.Scan(scan => scan.FromAssemblies(typeof(ICommandHandler<>).Assembly)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>))).AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>))).AsImplementedInterfaces().WithScopedLifetime()
             .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>))).AsImplementedInterfaces().WithScopedLifetime()
            );

            services.AddValidatorsFromAssembly(typeof(CreateOrderCommandValidator).Assembly);

            services.Decorate(typeof(ICommandHandler<>), typeof(ValidationHandler<,>));

            services.Decorate(typeof(ICommandHandler<>), typeof(ExceptionHandler<,>));

            return services;
        }
    }
}
