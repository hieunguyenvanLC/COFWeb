using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Configurations
{
    public class OrderDetailMapping : EntityTypeConfiguration<OrderDetail>
    {
        public OrderDetailMapping()
        {
            ToTable("OrderDetail");
            HasRequired(x => x.Order).WithMany(x => x.OrderDetails).HasForeignKey(x => x.OrderId).WillCascadeOnDelete(false);
            HasRequired(x => x.ProductSize).WithMany(x => x.OrderDetails).HasForeignKey(x => x.ProductSizeId).WillCascadeOnDelete(false);
        }
    }
}
