using Discount.Application.Commands;
using Discount.Application.DTOs;
using Discount.Application.Extensions;
using Discount.Application.Mappers;
using Discount.Core.Entities.Repositories;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers
{
    public class UpdateDiscountHandler(IDiscountRepository discountRepository) : IRequestHandler<UpdateDiscountCommand, CouponDto>
    {
        private readonly IDiscountRepository _discountRepository = discountRepository;
        public async Task<CouponDto> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {
            var validationErrors = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(request.ProductName))
                validationErrors[nameof(request.ProductName)] = $"Product Name must not be empty.";
            if (string.IsNullOrEmpty(request.Description))
                validationErrors[nameof(request.Description)] = $"Description must not be empty.";
            if (request.Amount <= 0)
                validationErrors[nameof(request.Amount)] = $"Amount must be greater than zero.";
            if (validationErrors.Count != 0)
                throw GrpcErrorHelper.CreateValidationException(validationErrors);
            var coupon = request.ToEntity();
            var updatedCoupon = await _discountRepository.UpdateDiscountAsync(coupon);
            if (!updatedCoupon)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Could not update discount for the Product Name: {request.ProductName}."));
            }
            return coupon.ToDto();
        }
    }
}
