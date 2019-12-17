using COF.BusinessLogic.Models.Order;
using COF.BusinessLogic.Models.Report;
using COF.BusinessLogic.Settings;
using COF.Common.Helper;
using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Models;
using COF.DataAccess.EF.Repositories;
using FluentValidation.Results;
using Newtonsoft.Json;
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
        Task<BusinessLogicResult<List<OrderModel>>> GetAllOrderWithPaging(int shopId, int pageIndex, int pageSize, string keyword);
        BusinessLogicResult<PartnerDailyOrderReport> GetDailyOrders(int partnerId);
        BusinessLogicResult<List<Order>> GetOrdersInMonth(int partnerId);
        BusinessLogicResult<List<Order>> GetOrdersInMonthByShopId(int shopId);
        BusinessLogicResult<List<Order>> GetOrdersInYear(int partnerId);
        BusinessLogicResult<List<Order>> GetOrdersInYearByShopId(int shopId);
        int GetTotalOrder(int partnerId);
        Task<BusinessLogicResult<bool>> CancelOrder(int partnerId, string orderCode, string cancelByUserId, string reason);
        Task<BusinessLogicResult<Order>> GetByOrderCodeAsync(string orderCode);
        BusinessLogicResult<List<Order>> GetOrdersInRangeShopId(int shopId,DateTime fromDate, DateTime toDate);
        BusinessLogicResult<List<Order>> GetOrdersInRange(int partnerId, DateTime fromDate, DateTime toDate);
        Task<BusinessLogicResult<bool>> CalculateRmsAfterOrderFinshed(int orderId);
    }

    public class OrderService : IOrderService
    {
        #region fields
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShopRepository _shopRepository;
        private readonly IBonusLevelRepository _bonusLevelRepository;
        private readonly IWorkContext _workContext;
        private readonly IProductSizeRepository _productSizeRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IPartnerService _partnerService;
        private readonly IBonusPointHistoryRepository _bonusPointHistoryRepository;
        private readonly IRawMaterialRepository _rawMaterialRepository;
        private readonly IProductSizeRawMaterialRepository _productSizeRawMaterialRepository;
        private readonly IRawMaterialHistoryRepository _materialHistoryRepository;
        #endregion

        #region ctor
        public OrderService(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            IShopRepository shopRepository,
            IWorkContext workContext,
            IProductSizeRepository productSizeRepository,
            ICustomerRepository customerRepository,
            IPartnerService partnerService,
            IBonusLevelRepository bonusLevelRepository,
            IBonusPointHistoryRepository bonusPointHistoryRepository,
            IRawMaterialRepository rawMaterialRepository,
            IProductSizeRawMaterialRepository productSizeRawMaterialRepository,
            IRawMaterialHistoryRepository materialHistoryRepository

           )
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _shopRepository = shopRepository;
            _workContext = workContext;
            _productSizeRepository = productSizeRepository;
            _customerRepository = customerRepository;
            _partnerService = partnerService;
            _bonusLevelRepository = bonusLevelRepository;
            _bonusPointHistoryRepository = bonusPointHistoryRepository;
            _rawMaterialRepository = rawMaterialRepository;
            _productSizeRawMaterialRepository = productSizeRawMaterialRepository;
            _materialHistoryRepository = materialHistoryRepository;
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
                Customer customer = null; 
                if (model.CustomerId != null)
                {
                    customer = await _customerRepository.GetByIdAsync(model.CustomerId);

                    if (customer is null)
                    {
                        return new BusinessLogicResult<Order>
                        {
                            Success = false,
                            Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Khách hàng", "Khách hàng không tồn tại.") })
                        };
                    }
                }

                var order = await _orderRepository.GetByOrderCode(model.OrderCode);

                // validate use Active Bonus Point
                if (model.DiscountType == DiscountType.UseActiveBonus)
                {
                    if ((decimal) model.FinalAmount >= customer.ActiveBonusPoint * 1000.0m)
                    {
                        return new BusinessLogicResult<Order>
                        {
                            Success = false,
                            Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Khách hàng", "Tổng số điểm hiện tại không đủ để trừ.") })
                        };
                    }
                }

                if (order is null)
                {

                    order = new Order
                    {
                        CustomerId = model.CustomerId,
                        PartnerId = shop.PartnerId,
                        UserId = _workContext.CurrentUserId,
                        ShopId = shop.Id,
                        OrderCode = model.OrderCode,
                        TotalAmount = model.TotalAmount,
                        CheckInDate = model.CheckInDate,
                        ApproveDate = model.ApproveDate,
                        FinalAmount = model.FinalAmount,
                        OrderStatus = model.OrderStatus,
                        Notes = model.Notes,
                        FeeDescription = model.FeeDescription,
                        CheckInPerson = model.CheckInPerson,
                        ApprovePerson = model.ApprovePerson,
                        SourceId = model.SourceID,
                        TableId = model.TableId,
                        IsFixedPrice = model.IsFixedPrice,
                        SourceType = model.SourceType,
                        LastRecordDate = model.LastRecordDate,
                        ServedPerson = model.ServedPerson,
                        DeliveryAddress = model.DeliveryAddress,
                        DeliveryStatus = model.DeliveryStatus,
                        DeliveryCustomer = model.DeliveryCustomer,
                        TotalInvoicePrint = model.TotalInvoicePrint,
                        VAT = model.VAT,
                        VATAmount = model.VATAmount,
                        NumberOfGuest = model.NumberOfGuest,
                        Att1 = model.Att1,
                        Att2 = model.Att2,
                        Att3 = model.Att3,
                        Att4 = model.Att4,
                        Att5 = model.Att5,
                        GroupPaymentStatus = model.GroupPaymentStatus,
                        LastModifiedOrderDetail = model.LastModifiedOrderDetail,
                        LastModifiedPayment = model.LastModifiedPayment,
                        ApiLog = JsonConvert.SerializeObject(model),
                        DiscountType = model.DiscountType,
                    };

                    order.OrderDetails = new List<OrderDetail>();
                    foreach (var item in model.OrderDetailViewModels)
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
                            CreatedBy = _workContext.CurrentUser.FullName,
                            CategoryId = productSize.Product.CategoryId,
                            Description = ""
                        });
                        
                    }
                    if (order.DiscountType == DiscountType.OneGetOne)
                    {
                        var lowestUnit = order.OrderDetails.OrderBy(x => x.UnitPrice).FirstOrDefault();
                        lowestUnit.UnitPrice = 0;
                        lowestUnit.Description = "Sản phẩm thuộc diện mua 1 tặng 1";
                    }

                    _orderRepository.Add(order, _workContext.CurrentUser.FullName);
                }
                else
                {
                    if (order.OrderStatus !=
                     OrderStatus.PosCancel || order.OrderStatus != OrderStatus.PosPreCancel ||
                     order.OrderStatus == OrderStatus.PreCancel)
                    {
                        order.CancelDate = DateTimeHelper.CurentVnTime;
                        order.CancelBy = _workContext.CurrentUser.FullName;
                    }

                    order.OrderStatus = model.OrderStatus;
                    _orderRepository.Update(order);
                    await _unitOfWork.SaveChangesAsync();
                }
                if (order.OrderStatus != 
                    OrderStatus.PosCancel && order.OrderStatus != OrderStatus.PosPreCancel &&
                    order.OrderStatus !=  OrderStatus.PreCancel)
                {
                    if (customer != null)
                    {
                        var allLevels = await _bonusLevelRepository.GetAllAsync();
                        var point = Math.Round((decimal)model.FinalAmount * 1.0m / customer.BonusLevel.MoneyToOnePoint);


                        var orderHistory = new BonusPointHistory
                        {
                            PartnerId = customer.PartnerId,
                            CustomerId = customer.Id,
                            OldLevel = customer.BonusLevel.Name,
                            OldPoint = customer.ActiveBonusPoint
                        };

                        customer.TotalBonusPoint += point;

                        if (model.DiscountType == DiscountType.UseActiveBonus)
                        {
                            customer.ActiveBonusPoint -= point;
                        }
                        else
                        {
                            customer.ActiveBonusPoint += point;
                        }

                        var newLevel = allLevels.FirstOrDefault(x => x.StartPointToReach <= customer.TotalBonusPoint && x.EndPointToReach >= customer.TotalBonusPoint);
                        customer.BonusLevelId = newLevel.Id;

                        orderHistory.Level = newLevel.Name;
                        orderHistory.Point = customer.ActiveBonusPoint;
                        _bonusPointHistoryRepository.Add(orderHistory);
                        _customerRepository.Update(customer);
                    }
                    await _unitOfWork.SaveChangesAsync();
                    await CalculateRmsAfterOrderFinshed(order.Id);
                    
                }

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

        public async Task<BusinessLogicResult<List<OrderModel>>> GetAllOrderWithPaging(int shopId, int pageIndex, int pageSize, string keyword)
        {
            try
            {
                var sql = "exec [dbo].[AllOrderByShopWithPaging] @p0, @p1, @p2, @p3";
                var queryRes = await _unitOfWork.Context.Database.SqlQuery<OrderModel>(sql, shopId, pageIndex, pageSize, keyword).ToListAsync();

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
                        TotalCost = y.FinalAmount
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

        public BusinessLogicResult<List<Order>> GetOrdersInMonth(int partnerId)
        {
            var allOrders = _orderRepository.GetAllOrdersInCurrentMonth(partnerId);
            return new BusinessLogicResult<List<Order>>
            {
                Result = allOrders,
                Success = true
            };
        }

        public BusinessLogicResult<List<Order>> GetOrdersInMonthByShopId(int shopId)
        {
            var allOrders = _orderRepository.GetAllOrdersInCurrentMonthByShop(shopId);
            return new BusinessLogicResult<List<Order>>
            {
                Result = allOrders,
                Success = true
            };
        }

        public BusinessLogicResult<List<Order>> GetOrdersInYear(int partnerId)
        {
            var allOrders = _orderRepository.GetAllOrdersInCurrentYear(partnerId);
            return new BusinessLogicResult<List<Order>>
            {
                Result = allOrders,
                Success = true
            };
        }

        public BusinessLogicResult<List<Order>> GetOrdersInYearByShopId(int shopId)
        {
            var allOrders = _orderRepository.GetAllOrdersInCurrentYearByShop(shopId);
            return new BusinessLogicResult<List<Order>>
            {
                Result = allOrders,
                Success = true
            };
        }

        public int GetTotalOrder(int partnerId)
        {
            return _orderRepository.GetTotalOrder(partnerId);
        }

        public async Task<BusinessLogicResult<bool>> CancelOrder(int partnerId, string orderCode,string cancelByUserId, string reason)
        {
            try
            {
                var order = await _orderRepository.GetSingleAsync(filter: x => x.OrderCode == orderCode && x.PartnerId == partnerId);
                if (order is null)
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra", $"Mã hóa đơn không tồn tại.") })
                    };
                }

                order.OrderStatus = OrderStatus.PreCancel;
                order.CancelDate = DateTimeHelper.CurentVnTime;
                order.CancelBy = cancelByUserId;
                _orderRepository.Update(order, cancelByUserId );
                await _unitOfWork.SaveChangesAsync();
                return new BusinessLogicResult<bool>
                {
                    Result = true,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra", ex.Message) })
                };
            }
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
							   p.ProductName,
                               od.Description
                        from OrderDetail od
                        join ProductSize pd on od.ProductSizeId = pd.Id
                        join Product p on p.Id = pd.ProductId
                        join Size size on size.Id = pd.SizeId
                        where od.OrderId in (select data from [dbo].[nop_splitstring_to_table](@p0,','))                          
                    ";
            var result = await _unitOfWork.Context.Database.SqlQuery<OrderDetailModelVm>(sql, string.Join(",", orderIds)).ToListAsync();
            return result;
        }

        public BusinessLogicResult<List<Order>> GetOrdersInRangeShopId(int shopId, DateTime fromDate, DateTime toDate)
        {
            var result = _orderRepository.GetAllOrdersInRangeByShop(shopId, fromDate, toDate);
            return new BusinessLogicResult<List<Order>>
            {
                Success = true,
                Result = result
            };
        }

        public BusinessLogicResult<List<Order>> GetOrdersInRange(int partnerId, DateTime fromDate, DateTime toDate)
        {
            var result = _orderRepository.GetAllOrdersInRange(partnerId, fromDate, toDate);
            return new BusinessLogicResult<List<Order>>
            {
                Success = true,
                Result = result
            };
        }

        public async Task<BusinessLogicResult<Order>> GetByOrderCodeAsync(string orderCode)
        {
            var result = await _orderRepository.GetByOrderCode(orderCode);
            return new BusinessLogicResult<Order>
            {
                Success = true,
                Result = result
            };
        }

        public async Task<BusinessLogicResult<bool>> CalculateRmsAfterOrderFinshed(int orderId)
        {
            try
            {  
                var order = await _orderRepository.GetByIdAsync(orderId);
                var orderDetails = order.OrderDetails;
                var productSizeIds = orderDetails.Select(x => x.ProductSizeId).Distinct().ToList();
                
                var allRms = await _productSizeRawMaterialRepository.GetByFilterAsync(x => productSizeIds.Contains(x.ProductSizeId));
                foreach (var detail in orderDetails)
                {             
                    var rms = allRms.Where(x => x.ProductSizeId == x.ProductSizeId).ToList();
                    foreach (var rawMaterial in rms)
                    {
                        var currentRm = await _rawMaterialRepository.GetByIdAsync(rawMaterial.RawMaterialId);
                        var amount = rawMaterial.Amount * detail.Quantity;
                        currentRm.AutoTotalQty -= amount;
                        var transaction = new RawMaterialHistory
                        {
                            PartnerId = order.PartnerId,
                            InputTypeId = InputType.Auto,
                            Quantity = amount,
                            TotalQtyAtTimeAccess = currentRm.AutoTotalQty,
                            TimeAccess = DateTimeHelper.CurentVnTime,
                            RawMaterialId = currentRm.Id,
                            TransactionTypeId = TransactionType.Decreasement,
                            CreatedBy = order.CreatedBy,
                            Description = $"Đơn hàng #{order.OrderCode}, Sản phẩm : {detail.ProductSize.Product.ProductName} , Size : {detail.ProductSize.Size.Name}, Số lượng : {detail.Quantity}",
                            OrderId = order.Id
                        };
                        _materialHistoryRepository.Add(transaction);
                        await _unitOfWork.SaveChangesAsync();

                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

    }
}
