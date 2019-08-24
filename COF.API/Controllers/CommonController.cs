using COF.API.Controllers.Core;
using COF.BusinessLogic.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace COF.API.Controllers
{
    [Authorize]
    public class CommonController : MvcControllerBase
    {
        #region fields
        private readonly ISizeService _commonService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;


        #endregion
        #region ctor
        public CommonController(
            ISizeService commonService,
            IRoleService roleService)
        {
            _commonService = commonService;
            _roleService = roleService;
        }
        #endregion

        [HttpGet]
        public async Task<JsonResult> GetAllSizes()
        {
            try
            {
                var res = await _commonService.GetAllSizesAsync();
                return HttpGetSuccessResponse(res);
            }
            catch (Exception ex)
            {
                return HttpGetErrorResponse(ex.Message);
            }
           
        }

        [HttpGet]
        public async Task<JsonResult> GetAllRoles()
        {
            try
            {
                var allRoles = await _roleService.GetAllRoles();
                var result = allRoles.Select(x => new
                {
                    RoleId = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList();
                return HttpGetSuccessResponse(result);
            }
            catch (Exception ex)
            {
                return HttpGetErrorResponse(ex.Message);
            }

        }


    }
}