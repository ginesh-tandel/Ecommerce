using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries
{
    public record GetProductByBrandQuery(string brandName) : IRequest<IList<ProductResponse>>;
}
