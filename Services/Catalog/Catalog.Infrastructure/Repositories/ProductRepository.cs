using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<ProductBrand> _brands;
        private readonly IMongoCollection<ProductType> _types;
        private readonly IMongoCollection<Product> _products;
        public ProductRepository(IOptions<DatabaseSettings> options)
        {
            var config = options.Value;
            var client = new MongoClient(config.ConnectionString);
            var db = client.GetDatabase(config.DatabaseName);
            _brands = db.GetCollection<ProductBrand>(config.BrandCollectionName);
            _types = db.GetCollection<ProductType>(config.TypeCollectionName);
            _products = db.GetCollection<Product>(config.ProductCollectionName);
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            await _products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
            var deletedProduct = await _products.DeleteOneAsync(p => p.Id == productId);
            return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
        }

        public async Task<ProductBrand> GetBrandByIdAsync(string brandId)
        {
            return await _brands.Find(b => b.Id == brandId).FirstOrDefaultAsync();
        }

        public Task<Product> GetProductByIdAsync(string id)
        {
            return _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Pagination<Product>> GetProducts(CatalogSpecParams catalogSpecParams)
        {
            var filter = Builders<Product>.Filter.Empty;
            if (!string.IsNullOrEmpty(catalogSpecParams.BrandId))
            {
                filter &= Builders<Product>.Filter.Eq(p => p.Brand.Id, catalogSpecParams.BrandId);
            }
            if (!string.IsNullOrEmpty(catalogSpecParams.TypeId))
            {
                filter &= Builders<Product>.Filter.Eq(p => p.Type.Id, catalogSpecParams.TypeId);
            }
            if (!string.IsNullOrEmpty(catalogSpecParams.Search))
            {
                filter &= Builders<Product>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression($".*{catalogSpecParams.Search}.*", "i"));
            }
            var totalItems = await _products.CountDocumentsAsync(filter);
            var data = await ApplyDataFilters(catalogSpecParams, filter);
            return new Pagination<Product>(
                catalogSpecParams.PageIndex,
                catalogSpecParams.PageSize,
                (int)totalItems, data
                );
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _products.Find(p => true).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByBrandAsync(string name)
        {
            return await _products.Find(p => p.Brand.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            var filter = Builders<Product>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression($".*{name}.*", "i"));
            return await _products.Find(filter).ToListAsync();
        }

        public async Task<ProductType> GetTypeByIdAsync(string typeId)
        {
            return await _types.Find(t => t.Id == typeId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var updatedProduct = await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
            return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
        }
        private async Task<IReadOnlyCollection<Product>> ApplyDataFilters(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
        {
            
            var sort = Builders<Product>.Sort.Ascending(p => p.Name);
            if (!string.IsNullOrEmpty(catalogSpecParams.Sort))
            {
                sort = catalogSpecParams.Sort switch
                {
                    "priceAsc" => Builders<Product>.Sort.Ascending(p => p.Price),
                    "priceDesc" => Builders<Product>.Sort.Descending(p => p.Price),
                    _ => sort
                };
            }

            return await _products.Find(filter)
                .Sort(sort)
                .Skip((catalogSpecParams.PageIndex - 1) * catalogSpecParams.PageSize)
                .Limit(catalogSpecParams.PageSize)
                .ToListAsync();
        }
    }
}
