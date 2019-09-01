using COF.API.Controllers.Core;
using COF.API.Models.Shared;
using COF.BusinessLogic.Models.User;
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
        private readonly IRoleService _roleService;
        #endregion

        #region ctor
        public StaffController(
            IUserService userService,
            IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
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

        
        [HttpPost]
        public async Task<ActionResult> GetById(string id)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelStateErrorMessages();
                var message = string.Join("<br/>", errors.Select(x => $"- {x}"));
                return HttpPostErrorResponse(message);
            }
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user is null)
                {
                    return HttpPostErrorResponse(message: "User không tồn tại.");
                }

                var result = new UserDetailModel
                {
                    Username = user.UserName,
                    Address = user.Address,
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber, 
                    ShopId = user.ShopHasUsers.FirstOrDefault()?.ShopId
                };

                
                var roles = UserManager.GetRoles(user.Id);
                var role = await _roleService.GetByNameAsync(roles.FirstOrDefault());
                result.RoleId = role?.Id;
                return HttpPostSuccessResponse(result);                
            }
            catch (Exception ex)
            {
                return HttpPostErrorResponse(message: ex.Message);
            }
        }
    }
}