using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Settings;      
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class TypeRepository : ITypeRepository
    {
        private readonly IMongoCollection<ProductType> _types;
        public TypeRepository(IOptions<DatabaseSettings> options)
        {
            var config = options.Value;
            var client = new MongoClient(config.ConnectionString);
            var db = client.GetDatabase(config.DatabaseName);
            _types = db.GetCollection<ProductType>(config.TypeCollectionName);
        }
        public async Task<IEnumerable<ProductType>> GetAllTypesAsync()
        {
            return await _types.Find(_ => true).ToListAsync();
        }

        public async Task<ProductType> GetByIdAsync(string id)
        {
            return await _types.Find(m => m.Id == id).FirstOrDefaultAsync();
        }
    }
}
