using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class RolePermission : BaseEntity
    {
        public bool CanView { get; set; }
        public bool CanModify { get; set; }
        public string RoleId { get; set; }
        public int PermissionId { get; set; }
        public virtual AppRole Role { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
