using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class Table : BaseEntity
    {
        [MaxLength(45)]
        public string TableNumber { get; set; }
        [MaxLength(45)]
        public string Description {get;set;}
        [MaxLength(45)]
        public string Status { get; set; }
        public int ShopId { get; set; }
        public virtual Shop Shop { get; set; }

        public virtual ICollection<TableHasOrder> TableHasOrders { get; set; }
    }
}
