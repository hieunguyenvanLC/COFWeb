using COF.BusinessLogic.Services.Export;
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
    }
    public class ReportService : IReportService
    {
        #region fields
        private readonly IPartnerService _partnerService;
        private readonly IExcelExportService _excelExportService;
        private readonly IOrderService _orderService;
        #endregion

        #region ctor
        public ReportService(
            IPartnerService partnerService,
            IExcelExportService excelExportService,
            IOrderService orderService
            )
        {
            _partnerService = partnerService;
            _excelExportService = excelExportService;
            _orderService = orderService;
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
            }
        }
    }
}
