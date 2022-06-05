using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountService: DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repoistory;
        private readonly IMapper _mapper;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(IDiscountRepository repoistory, ILogger<DiscountService> logger, IMapper mapper)
        {
            _repoistory = repoistory;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repoistory.GetDiscount(request.ProductName);
            if(coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount With Product Name {request.ProductName} Not Found"));
            }
            _logger.LogInformation("Discount is retrived for Product Name: {ProductName}, Amount {Amount}", coupon.ProductName, coupon.Amount);

            var couponeModel =  _mapper.Map<CouponModel>(coupon);
            return couponeModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            var newCoupon = await _repoistory.CreateDiscount(coupon);

            _logger.LogInformation("Discount is Successfully Created");
            var couponeModel = _mapper.Map<CouponModel>(newCoupon);

            return couponeModel;
        }
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            var updatedCoupon = await _repoistory.UpdateDiscount(coupon);

            _logger.LogInformation("Discount is Successfully Updaated");
            var couponeModel = _mapper.Map<CouponModel>(updatedCoupon);

            return couponeModel;

        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _repoistory.DeleteDiscount(request.ProductName);
            var response = new DeleteDiscountResponse {
                Success = deleted
            };

            return response;
        }
    }
}
