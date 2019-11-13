using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Configurations
{
    public class ProductSizeRawMaterialMapping : EntityTypeConfiguration<ProductSizeRawMaterial>
    {
        public ProductSizeRawMaterialMapping()
        {
            ToTable("ProductSizeRawMaterial");
            HasRequired(x => x.ProductSize).WithMany(x => x.ProductSizeRawMaterials).HasForeignKey(x => x.ProductSizeId);
            HasRequired(x => x.RawMaterial).WithMany(x => x.ProductSizeRawMaterials).HasForeignKey(x => x.RawMaterialId);
        }
    }
}
