using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Configurations
{
    public class TableHasOrderMapping : EntityTypeConfiguration<TableHasOrder>
    {
        public TableHasOrderMapping()
        {
            ToTable("TableHasOrder");
            HasRequired(x => x.Table).WithMany(x => x.TableHasOrders).HasForeignKey(x => x.TableId).WillCascadeOnDelete(false);
            HasRequired(x => x.Order).WithMany(x => x.TableHasOrders).HasForeignKey(x => x.OrderId).WillCascadeOnDelete(false);
        }
    }
}
