using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class UpdateOrderDto
    {
        public int OrderId { get; set; }
        public int? OrderStatusId { get; set; }
        public decimal? TotalMoney { get; set; }
        public string Note { get; set; }
        public bool IsPaid { get; set; }
    }
}
