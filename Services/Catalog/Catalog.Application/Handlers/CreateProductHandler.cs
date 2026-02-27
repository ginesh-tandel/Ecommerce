using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class CreateProductHandler(IProductRepository productRepository) : IRequestHandler<CreateProductCommand, ProductResponse>
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var brand = await _productRepository.GetBrandByIdAsync(request.BrandId);
            var type = await _productRepository.GetTypeByIdAsync(request.TypeId);
            if (brand == null || type == null)
            {
                throw new ApplicationException("Brand or Type not found");
            }
            var product = request.ToEntity(brand, type);
            var createdProduct = await _productRepository.CreateProductAsync(product);
            return createdProduct.ToResponse();
        }
    }
}
