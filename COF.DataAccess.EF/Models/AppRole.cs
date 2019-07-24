using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base()
        {

        }


        public AppRole(string name, string description) : base(name)
        {
            this.Description = description;
        }
        public virtual string Description { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
