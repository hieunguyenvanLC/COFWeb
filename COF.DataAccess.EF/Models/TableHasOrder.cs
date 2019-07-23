using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class TableHasOrder : BaseEntity
    {
        public int TableId { get; set; }
        public int OrderId { get; set; }

        public virtual Table Table { get; set; }
        public virtual Order Order { get; set; }
    }
}
