using COF.BusinessLogic.Models.Order;
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
        public DateTime PaymentDate { get; set; }
        [Required]
        public string OrderCode { get; set; }
        [Required]
        public List<OrderDetailModel> OrderDetails { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public OrderType OrderType { get; set; }

    }
    public class OrderDetailModel
    {
        public int ProductSizeId { get; set; }
        public int Quantity { get; set; }
    }
}