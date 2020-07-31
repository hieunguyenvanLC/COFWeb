using COF.API.Controllers.Core;
using COF.API.Models.Shared;
using COF.BusinessLogic.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ServiceModels = COF.BusinessLogic.Models;

namespace COF.API.Controllers
{
    [Authorize(Roles = "ShopManager,PartnerAdmin")]
    public class CustomerController : MvcControllerBase
    {
        #region fields
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        #endregion

        #region ctor
        public CustomerController(
            ICustomerService customerService,
            IUserService userService)
        {
            _customerService = customerService;
            _userService = userService;
        }
        #endregion
        [Route("khach-hang")]
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetAllCustomerWithPaging(
           int pageIndex = 1,
           int pageSize = 10,
           string keyword = "",
           string filter = "")
        {
            try
            {
                var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
                var queryRes = await _customerService.GetAllCustomerWithPaging(user.PartnerId.GetValueOrDefault(), pageIndex, pageSize, keyword);
                if (!queryRes.Success)
                {
                    return HttpGetErrorResponse(queryRes.Validations.Errors[0].ToString());
                }
                var totalData = queryRes.Result;
                var record = totalData.FirstOrDefault();
                var totalRecord = record.RowCounts;

                totalData.Remove(record);
                var res = new PaginationSet<ServiceModels.Customer.CustomerSearchPagingModel>
                {
                    Items = totalData,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalRows = totalRecord.GetValueOrDefault()
                };
                return HttpGetSuccessResponse(res);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}