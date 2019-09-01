using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Models.User
{
    public class UserRoleModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Roles { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class UserPagingModel
    {
        public long? RowCounts { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Roles { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Shop { get; set; }
        public string JoinDate => CreatedDate.HasValue ? CreatedDate.Value.ToString("dd-MM-yyyy") : "";
    }

    public class UserDetailModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public int? ShopId { get; set; }
        public string RoleId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
