using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetProductByNameHandler(IProductRepository productRepository) : IRequestHandler<GetProductByNameQuery, List<ProductResponse>>
    {
        private readonly IProductRepository _productRepository = productRepository;
        public async Task<List<ProductResponse>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductsByNameAsync(request.Name);
            return product.ToResponseList();
        }
    }
}
