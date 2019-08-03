using COF.BusinessLogic.Models.Customer;
using COF.DataAccess.EF.Models;
using COF.DataAccess.EF.Repositories;
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

    }
    public class CustomerService : ICustomerService
    {
        #region fields
        private readonly ICustomerRepository _customerRepository;
        #endregion

        #region ctor
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
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
                PhoneNumber = x.PhoneNumber
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


        #endregion
    }
}
