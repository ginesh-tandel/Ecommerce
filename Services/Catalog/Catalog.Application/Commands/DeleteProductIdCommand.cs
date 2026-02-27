using MediatR;

namespace Catalog.Application.Commands
{
    public record DeleteProductIdCommand(string Id) : IRequest<bool>;
}
