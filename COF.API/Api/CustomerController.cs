using COF.API.Core;
using COF.BusinessLogic.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace COF.API.Api
{
    [RoutePrefix("api/customer")]
    [Authorize]
    public class CustomerController : ApiControllerBase
    {
        #region fileds
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        #endregion

        #region ctor

        public CustomerController(ICustomerService customerService, IUserService userService)
        {
            _customerService = customerService;
            _userService = userService;
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

        #endregion
    }
}
