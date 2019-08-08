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
}
