using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Configurations
{
    public class AppRoleMapping : EntityTypeConfiguration<AppRole>
    {
        public AppRoleMapping()
        {
            ToTable("Role");
        }
    }
}
