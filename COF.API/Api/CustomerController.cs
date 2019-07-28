using COF.API.Core;
using COF.BusinessLogic.Services;
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
    public class CustomerController : ApiControllerBase
    {
        #region fileds
        private readonly ICustomerService _customerService;
        #endregion

        #region ctor

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        #endregion

        #region public methods
        [Route("search")]
        [HttpGet]
        public async Task<HttpResponseMessage> SearchCustomer(string keyword)
        {
            var result = await _customerService.GetAllCustomersAsync(keyword);
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
