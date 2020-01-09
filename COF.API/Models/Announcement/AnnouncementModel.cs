using COF.API.Models.Order;
using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COF.API.Models.Announcement
{
    public class AnnouncementModel
    {
        public int OrderId { get; set; }
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
        public int GroupPaymentStatus { get; set; }
        public int StoreId { get; set; }
        public List<OrderDetailModel> OrderDetailViewModels { get; set; }
        public DiscountType DiscountType { get; set; }
        public double DiscountAmount { get; set; }
    }
}