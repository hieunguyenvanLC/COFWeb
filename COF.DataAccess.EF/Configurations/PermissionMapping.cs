using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Configurations
{
    public class PermissionMapping : EntityTypeConfiguration<Permission>
    {
        public PermissionMapping()
        {
            ToTable("Permission");

        }
    }
}
