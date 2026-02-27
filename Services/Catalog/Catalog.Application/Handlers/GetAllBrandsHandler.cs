using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllBrandsHandler(IBrandRepository brandRepository) : IRequestHandler<GetAllBrandsQuery, List<BrandResponse>>
    {
        private readonly IBrandRepository _brandRepository = brandRepository;

        public async Task<List<BrandResponse>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _brandRepository.GetAllBrandsAsync();
            return brands.ToResponseList();
        }
    }
}
