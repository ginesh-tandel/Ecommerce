using Discount.Grpc.Protos;

namespace Basket.Application.GrpcService
{
    public class DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService = discountProtoService;
        public async Task<CouponModel> GetDiscount(string productName)
        {
            var request = new GetDiscountRequest { ProductName = productName };
            return await _discountProtoService.GetDiscountAsync(request);
        }
    }
}
