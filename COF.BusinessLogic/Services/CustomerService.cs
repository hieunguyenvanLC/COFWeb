using COF.BusinessLogic.Settings;
using CapstoneProjectServer.DataAccess.EF.Infrastructure;
using CapstoneProjectServer.DataAccess.EF.Models;
using CapstoneProjectServer.DataAccess.EF.Repositories;
using CapstoneProjectServer.Models.dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services
{
    public interface ICustomerService : ILogic
    {
        Task<IEnumerable<Customer>> SearchCustomer(CustomerSearchDto customerSearchDto);
        Task<IEnumerable<Order>> SearchOrder(OrderSearchDto customerSearchDto);
        Task<IEnumerable<Customer>> SearchCustomerByIds(List<int> ids);
        Task<CustomerDetailDto> GetCustomerByIdAsync(int customerId);
        Task<IEnumerable<Order>> SearchAllOrdersByCustomerIdAsync(int customerId);
        Task<bool> HasOrderWithSupplier(int customerId, int supplierId);
        Task<BusinessLogicResult<bool>> DoFeedback(FeedbackDto feedbackDto);
        Task<BusinessLogicResult<Order>> CreateOrder(OrderCreateDto orderCreateDto);
        Task<BusinessLogicResult<bool>> UpdateOrderStatus(UpdateOrderDto updateOrderDto);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderStatus>> GetAllOrderStatus();
        Task<Order> GetOrderByIdForWebApiAsync(int orderId);
        Task<IEnumerable<Feedback>> GetFeedbacks(int supplierId);
        Task<BusinessLogicResult<bool>> ActivateCustomer(int customerId);
        Task<BusinessLogicResult<bool>> UpdateOrderPaymentStatus(int orderId);
        Task<BusinessLogicResult<bool>> CancelOrderByCustomer(CancelOrderDto cancelOrder);
    }
    public class CustomerService : BaseService, ICustomerService
    {

        public CustomerService(IRepositoryHelper repositoryHelper)
        {
            this.RepositoryHelper = repositoryHelper;
            this.UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepositoryHelper RepositoryHelper;

        public async Task<IEnumerable<Customer>> SearchCustomer(CustomerSearchDto customerSearchDto)
        {
            var customerRepo = RepositoryHelper.GetRepository<ICustomerRepository>(UnitOfWork);
            var data = await customerRepo.GetAllCustomerByConditon(customerSearchDto);
            return data;
        }

        public async Task<IEnumerable<Order>> SearchOrder(OrderSearchDto customerSearchDto)
        {
            var orderRepo = RepositoryHelper.GetRepository<IOrderRepository>(UnitOfWork);
            var data = await orderRepo.GetAllOrderSearchConditon(customerSearchDto);
            return data;
        }

        public async Task<IEnumerable<Customer>> SearchCustomerByIds(List<int> ids)
        {
            var customerRepo = RepositoryHelper.GetRepository<ICustomerRepository>(UnitOfWork);
            var data = await customerRepo.GetCustomersByIds(ids);
            return data;
        }

        public async Task<CustomerDetailDto> GetCustomerByIdAsync(int customerId)
        {
            var customerRepo = RepositoryHelper.GetRepository<ICustomerRepository>(UnitOfWork);
            var accountRepo = RepositoryHelper.GetRepository<IAccountRepository>(UnitOfWork);
            var customer = await customerRepo.GetSimpleById(customerId);
            var account = await accountRepo.GetByIdAsync(customer.AccountId);
            CustomerDetailDto customerDetailDto = new CustomerDetailDto()
            {
                CustomerId = customer.CustomerId,
                Address = customer.Address,
                FullName = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                Age = customer.DateOfBirth.HasValue ? (int)((DateTime.Now - customer.DateOfBirth.Value).TotalDays / 365.242199) : default(int?),
                accountDetailDto = new AccountDetailDto()
                {
                    AccountId = account.AccountId,
                    Username = account.Username,
                    CreatedDate = account.CreatedDate.Value.ToString("dd-MM-yyyy")
                }
            };
            return customerDetailDto;
        }

        public async Task<IEnumerable<Order>> SearchAllOrdersByCustomerIdAsync(int customerId)
        {
            var orderRepo = RepositoryHelper.GetRepository<IOrderRepository>(UnitOfWork);
            var data = await orderRepo.GetAllSimpleOrderByCustomerIdAsync(customerId);
            return data;
        }

        public async Task<bool> HasOrderWithSupplier(int customerId, int supplierId)
        {
            var orderRepo = RepositoryHelper.GetRepository<IOrderRepository>(UnitOfWork);
            var result = await orderRepo.HasOrderWithSupplier(customerId, supplierId);
            return result;
        }

        public async Task<BusinessLogicResult<bool>> DoFeedback(FeedbackDto feedbackDto)
        {
            try
            {
                var feedbackRepo = RepositoryHelper.GetRepository<IFeedbackRepository>(UnitOfWork);
                var customerRepo = RepositoryHelper.GetRepository<ICustomerRepository>(UnitOfWork);
                var supplierRepo = RepositoryHelper.GetRepository<ISupplierRepository>(UnitOfWork);
                var customer = await customerRepo.GetByIdAsync(feedbackDto.CustomerId);
                var supplier = await supplierRepo.GetByIdAsync(feedbackDto.SupplierId);
                var feedBack = new Feedback()
                {
                    SupplierId = feedbackDto.SupplierId,
                    CustomerId = feedbackDto.CustomerId,
                    Customer = customer,
                    Supplier = supplier,
                    OrderId = feedbackDto.OrderId,
                    FeedbackContent = feedbackDto.FeedbackContent,
                    NumberOfStart = feedbackDto.NumberOfStar
                };
                feedbackRepo.Create(feedBack, AccountId.ToString());
                var saveResult = await UnitOfWork.SaveChangesAsync();
                if (saveResult.Any())
                {
                    return new BusinessLogicResult<bool>()
                    {
                        Success = false,
                        Validations = saveResult.AsFluentValidationResult()
                    };
                }
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Tạo feedback", ex.Message) })
                };
            }

            return new BusinessLogicResult<bool>()
            {
                Success = true,
                Result = true
            };

        }

        public async Task<BusinessLogicResult<Order>> CreateOrder(OrderCreateDto orderCreateDto)
        {
            var customerRepo = RepositoryHelper.GetRepository<ICustomerRepository>(UnitOfWork);
            var orderRepo = RepositoryHelper.GetRepository<IOrderRepository>(UnitOfWork);
            var supplierRepo = RepositoryHelper.GetRepository<ISupplierRepository>(UnitOfWork);
            var orderDetailRepo = RepositoryHelper.GetRepository<IOrderDetaillRepository>(UnitOfWork);
            using (var transaction = UnitOfWork.BeginTransaction())
            {

                try
                {
                    var customer = await customerRepo.GetByIdAsync(orderCreateDto.CustomerId);
                    decimal? totalPrice = null;
                    if (!orderCreateDto.OrderDetails.Any(x => x.Price == "0"))
                    {
                        totalPrice = orderCreateDto.OrderDetails.Sum(x => decimal.Parse(x.Price) * (1.0m * x.Quantity));
                    }
                    DateTime implementDate;
                    var parseDate = DateTime.TryParseExact(orderCreateDto.ImplementDate + ":00", "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out implementDate);
                    if (!parseDate)
                    {
                        return new BusinessLogicResult<Order>
                        {
                            Success = false,
                            Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Cap nhat trang thai", "Ngày hẹn không đúng format") })
                        };
                    }

                    var order = new Order()
                    {
                        SupplierId = orderCreateDto.SupplierId,
                        CustomerId = orderCreateDto.CustomerId,
                        PhoneNumber = orderCreateDto.PhoneNumber,
                        Address = orderCreateDto.Address,
                        TotalPrice = totalPrice,
                        OrderStatusId = (int)CapstoneProjectServer.Models.Enumrations.Enum.OrderStatus.InProcessing,
                        ImplementDate = implementDate,
                        Description = orderCreateDto.Description,
                        IsPaid = false
                    };
                    orderRepo.Create(order, AccountId.ToString());
                    var dbValresults = await UnitOfWork.SaveChangesAsync();
                    if (dbValresults.Any())
                    {
                        transaction.Rollback();
                        return new BusinessLogicResult<Order>
                        {
                            Success = false,
                            Validations = dbValresults.AsFluentValidationResult(),
                        };
                    }
                    var orderDetails = new List<OrderDetail>();
                    foreach (var item in orderCreateDto.OrderDetails)
                    {
                        var detail = new OrderDetail()
                        {
                            OrderId = order.OrderId,
                            Price = decimal.Parse(item.Price),
                            ServiceId = item.ServiceId,
                            Quantity = item.Quantity,
                            ServiceName = item.Service
                        };
                        orderDetails.Add(detail);
                    }
                    await orderDetailRepo.InsertListOrderDetail(orderDetails, AccountId.ToString());
                    //dbValresults = await UnitOfWork.SaveChangesAsync();
                    //if (dbValresults.Any())
                    //{
                    //    transaction.Rollback();
                    //    return new BusinessLogicResult<Order>
                    //    {
                    //        Success = false,
                    //        Validations = dbValresults.AsFluentValidationResult()
                    //    };
                    //}

                    transaction.Commit();
                    return new BusinessLogicResult<Order>
                    {
                        Success = true,
                        Result = order
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BusinessLogicResult<Order>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Đăng kí thành viên", ex.Message) })
                    };
                }
            }

        }

        public async Task<BusinessLogicResult<bool>> UpdateOrderStatus(UpdateOrderDto updateOrderDto)
        {
            try
            {

                var orderRepo = RepositoryHelper.GetRepository<IOrderRepository>(UnitOfWork);
                var accountRepo = RepositoryHelper.GetRepository<IAccountRepository>(UnitOfWork);
                var order = await orderRepo.GetByIdAsync(updateOrderDto.OrderId);
                var lastUpdatedBy = !String.IsNullOrWhiteSpace(order.UpdatedBy) ? int.Parse(order.UpdatedBy) : int.Parse(order.CreatedBy);
                var lastUpdateAccount = await accountRepo.GetSimpleByIdAsync(lastUpdatedBy);
                if (lastUpdateAccount.AccountTypeId == (int)CapstoneProjectServer.Models.Enumrations.Enum.AccountType.Customer && order.OrderStatusId == (int)CapstoneProjectServer.Models.Enumrations.Enum.OrderStatus.Cancel)
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Cap nhat trang thai", "Không thể cập nhật trạng thái bởi vì khách hàng đã hủy đơn hàng trên.") })
                    };
                }
                if (updateOrderDto.OrderStatusId != null)
                {
                    order.OrderStatu = null;
                    order.OrderStatusId = updateOrderDto.OrderStatusId;
                }
                order.IsPaid = updateOrderDto.IsPaid;
                order.SupplierNote = updateOrderDto.Note;
                if (updateOrderDto.TotalMoney != null)
                {
                    order.TotalPrice = updateOrderDto.TotalMoney;
                }

                orderRepo.Update(order, AccountId.ToString());
                var dbValresults = await UnitOfWork.SaveChangesAsync();
                if (dbValresults.Any())
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = dbValresults.AsFluentValidationResult()
                    };
                }
                return new BusinessLogicResult<bool>
                {
                    Success = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Cập nhật hóa đơn", ex.Message) })
                };
            }


        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var orderRepo = RepositoryHelper.GetRepository<IOrderRepository>(UnitOfWork);
            var result = await orderRepo.GetSimpleByIdAsync(orderId);
            return result;
        }

        public async Task<IEnumerable<OrderStatus>> GetAllOrderStatus()
        {
            var result = await UnitOfWork.Context.Set<OrderStatus>().ToListAsync();
            return result;
        }

        public async Task<Order> GetOrderByIdForWebApiAsync(int orderId)
        {
            var orderRepo = RepositoryHelper.GetRepository<IOrderRepository>(UnitOfWork);
            var result = await orderRepo.GetComplexByIdAsync(orderId);
            return result;
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacks(int supplierId)
        {
            var feedbackRepo = RepositoryHelper.GetRepository<IFeedbackRepository>(UnitOfWork);
            var result = await feedbackRepo.GetFeedbacksBySupplierId(supplierId);
            return result;
        }

        public async Task<BusinessLogicResult<bool>> ActivateCustomer(int customerId)
        {
            try
            {
                var repo = RepositoryHelper.GetRepository<ICustomerRepository>(UnitOfWork);
                var customer = await repo.GetByIdAsync(customerId);
                customer.IsActived = true;
                repo.Update(customer);
                var dbValresults = await UnitOfWork.SaveChangesAsync();
                if (dbValresults.Any())
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = dbValresults.AsFluentValidationResult()
                    };
                }
                return new BusinessLogicResult<bool>
                {
                    Success = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Cập nhật hóa đơn", ex.Message) })
                };
            }
        }

        public async Task<BusinessLogicResult<bool>> UpdateOrderPaymentStatus(int orderId)
        {
            try
            {
                var orderRepo = RepositoryHelper.GetRepository<IOrderRepository>(UnitOfWork);
                var order = await orderRepo.GetByIdAsync(orderId);
                var customerRepo = RepositoryHelper.GetRepository<ICustomerRepository>(UnitOfWork);
                var customer = await customerRepo.GetByIdAsync(order.CustomerId);
                order.IsPaid = true;
                order.PaymentDate = DateTime.UtcNow.AddHours(7);
                orderRepo.Update(order, customer.AccountId.ToString());
                var dbValresults = await UnitOfWork.SaveChangesAsync();
                if (dbValresults.Any())
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = dbValresults.AsFluentValidationResult()
                    };
                }
                return new BusinessLogicResult<bool>
                {
                    Success = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Đăng kí thành viên", ex.Message) })
                };
            }

        }

        public async Task<BusinessLogicResult<bool>> CancelOrderByCustomer(CancelOrderDto cancelOrder)
        {
            try
            {
                var orderRepo = RepositoryHelper.GetRepository<IOrderRepository>(UnitOfWork);
                var customerRepo = RepositoryHelper.GetRepository<ICustomerRepository>(UnitOfWork);
                var order = await orderRepo.GetOrderByIdAsync(cancelOrder.OrderId);
                var diffentDate = (order.ImplementDate.Value - DateTime.UtcNow.AddHours(7));
                if (diffentDate.Hours < 3)
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Cap nhat trang thai", "Thời gian hủy tối đa 3 tiếng trước giờ đặt.") })
                    };
                }
                order.OrderStatusId = (int)CapstoneProjectServer.Models.Enumrations.Enum.OrderStatus.Cancel;
                order.CancelReason = cancelOrder.Reason;
                orderRepo.Update(order, order.Customer.AccountId.ToString());
                var dbValresults = await UnitOfWork.SaveChangesAsync();
                if (dbValresults.Any())
                {
                    return new BusinessLogicResult<bool>
                    {
                        Success = false,
                        Validations = dbValresults.AsFluentValidationResult()
                    };
                }
                return new BusinessLogicResult<bool>
                {
                    Success = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Đăng kí thành viên", ex.Message) })
                };
            }
        }
    }
}
