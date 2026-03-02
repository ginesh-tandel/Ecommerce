using Discount.Application.DTOs;
using Discount.Application.Extensions;
using Discount.Application.Mappers;
using Discount.Application.Queries;
using Discount.Core.Entities.Repositories;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers
{
    public class GetDiscountHandler(IDiscountRepository discountRepository) : IRequestHandler<GetDiscountQuery, CouponDto>
    {
        private readonly IDiscountRepository _discountRepository = discountRepository;
        public async Task<CouponDto> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ProductName))
            {
                var validationErrors = new Dictionary<string, string>
                {
                    { nameof(request.ProductName), $"Product name must not be empty." }
                };
                throw  GrpcErrorHelper.CreateValidationException(validationErrors);
            }
            var coupon = await _discountRepository.GetDiscountAsync(request.ProductName) ?? throw new RpcException(new Status(StatusCode.Internal, $"No discount found for the Product Name: {request.ProductName}."));
            return coupon.ToDto();
        }
    }
}
