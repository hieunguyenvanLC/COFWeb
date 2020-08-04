using COF.API.Core;
using COF.API.Models.Customer;
using COF.BusinessLogic.Services;
using COF.BusinessLogic.Settings;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ServiceModels = COF.BusinessLogic.Models;
namespace COF.API.Api
{
    [RoutePrefix("api/customer")]
    [Authorize]
    public class CustomerController : ApiControllerBase
    {
        #region fileds
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        #endregion

        #region ctor

        public CustomerController(
            ICustomerService customerService, 
            IUserService userService,
            IWorkContext workContext)
        {
            _customerService = customerService;
            _userService = userService;
            _workContext = workContext;
        }
        #endregion

        #region public methods
        [Route("search")]
        [HttpGet]
        public async Task<HttpResponseMessage> SearchCustomer(string keyword = "")
        {
            var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
            var result = await _customerService.GetAllCustomersAsync(user.PartnerId.GetValueOrDefault(),keyword);
            return SuccessResult(result);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetCustomerInfo([FromUri] int id)
        {
            if (id <= 0)
            {
                return ErrorResult("Thông tin khách hàng không tồn tại");
            }
            var result = await _customerService.GetByIdAsync(id);
            if (result is null) return ErrorResult("Thông tin khách hàng không tồn tại");
            return SuccessResult(result);
        }
        
        [Route("info")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetCustomerInfo()
        {
            if (!User.IsInRole("Customer"))
            {
                return ErrorResult("User không có role Khách hàng.");
            }

            var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
            var result = await _customerService.GetByUserName(user.UserName);
            if (result is null) return ErrorResult("Thông tin khách hàng không tồn tại");
            return SuccessResult(result);
        }

        [Route("create")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetCustomerInfo([FromBody] CustomerCreateModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ErrorResult(ModelStateErrorMessage());
                }

                var createModel = new ServiceModels.Customer.CustomerCreateModel
                {
                    Address = model.Address,
                    Email = model.Email,
                    FullName = model.FullName,
                    Gender = model.Gender,
                    PhoneNumber = model.PhoneNumber
                };
                var logicResult = await _customerService.CreateAsync(_workContext.CurrentUser.PartnerId.GetValueOrDefault(),createModel);
                if (logicResult.Validations != null)
                {
                    return ErrorResult(logicResult.Validations.Errors[0].ErrorMessage);
                }

                return SuccessResult(new CustomerCreateResultModel { CustomerId = logicResult.Result.Id });

            }
            catch (Exception ex)
            {
                return ErrorResult(ex.Message);
            }
            
        }

        #endregion
    }
}
