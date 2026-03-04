using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Application.Settings;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Basket.Infrastructure.Settings;
using Discount.Grpc.Protos;
using Microsoft.Extensions.Options;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();

// Register MediatR and scan for handlers in the specified assemblies
var assemblies = new Assembly[] {
    Assembly.GetExecutingAssembly(),
    typeof(CreateShoppingCartHandler).Assembly
};
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(assemblies));

// Register repositories
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

// Configure Cache settings
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));

// Configure gRPC settings
builder.Services.Configure<GrpcSettings>(builder.Configuration.GetSection("GrpcSettings"));

// Register gRPC client for Discount service
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>((sp, config) =>
{
    var grpcSettings = sp.GetRequiredService<IOptions<GrpcSettings>>().Value;
    config.Address = new Uri(grpcSettings.DiscountUrl);
});

// Register the DiscountGrpcService to be used in handlers
builder.Services.AddScoped<DiscountGrpcService>();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>((sp, config) =>
{
    var grpcSettings = sp.GetRequiredService<IOptions<GrpcSettings>>().Value;
    config.Address = new Uri(grpcSettings.DiscountUrl);
});
// Configure Redis cache
builder.Services.AddStackExchangeRedisCache((options) =>
{
    options.Configuration = builder.Configuration.GetSection("CacheSettings").GetValue<string>("ConnectionString");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
