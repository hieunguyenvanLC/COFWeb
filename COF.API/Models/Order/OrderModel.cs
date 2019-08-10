using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COF.API.Models.Order
{
    public class OrderModel
    {
    }

    public class OrderCreateModel
    {
        [Required]
        public int ShopId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public List<OrderDetailModel> OrderDetails { get; set; }
    }
    public class OrderDetailModel
    {
        public int ProductSizeId { get; set; }
        public int Quantity { get; set; }
    }
}