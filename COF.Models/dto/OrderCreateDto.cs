using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class OrderCreateDto
    {
        public int? OrderId { get; set; }
        public int CustomerId { get; set; }
        public String PhoneNumber { get; set; }
        public String Address { get; set; }
        public String Total { get; set; }
        public int SupplierId { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
        public String ImplementDate { get; set; }
        public string Description { get; set; }
    }
}
