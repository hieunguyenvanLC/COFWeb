using System;
using COF.AzureFunctions.Ioc;
using COF.BusinessLogic.Services.Reports;
using CommonServiceLocator;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace COF.AzureFunctions.Functions
{
    public static class DailyOrderExportFunction
    {
        public static IServiceLocator ServiceLocator { get; set; } = new ServiceLocatorBuilder()
            .RegisterModule<FuncModule>()
            .Build();

        [FunctionName("DailyOrderExportFunction")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            try
            {
                log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
                var reportService = ServiceLocator.GetInstance<IReportService>();
                reportService.ExportDailyOrderReport();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            
        }
    }
}
