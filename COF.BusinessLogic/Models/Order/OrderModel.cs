using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Models.Order
{
    class OrderModel
    {
    }
    public class OrderCreateModel
    {
        public int CustomerId { get; set; }
        public int ShopId { get; set; }
        public int UserId { get; set; }
        public List<OrderDetailModel> OrderDetails { get; set; }
    }

    public class OrderDetailModel
    {
        public int ProductSizeId { get; set; }
        public int Quantity { get; set; }
    }
}
