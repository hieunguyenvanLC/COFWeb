using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class Order : BaseEntity, IPartner
    {
        [MaxLength(256)]
        public string Date { get; set; }
        [MaxLength(256)]
        public string TimeCreated { get; set; }

        [MaxLength(256)]
        public string TimeCompleted { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        public string UserId { get; set; }
        public virtual AppUser User { get; set; }

        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int ShopId { get; set; }
        public string PaymentType { get; set; }
        public string BonusPoint { get; set; }

        public virtual Shop Shop { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<TableHasOrder> TableHasOrders { get; set; }
        public virtual ICollection<BonusPointHistory> BonusPointHistories { get; set; }
        public int PartnerId { get; set; }
        public decimal TotalCost { get; set; }
        public Nullable<int> SourceId { get; set; }
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
        public DateTime? CancelDate { get; set; }
        public string CancelBy { get; set; }
    }
    public enum OrderStatus
    {
        //PosNew = 16, // Hóa đươn tạo mới từ POS
        [Description("Vừa tạo")]
        New = 8, //Đơn hàng mới tạo, từ tổng đài,web...//DeliveryStatus: New,Assgigned

        PosProcessing = 17,
        Processing = 10,//DeliveryStatus: POSAccepted, PosUnAccepted, Delivery

        PosFinished = 11,//Ket thuc o POS: POSFinished ->Finish//For POS online
        Finish = 2,//Don hang da gui len Server thanh cong: 

        PosCancel = 13,
        Cancel = 3,// Don hang da bi huy SAU KHI CHẾ BIẾN//DeliveryStatus: Cancel,Fail,Precancel

        PosPreCancel = 12,//Don hang bi huy truoc khi che bien - CHUA SUBMIT LEN SERVER
        PreCancel = 4,//Don hang bi huy truoc khi che bien - DA SUBMIT LEN SERVER
    }
}
