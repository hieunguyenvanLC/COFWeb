using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class OrderSearchDto
    {
        public int? CustomerId { get; set; }
        public String CustomerName { get; set; }
        public String PhoneNumber { get; set; }
        public String Address { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int SupplierId { get; set; }
        public int? OrderStatusId { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int? OrderId { get; set; }
        public bool? IsPaid { get; set; }
    }
}
