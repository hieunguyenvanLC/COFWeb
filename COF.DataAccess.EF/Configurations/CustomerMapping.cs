using COF.DataAccess.EF.Models;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace COF.DataAccess.EF.Configurations
{
    public class CustomerMapping : EntityTypeConfiguration<Customer>
    {
        public CustomerMapping()
        {
            ToTable("Customer");
            HasOptional(x => x.BonusLevel).WithMany(x => x.Customers).HasForeignKey(x => x.BonusLevelId).WillCascadeOnDelete(false);
        }
    }
}
