using COF.DataAccess.EF.Models;
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
        public string OrderCode { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string StaffName { get; set; }
        public double TotalCost { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreateDateTime => CreatedDate.HasValue ? CreatedDate.Value.ToString("dd-MM-yyyy HH:mm ") : "";
        public List<OrderDetailModelVm> OrderDetails { get; set; }
        public DateTime? CancelDate { get; set; }
        public string CancelDateTime => CancelDate.HasValue ? CancelDate.Value.ToString("dd-MM-yyyy HH:mm ") : "";
        public string CancelBy { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string OrderStatusDisplay
        {
            get
            {
                var description = "";
                switch (OrderStatus)
                {
                    case OrderStatus.PosFinished:
                        description = "Hoàn tất";
                        break;
                    case OrderStatus.PreCancel:
                        description = "Hủy";
                        break;
                    case OrderStatus.Cancel:
                        description = "Hủy";
                        break;
                    case OrderStatus.PosCancel:
                        description = "Hủy";
                        break;
                    case OrderStatus.PosPreCancel:
                        description = "Hủy";
                        break;
                }
                return description;
            }
        }
        public DiscountType DiscountType { get; set; }
        public string DiscountTypeDisplay
        {
            get
            {
                var description = "";
                switch (DiscountType)
                {
                    case DiscountType.OneGetOne:
                        description = "1 tặng 1";
                        break;
                    case DiscountType._20kDiscount:
                        description = "20.000";
                        break;
                }
                return description;
            }
        }
    }
    public class OrderCreateModel
    {
        public string OrderCode { get; set; }
        public System.DateTime CheckInDate { get; set; }
        public Nullable<System.DateTime> CheckOutDate { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public double TotalAmount { get; set; }
        public double FinalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string Notes { get; set; }
        public string FeeDescription { get; set; }
        public string CheckInPerson { get; set; }
        public string CheckOutPerson { get; set; }
        public string ApprovePerson { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> SourceID { get; set; }
        public Nullable<int> TableId { get; set; }
        public bool IsFixedPrice { get; set; }
        public int SourceType { get; set; }
        public Nullable<System.DateTime> LastRecordDate { get; set; }
        public string ServedPerson { get; set; }
        public string DeliveryAddress { get; set; }
        public int DeliveryStatus { get; set; }
        public string DeliveryPhone { get; set; }
        public string DeliveryCustomer { get; set; }
        public int TotalInvoicePrint { get; set; }
        public double VAT { get; set; }
        public double VATAmount { get; set; }
        public int NumberOfGuest { get; set; }
        public string Att1 { get; set; }
        public string Att2 { get; set; }
        public string Att3 { get; set; }
        public string Att4 { get; set; }
        public string Att5 { get; set; }
        public int GroupPaymentStatus { get; set; }
        public int StoreId { get; set; }
        public Nullable<System.DateTime> LastModifiedPayment { get; set; }
        public Nullable<System.DateTime> LastModifiedOrderDetail { get; set; }
        public List<OrderDetailModel> OrderDetailViewModels { get; set; }
        public DiscountType DiscountType { get; set; }
        public double DiscountAmount { get; set; }
        public string CreatedBy { get; set; }
        public string CanceledBy { get; set; }
        public DateTime? CanceledDate { get; set; }
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
        public string Description { get; set; }
    }

    public enum OrderType
    {
         AtShop = 1,
         Online = 2,
         MobileApplication = 3
    }

    
}
