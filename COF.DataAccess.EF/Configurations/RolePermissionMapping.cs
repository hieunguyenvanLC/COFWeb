using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Configurations
{
    public class RolePermissionMapping : EntityTypeConfiguration<RolePermission>
    {
        public RolePermissionMapping()
        {
            ToTable("RolePermission");
            HasRequired(x => x.Role).WithMany(x => x.RolePermissions).HasForeignKey(x => x.RoleId);
            HasRequired(x => x.Permission).WithMany(x => x.RolePermissions).HasForeignKey(x => x.PermissionId);

        }
    }
}
