using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly IMongoCollection<ProductBrand> _brands;
        public BrandRepository(IOptions<DatabaseSettings> options)
        {
            var config = options.Value;
            var client = new MongoClient(config.ConnectionString);
            var db = client.GetDatabase(config.DatabaseName);
            _brands = db.GetCollection<ProductBrand>(config.BrandCollectionName);
        }
        public async Task<IEnumerable<ProductBrand>> GetAllBrandsAsync()
        {
            return await _brands.Find(_ => true).ToListAsync();
        }

        public async Task<ProductBrand> GetBrandByIdAsync(string id)
        {
            return await _brands.Find(m => m.Id == id).FirstOrDefaultAsync();
        }
    }
}
