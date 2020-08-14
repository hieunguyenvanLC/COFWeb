using COF.DataAccess.EF.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace COF.API.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public AuthorizationServerProvider()
        {
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            await Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            UserManager<AppUser> userManager = context.OwinContext.GetUserManager<UserManager<AppUser>>();
            AppUser user;
            try
            {
                user = await userManager.FindAsync(context.UserName, context.Password);
            }
            catch (Exception ex)
            {
                // Could not retrieve the user due to error.
                context.SetError("server_error", $"Error during processing. {ex.Message}");
                context.Rejected();
                return;
            }

            if (user != null)
            {
                
                var roles = userManager.GetRoles(user.Id);
                ClaimsIdentity identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ExternalBearer);
                string avatar = string.IsNullOrEmpty(user.Avatar) ? "" : user.Avatar;
                string email = string.IsNullOrEmpty(user.Email) ? "" : user.Email;
                identity.AddClaim(new Claim("fullName", user.FullName));
                identity.AddClaim(new Claim("avatar", avatar));
                identity.AddClaim(new Claim("email", email));
                identity.AddClaim(new Claim("username", user.UserName));
                identity.AddClaim(new Claim("roles", JsonConvert.SerializeObject(roles)));

                var allShops = user.ShopHasUsers.Select(x => new { Id = x.ShopId, Name = x.Shop.ShopName }).ToList();
                var props = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        {"fullName", user.FullName},
                        {"avatar", avatar },
                        {"email", email},
                        {"username", user.UserName},
                        {"roles",JsonConvert.SerializeObject(roles) },
                        {"parnterId", user.PartnerId.GetValueOrDefault().ToString()},
                        {"shopIds", JsonConvert.SerializeObject(allShops)}

                    });
                context.Validated(new AuthenticationTicket(identity, props));
            }
            else
            {
                context.SetError("invalid_grant", "Tài khoản hoặc mật khẩu không đúng.");
                context.Rejected();
            }
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }

    }
}