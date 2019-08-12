using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Models.Report
{
    public class PartnerDailyOrderReport
    {
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public List<ShopDailyReportModel> Shops { get; set; }
    }
    public class ShopDailyReportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public List<OrdersInDayModel> Orders { get; set; }
    }

    public class OrdersInDayModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string StaffName { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreateDateTime => CreatedDate.HasValue ? CreatedDate.Value.ToString("dd-MM-yyyy HH:mm ") : "";
    }
}
