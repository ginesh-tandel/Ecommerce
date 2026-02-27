using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllProductsHandler(IProductRepository productRepository) : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<Pagination<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetProducts(request.catalogSpecParams);
            return products.ToResponse();
        }
    }
}
