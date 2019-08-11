using COF.API.Core;
using COF.API.Filter.Api;
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
    [RoutePrefix("api/account")]
    [Authorize]
    public class AccountController : ApiControllerBase
    {
        #region fields
        private readonly IUserService _userService;
        private readonly IPartnerService _partnerService;
        #endregion
        #region ctor
        public AccountController(IUserService userService, IPartnerService partnerService)
        {
            _userService = userService;
            _partnerService = partnerService;
        }
        #endregion

        #region public methods
        [HttpGet]
        [Route("partner/{partnerId}")]
        [ValidateRolePermissionAttribute(RoleClaim ="PartnerAdmin")]
        public async Task<HttpResponseMessage> GetAllAccountByPartnerId([FromUri]int partnerId)
        {
            try
            { 
                var partnerInfo = await _partnerService.GetByIdAsync(partnerId);
                var partner = partnerInfo.Result;

                if (partner is null)
                {
                    return ErrorResult("Đối tác không tồn tại");
                }
                var allUsers = await _userService.GetAppUsersByPartnerId(partnerId);
                return SuccessResult(allUsers.Result);
            }
            catch (Exception ex)
            {
                return ErrorResult(ex.Message);
            }   
        }
       

        #endregion


    }
}
