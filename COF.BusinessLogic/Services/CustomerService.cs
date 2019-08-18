using COF.BusinessLogic.Models.Customer;
using COF.BusinessLogic.Settings;
using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Models;
using COF.DataAccess.EF.Repositories;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services
{
    public interface ICustomerService
    {
        /// <summary>
        /// Find customer by keyword (FullName, PhoneNumber) 
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        Task<List<CustomerModel>> GetAllCustomersAsync(int partnerId, string keyword);

        /// <summary>
        /// Get customer by id async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CustomerModel> GetByIdAsync(int id);

        Task<BusinessLogicResult<Customer>> CreateAsync(int partnerId, CustomerCreateModel model);
        
        /// <summary>
        /// Gets the total customer.
        /// </summary>
        /// <param name="partnerId">The partner identifier.</param>
        /// <returns></returns>
        int GetTotalCustomer(int partnerId);

        /// <summary>
        /// Gets all customer with paging.
        /// </summary>
        /// <param name="partnerId">The partner identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="keyword">The keyword.</param>
        /// <returns></returns>
        Task<BusinessLogicResult<List<CustomerSearchPagingModel>>> GetAllCustomerWithPaging(int partnerId, int pageIndex, int pageSize, string keyword);
    }
    public class CustomerService : ICustomerService
    {
        #region fields
        private readonly ICustomerRepository _customerRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkContext _workContext;
        #endregion

        #region ctor
        public CustomerService(
            ICustomerRepository customerRepository,
            IShopRepository shopRepository,
            IUnitOfWork unitOfWork,
            IWorkContext workContext)
        {
            _customerRepository = customerRepository;
            _shopRepository = shopRepository;
            _unitOfWork = unitOfWork;
            _workContext = workContext;
        }

        
        #endregion

        #region public methods
        public async Task<List<CustomerModel>> GetAllCustomersAsync(int partnerId,string keyword)
        {
            var allCustomers = await _customerRepository.GetAllCustomersAsync(partnerId, keyword);
            var result = allCustomers.Select(x => new CustomerModel
            {
                Id = x.Id,
                FullName = x.FullName,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address
            }).ToList();
            return result;
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            CustomerModel resut = null;
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer != null)
            {
                resut = new CustomerModel
                {
                    Id = customer.Id,
                    FullName = customer.FullName,
                    PhoneNumber = customer.PhoneNumber
                };
            }
            return resut;
        }

        public async Task<BusinessLogicResult<Customer>> CreateAsync(int partnerId, CustomerCreateModel model)
        {
            try
            {
                var customer = new Customer
                {
                    FullName = model.FullName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    PartnerId = partnerId,

                };
                var duplicatedUser = _customerRepository.GetByFilter((x) => x.PhoneNumber == customer.PhoneNumber);
                if (duplicatedUser.Any())
                {
                    return new BusinessLogicResult<Customer>
                    {
                        Success = false,
                        Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra", "Số điện thoại đã được đăng kí.") })
                    };
                }
                _customerRepository.Add(customer, _workContext.CurrentUser.FullName);
                await _unitOfWork.SaveChangesAsync();
                return new BusinessLogicResult<Customer>
                {
                    Result = customer,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<Customer>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra", ex.Message) })
                };
            }
        }

        public int GetTotalCustomer(int partnerId)
        {
            return _customerRepository.GetTotalByPartnerId(partnerId);
        }


        public async Task<BusinessLogicResult<List<CustomerSearchPagingModel>>> GetAllCustomerWithPaging(int partnerId, int pageIndex, int pageSize, string keyword)
        {
            try
            {
                var sql = "exec [dbo].[AllCustomerByPartnerIdWithPaging] @p0, @p1, @p2, @p3";
                var queryRes = await _unitOfWork.Context.Database.SqlQuery<CustomerSearchPagingModel>(sql, partnerId, pageIndex, pageSize, keyword).ToListAsync();

        
                return new BusinessLogicResult<List<CustomerSearchPagingModel>>
                {
                    Result = queryRes,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<List<CustomerSearchPagingModel>>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<ValidationFailure> { new ValidationFailure("Lỗi xảy ra", ex.Message) })
                };
            }

        }

        #endregion
    }
}
