using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class UpdateCommandHandler(IProductRepository productRepository) : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository = productRepository;
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Product with id {request.Id} not found.");
            var brand = await _productRepository.GetBrandByIdAsync(request.BrandId);
            var type = await _productRepository.GetTypeByIdAsync(request.TypeId);
            if (brand == null || type == null)
            {
                throw new ApplicationException("Brand or Type not found");
            }
            var updateProduct = request.ToUpdateEntity(product, brand, type);
            return await _productRepository.UpdateProductAsync(updateProduct);
        }
    }
}
