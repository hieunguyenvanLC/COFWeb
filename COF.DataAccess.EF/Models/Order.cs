using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class Order : BaseEntity
    {
        [MaxLength(45)]
        public string OrderNumber { get; set; }

        [MaxLength(45)]
        public string ItemName { get; set; }

        [MaxLength(45)]
        public string Date { get; set; }
        [MaxLength(45)]
        public string TimeCreated { get; set; }

        [MaxLength(45)]
        public string TimeCompleted { get; set; }

        [MaxLength(45)]
        public string Description { get; set; }

        public int UserId { get; set; }

        public int CustomerId { get; set; }

        public string OrderStatus { get; set; }
        public int ShopId { get; set; }
        public string PaymentType { get; set; }
        public string BonusPoint { get; set; }

        public virtual Shop Shop { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<TableHasOrder> TableHasOrders { get; set; }
    }
}
