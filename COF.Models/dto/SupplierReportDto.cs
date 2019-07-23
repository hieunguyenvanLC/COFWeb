using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class SupplierReportDto
    {
        public List<OrderDto> TodayJobs { get; set; }
        public List<OrderDto> _4DaysAfteJobs { get; set; }
        public FirebaseCountDto BookingCount { get; set; }
        public FirebaseCountDto ChatingCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrder { get; set; }
        public string TotalRevenueDisplay
        {
            get
            {
                return String.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:c0}", TotalRevenue);
            }
        }
        public List<OrderReportInMonth> ReportInMonth { get; set; }
        public List<OrderReportInQuarter> ReportInQuarter { get; set; }
        public ReportDetail ReportDetailForMonth
        {
            get
            {
                return new ReportDetail
                {

                    Finish = ReportInMonth.Sum(x => x.Finish),
                    Inprogress = ReportInMonth.Sum(x => x.Inprogress),
                    Cancel = ReportInMonth.Sum(x => x.Cancel),
                    Total = ReportInMonth.Sum(x => x.Total),
                };
            }
        }
        public ReportDetail ReporDetailtForYear
        {
            get
            {
                return new ReportDetail
                {

                    Finish = ReportInQuarter.Sum(x => x.Finish),
                    Inprogress = ReportInQuarter.Sum(x => x.Inprogress),
                    Cancel = ReportInQuarter.Sum(x => x.Cancel),
                    Total = ReportInQuarter.Sum(x => x.Total),
                };
            }
        }
        public int NewCustomer { get; set; }
    }
    public class FirebaseCountDto
    {
        public bool Success { get; set; }
        public int Count { get; set; }
    }

    public class OrderReportInQuarter
    {
        public int Total { get; set; }
        public int Month { get; set; }
        public int Cancel { get; set; }
        public int Inprogress { get; set; }
        public int Finish { get; set; }
    }
    public class ReportDetail
    {
        public int Total { get; set; }
        public int Cancel { get; set; }
        public int Inprogress { get; set; }
        public int Finish { get; set; }
    }
}
