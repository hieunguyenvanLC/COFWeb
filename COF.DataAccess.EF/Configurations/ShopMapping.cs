﻿using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Configurations
{
    public class ShopMapping : EntityTypeConfiguration<Shop>
    {
        public ShopMapping()
        {
            ToTable("Shop");

            HasRequired(x => x.Partner).WithMany(x => x.Shops).HasForeignKey(x => x.PartnerId).WillCascadeOnDelete(false);
        }
    }
}
