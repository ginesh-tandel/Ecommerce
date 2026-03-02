using Discount.API.Services;
using Discount.Application.Handlers;
using Discount.Core.Entities.Repositories;
using Discount.Infrastrucure.Repositories;
using Discount.Infrastrucure.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddOpenApi();

// Register MediatR and scan for handlers in the specified assemblies
var assemblies = new Assembly[] {
    Assembly.GetExecutingAssembly(),
    typeof(CreateDiscountHandler).Assembly
};
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(assemblies));

// Register repositories
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

// 
builder.Services.AddGrpc();

// Database settings
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

var app = builder.Build();

app.MigrateDatabase();
app.UseRouting();
app.UseEndpoints(endpoint =>
{
    endpoint.MapGrpcService<DiscountService>();
});

app.Run();
