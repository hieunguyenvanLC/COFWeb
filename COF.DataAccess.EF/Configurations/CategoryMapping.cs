using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Configurations
{
    public class CategoryMapping : EntityTypeConfiguration<Category>
    {
        public CategoryMapping()
        {
            ToTable("Category");

            HasRequired(x => x.Shop).WithMany(x => x.Categories).HasForeignKey(x => x.ShopId).WillCascadeOnDelete(false);
        }
    }
}
