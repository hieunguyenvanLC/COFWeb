using COF.API.Core;
using COF.BusinessLogic.Services.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace COF.API.Api.Core
{
    [Route("api/export")]
    [Authorize]
    public class ExportController : ApiControllerBase
    {
        private readonly IReportService _reportService;
        public ExportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [Route("daily-order")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage ExportDailyExport()
        {
            try
            {
                _reportService.ExportDailyOrderReport();
                return SuccessResult();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
