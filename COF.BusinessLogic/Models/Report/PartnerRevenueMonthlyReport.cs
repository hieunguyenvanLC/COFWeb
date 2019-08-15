using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Models.Report
{
    public class ShopRevenueMonthlyReport
    {
        public string Shop { get; set; }
        public int TotalOrder { get; set; }
        public decimal TotalMoney { get; set; }
    }
}
