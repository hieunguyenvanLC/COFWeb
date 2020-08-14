using COF.API.Core;
using COF.API.Filter.Api;
using COF.API.Models.Customer;
using COF.BusinessLogic.Services;
using COF.DataAccess.EF.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ServiceModels = COF.BusinessLogic.Models;

namespace COF.API.Api
{
    [RoutePrefix("api/account")]
    [Authorize]
    public class AccountController : ApiControllerBase
    {
        #region fields
        private readonly IUserService _userService;
        private readonly IPartnerService _partnerService;
        private readonly ICustomerService _customerService;
        #endregion
        #region ctor
        public AccountController(
            IUserService userService, 
            IPartnerService partnerService,
            ICustomerService customerService)
        {
            _userService = userService;
            _partnerService = partnerService;
            _customerService = customerService;
        }
        #endregion

        #region public methods
        [HttpGet]
        [Route("partner/{partnerId}")]
        [ValidateRolePermission(RoleClaim = "PartnerAdmin,ShopManager")]
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

        [Route("register")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<HttpResponseMessage> GetCustomerInfo([FromBody] CustomerRegisterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ErrorResult(ModelStateErrorMessage());
                }

                var partnerId = _partnerService.GetAll().Result.FirstOrDefault().Id;
                model.PartnerId = partnerId;

                var user = new AppUser
                {
                    UserName = model.UserName,
                    PartnerId = model.PartnerId,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    FullName = model.FullName,
                    EmailConfirmed = true,
                    Email = model.Email,
                    BirthDay = DateTime.Now,
                    Avatar = "",
                    Gender = true,

                };
                

                var result = await AppUserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var createdUser = AppUserManager.FindByName(user.UserName);

                    AppUserManager.AddToRoles(createdUser.Id, new string[] { "Customer" });

                    var createModel = new ServiceModels.Customer.CustomerCreateModel()
                    {
                        Address = model.Address,
                        Email = model.Email,
                        FullName = model.FullName,
                        Gender = model.Gender,
                        PhoneNumber = model.PhoneNumber,
                        Username = user.UserName
                    };

                    var logicResult = await _customerService.CreateAsync(model.PartnerId, createModel);
                    if (logicResult.Validations != null)
                    {
                        AppUserManager.Delete(createdUser);
                        return ErrorResult(logicResult.Validations.Errors[0].ErrorMessage);
                        
                    }

                    return SuccessResult("Tạo mới thành công.");
                }
                else
                {
                    var errors = string.Join("<br/>", result.Errors.Select(x => $"- {x}"));
                    return ErrorResult(message: $"{errors}");
                }

            }
            catch (Exception ex)
            {
                var createdUser = AppUserManager.FindByName(model.UserName);
                if (createdUser != null) AppUserManager.Delete(createdUser);
                return ErrorResult(ex.Message);
            }
        }
        #endregion


    }
}
