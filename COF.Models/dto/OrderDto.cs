using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedDateDisplay { get; set; }
        public int CustomerId { get; set; }
        public string AccountName { get; set; }
        public string CustomerName { get; set; }
        public string OrderStatus { get; set; }
        public int OrderStatusId { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public SupplierDto SupplierInfo { get; set; }
        public string Total { get; set; }
        public decimal? PaymentPrice { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
        public bool IsPaid { get; set; }
        public string WorkDate { get; set; }
        public string WorkTime { get; set; }
        public string Description { get; set; }
        public string SupplierNote { get; set; }
    }
}
