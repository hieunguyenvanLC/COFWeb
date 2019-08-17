using COF.BusinessLogic.Models.Report;
using COF.BusinessLogic.Services.AzureBlob;
using COF.BusinessLogic.Services.Export;
using COF.Common.Helper;
using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services.Reports
{
    public interface IReportService
    {
        void ExportDailyOrderReport();
        List<ShopRevenueMonthlyReport> GetPartnerRevenueMonthlyReport(int partnerId);
        List<ShopRevenueReportModel> GetShopRevenueReportImMonthModels(int partnerId,int? shopId);
        List<ShopRevenueReportModel> GetShopRevenueReportInYearModels(int partnerId, int? shopId);

    }
    public class ReportService : IReportService
    {
        #region fields
        private readonly IPartnerService _partnerService;
        private readonly IExcelExportService _excelExportService;
        private readonly IOrderService _orderService;
        private readonly IAzureBlobSavingService _azureBlobSavingService;
        #endregion

        #region ctor
        public ReportService(
            IPartnerService partnerService,
            IExcelExportService excelExportService,
            IOrderService orderService,
            IAzureBlobSavingService azureBlobSavingService
            )
        {
            _partnerService = partnerService;
            _excelExportService = excelExportService;
            _orderService = orderService;
            _azureBlobSavingService = azureBlobSavingService;
        }
        #endregion
        public void ExportDailyOrderReport()
        {
            var partners = _partnerService.GetAll();
            foreach (var partner in partners.Result)
            {
                var queryRes = _orderService.GetDailyOrders(partner.Id);
                var dailyOrders = queryRes.Result;
                var bytes = _excelExportService.ExportExcelOutsource(dailyOrders);
                var fileName = $"{partner.Name}_Danh_sach_hoa_don_{DateTime.UtcNow.AddHours(7).ToString("dd-MM-yyyy")}.xlsx";
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                _azureBlobSavingService.SavingFileToAzureBlob(bytes, fileName, contentType, AzureHelper.DailyOrderExportContainer);
            }
        }

        public List<ShopRevenueMonthlyReport> GetPartnerRevenueMonthlyReport(int partnerId)
        {
            var partner = _partnerService.GetById(partnerId);
            var shops = partner.Result.Shops.ToList();
            var allOrders = _orderService.GetOrdersInMonth(partnerId);
            var result = shops.Select(shop => new ShopRevenueMonthlyReport
            {
                Shop = shop.ShopName,
                TotalMoney = allOrders.Result
                                .Where(x => x.ShopId == shop.Id)
                                .Select(x => x.FinalAmount).DefaultIfEmpty(0).Sum(),
                TotalOrder = allOrders.Result
                                .Count(x => x.ShopId == shop.Id)
            }).ToList();
            return result;

        }

        public List<ShopRevenueReportModel> GetShopRevenueReportImMonthModels(int partnerId, int? shopId)
        {
            var partner = _partnerService.GetById(partnerId);
            List<Order> allOrders = null;
            if (shopId != null)
            {
                var queryRes = _orderService.GetOrdersInMonthByShopId(shopId.Value);
                allOrders = queryRes.Result;
            }
            else
            {
                var queryRes = _orderService.GetOrdersInMonth(partnerId);
                allOrders = queryRes.Result;
            }
            var currentDate = DateTime.UtcNow.AddHours(7); 
            var dateInMonth = Enumerable.Range(1, DateTime.DaysInMonth(currentDate.Year,currentDate.Month))  // Days: 1, 2 ... 31 etc.
                    .Select(day => new DateTime(currentDate.Year, currentDate.Month, day)) // Map each day to a date
                    .Where(day => day.Date <= currentDate.Date)
                    .ToList(); // Load dates into a list
            var result = dateInMonth.Select(x => new ShopRevenueReportModel
            {
                Header = x.Date.ToString("dd/MM"),
                TotalMoney = allOrders.Where(y => y.CreatedOnUtc.Date == x.Date)
                                .Select(y => y.FinalAmount).DefaultIfEmpty(0).Sum(),
                TotalOrder = allOrders.Where(y => y.CreatedOnUtc.Date == x.Date).Count()
            }).ToList();

            return result;
        }

        public List<ShopRevenueReportModel> GetShopRevenueReportInYearModels(int partnerId, int? shopId)
        {
            var partner = _partnerService.GetById(partnerId);
            List<Order> allOrders = null;
            if (shopId != null)
            {
                var queryRes = _orderService.GetOrdersInYearByShopId(shopId.Value);
                allOrders = queryRes.Result;
            }
            else
            {
                var queryRes = _orderService.GetOrdersInYear(partnerId);
                allOrders = queryRes.Result;
            }
            var currentDate = DateTime.UtcNow.AddHours(7);
            var result = new  List<ShopRevenueReportModel>();
            for (int i = 1; i <= currentDate.Month; i++)
            {
                var tmp = new ShopRevenueReportModel
                {
                    Header = $"Tháng {i}",
                    TotalMoney = allOrders.Where(x => x.CreatedOnUtc.Month == i)
                                 .Select(y => y.FinalAmount).DefaultIfEmpty(0).Sum(),
                    TotalOrder = allOrders.Where(x => x.CreatedOnUtc.Month == i).Count()
                };
                result.Add(tmp);
            }
            return result;
        }
    }
}
