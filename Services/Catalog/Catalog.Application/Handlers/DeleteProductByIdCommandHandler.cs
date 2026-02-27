using Catalog.Application.Commands;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class DeleteProductByIdCommandHandler(IProductRepository productRepository) : IRequestHandler<DeleteProductIdCommand, bool>
    {
        private readonly IProductRepository _productRepository = productRepository;
        public async Task<bool> Handle(DeleteProductIdCommand request, CancellationToken cancellationToken)
        {
            return await _productRepository.DeleteProductAsync(request.Id);
        }
    }
}
