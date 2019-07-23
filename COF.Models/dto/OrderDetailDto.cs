using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class OrderDetailDto
    {
        public int Index { get; set; }
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public string Service { get; set; }
        public string Price { get; set; }
        public string OrignalPrice { get; set; }
        public int ServiceId { get; set; }
        public int? Quantity { get; set; }
    }
}
