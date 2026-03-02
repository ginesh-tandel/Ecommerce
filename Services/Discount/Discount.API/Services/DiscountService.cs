using Discount.Application.Queries;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;
using Discount.Application.Mappers;
using Discount.Application.Commands;

namespace Discount.API.Services
{
    public class DiscountService(IMediator mediator) : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IMediator _mediator = mediator;
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var query = new GetDiscountQuery(request.ProductName);
            var coupon = await _mediator.Send(query);
            return coupon.ToModel();
        }
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var command = request.Coupon.ToCreateDiscountCommand();
            var coupon = await _mediator.Send(command);
            return coupon.ToModel();
        }
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var command = request.Coupon.ToUpdateDiscountCommand();
            var coupon = await _mediator.Send(command);
            return coupon.ToModel();
        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var command = new DeleteDiscountCommand(request.ProductName);
            var deleted = await _mediator.Send(command);
            return new DeleteDiscountResponse
            {
                Success = deleted,
            };
        }
    }
}
