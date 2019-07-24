using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class Customer : BaseEntity
    {
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int BonusLevelId { get; set; }
        public virtual BonusLevel BonusLevel { get; set; }
        public decimal TotalBonusPoint { get; set; }
        public decimal ActiveBonusPoint { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<BonusPointHistory> BonusPointHistories { get; set; }

    }
}
