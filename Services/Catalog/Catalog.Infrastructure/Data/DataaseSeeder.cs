using Catalog.Core.Entities;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public class DataaseSeeder
    {
        public static async Task SeedData(IOptions<DatabaseSettings> options)
        {
            var config = options.Value;
            var client = new MongoClient(config.ConnectionString);
            var db = client.GetDatabase(config.DatabaseName);
            var brands = db.GetCollection<ProductBrand>(config.BrandCollectionName);
            var types = db.GetCollection<ProductType>(config.TypeCollectionName);
            var products = db.GetCollection<Product>(config.ProductCollectionName);
            var seedBasePath = Path.Combine("Data", "SeedData");

            List<ProductBrand> brandList = [];
            if (await brands.CountDocumentsAsync(_ => true) == 0)
            {
                var brandData = await File.ReadAllTextAsync(Path.Combine(seedBasePath, "brands.json"));
                brandList = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                await brands.InsertManyAsync(brandList);
            }
            else
                brandList = await brands.Find(_ => true).ToListAsync();

            List<ProductType> typeList = [];
            if (await types.CountDocumentsAsync(_ => true) == 0)
            {
                var typeData = await File.ReadAllTextAsync(Path.Combine(seedBasePath, "types.json"));
                typeList = JsonSerializer.Deserialize<List<ProductType>>(typeData);
                await types.InsertManyAsync(typeList);
            }
            else
                typeList = await types.Find(_ => true).ToListAsync();

            List<Product> productList = [];
            if (await products.CountDocumentsAsync(_ => true) == 0)
            {
                var productData = await File.ReadAllTextAsync(Path.Combine(seedBasePath, "products.json"));
                productList = JsonSerializer.Deserialize<List<Product>>(productData);
                foreach (var product in productList)
                {
                    product.Id = null;
                    if (product.CreatedDate == default)
                        product.CreatedDate = DateTime.UtcNow;
                }
                await products.InsertManyAsync(productList);
            }
            else
                productList = await products.Find(_ => true).ToListAsync();
        }
    }
}
