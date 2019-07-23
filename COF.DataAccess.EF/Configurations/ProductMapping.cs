﻿using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Configurations
{
    public class ProductMapping : EntityTypeConfiguration<Product>
    {
        public ProductMapping()
        {
            ToTable("Product");
            HasRequired(x => x.Shop).WithMany(x => x.Products).HasForeignKey(x => x.ShopId).WillCascadeOnDelete(false);
        }
    }
}
