using Catalog.Core.Entities;
using Catalog.Core.Specifications;

namespace Catalog.Core.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Pagination<Product>> GetProducts(CatalogSpecParams specParams);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
        Task<IEnumerable<Product>> GetProductsByBrandAsync(string name);
        Task<Product> GetProductByIdAsync(string id);
        Task<Product> CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(string productId);
        Task<ProductBrand> GetBrandByIdAsync(string brandId);
        Task<ProductType> GetTypeByIdAsync(string typeId);
    }
}
