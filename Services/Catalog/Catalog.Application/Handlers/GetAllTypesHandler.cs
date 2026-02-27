using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllTypesHandler(ITypeRepository typeRepository) : IRequestHandler<GetAllTypesQuery, IList<TypeResponse>>
    {
        private readonly ITypeRepository _typeRepository = typeRepository;

        public async Task<IList<TypeResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var types = await _typeRepository.GetAllTypesAsync();
            return types.ToResponseList();
        }
    }
}
