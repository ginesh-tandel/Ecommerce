using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetProductByBrandHandler(IProductRepository productRepository) : IRequestHandler<GetProductByBrandQuery, IList<ProductResponse>>
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<IList<ProductResponse>> Handle(GetProductByBrandQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetProductsByBrandAsync(request.brandName);
            return products.ToResponseList();
        }
    }
}
