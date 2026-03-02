using Discount.Application.Commands;
using Discount.Core.Entities.Repositories;
using MediatR;

namespace Discount.Application.Handlers
{
    public class DeleteDiscountHandler(IDiscountRepository discountRepository) : IRequestHandler<DeleteDiscountCommand, bool>
    {
        private readonly IDiscountRepository _discountRepository = discountRepository;
        public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var validationErrors = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(request.ProductName))
                validationErrors[nameof(request.ProductName)] = $"Product Name must not be empty.";
            return await _discountRepository.DeleteDiscountAsync(request.ProductName);
        }
    }
}
