using COF.BusinessLogic.Models.Order;
using COF.BusinessLogic.Models.Report;
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
        Task<BusinessLogicResult<List<OrderModel>>> GetAllOrderWithPaging(int shopId, int pageIndex, int pageSize, string filter);
        BusinessLogicResult<PartnerDailyOrderReport> GetDailyOrders(int partnerId);
    }

    public class OrderService : IOrderService
    {
        #region fields
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShopRepository _shopRepository;
        private readonly IWorkContext _workContext;
        private readonly IProductSizeRepository _productSizeRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IPartnerService _partnerService;
        #endregion

        #region ctor
        public OrderService(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            IShopRepository shopRepository,
            IWorkContext workContext,
            IProductSizeRepository productSizeRepository,
            ICustomerRepository customerRepository,
            IPartnerService partnerService
           )
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _shopRepository = shopRepository;
            _workContext = workContext;
            _productSizeRepository = productSizeRepository;
            _customerRepository = customerRepository;
            _partnerService = partnerService;
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

                var customer = await _customerRepository.GetByIdAsync(model.CustomerId);

                if (customer is null)
                {
                    return new BusinessLogicResult<Order>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Khách hàng", "Khách hàng không tồn tại.") })
                    };
                }

                var order = new Order
                {
                    CustomerId = model.CustomerId,
                    PartnerId = shop.PartnerId,
                    UserId = _workContext.CurrentUserId,
                    ShopId = shop.Id,
                    OrderNumber = model.OrderCode,
                    TotalCost = model.TotalAmount
                };

                order.OrderDetails = new List<OrderDetail>();
                foreach (var item in model.OrderDetails)
                {
                    var productSize = await _productSizeRepository.GetByIdAsync(item.ProductSizeId);
                    if (productSize is null)
                    {
                        return new BusinessLogicResult<Order>
                        {
                            Success = false,
                            Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra", $"Sản phẩm với Id {item.ProductSizeId} không tồn tại. ") })
                        };
                    }
                    order.OrderDetails.Add(new OrderDetail
                    {
                        PartnerId = shop.PartnerId,
                        ProductSizeId = productSize.Id,
                        Quantity = item.Quantity,
                        UnitPrice = productSize.Cost,
                        CreatedBy = _workContext.CurrentUser.FullName
                    });
                }

                _orderRepository.Add(order, _workContext.CurrentUser.FullName);
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

        public async Task<BusinessLogicResult<List<OrderModel>>> GetAllOrderWithPaging(int shopId, int pageIndex, int pageSize, string filter)
        {
            try
            {
                var sql = "exec [dbo].[AllOrderByShopWithPaging] @p0, @p1, @p2, @p3";
                var queryRes = await _unitOfWork.Context.Database.SqlQuery<OrderModel>(sql, shopId, pageIndex, pageSize, "").ToListAsync();

                if (queryRes.Any())
                {
                    var allOrderDetails = await GetAllOrderDetailsWithOrderIds(queryRes.Select(x => x.Id).ToList());
                    queryRes.ForEach(order =>
                    {
                        order.OrderDetails = allOrderDetails.Where(x => x.OrderId == order.Id).ToList();
                    });
                }

                return new BusinessLogicResult<List<OrderModel>>
                {
                    Result = queryRes,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<List<OrderModel>>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra", ex.Message) })
                };
            }
            
        }

        public BusinessLogicResult<PartnerDailyOrderReport> GetDailyOrders(int partnerId)
        {
            var orders = _orderRepository.GetDailyOrders(partnerId);

            var dailyOrderReport = new PartnerDailyOrderReport();
            var partnerQuery = _partnerService.GetById(partnerId);
            
            dailyOrderReport.PartnerId = partnerQuery.Result.Id;
            dailyOrderReport.PartnerName = partnerQuery.Result.Name;

            var shops = partnerQuery.Result.Shops.ToList();

            foreach (var shop in shops)
            {
                var tempData = new ShopDailyReportModel
                {
                    Id = shop.Id,
                    Name = shop.ShopName,
                    Address = shop.Address,
                    PhoneNumber = shop.PhoneNumber,
                    Orders = orders.Where(x => x.ShopId == shop.Id).Select(y => new OrdersInDayModel
                    {
                        Id = y.Id,
                        CreatedDate = y.CreatedOnUtc,
                        CustomerName = y.Customer.FullName,
                        Address = y.Customer.Address,
                        PhoneNumber = y.Customer.PhoneNumber,
                        StaffName = y.User.FullName,
                        TotalCost = y.TotalCost
                    }).ToList()
                };
                dailyOrderReport.Shops.Add(tempData);
            }

            return new BusinessLogicResult<PartnerDailyOrderReport>
            {
                Result = dailyOrderReport,
                Success = true
            };
        }
        #endregion

        #region private methods
        private async Task<List<OrderDetailModelVm>> GetAllOrderDetailsWithOrderIds(List<int> orderIds)
        {
            var sql = @"select od.Id,
                               od.OrderId,
                               size.Name as Size,
							   od.UnitPrice as Cost,
							   od.Quantity,
							   p.ProductName
                        from OrderDetail od
                        join ProductSize pd on od.ProductSizeId = pd.Id
                        join Product p on p.Id = pd.ProductId
                        join Size size on size.Id = pd.SizeId
                        where od.OrderId in (select data from [dbo].[nop_splitstring_to_table](@p0,','))                          
                    ";
            var result = await _unitOfWork.Context.Database.SqlQuery<OrderDetailModelVm>(sql, string.Join(",", orderIds)).ToListAsync();
            return result;
        }
        #endregion

    }
}
