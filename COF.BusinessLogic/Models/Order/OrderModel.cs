using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COF.BusinessLogic.Models.Order
{
    public class OrderModel
    {
        public long? RowCounts { get; set; }
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string StaffName { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreateDateTime => CreatedDate.HasValue ? CreatedDate.Value.ToString("dd-MM-yyyy HH:mm ") : "";
        public List<OrderDetailModelVm> OrderDetails { get; set; }
    }
    public class OrderCreateModel
    {
        public int CustomerId { get; set; }
        public int ShopId { get; set; }
        public int UserId { get; set; }
        public List<OrderDetailModel> OrderDetails { get; set; }
        public string OrderCode { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public OrderType OrderType { get; set; }
    }

    public class OrderDetailModel
    {
        public int ProductSizeId { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderDetailModelVm
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Size { get; set; }
        public decimal Cost { get; set; }
        public string  ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Cost * Quantity;
    }

    public enum OrderType
    {
         AtShop = 1,
         Online = 2
    }
}
