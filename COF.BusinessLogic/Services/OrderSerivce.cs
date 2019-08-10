using COF.BusinessLogic.Models.Order;
using COF.BusinessLogic.Settings;
using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Models;
using COF.DataAccess.EF.Repositories;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services
{
    public interface IOrderService
    {
        Task<BusinessLogicResult<Order>> CreateOrderAsync(int shopId, OrderCreateModel model);
    }

    public class OrderService : IOrderService
    {
        #region fields
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShopRepository _shopRepository;
       private readonly IWorkContext _workContext;
        private readonly IProductSizeRepository _productSizeRepository;
        #endregion

        #region ctor
        public OrderService(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            IShopRepository shopRepository,
            IWorkContext workContext,
            IProductSizeRepository productSizeRepository
           )
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _shopRepository = shopRepository;
            _workContext = workContext;
            _productSizeRepository = productSizeRepository;
        }

        #endregion

        #region public methods
        public async Task<BusinessLogicResult<Order>> CreateOrderAsync(int shopId, OrderCreateModel model)
        {
            try
            {
                 var shop = await _shopRepository.GetByIdAsync(shopId);
                 if (shop is null)
                 {
                    return new BusinessLogicResult<Order>
                    {
                        Success = false, 
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Shop", "Shop Id không tồn tại.") })
                    };
                 }
                var order = new Order
                {
                    CustomerId = model.CustomerId,
                    PartnerId = shop.PartnerId,
                    UserId = _workContext.CurrentUserId,
                    ShopId = shop.Id
                };


                foreach (var item in model.OrderDetails)
                {
                    var productSize = await _productSizeRepository.GetByIdAsync(item.ProductSizeId);
                    order.OrderDetails.Add(new OrderDetail
                    {
                        PartnerId = shop.PartnerId,
                        ProductSizeId = productSize.Id,
                        Quantity = item.Quantity,
                        UnitPrice = productSize.Cost
                    });
                }
                _orderRepository.Add(order, _workContext.CurrentUser.FullName);
                //_orderRepository.Add(order, "");
                await _unitOfWork.SaveChangesAsync();
                return new BusinessLogicResult<Order>
                {
                    Result = order,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<Order>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra", ex.Message) })
                };
            }
        }
        #endregion

    }
}
