using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class OrderDetail : BaseEntity, IPartner
    {
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int ProductSizeId { get; set; }
        public int OrderId { get; set; }

        public virtual ProductSize ProductSize { get; set; }
        public virtual Order Order { get; set; }
        public int PartnerId { get; set; }
    }
}
