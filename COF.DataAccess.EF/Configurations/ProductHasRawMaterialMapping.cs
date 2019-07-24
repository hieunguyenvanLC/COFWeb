using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Configurations
{
    public class ProductHasRawMaterialMapping : EntityTypeConfiguration<ProductHasRawMaterial>
    {
        public ProductHasRawMaterialMapping()
        {
            ToTable("ProductHasRawMaterial");
            HasRequired(x => x.Product).WithMany(x => x.ProductHasRawMaterials).HasForeignKey(x => x.ProductId);

            HasRequired(x => x.RawMaterial).WithMany(x => x.ProductHasRawMaterials).HasForeignKey(x => x.RawMaterialId);

        }
    }
}
