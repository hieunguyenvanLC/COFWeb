using COF.BusinessLogic.Models.Report;
using COF.BusinessLogic.Services.AzureBlob;
using COF.BusinessLogic.Services.Export;
using COF.Common.Helper;
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
                Total = allOrders.Result
                                .Where(x => x.ShopId == shop.Id)
                                .Select(x => x.TotalCost).DefaultIfEmpty(0).Sum()
                                
            }).ToList();
            return result;

        }
    }
}
