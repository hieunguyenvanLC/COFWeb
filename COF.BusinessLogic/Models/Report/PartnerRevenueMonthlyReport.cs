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
        public double TotalMoney { get; set; }
    }

    public class ShopRevenueReportModel
    {
        public string Header { get; set; }
        public int TotalOrder { get; set; }
        public double TotalMoney { get; set; }
    }
}
