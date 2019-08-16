using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Configurations
{
    public class OrderMapping : EntityTypeConfiguration<Order>
    {
        public OrderMapping()
        {
            ToTable("Order");
            HasRequired(x => x.Shop).WithMany(x => x.Orders).HasForeignKey(x => x.ShopId).WillCascadeOnDelete(false);
            HasOptional(x => x.Customer).WithMany(x => x.Orders).HasForeignKey(x => x.CustomerId).WillCascadeOnDelete(false);
            HasRequired(x => x.User).WithMany(x => x.Orders).HasForeignKey(x => x.UserId).WillCascadeOnDelete(false);
        }
    }
}
