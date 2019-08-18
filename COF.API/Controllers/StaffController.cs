using COF.API.Controllers.Core;
using COF.API.Models.Shared;
using COF.BusinessLogic.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ServiceModels = COF.BusinessLogic.Models;
namespace COF.API.Controllers
{
    [Authorize(Roles = "PartnerAdmin")]
    public class StaffController : MvcControllerBase
    {
        #region fields 
        private readonly IUserService _userService;
        #endregion

        #region ctor
        public StaffController(
            IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        [Route("nhan-vien")]
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetAllUserWithPaging(
           int pageIndex = 1,
           int pageSize = 10,
           string keyword = "",
           string filter = "")
        {
            try
            {
                var user = await _userService.GetByIdAsync(User.Identity.GetUserId());
                var queryRes = await _userService.GetAllUserWithPaging(user.PartnerId.GetValueOrDefault(), pageIndex, pageSize, keyword);
                if (!queryRes.Success)
                {
                    return HttpGetErrorResponse(queryRes.Validations.Errors[0].ToString());
                }
                var totalData = queryRes.Result;
                var record = totalData.FirstOrDefault();
                var totalRecord = record.RowCounts;

                totalData.Remove(record);
                var res = new PaginationSet<ServiceModels.User.UserPagingModel>
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