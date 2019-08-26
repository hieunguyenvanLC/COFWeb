using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class BonusLevel : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartPointToReach { get; set; }
        public decimal EndPointToReach { get; set; }
        public decimal MoneyToOnePoint { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
