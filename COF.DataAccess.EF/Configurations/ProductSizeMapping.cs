using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Configurations
{
    class ProductSizeMapping : EntityTypeConfiguration<ProductSize>
    {
        public ProductSizeMapping()
        {
            ToTable("ProductSize");
            HasRequired(x => x.Product).WithMany(x => x.ProductSizes).HasForeignKey(x => x.ProductId).WillCascadeOnDelete(false);

            HasRequired(x => x.Size).WithMany(x => x.ProductSizes).HasForeignKey(x => x.SizeId).WillCascadeOnDelete(false);

        }
    }
}
