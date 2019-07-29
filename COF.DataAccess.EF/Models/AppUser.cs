using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    
    public class AppUser : IdentityUser
    {
        [MaxLength(256)]
        public string FullName { set; get; }

        [MaxLength(256)]
        public string Address { set; get; }

        public string Avatar { get; set; }

        public DateTime? BirthDay { set; get; }
        public string City { get; set; }
        public string State { get; set; }

        public bool? Gender { get; set; }

        public int? PartnerId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
        public virtual ICollection<Order> Orders { set; get; }
        public virtual ICollection<UserWorkingTime> UserWorkingTimes { get; set; }
        public virtual ICollection<ShopHasUser> ShopHasUsers { get; set; }
    }
}
