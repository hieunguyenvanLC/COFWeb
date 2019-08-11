using COF.API.Controllers.Core;
using COF.BusinessLogic.Services;
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
        #endregion

        #region ctor
        public CommonController(ISizeService commonService)
        {
            _commonService = commonService;
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
    }
}