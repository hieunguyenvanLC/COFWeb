using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class BonusPointHistory : BaseEntity
    {
        public TransactionType TypeAccess { get; set; }
        public decimal OldPoint { get; set; }
        public decimal Point { get; set; }
        public string OldLevel { get; set; }
        public string Level { get; set; }
        public int CustomerId { get; set; }
        public string Description { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
