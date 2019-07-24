using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Configurations
{
    public class BonusPointHistoryMapping : EntityTypeConfiguration<BonusPointHistory>
    {
        public BonusPointHistoryMapping()
        {
            ToTable("BonusPointHistory");
            HasRequired(x => x.Customer).WithMany(x => x.BonusPointHistories).HasForeignKey(x => x.CustomerId).WillCascadeOnDelete(false);
            HasRequired(x => x.Order).WithMany(x => x.BonusPointHistories).HasForeignKey(x => x.OrderId).WillCascadeOnDelete(false);
        }
    }
}
