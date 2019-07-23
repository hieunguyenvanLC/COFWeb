using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Configurations
{
    public class TableMapping : EntityTypeConfiguration<Table>
    {
        public TableMapping()
        {
            ToTable("Table");
            HasRequired(x => x.Shop).WithMany(x => x.Tables).HasForeignKey(x => x.ShopId).WillCascadeOnDelete(false);
        }
    }
}
